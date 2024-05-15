namespace PipeWatch
{
	using PipeWatch.Properties;

	public partial class SettingsDlg : Form
	{
		public SettingsDlg()
		{
			InitializeComponent();
			m_textRequestorId.Text = Settings.Default.GitUserName;
			m_textDevOpsPat.Text = Settings.Default.GitApiKey;
			m_textProjectList.Text = Settings.Default.WatchProjects;
		}

		/// <summary>
		/// Checks that the given settings are valid and usable, showing a message if they aren't.
		/// </summary>
		internal static bool ValidateSettingsAndWarn( IWin32Window parent, Settings settings )
		{
			DevopsAccess access = new DevopsAccess( settings );
			if( !access.ValidateUserSettings( out string? invalidReason ) )
			{
				MessageBox.Show( parent, "The configured user settings are invalid:\n\n" + invalidReason,
					"Missing input", MessageBoxButtons.OK, MessageBoxIcon.Error );
				return false;
			}

			return true;
		}

		private void m_btnSave_Click( object sender, EventArgs e )
		{
			if( m_textRequestorId.Text == string.Empty ||
				m_textDevOpsPat.Text == string.Empty ||
				m_textProjectList.Text == string.Empty )
			{
				MessageBox.Show( this, "Please provide a value for all settings before proceeding.",
					"Missing input", MessageBoxButtons.OK, MessageBoxIcon.Warning );
				return;
			}

			string[] projects = m_textProjectList.Text
				.Split( ";" )
				.Select( x => x.Trim() )
				.Where( x => x.Length > 0 )
				.ToArray();
			if( projects.Length == 0 )
			{
				MessageBox.Show( this, "Please specify at least one DevOps project to monitor, separated by semicolons.",
					"Missing input", MessageBoxButtons.OK, MessageBoxIcon.Warning );
				return;
			}

			Settings.Default.GitUserName = m_textRequestorId.Text;
			Settings.Default.GitApiKey = m_textDevOpsPat.Text;
			Settings.Default.WatchProjects = string.Join( ';', projects );
			if( !ValidateSettingsAndWarn( this, Settings.Default ) )
			{
				Settings.Default.Reload();
				return;
			}

			Settings.Default.Save();
			this.DialogResult = DialogResult.OK;
		}
	}
}
