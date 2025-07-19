namespace FFXIVLooseTextureCompiler {
    partial class ConnectionDisplay {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConnectionDisplay));
            label1 = new Label();
            sharingTextbox = new TextBox();
            label2 = new Label();
            sendId = new TextBox();
            sendModButton = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold);
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(254, 37);
            label1.TabIndex = 0;
            label1.Text = "Share This Id Code";
            // 
            // sharingTextbox
            // 
            sharingTextbox.Location = new Point(0, 40);
            sharingTextbox.Name = "sharingTextbox";
            sharingTextbox.Size = new Size(348, 23);
            sharingTextbox.TabIndex = 1;
            sharingTextbox.TextChanged += sharingTextbox_TextChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold);
            label2.Location = new Point(0, 64);
            label2.Name = "label2";
            label2.Size = new Size(259, 37);
            label2.TabIndex = 2;
            label2.Text = "Enter Id Code Here";
            // 
            // sendId
            // 
            sendId.Location = new Point(1, 104);
            sendId.Name = "sendId";
            sendId.Size = new Size(348, 23);
            sendId.TabIndex = 3;
            // 
            // sendModButton
            // 
            sendModButton.Location = new Point(0, 136);
            sendModButton.Name = "sendModButton";
            sendModButton.Size = new Size(352, 32);
            sendModButton.TabIndex = 4;
            sendModButton.Text = "Send Current Mod";
            sendModButton.UseVisualStyleBackColor = true;
            sendModButton.Click += sendModButton_Click;
            // 
            // ConnectionDisplay
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(351, 169);
            ControlBox = false;
            Controls.Add(sendModButton);
            Controls.Add(sendId);
            Controls.Add(label2);
            Controls.Add(sharingTextbox);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ConnectionDisplay";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FFXIV Loose Texture Compiler";
            FormClosing += ConnectionDisplay_FormClosing;
            Load += ConnectionDisplay_Load;
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private Label label1;
        private TextBox sharingTextbox;
        private Label label2;
        private TextBox sendId;
        private Button sendModButton;
    }
}