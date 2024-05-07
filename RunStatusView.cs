
namespace PipeWatch
{
	using System.Diagnostics;
	using static DevopsAccess;

	public partial class RunStatusView : UserControl
	{
		private RunInfo m_run;

		public RunStatusView( RunInfo run )
		{
			InitializeComponent();
			m_labelPipelineName.DoubleClick += RunStatusView_DoubleClick;
			m_labelBuildDescription.DoubleClick += RunStatusView_DoubleClick;
			m_imageStatus.DoubleClick += RunStatusView_DoubleClick;

			m_run = run;
			m_labelPipelineName.Text = run.Pipeline;
			m_labelBuildDescription.Text = run.Name;
			m_labelStartTime.Text = FormatTimeShort( run.StartTime );
		}

		public static Size GetViewSize()
		{
			RunInfo dummy = new RunInfo { Pipeline = string.Empty, Name = string.Empty };
			return new RunStatusView( dummy ).Size;
		}

		public RunInfo Run => m_run;

		public enum PipelineState
		{
			NotStarted,
			Running,
			Succeeded,
			Failed,
			Canceled
		}

		public PipelineState State
		{
			get => m_state;
			set
			{
				m_state = value;
				switch( m_state )
				{
				case PipelineState.NotStarted:
					m_imageStatus.Image = Properties.Resources.waiting;
					break;
				case PipelineState.Running:
					m_imageStatus.Image = Properties.Resources.running;
					break;
				case PipelineState.Succeeded:
					m_imageStatus.Image = Properties.Resources.success;
					break;
				case PipelineState.Failed:
					m_imageStatus.Image = Properties.Resources.failure;
					break;
				case PipelineState.Canceled:
					m_imageStatus.Image = Properties.Resources.canceled;
					break;
				}
			}
		}
		private PipelineState m_state;

		private void RunStatusView_DoubleClick( object? sender, EventArgs e )
		{
			ProcessStartInfo psi = new ProcessStartInfo();
			psi.FileName = m_run.Url;
			psi.UseShellExecute = true;
			Process.Start( psi );
		}

		private void cmdDismiss_LinkClicked( object sender, LinkLabelLinkClickedEventArgs e )
		{
			WatchWnd.Instance?.DismissRun( this );
		}

		private string FormatTimeShort( DateTimeOffset time )
		{
			DateTimeOffset now = DateTimeOffset.Now;
			if( now.Day != time.Day )
				return time.ToLocalTime().ToString( "MMM d" );

			var elapsed = now - time;
			if( elapsed.TotalHours < 1 )
				return CreateAgoString( "minute", (int)elapsed.TotalMinutes );
			else
				return CreateAgoString( "hour", (int)Math.Round( elapsed.TotalHours ) );
		}

		private static string CreateAgoString( string unit, int count )
		{
			return string.Format( "{0} {1} ago", count,
				count == 1 ? unit : unit + "s" );
		}
	}
}
