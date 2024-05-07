namespace PipeWatch
{
	partial class RunStatusView
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			m_labelPipelineName = new Label();
			m_labelBuildDescription = new Label();
			m_imageStatus = new PictureBox();
			m_cmdDismiss = new LinkLabel();
			m_labelStartTime = new Label();
			((System.ComponentModel.ISupportInitialize)m_imageStatus).BeginInit();
			SuspendLayout();
			// 
			// m_labelPipelineName
			// 
			m_labelPipelineName.AutoSize = true;
			m_labelPipelineName.Font = new Font( "Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0 );
			m_labelPipelineName.Location = new Point( 25, 4 );
			m_labelPipelineName.Name = "m_labelPipelineName";
			m_labelPipelineName.Size = new Size( 131, 17 );
			m_labelPipelineName.TabIndex = 0;
			m_labelPipelineName.Text = "Pipeline Name Here";
			// 
			// m_labelBuildDescription
			// 
			m_labelBuildDescription.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			m_labelBuildDescription.Location = new Point( 3, 26 );
			m_labelBuildDescription.Name = "m_labelBuildDescription";
			m_labelBuildDescription.Size = new Size( 365, 43 );
			m_labelBuildDescription.TabIndex = 1;
			m_labelBuildDescription.Text = "Build description here";
			// 
			// m_imageStatus
			// 
			m_imageStatus.Location = new Point( 3, 3 );
			m_imageStatus.Name = "m_imageStatus";
			m_imageStatus.Size = new Size( 20, 20 );
			m_imageStatus.TabIndex = 2;
			m_imageStatus.TabStop = false;
			// 
			// m_cmdDismiss
			// 
			m_cmdDismiss.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			m_cmdDismiss.AutoSize = true;
			m_cmdDismiss.Location = new Point( 321, 5 );
			m_cmdDismiss.Name = "m_cmdDismiss";
			m_cmdDismiss.Size = new Size( 47, 15 );
			m_cmdDismiss.TabIndex = 3;
			m_cmdDismiss.TabStop = true;
			m_cmdDismiss.Text = "Dismiss";
			m_cmdDismiss.LinkClicked += cmdDismiss_LinkClicked;
			// 
			// m_labelStartTime
			// 
			m_labelStartTime.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			m_labelStartTime.Location = new Point( 222, 4 );
			m_labelStartTime.Name = "m_labelStartTime";
			m_labelStartTime.Size = new Size( 93, 17 );
			m_labelStartTime.TabIndex = 4;
			m_labelStartTime.Text = "label1";
			m_labelStartTime.TextAlign = ContentAlignment.MiddleRight;
			// 
			// RunStatusView
			// 
			AutoScaleDimensions = new SizeF( 7F, 15F );
			AutoScaleMode = AutoScaleMode.Font;
			BackColor = SystemColors.Control;
			BorderStyle = BorderStyle.FixedSingle;
			Controls.Add( m_labelStartTime );
			Controls.Add( m_cmdDismiss );
			Controls.Add( m_imageStatus );
			Controls.Add( m_labelBuildDescription );
			Controls.Add( m_labelPipelineName );
			Name = "RunStatusView";
			Size = new Size( 371, 69 );
			DoubleClick += RunStatusView_DoubleClick;
			((System.ComponentModel.ISupportInitialize)m_imageStatus).EndInit();
			ResumeLayout( false );
			PerformLayout();
		}

		#endregion

		private Label m_labelPipelineName;
		private Label m_labelBuildDescription;
		private PictureBox m_imageStatus;
		private LinkLabel m_cmdDismiss;
		private Label m_labelStartTime;
	}
}
