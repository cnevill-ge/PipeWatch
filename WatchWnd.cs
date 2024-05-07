namespace PipeWatch
{
	using PipeWatch.Properties;
	using System;
	using System.Diagnostics;
	using System.Linq;
	using System.Runtime.InteropServices;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Windows.Forms;
	using static DevopsAccess;

	public partial class WatchWnd : Form
	{
		private readonly DevopsAccess m_devops;
		private readonly Size m_singleRunSize;
		private readonly Label m_labelNoRuns;
		private bool m_refreshing;

		public WatchWnd()
		{
			WatchWnd.Instance = this;
			InitializeComponent();
			m_devops = new DevopsAccess( Settings.Default );

			m_singleRunSize = RunStatusView.GetViewSize();

			// Setup the label we show when there's nothing going on.
			m_labelNoRuns = new Label();
			m_labelNoRuns.Text = "Nothing to see here, go build something.";
			m_labelNoRuns.TextAlign = ContentAlignment.MiddleCenter;
			m_labelNoRuns.AutoSize = false;
			m_labelNoRuns.Size = m_singleRunSize;
			m_labelNoRuns.Location = new Point( 0, m_toolStrip.Height );
			m_labelNoRuns.Visible = false;
			this.Controls.Add( m_labelNoRuns );
		}

		public static WatchWnd? Instance { get; private set; }

		private const int GutterSize = 10;

		protected override void OnLoad( EventArgs e )
		{
			base.OnLoad( e );
			BeginInvoke( () => DoInitialLoad() );
		}

		private void DoInitialLoad()
		{
			// Has the user configured any settings yet?
			if( string.IsNullOrEmpty( Settings.Default.GitUserName ) )
			{
				SettingsDlg dlg = new SettingsDlg();
				if( DialogResult.OK != dlg.ShowDialog( this ) )
				{
					Close();
					return;
				}
			}

			// Make sure the user's settings are actually valid.
			if( !SettingsDlg.ValidateSettingsAndWarn( this, Settings.Default ) )
			{
				SettingsDlg dlg = new SettingsDlg();
				if( DialogResult.OK != dlg.ShowDialog( this ) )
				{
					Close();
					return;
				}
			}

			RefreshList();

			m_refreshTimer.Interval = (int)TimeSpan.FromSeconds( 30 ).TotalMilliseconds;
			m_refreshTimer.Tick += timer_Tick;
			m_refreshTimer.Start();
		}

		internal void DismissRun( RunStatusView view )
		{
			m_devops.HideRun( view.Run );
			m_flowPanel.Controls.Remove( view );
			BeginInvoke( () => RefreshList() );
		}

		private void timer_Tick( object? sender, EventArgs e )
		{
			if( m_refreshing )
				return;

			RefreshList();
		}

		private void toolRefresh_Click( object sender, EventArgs e )
		{
			RefreshList();
		}

		private void toolSettings_Click( object sender, EventArgs e )
		{
			SettingsDlg dlg = new SettingsDlg();
			if( DialogResult.OK != dlg.ShowDialog( this ) )
				return;
			m_devops.RefreshUserSettings();
			RefreshList();
		}

		private const string DragFormatUrlW = "UniformResourceLocatorW";

		protected override void OnDragEnter( DragEventArgs drgevent )
		{
			IDataObject data = drgevent!.Data!;
			if( data.GetDataPresent( DragFormatUrlW ) )
				drgevent.Effect = DragDropEffects.Copy;
			base.OnDragEnter( drgevent );
		}

		protected override void OnDragDrop( DragEventArgs drgevent )
		{
			MemoryStream rawData = (MemoryStream)drgevent!.Data!.GetData( DragFormatUrlW )!;
			string? url = Encoding.Unicode.GetString( rawData.ToArray() );
			Debug.WriteLine( "URL: " + url );
			Match m = Regex.Match( url, @"/(\w+)/_build/.*buildId=(\d+)" );
			if( m.Success )
			{
				string project = m.Groups[1].Value;
				int buildId = int.Parse( m.Groups[2].Value );
				m_devops.ForceIncludeRun( project, buildId );
				RefreshList();
			}
			else
			{
				MessageBox.Show( this, "Can't find a DevOps build ID in " + url );
			}

			base.OnDragDrop( drgevent );
		}

		private void RefreshList()
		{
			RunInfo[]? runs = GetRunsOfInterest();
			if( runs == null )
				return;

			// Hide/show the "no runs" label
			if( runs.Length == 0 )
			{
				m_flowPanel.Visible = false;
				m_labelNoRuns.Visible = true;
				this.ClientSize = new Size( m_singleRunSize.Width, m_labelNoRuns.Height + m_toolStrip.Height );
				return;
			}
			else
			{
				m_flowPanel.Visible = true;
				m_labelNoRuns.Visible = false;
			}

			FlashWindowIfNeeded( runs );

			m_flowPanel.Controls.Clear();

			int halfGutter = GutterSize / 2;
			foreach( RunInfo run in runs )
			{
				var view = new RunStatusView( run );
				view.Margin = new Padding( halfGutter, halfGutter, halfGutter, 0 );

				if( run.Status == BuildStatus.NotStarted )
				{
					view.State = RunStatusView.PipelineState.NotStarted;
				}
				else if( run.Status == BuildStatus.InProgress )
				{
					view.State = RunStatusView.PipelineState.Running;
				}
				else if( run.Status == BuildStatus.Completed )
				{
					if( run.Result == BuildResult.Succeeded )
						view.State = RunStatusView.PipelineState.Succeeded;
					else if( run.Result == BuildResult.Canceled )
						view.State = RunStatusView.PipelineState.Canceled;
					else
						view.State = RunStatusView.PipelineState.Failed;
				}

				m_flowPanel.Controls.Add( view );
			}

			int viewsHeight = ((m_singleRunSize.Height + halfGutter) * m_flowPanel.Controls.Count) + halfGutter;
			this.ClientSize = new Size( m_singleRunSize.Width + GutterSize,
				m_toolStrip.Height + viewsHeight );
		}

		private void FlashWindowIfNeeded( RunInfo[] newRuns )
		{
			var oldRunsById = m_flowPanel.Controls
				.Cast<RunStatusView>()
				.Select( x => x.Run )
				.ToLookup( r => r.Id );
			foreach( RunInfo newRun in newRuns )
			{
				// Is this a totally new run?
				if( !oldRunsById.Contains( newRun.Id ) )
					continue;

				// Did the run finish?
				RunInfo oldRun = oldRunsById[newRun.Id].Single();
				if( oldRun.Status == BuildStatus.InProgress &&
					newRun.Status == BuildStatus.Completed )
				{
					FlashThisWindow();
				}
			}
		}

		private RunInfo[]? GetRunsOfInterest()
		{
			this.UseWaitCursor = true;
			m_refreshing = true;
			try
			{
				return m_devops.GetRunsOfInterest().OrderBy( x => x.StartTime ).ToArray();
			}
			catch( Exception ex )
			{
				MessageBox.Show( this, "Error refreshing from DevOps: " + ex.ToString(), "DevOps Access Error",
					MessageBoxButtons.OK, MessageBoxIcon.Stop );
				return null;
			}
			finally
			{
				this.UseWaitCursor = false;
				m_refreshing = false;
			}
		}

		private void FlashThisWindow()
		{
			FLASHWINFO info = new FLASHWINFO();
			info.cbSize = Convert.ToUInt32( Marshal.SizeOf( info ) );
			info.hwnd = this.Handle;
			info.dwFlags = FLASHW_ALL | FLASHW_TIMERNOFG;
			info.uCount = UInt32.MaxValue;
			info.dwTimeout = 0;
			FlashWindowEx( ref info );
		}

		[DllImport( "user32.dll" )]
		[return: MarshalAs( UnmanagedType.Bool )]
		static extern bool FlashWindowEx( ref FLASHWINFO pwfi );

		[StructLayout( LayoutKind.Sequential )]
		public struct FLASHWINFO
		{
			public UInt32 cbSize;
			public IntPtr hwnd;
			public UInt32 dwFlags;
			public UInt32 uCount;
			public UInt32 dwTimeout;
		}

		// Flags for FlashWindowEx
		public const UInt32 FLASHW_CAPTION = 1;
		public const UInt32 FLASHW_TRAY = 2;
		public const UInt32 FLASHW_ALL = 3;
		public const UInt32 FLASHW_TIMERNOFG = 12;
	}
}
