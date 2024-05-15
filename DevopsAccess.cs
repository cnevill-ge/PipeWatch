
namespace PipeWatch
{
	using RestSharp.Authenticators;
	using RestSharp;
	using System.Diagnostics;
	using System.Text.Json.Nodes;
	using System.Text.RegularExpressions;
	using PipeWatch.Properties;
	using System.Security.Policy;
	using System.Windows.Forms.VisualStyles;
	using System.ComponentModel.Design;

	public class DevopsAccess
	{
		private readonly Settings m_settings;
		private RestClient m_restClient;
		private readonly string m_hideRunFile;
		private List<RunId> m_hideRuns = new List<RunId>();
		private readonly Dictionary<string, string> m_commitMessageCache = new Dictionary<string, string>();
		private readonly Dictionary<string, string> m_prTitleCache = new Dictionary<string, string>();
		private readonly Dictionary<int, StaticRunInfo> m_staticRunInfoCache = new Dictionary<int, StaticRunInfo>();

		private record struct ExtraBuildInfo( string Project, int BuildId );
		private List<ExtraBuildInfo> m_addRuns = new List<ExtraBuildInfo>();

		private string[] WatchProjects => m_settings.WatchProjects.Split( ";" );

		internal DevopsAccess( Settings settings )
		{
			m_settings = settings;
			m_hideRunFile = Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.ApplicationData ),
				"Microsmurf", "PipeWatch", "hideRuns.txt" );
			if( File.Exists( m_hideRunFile ) )
				LoadHiddenRunList();
			RefreshUserSettings();
		}

		public const int MaxLookbackCount = 25;
		public static readonly TimeSpan MaxLookbackAge = TimeSpan.FromDays( 7 );

		/// <summary>
		/// Records the last URL that this object attempted to query, only containing a value if the
		/// query fails for some reason.
		/// </summary>
		public string? LastQueryUrl { get; private set; }

		/// <summary>
		/// Checks that the Settings object provided at construction seems to be valid.
		/// </summary>
		public bool ValidateUserSettings( out string? invalidReason )
		{
			JsonNode? node = m_restClient.Get<JsonNode>( new RestRequest( "_apis/projects" ) )!;
			if( node == null )
			{
				invalidReason = "Unable to get the list of projects from DevOps; this may indicate that the DevOps PAT is invalid or has expired.";
				return false;
			}

			JsonArray? values = node["value"] as JsonArray;
			if( values == null )
			{
				invalidReason = "The _apis/projects endpoint returned unexpected data.";
				return false;
			}

			// Do all the referenced projects exist?
			string[] names = values.Select( x => x!.GetString( "name" ) ).ToArray();
			string[] missingNames = this.WatchProjects.Where( name => !names.Contains( name, StringComparer.OrdinalIgnoreCase ) ).ToArray();
			if( missingNames.Length > 0 )
			{
				invalidReason = "Can't find the following DevOps project(s): " + string.Join( ",", missingNames ); ;
				return false;
			}

			invalidReason = null;
			return true;
		}

		/// <summary>
		/// Call this when the user's settings have been loaded or refreshed.
		/// </summary>
		public void RefreshUserSettings()
		{
			var options = new RestClientOptions( "https://dev.azure.com/geaviationdigital-dss" )
			{
				Authenticator = new HttpBasicAuthenticator( "", m_settings.GitApiKey )
			};

			m_restClient = new RestClient( options );
		}

		/// <summary>
		/// Identifies a pipeline run. This includes a start time because we treat retries as new
		/// runs for display purposes. Meaning we will show a new run if a previously-hidden run
		/// gets manually retried.
		/// </summary>
		public record struct RunId( int BuildId, DateTimeOffset StartTime );

		public record RunInfo
		{
			public RunId Id { get; init; }
			public string Url { get; init; }
			public string Pipeline { get; init; }
			public string Name { get; init; }
			public string Branch { get; init; }
			public string Reason { get; init; }
			public DateTimeOffset StartTime { get; init; }
			public BuildStatus Status { get; init; }
			public BuildResult Result { get; init; }
		}

		public enum BuildStatus { None, All, Cancelling, Completed, InProgress, NotStarted, Postponed }
		public enum BuildResult { None, Succeeded, Failed, Canceled, PartiallySucceeded }

		/// <summary>
		/// Gets the set of runs that should be displayed in the UI.
		/// </summary>
		public IEnumerable<RunInfo> GetRunsOfInterest()
		{
			// Show recent runs from known projects
			string[] projects = Settings.Default.WatchProjects.Split( ";" );
			foreach( string project in projects )
			{
				foreach( JsonNode build in GetRecentBuildsByProject( project ) )
				{
					var runInfo = GetRunInfoForBuild( project, build );
					if( runInfo == null )
						continue;
					yield return runInfo;
				}
			}

			// Show all the runs the user explicitly added
			var adds = m_addRuns.ToArray();
			foreach( var extra in adds )
			{
				var build = QueryDevops( extra.Project, $"build/builds/{extra.BuildId}" );
				if( build == null )
					continue;
				var runInfo = GetRunInfoForBuild( extra.Project, build );
				if( runInfo == null )
					continue;

				// Remove this after it gets stale, the user can re-add if they want
				if( (DateTimeOffset.Now - runInfo.StartTime).TotalDays > 3 )
				{
					m_addRuns.Remove( extra );
					continue;
				}

				if( !m_hideRuns.Contains( runInfo.Id ) )
					yield return runInfo;
			}
		}

		/// <summary>
		/// Causes the given build to be included by GetRunsOfInterest even if it doesn't meet usual filtering criteria.
		/// </summary>
		public void ForceIncludeRun( string project, int buildId )
		{
			m_addRuns.Add( new ExtraBuildInfo { Project = project, BuildId = buildId } );
		}

		/// <summary>
		/// Hides a particular run from the results of GetRunsOfInterest.
		/// </summary>
		public void HideRun( RunInfo run )
		{
			m_hideRuns.Add( run.Id );
			SaveHiddenRunList();
		}

		private RunInfo? GetRunInfoForBuild( string project, JsonNode build )
		{
			int buildId = build.GetProp<int>( "id" );
			Debug.WriteLine( $"Build: {buildId}" );

			DateTimeOffset startTime = build["startTime"] != null
					? build.GetProp<DateTimeOffset>( "startTime" )
					: build.GetProp<DateTimeOffset>( "queueTime" );

			// In some cases we will see very old builds even with a reasonable	'top' filter in GetRecentBuildsByProject.
			// These builds may reference repositories which don't exist or in other ways confuse this code.
			if( (DateTimeOffset.UtcNow - startTime) > MaxLookbackAge )
				return null;

			RunId id = new RunId( buildId, startTime );

			// Should be skipping this build?
			if( m_hideRuns.Contains( id ) )
				return null;

			StaticRunInfo? runInfo = GetStaticRunInfo( project, build );
			if( runInfo == null )
				return null;

			string runName = runInfo.Name;

			// Should we augment the commit message with something more useful?
			var appendCommitMessage = build.GetProp<bool?>( "appendCommitMessageToRunName" ) ?? false;
			if( appendCommitMessage )
			{
				if( runInfo.PullRequestTitle != null )
				{
					runName += " - " + runInfo.PullRequestTitle;
				}
				else if( (build["triggerInfo"] is JsonNode triggerInfo) &&
					(triggerInfo["ci.sourceBranch"] is JsonNode branch_) &&
					(!branch_.GetValue<string>().EndsWith( "/main" )) )
				{
					string branch = branch_.GetValue<string>();
					runName += " - CI build for " + branch;
				}
				else
				{
					var repoId = build.GetString( "repository", "id" );
					string sourceVersion = build.GetString( "sourceVersion" );
					runName += " - " + GetCommitComment( project, repoId, sourceVersion );
				}
			}

			var info = new RunInfo
			{
				Id = id,
				Url = build.GetString( "_links", "web", "href" ),
				Pipeline = build.GetString( "definition", "name" ),
				Name = runName,
				Branch = Regex.Replace( build.GetString( "sourceBranch" ), "refs/heads", "" ),
				Reason = build.GetString( "reason" ),
				Status = Enum.Parse<BuildStatus>( build.GetProp<string>( "status" ), ignoreCase: true ),
				StartTime = startTime,
				Result = (build["result"] != null)
					? Enum.Parse<BuildResult>( build.GetProp<string>( "result" ), ignoreCase: true )
					: BuildResult.None
			};

			return info;
		}

		private IEnumerable<JsonNode> GetRecentBuildsByProject( string project )
		{
			// Although we refer mostly to "runs" in this class, DevOps gives back slightly different
			// information for Builds vs. Runs. The interface for listing builds is more useful so we
			// start there. Note that build ids and run ids are the same (a build and a run with the same
			// id represent the same activity).
			var response = QueryDevops( project, "build/builds", request =>
			{
				request.AddQueryParameter( "$top", MaxLookbackCount );
				request.AddQueryParameter( "requestedFor", Properties.Settings.Default.GitUserName );
				request.AddQueryParameter( "queryOrder", "startTimeDescending" );
			} );
			if( response == null )
				yield break;

			JsonArray? builds = (JsonArray?)response["value"];
			if( builds == null )
				yield break;

			foreach( var build in builds )
				yield return build!;
		}

		private record StaticRunInfo( string Name, string? PullRequestTitle );

		private StaticRunInfo? GetStaticRunInfo( string project, JsonNode build )
		{
			int buildId = build.GetProp<int>( "id" );
	
			StaticRunInfo? info;
			if( !m_staticRunInfoCache.TryGetValue( buildId, out info ) )
			{
				// Get the run information, which has a lot of overlap but also some information that isn't part of
				// the build information
				int pipeline = build.GetProp<int>( "definition", "id" );
				var run = QueryDevops( project, $"pipelines/{pipeline}/runs/{buildId}?api-version=7.2-preview.1" );
				if( run == null )
					return null;

				string runName = run.GetString( "name" );

				// Is this a PR build?
				string? prTitle = null;
				if( (run["variables"] is JsonNode variables) && (variables["system.pullRequest.pullRequestId"] is JsonNode prId_) )
				{
					var repoId = build.GetString( "repository", "id" );
					int prId = int.Parse( prId_.GetString( "value" ) );
					prTitle = GetPullRequestTitle( project, repoId, prId );
				}

				info = new StaticRunInfo( runName, prTitle );
				m_staticRunInfoCache.Add( buildId, info );
			}

			return info;
		}

		private string GetPullRequestTitle( string project, string repoId, int pullRequestId )
		{
			string key = string.Join( ":", project, repoId, pullRequestId );
			if( !m_prTitleCache.TryGetValue( key, out string title ) )
			{
				var pr = QueryDevops( project, $"git/repositories/{repoId}/pullRequests/{pullRequestId}" );
				title = pr?.GetString( "title" )!;
				m_prTitleCache.Add( key, title );
			}
			return title;
		}

		private string GetCommitComment( string project, string repoId, string commitSha )
		{
			string key = string.Join( ":", project, repoId, commitSha );
			if( !m_commitMessageCache.TryGetValue( key, out string firstLine ) )
			{
				var commit = QueryDevops( project, $"git/repositories/{repoId}/commits/{commitSha}" );
				firstLine = Regex.Replace( commit.GetString( "comment" ), @"\n.*", string.Empty );
				m_commitMessageCache.Add( key, firstLine );
			}
			return firstLine;
		}

		private JsonNode QueryDevops( string project, string url, Action<RestRequest>? augmentRequest = null )
		{
			url = project + "/_apis/" + url;
			Debug.WriteLine( "API call: " + url );
			var request = new RestRequest( url );
			if( augmentRequest != null )
				augmentRequest( request );

			this.LastQueryUrl = m_restClient.Options.BaseUrl + "/" + url;
			return m_restClient.Get<JsonNode>( request )!;
		}

		private void LoadHiddenRunList()
		{
			try
			{
				m_hideRuns = File.ReadAllLines( m_hideRunFile )
					.Select( line =>
					{
						var parts  = line.Split( "|" );
						return new RunId
						{
							BuildId = int.Parse( parts[0] ),
							StartTime = DateTimeOffset.Parse( parts[1] )
						};
					} )
					.OrderBy( x => x.BuildId )
					.ThenBy( x => x.StartTime )
					.ToList();
			}
			catch
			{
			}
		}

		private void SaveHiddenRunList()
		{
			string directory = Path.GetDirectoryName( m_hideRunFile )!;
			if( !Directory.Exists( directory ) )
				Directory.CreateDirectory( directory );
			File.WriteAllLines( m_hideRunFile,
				m_hideRuns.Select( r => r.BuildId.ToString() + "|" + r.StartTime.ToString( "O" ) )
			);
		}
	}

	static class JsonHelpers
	{
		public static string GetString( this JsonNode node, params string[] properties )
		{
			return GetProp<string>( node, properties );
		}

		public static JsonNode GetChild( this JsonNode node, params string[] properties )
		{
			foreach( string property in properties )
			{
				var sub = node[property];
				if( sub == null )
					throw new Exception( $"Can't find property {property} under {node.ToJsonString()}" );
				node = sub;
			}
			return node;
		}

		public static T GetProp<T>( this JsonNode node, params string[] properties )
		{
			node = node.GetChild( properties );
			return node.GetValue<T>();
		}
	}
}
