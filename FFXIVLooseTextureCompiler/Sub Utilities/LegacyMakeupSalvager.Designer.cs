namespace FFXIVLooseTextureCompiler.Sub_Utilities {
    partial class LegacyMakeupSalvager {
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
            subRaceListBox = new ComboBox();
            faceNumberListBox = new ComboBox();
            convertMakeupButton = new Button();
            skipUnderlayCheckBox = new CheckBox();
            racialGender = new ComboBox();
            skipLipCorrection = new CheckBox();
            textBox1 = new TextBox();
            textureIsNormalMap = new CheckBox();
            SuspendLayout();
            // 
            // subRaceListBox
            // 
            subRaceListBox.FormattingEnabled = true;
            subRaceListBox.Items.AddRange(new object[] { "Midlander", "Highlander", "Wildwood", "Duskwight", "Plainsfolk", "Dunesfolk", "Seeker", "Keeper", "Sea Wolf", "Hellsguard", "Raen", "Xaela", "Helion", "The Lost", "Rava", "Veena" });
            subRaceListBox.Location = new Point(0, 0);
            subRaceListBox.Name = "subRaceListBox";
            subRaceListBox.Size = new Size(204, 23);
            subRaceListBox.TabIndex = 0;
            // 
            // faceNumberListBox
            // 
            faceNumberListBox.FormattingEnabled = true;
            faceNumberListBox.Items.AddRange(new object[] { "Face 1", "Face 2", "Face 3", "Face 4", "Face 5", "Face 6", "Face 7" });
            faceNumberListBox.Location = new Point(210, 0);
            faceNumberListBox.Name = "faceNumberListBox";
            faceNumberListBox.Size = new Size(205, 23);
            faceNumberListBox.TabIndex = 1;
            // 
            // convertMakeupButton
            // 
            convertMakeupButton.AllowDrop = true;
            convertMakeupButton.Location = new Point(0, 52);
            convertMakeupButton.Name = "convertMakeupButton";
            convertMakeupButton.Size = new Size(622, 37);
            convertMakeupButton.TabIndex = 2;
            convertMakeupButton.Text = "Pick Makeup";
            convertMakeupButton.UseVisualStyleBackColor = true;
            convertMakeupButton.Click += convertMakeupButton_Click;
            convertMakeupButton.DragDrop += filePath_DragDrop;
            convertMakeupButton.DragEnter += filePath_DragEnter;
            // 
            // skipUnderlayCheckBox
            // 
            skipUnderlayCheckBox.AutoSize = true;
            skipUnderlayCheckBox.Location = new Point(4, 28);
            skipUnderlayCheckBox.Name = "skipUnderlayCheckBox";
            skipUnderlayCheckBox.Size = new Size(139, 19);
            skipUnderlayCheckBox.TabIndex = 3;
            skipUnderlayCheckBox.Text = "Skip Underlay Texture";
            skipUnderlayCheckBox.UseVisualStyleBackColor = true;
            // 
            // racialGender
            // 
            racialGender.FormattingEnabled = true;
            racialGender.Items.AddRange(new object[] { "Feminine", "Masculine" });
            racialGender.Location = new Point(421, 0);
            racialGender.Name = "racialGender";
            racialGender.Size = new Size(201, 23);
            racialGender.TabIndex = 4;
            // 
            // skipLipCorrection
            // 
            skipLipCorrection.AutoSize = true;
            skipLipCorrection.Location = new Point(210, 28);
            skipLipCorrection.Name = "skipLipCorrection";
            skipLipCorrection.Size = new Size(140, 19);
            skipLipCorrection.TabIndex = 5;
            skipLipCorrection.Text = "Skip Facial Correction";
            skipLipCorrection.UseVisualStyleBackColor = true;
            skipLipCorrection.CheckedChanged += skipLipCorrection_CheckedChanged;
            // 
            // textBox1
            // 
            textBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBox1.BorderStyle = BorderStyle.None;
            textBox1.Enabled = false;
            textBox1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBox1.ForeColor = SystemColors.InfoText;
            textBox1.HideSelection = false;
            textBox1.Location = new Point(4, 92);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(618, 137);
            textBox1.TabIndex = 6;
            textBox1.Text = "Select the makeup race, face, and gender and then drag and drop your makeups onto this window! Attempts to makes things Dawntrail compatible.";
            textBox1.TextAlign = HorizontalAlignment.Center;
            textBox1.TextChanged += textBox1_TextChanged;
            textBox1.DragDrop += filePath_DragDrop;
            textBox1.DragEnter += filePath_DragEnter;
            // 
            // textureIsNormalMap
            // 
            textureIsNormalMap.AutoSize = true;
            textureIsNormalMap.Location = new Point(421, 29);
            textureIsNormalMap.Name = "textureIsNormalMap";
            textureIsNormalMap.Size = new Size(145, 19);
            textureIsNormalMap.TabIndex = 7;
            textureIsNormalMap.Text = "Texture Is Normal Map";
            textureIsNormalMap.UseVisualStyleBackColor = true;
            // 
            // LegacyMakeupSalvager
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(626, 241);
            Controls.Add(textureIsNormalMap);
            Controls.Add(textBox1);
            Controls.Add(skipLipCorrection);
            Controls.Add(racialGender);
            Controls.Add(skipUnderlayCheckBox);
            Controls.Add(convertMakeupButton);
            Controls.Add(faceNumberListBox);
            Controls.Add(subRaceListBox);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "LegacyMakeupSalvager";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Legacy Makeup Salvager";
            Load += LegacyMakeupSalvager_Load;
            DragDrop += filePath_DragDrop;
            DragEnter += filePath_DragEnter;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox subRaceListBox;
        private ComboBox faceNumberListBox;
        private Button convertMakeupButton;
        private CheckBox skipUnderlayCheckBox;
        private ComboBox comboBox1;
        private ComboBox racialGender;
        private CheckBox skipLipCorrection;
        private TextBox textBox1;
        private CheckBox textureIsNormalMap;
    }
}