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
            this.label1 = new System.Windows.Forms.Label();
            this.sharingTextbox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.sendId = new System.Windows.Forms.TextBox();
            this.sendModButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(254, 37);
            this.label1.TabIndex = 0;
            this.label1.Text = "Share This Id Code";
            // 
            // sharingTextbox
            // 
            this.sharingTextbox.Location = new System.Drawing.Point(0, 40);
            this.sharingTextbox.Name = "sharingTextbox";
            this.sharingTextbox.Size = new System.Drawing.Size(348, 23);
            this.sharingTextbox.TabIndex = 1;
            this.sharingTextbox.TextChanged += new System.EventHandler(this.sharingTextbox_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(0, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(259, 37);
            this.label2.TabIndex = 2;
            this.label2.Text = "Enter Id Code Here";
            // 
            // sendId
            // 
            this.sendId.Location = new System.Drawing.Point(1, 104);
            this.sendId.Name = "sendId";
            this.sendId.Size = new System.Drawing.Size(348, 23);
            this.sendId.TabIndex = 3;
            // 
            // sendModButton
            // 
            this.sendModButton.Location = new System.Drawing.Point(0, 136);
            this.sendModButton.Name = "sendModButton";
            this.sendModButton.Size = new System.Drawing.Size(352, 32);
            this.sendModButton.TabIndex = 4;
            this.sendModButton.Text = "Send Current Mod";
            this.sendModButton.UseVisualStyleBackColor = true;
            this.sendModButton.Click += new System.EventHandler(this.sendModButton_Click);
            // 
            // ConnectionDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(351, 169);
            this.ControlBox = false;
            this.Controls.Add(this.sendModButton);
            this.Controls.Add(this.sendId);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.sharingTextbox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConnectionDisplay";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FFXIV Loose Texture Compiler";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label1;
        private TextBox sharingTextbox;
        private Label label2;
        private TextBox sendId;
        private Button sendModButton;
    }
}