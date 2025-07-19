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
            acceptChangesButton = new Button();
            button1 = new Button();
            label5 = new Label();
            replacementString = new TextBox();
            label1 = new Label();
            normal = new FilePicker();
            mask = new FilePicker();
            baseTexture = new FilePicker();
            bounds = new FilePicker();
            glow = new FilePicker();
            label2 = new Label();
            groupTextBox = new TextBox();
            material = new FilePicker();
            layersMaskButton = new Button();
            layerNormalButton = new Button();
            layerBaseButton = new Button();
            SuspendLayout();
            // 
            // acceptChangesButton
            // 
            acceptChangesButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            acceptChangesButton.Location = new Point(266, 346);
            acceptChangesButton.Name = "acceptChangesButton";
            acceptChangesButton.Size = new Size(143, 24);
            acceptChangesButton.TabIndex = 8;
            acceptChangesButton.Text = "Confirm Replacement";
            acceptChangesButton.UseVisualStyleBackColor = true;
            acceptChangesButton.Click += acceptChangesButton_Click;
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button1.Location = new Point(415, 346);
            button1.Name = "button1";
            button1.Size = new Size(140, 24);
            button1.TabIndex = 9;
            button1.Text = "Cancel";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold);
            label5.Location = new Point(4, 0);
            label5.Name = "label5";
            label5.Size = new Size(307, 30);
            label5.TabIndex = 11;
            label5.Text = "Find Containing This In Name";
            // 
            // replacementString
            // 
            replacementString.Location = new Point(4, 36);
            replacementString.Name = "replacementString";
            replacementString.Size = new Size(551, 23);
            replacementString.TabIndex = 10;
            replacementString.TextChanged += replacementString_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold);
            label1.Location = new Point(4, 120);
            label1.Name = "label1";
            label1.RightToLeft = RightToLeft.Yes;
            label1.Size = new Size(371, 30);
            label1.TabIndex = 12;
            label1.Text = "Then Replace Desired Files With This";
            // 
            // normal
            // 
            normal.BackColor = SystemColors.GradientInactiveCaption;
            normal.CurrentPath = null;
            normal.Filter = null;
            normal.Index = -1;
            normal.IsMaterial = false;
            normal.Location = new Point(4, 190);
            normal.Margin = new Padding(4, 3, 4, 3);
            normal.MinimumSize = new Size(300, 28);
            normal.Name = "normal";
            normal.Size = new Size(427, 28);
            normal.TabIndex = 14;
            // 
            // mask
            // 
            mask.BackColor = Color.FromArgb(255, 192, 255);
            mask.CurrentPath = null;
            mask.Filter = null;
            mask.Index = -1;
            mask.IsMaterial = false;
            mask.Location = new Point(5, 222);
            mask.Margin = new Padding(4, 3, 4, 3);
            mask.MinimumSize = new Size(300, 28);
            mask.Name = "mask";
            mask.Size = new Size(426, 28);
            mask.TabIndex = 15;
            mask.Load += multi_Load;
            // 
            // baseTexture
            // 
            baseTexture.BackColor = Color.LavenderBlush;
            baseTexture.CurrentPath = null;
            baseTexture.Filter = null;
            baseTexture.Index = -1;
            baseTexture.IsMaterial = false;
            baseTexture.Location = new Point(5, 156);
            baseTexture.Margin = new Padding(4, 3, 4, 3);
            baseTexture.MinimumSize = new Size(300, 28);
            baseTexture.Name = "baseTexture";
            baseTexture.Size = new Size(427, 28);
            baseTexture.TabIndex = 16;
            // 
            // bounds
            // 
            bounds.CurrentPath = null;
            bounds.Filter = null;
            bounds.Index = -1;
            bounds.IsMaterial = false;
            bounds.Location = new Point(5, 254);
            bounds.Margin = new Padding(4, 3, 4, 3);
            bounds.MinimumSize = new Size(300, 28);
            bounds.Name = "bounds";
            bounds.Size = new Size(550, 28);
            bounds.TabIndex = 17;
            // 
            // glow
            // 
            glow.CurrentPath = null;
            glow.Filter = null;
            glow.Index = -1;
            glow.IsMaterial = false;
            glow.Location = new Point(5, 316);
            glow.Margin = new Padding(4, 3, 4, 3);
            glow.MinimumSize = new Size(300, 28);
            glow.Name = "glow";
            glow.Size = new Size(550, 28);
            glow.TabIndex = 18;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold);
            label2.Location = new Point(4, 60);
            label2.Name = "label2";
            label2.Size = new Size(194, 30);
            label2.TabIndex = 19;
            label2.Text = "And In This Group";
            // 
            // groupTextBox
            // 
            groupTextBox.Location = new Point(4, 92);
            groupTextBox.Name = "groupTextBox";
            groupTextBox.Size = new Size(551, 23);
            groupTextBox.TabIndex = 20;
            // 
            // material
            // 
            material.CurrentPath = null;
            material.Filter = null;
            material.Index = -1;
            material.IsMaterial = true;
            material.Location = new Point(5, 284);
            material.Margin = new Padding(4, 3, 4, 3);
            material.MinimumSize = new Size(300, 28);
            material.Name = "material";
            material.Size = new Size(550, 28);
            material.TabIndex = 21;
            // 
            // layersMaskButton
            // 
            layersMaskButton.Location = new Point(438, 224);
            layersMaskButton.Name = "layersMaskButton";
            layersMaskButton.Size = new Size(116, 23);
            layersMaskButton.TabIndex = 60;
            layersMaskButton.Text = "Layers";
            layersMaskButton.UseVisualStyleBackColor = true;
            layersMaskButton.Click += layersMaskButton_Click;
            // 
            // layerNormalButton
            // 
            layerNormalButton.Location = new Point(438, 192);
            layerNormalButton.Name = "layerNormalButton";
            layerNormalButton.Size = new Size(117, 23);
            layerNormalButton.TabIndex = 59;
            layerNormalButton.Text = "Layers";
            layerNormalButton.UseVisualStyleBackColor = true;
            layerNormalButton.Click += layerNormalButton_Click;
            // 
            // layerBaseButton
            // 
            layerBaseButton.Location = new Point(438, 159);
            layerBaseButton.Name = "layerBaseButton";
            layerBaseButton.Size = new Size(117, 23);
            layerBaseButton.TabIndex = 58;
            layerBaseButton.Text = "Layers";
            layerBaseButton.UseVisualStyleBackColor = true;
            layerBaseButton.Click += layerBaseButton_Click;
            // 
            // FindAndReplace
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(557, 372);
            Controls.Add(layersMaskButton);
            Controls.Add(layerNormalButton);
            Controls.Add(layerBaseButton);
            Controls.Add(material);
            Controls.Add(groupTextBox);
            Controls.Add(label2);
            Controls.Add(glow);
            Controls.Add(bounds);
            Controls.Add(baseTexture);
            Controls.Add(mask);
            Controls.Add(normal);
            Controls.Add(label1);
            Controls.Add(label5);
            Controls.Add(replacementString);
            Controls.Add(button1);
            Controls.Add(acceptChangesButton);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "FindAndReplace";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Find And Bulk Replace";
            Load += CustomPathDialog_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button acceptChangesButton;
        private Button button1;
        private Label label5;
        private TextBox replacementString;
        private Label label1;
        private FFXIVVoicePackCreator.FilePicker normal;
        private FFXIVVoicePackCreator.FilePicker mask;
        private FFXIVVoicePackCreator.FilePicker baseTexture;
        private FilePicker bounds;
        private FilePicker glow;
        private Label label2;
        private TextBox groupTextBox;
        private FilePicker material;
        private Button layersMaskButton;
        private Button layerNormalButton;
        private Button layerBaseButton;

        public FilePicker Base { get => baseTexture; set => baseTexture = value; }
        public FilePicker Mask { get => mask; set => mask = value; }
        public FilePicker Normal { get => normal; set => normal = value; }
        public TextBox ReplacementString { get => replacementString; set => replacementString = value; }
        public TextBox ReplacementGroup { get => groupTextBox; set => groupTextBox = value; }
        public FilePicker Bounds { get => bounds; set => bounds = value; }
        public FilePicker Glow { get => glow; set => glow = value; }
    }
}