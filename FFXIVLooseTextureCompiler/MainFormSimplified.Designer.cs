namespace FFXIVLooseTextureCompiler {
    partial class MainFormSimplified {
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainFormSimplified));
            skin = new FFXIVVoicePackCreator.FilePicker();
            face = new FFXIVVoicePackCreator.FilePicker();
            eyes = new FFXIVVoicePackCreator.FilePicker();
            bodyType = new ComboBox();
            subRace = new ComboBox();
            faceType = new ComboBox();
            modNameTextBox = new TextBox();
            label1 = new Label();
            generateButton = new Button();
            advancedModeButton = new Button();
            previewButton = new Button();
            normalGeneration = new ComboBox();
            label2 = new Label();
            exportProgress = new ProgressBar();
            progressChecker = new System.Windows.Forms.Timer(components);
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            newToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            saveAsToolStripMenuItem = new ToolStripMenuItem();
            toolsToolStripMenuItem = new ToolStripMenuItem();
            convertPictureToEyeMultiToolStripMenuItem = new ToolStripMenuItem();
            modShareToolStripMenuItem = new ToolStripMenuItem();
            enableModShareToolStripMenuItem = new ToolStripMenuItem();
            creditsToolStripMenuItem = new ToolStripMenuItem();
            donateButton = new Button();
            discordButton = new Button();
            exportPanel = new Panel();
            exportLabel = new Label();
            legacyMakeupSalvagerToolStripMenuItem = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            exportPanel.SuspendLayout();
            SuspendLayout();
            // 
            // skin
            // 
            skin.BackColor = Color.LavenderBlush;
            skin.CurrentPath = null;
            skin.Filter = null;
            skin.Index = -1;
            skin.Location = new Point(4, 100);
            skin.Margin = new Padding(4, 3, 4, 3);
            skin.MinimumSize = new Size(300, 28);
            skin.Name = "skin";
            skin.Size = new Size(348, 28);
            skin.TabIndex = 0;
            skin.OnFileSelected += skin_OnFileSelected;
            // 
            // face
            // 
            face.BackColor = Color.LavenderBlush;
            face.CurrentPath = null;
            face.Filter = null;
            face.Index = -1;
            face.Location = new Point(4, 128);
            face.Margin = new Padding(4, 3, 4, 3);
            face.MinimumSize = new Size(300, 28);
            face.Name = "face";
            face.Size = new Size(348, 28);
            face.TabIndex = 1;
            face.OnFileSelected += face_OnFileSelected;
            // 
            // eyes
            // 
            eyes.BackColor = Color.Lavender;
            eyes.CurrentPath = null;
            eyes.Filter = null;
            eyes.Index = -1;
            eyes.Location = new Point(4, 156);
            eyes.Margin = new Padding(4, 3, 4, 3);
            eyes.MinimumSize = new Size(300, 28);
            eyes.Name = "eyes";
            eyes.Size = new Size(348, 28);
            eyes.TabIndex = 2;
            eyes.OnFileSelected += eyes_OnFileSelected;
            // 
            // bodyType
            // 
            bodyType.FormattingEnabled = true;
            bodyType.Items.AddRange(new object[] { "Bibo+", "Gen3", "TBSE/HRBODY", "Otopop" });
            bodyType.Location = new Point(4, 76);
            bodyType.Name = "bodyType";
            bodyType.Size = new Size(108, 23);
            bodyType.TabIndex = 3;
            bodyType.Text = "Bibo+";
            bodyType.SelectedIndexChanged += bodyType_SelectedIndexChanged;
            // 
            // subRace
            // 
            subRace.FormattingEnabled = true;
            subRace.Items.AddRange(new object[] { "Midlander", "Highlander", "Wildwood", "Duskwight", "Seeker", "Keeper", "Sea Wolf", "Hellsguard", "Plainsfolk", "Dunesfolk", "Raen", "Xaela", "Helions", "The Lost", "Rava", "Veena" });
            subRace.Location = new Point(116, 76);
            subRace.Name = "subRace";
            subRace.Size = new Size(132, 23);
            subRace.TabIndex = 4;
            subRace.Text = "Midlander";
            subRace.SelectedIndexChanged += subRace_SelectedIndexChanged;
            // 
            // faceType
            // 
            faceType.FormattingEnabled = true;
            faceType.Items.AddRange(new object[] { "Face 1", "Face 2", "Face 3", "Face 4", "Face 5", "Face 6", "Face 7", "Face 8" });
            faceType.Location = new Point(252, 76);
            faceType.Name = "faceType";
            faceType.Size = new Size(97, 23);
            faceType.TabIndex = 5;
            faceType.Text = "Face 1";
            faceType.SelectedIndexChanged += faceType_SelectedIndexChanged;
            // 
            // modNameTextBox
            // 
            modNameTextBox.Location = new Point(4, 48);
            modNameTextBox.Name = "modNameTextBox";
            modNameTextBox.Size = new Size(344, 23);
            modNameTextBox.TabIndex = 6;
            modNameTextBox.TextChanged += modNameTextBox_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(4, 28);
            label1.Name = "label1";
            label1.Size = new Size(67, 15);
            label1.TabIndex = 7;
            label1.Text = "Mod Name";
            // 
            // generateButton
            // 
            generateButton.Location = new Point(172, 216);
            generateButton.Name = "generateButton";
            generateButton.Size = new Size(176, 23);
            generateButton.TabIndex = 8;
            generateButton.Text = "Finished (To finish mod)";
            generateButton.UseVisualStyleBackColor = true;
            generateButton.Click += generateButton_Click;
            // 
            // advancedModeButton
            // 
            advancedModeButton.BackColor = Color.IndianRed;
            advancedModeButton.ForeColor = Color.White;
            advancedModeButton.Location = new Point(232, 24);
            advancedModeButton.Name = "advancedModeButton";
            advancedModeButton.Size = new Size(119, 23);
            advancedModeButton.TabIndex = 9;
            advancedModeButton.Text = "Advanced Mode";
            advancedModeButton.UseVisualStyleBackColor = false;
            advancedModeButton.Click += advancedModeButton_Click;
            // 
            // previewButton
            // 
            previewButton.Location = new Point(4, 216);
            previewButton.Name = "previewButton";
            previewButton.Size = new Size(168, 23);
            previewButton.TabIndex = 10;
            previewButton.Text = "Preview (For quick edits)";
            previewButton.UseVisualStyleBackColor = true;
            previewButton.Click += previewButton_Click;
            // 
            // normalGeneration
            // 
            normalGeneration.FormattingEnabled = true;
            normalGeneration.Items.AddRange(new object[] { "No Bumps On Skin", "Bumps On Skin", "Inverted Bumps On Skin" });
            normalGeneration.Location = new Point(82, 188);
            normalGeneration.Name = "normalGeneration";
            normalGeneration.Size = new Size(265, 23);
            normalGeneration.TabIndex = 11;
            normalGeneration.Text = "No Bumps On Skin";
            normalGeneration.SelectedIndexChanged += normalGeneration_SelectedIndexChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(8, 192);
            label2.Name = "label2";
            label2.Size = new Size(69, 15);
            label2.TabIndex = 12;
            label2.Text = "Skin Bumps";
            // 
            // exportProgress
            // 
            exportProgress.Location = new Point(4, 216);
            exportProgress.Name = "exportProgress";
            exportProgress.Size = new Size(344, 24);
            exportProgress.TabIndex = 13;
            exportProgress.Visible = false;
            // 
            // progressChecker
            // 
            progressChecker.Enabled = true;
            progressChecker.Interval = 10;
            progressChecker.Tick += progressChecker_Tick;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, toolsToolStripMenuItem, modShareToolStripMenuItem, creditsToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(351, 24);
            menuStrip1.TabIndex = 14;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { newToolStripMenuItem, openToolStripMenuItem, saveToolStripMenuItem, saveAsToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            newToolStripMenuItem.Name = "newToolStripMenuItem";
            newToolStripMenuItem.Size = new Size(114, 22);
            newToolStripMenuItem.Text = "New";
            newToolStripMenuItem.Click += newToolStripMenuItem_Click;
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(114, 22);
            openToolStripMenuItem.Text = "Open";
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new Size(114, 22);
            saveToolStripMenuItem.Text = "Save";
            saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
            // 
            // saveAsToolStripMenuItem
            // 
            saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            saveAsToolStripMenuItem.Size = new Size(114, 22);
            saveAsToolStripMenuItem.Text = "Save As";
            saveAsToolStripMenuItem.Click += saveAsToolStripMenuItem_Click;
            // 
            // toolsToolStripMenuItem
            // 
            toolsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { convertPictureToEyeMultiToolStripMenuItem, legacyMakeupSalvagerToolStripMenuItem });
            toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            toolsToolStripMenuItem.Size = new Size(46, 20);
            toolsToolStripMenuItem.Text = "Tools";
            // 
            // convertPictureToEyeMultiToolStripMenuItem
            // 
            convertPictureToEyeMultiToolStripMenuItem.Name = "convertPictureToEyeMultiToolStripMenuItem";
            convertPictureToEyeMultiToolStripMenuItem.Size = new Size(233, 22);
            convertPictureToEyeMultiToolStripMenuItem.Text = "Convert Picture To Eye Texture";
            convertPictureToEyeMultiToolStripMenuItem.Click += convertPictureToEyeMultiToolStripMenuItem_Click;
            // 
            // modShareToolStripMenuItem
            // 
            modShareToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { enableModShareToolStripMenuItem });
            modShareToolStripMenuItem.Name = "modShareToolStripMenuItem";
            modShareToolStripMenuItem.Size = new Size(76, 20);
            modShareToolStripMenuItem.Text = "Mod Share";
            // 
            // enableModShareToolStripMenuItem
            // 
            enableModShareToolStripMenuItem.Name = "enableModShareToolStripMenuItem";
            enableModShareToolStripMenuItem.Size = new Size(169, 22);
            enableModShareToolStripMenuItem.Text = "Enable Mod Share";
            enableModShareToolStripMenuItem.Click += enableModShareToolStripMenuItem_Click;
            // 
            // creditsToolStripMenuItem
            // 
            creditsToolStripMenuItem.Name = "creditsToolStripMenuItem";
            creditsToolStripMenuItem.Size = new Size(56, 20);
            creditsToolStripMenuItem.Text = "Credits";
            creditsToolStripMenuItem.Click += creditsToolStripMenuItem_Click;
            // 
            // donateButton
            // 
            donateButton.BackColor = Color.LightCoral;
            donateButton.ForeColor = Color.White;
            donateButton.Location = new Point(292, 0);
            donateButton.Name = "donateButton";
            donateButton.Size = new Size(59, 23);
            donateButton.TabIndex = 15;
            donateButton.Text = "Donate";
            donateButton.UseVisualStyleBackColor = false;
            donateButton.Click += donateButton_Click;
            // 
            // discordButton
            // 
            discordButton.BackColor = Color.SlateBlue;
            discordButton.ForeColor = Color.White;
            discordButton.Location = new Point(232, 0);
            discordButton.Name = "discordButton";
            discordButton.Size = new Size(59, 23);
            discordButton.TabIndex = 16;
            discordButton.Text = "Discord";
            discordButton.UseVisualStyleBackColor = false;
            discordButton.Click += discordButton_Click;
            // 
            // exportPanel
            // 
            exportPanel.BackColor = SystemColors.AppWorkspace;
            exportPanel.Controls.Add(exportLabel);
            exportPanel.Location = new Point(0, 0);
            exportPanel.Name = "exportPanel";
            exportPanel.Size = new Size(352, 212);
            exportPanel.TabIndex = 17;
            exportPanel.Visible = false;
            // 
            // exportLabel
            // 
            exportLabel.AutoSize = true;
            exportLabel.Font = new Font("Segoe UI", 26.25F, FontStyle.Bold);
            exportLabel.ForeColor = SystemColors.ButtonHighlight;
            exportLabel.Location = new Point(72, 76);
            exportLabel.Name = "exportLabel";
            exportLabel.Size = new Size(210, 47);
            exportLabel.TabIndex = 0;
            exportLabel.Text = "Exporting...";
            // 
            // legacyMakeupSalvagerToolStripMenuItem
            // 
            legacyMakeupSalvagerToolStripMenuItem.Name = "legacyMakeupSalvagerToolStripMenuItem";
            legacyMakeupSalvagerToolStripMenuItem.Size = new Size(233, 22);
            legacyMakeupSalvagerToolStripMenuItem.Text = "Legacy Makeup Salvager";
            legacyMakeupSalvagerToolStripMenuItem.Click += legacyMakeupSalvagerToolStripMenuItem_Click;
            // 
            // MainFormSimplified
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(351, 241);
            Controls.Add(discordButton);
            Controls.Add(donateButton);
            Controls.Add(label2);
            Controls.Add(normalGeneration);
            Controls.Add(previewButton);
            Controls.Add(advancedModeButton);
            Controls.Add(generateButton);
            Controls.Add(label1);
            Controls.Add(modNameTextBox);
            Controls.Add(faceType);
            Controls.Add(subRace);
            Controls.Add(bodyType);
            Controls.Add(eyes);
            Controls.Add(face);
            Controls.Add(skin);
            Controls.Add(menuStrip1);
            Controls.Add(exportPanel);
            Controls.Add(exportProgress);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            Name = "MainFormSimplified";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Loose Texture Compiler (Simplified)";
            FormClosing += MainFormSimplified_FormClosing;
            Load += MainFormSimplified_Load;
            Shown += MainWindowSimplified_Load;
            VisibleChanged += MainWindowSimplified_Load;
            KeyDown += MainWindow_KeyDown;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            exportPanel.ResumeLayout(false);
            exportPanel.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private FFXIVVoicePackCreator.FilePicker skin;
        private FFXIVVoicePackCreator.FilePicker face;
        private FFXIVVoicePackCreator.FilePicker eyes;
        private ComboBox bodyType;
        private ComboBox subRace;
        private ComboBox faceType;
        private TextBox modNameTextBox;
        private Label label1;
        private Button generateButton;
        private Button advancedModeButton;
        private Button previewButton;
        private ComboBox normalGeneration;
        private Label label2;
        private ProgressBar exportProgress;
        private System.Windows.Forms.Timer progressChecker;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem creditsToolStripMenuItem;
        private ToolStripMenuItem modShareToolStripMenuItem;
        private ToolStripMenuItem enableModShareToolStripMenuItem;
        private ToolStripMenuItem newToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private Button donateButton;
        private Button discordButton;
        private Panel exportPanel;
        private Label exportLabel;
        private ToolStripMenuItem toolsToolStripMenuItem;
        private ToolStripMenuItem convertPictureToEyeMultiToolStripMenuItem;
        private ToolStripMenuItem legacyMakeupSalvagerToolStripMenuItem;

        public ComboBox BodyType { get => bodyType; set => bodyType = value; }
        public ComboBox SubRace { get => subRace; set => subRace = value; }
        public ComboBox FaceType { get => faceType; set => faceType = value; }
        public ComboBox NormalGeneration { get => normalGeneration; set => normalGeneration = value; }
    }
}