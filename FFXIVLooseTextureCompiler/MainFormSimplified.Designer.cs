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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainFormSimplified));
            this.skin = new FFXIVVoicePackCreator.FilePicker();
            this.face = new FFXIVVoicePackCreator.FilePicker();
            this.eyes = new FFXIVVoicePackCreator.FilePicker();
            this.bodyType = new System.Windows.Forms.ComboBox();
            this.subRace = new System.Windows.Forms.ComboBox();
            this.faceType = new System.Windows.Forms.ComboBox();
            this.modNameTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.generateButton = new System.Windows.Forms.Button();
            this.advancedModeButton = new System.Windows.Forms.Button();
            this.previewButton = new System.Windows.Forms.Button();
            this.normalGeneration = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.exportProgress = new System.Windows.Forms.ProgressBar();
            this.progressChecker = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modShareToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enableModShareToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.creditsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.donateButton = new System.Windows.Forms.Button();
            this.discordButton = new System.Windows.Forms.Button();
            this.exportPanel = new System.Windows.Forms.Panel();
            this.exportLabel = new System.Windows.Forms.Label();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.convertPictureToEyeMultiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.exportPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // skin
            // 
            this.skin.BackColor = System.Drawing.Color.LavenderBlush;
            this.skin.CurrentPath = null;
            this.skin.Filter = null;
            this.skin.Index = -1;
            this.skin.Location = new System.Drawing.Point(4, 100);
            this.skin.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.skin.MinimumSize = new System.Drawing.Size(300, 28);
            this.skin.Name = "skin";
            this.skin.Size = new System.Drawing.Size(348, 28);
            this.skin.TabIndex = 0;
            this.skin.OnFileSelected += new System.EventHandler(this.skin_OnFileSelected);
            // 
            // face
            // 
            this.face.BackColor = System.Drawing.Color.LavenderBlush;
            this.face.CurrentPath = null;
            this.face.Filter = null;
            this.face.Index = -1;
            this.face.Location = new System.Drawing.Point(4, 128);
            this.face.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.face.MinimumSize = new System.Drawing.Size(300, 28);
            this.face.Name = "face";
            this.face.Size = new System.Drawing.Size(348, 28);
            this.face.TabIndex = 1;
            this.face.OnFileSelected += new System.EventHandler(this.face_OnFileSelected);
            // 
            // eyes
            // 
            this.eyes.BackColor = System.Drawing.Color.Lavender;
            this.eyes.CurrentPath = null;
            this.eyes.Filter = null;
            this.eyes.Index = -1;
            this.eyes.Location = new System.Drawing.Point(4, 156);
            this.eyes.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.eyes.MinimumSize = new System.Drawing.Size(300, 28);
            this.eyes.Name = "eyes";
            this.eyes.Size = new System.Drawing.Size(348, 28);
            this.eyes.TabIndex = 2;
            this.eyes.OnFileSelected += new System.EventHandler(this.eyes_OnFileSelected);
            // 
            // bodyType
            // 
            this.bodyType.FormattingEnabled = true;
            this.bodyType.Items.AddRange(new object[] {
            "Bibo+",
            "Gen3",
            "TBSE/HRBODY",
            "Otopop"});
            this.bodyType.Location = new System.Drawing.Point(4, 76);
            this.bodyType.Name = "bodyType";
            this.bodyType.Size = new System.Drawing.Size(108, 23);
            this.bodyType.TabIndex = 3;
            this.bodyType.Text = "Bibo+";
            this.bodyType.SelectedIndexChanged += new System.EventHandler(this.bodyType_SelectedIndexChanged);
            // 
            // subRace
            // 
            this.subRace.FormattingEnabled = true;
            this.subRace.Items.AddRange(new object[] {
            "Midlander",
            "Highlander",
            "Wildwood",
            "Duskwight",
            "Seeker",
            "Keeper",
            "Sea Wolf",
            "Hellsguard",
            "Plainsfolk",
            "Dunesfolk",
            "Raen",
            "Xaela",
            "Helions",
            "The Lost",
            "Rava",
            "Veena"});
            this.subRace.Location = new System.Drawing.Point(116, 76);
            this.subRace.Name = "subRace";
            this.subRace.Size = new System.Drawing.Size(132, 23);
            this.subRace.TabIndex = 4;
            this.subRace.Text = "Midlander";
            this.subRace.SelectedIndexChanged += new System.EventHandler(this.subRace_SelectedIndexChanged);
            // 
            // faceType
            // 
            this.faceType.FormattingEnabled = true;
            this.faceType.Items.AddRange(new object[] {
            "Face 1",
            "Face 2",
            "Face 3",
            "Face 4",
            "Face 5",
            "Face 6",
            "Face 7",
            "Face 8"});
            this.faceType.Location = new System.Drawing.Point(252, 76);
            this.faceType.Name = "faceType";
            this.faceType.Size = new System.Drawing.Size(97, 23);
            this.faceType.TabIndex = 5;
            this.faceType.Text = "Face 1";
            this.faceType.SelectedIndexChanged += new System.EventHandler(this.faceType_SelectedIndexChanged);
            // 
            // modNameTextBox
            // 
            this.modNameTextBox.Location = new System.Drawing.Point(4, 48);
            this.modNameTextBox.Name = "modNameTextBox";
            this.modNameTextBox.Size = new System.Drawing.Size(344, 23);
            this.modNameTextBox.TabIndex = 6;
            this.modNameTextBox.TextChanged += new System.EventHandler(this.modNameTextBox_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 7;
            this.label1.Text = "Mod Name";
            // 
            // generateButton
            // 
            this.generateButton.Location = new System.Drawing.Point(172, 216);
            this.generateButton.Name = "generateButton";
            this.generateButton.Size = new System.Drawing.Size(176, 23);
            this.generateButton.TabIndex = 8;
            this.generateButton.Text = "Finalize (To finish mod)";
            this.generateButton.UseVisualStyleBackColor = true;
            this.generateButton.Click += new System.EventHandler(this.generateButton_Click);
            // 
            // advancedModeButton
            // 
            this.advancedModeButton.BackColor = System.Drawing.Color.IndianRed;
            this.advancedModeButton.ForeColor = System.Drawing.Color.White;
            this.advancedModeButton.Location = new System.Drawing.Point(232, 24);
            this.advancedModeButton.Name = "advancedModeButton";
            this.advancedModeButton.Size = new System.Drawing.Size(119, 23);
            this.advancedModeButton.TabIndex = 9;
            this.advancedModeButton.Text = "Advanced Mode";
            this.advancedModeButton.UseVisualStyleBackColor = false;
            this.advancedModeButton.Click += new System.EventHandler(this.advancedModeButton_Click);
            // 
            // previewButton
            // 
            this.previewButton.Location = new System.Drawing.Point(4, 216);
            this.previewButton.Name = "previewButton";
            this.previewButton.Size = new System.Drawing.Size(168, 23);
            this.previewButton.TabIndex = 10;
            this.previewButton.Text = "Preview (For quick edits)";
            this.previewButton.UseVisualStyleBackColor = true;
            this.previewButton.Click += new System.EventHandler(this.previewButton_Click);
            // 
            // normalGeneration
            // 
            this.normalGeneration.FormattingEnabled = true;
            this.normalGeneration.Items.AddRange(new object[] {
            "No Bumps On Skin",
            "Bumps On Skin",
            "Inverted Bumps On Skin"});
            this.normalGeneration.Location = new System.Drawing.Point(82, 188);
            this.normalGeneration.Name = "normalGeneration";
            this.normalGeneration.Size = new System.Drawing.Size(265, 23);
            this.normalGeneration.TabIndex = 11;
            this.normalGeneration.Text = "No Bumps On Skin";
            this.normalGeneration.SelectedIndexChanged += new System.EventHandler(this.normalGeneration_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 192);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 15);
            this.label2.TabIndex = 12;
            this.label2.Text = "Skin Bumps";
            // 
            // exportProgress
            // 
            this.exportProgress.Location = new System.Drawing.Point(4, 216);
            this.exportProgress.Name = "exportProgress";
            this.exportProgress.Size = new System.Drawing.Size(344, 24);
            this.exportProgress.TabIndex = 13;
            this.exportProgress.Visible = false;
            // 
            // progressChecker
            // 
            this.progressChecker.Enabled = true;
            this.progressChecker.Interval = 10;
            this.progressChecker.Tick += new System.EventHandler(this.progressChecker_Tick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.modShareToolStripMenuItem,
            this.creditsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(351, 24);
            this.menuStrip1.TabIndex = 14;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.saveAsToolStripMenuItem.Text = "Save As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // modShareToolStripMenuItem
            // 
            this.modShareToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.enableModShareToolStripMenuItem});
            this.modShareToolStripMenuItem.Name = "modShareToolStripMenuItem";
            this.modShareToolStripMenuItem.Size = new System.Drawing.Size(76, 20);
            this.modShareToolStripMenuItem.Text = "Mod Share";
            // 
            // enableModShareToolStripMenuItem
            // 
            this.enableModShareToolStripMenuItem.Name = "enableModShareToolStripMenuItem";
            this.enableModShareToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.enableModShareToolStripMenuItem.Text = "Enable Mod Share";
            this.enableModShareToolStripMenuItem.Click += new System.EventHandler(this.enableModShareToolStripMenuItem_Click);
            // 
            // creditsToolStripMenuItem
            // 
            this.creditsToolStripMenuItem.Name = "creditsToolStripMenuItem";
            this.creditsToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
            this.creditsToolStripMenuItem.Text = "Credits";
            this.creditsToolStripMenuItem.Click += new System.EventHandler(this.creditsToolStripMenuItem_Click);
            // 
            // donateButton
            // 
            this.donateButton.BackColor = System.Drawing.Color.LightCoral;
            this.donateButton.ForeColor = System.Drawing.Color.White;
            this.donateButton.Location = new System.Drawing.Point(292, 0);
            this.donateButton.Name = "donateButton";
            this.donateButton.Size = new System.Drawing.Size(59, 23);
            this.donateButton.TabIndex = 15;
            this.donateButton.Text = "Donate";
            this.donateButton.UseVisualStyleBackColor = false;
            this.donateButton.Click += new System.EventHandler(this.donateButton_Click);
            // 
            // discordButton
            // 
            this.discordButton.BackColor = System.Drawing.Color.SlateBlue;
            this.discordButton.ForeColor = System.Drawing.Color.White;
            this.discordButton.Location = new System.Drawing.Point(232, 0);
            this.discordButton.Name = "discordButton";
            this.discordButton.Size = new System.Drawing.Size(59, 23);
            this.discordButton.TabIndex = 16;
            this.discordButton.Text = "Discord";
            this.discordButton.UseVisualStyleBackColor = false;
            this.discordButton.Click += new System.EventHandler(this.discordButton_Click);
            // 
            // exportPanel
            // 
            this.exportPanel.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.exportPanel.Controls.Add(this.exportLabel);
            this.exportPanel.Location = new System.Drawing.Point(0, 0);
            this.exportPanel.Name = "exportPanel";
            this.exportPanel.Size = new System.Drawing.Size(352, 212);
            this.exportPanel.TabIndex = 17;
            this.exportPanel.Visible = false;
            // 
            // exportLabel
            // 
            this.exportLabel.AutoSize = true;
            this.exportLabel.Font = new System.Drawing.Font("Segoe UI", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.exportLabel.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.exportLabel.Location = new System.Drawing.Point(72, 76);
            this.exportLabel.Name = "exportLabel";
            this.exportLabel.Size = new System.Drawing.Size(210, 47);
            this.exportLabel.TabIndex = 0;
            this.exportLabel.Text = "Exporting...";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.convertPictureToEyeMultiToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // convertPictureToEyeMultiToolStripMenuItem
            // 
            this.convertPictureToEyeMultiToolStripMenuItem.Name = "convertPictureToEyeMultiToolStripMenuItem";
            this.convertPictureToEyeMultiToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.convertPictureToEyeMultiToolStripMenuItem.Text = "Convert Picture To Eye Texture";
            this.convertPictureToEyeMultiToolStripMenuItem.Click += new System.EventHandler(this.convertPictureToEyeMultiToolStripMenuItem_Click);
            // 
            // MainFormSimplified
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(351, 241);
            this.Controls.Add(this.discordButton);
            this.Controls.Add(this.donateButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.normalGeneration);
            this.Controls.Add(this.previewButton);
            this.Controls.Add(this.advancedModeButton);
            this.Controls.Add(this.generateButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.modNameTextBox);
            this.Controls.Add(this.faceType);
            this.Controls.Add(this.subRace);
            this.Controls.Add(this.bodyType);
            this.Controls.Add(this.eyes);
            this.Controls.Add(this.face);
            this.Controls.Add(this.skin);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.exportPanel);
            this.Controls.Add(this.exportProgress);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainFormSimplified";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Loose Texture Compiler (Simplified)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFormSimplified_FormClosing);
            this.Load += new System.EventHandler(this.MainFormSimplified_Load);
            this.Shown += new System.EventHandler(this.MainWindowSimplified_Load);
            this.VisibleChanged += new System.EventHandler(this.MainWindowSimplified_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainWindow_KeyDown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.exportPanel.ResumeLayout(false);
            this.exportPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

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

        public ComboBox BodyType { get => bodyType; set => bodyType = value; }
        public ComboBox SubRace { get => subRace; set => subRace = value; }
        public ComboBox FaceType { get => faceType; set => faceType = value; }
        public ComboBox NormalGeneration { get => normalGeneration; set => normalGeneration = value; }
    }
}