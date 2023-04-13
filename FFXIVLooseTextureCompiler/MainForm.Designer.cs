namespace FFXIVLooseTextureCompiler {
    partial class MainWindow {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Timer autoGenerateTImer;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.genderListBody = new System.Windows.Forms.ComboBox();
            this.raceList = new System.Windows.Forms.ComboBox();
            this.tailList = new System.Windows.Forms.ComboBox();
            this.baseBodyList = new System.Windows.Forms.ComboBox();
            this.generateButton = new System.Windows.Forms.Button();
            this.uniqueAuRa = new System.Windows.Forms.CheckBox();
            this.multi = new FFXIVVoicePackCreator.FilePicker();
            this.normal = new FFXIVVoicePackCreator.FilePicker();
            this.diffuse = new FFXIVVoicePackCreator.FilePicker();
            this.asymCheckbox = new System.Windows.Forms.CheckBox();
            this.facePart = new System.Windows.Forms.ComboBox();
            this.faceType = new System.Windows.Forms.ComboBox();
            this.subRaceList = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.modDescriptionTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.modWebsiteTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.modAuthorTextBox = new System.Windows.Forms.TextBox();
            this.nameLabel = new System.Windows.Forms.Label();
            this.modNameTextBox = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.templatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importCustomTemplateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findAndBulkReplaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.convertStandaloneTextureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.biboToGen3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.biboToGen2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gen3ToBiboToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gen3ToGen2ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.gen2ToGen3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gen2ToBiboToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.otopopToVanillaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vanillaToOtopopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.devToolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textureToLTCTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pNGToLTCTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optimizePNGToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.convertLTCTToPNGToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bulkTexViewerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bulkImageToTexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.diffuseMergerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.multiCreatorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageToRGBChannelsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xNormalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitImageToRGBAndAlphaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mergeRGBAndAlphaImagesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.extractAtramentumLuminisGlowMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changePenumbraPathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modShareToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enableModshareToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sendCurrentModToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.creditsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.howToGetTexturesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.howDoIUseThisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.howDoIMakeStuffBumpyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.howDoIMakeStuffGlowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.canIMakeMyBiboOrGen3BodyWorkOnAnotherBodyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.canICustomizeTheGroupsThisToolExporrtsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.canIReplaceABunchOfStuffAtOnceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.whatIsModshareAndCanIQuicklySendAModToSomebodyElseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.donateButton = new System.Windows.Forms.Button();
            this.textureList = new System.Windows.Forms.ListBox();
            this.materialListContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editPathsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.omniExportModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveUpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveDownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bulkReplaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addBodyButton = new System.Windows.Forms.Button();
            this.addFaceButton = new System.Windows.Forms.Button();
            this.currentEditLabel = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.removeSelection = new System.Windows.Forms.Button();
            this.clearList = new System.Windows.Forms.Button();
            this.addCustomPathButton = new System.Windows.Forms.Button();
            this.moveUpButton = new System.Windows.Forms.Button();
            this.moveDownButton = new System.Windows.Forms.Button();
            this.ffxivRefreshTimer = new System.Windows.Forms.Timer(this.components);
            this.generationCooldown = new System.Windows.Forms.Timer(this.components);
            this.generationType = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.exportProgress = new System.Windows.Forms.ProgressBar();
            this.bakeNormals = new System.Windows.Forms.CheckBox();
            this.generateMultiCheckBox = new System.Windows.Forms.CheckBox();
            this.mask = new FFXIVVoicePackCreator.FilePicker();
            this.discordButton = new System.Windows.Forms.Button();
            this.faceExtra = new System.Windows.Forms.ComboBox();
            this.glow = new FFXIVVoicePackCreator.FilePicker();
            this.finalizeButton = new System.Windows.Forms.Button();
            this.auraFaceScalesDropdown = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.exportLabel = new System.Windows.Forms.Label();
            this.exportPanel = new System.Windows.Forms.Panel();
            this.listenForFiles = new System.ComponentModel.BackgroundWorker();
            this.modVersionTextBox = new System.Windows.Forms.TextBox();
            this.ipBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.processGeneration = new System.ComponentModel.BackgroundWorker();
            this.whatAreTemplatesAndHowDoIUseThemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            autoGenerateTImer = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1.SuspendLayout();
            this.materialListContextMenu.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.exportPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // autoGenerateTImer
            // 
            autoGenerateTImer.Interval = 200;
            autoGenerateTImer.Tick += new System.EventHandler(this.autoGenerateTImer_Tick);
            // 
            // genderListBody
            // 
            this.genderListBody.FormattingEnabled = true;
            this.genderListBody.Items.AddRange(new object[] {
            "Masculine",
            "Feminine"});
            this.genderListBody.Location = new System.Drawing.Point(148, 112);
            this.genderListBody.Name = "genderListBody";
            this.genderListBody.Size = new System.Drawing.Size(80, 23);
            this.genderListBody.TabIndex = 1;
            this.genderListBody.Text = "Masculine";
           
            // 
            // raceList
            // 
            this.raceList.FormattingEnabled = true;
            this.raceList.Items.AddRange(new object[] {
            "Midlander",
            "Highlander",
            "Elezen",
            "Miqo\'te",
            "Roegadyn",
            "Lalafell",
            "Raen",
            "Xaela",
            "Hrothgar",
            "Viera"});
            this.raceList.Location = new System.Drawing.Point(228, 112);
            this.raceList.Name = "raceList";
            this.raceList.Size = new System.Drawing.Size(84, 23);
            this.raceList.TabIndex = 2;
            this.raceList.Text = "Highlander";
            this.raceList.SelectedIndexChanged += new System.EventHandler(this.raceList_SelectedIndexChanged);
            // 
            // tailList
            // 
            this.tailList.Enabled = false;
            this.tailList.FormattingEnabled = true;
            this.tailList.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8"});
            this.tailList.Location = new System.Drawing.Point(312, 112);
            this.tailList.Name = "tailList";
            this.tailList.Size = new System.Drawing.Size(32, 23);
            this.tailList.TabIndex = 3;
            this.tailList.Text = "8";
            // 
            // baseBodyList
            // 
            this.baseBodyList.FormattingEnabled = true;
            this.baseBodyList.Items.AddRange(new object[] {
            "Vanilla and Gen2",
            "BIBO+",
            "EVE",
            "Gen3 and T&F3",
            "SCALES+",
            "TBSE and HRBODY",
            "TAIL",
            "Otopop"});
            this.baseBodyList.Location = new System.Drawing.Point(4, 112);
            this.baseBodyList.Name = "baseBodyList";
            this.baseBodyList.Size = new System.Drawing.Size(144, 23);
            this.baseBodyList.TabIndex = 4;
            this.baseBodyList.Text = "Gen3 and T&F3";
            this.baseBodyList.SelectedIndexChanged += new System.EventHandler(this.baseBodyList_SelectedIndexChanged);
            // 
            // generateButton
            // 
            this.generateButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.generateButton.Location = new System.Drawing.Point(400, 608);
            this.generateButton.Name = "generateButton";
            this.generateButton.Size = new System.Drawing.Size(64, 24);
            this.generateButton.TabIndex = 7;
            this.generateButton.Text = "Generate";
            this.generateButton.UseVisualStyleBackColor = true;
            this.generateButton.Click += new System.EventHandler(this.generateButton_Click);
            // 
            // uniqueAuRa
            // 
            this.uniqueAuRa.AutoSize = true;
            this.uniqueAuRa.Location = new System.Drawing.Point(348, 8);
            this.uniqueAuRa.Name = "uniqueAuRa";
            this.uniqueAuRa.Size = new System.Drawing.Size(98, 19);
            this.uniqueAuRa.TabIndex = 20;
            this.uniqueAuRa.Text = "Unique Au Ra";
            this.uniqueAuRa.UseVisualStyleBackColor = true;
            // 
            // multi
            // 
            this.multi.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.multi.CurrentPath = null;
            this.multi.Enabled = false;
            this.multi.Filter = "Texture File|*.png;*.dds;*.bmp;**.tex;";
            this.multi.Index = -1;
            this.multi.Location = new System.Drawing.Point(4, 512);
            this.multi.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.multi.MinimumSize = new System.Drawing.Size(300, 28);
            this.multi.Name = "multi";
            this.multi.Size = new System.Drawing.Size(528, 28);
            this.multi.TabIndex = 19;
            this.multi.OnFileSelected += new System.EventHandler(this.multi_OnFileSelected);
            this.multi.Enter += new System.EventHandler(this.multi_Enter);
            this.multi.Leave += new System.EventHandler(this.multi_Leave);
            // 
            // normal
            // 
            this.normal.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.normal.CurrentPath = null;
            this.normal.Enabled = false;
            this.normal.Filter = "Texture File|*.png;*.dds;*.bmp;**.tex;";
            this.normal.Index = -1;
            this.normal.Location = new System.Drawing.Point(4, 480);
            this.normal.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.normal.MinimumSize = new System.Drawing.Size(300, 28);
            this.normal.Name = "normal";
            this.normal.Size = new System.Drawing.Size(528, 28);
            this.normal.TabIndex = 18;
            this.normal.OnFileSelected += new System.EventHandler(this.multi_OnFileSelected);
          
            this.normal.Enter += new System.EventHandler(this.multi_Enter);
            this.normal.Leave += new System.EventHandler(this.multi_Leave);
            // 
            // diffuse
            // 
            this.diffuse.BackColor = System.Drawing.Color.LavenderBlush;
            this.diffuse.CurrentPath = null;
            this.diffuse.Enabled = false;
            this.diffuse.Filter = "Texture File|*.png;*.dds;*.bmp;**.tex;";
            this.diffuse.Index = -1;
            this.diffuse.Location = new System.Drawing.Point(4, 448);
            this.diffuse.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.diffuse.MinimumSize = new System.Drawing.Size(300, 28);
            this.diffuse.Name = "diffuse";
            this.diffuse.Size = new System.Drawing.Size(528, 28);
            this.diffuse.TabIndex = 17;
            this.diffuse.OnFileSelected += new System.EventHandler(this.multi_OnFileSelected);
            this.diffuse.Enter += new System.EventHandler(this.multi_Enter);
            this.diffuse.Leave += new System.EventHandler(this.multi_Leave);
            // 
            // asymCheckbox
            // 
            this.asymCheckbox.AutoSize = true;
            this.asymCheckbox.Location = new System.Drawing.Point(392, 8);
            this.asymCheckbox.Name = "asymCheckbox";
            this.asymCheckbox.Size = new System.Drawing.Size(56, 19);
            this.asymCheckbox.TabIndex = 23;
            this.asymCheckbox.Text = "Asym";
            this.asymCheckbox.UseVisualStyleBackColor = true;
            // 
            // facePart
            // 
            this.facePart.FormattingEnabled = true;
            this.facePart.Items.AddRange(new object[] {
            "Face",
            "Eyebrows",
            "Eyes",
            "Ears",
            "Face Paint",
            "Hair",
            "Face B",
            "Etc B"});
            this.facePart.Location = new System.Drawing.Point(148, 140);
            this.facePart.Name = "facePart";
            this.facePart.Size = new System.Drawing.Size(80, 23);
            this.facePart.TabIndex = 4;
            this.facePart.Text = "Eyebrows";
            this.facePart.SelectedIndexChanged += new System.EventHandler(this.facePart_SelectedIndexChanged);
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
            "Face 8",
            "Face 9"});
            this.faceType.Location = new System.Drawing.Point(88, 140);
            this.faceType.Name = "faceType";
            this.faceType.Size = new System.Drawing.Size(60, 23);
            this.faceType.TabIndex = 3;
            this.faceType.Text = "Face 4";
            // 
            // subRaceList
            // 
            this.subRaceList.FormattingEnabled = true;
            this.subRaceList.Items.AddRange(new object[] {
            "Midlander",
            "Highlander",
            "Wildwood",
            "Duskwight",
            "Seeker",
            "Keeper",
            "Sea Wolves",
            "Hellsgaurd",
            "Plainsfolk",
            "Dunesfolk",
            "Raen",
            "Xaela",
            "Helions",
            "The Lost",
            "Rava",
            "Veena"});
            this.subRaceList.Location = new System.Drawing.Point(4, 140);
            this.subRaceList.Name = "subRaceList";
            this.subRaceList.Size = new System.Drawing.Size(84, 23);
            this.subRaceList.TabIndex = 2;
            this.subRaceList.Text = "Sea Wolves";
            this.subRaceList.SelectedIndexChanged += new System.EventHandler(this.subRaceList_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 88);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 15);
            this.label4.TabIndex = 23;
            this.label4.Text = "Description";
            // 
            // modDescriptionTextBox
            // 
            this.modDescriptionTextBox.Location = new System.Drawing.Point(96, 84);
            this.modDescriptionTextBox.Name = "modDescriptionTextBox";
            this.modDescriptionTextBox.Size = new System.Drawing.Size(436, 23);
            this.modDescriptionTextBox.TabIndex = 22;
            this.modDescriptionTextBox.Text = "Exported by FFXIV Loose Texture Compiler";
            this.modDescriptionTextBox.TextChanged += new System.EventHandler(this.modDescriptionTextBox_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(256, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 15);
            this.label3.TabIndex = 21;
            this.label3.Text = "Version";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(256, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 15);
            this.label2.TabIndex = 16;
            this.label2.Text = "Website";
            // 
            // modWebsiteTextBox
            // 
            this.modWebsiteTextBox.Location = new System.Drawing.Point(328, 56);
            this.modWebsiteTextBox.Name = "modWebsiteTextBox";
            this.modWebsiteTextBox.Size = new System.Drawing.Size(204, 23);
            this.modWebsiteTextBox.TabIndex = 15;
            this.modWebsiteTextBox.Text = "https://github.com/Sebane1/FFXIVLooseTextureCompiler";
            this.modWebsiteTextBox.TextChanged += new System.EventHandler(this.modDescriptionTextBox_TextChanged);
            this.modWebsiteTextBox.Leave += new System.EventHandler(this.modWebsiteTextBox_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 15);
            this.label1.TabIndex = 14;
            this.label1.Text = "Author";
            // 
            // modAuthorTextBox
            // 
            this.modAuthorTextBox.Location = new System.Drawing.Point(96, 56);
            this.modAuthorTextBox.Name = "modAuthorTextBox";
            this.modAuthorTextBox.Size = new System.Drawing.Size(148, 23);
            this.modAuthorTextBox.TabIndex = 13;
            this.modAuthorTextBox.Text = "FFXIV Loose Texture Compiler";
            this.modAuthorTextBox.TextChanged += new System.EventHandler(this.modDescriptionTextBox_TextChanged);
            this.modAuthorTextBox.Leave += new System.EventHandler(this.modAuthorTextBox_Leave);
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(12, 32);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(39, 15);
            this.nameLabel.TabIndex = 12;
            this.nameLabel.Text = "Name";
            // 
            // modNameTextBox
            // 
            this.modNameTextBox.Location = new System.Drawing.Point(96, 28);
            this.modNameTextBox.Name = "modNameTextBox";
            this.modNameTextBox.Size = new System.Drawing.Size(148, 23);
            this.modNameTextBox.TabIndex = 11;
            this.modNameTextBox.TextChanged += new System.EventHandler(this.modDescriptionTextBox_TextChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.configToolStripMenuItem,
            this.modShareToolStripMenuItem,
            this.creditsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(537, 24);
            this.menuStrip1.TabIndex = 24;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.templatesToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.saveAsToolStripMenuItem.Text = "Save As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // templatesToolStripMenuItem
            // 
            this.templatesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importCustomTemplateToolStripMenuItem});
            this.templatesToolStripMenuItem.Name = "templatesToolStripMenuItem";
            this.templatesToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.templatesToolStripMenuItem.Text = "Templates";
            // 
            // importCustomTemplateToolStripMenuItem
            // 
            this.importCustomTemplateToolStripMenuItem.Name = "importCustomTemplateToolStripMenuItem";
            this.importCustomTemplateToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.importCustomTemplateToolStripMenuItem.Text = "Import Project As Template";
            this.importCustomTemplateToolStripMenuItem.Click += new System.EventHandler(this.importCustomTemplateToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.findAndBulkReplaceToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            
            // 
            // findAndBulkReplaceToolStripMenuItem
            // 
            this.findAndBulkReplaceToolStripMenuItem.Name = "findAndBulkReplaceToolStripMenuItem";
            this.findAndBulkReplaceToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.findAndBulkReplaceToolStripMenuItem.Text = "Find And Bulk Replace";
            this.findAndBulkReplaceToolStripMenuItem.Click += new System.EventHandler(this.findAndBulkReplaceToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.convertStandaloneTextureToolStripMenuItem,
            this.devToolsToolStripMenuItem,
            this.bulkTexViewerToolStripMenuItem,
            this.bulkImageToTexToolStripMenuItem,
            this.diffuseMergerToolStripMenuItem,
            this.multiCreatorToolStripMenuItem,
            this.imageToRGBChannelsToolStripMenuItem,
            this.xNormalToolStripMenuItem,
            this.splitImageToRGBAndAlphaToolStripMenuItem,
            this.mergeRGBAndAlphaImagesToolStripMenuItem,
            this.extractAtramentumLuminisGlowMapToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // convertStandaloneTextureToolStripMenuItem
            // 
            this.convertStandaloneTextureToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.biboToGen3ToolStripMenuItem,
            this.biboToGen2ToolStripMenuItem,
            this.gen3ToBiboToolStripMenuItem,
            this.gen3ToGen2ToolStripMenuItem1,
            this.gen2ToGen3ToolStripMenuItem,
            this.gen2ToBiboToolStripMenuItem,
            this.otopopToVanillaToolStripMenuItem,
            this.vanillaToOtopopToolStripMenuItem});
            this.convertStandaloneTextureToolStripMenuItem.Name = "convertStandaloneTextureToolStripMenuItem";
            this.convertStandaloneTextureToolStripMenuItem.Size = new System.Drawing.Size(299, 22);
            this.convertStandaloneTextureToolStripMenuItem.Text = "Convert Standalone Texture";
            // 
            // biboToGen3ToolStripMenuItem
            // 
            this.biboToGen3ToolStripMenuItem.Name = "biboToGen3ToolStripMenuItem";
            this.biboToGen3ToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.biboToGen3ToolStripMenuItem.Text = "Bibo+ to Gen3";
            this.biboToGen3ToolStripMenuItem.Click += new System.EventHandler(this.biboToGen3ToolStripMenuItem_Click);
            // 
            // biboToGen2ToolStripMenuItem
            // 
            this.biboToGen2ToolStripMenuItem.Name = "biboToGen2ToolStripMenuItem";
            this.biboToGen2ToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.biboToGen2ToolStripMenuItem.Text = "Bibo+ to Gen2";
            this.biboToGen2ToolStripMenuItem.Click += new System.EventHandler(this.biboToGen2ToolStripMenuItem_Click);
            // 
            // gen3ToBiboToolStripMenuItem
            // 
            this.gen3ToBiboToolStripMenuItem.Name = "gen3ToBiboToolStripMenuItem";
            this.gen3ToBiboToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.gen3ToBiboToolStripMenuItem.Text = "Gen3 to Bibo+";
            this.gen3ToBiboToolStripMenuItem.Click += new System.EventHandler(this.gen3ToBiboToolStripMenuItem_Click);
            // 
            // gen3ToGen2ToolStripMenuItem1
            // 
            this.gen3ToGen2ToolStripMenuItem1.Name = "gen3ToGen2ToolStripMenuItem1";
            this.gen3ToGen2ToolStripMenuItem1.Size = new System.Drawing.Size(166, 22);
            this.gen3ToGen2ToolStripMenuItem1.Text = "Gen3 to Gen2";
            this.gen3ToGen2ToolStripMenuItem1.Click += new System.EventHandler(this.gen3ToGen2ToolStripMenuItem_Click);
            // 
            // gen2ToGen3ToolStripMenuItem
            // 
            this.gen2ToGen3ToolStripMenuItem.Name = "gen2ToGen3ToolStripMenuItem";
            this.gen2ToGen3ToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.gen2ToGen3ToolStripMenuItem.Text = "Gen2 to Gen3";
            this.gen2ToGen3ToolStripMenuItem.Click += new System.EventHandler(this.gen2ToGen3ToolStripMenuItem_Click);
            // 
            // gen2ToBiboToolStripMenuItem
            // 
            this.gen2ToBiboToolStripMenuItem.Name = "gen2ToBiboToolStripMenuItem";
            this.gen2ToBiboToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.gen2ToBiboToolStripMenuItem.Text = "Gen2 to Bibo+";
            this.gen2ToBiboToolStripMenuItem.Click += new System.EventHandler(this.gen2ToBiboToolStripMenuItem_Click);
            // 
            // otopopToVanillaToolStripMenuItem
            // 
            this.otopopToVanillaToolStripMenuItem.Name = "otopopToVanillaToolStripMenuItem";
            this.otopopToVanillaToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.otopopToVanillaToolStripMenuItem.Text = "Otopop to Vanilla";
            this.otopopToVanillaToolStripMenuItem.Click += new System.EventHandler(this.otopopToVanillaToolStripMenuItem_Click);
            // 
            // vanillaToOtopopToolStripMenuItem
            // 
            this.vanillaToOtopopToolStripMenuItem.Name = "vanillaToOtopopToolStripMenuItem";
            this.vanillaToOtopopToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.vanillaToOtopopToolStripMenuItem.Text = "Vanilla to Otopop";
            this.vanillaToOtopopToolStripMenuItem.Click += new System.EventHandler(this.vanillaToOtopopToolStripMenuItem_Click);
            // 
            // devToolsToolStripMenuItem
            // 
            this.devToolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.textureToLTCTToolStripMenuItem,
            this.pNGToLTCTToolStripMenuItem,
            this.optimizePNGToolStripMenuItem,
            this.convertLTCTToPNGToolStripMenuItem});
            this.devToolsToolStripMenuItem.Name = "devToolsToolStripMenuItem";
            this.devToolsToolStripMenuItem.Size = new System.Drawing.Size(299, 22);
            this.devToolsToolStripMenuItem.Text = "Dev Tools";
            this.devToolsToolStripMenuItem.Visible = false;
            // 
            // textureToLTCTToolStripMenuItem
            // 
            this.textureToLTCTToolStripMenuItem.Name = "textureToLTCTToolStripMenuItem";
            this.textureToLTCTToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.textureToLTCTToolStripMenuItem.Text = "Texture To LTCT";
            this.textureToLTCTToolStripMenuItem.Click += new System.EventHandler(this.bulkConvertImagesToLTCTToolStripMenuItem_Click);
            // 
            // pNGToLTCTToolStripMenuItem
            // 
            this.pNGToLTCTToolStripMenuItem.Name = "pNGToLTCTToolStripMenuItem";
            this.pNGToLTCTToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.pNGToLTCTToolStripMenuItem.Text = "PNG To LTCT";
            this.pNGToLTCTToolStripMenuItem.Click += new System.EventHandler(this.convertPNGToLTCTToolStripMenuItem_Click);
            // 
            // optimizePNGToolStripMenuItem
            // 
            this.optimizePNGToolStripMenuItem.Name = "optimizePNGToolStripMenuItem";
            this.optimizePNGToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.optimizePNGToolStripMenuItem.Text = "Optimize PNG";
            this.optimizePNGToolStripMenuItem.Click += new System.EventHandler(this.optimizePNGToolStripMenuItem_Click);
            // 
            // convertLTCTToPNGToolStripMenuItem
            // 
            this.convertLTCTToPNGToolStripMenuItem.Name = "convertLTCTToPNGToolStripMenuItem";
            this.convertLTCTToPNGToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.convertLTCTToPNGToolStripMenuItem.Text = "Convert LTCT To PNG";
            this.convertLTCTToPNGToolStripMenuItem.Click += new System.EventHandler(this.bulkConvertLTCTToPNGToolStripMenuItem_Click);
            // 
            // bulkTexViewerToolStripMenuItem
            // 
            this.bulkTexViewerToolStripMenuItem.Name = "bulkTexViewerToolStripMenuItem";
            this.bulkTexViewerToolStripMenuItem.Size = new System.Drawing.Size(299, 22);
            this.bulkTexViewerToolStripMenuItem.Text = "Bulk Tex File Manager";
            this.bulkTexViewerToolStripMenuItem.Click += new System.EventHandler(this.bulkTexViewerToolStripMenuItem_Click);
            // 
            // bulkImageToTexToolStripMenuItem
            // 
            this.bulkImageToTexToolStripMenuItem.Name = "bulkImageToTexToolStripMenuItem";
            this.bulkImageToTexToolStripMenuItem.Size = new System.Drawing.Size(299, 22);
            this.bulkImageToTexToolStripMenuItem.Text = "Bulk Image To Tex";
            this.bulkImageToTexToolStripMenuItem.Click += new System.EventHandler(this.bulkImageToTexToolStripMenuItem_Click);
            // 
            // diffuseMergerToolStripMenuItem
            // 
            this.diffuseMergerToolStripMenuItem.Name = "diffuseMergerToolStripMenuItem";
            this.diffuseMergerToolStripMenuItem.Size = new System.Drawing.Size(299, 22);
            this.diffuseMergerToolStripMenuItem.Text = "Diffuse Merger";
            this.diffuseMergerToolStripMenuItem.Click += new System.EventHandler(this.diffuseMergerToolStripMenuItem_Click);
            // 
            // multiCreatorToolStripMenuItem
            // 
            this.multiCreatorToolStripMenuItem.Name = "multiCreatorToolStripMenuItem";
            this.multiCreatorToolStripMenuItem.Size = new System.Drawing.Size(299, 22);
            this.multiCreatorToolStripMenuItem.Text = "RGBA Merger";
            this.multiCreatorToolStripMenuItem.Click += new System.EventHandler(this.multiCreatorToolStripMenuItem_Click);
            // 
            // imageToRGBChannelsToolStripMenuItem
            // 
            this.imageToRGBChannelsToolStripMenuItem.Name = "imageToRGBChannelsToolStripMenuItem";
            this.imageToRGBChannelsToolStripMenuItem.Size = new System.Drawing.Size(299, 22);
            this.imageToRGBChannelsToolStripMenuItem.Text = "Split Image To RGBA Channels";
            this.imageToRGBChannelsToolStripMenuItem.Click += new System.EventHandler(this.imageToRGBChannelsToolStripMenuItem_Click);
            // 
            // xNormalToolStripMenuItem
            // 
            this.xNormalToolStripMenuItem.Name = "xNormalToolStripMenuItem";
            this.xNormalToolStripMenuItem.Size = new System.Drawing.Size(299, 22);
            this.xNormalToolStripMenuItem.Text = "XNormal";
            this.xNormalToolStripMenuItem.Click += new System.EventHandler(this.xNormalToolStripMenuItem_Click);
            // 
            // splitImageToRGBAndAlphaToolStripMenuItem
            // 
            this.splitImageToRGBAndAlphaToolStripMenuItem.Name = "splitImageToRGBAndAlphaToolStripMenuItem";
            this.splitImageToRGBAndAlphaToolStripMenuItem.Size = new System.Drawing.Size(299, 22);
            this.splitImageToRGBAndAlphaToolStripMenuItem.Text = "Split Image to RGB and Alpha";
            this.splitImageToRGBAndAlphaToolStripMenuItem.Click += new System.EventHandler(this.splitImageToRGBAndAlphaToolStripMenuItem_Click);
            // 
            // mergeRGBAndAlphaImagesToolStripMenuItem
            // 
            this.mergeRGBAndAlphaImagesToolStripMenuItem.Name = "mergeRGBAndAlphaImagesToolStripMenuItem";
            this.mergeRGBAndAlphaImagesToolStripMenuItem.Size = new System.Drawing.Size(299, 22);
            this.mergeRGBAndAlphaImagesToolStripMenuItem.Text = "Merge RGB and Alpha Images";
            this.mergeRGBAndAlphaImagesToolStripMenuItem.Click += new System.EventHandler(this.mergeRGBAndAlphaImagesToolStripMenuItem_Click);
            // 
            // extractAtramentumLuminisGlowMapToolStripMenuItem
            // 
            this.extractAtramentumLuminisGlowMapToolStripMenuItem.Name = "extractAtramentumLuminisGlowMapToolStripMenuItem";
            this.extractAtramentumLuminisGlowMapToolStripMenuItem.Size = new System.Drawing.Size(299, 22);
            this.extractAtramentumLuminisGlowMapToolStripMenuItem.Text = "Atramentum Luminis Diffuse To Glow Map";
            this.extractAtramentumLuminisGlowMapToolStripMenuItem.Click += new System.EventHandler(this.extractAtramentumLuminisGlowMapToolStripMenuItem_Click);
            // 
            // configToolStripMenuItem
            // 
            this.configToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.changePenumbraPathToolStripMenuItem});
            this.configToolStripMenuItem.Name = "configToolStripMenuItem";
            this.configToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
            this.configToolStripMenuItem.Text = "Config";
            // 
            // changePenumbraPathToolStripMenuItem
            // 
            this.changePenumbraPathToolStripMenuItem.Name = "changePenumbraPathToolStripMenuItem";
            this.changePenumbraPathToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.changePenumbraPathToolStripMenuItem.Text = "Change Penumbra Path";
            this.changePenumbraPathToolStripMenuItem.Click += new System.EventHandler(this.changePenumbraPathToolStripMenuItem_Click);
            // 
            // modShareToolStripMenuItem
            // 
            this.modShareToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.enableModshareToolStripMenuItem,
            this.sendCurrentModToolStripMenuItem});
            this.modShareToolStripMenuItem.Name = "modShareToolStripMenuItem";
            this.modShareToolStripMenuItem.Size = new System.Drawing.Size(76, 20);
            this.modShareToolStripMenuItem.Text = "Mod Share";
           
            // 
            // enableModshareToolStripMenuItem
            // 
            this.enableModshareToolStripMenuItem.Name = "enableModshareToolStripMenuItem";
            this.enableModshareToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.enableModshareToolStripMenuItem.Text = "Enable Modshare";
            this.enableModshareToolStripMenuItem.Click += new System.EventHandler(this.enableModshareToolStripMenuItem_Click);
            // 
            // sendCurrentModToolStripMenuItem
            // 
            this.sendCurrentModToolStripMenuItem.Enabled = false;
            this.sendCurrentModToolStripMenuItem.Name = "sendCurrentModToolStripMenuItem";
            this.sendCurrentModToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.sendCurrentModToolStripMenuItem.Text = "Send Current Mod";
            this.sendCurrentModToolStripMenuItem.Click += new System.EventHandler(this.sendCurrentModToolStripMenuItem_Click);
            // 
            // creditsToolStripMenuItem
            // 
            this.creditsToolStripMenuItem.Name = "creditsToolStripMenuItem";
            this.creditsToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
            this.creditsToolStripMenuItem.Text = "Credits";
            this.creditsToolStripMenuItem.Click += new System.EventHandler(this.creditsToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.howToGetTexturesToolStripMenuItem,
            this.howDoIUseThisToolStripMenuItem,
            this.howDoIMakeStuffBumpyToolStripMenuItem,
            this.howDoIMakeStuffGlowToolStripMenuItem,
            this.canIMakeMyBiboOrGen3BodyWorkOnAnotherBodyToolStripMenuItem,
            this.canICustomizeTheGroupsThisToolExporrtsToolStripMenuItem,
            this.canIReplaceABunchOfStuffAtOnceToolStripMenuItem,
            this.whatIsModshareAndCanIQuicklySendAModToSomebodyElseToolStripMenuItem,
            this.whatAreTemplatesAndHowDoIUseThemToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // howToGetTexturesToolStripMenuItem
            // 
            this.howToGetTexturesToolStripMenuItem.Name = "howToGetTexturesToolStripMenuItem";
            this.howToGetTexturesToolStripMenuItem.Size = new System.Drawing.Size(430, 22);
            this.howToGetTexturesToolStripMenuItem.Text = "How do I get textures?";
            this.howToGetTexturesToolStripMenuItem.Click += new System.EventHandler(this.howToGetTexturesToolStripMenuItem_Click);
            // 
            // howDoIUseThisToolStripMenuItem
            // 
            this.howDoIUseThisToolStripMenuItem.Name = "howDoIUseThisToolStripMenuItem";
            this.howDoIUseThisToolStripMenuItem.Size = new System.Drawing.Size(430, 22);
            this.howDoIUseThisToolStripMenuItem.Text = "How do I use this?";
            this.howDoIUseThisToolStripMenuItem.Click += new System.EventHandler(this.howDoIUseThisToolStripMenuItem_Click);
            // 
            // howDoIMakeStuffBumpyToolStripMenuItem
            // 
            this.howDoIMakeStuffBumpyToolStripMenuItem.Name = "howDoIMakeStuffBumpyToolStripMenuItem";
            this.howDoIMakeStuffBumpyToolStripMenuItem.Size = new System.Drawing.Size(430, 22);
            this.howDoIMakeStuffBumpyToolStripMenuItem.Text = "How do I make stuff bumpy?";
            this.howDoIMakeStuffBumpyToolStripMenuItem.Click += new System.EventHandler(this.howDoIMakeStuffBumpyToolStripMenuItem_Click);
            // 
            // howDoIMakeStuffGlowToolStripMenuItem
            // 
            this.howDoIMakeStuffGlowToolStripMenuItem.Name = "howDoIMakeStuffGlowToolStripMenuItem";
            this.howDoIMakeStuffGlowToolStripMenuItem.Size = new System.Drawing.Size(430, 22);
            this.howDoIMakeStuffGlowToolStripMenuItem.Text = "How do I make stuff glow?";
            this.howDoIMakeStuffGlowToolStripMenuItem.Click += new System.EventHandler(this.howDoIMakeStuffGlowToolStripMenuItem_Click);
            // 
            // canIMakeMyBiboOrGen3BodyWorkOnAnotherBodyToolStripMenuItem
            // 
            this.canIMakeMyBiboOrGen3BodyWorkOnAnotherBodyToolStripMenuItem.Name = "canIMakeMyBiboOrGen3BodyWorkOnAnotherBodyToolStripMenuItem";
            this.canIMakeMyBiboOrGen3BodyWorkOnAnotherBodyToolStripMenuItem.Size = new System.Drawing.Size(430, 22);
            this.canIMakeMyBiboOrGen3BodyWorkOnAnotherBodyToolStripMenuItem.Text = "Can I make my Bibo+ or Gen3 body texture work on another body?";
            this.canIMakeMyBiboOrGen3BodyWorkOnAnotherBodyToolStripMenuItem.Click += new System.EventHandler(this.canIMakeMyBiboOrGen3BodyWorkOnAnotherBodyToolStripMenuItem_Click);
            // 
            // canICustomizeTheGroupsThisToolExporrtsToolStripMenuItem
            // 
            this.canICustomizeTheGroupsThisToolExporrtsToolStripMenuItem.Name = "canICustomizeTheGroupsThisToolExporrtsToolStripMenuItem";
            this.canICustomizeTheGroupsThisToolExporrtsToolStripMenuItem.Size = new System.Drawing.Size(430, 22);
            this.canICustomizeTheGroupsThisToolExporrtsToolStripMenuItem.Text = "Can I customize the groups this tool exporrts";
            this.canICustomizeTheGroupsThisToolExporrtsToolStripMenuItem.Click += new System.EventHandler(this.canICustomizeTheGroupsThisToolExporrtsToolStripMenuItem_Click);
            // 
            // canIReplaceABunchOfStuffAtOnceToolStripMenuItem
            // 
            this.canIReplaceABunchOfStuffAtOnceToolStripMenuItem.Name = "canIReplaceABunchOfStuffAtOnceToolStripMenuItem";
            this.canIReplaceABunchOfStuffAtOnceToolStripMenuItem.Size = new System.Drawing.Size(430, 22);
            this.canIReplaceABunchOfStuffAtOnceToolStripMenuItem.Text = "Can I replace a bunch of stuff at once?";
            this.canIReplaceABunchOfStuffAtOnceToolStripMenuItem.Click += new System.EventHandler(this.canIReplaceABunchOfStuffAtOnceToolStripMenuItem_Click);
            // 
            // whatIsModshareAndCanIQuicklySendAModToSomebodyElseToolStripMenuItem
            // 
            this.whatIsModshareAndCanIQuicklySendAModToSomebodyElseToolStripMenuItem.Name = "whatIsModshareAndCanIQuicklySendAModToSomebodyElseToolStripMenuItem";
            this.whatIsModshareAndCanIQuicklySendAModToSomebodyElseToolStripMenuItem.Size = new System.Drawing.Size(430, 22);
            this.whatIsModshareAndCanIQuicklySendAModToSomebodyElseToolStripMenuItem.Text = "What is modshare, and can I quickly send a mod to somebody else?";
            this.whatIsModshareAndCanIQuicklySendAModToSomebodyElseToolStripMenuItem.Click += new System.EventHandler(this.whatIsModshareAndCanIQuicklySendAModToSomebodyElseToolStripMenuItem_Click);
            // 
            // donateButton
            // 
            this.donateButton.BackColor = System.Drawing.Color.IndianRed;
            this.donateButton.ForeColor = System.Drawing.Color.White;
            this.donateButton.Location = new System.Drawing.Point(464, 0);
            this.donateButton.Name = "donateButton";
            this.donateButton.Size = new System.Drawing.Size(75, 23);
            this.donateButton.TabIndex = 25;
            this.donateButton.Text = "Donate";
            this.donateButton.UseVisualStyleBackColor = false;
            this.donateButton.Click += new System.EventHandler(this.donateButton_Click);
            // 
            // textureList
            // 
            this.textureList.ContextMenuStrip = this.materialListContextMenu;
            this.textureList.FormattingEnabled = true;
            this.textureList.ItemHeight = 15;
            this.textureList.Location = new System.Drawing.Point(4, 204);
            this.textureList.Name = "textureList";
            this.textureList.Size = new System.Drawing.Size(528, 184);
            this.textureList.TabIndex = 26;
            this.textureList.SelectedIndexChanged += new System.EventHandler(this.materialList_SelectedIndexChanged);
            // 
            // materialListContextMenu
            // 
            this.materialListContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editPathsToolStripMenuItem,
            this.omniExportModeToolStripMenuItem,
            this.moveUpToolStripMenuItem,
            this.moveDownToolStripMenuItem,
            this.bulkReplaceToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.materialListContextMenu.Name = "materialListContextMenu";
            this.materialListContextMenu.Size = new System.Drawing.Size(236, 136);
            this.materialListContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.materialListContextMenu_Opening);
            // 
            // editPathsToolStripMenuItem
            // 
            this.editPathsToolStripMenuItem.Name = "editPathsToolStripMenuItem";
            this.editPathsToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.editPathsToolStripMenuItem.Text = "Edit Internal Texture Set Values";
            this.editPathsToolStripMenuItem.Click += new System.EventHandler(this.editPathsToolStripMenuItem_Click);
            // 
            // omniExportModeToolStripMenuItem
            // 
            this.omniExportModeToolStripMenuItem.Name = "omniExportModeToolStripMenuItem";
            this.omniExportModeToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.omniExportModeToolStripMenuItem.Text = "Enable Universal Compatibility";
            this.omniExportModeToolStripMenuItem.Click += new System.EventHandler(this.omniExportModeToolStripMenuItem_Click);
            // 
            // moveUpToolStripMenuItem
            // 
            this.moveUpToolStripMenuItem.Name = "moveUpToolStripMenuItem";
            this.moveUpToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.moveUpToolStripMenuItem.Text = "Move Up";
            this.moveUpToolStripMenuItem.Click += new System.EventHandler(this.moveUpButton_Click);
            // 
            // moveDownToolStripMenuItem
            // 
            this.moveDownToolStripMenuItem.Name = "moveDownToolStripMenuItem";
            this.moveDownToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.moveDownToolStripMenuItem.Text = "Move Down";
            this.moveDownToolStripMenuItem.Click += new System.EventHandler(this.moveDownButton_Click);
            // 
            // bulkReplaceToolStripMenuItem
            // 
            this.bulkReplaceToolStripMenuItem.Name = "bulkReplaceToolStripMenuItem";
            this.bulkReplaceToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.bulkReplaceToolStripMenuItem.Text = "Bulk Replace Values";
            this.bulkReplaceToolStripMenuItem.Click += new System.EventHandler(this.bulkReplaceToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.removeSelectionButton_Click);
            // 
            // addBodyButton
            // 
            this.addBodyButton.Location = new System.Drawing.Point(448, 112);
            this.addBodyButton.Name = "addBodyButton";
            this.addBodyButton.Size = new System.Drawing.Size(84, 23);
            this.addBodyButton.TabIndex = 27;
            this.addBodyButton.Text = "Add Body";
            this.addBodyButton.UseVisualStyleBackColor = true;
            this.addBodyButton.Click += new System.EventHandler(this.addBodyEditButton_Click);
            // 
            // addFaceButton
            // 
            this.addFaceButton.Location = new System.Drawing.Point(448, 140);
            this.addFaceButton.Name = "addFaceButton";
            this.addFaceButton.Size = new System.Drawing.Size(84, 23);
            this.addFaceButton.TabIndex = 28;
            this.addFaceButton.Text = "Add Face";
            this.addFaceButton.UseVisualStyleBackColor = true;
            this.addFaceButton.Click += new System.EventHandler(this.addFaceButton_Click);
            // 
            // currentEditLabel
            // 
            this.currentEditLabel.AutoSize = true;
            this.currentEditLabel.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.currentEditLabel.Location = new System.Drawing.Point(4, 412);
            this.currentEditLabel.Name = "currentEditLabel";
            this.currentEditLabel.Size = new System.Drawing.Size(447, 30);
            this.currentEditLabel.TabIndex = 29;
            this.currentEditLabel.Text = "Please select a texture set to start importing";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label6.Location = new System.Drawing.Point(4, 172);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(155, 30);
            this.label6.TabIndex = 30;
            this.label6.Text = "Texture set list";
            // 
            // removeSelection
            // 
            this.removeSelection.Location = new System.Drawing.Point(4, 388);
            this.removeSelection.Name = "removeSelection";
            this.removeSelection.Size = new System.Drawing.Size(112, 23);
            this.removeSelection.TabIndex = 31;
            this.removeSelection.Text = "Remove Selection From List";
            this.removeSelection.UseVisualStyleBackColor = true;
            this.removeSelection.Click += new System.EventHandler(this.removeSelectionButton_Click);
            // 
            // clearList
            // 
            this.clearList.Location = new System.Drawing.Point(116, 388);
            this.clearList.Name = "clearList";
            this.clearList.Size = new System.Drawing.Size(72, 23);
            this.clearList.TabIndex = 32;
            this.clearList.Text = "Clear List";
            this.clearList.UseVisualStyleBackColor = true;
            this.clearList.Click += new System.EventHandler(this.clearList_Click);
            // 
            // addCustomPathButton
            // 
            this.addCustomPathButton.Location = new System.Drawing.Point(448, 168);
            this.addCustomPathButton.Name = "addCustomPathButton";
            this.addCustomPathButton.Size = new System.Drawing.Size(84, 23);
            this.addCustomPathButton.TabIndex = 33;
            this.addCustomPathButton.Text = "Custom Path";
            this.addCustomPathButton.UseVisualStyleBackColor = true;
            this.addCustomPathButton.Click += new System.EventHandler(this.addCustomPathButton_Click);
            // 
            // moveUpButton
            // 
            this.moveUpButton.Location = new System.Drawing.Point(188, 388);
            this.moveUpButton.Name = "moveUpButton";
            this.moveUpButton.Size = new System.Drawing.Size(68, 23);
            this.moveUpButton.TabIndex = 34;
            this.moveUpButton.Text = "Move Up";
            this.moveUpButton.UseVisualStyleBackColor = true;
            this.moveUpButton.Click += new System.EventHandler(this.moveUpButton_Click);
            // 
            // moveDownButton
            // 
            this.moveDownButton.Location = new System.Drawing.Point(256, 388);
            this.moveDownButton.Name = "moveDownButton";
            this.moveDownButton.Size = new System.Drawing.Size(80, 23);
            this.moveDownButton.TabIndex = 35;
            this.moveDownButton.Text = "Move Down";
            this.moveDownButton.UseVisualStyleBackColor = true;
            this.moveDownButton.Click += new System.EventHandler(this.moveDownButton_Click);
            // 
            // ffxivRefreshTimer
            // 
            this.ffxivRefreshTimer.Enabled = true;
            this.ffxivRefreshTimer.Interval = 10000;
            this.ffxivRefreshTimer.Tick += new System.EventHandler(this.ffxivRefreshTimer_Tick);
            // 
            // generationCooldown
            // 
            this.generationCooldown.Interval = 1000;
            this.generationCooldown.Tick += new System.EventHandler(this.generationCooldown_Tick);
            // 
            // generationType
            // 
            this.generationType.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.generationType.FormattingEnabled = true;
            this.generationType.Items.AddRange(new object[] {
            "Detailed",
            "Simple"});
            this.generationType.Location = new System.Drawing.Point(80, 607);
            this.generationType.Name = "generationType";
            this.generationType.Size = new System.Drawing.Size(70, 23);
            this.generationType.TabIndex = 36;
            this.generationType.Text = "Verbose";
            this.generationType.SelectedIndexChanged += new System.EventHandler(this.generationType_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 611);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 15);
            this.label5.TabIndex = 37;
            this.label5.Text = "Choice Type";
            // 
            // exportProgress
            // 
            this.exportProgress.Location = new System.Drawing.Point(0, 604);
            this.exportProgress.Name = "exportProgress";
            this.exportProgress.Size = new System.Drawing.Size(536, 32);
            this.exportProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.exportProgress.TabIndex = 38;
            this.exportProgress.Visible = false;
            
            // 
            // bakeNormals
            // 
            this.bakeNormals.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.bakeNormals.AutoSize = true;
            this.bakeNormals.Location = new System.Drawing.Point(156, 611);
            this.bakeNormals.Name = "bakeNormals";
            this.bakeNormals.Size = new System.Drawing.Size(116, 19);
            this.bakeNormals.TabIndex = 39;
            this.bakeNormals.Text = "Generate Normal";
            this.bakeNormals.UseVisualStyleBackColor = true;
            this.bakeNormals.CheckedChanged += new System.EventHandler(this.bakeMissingNormalsCheckbox_CheckedChanged);
            // 
            // generateMultiCheckBox
            // 
            this.generateMultiCheckBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.generateMultiCheckBox.AutoSize = true;
            this.generateMultiCheckBox.Location = new System.Drawing.Point(276, 611);
            this.generateMultiCheckBox.Name = "generateMultiCheckBox";
            this.generateMultiCheckBox.Size = new System.Drawing.Size(104, 19);
            this.generateMultiCheckBox.TabIndex = 40;
            this.generateMultiCheckBox.Text = "Generate Multi";
            this.generateMultiCheckBox.UseVisualStyleBackColor = true;
            this.generateMultiCheckBox.CheckedChanged += new System.EventHandler(this.generateMultiCheckBox_CheckedChanged);
            // 
            // mask
            // 
            this.mask.CurrentPath = null;
            this.mask.Enabled = false;
            this.mask.Filter = "Texture File|*.png;*.dds;*.bmp;**.tex;";
            this.mask.Index = -1;
            this.mask.Location = new System.Drawing.Point(4, 576);
            this.mask.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.mask.MinimumSize = new System.Drawing.Size(300, 28);
            this.mask.Name = "mask";
            this.mask.Size = new System.Drawing.Size(528, 28);
            this.mask.TabIndex = 41;
            this.mask.OnFileSelected += new System.EventHandler(this.multi_OnFileSelected);
            this.mask.Enter += new System.EventHandler(this.multi_Enter);
            this.mask.Leave += new System.EventHandler(this.multi_Leave);
            // 
            // discordButton
            // 
            this.discordButton.BackColor = System.Drawing.Color.MediumSlateBlue;
            this.discordButton.ForeColor = System.Drawing.Color.White;
            this.discordButton.Location = new System.Drawing.Point(388, 0);
            this.discordButton.Name = "discordButton";
            this.discordButton.Size = new System.Drawing.Size(75, 23);
            this.discordButton.TabIndex = 42;
            this.discordButton.Text = "Discord";
            this.discordButton.UseVisualStyleBackColor = false;
            this.discordButton.Click += new System.EventHandler(this.discordButton_Click);
            // 
            // faceExtra
            // 
            this.faceExtra.Enabled = false;
            this.faceExtra.FormattingEnabled = true;
            this.faceExtra.Location = new System.Drawing.Point(228, 140);
            this.faceExtra.Name = "faceExtra";
            this.faceExtra.Size = new System.Drawing.Size(48, 23);
            this.faceExtra.TabIndex = 43;
            this.faceExtra.Text = "999";
            // 
            // glow
            // 
            this.glow.CurrentPath = null;
            this.glow.Enabled = false;
            this.glow.Filter = "Texture File|*.png;*.dds;*.bmp;**.tex;";
            this.glow.Index = -1;
            this.glow.Location = new System.Drawing.Point(4, 544);
            this.glow.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.glow.MinimumSize = new System.Drawing.Size(300, 28);
            this.glow.Name = "glow";
            this.glow.Size = new System.Drawing.Size(528, 28);
            this.glow.TabIndex = 45;
            this.glow.OnFileSelected += new System.EventHandler(this.multi_OnFileSelected);
            this.glow.Enter += new System.EventHandler(this.multi_Enter);
            this.glow.Leave += new System.EventHandler(this.multi_Leave);
            // 
            // finalizeButton
            // 
            this.finalizeButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.finalizeButton.Location = new System.Drawing.Point(468, 608);
            this.finalizeButton.Name = "finalizeButton";
            this.finalizeButton.Size = new System.Drawing.Size(64, 24);
            this.finalizeButton.TabIndex = 46;
            this.finalizeButton.Text = "Finalize";
            this.finalizeButton.UseVisualStyleBackColor = true;
            this.finalizeButton.Click += new System.EventHandler(this.finalizeButton_Click);
            // 
            // auraFaceScalesDropdown
            // 
            this.auraFaceScalesDropdown.FormattingEnabled = true;
            this.auraFaceScalesDropdown.Items.AddRange(new object[] {
            "Vanilla Scales",
            "Scaleless Vanilla",
            "Scaleless Varied"});
            this.auraFaceScalesDropdown.Location = new System.Drawing.Point(276, 140);
            this.auraFaceScalesDropdown.Name = "auraFaceScalesDropdown";
            this.auraFaceScalesDropdown.Size = new System.Drawing.Size(108, 23);
            this.auraFaceScalesDropdown.TabIndex = 48;
            this.auraFaceScalesDropdown.Text = "Vanilla Scales";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Lavender;
            this.panel1.Controls.Add(this.uniqueAuRa);
            this.panel1.Location = new System.Drawing.Point(0, 108);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(540, 30);
            this.panel1.TabIndex = 49;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Honeydew;
            this.panel2.Controls.Add(this.asymCheckbox);
            this.panel2.Location = new System.Drawing.Point(-4, 136);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(540, 30);
            this.panel2.TabIndex = 50;
            // 
            // exportLabel
            // 
            this.exportLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.exportLabel.BackColor = System.Drawing.SystemColors.GrayText;
            this.exportLabel.Font = new System.Drawing.Font("Segoe UI", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.exportLabel.ForeColor = System.Drawing.Color.Snow;
            this.exportLabel.Location = new System.Drawing.Point(0, 264);
            this.exportLabel.Name = "exportLabel";
            this.exportLabel.Size = new System.Drawing.Size(536, 65);
            this.exportLabel.TabIndex = 0;
            this.exportLabel.Text = "Exporting";
            this.exportLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // exportPanel
            // 
            this.exportPanel.BackColor = System.Drawing.SystemColors.GrayText;
            this.exportPanel.Controls.Add(this.exportLabel);
            this.exportPanel.Location = new System.Drawing.Point(0, 0);
            this.exportPanel.Name = "exportPanel";
            this.exportPanel.Size = new System.Drawing.Size(536, 608);
            this.exportPanel.TabIndex = 44;
            this.exportPanel.Visible = false;
            // 
            // listenForFiles
            // 
            this.listenForFiles.WorkerSupportsCancellation = true;
            this.listenForFiles.DoWork += new System.ComponentModel.DoWorkEventHandler(this.listenForFiles_DoWork);
            // 
            // modVersionTextBox
            // 
            this.modVersionTextBox.Location = new System.Drawing.Point(328, 28);
            this.modVersionTextBox.Name = "modVersionTextBox";
            this.modVersionTextBox.Size = new System.Drawing.Size(40, 23);
            this.modVersionTextBox.TabIndex = 20;
            this.modVersionTextBox.Text = "1.0.0";
            this.modVersionTextBox.TextChanged += new System.EventHandler(this.modDescriptionTextBox_TextChanged);
            // 
            // ipBox
            // 
            this.ipBox.Location = new System.Drawing.Point(444, 28);
            this.ipBox.Name = "ipBox";
            this.ipBox.Size = new System.Drawing.Size(88, 23);
            this.ipBox.TabIndex = 51;
            this.ipBox.Text = "0.0.0.0";
            this.ipBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ipBox_KeyUp);
            this.ipBox.Leave += new System.EventHandler(this.ipBox_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(376, 32);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(61, 15);
            this.label8.TabIndex = 52;
            this.label8.Text = "Remote IP";
            // 
            // processGeneration
            // 
            this.processGeneration.WorkerReportsProgress = true;
            this.processGeneration.WorkerSupportsCancellation = true;
            this.processGeneration.DoWork += new System.ComponentModel.DoWorkEventHandler(this.processGeneration_DoWork);
            this.processGeneration.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.processGeneration_ProgressChanged);
            this.processGeneration.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.processGeneration_RunWorkerCompleted);
            // 
            // whatAreTemplatesAndHowDoIUseThemToolStripMenuItem
            // 
            this.whatAreTemplatesAndHowDoIUseThemToolStripMenuItem.Name = "whatAreTemplatesAndHowDoIUseThemToolStripMenuItem";
            this.whatAreTemplatesAndHowDoIUseThemToolStripMenuItem.Size = new System.Drawing.Size(430, 22);
            this.whatAreTemplatesAndHowDoIUseThemToolStripMenuItem.Text = "What are templates, and how do I use them?";
            this.whatAreTemplatesAndHowDoIUseThemToolStripMenuItem.Click += new System.EventHandler(this.whatAreTemplatesAndHowDoIUseThemToolStripMenuItem_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(537, 636);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.ipBox);
            this.Controls.Add(this.addFaceButton);
            this.Controls.Add(this.auraFaceScalesDropdown);
            this.Controls.Add(this.finalizeButton);
            this.Controls.Add(this.glow);
            this.Controls.Add(this.faceExtra);
            this.Controls.Add(this.discordButton);
            this.Controls.Add(this.mask);
            this.Controls.Add(this.generateMultiCheckBox);
            this.Controls.Add(this.bakeNormals);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.generationType);
            this.Controls.Add(this.moveDownButton);
            this.Controls.Add(this.moveUpButton);
            this.Controls.Add(this.addCustomPathButton);
            this.Controls.Add(this.clearList);
            this.Controls.Add(this.removeSelection);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.currentEditLabel);
            this.Controls.Add(this.addBodyButton);
            this.Controls.Add(this.multi);
            this.Controls.Add(this.normal);
            this.Controls.Add(this.diffuse);
            this.Controls.Add(this.textureList);
            this.Controls.Add(this.facePart);
            this.Controls.Add(this.donateButton);
            this.Controls.Add(this.faceType);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.subRaceList);
            this.Controls.Add(this.generateButton);
            this.Controls.Add(this.genderListBody);
            this.Controls.Add(this.modDescriptionTextBox);
            this.Controls.Add(this.raceList);
            this.Controls.Add(this.tailList);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.baseBodyList);
            this.Controls.Add(this.modNameTextBox);
            this.Controls.Add(this.modVersionTextBox);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.modAuthorTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.modWebsiteTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.exportProgress);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.exportPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FFXIV Loose Texture Compiler";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.materialListContextMenu.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.exportPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ComboBox genderListBody;
        private ComboBox raceList;
        private ComboBox tailList;
        private ComboBox baseBodyList;
        private Button generateButton;
        private Label label1;
        private TextBox modAuthorTextBox;
        private Label nameLabel;
        private TextBox modNameTextBox;
        private Label label2;
        private TextBox modWebsiteTextBox;
        private FFXIVVoicePackCreator.FilePicker multi;
        private FFXIVVoicePackCreator.FilePicker normal;
        private FFXIVVoicePackCreator.FilePicker diffuse;
        private Label label4;
        private TextBox modDescriptionTextBox;
        private Label label3;
        private ComboBox facePart;
        private ComboBox faceType;
        private ComboBox subRaceList;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem configToolStripMenuItem;
        private ToolStripMenuItem changePenumbraPathToolStripMenuItem;
        private Button donateButton;
        private CheckBox asymCheckbox;
        private CheckBox uniqueAuRa;
        private ListBox textureList;
        private Button addBodyButton;
        private Button addFaceButton;
        private Label currentEditLabel;
        private Label label6;
        private Button removeSelection;
        private Button clearList;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem newToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private Button addCustomPathButton;
        private ContextMenuStrip materialListContextMenu;
        private ToolStripMenuItem editPathsToolStripMenuItem;
        private ToolStripMenuItem moveUpToolStripMenuItem;
        private ToolStripMenuItem moveDownToolStripMenuItem;
        private Button moveUpButton;
        private Button moveDownButton;
        private System.Windows.Forms.Timer ffxivRefreshTimer;
        private System.Windows.Forms.Timer generationCooldown;
        private ComboBox generationType;
        private Label label5;
        private ToolStripMenuItem toolsToolStripMenuItem;
        private ToolStripMenuItem bulkTexViewerToolStripMenuItem;
        private ProgressBar exportProgress;
        private CheckBox bakeNormals;
        private CheckBox generateMultiCheckBox;
        private ToolStripMenuItem bulkReplaceToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem findAndBulkReplaceToolStripMenuItem;
        private ToolStripMenuItem diffuseMergerToolStripMenuItem;
        private FFXIVVoicePackCreator.FilePicker mask;
        private Button discordButton;
        private ComboBox faceExtra;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem howToGetTexturesToolStripMenuItem;
        private System.Windows.Forms.Timer autoGenerateTImer;
        private Label label7;
        private FFXIVVoicePackCreator.FilePicker glow;
        private ToolStripMenuItem deleteToolStripMenuItem;
        private ToolStripMenuItem omniExportModeToolStripMenuItem;
        private Button finalizeButton;
        private ToolStripMenuItem xNormalToolStripMenuItem;
        private ToolStripMenuItem convertStandaloneTextureToolStripMenuItem;
        private ToolStripMenuItem biboToGen3ToolStripMenuItem;
        private ToolStripMenuItem biboToGen2ToolStripMenuItem;
        private ToolStripMenuItem gen3ToBiboToolStripMenuItem;
        private ToolStripMenuItem gen3ToGen2ToolStripMenuItem1;
        private ToolStripMenuItem gen2ToGen3ToolStripMenuItem;
        private ToolStripMenuItem gen2ToBiboToolStripMenuItem;
        private ToolStripMenuItem otopopToVanillaToolStripMenuItem;
        private ToolStripMenuItem vanillaToOtopopToolStripMenuItem;
        private ToolStripMenuItem howDoIUseThisToolStripMenuItem;
        private ToolStripMenuItem howDoIMakeStuffBumpyToolStripMenuItem;
        private ToolStripMenuItem howDoIMakeStuffGlowToolStripMenuItem;
        private ToolStripMenuItem canIMakeMyBiboOrGen3BodyWorkOnAnotherBodyToolStripMenuItem;
        private ToolStripMenuItem canICustomizeTheGroupsThisToolExporrtsToolStripMenuItem;
        private ToolStripMenuItem canIReplaceABunchOfStuffAtOnceToolStripMenuItem;
        private ToolStripMenuItem bulkImageToTexToolStripMenuItem;
        private ToolStripMenuItem extractAtramentumLuminisGlowMapToolStripMenuItem;
        private ComboBox auraFaceScalesDropdown;
        private ToolStripMenuItem creditsToolStripMenuItem;
        private Panel panel1;
        private Panel panel2;
        private Label exportLabel;
        private Panel exportPanel;
        private ToolStripMenuItem modShareToolStripMenuItem;
        private ToolStripMenuItem sendCurrentModToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker listenForFiles;
        private TextBox modVersionTextBox;
        private TextBox ipBox;
        private Label label8;
        private ToolStripMenuItem enableModshareToolStripMenuItem;
        private ToolStripMenuItem multiCreatorToolStripMenuItem;
        private ToolStripMenuItem splitImageToRGBAndAlphaToolStripMenuItem;
        private ToolStripMenuItem mergeRGBAndAlphaImagesToolStripMenuItem;
        private ToolStripMenuItem whatIsModshareAndCanIQuicklySendAModToSomebodyElseToolStripMenuItem;
        private ToolStripMenuItem imageToRGBChannelsToolStripMenuItem;
        private ToolStripMenuItem devToolsToolStripMenuItem;
        private ToolStripMenuItem textureToLTCTToolStripMenuItem;
        private ToolStripMenuItem pNGToLTCTToolStripMenuItem;
        private ToolStripMenuItem optimizePNGToolStripMenuItem;
        private ToolStripMenuItem convertLTCTToPNGToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker processGeneration;
        private ToolStripMenuItem templatesToolStripMenuItem;
        private ToolStripMenuItem importCustomTemplateToolStripMenuItem;
        private ToolStripMenuItem whatAreTemplatesAndHowDoIUseThemToolStripMenuItem;
    }
}