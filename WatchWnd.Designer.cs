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
			m_refreshTimer = new System.Windows.Forms.Timer( components );
			m_flowPanel = new FlowLayoutPanel();
			m_toolStrip = new ToolStrip();
			m_toolRefresh = new ToolStripButton();
			m_toolSettings = new ToolStripButton();
			m_toolStrip.SuspendLayout();
			SuspendLayout();
			// 
			// m_flowPanel
			// 
			m_flowPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			m_flowPanel.Location = new Point( 0, 26 );
			m_flowPanel.Name = "m_flowPanel";
			m_flowPanel.Size = new Size( 316, 127 );
			m_flowPanel.TabIndex = 0;
			// 
			// m_toolStrip
			// 
			m_toolStrip.Items.AddRange( new ToolStripItem[] { m_toolRefresh, m_toolSettings } );
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
			// WatchWnd
			// 
			AllowDrop = true;
			AutoScaleDimensions = new SizeF( 7F, 15F );
			AutoScaleMode = AutoScaleMode.Font;
			BackColor = SystemColors.Window;
			ClientSize = new Size( 316, 153 );
			Controls.Add( m_toolStrip );
			Controls.Add( m_flowPanel );
			Name = "WatchWnd";
			Text = "Pipe Watch";
			m_toolStrip.ResumeLayout( false );
			m_toolStrip.PerformLayout();
			ResumeLayout( false );
			PerformLayout();
		}

		#endregion
		private System.Windows.Forms.Timer m_refreshTimer;
		private FlowLayoutPanel m_flowPanel;
		private ToolStrip m_toolStrip;
		private ToolStripButton m_toolRefresh;
		private ToolStripButton m_toolSettings;
	}
}
