namespace PipeWatch
{
	partial class SettingsDlg
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			label1 = new Label();
			m_textRequestorId = new TextBox();
			m_textDevOpsPat = new TextBox();
			label2 = new Label();
			m_textProjectList = new TextBox();
			label3 = new Label();
			m_btnCancel = new Button();
			m_btnSave = new Button();
			SuspendLayout();
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new Point( 12, 9 );
			label1.Name = "label1";
			label1.Size = new Size( 296, 15 );
			label1.TabIndex = 0;
			label1.Text = "Build requestor of interest (probably first.last@ge.com)";
			// 
			// m_textRequestorId
			// 
			m_textRequestorId.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			m_textRequestorId.Location = new Point( 12, 27 );
			m_textRequestorId.Name = "m_textRequestorId";
			m_textRequestorId.Size = new Size( 350, 23 );
			m_textRequestorId.TabIndex = 1;
			// 
			// m_textDevOpsPat
			// 
			m_textDevOpsPat.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			m_textDevOpsPat.Location = new Point( 12, 82 );
			m_textDevOpsPat.Name = "m_textDevOpsPat";
			m_textDevOpsPat.PasswordChar = '*';
			m_textDevOpsPat.Size = new Size( 350, 23 );
			m_textDevOpsPat.TabIndex = 3;
			m_textDevOpsPat.Text = "abcdefg";
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new Point( 12, 64 );
			label2.Name = "label2";
			label2.Size = new Size( 307, 15 );
			label2.TabIndex = 2;
			label2.Text = "Your DevOps PAT (needs Read access for Build and Code)";
			// 
			// m_textProjectList
			// 
			m_textProjectList.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			m_textProjectList.Location = new Point( 12, 142 );
			m_textProjectList.Name = "m_textProjectList";
			m_textProjectList.Size = new Size( 350, 23 );
			m_textProjectList.TabIndex = 5;
			// 
			// label3
			// 
			label3.AutoSize = true;
			label3.Location = new Point( 12, 124 );
			label3.Name = "label3";
			label3.Size = new Size( 275, 15 );
			label3.TabIndex = 4;
			label3.Text = "DevOps projects to monitor (semicolon-separated)";
			// 
			// m_btnCancel
			// 
			m_btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			m_btnCancel.Location = new Point( 287, 181 );
			m_btnCancel.Name = "m_btnCancel";
			m_btnCancel.Size = new Size( 75, 23 );
			m_btnCancel.TabIndex = 6;
			m_btnCancel.Text = "Cancel";
			m_btnCancel.UseVisualStyleBackColor = true;
			// 
			// m_btnSave
			// 
			m_btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			m_btnSave.Location = new Point( 206, 181 );
			m_btnSave.Name = "m_btnSave";
			m_btnSave.Size = new Size( 75, 23 );
			m_btnSave.TabIndex = 7;
			m_btnSave.Text = "Save";
			m_btnSave.UseVisualStyleBackColor = true;
			m_btnSave.Click += m_btnSave_Click;
			// 
			// SettingsDlg
			// 
			AcceptButton = m_btnSave;
			AutoScaleDimensions = new SizeF( 7F, 15F );
			AutoScaleMode = AutoScaleMode.Font;
			CancelButton = m_btnCancel;
			ClientSize = new Size( 374, 216 );
			ControlBox = false;
			Controls.Add( m_btnSave );
			Controls.Add( m_btnCancel );
			Controls.Add( m_textProjectList );
			Controls.Add( label3 );
			Controls.Add( m_textDevOpsPat );
			Controls.Add( label2 );
			Controls.Add( m_textRequestorId );
			Controls.Add( label1 );
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "SettingsDlg";
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterParent;
			Text = "Settings";
			ResumeLayout( false );
			PerformLayout();
		}

		#endregion

		private Label label1;
		private TextBox m_textRequestorId;
		private TextBox m_textDevOpsPat;
		private Label label2;
		private TextBox m_textProjectList;
		private Label label3;
		private Button m_btnCancel;
		private Button m_btnSave;
	}
}