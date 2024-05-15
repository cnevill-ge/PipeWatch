namespace PipeWatch
{
	partial class WatchWnd
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose( bool disposing )
		{
			if( disposing && (components != null) )
			{
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		/// <summary>
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( WatchWnd ) );
			m_refreshTimer = new System.Windows.Forms.Timer( components );
			m_flowPanel = new FlowLayoutPanel();
			m_toolStrip = new ToolStrip();
			m_toolRefresh = new ToolStripButton();
			m_toolSettings = new ToolStripButton();
			m_statusStrip = new StatusStrip();
			m_statusError = new ToolStripStatusLabel();
			m_toolDismissAll = new ToolStripButton();
			m_toolStrip.SuspendLayout();
			m_statusStrip.SuspendLayout();
			SuspendLayout();
			// 
			// m_flowPanel
			// 
			m_flowPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			m_flowPanel.Location = new Point( 0, 26 );
			m_flowPanel.Name = "m_flowPanel";
			m_flowPanel.Size = new Size( 316, 102 );
			m_flowPanel.TabIndex = 0;
			// 
			// m_toolStrip
			// 
			m_toolStrip.Items.AddRange( new ToolStripItem[] { m_toolRefresh, m_toolSettings, m_toolDismissAll } );
			m_toolStrip.Location = new Point( 0, 0 );
			m_toolStrip.Name = "m_toolStrip";
			m_toolStrip.Size = new Size( 316, 25 );
			m_toolStrip.TabIndex = 1;
			m_toolStrip.Text = "toolStrip1";
			// 
			// m_toolRefresh
			// 
			m_toolRefresh.Image = Properties.Resources.refresh;
			m_toolRefresh.ImageTransparentColor = Color.Magenta;
			m_toolRefresh.Name = "m_toolRefresh";
			m_toolRefresh.Size = new Size( 66, 22 );
			m_toolRefresh.Text = "Refresh";
			m_toolRefresh.Click += toolRefresh_Click;
			// 
			// m_toolSettings
			// 
			m_toolSettings.Image = Properties.Resources.settings;
			m_toolSettings.ImageTransparentColor = Color.Magenta;
			m_toolSettings.Name = "m_toolSettings";
			m_toolSettings.Size = new Size( 78, 22 );
			m_toolSettings.Text = "Settings...";
			m_toolSettings.Click += toolSettings_Click;
			// 
			// m_statusStrip
			// 
			m_statusStrip.Items.AddRange( new ToolStripItem[] { m_statusError } );
			m_statusStrip.Location = new Point( 0, 131 );
			m_statusStrip.Name = "m_statusStrip";
			m_statusStrip.Size = new Size( 316, 22 );
			m_statusStrip.TabIndex = 2;
			m_statusStrip.Text = "statusStrip1";
			// 
			// m_statusError
			// 
			m_statusError.BackColor = Color.Transparent;
			m_statusError.Image = Properties.Resources.failure;
			m_statusError.IsLink = true;
			m_statusError.Name = "m_statusError";
			m_statusError.Size = new Size( 98, 17 );
			m_statusError.Text = "Error occurred";
			m_statusError.Click += statusError_Click;
			// 
			// m_toolDismissAll
			// 
			m_toolDismissAll.DisplayStyle = ToolStripItemDisplayStyle.Text;
			m_toolDismissAll.Image = (Image)resources.GetObject( "m_toolDismissAll.Image" );
			m_toolDismissAll.ImageTransparentColor = Color.Magenta;
			m_toolDismissAll.Name = "m_toolDismissAll";
			m_toolDismissAll.Size = new Size( 68, 22 );
			m_toolDismissAll.Text = "Dismiss All";
			m_toolDismissAll.Click += toolDismissAll_Click;
			// 
			// WatchWnd
			// 
			AllowDrop = true;
			AutoScaleDimensions = new SizeF( 7F, 15F );
			AutoScaleMode = AutoScaleMode.Font;
			BackColor = SystemColors.Window;
			ClientSize = new Size( 316, 153 );
			Controls.Add( m_statusStrip );
			Controls.Add( m_toolStrip );
			Controls.Add( m_flowPanel );
			Name = "WatchWnd";
			Text = "Pipe Watch";
			m_toolStrip.ResumeLayout( false );
			m_toolStrip.PerformLayout();
			m_statusStrip.ResumeLayout( false );
			m_statusStrip.PerformLayout();
			ResumeLayout( false );
			PerformLayout();
		}

		#endregion
		private System.Windows.Forms.Timer m_refreshTimer;
		private FlowLayoutPanel m_flowPanel;
		private ToolStrip m_toolStrip;
		private ToolStripButton m_toolRefresh;
		private ToolStripButton m_toolSettings;
		private StatusStrip m_statusStrip;
		private ToolStripStatusLabel m_statusError;
		private ToolStripButton m_toolDismissAll;
	}
}
