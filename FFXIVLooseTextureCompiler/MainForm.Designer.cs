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
            this.genderList = new System.Windows.Forms.ComboBox();
            this.raceList = new System.Windows.Forms.ComboBox();
            this.tailList = new System.Windows.Forms.ComboBox();
            this.baseBodyList = new System.Windows.Forms.ComboBox();
            this.generateButton = new System.Windows.Forms.Button();
            this.tabPages = new System.Windows.Forms.TabControl();
            this.bodyPage = new System.Windows.Forms.TabPage();
            this.label4 = new System.Windows.Forms.Label();
            this.modDescriptionTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.modVersionTextBox = new System.Windows.Forms.TextBox();
            this.multi = new FFXIVVoicePackCreator.FilePicker();
            this.normal = new FFXIVVoicePackCreator.FilePicker();
            this.diffuse = new FFXIVVoicePackCreator.FilePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.modWebsiteTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.modAuthorTextBox = new System.Windows.Forms.TextBox();
            this.nameLabel = new System.Windows.Forms.Label();
            this.modNameTextBox = new System.Windows.Forms.TextBox();
            this.facePage = new System.Windows.Forms.TabPage();
            this.tabPages.SuspendLayout();
            this.bodyPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // genderList
            // 
            this.genderList.FormattingEnabled = true;
            this.genderList.Items.AddRange(new object[] {
            "Masculine",
            "Feminine"});
            this.genderList.Location = new System.Drawing.Point(120, 4);
            this.genderList.Name = "genderList";
            this.genderList.Size = new System.Drawing.Size(112, 23);
            this.genderList.TabIndex = 1;
            this.genderList.SelectedIndexChanged += new System.EventHandler(this.genderList_SelectedIndexChanged);
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
            this.generateButton.Location = new System.Drawing.Point(12, 240);
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
            this.tabPages.Location = new System.Drawing.Point(8, 4);
            this.tabPages.Name = "tabPages";
            this.tabPages.SelectedIndex = 0;
            this.tabPages.Size = new System.Drawing.Size(436, 236);
            this.tabPages.TabIndex = 11;
            // 
            // bodyPage
            // 
            this.bodyPage.Controls.Add(this.label4);
            this.bodyPage.Controls.Add(this.modDescriptionTextBox);
            this.bodyPage.Controls.Add(this.label3);
            this.bodyPage.Controls.Add(this.modVersionTextBox);
            this.bodyPage.Controls.Add(this.multi);
            this.bodyPage.Controls.Add(this.normal);
            this.bodyPage.Controls.Add(this.diffuse);
            this.bodyPage.Controls.Add(this.label2);
            this.bodyPage.Controls.Add(this.modWebsiteTextBox);
            this.bodyPage.Controls.Add(this.label1);
            this.bodyPage.Controls.Add(this.modAuthorTextBox);
            this.bodyPage.Controls.Add(this.nameLabel);
            this.bodyPage.Controls.Add(this.modNameTextBox);
            this.bodyPage.Controls.Add(this.genderList);
            this.bodyPage.Controls.Add(this.raceList);
            this.bodyPage.Controls.Add(this.tailList);
            this.bodyPage.Controls.Add(this.baseBodyList);
            this.bodyPage.Location = new System.Drawing.Point(4, 24);
            this.bodyPage.Name = "bodyPage";
            this.bodyPage.Padding = new System.Windows.Forms.Padding(3);
            this.bodyPage.Size = new System.Drawing.Size(428, 208);
            this.bodyPage.TabIndex = 0;
            this.bodyPage.Text = "Body";
            this.bodyPage.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 188);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 15);
            this.label4.TabIndex = 23;
            this.label4.Text = "Description";
            // 
            // modDescriptionTextBox
            // 
            this.modDescriptionTextBox.Location = new System.Drawing.Point(84, 184);
            this.modDescriptionTextBox.Name = "modDescriptionTextBox";
            this.modDescriptionTextBox.Size = new System.Drawing.Size(336, 23);
            this.modDescriptionTextBox.TabIndex = 22;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(244, 156);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 15);
            this.label3.TabIndex = 21;
            this.label3.Text = "Version";
            // 
            // modVersionTextBox
            // 
            this.modVersionTextBox.Location = new System.Drawing.Point(316, 152);
            this.modVersionTextBox.Name = "modVersionTextBox";
            this.modVersionTextBox.Size = new System.Drawing.Size(104, 23);
            this.modVersionTextBox.TabIndex = 20;
            this.modVersionTextBox.Text = "1.0.0";
            // 
            // multi
            // 
            this.multi.Index = -1;
            this.multi.Location = new System.Drawing.Point(4, 84);
            this.multi.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.multi.MinimumSize = new System.Drawing.Size(300, 28);
            this.multi.Name = "multi";
            this.multi.Size = new System.Drawing.Size(420, 28);
            this.multi.TabIndex = 19;
            // 
            // normal
            // 
            this.normal.Index = -1;
            this.normal.Location = new System.Drawing.Point(4, 56);
            this.normal.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.normal.MinimumSize = new System.Drawing.Size(300, 28);
            this.normal.Name = "normal";
            this.normal.Size = new System.Drawing.Size(420, 28);
            this.normal.TabIndex = 18;
            // 
            // diffuse
            // 
            this.diffuse.Index = -1;
            this.diffuse.Location = new System.Drawing.Point(4, 28);
            this.diffuse.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.diffuse.MinimumSize = new System.Drawing.Size(300, 28);
            this.diffuse.Name = "diffuse";
            this.diffuse.Size = new System.Drawing.Size(420, 28);
            this.diffuse.TabIndex = 17;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(244, 124);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 15);
            this.label2.TabIndex = 16;
            this.label2.Text = "Website";
            // 
            // modWebsiteTextBox
            // 
            this.modWebsiteTextBox.Location = new System.Drawing.Point(316, 120);
            this.modWebsiteTextBox.Name = "modWebsiteTextBox";
            this.modWebsiteTextBox.Size = new System.Drawing.Size(104, 23);
            this.modWebsiteTextBox.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 156);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 15);
            this.label1.TabIndex = 14;
            this.label1.Text = "Author";
            // 
            // modAuthorTextBox
            // 
            this.modAuthorTextBox.Location = new System.Drawing.Point(84, 152);
            this.modAuthorTextBox.Name = "modAuthorTextBox";
            this.modAuthorTextBox.Size = new System.Drawing.Size(148, 23);
            this.modAuthorTextBox.TabIndex = 13;
            this.modAuthorTextBox.Text = "FFXIV Loose Texture Compiler";
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(8, 124);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(39, 15);
            this.nameLabel.TabIndex = 12;
            this.nameLabel.Text = "Name";
            // 
            // modNameTextBox
            // 
            this.modNameTextBox.Location = new System.Drawing.Point(84, 120);
            this.modNameTextBox.Name = "modNameTextBox";
            this.modNameTextBox.Size = new System.Drawing.Size(148, 23);
            this.modNameTextBox.TabIndex = 11;
            // 
            // facePage
            // 
            this.facePage.Location = new System.Drawing.Point(4, 24);
            this.facePage.Name = "facePage";
            this.facePage.Padding = new System.Windows.Forms.Padding(3);
            this.facePage.Size = new System.Drawing.Size(192, 72);
            this.facePage.TabIndex = 1;
            this.facePage.Text = "Face";
            this.facePage.UseVisualStyleBackColor = true;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(455, 267);
            this.Controls.Add(this.generateButton);
            this.Controls.Add(this.tabPages);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "MainWindow";
            this.Text = "FFXIV Loose Texture Compiler";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabPages.ResumeLayout(false);
            this.bodyPage.ResumeLayout(false);
            this.bodyPage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private ComboBox genderList;
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
        private FFXIVVoicePackCreator.FilePicker multi;
        private FFXIVVoicePackCreator.FilePicker normal;
        private FFXIVVoicePackCreator.FilePicker diffuse;
        private Label label4;
        private TextBox modDescriptionTextBox;
        private Label label3;
        private TextBox modVersionTextBox;
    }
}