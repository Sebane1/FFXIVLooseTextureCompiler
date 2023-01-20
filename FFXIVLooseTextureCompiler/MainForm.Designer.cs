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
            this.genderListBody = new System.Windows.Forms.ComboBox();
            this.raceList = new System.Windows.Forms.ComboBox();
            this.tailList = new System.Windows.Forms.ComboBox();
            this.baseBodyList = new System.Windows.Forms.ComboBox();
            this.generateButton = new System.Windows.Forms.Button();
            this.tabPages = new System.Windows.Forms.TabControl();
            this.bodyPage = new System.Windows.Forms.TabPage();
            this.uniqueAuRa = new System.Windows.Forms.CheckBox();
            this.multiB = new FFXIVVoicePackCreator.FilePicker();
            this.normalB = new FFXIVVoicePackCreator.FilePicker();
            this.diffuseB = new FFXIVVoicePackCreator.FilePicker();
            this.facePage = new System.Windows.Forms.TabPage();
            this.asymCheckbox = new System.Windows.Forms.CheckBox();
            this.multiF = new FFXIVVoicePackCreator.FilePicker();
            this.normalF = new FFXIVVoicePackCreator.FilePicker();
            this.diffuseF = new FFXIVVoicePackCreator.FilePicker();
            this.facePart = new System.Windows.Forms.ComboBox();
            this.faceType = new System.Windows.Forms.ComboBox();
            this.subRaceList = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.modDescriptionTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.modVersionTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.modWebsiteTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.modAuthorTextBox = new System.Windows.Forms.TextBox();
            this.nameLabel = new System.Windows.Forms.Label();
            this.modNameTextBox = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.configToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changePenumbraPathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.donateButton = new System.Windows.Forms.Button();
            this.tabPages.SuspendLayout();
            this.bodyPage.SuspendLayout();
            this.facePage.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // genderListBody
            // 
            this.genderListBody.FormattingEnabled = true;
            this.genderListBody.Items.AddRange(new object[] {
            "Masculine",
            "Feminine"});
            this.genderListBody.Location = new System.Drawing.Point(120, 4);
            this.genderListBody.Name = "genderListBody";
            this.genderListBody.Size = new System.Drawing.Size(112, 23);
            this.genderListBody.TabIndex = 1;
            this.genderListBody.SelectedIndexChanged += new System.EventHandler(this.genderList_SelectedIndexChanged);
            // 
            // raceList
            // 
            this.raceList.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.raceList.FormattingEnabled = true;
            this.raceList.Items.AddRange(new object[] {
            "Midlander",
            "Highlander",
            "Elezen",
            "Miqo\'te",
            "Roegadyn",
            "Lalalfell",
            "Raen",
            "Xaela",
            "Hrothgar",
            "Viera"});
            this.raceList.Location = new System.Drawing.Point(236, 4);
            this.raceList.Name = "raceList";
            this.raceList.Size = new System.Drawing.Size(108, 23);
            this.raceList.TabIndex = 2;
            this.raceList.SelectedIndexChanged += new System.EventHandler(this.raceList_SelectedIndexChanged);
            // 
            // tailList
            // 
            this.tailList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
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
            this.tailList.Location = new System.Drawing.Point(348, 4);
            this.tailList.Name = "tailList";
            this.tailList.Size = new System.Drawing.Size(76, 23);
            this.tailList.TabIndex = 3;
            // 
            // baseBodyList
            // 
            this.baseBodyList.FormattingEnabled = true;
            this.baseBodyList.Items.AddRange(new object[] {
            "Vanilla",
            "BIBO+",
            "EVE",
            "GEN3",
            "SCALES+",
            "TBSE/HRBODY",
            "TAIL"});
            this.baseBodyList.Location = new System.Drawing.Point(4, 4);
            this.baseBodyList.Name = "baseBodyList";
            this.baseBodyList.Size = new System.Drawing.Size(112, 23);
            this.baseBodyList.TabIndex = 4;
            this.baseBodyList.SelectedIndexChanged += new System.EventHandler(this.baseBodyList_SelectedIndexChanged);
            // 
            // generateButton
            // 
            this.generateButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.generateButton.Location = new System.Drawing.Point(12, 279);
            this.generateButton.Name = "generateButton";
            this.generateButton.Size = new System.Drawing.Size(428, 23);
            this.generateButton.TabIndex = 7;
            this.generateButton.Text = "Generate";
            this.generateButton.UseVisualStyleBackColor = true;
            this.generateButton.Click += new System.EventHandler(this.generateButton_Click);
            // 
            // tabPages
            // 
            this.tabPages.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabPages.Controls.Add(this.bodyPage);
            this.tabPages.Controls.Add(this.facePage);
            this.tabPages.Location = new System.Drawing.Point(8, 28);
            this.tabPages.Name = "tabPages";
            this.tabPages.SelectedIndex = 0;
            this.tabPages.Size = new System.Drawing.Size(436, 172);
            this.tabPages.TabIndex = 11;
            // 
            // bodyPage
            // 
            this.bodyPage.Controls.Add(this.uniqueAuRa);
            this.bodyPage.Controls.Add(this.multiB);
            this.bodyPage.Controls.Add(this.normalB);
            this.bodyPage.Controls.Add(this.diffuseB);
            this.bodyPage.Controls.Add(this.genderListBody);
            this.bodyPage.Controls.Add(this.raceList);
            this.bodyPage.Controls.Add(this.tailList);
            this.bodyPage.Controls.Add(this.baseBodyList);
            this.bodyPage.Location = new System.Drawing.Point(4, 24);
            this.bodyPage.Name = "bodyPage";
            this.bodyPage.Padding = new System.Windows.Forms.Padding(3);
            this.bodyPage.Size = new System.Drawing.Size(428, 144);
            this.bodyPage.TabIndex = 0;
            this.bodyPage.Text = "Body";
            this.bodyPage.UseVisualStyleBackColor = true;
            // 
            // uniqueAuRa
            // 
            this.uniqueAuRa.AutoSize = true;
            this.uniqueAuRa.Location = new System.Drawing.Point(328, 32);
            this.uniqueAuRa.Name = "uniqueAuRa";
            this.uniqueAuRa.Size = new System.Drawing.Size(98, 19);
            this.uniqueAuRa.TabIndex = 20;
            this.uniqueAuRa.Text = "Unique Au Ra";
            this.uniqueAuRa.UseVisualStyleBackColor = true;
            // 
            // multiB
            // 
            this.multiB.Filter = null;
            this.multiB.Index = -1;
            this.multiB.Location = new System.Drawing.Point(4, 112);
            this.multiB.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.multiB.MinimumSize = new System.Drawing.Size(300, 28);
            this.multiB.Name = "multiB";
            this.multiB.Size = new System.Drawing.Size(420, 28);
            this.multiB.TabIndex = 19;
            // 
            // normalB
            // 
            this.normalB.Filter = null;
            this.normalB.Index = -1;
            this.normalB.Location = new System.Drawing.Point(4, 80);
            this.normalB.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.normalB.MinimumSize = new System.Drawing.Size(300, 28);
            this.normalB.Name = "normalB";
            this.normalB.Size = new System.Drawing.Size(420, 28);
            this.normalB.TabIndex = 18;
            // 
            // diffuseB
            // 
            this.diffuseB.Filter = null;
            this.diffuseB.Index = -1;
            this.diffuseB.Location = new System.Drawing.Point(4, 48);
            this.diffuseB.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.diffuseB.MinimumSize = new System.Drawing.Size(300, 28);
            this.diffuseB.Name = "diffuseB";
            this.diffuseB.Size = new System.Drawing.Size(420, 28);
            this.diffuseB.TabIndex = 17;
            // 
            // facePage
            // 
            this.facePage.Controls.Add(this.asymCheckbox);
            this.facePage.Controls.Add(this.multiF);
            this.facePage.Controls.Add(this.normalF);
            this.facePage.Controls.Add(this.diffuseF);
            this.facePage.Controls.Add(this.facePart);
            this.facePage.Controls.Add(this.faceType);
            this.facePage.Controls.Add(this.subRaceList);
            this.facePage.Location = new System.Drawing.Point(4, 24);
            this.facePage.Name = "facePage";
            this.facePage.Padding = new System.Windows.Forms.Padding(3);
            this.facePage.Size = new System.Drawing.Size(192, 72);
            this.facePage.TabIndex = 1;
            this.facePage.Text = "Face";
            this.facePage.UseVisualStyleBackColor = true;
            // 
            // asymCheckbox
            // 
            this.asymCheckbox.AutoSize = true;
            this.asymCheckbox.Location = new System.Drawing.Point(328, 32);
            this.asymCheckbox.Name = "asymCheckbox";
            this.asymCheckbox.Size = new System.Drawing.Size(99, 19);
            this.asymCheckbox.TabIndex = 23;
            this.asymCheckbox.Text = "Asymmetrical";
            this.asymCheckbox.UseVisualStyleBackColor = true;
            // 
            // multiF
            // 
            this.multiF.Filter = null;
            this.multiF.Index = -1;
            this.multiF.Location = new System.Drawing.Point(4, 112);
            this.multiF.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.multiF.MinimumSize = new System.Drawing.Size(300, 28);
            this.multiF.Name = "multiF";
            this.multiF.Size = new System.Drawing.Size(420, 28);
            this.multiF.TabIndex = 22;
            // 
            // normalF
            // 
            this.normalF.Filter = null;
            this.normalF.Index = -1;
            this.normalF.Location = new System.Drawing.Point(4, 80);
            this.normalF.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.normalF.MinimumSize = new System.Drawing.Size(300, 28);
            this.normalF.Name = "normalF";
            this.normalF.Size = new System.Drawing.Size(420, 28);
            this.normalF.TabIndex = 21;
            // 
            // diffuseF
            // 
            this.diffuseF.Filter = null;
            this.diffuseF.Index = -1;
            this.diffuseF.Location = new System.Drawing.Point(4, 48);
            this.diffuseF.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.diffuseF.MinimumSize = new System.Drawing.Size(300, 28);
            this.diffuseF.Name = "diffuseF";
            this.diffuseF.Size = new System.Drawing.Size(420, 28);
            this.diffuseF.TabIndex = 20;
            // 
            // facePart
            // 
            this.facePart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.facePart.FormattingEnabled = true;
            this.facePart.Items.AddRange(new object[] {
            "Face",
            "Eyebrows",
            "Eyes",
            "Ears"});
            this.facePart.Location = new System.Drawing.Point(40, 4);
            this.facePart.Name = "facePart";
            this.facePart.Size = new System.Drawing.Size(144, 23);
            this.facePart.TabIndex = 4;
            // 
            // faceType
            // 
            this.faceType.FormattingEnabled = true;
            this.faceType.Items.AddRange(new object[] {
            "Face 1",
            "Face 2",
            "Face 3",
            "Face 4"});
            this.faceType.Location = new System.Drawing.Point(140, 4);
            this.faceType.Name = "faceType";
            this.faceType.Size = new System.Drawing.Size(132, 23);
            this.faceType.TabIndex = 3;
            // 
            // subRaceList
            // 
            this.subRaceList.FormattingEnabled = true;
            this.subRaceList.Items.AddRange(new object[] {
            "Midlander",
            "Highlander",
            "Wildwood",
            "Duskwight",
            "Seeker Of The Sun",
            "Keeper Of The Moon",
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
            this.subRaceList.Location = new System.Drawing.Point(8, 4);
            this.subRaceList.Name = "subRaceList";
            this.subRaceList.Size = new System.Drawing.Size(128, 23);
            this.subRaceList.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 256);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 15);
            this.label4.TabIndex = 23;
            this.label4.Text = "Description";
            // 
            // modDescriptionTextBox
            // 
            this.modDescriptionTextBox.Location = new System.Drawing.Point(96, 252);
            this.modDescriptionTextBox.Name = "modDescriptionTextBox";
            this.modDescriptionTextBox.Size = new System.Drawing.Size(336, 23);
            this.modDescriptionTextBox.TabIndex = 22;
            this.modDescriptionTextBox.Text = "Exported by FFXIV Loose Texture Compiler";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(256, 232);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 15);
            this.label3.TabIndex = 21;
            this.label3.Text = "Version";
            // 
            // modVersionTextBox
            // 
            this.modVersionTextBox.Location = new System.Drawing.Point(328, 228);
            this.modVersionTextBox.Name = "modVersionTextBox";
            this.modVersionTextBox.Size = new System.Drawing.Size(104, 23);
            this.modVersionTextBox.TabIndex = 20;
            this.modVersionTextBox.Text = "1.0.0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(256, 204);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 15);
            this.label2.TabIndex = 16;
            this.label2.Text = "Website";
            // 
            // modWebsiteTextBox
            // 
            this.modWebsiteTextBox.Location = new System.Drawing.Point(328, 200);
            this.modWebsiteTextBox.Name = "modWebsiteTextBox";
            this.modWebsiteTextBox.Size = new System.Drawing.Size(104, 23);
            this.modWebsiteTextBox.TabIndex = 15;
            this.modWebsiteTextBox.Text = "https://github.com/Sebane1/FFXIVLooseTextureCompiler";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 232);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 15);
            this.label1.TabIndex = 14;
            this.label1.Text = "Author";
            // 
            // modAuthorTextBox
            // 
            this.modAuthorTextBox.Location = new System.Drawing.Point(96, 228);
            this.modAuthorTextBox.Name = "modAuthorTextBox";
            this.modAuthorTextBox.Size = new System.Drawing.Size(148, 23);
            this.modAuthorTextBox.TabIndex = 13;
            this.modAuthorTextBox.Text = "FFXIV Loose Texture Compiler";
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(20, 204);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(39, 15);
            this.nameLabel.TabIndex = 12;
            this.nameLabel.Text = "Name";
            // 
            // modNameTextBox
            // 
            this.modNameTextBox.Location = new System.Drawing.Point(96, 200);
            this.modNameTextBox.Name = "modNameTextBox";
            this.modNameTextBox.Size = new System.Drawing.Size(148, 23);
            this.modNameTextBox.TabIndex = 11;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(455, 24);
            this.menuStrip1.TabIndex = 24;
            this.menuStrip1.Text = "menuStrip1";
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
            // donateButton
            // 
            this.donateButton.BackColor = System.Drawing.Color.IndianRed;
            this.donateButton.ForeColor = System.Drawing.Color.White;
            this.donateButton.Location = new System.Drawing.Point(380, 0);
            this.donateButton.Name = "donateButton";
            this.donateButton.Size = new System.Drawing.Size(75, 23);
            this.donateButton.TabIndex = 25;
            this.donateButton.Text = "Donate";
            this.donateButton.UseVisualStyleBackColor = false;
            this.donateButton.Click += new System.EventHandler(this.donateButton_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(455, 306);
            this.Controls.Add(this.donateButton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.generateButton);
            this.Controls.Add(this.modDescriptionTextBox);
            this.Controls.Add(this.tabPages);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.modNameTextBox);
            this.Controls.Add(this.modVersionTextBox);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.modAuthorTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.modWebsiteTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainWindow";
            this.Text = "FFXIV Loose Texture Compiler Alpha";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabPages.ResumeLayout(false);
            this.bodyPage.ResumeLayout(false);
            this.bodyPage.PerformLayout();
            this.facePage.ResumeLayout(false);
            this.facePage.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ComboBox genderListBody;
        private ComboBox raceList;
        private ComboBox tailList;
        private ComboBox baseBodyList;
        private Button generateButton;
        private TabControl tabPages;
        private TabPage bodyPage;
        private TabPage facePage;
        private Label label1;
        private TextBox modAuthorTextBox;
        private Label nameLabel;
        private TextBox modNameTextBox;
        private Label label2;
        private TextBox modWebsiteTextBox;
        private FFXIVVoicePackCreator.FilePicker multiB;
        private FFXIVVoicePackCreator.FilePicker normalB;
        private FFXIVVoicePackCreator.FilePicker diffuseB;
        private Label label4;
        private TextBox modDescriptionTextBox;
        private Label label3;
        private TextBox modVersionTextBox;
        private FFXIVVoicePackCreator.FilePicker multiF;
        private FFXIVVoicePackCreator.FilePicker normalF;
        private FFXIVVoicePackCreator.FilePicker diffuseF;
        private ComboBox facePart;
        private ComboBox faceType;
        private ComboBox subRaceList;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem configToolStripMenuItem;
        private ToolStripMenuItem changePenumbraPathToolStripMenuItem;
        private Button donateButton;
        private CheckBox asymCheckbox;
        private CheckBox uniqueAuRa;
    }
}