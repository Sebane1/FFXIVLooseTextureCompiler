namespace FFXIVLooseTextureCompiler {
    partial class CustomPathDialog {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomPathDialog));
            textureSetNameTextBox = new TextBox();
            label1 = new Label();
            baseTextureLabel = new Label();
            internalBasePathTextBox = new TextBox();
            normalLabel = new Label();
            internalNormalPathTextBox = new TextBox();
            multiLabel = new Label();
            internalMaskPathTextbox = new TextBox();
            acceptChangesButton = new Button();
            button1 = new Button();
            label5 = new Label();
            groupNameTextBox = new TextBox();
            ignoreNormalsCheckbox = new CheckBox();
            ignoreMultiCheckbox = new CheckBox();
            label2 = new Label();
            normalCorrection = new TextBox();
            invertNormals = new CheckBox();
            groupChoiceType = new ComboBox();
            label3 = new Label();
            skinTypeLabel = new Label();
            skinTypeSelection = new ComboBox();
            label4 = new Label();
            internalMaterialPathTextBox = new TextBox();
            material = new FFXIVVoicePackCreator.FilePicker();
            usesAlternateTextures = new CheckBox();
            SuspendLayout();
            // 
            // textureSetNameTextBox
            // 
            textureSetNameTextBox.Location = new Point(113, 36);
            textureSetNameTextBox.Name = "textureSetNameTextBox";
            textureSetNameTextBox.Size = new Size(324, 23);
            textureSetNameTextBox.TabIndex = 0;
            textureSetNameTextBox.TextChanged += materialSetNameTextBox_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(4, 40);
            label1.Name = "label1";
            label1.Size = new Size(99, 15);
            label1.TabIndex = 1;
            label1.Text = "Texture Set Name";
            // 
            // baseTextureLabel
            // 
            baseTextureLabel.AutoSize = true;
            baseTextureLabel.Location = new Point(4, 68);
            baseTextureLabel.Name = "baseTextureLabel";
            baseTextureLabel.Size = new Size(74, 15);
            baseTextureLabel.TabIndex = 3;
            baseTextureLabel.Text = "Internal Base";
            // 
            // internalBasePathTextBox
            // 
            internalBasePathTextBox.Location = new Point(113, 64);
            internalBasePathTextBox.Name = "internalBasePathTextBox";
            internalBasePathTextBox.Size = new Size(324, 23);
            internalBasePathTextBox.TabIndex = 2;
            // 
            // normalLabel
            // 
            normalLabel.AutoSize = true;
            normalLabel.Location = new Point(4, 96);
            normalLabel.Name = "normalLabel";
            normalLabel.Size = new Size(90, 15);
            normalLabel.TabIndex = 5;
            normalLabel.Text = "Internal Normal";
            // 
            // internalNormalPathTextBox
            // 
            internalNormalPathTextBox.Location = new Point(113, 92);
            internalNormalPathTextBox.Name = "internalNormalPathTextBox";
            internalNormalPathTextBox.Size = new Size(324, 23);
            internalNormalPathTextBox.TabIndex = 4;
            // 
            // multiLabel
            // 
            multiLabel.AutoSize = true;
            multiLabel.Location = new Point(4, 124);
            multiLabel.Name = "multiLabel";
            multiLabel.Size = new Size(78, 15);
            multiLabel.TabIndex = 7;
            multiLabel.Text = "Internal Mask";
            // 
            // internalMaskPathTextbox
            // 
            internalMaskPathTextbox.Location = new Point(112, 120);
            internalMaskPathTextbox.Name = "internalMaskPathTextbox";
            internalMaskPathTextbox.Size = new Size(325, 23);
            internalMaskPathTextbox.TabIndex = 6;
            // 
            // acceptChangesButton
            // 
            acceptChangesButton.Location = new Point(264, 310);
            acceptChangesButton.Name = "acceptChangesButton";
            acceptChangesButton.Size = new Size(111, 23);
            acceptChangesButton.TabIndex = 8;
            acceptChangesButton.Text = "Confirm Changes";
            acceptChangesButton.UseVisualStyleBackColor = true;
            acceptChangesButton.Click += acceptChangesButton_Click;
            // 
            // button1
            // 
            button1.Location = new Point(376, 310);
            button1.Name = "button1";
            button1.Size = new Size(59, 23);
            button1.TabIndex = 9;
            button1.Text = "Cancel";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(4, 12);
            label5.Name = "label5";
            label5.Size = new Size(40, 15);
            label5.TabIndex = 11;
            label5.Text = "Group";
            // 
            // groupNameTextBox
            // 
            groupNameTextBox.Location = new Point(112, 8);
            groupNameTextBox.Name = "groupNameTextBox";
            groupNameTextBox.Size = new Size(324, 23);
            groupNameTextBox.TabIndex = 10;
            // 
            // ignoreNormalsCheckbox
            // 
            ignoreNormalsCheckbox.AutoSize = true;
            ignoreNormalsCheckbox.Location = new Point(265, 289);
            ignoreNormalsCheckbox.Name = "ignoreNormalsCheckbox";
            ignoreNormalsCheckbox.Size = new Size(150, 19);
            ignoreNormalsCheckbox.TabIndex = 12;
            ignoreNormalsCheckbox.Text = "Dont Generate Normals";
            ignoreNormalsCheckbox.UseVisualStyleBackColor = true;
            // 
            // ignoreMultiCheckbox
            // 
            ignoreMultiCheckbox.AutoSize = true;
            ignoreMultiCheckbox.Location = new Point(265, 264);
            ignoreMultiCheckbox.Name = "ignoreMultiCheckbox";
            ignoreMultiCheckbox.Size = new Size(133, 19);
            ignoreMultiCheckbox.TabIndex = 13;
            ignoreMultiCheckbox.Text = "Dont Generate Mask";
            ignoreMultiCheckbox.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(3, 181);
            label2.Name = "label2";
            label2.Size = new Size(106, 15);
            label2.TabIndex = 15;
            label2.Text = "Normal Correction";
            // 
            // normalCorrection
            // 
            normalCorrection.Location = new Point(112, 177);
            normalCorrection.Name = "normalCorrection";
            normalCorrection.Size = new Size(324, 23);
            normalCorrection.TabIndex = 14;
            // 
            // invertNormals
            // 
            invertNormals.AutoSize = true;
            invertNormals.Location = new Point(113, 289);
            invertNormals.Name = "invertNormals";
            invertNormals.Size = new Size(104, 19);
            invertNormals.TabIndex = 16;
            invertNormals.Text = "Invert Normals";
            invertNormals.UseVisualStyleBackColor = true;
            invertNormals.CheckedChanged += invertNormals_CheckedChanged;
            // 
            // groupChoiceType
            // 
            groupChoiceType.FormattingEnabled = true;
            groupChoiceType.Items.AddRange(new object[] { "Use Global Setting" });
            groupChoiceType.Location = new Point(112, 310);
            groupChoiceType.Name = "groupChoiceType";
            groupChoiceType.Size = new Size(148, 23);
            groupChoiceType.TabIndex = 17;
            groupChoiceType.SelectedIndexChanged += groupChoiceType_SelectedIndexChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(4, 314);
            label3.Name = "label3";
            label3.Size = new Size(107, 15);
            label3.TabIndex = 18;
            label3.Text = "Group Choice Type";
            // 
            // skinTypeLabel
            // 
            skinTypeLabel.AutoSize = true;
            skinTypeLabel.Location = new Point(4, 209);
            skinTypeLabel.Name = "skinTypeLabel";
            skinTypeLabel.Size = new Size(106, 15);
            skinTypeLabel.TabIndex = 19;
            skinTypeLabel.Text = "Skin Underlay Type";
            // 
            // skinTypeSelection
            // 
            skinTypeSelection.FormattingEnabled = true;
            skinTypeSelection.Location = new Point(112, 205);
            skinTypeSelection.Name = "skinTypeSelection";
            skinTypeSelection.Size = new Size(324, 23);
            skinTypeSelection.TabIndex = 20;
            skinTypeSelection.SelectedIndexChanged += skinTypeSelection_SelectedIndexChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(5, 152);
            label4.Name = "label4";
            label4.Size = new Size(93, 15);
            label4.TabIndex = 22;
            label4.Text = "Internal Material";
            // 
            // internalMaterialPathTextBox
            // 
            internalMaterialPathTextBox.Location = new Point(112, 148);
            internalMaterialPathTextBox.Name = "internalMaterialPathTextBox";
            internalMaterialPathTextBox.Size = new Size(326, 23);
            internalMaterialPathTextBox.TabIndex = 21;
            // 
            // material
            // 
            material.CurrentPath = null;
            material.Filter = null;
            material.Index = -1;
            material.IsMaterial = true;
            material.Location = new Point(34, 234);
            material.Margin = new Padding(4, 3, 4, 3);
            material.MinimumSize = new Size(300, 28);
            material.Name = "material";
            material.Size = new Size(402, 28);
            material.TabIndex = 23;
            // 
            // usesAlternateTextures
            // 
            usesAlternateTextures.AutoSize = true;
            usesAlternateTextures.Location = new Point(113, 264);
            usesAlternateTextures.Name = "usesAlternateTextures";
            usesAlternateTextures.Size = new Size(147, 19);
            usesAlternateTextures.TabIndex = 24;
            usesAlternateTextures.Text = "Uses Alternate Textures";
            usesAlternateTextures.UseVisualStyleBackColor = true;
            // 
            // CustomPathDialog
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(442, 338);
            Controls.Add(usesAlternateTextures);
            Controls.Add(material);
            Controls.Add(label4);
            Controls.Add(internalMaterialPathTextBox);
            Controls.Add(skinTypeSelection);
            Controls.Add(skinTypeLabel);
            Controls.Add(label3);
            Controls.Add(groupChoiceType);
            Controls.Add(label2);
            Controls.Add(normalCorrection);
            Controls.Add(ignoreMultiCheckbox);
            Controls.Add(ignoreNormalsCheckbox);
            Controls.Add(label5);
            Controls.Add(groupNameTextBox);
            Controls.Add(button1);
            Controls.Add(acceptChangesButton);
            Controls.Add(multiLabel);
            Controls.Add(internalMaskPathTextbox);
            Controls.Add(normalLabel);
            Controls.Add(internalNormalPathTextBox);
            Controls.Add(baseTextureLabel);
            Controls.Add(internalBasePathTextBox);
            Controls.Add(label1);
            Controls.Add(textureSetNameTextBox);
            Controls.Add(invertNormals);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "CustomPathDialog";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Custom Path";
            Load += CustomPathDialog_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textureSetNameTextBox;
        private Label label1;
        private Label baseTextureLabel;
        private TextBox internalBasePathTextBox;
        private Label normalLabel;
        private TextBox internalNormalPathTextBox;
        private Label multiLabel;
        private TextBox internalMaskPathTextbox;
        private Button acceptChangesButton;
        private Button button1;
        private Label label5;
        private TextBox groupNameTextBox;
        private CheckBox ignoreNormalsCheckbox;
        private CheckBox ignoreMultiCheckbox;
        private Label label2;
        private TextBox normalCorrection;
        private CheckBox invertNormals;
        private ComboBox groupChoiceType;
        private Label label3;
        private Label skinTypeLabel;
        private ComboBox skinTypeSelection;
        private Label label4;
        private TextBox internalMaterialPathTextBox;
        private FFXIVVoicePackCreator.FilePicker material;
        private CheckBox usesAlternateTextures;
    }
}