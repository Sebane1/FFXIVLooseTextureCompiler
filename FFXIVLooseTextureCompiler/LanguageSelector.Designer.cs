namespace FFXIVLooseTextureCompiler {
    partial class LanguageSelector {
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
            englishButton = new Button();
            frenchButton = new Button();
            germanButton = new Button();
            japaneseButton = new Button();
            chineseButton = new Button();
            swedishButton = new Button();
            SuspendLayout();
            // 
            // englishButton
            // 
            englishButton.Location = new Point(12, 12);
            englishButton.Name = "englishButton";
            englishButton.Size = new Size(176, 33);
            englishButton.TabIndex = 0;
            englishButton.Text = "English";
            englishButton.UseVisualStyleBackColor = true;
            englishButton.Click += englishButton_Click;
            // 
            // frenchButton
            // 
            frenchButton.Location = new Point(12, 51);
            frenchButton.Name = "frenchButton";
            frenchButton.Size = new Size(176, 33);
            frenchButton.TabIndex = 1;
            frenchButton.Text = "Français";
            frenchButton.UseVisualStyleBackColor = true;
            frenchButton.Click += frenchButton_Click;
            // 
            // germanButton
            // 
            germanButton.Location = new Point(12, 90);
            germanButton.Name = "germanButton";
            germanButton.Size = new Size(176, 33);
            germanButton.TabIndex = 2;
            germanButton.Text = "Deutsch";
            germanButton.UseVisualStyleBackColor = true;
            germanButton.Click += germanButton_Click;
            // 
            // japaneseButton
            // 
            japaneseButton.Location = new Point(12, 129);
            japaneseButton.Name = "japaneseButton";
            japaneseButton.Size = new Size(176, 33);
            japaneseButton.TabIndex = 3;
            japaneseButton.Text = "日本語";
            japaneseButton.UseVisualStyleBackColor = true;
            japaneseButton.Click += japaneseButton_Click;
            // 
            // chineseButton
            // 
            chineseButton.Location = new Point(12, 168);
            chineseButton.Name = "chineseButton";
            chineseButton.Size = new Size(176, 33);
            chineseButton.TabIndex = 4;
            chineseButton.Text = "中国人";
            chineseButton.UseVisualStyleBackColor = true;
            chineseButton.Click += chineseButton_Click;
            // 
            // swedishButton
            // 
            swedishButton.Location = new Point(12, 207);
            swedishButton.Name = "swedishButton";
            swedishButton.Size = new Size(176, 33);
            swedishButton.TabIndex = 5;
            swedishButton.Text = "Svensk";
            swedishButton.UseVisualStyleBackColor = true;
            swedishButton.Click += swedishButton_Click;
            // 
            // LanguageSelector
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(200, 253);
            Controls.Add(swedishButton);
            Controls.Add(chineseButton);
            Controls.Add(japaneseButton);
            Controls.Add(germanButton);
            Controls.Add(frenchButton);
            Controls.Add(englishButton);
            FormBorderStyle = FormBorderStyle.None;
            Name = "LanguageSelector";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "LanguageSelector";
            TopMost = true;
            ResumeLayout(false);
        }

        #endregion

        private Button englishButton;
        private Button frenchButton;
        private Button germanButton;
        private Button japaneseButton;
        private Button chineseButton;
        private Button swedishButton;
    }
}