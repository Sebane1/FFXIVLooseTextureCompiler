using FFXIVVoicePackCreator;

namespace FFXIVLooseTextureCompiler {
    partial class FindAndReplace {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FindAndReplace));
            this.acceptChangesButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.replacementString = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.normal = new FFXIVVoicePackCreator.FilePicker();
            this.multi = new FFXIVVoicePackCreator.FilePicker();
            this.diffuse = new FFXIVVoicePackCreator.FilePicker();
            this.mask = new FFXIVVoicePackCreator.FilePicker();
            this.glow = new FFXIVVoicePackCreator.FilePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.groupTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // acceptChangesButton
            // 
            this.acceptChangesButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptChangesButton.Location = new System.Drawing.Point(200, 320);
            this.acceptChangesButton.Name = "acceptChangesButton";
            this.acceptChangesButton.Size = new System.Drawing.Size(143, 24);
            this.acceptChangesButton.TabIndex = 8;
            this.acceptChangesButton.Text = "Confirm Replaceement";
            this.acceptChangesButton.UseVisualStyleBackColor = true;
            this.acceptChangesButton.Click += new System.EventHandler(this.acceptChangesButton_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(344, 320);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(59, 24);
            this.button1.TabIndex = 9;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label5.Location = new System.Drawing.Point(4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(307, 30);
            this.label5.TabIndex = 11;
            this.label5.Text = "Find Containing This In Name";
            // 
            // replacementString
            // 
            this.replacementString.Location = new System.Drawing.Point(4, 36);
            this.replacementString.Name = "replacementString";
            this.replacementString.Size = new System.Drawing.Size(400, 23);
            this.replacementString.TabIndex = 10;
            this.replacementString.TextChanged += new System.EventHandler(this.replacementString_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(4, 120);
            this.label1.Name = "label1";
            this.label1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label1.Size = new System.Drawing.Size(371, 30);
            this.label1.TabIndex = 12;
            this.label1.Text = "Then Replace Desired Files With This";
            // 
            // normal
            // 
            this.normal.CurrentPath = null;
            this.normal.Filter = null;
            this.normal.Index = -1;
            this.normal.Location = new System.Drawing.Point(5, 190);
            this.normal.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.normal.MinimumSize = new System.Drawing.Size(300, 28);
            this.normal.Name = "normal";
            this.normal.Size = new System.Drawing.Size(404, 28);
            this.normal.TabIndex = 14;
            // 
            // multi
            // 
            this.multi.CurrentPath = null;
            this.multi.Filter = null;
            this.multi.Index = -1;
            this.multi.Location = new System.Drawing.Point(5, 222);
            this.multi.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.multi.MinimumSize = new System.Drawing.Size(300, 28);
            this.multi.Name = "multi";
            this.multi.Size = new System.Drawing.Size(404, 28);
            this.multi.TabIndex = 15;
            // 
            // diffuse
            // 
            this.diffuse.CurrentPath = null;
            this.diffuse.Filter = null;
            this.diffuse.Index = -1;
            this.diffuse.Location = new System.Drawing.Point(5, 156);
            this.diffuse.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.diffuse.MinimumSize = new System.Drawing.Size(300, 28);
            this.diffuse.Name = "diffuse";
            this.diffuse.Size = new System.Drawing.Size(404, 28);
            this.diffuse.TabIndex = 16;
            // 
            // mask
            // 
            this.mask.CurrentPath = null;
            this.mask.Filter = null;
            this.mask.Index = -1;
            this.mask.Location = new System.Drawing.Point(5, 254);
            this.mask.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.mask.MinimumSize = new System.Drawing.Size(300, 28);
            this.mask.Name = "mask";
            this.mask.Size = new System.Drawing.Size(404, 28);
            this.mask.TabIndex = 17;
            // 
            // glow
            // 
            this.glow.CurrentPath = null;
            this.glow.Filter = null;
            this.glow.Index = -1;
            this.glow.Location = new System.Drawing.Point(5, 286);
            this.glow.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.glow.MinimumSize = new System.Drawing.Size(300, 28);
            this.glow.Name = "glow";
            this.glow.Size = new System.Drawing.Size(404, 28);
            this.glow.TabIndex = 18;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(4, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(194, 30);
            this.label2.TabIndex = 19;
            this.label2.Text = "And In This Group";
            // 
            // groupTextBox
            // 
            this.groupTextBox.Location = new System.Drawing.Point(4, 92);
            this.groupTextBox.Name = "groupTextBox";
            this.groupTextBox.Size = new System.Drawing.Size(400, 23);
            this.groupTextBox.TabIndex = 20;
            // 
            // FindAndReplace
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(414, 346);
            this.Controls.Add(this.groupTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.glow);
            this.Controls.Add(this.mask);
            this.Controls.Add(this.diffuse);
            this.Controls.Add(this.multi);
            this.Controls.Add(this.normal);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.replacementString);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.acceptChangesButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FindAndReplace";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Find And Bulk Replace";
            this.Load += new System.EventHandler(this.CustomPathDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Button acceptChangesButton;
        private Button button1;
        private Label label5;
        private TextBox replacementString;
        private Label label1;
        private FFXIVVoicePackCreator.FilePicker normal;
        private FFXIVVoicePackCreator.FilePicker multi;
        private FFXIVVoicePackCreator.FilePicker diffuse;
        private FilePicker mask;
        private FilePicker glow;
        private Label label2;
        private TextBox groupTextBox;

        public FilePicker Diffuse { get => diffuse; set => diffuse = value; }
        public FilePicker Multi { get => multi; set => multi = value; }
        public FilePicker Normal { get => normal; set => normal = value; }
        public TextBox ReplacementString { get => replacementString; set => replacementString = value; }
        public TextBox ReplacementGroup { get => groupTextBox; set => groupTextBox = value; }
        public FilePicker Mask { get => mask; set => mask = value; }
        public FilePicker Glow { get => glow; set => glow = value; }
    }
}