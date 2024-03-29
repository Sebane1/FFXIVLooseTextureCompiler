﻿namespace FFXIVLooseTextureCompiler {
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
            components = new System.ComponentModel.Container();
            System.Windows.Forms.Timer autoGenerateTImer;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            genderList = new ComboBox();
            raceList = new ComboBox();
            tailList = new ComboBox();
            baseBodyList = new ComboBox();
            generateButton = new Button();
            uniqueAuRa = new CheckBox();
            multi = new FFXIVVoicePackCreator.FilePicker();
            normal = new FFXIVVoicePackCreator.FilePicker();
            diffuse = new FFXIVVoicePackCreator.FilePicker();
            asymCheckbox = new CheckBox();
            facePart = new ComboBox();
            faceTypeList = new ComboBox();
            subRaceList = new ComboBox();
            label4 = new Label();
            modDescriptionTextBox = new TextBox();
            label3 = new Label();
            label2 = new Label();
            modWebsiteTextBox = new TextBox();
            label1 = new Label();
            modAuthorTextBox = new TextBox();
            nameLabel = new Label();
            modNameTextBox = new TextBox();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            newToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            saveAsToolStripMenuItem = new ToolStripMenuItem();
            templatesToolStripMenuItem = new ToolStripMenuItem();
            importCustomTemplateToolStripMenuItem = new ToolStripMenuItem();
            editToolStripMenuItem = new ToolStripMenuItem();
            findAndBulkReplaceToolStripMenuItem = new ToolStripMenuItem();
            toolsToolStripMenuItem = new ToolStripMenuItem();
            extractAtramentumLuminisGlowMapToolStripMenuItem = new ToolStripMenuItem();
            convertStandaloneTextureToolStripMenuItem = new ToolStripMenuItem();
            biboToGen3ToolStripMenuItem = new ToolStripMenuItem();
            biboToGen2ToolStripMenuItem = new ToolStripMenuItem();
            gen3ToBiboToolStripMenuItem = new ToolStripMenuItem();
            gen3ToGen2ToolStripMenuItem1 = new ToolStripMenuItem();
            gen2ToGen3ToolStripMenuItem = new ToolStripMenuItem();
            gen2ToBiboToolStripMenuItem = new ToolStripMenuItem();
            otopopToVanillaToolStripMenuItem = new ToolStripMenuItem();
            vanillaToOtopopToolStripMenuItem = new ToolStripMenuItem();
            eyeToolsToolStripMenuItem = new ToolStripMenuItem();
            convertImageToEyeMultiToolStripMenuItem = new ToolStripMenuItem();
            convertImagesToAsymEyeMapsToolStripMenuItem = new ToolStripMenuItem();
            convertFolderToEyeMapsToolStripMenuItem = new ToolStripMenuItem();
            multiMapToGrayscaleToolStripMenuItem = new ToolStripMenuItem();
            hairToolsToolStripMenuItem = new ToolStripMenuItem();
            hairDiffuseToFFXIVHairMapsToolStripMenuItem = new ToolStripMenuItem();
            clothingToolsToolStripMenuItem = new ToolStripMenuItem();
            convertDiffuseToNormalAndMultiToolStripMenuItem = new ToolStripMenuItem();
            colourChannelSplittingToolStripMenuItem = new ToolStripMenuItem();
            diffuseMergerToolStripMenuItem = new ToolStripMenuItem();
            multiCreatorToolStripMenuItem = new ToolStripMenuItem();
            mergeRGBAndAlphaImagesToolStripMenuItem = new ToolStripMenuItem();
            imageToRGBChannelsToolStripMenuItem = new ToolStripMenuItem();
            splitImageToRGBAndAlphaToolStripMenuItem = new ToolStripMenuItem();
            textureToBodyMultiToolStripMenuItem = new ToolStripMenuItem();
            textureToFaceMultiToolStripMenuItem = new ToolStripMenuItem();
            textureToAsymFaceMultiToolStripMenuItem = new ToolStripMenuItem();
            xNormalToolStripMenuItem = new ToolStripMenuItem();
            imageToTexConversionToolStripMenuItem = new ToolStripMenuItem();
            bulkTexViewerToolStripMenuItem = new ToolStripMenuItem();
            bulkImageToTexToolStripMenuItem = new ToolStripMenuItem();
            recursiveBulkImageToTexToolStripMenuItem = new ToolStripMenuItem();
            devToolsToolStripMenuItem = new ToolStripMenuItem();
            textureToLTCTToolStripMenuItem = new ToolStripMenuItem();
            pNGToLTCTToolStripMenuItem = new ToolStripMenuItem();
            convertLTCTToPNGToolStripMenuItem = new ToolStripMenuItem();
            configToolStripMenuItem = new ToolStripMenuItem();
            changePenumbraPathToolStripMenuItem = new ToolStripMenuItem();
            modShareToolStripMenuItem = new ToolStripMenuItem();
            enableModshareToolStripMenuItem = new ToolStripMenuItem();
            sendCurrentModToolStripMenuItem = new ToolStripMenuItem();
            creditsToolStripMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            howToGetTexturesToolStripMenuItem = new ToolStripMenuItem();
            howDoIUseThisToolStripMenuItem = new ToolStripMenuItem();
            howDoIMakeStuffBumpyToolStripMenuItem = new ToolStripMenuItem();
            howDoIMakeStuffGlowToolStripMenuItem = new ToolStripMenuItem();
            howDoIMakeEyesToolStripMenuItem = new ToolStripMenuItem();
            canIMakeMyBiboOrGen3BodyWorkOnAnotherBodyToolStripMenuItem = new ToolStripMenuItem();
            canICustomizeTheGroupsThisToolExporrtsToolStripMenuItem = new ToolStripMenuItem();
            canIReplaceABunchOfStuffAtOnceToolStripMenuItem = new ToolStripMenuItem();
            whatIsModshareAndCanIQuicklySendAModToSomebodyElseToolStripMenuItem = new ToolStripMenuItem();
            whatAreTemplatesAndHowDoIUseThemToolStripMenuItem = new ToolStripMenuItem();
            thisToolIsTooHardMakeItSimplerToolStripMenuItem = new ToolStripMenuItem();
            donateButton = new Button();
            textureList = new ListBox();
            materialListContextMenu = new ContextMenuStrip(components);
            editPathsToolStripMenuItem = new ToolStripMenuItem();
            omniExportModeToolStripMenuItem = new ToolStripMenuItem();
            moveUpToolStripMenuItem = new ToolStripMenuItem();
            moveDownToolStripMenuItem = new ToolStripMenuItem();
            bulkNameReplacement = new ToolStripMenuItem();
            bulkReplaceToolStripMenuItem = new ToolStripMenuItem();
            duplicateToolStripMenuItem = new ToolStripMenuItem();
            deleteToolStripMenuItem = new ToolStripMenuItem();
            addBodyButton = new Button();
            addFaceButton = new Button();
            currentEditLabel = new Label();
            label6 = new Label();
            removeSelection = new Button();
            clearList = new Button();
            addCustomPathButton = new Button();
            moveUpButton = new Button();
            moveDownButton = new Button();
            ffxivRefreshTimer = new System.Windows.Forms.Timer(components);
            generationCooldown = new System.Windows.Forms.Timer(components);
            generationType = new ComboBox();
            label5 = new Label();
            exportProgress = new ProgressBar();
            bakeNormals = new CheckBox();
            generateMultiCheckBox = new CheckBox();
            mask = new FFXIVVoicePackCreator.FilePicker();
            discordButton = new Button();
            faceExtraList = new ComboBox();
            glow = new FFXIVVoicePackCreator.FilePicker();
            finalizeButton = new Button();
            auraFaceScalesDropdown = new ComboBox();
            panel1 = new Panel();
            panel2 = new Panel();
            exportLabel = new Label();
            exportPanel = new Panel();
            listenForFiles = new System.ComponentModel.BackgroundWorker();
            modVersionTextBox = new TextBox();
            ipBox = new TextBox();
            label8 = new Label();
            processGeneration = new System.ComponentModel.BackgroundWorker();
            helperToolTip = new ToolTip(components);
            autoGenerateTImer = new System.Windows.Forms.Timer(components);
            menuStrip1.SuspendLayout();
            materialListContextMenu.SuspendLayout();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            exportPanel.SuspendLayout();
            SuspendLayout();
            // 
            // autoGenerateTImer
            // 
            autoGenerateTImer.Interval = 200;
            autoGenerateTImer.Tick += autoGenerateTImer_Tick;
            // 
            // genderList
            // 
            genderList.FormattingEnabled = true;
            genderList.Items.AddRange(new object[] { "Masculine", "Feminine" });
            genderList.Location = new Point(148, 112);
            genderList.Name = "genderList";
            genderList.Size = new Size(80, 23);
            genderList.TabIndex = 1;
            genderList.Text = "Masculine";
            // 
            // raceList
            // 
            raceList.FormattingEnabled = true;
            raceList.Items.AddRange(new object[] { "Midlander", "Highlander", "Elezen", "Miqo'te", "Roegadyn", "Lalafell", "Raen", "Xaela", "Hrothgar", "Viera" });
            raceList.Location = new Point(228, 112);
            raceList.Name = "raceList";
            raceList.Size = new Size(84, 23);
            raceList.TabIndex = 2;
            raceList.Text = "Highlander";
            raceList.SelectedIndexChanged += raceList_SelectedIndexChanged;
            // 
            // tailList
            // 
            tailList.Enabled = false;
            tailList.FormattingEnabled = true;
            tailList.Items.AddRange(new object[] { "1", "2", "3", "4", "5", "6", "7", "8" });
            tailList.Location = new Point(312, 112);
            tailList.Name = "tailList";
            tailList.Size = new Size(32, 23);
            tailList.TabIndex = 3;
            tailList.Text = "8";
            // 
            // baseBodyList
            // 
            baseBodyList.FormattingEnabled = true;
            baseBodyList.Items.AddRange(new object[] { "Vanilla and Gen2", "BIBO+", "EVE", "Gen3 and T&F3", "SCALES+", "TBSE and HRBODY", "TAIL", "Otopop" });
            baseBodyList.Location = new Point(4, 112);
            baseBodyList.Name = "baseBodyList";
            baseBodyList.Size = new Size(144, 23);
            baseBodyList.TabIndex = 4;
            baseBodyList.Text = "Gen3 and T&F3";
            baseBodyList.SelectedIndexChanged += baseBodyList_SelectedIndexChanged;
            // 
            // generateButton
            // 
            generateButton.Anchor = AnchorStyles.Bottom;
            generateButton.Location = new Point(412, 608);
            generateButton.Name = "generateButton";
            generateButton.Size = new Size(56, 24);
            generateButton.TabIndex = 7;
            generateButton.Text = "Preview";
            generateButton.UseVisualStyleBackColor = true;
            generateButton.Click += generateButton_Click;
            // 
            // uniqueAuRa
            // 
            uniqueAuRa.AutoSize = true;
            uniqueAuRa.Location = new Point(348, 8);
            uniqueAuRa.Name = "uniqueAuRa";
            uniqueAuRa.Size = new Size(98, 19);
            uniqueAuRa.TabIndex = 20;
            uniqueAuRa.Text = "Unique Au Ra";
            uniqueAuRa.UseVisualStyleBackColor = true;
            // 
            // multi
            // 
            multi.BackColor = Color.FromArgb(255, 224, 192);
            multi.CurrentPath = null;
            multi.Enabled = false;
            multi.Filter = "Texture File|*.png;*.dds;*.bmp;**.tex;";
            multi.Index = -1;
            multi.Location = new Point(4, 512);
            multi.Margin = new Padding(4, 3, 4, 3);
            multi.MinimumSize = new Size(300, 28);
            multi.Name = "multi";
            multi.Size = new Size(528, 28);
            multi.TabIndex = 19;
            helperToolTip.SetToolTip(multi, "The orange looking image goes here.");
            multi.OnFileSelected += multi_OnFileSelected;
            multi.Enter += multi_Enter;
            multi.Leave += multi_Leave;
            // 
            // normal
            // 
            normal.BackColor = SystemColors.GradientInactiveCaption;
            normal.CurrentPath = null;
            normal.Enabled = false;
            normal.Filter = "Texture File|*.png;*.dds;*.bmp;**.tex;";
            normal.Index = -1;
            normal.Location = new Point(4, 480);
            normal.Margin = new Padding(4, 3, 4, 3);
            normal.MinimumSize = new Size(300, 28);
            normal.Name = "normal";
            normal.Size = new Size(528, 28);
            normal.TabIndex = 18;
            helperToolTip.SetToolTip(normal, "The blue and red bump map goes here");
            normal.OnFileSelected += multi_OnFileSelected;
            normal.Enter += multi_Enter;
            normal.Leave += multi_Leave;
            // 
            // diffuse
            // 
            diffuse.BackColor = Color.LavenderBlush;
            diffuse.CurrentPath = null;
            diffuse.Enabled = false;
            diffuse.Filter = "Texture File|*.png;*.dds;*.bmp;**.tex;";
            diffuse.Index = -1;
            diffuse.Location = new Point(4, 448);
            diffuse.Margin = new Padding(4, 3, 4, 3);
            diffuse.MinimumSize = new Size(300, 28);
            diffuse.Name = "diffuse";
            diffuse.Size = new Size(528, 28);
            diffuse.TabIndex = 17;
            helperToolTip.SetToolTip(diffuse, "Skin, and tattoo overlays go here.");
            diffuse.OnFileSelected += multi_OnFileSelected;
            diffuse.Enter += multi_Enter;
            diffuse.Leave += multi_Leave;
            // 
            // asymCheckbox
            // 
            asymCheckbox.AutoSize = true;
            asymCheckbox.Location = new Point(392, 8);
            asymCheckbox.Name = "asymCheckbox";
            asymCheckbox.Size = new Size(56, 19);
            asymCheckbox.TabIndex = 23;
            asymCheckbox.Text = "Asym";
            asymCheckbox.UseVisualStyleBackColor = true;
            // 
            // facePart
            // 
            facePart.FormattingEnabled = true;
            facePart.Items.AddRange(new object[] { "Face", "Eyebrows", "Eyes", "Ears", "Face Paint", "Hair", "Face B", "Etc B" });
            facePart.Location = new Point(148, 140);
            facePart.Name = "facePart";
            facePart.Size = new Size(80, 23);
            facePart.TabIndex = 4;
            facePart.Text = "Eyebrows";
            facePart.SelectedIndexChanged += facePart_SelectedIndexChanged;
            // 
            // faceTypeList
            // 
            faceTypeList.FormattingEnabled = true;
            faceTypeList.Items.AddRange(new object[] { "Face 1", "Face 2", "Face 3", "Face 4", "Face 5", "Face 6", "Face 7", "Face 8", "Face 9" });
            faceTypeList.Location = new Point(88, 140);
            faceTypeList.Name = "faceTypeList";
            faceTypeList.Size = new Size(60, 23);
            faceTypeList.TabIndex = 3;
            faceTypeList.Text = "Face 4";
            // 
            // subRaceList
            // 
            subRaceList.FormattingEnabled = true;
            subRaceList.Items.AddRange(new object[] { "Midlander", "Highlander", "Wildwood", "Duskwight", "Seeker", "Keeper", "Sea Wolf", "Hellsguard", "Plainsfolk", "Dunesfolk", "Raen", "Xaela", "Helion", "The Lost", "Rava", "Veena" });
            subRaceList.Location = new Point(4, 140);
            subRaceList.Name = "subRaceList";
            subRaceList.Size = new Size(84, 23);
            subRaceList.TabIndex = 2;
            subRaceList.Text = "Sea Wolves";
            subRaceList.SelectedIndexChanged += subRaceList_SelectedIndexChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 88);
            label4.Name = "label4";
            label4.Size = new Size(67, 15);
            label4.TabIndex = 23;
            label4.Text = "Description";
            // 
            // modDescriptionTextBox
            // 
            modDescriptionTextBox.Location = new Point(96, 84);
            modDescriptionTextBox.Name = "modDescriptionTextBox";
            modDescriptionTextBox.Size = new Size(436, 23);
            modDescriptionTextBox.TabIndex = 22;
            modDescriptionTextBox.Text = "Exported by FFXIV Loose Texture Compiler";
            modDescriptionTextBox.TextChanged += modDescriptionTextBox_TextChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(256, 32);
            label3.Name = "label3";
            label3.Size = new Size(45, 15);
            label3.TabIndex = 21;
            label3.Text = "Version";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(256, 60);
            label2.Name = "label2";
            label2.Size = new Size(49, 15);
            label2.TabIndex = 16;
            label2.Text = "Website";
            // 
            // modWebsiteTextBox
            // 
            modWebsiteTextBox.Location = new Point(328, 56);
            modWebsiteTextBox.Name = "modWebsiteTextBox";
            modWebsiteTextBox.Size = new Size(204, 23);
            modWebsiteTextBox.TabIndex = 15;
            modWebsiteTextBox.Text = "https://github.com/Sebane1/FFXIVLooseTextureCompiler";
            modWebsiteTextBox.TextChanged += modDescriptionTextBox_TextChanged;
            modWebsiteTextBox.Leave += modWebsiteTextBox_Leave;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 60);
            label1.Name = "label1";
            label1.Size = new Size(44, 15);
            label1.TabIndex = 14;
            label1.Text = "Author";
            // 
            // modAuthorTextBox
            // 
            modAuthorTextBox.Location = new Point(96, 56);
            modAuthorTextBox.Name = "modAuthorTextBox";
            modAuthorTextBox.Size = new Size(148, 23);
            modAuthorTextBox.TabIndex = 13;
            modAuthorTextBox.Text = "FFXIV Loose Texture Compiler";
            modAuthorTextBox.TextChanged += modDescriptionTextBox_TextChanged;
            modAuthorTextBox.Leave += modAuthorTextBox_Leave;
            // 
            // nameLabel
            // 
            nameLabel.AutoSize = true;
            nameLabel.Location = new Point(12, 32);
            nameLabel.Name = "nameLabel";
            nameLabel.Size = new Size(39, 15);
            nameLabel.TabIndex = 12;
            nameLabel.Text = "Name";
            // 
            // modNameTextBox
            // 
            modNameTextBox.Location = new Point(96, 28);
            modNameTextBox.Name = "modNameTextBox";
            modNameTextBox.Size = new Size(148, 23);
            modNameTextBox.TabIndex = 11;
            modNameTextBox.TextChanged += modDescriptionTextBox_TextChanged;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, editToolStripMenuItem, toolsToolStripMenuItem, configToolStripMenuItem, modShareToolStripMenuItem, creditsToolStripMenuItem, helpToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(537, 24);
            menuStrip1.TabIndex = 24;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { newToolStripMenuItem, openToolStripMenuItem, saveToolStripMenuItem, saveAsToolStripMenuItem, templatesToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            newToolStripMenuItem.Name = "newToolStripMenuItem";
            newToolStripMenuItem.Size = new Size(127, 22);
            newToolStripMenuItem.Text = "New";
            newToolStripMenuItem.Click += newToolStripMenuItem_Click;
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(127, 22);
            openToolStripMenuItem.Text = "Open";
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new Size(127, 22);
            saveToolStripMenuItem.Text = "Save";
            saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
            // 
            // saveAsToolStripMenuItem
            // 
            saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            saveAsToolStripMenuItem.Size = new Size(127, 22);
            saveAsToolStripMenuItem.Text = "Save As";
            saveAsToolStripMenuItem.Click += saveAsToolStripMenuItem_Click;
            // 
            // templatesToolStripMenuItem
            // 
            templatesToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { importCustomTemplateToolStripMenuItem });
            templatesToolStripMenuItem.Name = "templatesToolStripMenuItem";
            templatesToolStripMenuItem.Size = new Size(127, 22);
            templatesToolStripMenuItem.Text = "Templates";
            // 
            // importCustomTemplateToolStripMenuItem
            // 
            importCustomTemplateToolStripMenuItem.Name = "importCustomTemplateToolStripMenuItem";
            importCustomTemplateToolStripMenuItem.Size = new Size(217, 22);
            importCustomTemplateToolStripMenuItem.Text = "Import Project As Template";
            importCustomTemplateToolStripMenuItem.Click += importCustomTemplateToolStripMenuItem_Click;
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { findAndBulkReplaceToolStripMenuItem });
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new Size(39, 20);
            editToolStripMenuItem.Text = "Edit";
            // 
            // findAndBulkReplaceToolStripMenuItem
            // 
            findAndBulkReplaceToolStripMenuItem.Name = "findAndBulkReplaceToolStripMenuItem";
            findAndBulkReplaceToolStripMenuItem.Size = new Size(192, 22);
            findAndBulkReplaceToolStripMenuItem.Text = "Find And Bulk Replace";
            findAndBulkReplaceToolStripMenuItem.Click += findAndBulkReplaceToolStripMenuItem_Click;
            // 
            // toolsToolStripMenuItem
            // 
            toolsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { extractAtramentumLuminisGlowMapToolStripMenuItem, convertStandaloneTextureToolStripMenuItem, eyeToolsToolStripMenuItem, hairToolsToolStripMenuItem, clothingToolsToolStripMenuItem, colourChannelSplittingToolStripMenuItem, imageToTexConversionToolStripMenuItem, devToolsToolStripMenuItem });
            toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            toolsToolStripMenuItem.Size = new Size(46, 20);
            toolsToolStripMenuItem.Text = "Tools";
            // 
            // extractAtramentumLuminisGlowMapToolStripMenuItem
            // 
            extractAtramentumLuminisGlowMapToolStripMenuItem.Name = "extractAtramentumLuminisGlowMapToolStripMenuItem";
            extractAtramentumLuminisGlowMapToolStripMenuItem.Size = new Size(299, 22);
            extractAtramentumLuminisGlowMapToolStripMenuItem.Text = "Atramentum Luminis Diffuse To Glow Map";
            extractAtramentumLuminisGlowMapToolStripMenuItem.Click += extractAtramentumLuminisGlowMapToolStripMenuItem_Click;
            // 
            // convertStandaloneTextureToolStripMenuItem
            // 
            convertStandaloneTextureToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { biboToGen3ToolStripMenuItem, biboToGen2ToolStripMenuItem, gen3ToBiboToolStripMenuItem, gen3ToGen2ToolStripMenuItem1, gen2ToGen3ToolStripMenuItem, gen2ToBiboToolStripMenuItem, otopopToVanillaToolStripMenuItem, vanillaToOtopopToolStripMenuItem });
            convertStandaloneTextureToolStripMenuItem.Name = "convertStandaloneTextureToolStripMenuItem";
            convertStandaloneTextureToolStripMenuItem.Size = new Size(299, 22);
            convertStandaloneTextureToolStripMenuItem.Text = "Convert Standalone Texture";
            // 
            // biboToGen3ToolStripMenuItem
            // 
            biboToGen3ToolStripMenuItem.Name = "biboToGen3ToolStripMenuItem";
            biboToGen3ToolStripMenuItem.Size = new Size(166, 22);
            biboToGen3ToolStripMenuItem.Text = "Bibo+ to Gen3";
            biboToGen3ToolStripMenuItem.Click += biboToGen3ToolStripMenuItem_Click;
            // 
            // biboToGen2ToolStripMenuItem
            // 
            biboToGen2ToolStripMenuItem.Name = "biboToGen2ToolStripMenuItem";
            biboToGen2ToolStripMenuItem.Size = new Size(166, 22);
            biboToGen2ToolStripMenuItem.Text = "Bibo+ to Gen2";
            biboToGen2ToolStripMenuItem.Click += biboToGen2ToolStripMenuItem_Click;
            // 
            // gen3ToBiboToolStripMenuItem
            // 
            gen3ToBiboToolStripMenuItem.Name = "gen3ToBiboToolStripMenuItem";
            gen3ToBiboToolStripMenuItem.Size = new Size(166, 22);
            gen3ToBiboToolStripMenuItem.Text = "Gen3 to Bibo+";
            gen3ToBiboToolStripMenuItem.Click += gen3ToBiboToolStripMenuItem_Click;
            // 
            // gen3ToGen2ToolStripMenuItem1
            // 
            gen3ToGen2ToolStripMenuItem1.Name = "gen3ToGen2ToolStripMenuItem1";
            gen3ToGen2ToolStripMenuItem1.Size = new Size(166, 22);
            gen3ToGen2ToolStripMenuItem1.Text = "Gen3 to Gen2";
            gen3ToGen2ToolStripMenuItem1.Click += gen3ToGen2ToolStripMenuItem_Click;
            // 
            // gen2ToGen3ToolStripMenuItem
            // 
            gen2ToGen3ToolStripMenuItem.Name = "gen2ToGen3ToolStripMenuItem";
            gen2ToGen3ToolStripMenuItem.Size = new Size(166, 22);
            gen2ToGen3ToolStripMenuItem.Text = "Gen2 to Gen3";
            gen2ToGen3ToolStripMenuItem.Click += gen2ToGen3ToolStripMenuItem_Click;
            // 
            // gen2ToBiboToolStripMenuItem
            // 
            gen2ToBiboToolStripMenuItem.Name = "gen2ToBiboToolStripMenuItem";
            gen2ToBiboToolStripMenuItem.Size = new Size(166, 22);
            gen2ToBiboToolStripMenuItem.Text = "Gen2 to Bibo+";
            gen2ToBiboToolStripMenuItem.Click += gen2ToBiboToolStripMenuItem_Click;
            // 
            // otopopToVanillaToolStripMenuItem
            // 
            otopopToVanillaToolStripMenuItem.Name = "otopopToVanillaToolStripMenuItem";
            otopopToVanillaToolStripMenuItem.Size = new Size(166, 22);
            otopopToVanillaToolStripMenuItem.Text = "Otopop to Vanilla";
            otopopToVanillaToolStripMenuItem.Click += otopopToVanillaToolStripMenuItem_Click;
            // 
            // vanillaToOtopopToolStripMenuItem
            // 
            vanillaToOtopopToolStripMenuItem.Name = "vanillaToOtopopToolStripMenuItem";
            vanillaToOtopopToolStripMenuItem.Size = new Size(166, 22);
            vanillaToOtopopToolStripMenuItem.Text = "Vanilla to Otopop";
            vanillaToOtopopToolStripMenuItem.Click += vanillaToOtopopToolStripMenuItem_Click;
            // 
            // eyeToolsToolStripMenuItem
            // 
            eyeToolsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { convertImageToEyeMultiToolStripMenuItem, convertImagesToAsymEyeMapsToolStripMenuItem, convertFolderToEyeMapsToolStripMenuItem, multiMapToGrayscaleToolStripMenuItem });
            eyeToolsToolStripMenuItem.Name = "eyeToolsToolStripMenuItem";
            eyeToolsToolStripMenuItem.Size = new Size(299, 22);
            eyeToolsToolStripMenuItem.Text = "Eye Tools";
            // 
            // convertImageToEyeMultiToolStripMenuItem
            // 
            convertImageToEyeMultiToolStripMenuItem.Name = "convertImageToEyeMultiToolStripMenuItem";
            convertImageToEyeMultiToolStripMenuItem.Size = new Size(258, 22);
            convertImageToEyeMultiToolStripMenuItem.Text = "Convert Image To Eye Maps";
            convertImageToEyeMultiToolStripMenuItem.Click += convertImageToEyeMultiToolStripMenuItem_Click;
            // 
            // convertImagesToAsymEyeMapsToolStripMenuItem
            // 
            convertImagesToAsymEyeMapsToolStripMenuItem.Name = "convertImagesToAsymEyeMapsToolStripMenuItem";
            convertImagesToAsymEyeMapsToolStripMenuItem.Size = new Size(258, 22);
            convertImagesToAsymEyeMapsToolStripMenuItem.Text = "Convert Images To Asym Eye Maps";
            convertImagesToAsymEyeMapsToolStripMenuItem.Click += convertImagesToAsymEyeMapsToolStripMenuItem_Click;
            // 
            // convertFolderToEyeMapsToolStripMenuItem
            // 
            convertFolderToEyeMapsToolStripMenuItem.Name = "convertFolderToEyeMapsToolStripMenuItem";
            convertFolderToEyeMapsToolStripMenuItem.Size = new Size(258, 22);
            convertFolderToEyeMapsToolStripMenuItem.Text = "Convert Folder To Eye Maps";
            convertFolderToEyeMapsToolStripMenuItem.Click += convertFolderToEyeMapsToolStripMenuItem_Click;
            // 
            // multiMapToGrayscaleToolStripMenuItem
            // 
            multiMapToGrayscaleToolStripMenuItem.Name = "multiMapToGrayscaleToolStripMenuItem";
            multiMapToGrayscaleToolStripMenuItem.Size = new Size(258, 22);
            multiMapToGrayscaleToolStripMenuItem.Text = "Multi Map To Grayscale";
            multiMapToGrayscaleToolStripMenuItem.Click += multiMapToGrayscaleToolStripMenuItem_Click;
            // 
            // hairToolsToolStripMenuItem
            // 
            hairToolsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { hairDiffuseToFFXIVHairMapsToolStripMenuItem });
            hairToolsToolStripMenuItem.Name = "hairToolsToolStripMenuItem";
            hairToolsToolStripMenuItem.Size = new Size(299, 22);
            hairToolsToolStripMenuItem.Text = "Hair Tools";
            // 
            // hairDiffuseToFFXIVHairMapsToolStripMenuItem
            // 
            hairDiffuseToFFXIVHairMapsToolStripMenuItem.Name = "hairDiffuseToFFXIVHairMapsToolStripMenuItem";
            hairDiffuseToFFXIVHairMapsToolStripMenuItem.Size = new Size(240, 22);
            hairDiffuseToFFXIVHairMapsToolStripMenuItem.Text = "Hair Diffuse To FFXIV Hair Maps";
            hairDiffuseToFFXIVHairMapsToolStripMenuItem.Click += hairDiffuseToFFXIVHairMapsToolStripMenuItem_Click;
            // 
            // clothingToolsToolStripMenuItem
            // 
            clothingToolsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { convertDiffuseToNormalAndMultiToolStripMenuItem });
            clothingToolsToolStripMenuItem.Name = "clothingToolsToolStripMenuItem";
            clothingToolsToolStripMenuItem.Size = new Size(299, 22);
            clothingToolsToolStripMenuItem.Text = "Clothing Tools";
            // 
            // convertDiffuseToNormalAndMultiToolStripMenuItem
            // 
            convertDiffuseToNormalAndMultiToolStripMenuItem.Name = "convertDiffuseToNormalAndMultiToolStripMenuItem";
            convertDiffuseToNormalAndMultiToolStripMenuItem.Size = new Size(270, 22);
            convertDiffuseToNormalAndMultiToolStripMenuItem.Text = "Convert Diffuse To Normal And Multi";
            convertDiffuseToNormalAndMultiToolStripMenuItem.Click += convertDiffuseToNormalAndMultiToolStripMenuItem_Click;
            // 
            // colourChannelSplittingToolStripMenuItem
            // 
            colourChannelSplittingToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { diffuseMergerToolStripMenuItem, multiCreatorToolStripMenuItem, mergeRGBAndAlphaImagesToolStripMenuItem, imageToRGBChannelsToolStripMenuItem, splitImageToRGBAndAlphaToolStripMenuItem, textureToBodyMultiToolStripMenuItem, textureToFaceMultiToolStripMenuItem, textureToAsymFaceMultiToolStripMenuItem, xNormalToolStripMenuItem });
            colourChannelSplittingToolStripMenuItem.Name = "colourChannelSplittingToolStripMenuItem";
            colourChannelSplittingToolStripMenuItem.Size = new Size(299, 22);
            colourChannelSplittingToolStripMenuItem.Text = "Texture Manipulation";
            // 
            // diffuseMergerToolStripMenuItem
            // 
            diffuseMergerToolStripMenuItem.Name = "diffuseMergerToolStripMenuItem";
            diffuseMergerToolStripMenuItem.Size = new Size(233, 22);
            diffuseMergerToolStripMenuItem.Text = "Diffuse Merger";
            diffuseMergerToolStripMenuItem.Click += diffuseMergerToolStripMenuItem_Click;
            // 
            // multiCreatorToolStripMenuItem
            // 
            multiCreatorToolStripMenuItem.Name = "multiCreatorToolStripMenuItem";
            multiCreatorToolStripMenuItem.Size = new Size(233, 22);
            multiCreatorToolStripMenuItem.Text = "RGBA Merger";
            multiCreatorToolStripMenuItem.Click += multiCreatorToolStripMenuItem_Click;
            // 
            // mergeRGBAndAlphaImagesToolStripMenuItem
            // 
            mergeRGBAndAlphaImagesToolStripMenuItem.Name = "mergeRGBAndAlphaImagesToolStripMenuItem";
            mergeRGBAndAlphaImagesToolStripMenuItem.Size = new Size(233, 22);
            mergeRGBAndAlphaImagesToolStripMenuItem.Text = "Merge RGB and Alpha Images";
            mergeRGBAndAlphaImagesToolStripMenuItem.Click += mergeRGBAndAlphaImagesToolStripMenuItem_Click;
            // 
            // imageToRGBChannelsToolStripMenuItem
            // 
            imageToRGBChannelsToolStripMenuItem.Name = "imageToRGBChannelsToolStripMenuItem";
            imageToRGBChannelsToolStripMenuItem.Size = new Size(233, 22);
            imageToRGBChannelsToolStripMenuItem.Text = "Split Image To RGBA Channels";
            imageToRGBChannelsToolStripMenuItem.Click += imageToRGBChannelsToolStripMenuItem_Click;
            // 
            // splitImageToRGBAndAlphaToolStripMenuItem
            // 
            splitImageToRGBAndAlphaToolStripMenuItem.Name = "splitImageToRGBAndAlphaToolStripMenuItem";
            splitImageToRGBAndAlphaToolStripMenuItem.Size = new Size(233, 22);
            splitImageToRGBAndAlphaToolStripMenuItem.Text = "Split Image to RGB and Alpha";
            splitImageToRGBAndAlphaToolStripMenuItem.Click += splitImageToRGBAndAlphaToolStripMenuItem_Click;
            // 
            // textureToBodyMultiToolStripMenuItem
            // 
            textureToBodyMultiToolStripMenuItem.Name = "textureToBodyMultiToolStripMenuItem";
            textureToBodyMultiToolStripMenuItem.Size = new Size(233, 22);
            textureToBodyMultiToolStripMenuItem.Text = "Texture To Body Multi";
            textureToBodyMultiToolStripMenuItem.Click += textureToBodyMultiToolStripMenuItem_Click;
            // 
            // textureToFaceMultiToolStripMenuItem
            // 
            textureToFaceMultiToolStripMenuItem.Name = "textureToFaceMultiToolStripMenuItem";
            textureToFaceMultiToolStripMenuItem.Size = new Size(233, 22);
            textureToFaceMultiToolStripMenuItem.Text = "Texture To Face Multi";
            textureToFaceMultiToolStripMenuItem.Click += textureToFaceMultiToolStripMenuItem_Click;
            // 
            // textureToAsymFaceMultiToolStripMenuItem
            // 
            textureToAsymFaceMultiToolStripMenuItem.Name = "textureToAsymFaceMultiToolStripMenuItem";
            textureToAsymFaceMultiToolStripMenuItem.Size = new Size(233, 22);
            textureToAsymFaceMultiToolStripMenuItem.Text = "Texture To Asym Face Multi";
            textureToAsymFaceMultiToolStripMenuItem.Click += textureToAsymFaceMultiToolStripMenuItem_Click;
            // 
            // xNormalToolStripMenuItem
            // 
            xNormalToolStripMenuItem.Name = "xNormalToolStripMenuItem";
            xNormalToolStripMenuItem.Size = new Size(233, 22);
            xNormalToolStripMenuItem.Text = "XNormal";
            xNormalToolStripMenuItem.Click += xNormalToolStripMenuItem_Click;
            // 
            // imageToTexConversionToolStripMenuItem
            // 
            imageToTexConversionToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { bulkTexViewerToolStripMenuItem, bulkImageToTexToolStripMenuItem, recursiveBulkImageToTexToolStripMenuItem });
            imageToTexConversionToolStripMenuItem.Name = "imageToTexConversionToolStripMenuItem";
            imageToTexConversionToolStripMenuItem.Size = new Size(299, 22);
            imageToTexConversionToolStripMenuItem.Text = "Tex File Management";
            // 
            // bulkTexViewerToolStripMenuItem
            // 
            bulkTexViewerToolStripMenuItem.Name = "bulkTexViewerToolStripMenuItem";
            bulkTexViewerToolStripMenuItem.Size = new Size(221, 22);
            bulkTexViewerToolStripMenuItem.Text = "Bulk Tex File Manager";
            bulkTexViewerToolStripMenuItem.Click += bulkTexViewerToolStripMenuItem_Click;
            // 
            // bulkImageToTexToolStripMenuItem
            // 
            bulkImageToTexToolStripMenuItem.Name = "bulkImageToTexToolStripMenuItem";
            bulkImageToTexToolStripMenuItem.Size = new Size(221, 22);
            bulkImageToTexToolStripMenuItem.Text = "Bulk Image To Tex";
            bulkImageToTexToolStripMenuItem.Click += bulkImageToTexToolStripMenuItem_Click;
            // 
            // recursiveBulkImageToTexToolStripMenuItem
            // 
            recursiveBulkImageToTexToolStripMenuItem.Name = "recursiveBulkImageToTexToolStripMenuItem";
            recursiveBulkImageToTexToolStripMenuItem.Size = new Size(221, 22);
            recursiveBulkImageToTexToolStripMenuItem.Text = "Recursive Bulk Image To Tex";
            recursiveBulkImageToTexToolStripMenuItem.Click += recursiveBulkImageToTexToolStripMenuItem_Click;
            // 
            // devToolsToolStripMenuItem
            // 
            devToolsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { textureToLTCTToolStripMenuItem, pNGToLTCTToolStripMenuItem, convertLTCTToPNGToolStripMenuItem });
            devToolsToolStripMenuItem.Name = "devToolsToolStripMenuItem";
            devToolsToolStripMenuItem.Size = new Size(299, 22);
            devToolsToolStripMenuItem.Text = "Dev Tools";
            // 
            // textureToLTCTToolStripMenuItem
            // 
            textureToLTCTToolStripMenuItem.Name = "textureToLTCTToolStripMenuItem";
            textureToLTCTToolStripMenuItem.Size = new Size(185, 22);
            textureToLTCTToolStripMenuItem.Text = "Texture To LTCT";
            textureToLTCTToolStripMenuItem.Click += bulkConvertImagesToLTCTToolStripMenuItem_Click;
            // 
            // pNGToLTCTToolStripMenuItem
            // 
            pNGToLTCTToolStripMenuItem.Name = "pNGToLTCTToolStripMenuItem";
            pNGToLTCTToolStripMenuItem.Size = new Size(185, 22);
            pNGToLTCTToolStripMenuItem.Text = "PNG To LTCT";
            pNGToLTCTToolStripMenuItem.Click += convertPNGToLTCTToolStripMenuItem_Click;
            // 
            // convertLTCTToPNGToolStripMenuItem
            // 
            convertLTCTToPNGToolStripMenuItem.Name = "convertLTCTToPNGToolStripMenuItem";
            convertLTCTToPNGToolStripMenuItem.Size = new Size(185, 22);
            convertLTCTToPNGToolStripMenuItem.Text = "Convert LTCT To PNG";
            convertLTCTToPNGToolStripMenuItem.Click += bulkConvertLTCTToPNGToolStripMenuItem_Click;
            // 
            // configToolStripMenuItem
            // 
            configToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { changePenumbraPathToolStripMenuItem });
            configToolStripMenuItem.Name = "configToolStripMenuItem";
            configToolStripMenuItem.Size = new Size(55, 20);
            configToolStripMenuItem.Text = "Config";
            // 
            // changePenumbraPathToolStripMenuItem
            // 
            changePenumbraPathToolStripMenuItem.Name = "changePenumbraPathToolStripMenuItem";
            changePenumbraPathToolStripMenuItem.Size = new Size(200, 22);
            changePenumbraPathToolStripMenuItem.Text = "Change Penumbra Path";
            changePenumbraPathToolStripMenuItem.Click += changePenumbraPathToolStripMenuItem_Click;
            // 
            // modShareToolStripMenuItem
            // 
            modShareToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { enableModshareToolStripMenuItem, sendCurrentModToolStripMenuItem });
            modShareToolStripMenuItem.Name = "modShareToolStripMenuItem";
            modShareToolStripMenuItem.Size = new Size(76, 20);
            modShareToolStripMenuItem.Text = "Mod Share";
            // 
            // enableModshareToolStripMenuItem
            // 
            enableModshareToolStripMenuItem.Name = "enableModshareToolStripMenuItem";
            enableModshareToolStripMenuItem.Size = new Size(171, 22);
            enableModshareToolStripMenuItem.Text = "Enable Modshare";
            enableModshareToolStripMenuItem.Click += enableModshareToolStripMenuItem_Click;
            // 
            // sendCurrentModToolStripMenuItem
            // 
            sendCurrentModToolStripMenuItem.Enabled = false;
            sendCurrentModToolStripMenuItem.Name = "sendCurrentModToolStripMenuItem";
            sendCurrentModToolStripMenuItem.Size = new Size(171, 22);
            sendCurrentModToolStripMenuItem.Text = "Send Current Mod";
            sendCurrentModToolStripMenuItem.Click += sendCurrentModToolStripMenuItem_Click;
            // 
            // creditsToolStripMenuItem
            // 
            creditsToolStripMenuItem.Name = "creditsToolStripMenuItem";
            creditsToolStripMenuItem.Size = new Size(56, 20);
            creditsToolStripMenuItem.Text = "Credits";
            creditsToolStripMenuItem.Click += creditsToolStripMenuItem_Click;
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { howToGetTexturesToolStripMenuItem, howDoIUseThisToolStripMenuItem, howDoIMakeStuffBumpyToolStripMenuItem, howDoIMakeStuffGlowToolStripMenuItem, howDoIMakeEyesToolStripMenuItem, canIMakeMyBiboOrGen3BodyWorkOnAnotherBodyToolStripMenuItem, canICustomizeTheGroupsThisToolExporrtsToolStripMenuItem, canIReplaceABunchOfStuffAtOnceToolStripMenuItem, whatIsModshareAndCanIQuicklySendAModToSomebodyElseToolStripMenuItem, whatAreTemplatesAndHowDoIUseThemToolStripMenuItem, thisToolIsTooHardMakeItSimplerToolStripMenuItem });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(44, 20);
            helpToolStripMenuItem.Text = "Help";
            // 
            // howToGetTexturesToolStripMenuItem
            // 
            howToGetTexturesToolStripMenuItem.Name = "howToGetTexturesToolStripMenuItem";
            howToGetTexturesToolStripMenuItem.Size = new Size(430, 22);
            howToGetTexturesToolStripMenuItem.Text = "How do I get textures?";
            howToGetTexturesToolStripMenuItem.Click += howToGetTexturesToolStripMenuItem_Click;
            // 
            // howDoIUseThisToolStripMenuItem
            // 
            howDoIUseThisToolStripMenuItem.Name = "howDoIUseThisToolStripMenuItem";
            howDoIUseThisToolStripMenuItem.Size = new Size(430, 22);
            howDoIUseThisToolStripMenuItem.Text = "How do I use this?";
            howDoIUseThisToolStripMenuItem.Click += howDoIUseThisToolStripMenuItem_Click;
            // 
            // howDoIMakeStuffBumpyToolStripMenuItem
            // 
            howDoIMakeStuffBumpyToolStripMenuItem.Name = "howDoIMakeStuffBumpyToolStripMenuItem";
            howDoIMakeStuffBumpyToolStripMenuItem.Size = new Size(430, 22);
            howDoIMakeStuffBumpyToolStripMenuItem.Text = "How do I make stuff bumpy?";
            howDoIMakeStuffBumpyToolStripMenuItem.Click += howDoIMakeStuffBumpyToolStripMenuItem_Click;
            // 
            // howDoIMakeStuffGlowToolStripMenuItem
            // 
            howDoIMakeStuffGlowToolStripMenuItem.Name = "howDoIMakeStuffGlowToolStripMenuItem";
            howDoIMakeStuffGlowToolStripMenuItem.Size = new Size(430, 22);
            howDoIMakeStuffGlowToolStripMenuItem.Text = "How do I make stuff glow?";
            howDoIMakeStuffGlowToolStripMenuItem.Click += howDoIMakeStuffGlowToolStripMenuItem_Click;
            // 
            // howDoIMakeEyesToolStripMenuItem
            // 
            howDoIMakeEyesToolStripMenuItem.Name = "howDoIMakeEyesToolStripMenuItem";
            howDoIMakeEyesToolStripMenuItem.Size = new Size(430, 22);
            howDoIMakeEyesToolStripMenuItem.Text = "How do I make eyes?";
            howDoIMakeEyesToolStripMenuItem.Click += howDoIMakeEyesToolStripMenuItem_Click;
            // 
            // canIMakeMyBiboOrGen3BodyWorkOnAnotherBodyToolStripMenuItem
            // 
            canIMakeMyBiboOrGen3BodyWorkOnAnotherBodyToolStripMenuItem.Name = "canIMakeMyBiboOrGen3BodyWorkOnAnotherBodyToolStripMenuItem";
            canIMakeMyBiboOrGen3BodyWorkOnAnotherBodyToolStripMenuItem.Size = new Size(430, 22);
            canIMakeMyBiboOrGen3BodyWorkOnAnotherBodyToolStripMenuItem.Text = "Can I make my Bibo+ or Gen3 body texture work on another body?";
            canIMakeMyBiboOrGen3BodyWorkOnAnotherBodyToolStripMenuItem.Click += canIMakeMyBiboOrGen3BodyWorkOnAnotherBodyToolStripMenuItem_Click;
            // 
            // canICustomizeTheGroupsThisToolExporrtsToolStripMenuItem
            // 
            canICustomizeTheGroupsThisToolExporrtsToolStripMenuItem.Name = "canICustomizeTheGroupsThisToolExporrtsToolStripMenuItem";
            canICustomizeTheGroupsThisToolExporrtsToolStripMenuItem.Size = new Size(430, 22);
            canICustomizeTheGroupsThisToolExporrtsToolStripMenuItem.Text = "Can I customize the groups this tool exporrts";
            canICustomizeTheGroupsThisToolExporrtsToolStripMenuItem.Click += canICustomizeTheGroupsThisToolExporrtsToolStripMenuItem_Click;
            // 
            // canIReplaceABunchOfStuffAtOnceToolStripMenuItem
            // 
            canIReplaceABunchOfStuffAtOnceToolStripMenuItem.Name = "canIReplaceABunchOfStuffAtOnceToolStripMenuItem";
            canIReplaceABunchOfStuffAtOnceToolStripMenuItem.Size = new Size(430, 22);
            canIReplaceABunchOfStuffAtOnceToolStripMenuItem.Text = "Can I replace a bunch of stuff at once?";
            canIReplaceABunchOfStuffAtOnceToolStripMenuItem.Click += canIReplaceABunchOfStuffAtOnceToolStripMenuItem_Click;
            // 
            // whatIsModshareAndCanIQuicklySendAModToSomebodyElseToolStripMenuItem
            // 
            whatIsModshareAndCanIQuicklySendAModToSomebodyElseToolStripMenuItem.Name = "whatIsModshareAndCanIQuicklySendAModToSomebodyElseToolStripMenuItem";
            whatIsModshareAndCanIQuicklySendAModToSomebodyElseToolStripMenuItem.Size = new Size(430, 22);
            whatIsModshareAndCanIQuicklySendAModToSomebodyElseToolStripMenuItem.Text = "What is modshare, and can I quickly send a mod to somebody else?";
            whatIsModshareAndCanIQuicklySendAModToSomebodyElseToolStripMenuItem.Click += whatIsModshareAndCanIQuicklySendAModToSomebodyElseToolStripMenuItem_Click;
            // 
            // whatAreTemplatesAndHowDoIUseThemToolStripMenuItem
            // 
            whatAreTemplatesAndHowDoIUseThemToolStripMenuItem.Name = "whatAreTemplatesAndHowDoIUseThemToolStripMenuItem";
            whatAreTemplatesAndHowDoIUseThemToolStripMenuItem.Size = new Size(430, 22);
            whatAreTemplatesAndHowDoIUseThemToolStripMenuItem.Text = "What are templates, and how do I use them?";
            whatAreTemplatesAndHowDoIUseThemToolStripMenuItem.Click += whatAreTemplatesAndHowDoIUseThemToolStripMenuItem_Click;
            // 
            // thisToolIsTooHardMakeItSimplerToolStripMenuItem
            // 
            thisToolIsTooHardMakeItSimplerToolStripMenuItem.Name = "thisToolIsTooHardMakeItSimplerToolStripMenuItem";
            thisToolIsTooHardMakeItSimplerToolStripMenuItem.Size = new Size(430, 22);
            thisToolIsTooHardMakeItSimplerToolStripMenuItem.Text = "This tool is too hard, make it simpler.";
            thisToolIsTooHardMakeItSimplerToolStripMenuItem.Click += thisToolIsTooHardMakeItSimplerToolStripMenuItem_Click;
            // 
            // donateButton
            // 
            donateButton.BackColor = Color.IndianRed;
            donateButton.ForeColor = Color.White;
            donateButton.Location = new Point(464, 0);
            donateButton.Name = "donateButton";
            donateButton.Size = new Size(75, 23);
            donateButton.TabIndex = 25;
            donateButton.Text = "Donate";
            donateButton.UseVisualStyleBackColor = false;
            donateButton.Click += donateButton_Click;
            // 
            // textureList
            // 
            textureList.ContextMenuStrip = materialListContextMenu;
            textureList.FormattingEnabled = true;
            textureList.ItemHeight = 15;
            textureList.Location = new Point(4, 204);
            textureList.Name = "textureList";
            textureList.Size = new Size(528, 184);
            textureList.TabIndex = 26;
            textureList.SelectedIndexChanged += materialList_SelectedIndexChanged;
            // 
            // materialListContextMenu
            // 
            materialListContextMenu.Items.AddRange(new ToolStripItem[] { editPathsToolStripMenuItem, omniExportModeToolStripMenuItem, moveUpToolStripMenuItem, moveDownToolStripMenuItem, bulkNameReplacement, bulkReplaceToolStripMenuItem, duplicateToolStripMenuItem, deleteToolStripMenuItem });
            materialListContextMenu.Name = "materialListContextMenu";
            materialListContextMenu.Size = new Size(236, 180);
            materialListContextMenu.Opening += materialListContextMenu_Opening;
            // 
            // editPathsToolStripMenuItem
            // 
            editPathsToolStripMenuItem.Name = "editPathsToolStripMenuItem";
            editPathsToolStripMenuItem.Size = new Size(235, 22);
            editPathsToolStripMenuItem.Text = "Edit Internal Texture Set Values";
            editPathsToolStripMenuItem.Click += editPathsToolStripMenuItem_Click;
            // 
            // omniExportModeToolStripMenuItem
            // 
            omniExportModeToolStripMenuItem.Name = "omniExportModeToolStripMenuItem";
            omniExportModeToolStripMenuItem.Size = new Size(235, 22);
            omniExportModeToolStripMenuItem.Text = "Enable Universal Compatibility";
            omniExportModeToolStripMenuItem.Click += omniExportModeToolStripMenuItem_Click;
            // 
            // moveUpToolStripMenuItem
            // 
            moveUpToolStripMenuItem.Name = "moveUpToolStripMenuItem";
            moveUpToolStripMenuItem.Size = new Size(235, 22);
            moveUpToolStripMenuItem.Text = "Move Up";
            moveUpToolStripMenuItem.Click += moveUpButton_Click;
            // 
            // moveDownToolStripMenuItem
            // 
            moveDownToolStripMenuItem.Name = "moveDownToolStripMenuItem";
            moveDownToolStripMenuItem.Size = new Size(235, 22);
            moveDownToolStripMenuItem.Text = "Move Down";
            moveDownToolStripMenuItem.Click += moveDownButton_Click;
            // 
            // bulkNameReplacement
            // 
            bulkNameReplacement.Name = "bulkNameReplacement";
            bulkNameReplacement.Size = new Size(235, 22);
            bulkNameReplacement.Text = "Bulk Name Replacement";
            bulkNameReplacement.Click += bulkNameReplacement_Click;
            // 
            // bulkReplaceToolStripMenuItem
            // 
            bulkReplaceToolStripMenuItem.Name = "bulkReplaceToolStripMenuItem";
            bulkReplaceToolStripMenuItem.Size = new Size(235, 22);
            bulkReplaceToolStripMenuItem.Text = "Bulk Replace Values";
            bulkReplaceToolStripMenuItem.Click += bulkReplaceToolStripMenuItem_Click;
            // 
            // duplicateToolStripMenuItem
            // 
            duplicateToolStripMenuItem.Name = "duplicateToolStripMenuItem";
            duplicateToolStripMenuItem.Size = new Size(235, 22);
            duplicateToolStripMenuItem.Text = "Duplicate";
            duplicateToolStripMenuItem.Click += duplicateToolStripMenuItem_Click;
            // 
            // deleteToolStripMenuItem
            // 
            deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            deleteToolStripMenuItem.Size = new Size(235, 22);
            deleteToolStripMenuItem.Text = "Delete";
            deleteToolStripMenuItem.Click += removeSelectionButton_Click;
            // 
            // addBodyButton
            // 
            addBodyButton.Location = new Point(448, 112);
            addBodyButton.Name = "addBodyButton";
            addBodyButton.Size = new Size(84, 23);
            addBodyButton.TabIndex = 27;
            addBodyButton.Text = "Add Body";
            addBodyButton.UseVisualStyleBackColor = true;
            addBodyButton.Click += addBodyEditButton_Click;
            // 
            // addFaceButton
            // 
            addFaceButton.Location = new Point(448, 140);
            addFaceButton.Name = "addFaceButton";
            addFaceButton.Size = new Size(84, 23);
            addFaceButton.TabIndex = 28;
            addFaceButton.Text = "Add Face";
            addFaceButton.UseVisualStyleBackColor = true;
            addFaceButton.Click += addFaceButton_Click;
            // 
            // currentEditLabel
            // 
            currentEditLabel.AutoSize = true;
            currentEditLabel.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold);
            currentEditLabel.Location = new Point(4, 412);
            currentEditLabel.Name = "currentEditLabel";
            currentEditLabel.Size = new Size(447, 30);
            currentEditLabel.TabIndex = 29;
            currentEditLabel.Text = "Please select a texture set to start importing";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold);
            label6.Location = new Point(4, 172);
            label6.Name = "label6";
            label6.Size = new Size(155, 30);
            label6.TabIndex = 30;
            label6.Text = "Texture set list";
            // 
            // removeSelection
            // 
            removeSelection.Location = new Point(4, 388);
            removeSelection.Name = "removeSelection";
            removeSelection.Size = new Size(112, 23);
            removeSelection.TabIndex = 31;
            removeSelection.Text = "Remove Selection From List";
            removeSelection.UseVisualStyleBackColor = true;
            removeSelection.Click += removeSelectionButton_Click;
            // 
            // clearList
            // 
            clearList.Location = new Point(116, 388);
            clearList.Name = "clearList";
            clearList.Size = new Size(72, 23);
            clearList.TabIndex = 32;
            clearList.Text = "Clear List";
            clearList.UseVisualStyleBackColor = true;
            clearList.Click += clearList_Click;
            // 
            // addCustomPathButton
            // 
            addCustomPathButton.Location = new Point(448, 168);
            addCustomPathButton.Name = "addCustomPathButton";
            addCustomPathButton.Size = new Size(84, 23);
            addCustomPathButton.TabIndex = 33;
            addCustomPathButton.Text = "Custom Path";
            addCustomPathButton.UseVisualStyleBackColor = true;
            addCustomPathButton.Click += addCustomPathButton_Click;
            // 
            // moveUpButton
            // 
            moveUpButton.Location = new Point(188, 388);
            moveUpButton.Name = "moveUpButton";
            moveUpButton.Size = new Size(68, 23);
            moveUpButton.TabIndex = 34;
            moveUpButton.Text = "Move Up";
            moveUpButton.UseVisualStyleBackColor = true;
            moveUpButton.Click += moveUpButton_Click;
            // 
            // moveDownButton
            // 
            moveDownButton.Location = new Point(256, 388);
            moveDownButton.Name = "moveDownButton";
            moveDownButton.Size = new Size(80, 23);
            moveDownButton.TabIndex = 35;
            moveDownButton.Text = "Move Down";
            moveDownButton.UseVisualStyleBackColor = true;
            moveDownButton.Click += moveDownButton_Click;
            // 
            // ffxivRefreshTimer
            // 
            ffxivRefreshTimer.Enabled = true;
            ffxivRefreshTimer.Interval = 10000;
            ffxivRefreshTimer.Tick += ffxivRefreshTimer_Tick;
            // 
            // generationCooldown
            // 
            generationCooldown.Interval = 1000;
            generationCooldown.Tick += generationCooldown_Tick;
            // 
            // generationType
            // 
            generationType.Anchor = AnchorStyles.Bottom;
            generationType.FormattingEnabled = true;
            generationType.Items.AddRange(new object[] { "Detailed", "Simple", "Dropdown", "Group Is Checkbox" });
            generationType.Location = new Point(82, 607);
            generationType.Name = "generationType";
            generationType.Size = new Size(106, 23);
            generationType.TabIndex = 36;
            generationType.Text = "Detailed";
            generationType.SelectedIndexChanged += generationType_SelectedIndexChanged;
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Bottom;
            label5.AutoSize = true;
            label5.Location = new Point(4, 611);
            label5.Name = "label5";
            label5.Size = new Size(71, 15);
            label5.TabIndex = 37;
            label5.Text = "Choice Type";
            // 
            // exportProgress
            // 
            exportProgress.Location = new Point(0, 604);
            exportProgress.Name = "exportProgress";
            exportProgress.Size = new Size(536, 32);
            exportProgress.Style = ProgressBarStyle.Continuous;
            exportProgress.TabIndex = 38;
            exportProgress.Visible = false;
            // 
            // bakeNormals
            // 
            bakeNormals.Anchor = AnchorStyles.Bottom;
            bakeNormals.AutoSize = true;
            bakeNormals.Location = new Point(192, 611);
            bakeNormals.Name = "bakeNormals";
            bakeNormals.Size = new Size(116, 19);
            bakeNormals.TabIndex = 39;
            bakeNormals.Text = "Generate Normal";
            bakeNormals.UseVisualStyleBackColor = true;
            bakeNormals.CheckedChanged += bakeMissingNormalsCheckbox_CheckedChanged;
            // 
            // generateMultiCheckBox
            // 
            generateMultiCheckBox.Anchor = AnchorStyles.Bottom;
            generateMultiCheckBox.AutoSize = true;
            generateMultiCheckBox.Location = new Point(308, 611);
            generateMultiCheckBox.Name = "generateMultiCheckBox";
            generateMultiCheckBox.Size = new Size(104, 19);
            generateMultiCheckBox.TabIndex = 40;
            generateMultiCheckBox.Text = "Generate Multi";
            generateMultiCheckBox.UseVisualStyleBackColor = true;
            generateMultiCheckBox.CheckedChanged += generateMultiCheckBox_CheckedChanged;
            // 
            // mask
            // 
            mask.CurrentPath = null;
            mask.Enabled = false;
            mask.Filter = "Texture File|*.png;*.dds;*.bmp;**.tex;";
            mask.Index = -1;
            mask.Location = new Point(4, 576);
            mask.Margin = new Padding(4, 3, 4, 3);
            mask.MinimumSize = new Size(300, 28);
            mask.Name = "mask";
            mask.Size = new Size(528, 28);
            mask.TabIndex = 41;
            helperToolTip.SetToolTip(mask, "Used to restrict where generated normal maps actually use generated normals.");
            mask.OnFileSelected += multi_OnFileSelected;
            mask.Enter += multi_Enter;
            mask.Leave += multi_Leave;
            // 
            // discordButton
            // 
            discordButton.BackColor = Color.MediumSlateBlue;
            discordButton.ForeColor = Color.White;
            discordButton.Location = new Point(388, 0);
            discordButton.Name = "discordButton";
            discordButton.Size = new Size(75, 23);
            discordButton.TabIndex = 42;
            discordButton.Text = "Discord";
            discordButton.UseVisualStyleBackColor = false;
            discordButton.Click += discordButton_Click;
            // 
            // faceExtraList
            // 
            faceExtraList.Enabled = false;
            faceExtraList.FormattingEnabled = true;
            faceExtraList.Location = new Point(228, 140);
            faceExtraList.Name = "faceExtraList";
            faceExtraList.Size = new Size(48, 23);
            faceExtraList.TabIndex = 43;
            faceExtraList.Text = "999";
            // 
            // glow
            // 
            glow.CurrentPath = null;
            glow.Enabled = false;
            glow.Filter = "Texture File|*.png;*.dds;*.bmp;**.tex;";
            glow.Index = -1;
            glow.Location = new Point(4, 544);
            glow.Margin = new Padding(4, 3, 4, 3);
            glow.MinimumSize = new Size(300, 28);
            glow.Name = "glow";
            glow.Size = new Size(528, 28);
            glow.TabIndex = 45;
            helperToolTip.SetToolTip(glow, "Used to make the character glow. Use a transparent overlay where you want glow to happen. Similar to a using an overlay.");
            glow.OnFileSelected += multi_OnFileSelected;
            glow.Enter += multi_Enter;
            glow.Leave += multi_Leave;
            // 
            // finalizeButton
            // 
            finalizeButton.Anchor = AnchorStyles.Bottom;
            finalizeButton.Location = new Point(468, 608);
            finalizeButton.Name = "finalizeButton";
            finalizeButton.Size = new Size(64, 24);
            finalizeButton.TabIndex = 46;
            finalizeButton.Text = "Finished";
            finalizeButton.UseVisualStyleBackColor = true;
            finalizeButton.Click += finalizeButton_Click;
            // 
            // auraFaceScalesDropdown
            // 
            auraFaceScalesDropdown.FormattingEnabled = true;
            auraFaceScalesDropdown.Items.AddRange(new object[] { "Vanilla Scales", "Scaleless Vanilla", "Scaleless Varied" });
            auraFaceScalesDropdown.Location = new Point(276, 140);
            auraFaceScalesDropdown.Name = "auraFaceScalesDropdown";
            auraFaceScalesDropdown.Size = new Size(108, 23);
            auraFaceScalesDropdown.TabIndex = 48;
            auraFaceScalesDropdown.Text = "Vanilla Scales";
            // 
            // panel1
            // 
            panel1.BackColor = Color.Lavender;
            panel1.Controls.Add(uniqueAuRa);
            panel1.Location = new Point(0, 108);
            panel1.Name = "panel1";
            panel1.Size = new Size(540, 30);
            panel1.TabIndex = 49;
            // 
            // panel2
            // 
            panel2.BackColor = Color.Honeydew;
            panel2.Controls.Add(asymCheckbox);
            panel2.Location = new Point(-4, 136);
            panel2.Name = "panel2";
            panel2.Size = new Size(540, 30);
            panel2.TabIndex = 50;
            // 
            // exportLabel
            // 
            exportLabel.Anchor = AnchorStyles.None;
            exportLabel.BackColor = SystemColors.GrayText;
            exportLabel.Font = new Font("Segoe UI", 36F, FontStyle.Bold);
            exportLabel.ForeColor = Color.Snow;
            exportLabel.Location = new Point(0, 264);
            exportLabel.Name = "exportLabel";
            exportLabel.Size = new Size(536, 65);
            exportLabel.TabIndex = 0;
            exportLabel.Text = "Exporting";
            exportLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // exportPanel
            // 
            exportPanel.BackColor = SystemColors.GrayText;
            exportPanel.Controls.Add(exportLabel);
            exportPanel.Location = new Point(0, 0);
            exportPanel.Name = "exportPanel";
            exportPanel.Size = new Size(536, 608);
            exportPanel.TabIndex = 44;
            exportPanel.Visible = false;
            // 
            // listenForFiles
            // 
            listenForFiles.WorkerSupportsCancellation = true;
            listenForFiles.DoWork += listenForFiles_DoWork;
            // 
            // modVersionTextBox
            // 
            modVersionTextBox.Location = new Point(328, 28);
            modVersionTextBox.Name = "modVersionTextBox";
            modVersionTextBox.Size = new Size(40, 23);
            modVersionTextBox.TabIndex = 20;
            modVersionTextBox.Text = "1.0.0";
            modVersionTextBox.TextChanged += modDescriptionTextBox_TextChanged;
            // 
            // ipBox
            // 
            ipBox.Location = new Point(444, 28);
            ipBox.Name = "ipBox";
            ipBox.Size = new Size(88, 23);
            ipBox.TabIndex = 51;
            ipBox.Text = "0.0.0.0";
            ipBox.KeyUp += ipBox_KeyUp;
            ipBox.Leave += ipBox_TextChanged;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(376, 32);
            label8.Name = "label8";
            label8.Size = new Size(61, 15);
            label8.TabIndex = 52;
            label8.Text = "Remote IP";
            // 
            // processGeneration
            // 
            processGeneration.WorkerReportsProgress = true;
            processGeneration.WorkerSupportsCancellation = true;
            processGeneration.DoWork += processGeneration_DoWork;
            processGeneration.ProgressChanged += processGeneration_ProgressChanged;
            processGeneration.RunWorkerCompleted += processGeneration_RunWorkerCompleted;
            // 
            // MainWindow
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(537, 636);
            Controls.Add(generateButton);
            Controls.Add(label8);
            Controls.Add(ipBox);
            Controls.Add(addFaceButton);
            Controls.Add(auraFaceScalesDropdown);
            Controls.Add(finalizeButton);
            Controls.Add(glow);
            Controls.Add(faceExtraList);
            Controls.Add(discordButton);
            Controls.Add(mask);
            Controls.Add(generateMultiCheckBox);
            Controls.Add(bakeNormals);
            Controls.Add(label5);
            Controls.Add(generationType);
            Controls.Add(moveDownButton);
            Controls.Add(moveUpButton);
            Controls.Add(addCustomPathButton);
            Controls.Add(clearList);
            Controls.Add(removeSelection);
            Controls.Add(label6);
            Controls.Add(currentEditLabel);
            Controls.Add(addBodyButton);
            Controls.Add(multi);
            Controls.Add(normal);
            Controls.Add(diffuse);
            Controls.Add(textureList);
            Controls.Add(facePart);
            Controls.Add(donateButton);
            Controls.Add(faceTypeList);
            Controls.Add(label4);
            Controls.Add(subRaceList);
            Controls.Add(genderList);
            Controls.Add(modDescriptionTextBox);
            Controls.Add(raceList);
            Controls.Add(tailList);
            Controls.Add(label3);
            Controls.Add(baseBodyList);
            Controls.Add(modNameTextBox);
            Controls.Add(modVersionTextBox);
            Controls.Add(nameLabel);
            Controls.Add(modAuthorTextBox);
            Controls.Add(label1);
            Controls.Add(modWebsiteTextBox);
            Controls.Add(label2);
            Controls.Add(menuStrip1);
            Controls.Add(exportProgress);
            Controls.Add(panel1);
            Controls.Add(panel2);
            Controls.Add(exportPanel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            MaximizeBox = false;
            Name = "MainWindow";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FFXIV Loose Texture Compiler";
            FormClosing += MainWindow_FormClosing;
            Load += MainForm_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            materialListContextMenu.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            exportPanel.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private ComboBox genderList;
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
        private ComboBox faceTypeList;
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
        private ProgressBar exportProgress;
        private CheckBox bakeNormals;
        private CheckBox generateMultiCheckBox;
        private ToolStripMenuItem bulkReplaceToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem findAndBulkReplaceToolStripMenuItem;
        private FFXIVVoicePackCreator.FilePicker mask;
        private Button discordButton;
        private ComboBox faceExtraList;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem howToGetTexturesToolStripMenuItem;
        private System.Windows.Forms.Timer autoGenerateTImer;
        private Label label7;
        private FFXIVVoicePackCreator.FilePicker glow;
        private ToolStripMenuItem deleteToolStripMenuItem;
        private ToolStripMenuItem omniExportModeToolStripMenuItem;
        private Button finalizeButton;
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
        private ToolStripMenuItem whatIsModshareAndCanIQuicklySendAModToSomebodyElseToolStripMenuItem;
        private ToolStripMenuItem devToolsToolStripMenuItem;
        private ToolStripMenuItem textureToLTCTToolStripMenuItem;
        private ToolStripMenuItem pNGToLTCTToolStripMenuItem;
        private ToolStripMenuItem convertLTCTToPNGToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker processGeneration;
        private ToolStripMenuItem templatesToolStripMenuItem;
        private ToolStripMenuItem importCustomTemplateToolStripMenuItem;
        private ToolStripMenuItem whatAreTemplatesAndHowDoIUseThemToolStripMenuItem;
        private ToolStripMenuItem thisToolIsTooHardMakeItSimplerToolStripMenuItem;
        private ToolStripMenuItem eyeToolsToolStripMenuItem;
        private ToolStripMenuItem convertImageToEyeMultiToolStripMenuItem;
        private ToolStripMenuItem convertImagesToAsymEyeMapsToolStripMenuItem;
        private ToolStripMenuItem convertFolderToEyeMapsToolStripMenuItem;
        private ToolStripMenuItem colourChannelSplittingToolStripMenuItem;
        private ToolStripMenuItem diffuseMergerToolStripMenuItem;
        private ToolStripMenuItem multiCreatorToolStripMenuItem;
        private ToolStripMenuItem mergeRGBAndAlphaImagesToolStripMenuItem;
        private ToolStripMenuItem imageToRGBChannelsToolStripMenuItem;
        private ToolStripMenuItem splitImageToRGBAndAlphaToolStripMenuItem;
        private ToolStripMenuItem xNormalToolStripMenuItem;
        private ToolStripMenuItem imageToTexConversionToolStripMenuItem;
        private ToolStripMenuItem bulkTexViewerToolStripMenuItem;
        private ToolStripMenuItem bulkImageToTexToolStripMenuItem;
        private ToolStripMenuItem recursiveBulkImageToTexToolStripMenuItem;
        private ToolStripMenuItem multiMapToGrayscaleToolStripMenuItem;
        private ToolStripMenuItem howDoIMakeEyesToolStripMenuItem;
        private ToolTip helperToolTip;
        private ToolStripMenuItem textureToBodyMultiToolStripMenuItem;
        private ToolStripMenuItem textureToFaceMultiToolStripMenuItem;
        private ToolStripMenuItem textureToAsymFaceMultiToolStripMenuItem;
        private ToolStripMenuItem duplicateToolStripMenuItem;
        private ToolStripMenuItem bulkNameReplacement;
        private ToolStripMenuItem hairToolsToolStripMenuItem;
        private ToolStripMenuItem hairDiffuseToFFXIVHairMapsToolStripMenuItem;
        private ToolStripMenuItem clothingToolsToolStripMenuItem;
        private ToolStripMenuItem convertDiffuseToNormalAndMultiToolStripMenuItem;

        public ListBox TextureList { get => textureList; set => textureList = value; }
        public ComboBox SubRaceList { get => subRaceList; set => subRaceList = value; }
        public ComboBox FaceType { get => faceTypeList; set => faceTypeList = value; }
        public ComboBox FacePart { get => facePart; set => facePart = value; }
        public ComboBox BaseBodyList { get => baseBodyList; set => baseBodyList = value; }
        public TextBox ModNameTextBox { get => modNameTextBox; set => modNameTextBox = value; }
        public ProgressBar ExportProgress { get => exportProgress; set => exportProgress = value; }
        public ComboBox GenerationType { get => generationType; set => generationType = value; }
        public CheckBox BakeNormals { get => bakeNormals; set => bakeNormals = value; }
    }
}