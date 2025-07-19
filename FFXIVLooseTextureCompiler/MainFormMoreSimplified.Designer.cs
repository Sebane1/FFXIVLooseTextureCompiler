namespace FFXIVLooseTextureCompiler {
    partial class MainFormMoreSimplified {
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
            wizardPages = new TabControl();
            one = new TabPage();
            gender = new ComboBox();
            genderLabel = new Label();
            clan = new ComboBox();
            clanLabel = new Label();
            two = new TabPage();
            itemChangeQuestions = new CheckedListBox();
            label3 = new Label();
            four = new TabPage();
            noSelection = new Label();
            bodyLabel = new Label();
            faceLabel = new Label();
            bodyBox = new ComboBox();
            faceBox = new ComboBox();
            label7 = new Label();
            body = new FFXIVVoicePackCreator.FilePicker();
            face = new FFXIVVoicePackCreator.FilePicker();
            eyes = new FFXIVVoicePackCreator.FilePicker();
            five = new TabPage();
            universalPathingCompatibility = new Button();
            exportButton = new Button();
            label8 = new Label();
            modNameTextBox = new TextBox();
            nextButton = new Button();
            previousButton = new Button();
            discordButton = new Button();
            donateButton = new Button();
            wizardPages.SuspendLayout();
            one.SuspendLayout();
            two.SuspendLayout();
            four.SuspendLayout();
            five.SuspendLayout();
            SuspendLayout();
            // 
            // wizardPages
            // 
            wizardPages.Controls.Add(one);
            wizardPages.Controls.Add(two);
            wizardPages.Controls.Add(four);
            wizardPages.Controls.Add(five);
            wizardPages.Location = new Point(4, 4);
            wizardPages.Name = "wizardPages";
            wizardPages.SelectedIndex = 0;
            wizardPages.Size = new Size(529, 247);
            wizardPages.TabIndex = 0;
            wizardPages.SelectedIndexChanged += wizardPages_SelectedIndexChanged;
            // 
            // one
            // 
            one.Controls.Add(gender);
            one.Controls.Add(genderLabel);
            one.Controls.Add(clan);
            one.Controls.Add(clanLabel);
            one.Location = new Point(4, 24);
            one.Name = "one";
            one.Padding = new Padding(3);
            one.Size = new Size(521, 219);
            one.TabIndex = 0;
            one.Text = "Pick Traits";
            one.UseVisualStyleBackColor = true;
            // 
            // gender
            // 
            gender.FormattingEnabled = true;
            gender.Location = new Point(6, 70);
            gender.Name = "gender";
            gender.Size = new Size(509, 23);
            gender.TabIndex = 3;
            gender.Text = "Masculine";
            gender.SelectedIndexChanged += gender_SelectedIndexChanged;
            // 
            // genderLabel
            // 
            genderLabel.AutoSize = true;
            genderLabel.Font = new Font("Segoe UI", 27.75F, FontStyle.Bold);
            genderLabel.Location = new Point(2, 17);
            genderLabel.Name = "genderLabel";
            genderLabel.Size = new Size(262, 50);
            genderLabel.TabIndex = 2;
            genderLabel.Text = "Select Gender";
            genderLabel.TextAlign = ContentAlignment.MiddleCenter;
            genderLabel.Click += genderLabel_Click;
            genderLabel.Resize += label_Resize;
            // 
            // clan
            // 
            clan.FormattingEnabled = true;
            clan.Items.AddRange(new object[] { "Midlander", "Highlander", "Wildwood", "Duskwight", "Plainsfolk", "Dunesfolk", "Seeker", "Keeper", "Sea Wolf", "Hellsguard", "Raen", "Xaela", "Helion", "The Lost", "Rava", "Veena" });
            clan.Location = new Point(6, 153);
            clan.Name = "clan";
            clan.Size = new Size(509, 23);
            clan.TabIndex = 1;
            clan.Text = "Midlander";
            clan.SelectedIndexChanged += clan_SelectedIndexChanged;
            // 
            // clanLabel
            // 
            clanLabel.AutoSize = true;
            clanLabel.Font = new Font("Segoe UI", 27.75F, FontStyle.Bold);
            clanLabel.Location = new Point(5, 100);
            clanLabel.Name = "clanLabel";
            clanLabel.Size = new Size(212, 50);
            clanLabel.TabIndex = 0;
            clanLabel.Text = "Select Clan";
            clanLabel.TextAlign = ContentAlignment.MiddleCenter;
            clanLabel.Click += clanLabel_Click;
            clanLabel.Resize += label_Resize;
            // 
            // two
            // 
            two.Controls.Add(itemChangeQuestions);
            two.Controls.Add(label3);
            two.Location = new Point(4, 24);
            two.Name = "two";
            two.Padding = new Padding(3);
            two.Size = new Size(521, 219);
            two.TabIndex = 1;
            two.Text = "Pick Items";
            two.UseVisualStyleBackColor = true;
            // 
            // itemChangeQuestions
            // 
            itemChangeQuestions.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            itemChangeQuestions.FormattingEnabled = true;
            itemChangeQuestions.Items.AddRange(new object[] { "Eyes", "Face", "Body" });
            itemChangeQuestions.Location = new Point(6, 61);
            itemChangeQuestions.Name = "itemChangeQuestions";
            itemChangeQuestions.Size = new Size(509, 154);
            itemChangeQuestions.TabIndex = 4;
            itemChangeQuestions.ItemCheck += itemChangeQuestions_ItemCheck;
            itemChangeQuestions.SelectedIndexChanged += itemChangeQuestions_SelectedIndexChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 27.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(0, 3);
            label3.Name = "label3";
            label3.Size = new Size(409, 50);
            label3.TabIndex = 3;
            label3.Text = "What Will You Change";
            label3.TextAlign = ContentAlignment.MiddleCenter;
            label3.Click += label3_Click;
            // 
            // four
            // 
            four.Controls.Add(noSelection);
            four.Controls.Add(bodyLabel);
            four.Controls.Add(faceLabel);
            four.Controls.Add(bodyBox);
            four.Controls.Add(faceBox);
            four.Controls.Add(label7);
            four.Controls.Add(body);
            four.Controls.Add(face);
            four.Controls.Add(eyes);
            four.Location = new Point(4, 24);
            four.Name = "four";
            four.Size = new Size(521, 219);
            four.TabIndex = 2;
            four.Text = "Select Textures";
            four.UseVisualStyleBackColor = true;
            // 
            // noSelection
            // 
            noSelection.AutoSize = true;
            noSelection.Font = new Font("Segoe UI", 26.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            noSelection.ForeColor = Color.Red;
            noSelection.Location = new Point(5, 88);
            noSelection.Name = "noSelection";
            noSelection.Size = new Size(290, 47);
            noSelection.TabIndex = 11;
            noSelection.Text = "No Items Picked";
            noSelection.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // bodyLabel
            // 
            bodyLabel.AutoSize = true;
            bodyLabel.Location = new Point(8, 158);
            bodyLabel.Name = "bodyLabel";
            bodyLabel.Size = new Size(61, 15);
            bodyLabel.TabIndex = 10;
            bodyLabel.Text = "Body Type";
            // 
            // faceLabel
            // 
            faceLabel.AutoSize = true;
            faceLabel.Location = new Point(8, 65);
            faceLabel.Name = "faceLabel";
            faceLabel.Size = new Size(58, 15);
            faceLabel.TabIndex = 9;
            faceLabel.Text = "Face Type";
            // 
            // bodyBox
            // 
            bodyBox.FormattingEnabled = true;
            bodyBox.Location = new Point(83, 155);
            bodyBox.Name = "bodyBox";
            bodyBox.Size = new Size(437, 23);
            bodyBox.TabIndex = 8;
            bodyBox.Text = "Bibo+";
            bodyBox.SelectedIndexChanged += bodyBox_SelectedIndexChanged;
            // 
            // faceBox
            // 
            faceBox.FormattingEnabled = true;
            faceBox.Location = new Point(83, 62);
            faceBox.Name = "faceBox";
            faceBox.Size = new Size(432, 23);
            faceBox.TabIndex = 7;
            faceBox.Text = "Face 1";
            faceBox.SelectedIndexChanged += faceBox_SelectedIndexChanged;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 27.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label7.Location = new Point(0, 0);
            label7.Name = "label7";
            label7.Size = new Size(442, 50);
            label7.TabIndex = 6;
            label7.Text = "Drag And Drop Textures";
            label7.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // body
            // 
            body.BackColor = Color.FromArgb(255, 192, 192);
            body.CurrentPath = null;
            body.Filter = null;
            body.Index = -1;
            body.IsMaterial = false;
            body.Location = new Point(5, 183);
            body.Margin = new Padding(4, 3, 4, 3);
            body.MinimumSize = new Size(300, 28);
            body.Name = "body";
            body.Size = new Size(516, 28);
            body.TabIndex = 2;
            body.OnFileSelected += body_OnFileSelected;
            // 
            // face
            // 
            face.BackColor = Color.FromArgb(255, 192, 192);
            face.CurrentPath = null;
            face.Filter = null;
            face.Index = -1;
            face.IsMaterial = false;
            face.Location = new Point(5, 121);
            face.Margin = new Padding(4, 3, 4, 3);
            face.MinimumSize = new Size(300, 28);
            face.Name = "face";
            face.Size = new Size(516, 28);
            face.TabIndex = 1;
            face.OnFileSelected += face_OnFileSelected;
            // 
            // eyes
            // 
            eyes.BackColor = Color.Gainsboro;
            eyes.CurrentPath = "";
            eyes.Filter = null;
            eyes.Index = -1;
            eyes.IsMaterial = false;
            eyes.Location = new Point(5, 89);
            eyes.Margin = new Padding(4, 3, 4, 3);
            eyes.MinimumSize = new Size(300, 28);
            eyes.Name = "eyes";
            eyes.Size = new Size(516, 28);
            eyes.TabIndex = 0;
            eyes.OnFileSelected += eye_OnFileSelected;
            eyes.Load += eye_Load;
            // 
            // five
            // 
            five.Controls.Add(universalPathingCompatibility);
            five.Controls.Add(exportButton);
            five.Controls.Add(label8);
            five.Controls.Add(modNameTextBox);
            five.Location = new Point(4, 24);
            five.Name = "five";
            five.Size = new Size(521, 219);
            five.TabIndex = 3;
            five.Text = "Export";
            five.UseVisualStyleBackColor = true;
            // 
            // universalPathingCompatibility
            // 
            universalPathingCompatibility.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            universalPathingCompatibility.Location = new Point(4, 171);
            universalPathingCompatibility.Name = "universalPathingCompatibility";
            universalPathingCompatibility.Size = new Size(511, 36);
            universalPathingCompatibility.TabIndex = 8;
            universalPathingCompatibility.Text = "Universal Pathing Compatibility Notice";
            universalPathingCompatibility.UseVisualStyleBackColor = true;
            universalPathingCompatibility.Click += universalPathingCompatibility_Click;
            // 
            // exportButton
            // 
            exportButton.Font = new Font("Segoe UI", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            exportButton.Location = new Point(4, 92);
            exportButton.Name = "exportButton";
            exportButton.Size = new Size(511, 63);
            exportButton.TabIndex = 3;
            exportButton.Text = "Export";
            exportButton.UseVisualStyleBackColor = true;
            exportButton.Click += exportButton_Click;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 27.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label8.Location = new Point(0, 0);
            label8.Name = "label8";
            label8.Size = new Size(317, 50);
            label8.TabIndex = 7;
            label8.Text = "Enter Mod Name";
            label8.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // modNameTextBox
            // 
            modNameTextBox.Location = new Point(4, 53);
            modNameTextBox.Name = "modNameTextBox";
            modNameTextBox.Size = new Size(511, 23);
            modNameTextBox.TabIndex = 0;
            modNameTextBox.TextChanged += modNameTextBox_TextChanged;
            // 
            // nextButton
            // 
            nextButton.Location = new Point(379, 249);
            nextButton.Name = "nextButton";
            nextButton.Size = new Size(154, 23);
            nextButton.TabIndex = 1;
            nextButton.Text = "Next Step";
            nextButton.UseVisualStyleBackColor = true;
            nextButton.Click += nextButton_Click;
            // 
            // previousButton
            // 
            previousButton.Location = new Point(221, 249);
            previousButton.Name = "previousButton";
            previousButton.Size = new Size(157, 23);
            previousButton.TabIndex = 2;
            previousButton.Text = "Previous Step";
            previousButton.UseVisualStyleBackColor = true;
            previousButton.Click += previousButton_Click;
            // 
            // discordButton
            // 
            discordButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            discordButton.BackColor = Color.SlateBlue;
            discordButton.ForeColor = Color.White;
            discordButton.Location = new Point(4, 249);
            discordButton.Name = "discordButton";
            discordButton.Size = new Size(111, 23);
            discordButton.TabIndex = 18;
            discordButton.Text = "Discord";
            discordButton.UseVisualStyleBackColor = false;
            discordButton.Click += discordButton_Click;
            // 
            // donateButton
            // 
            donateButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            donateButton.BackColor = Color.LightCoral;
            donateButton.ForeColor = Color.White;
            donateButton.Location = new Point(113, 249);
            donateButton.Name = "donateButton";
            donateButton.Size = new Size(105, 23);
            donateButton.TabIndex = 17;
            donateButton.Text = "Donate";
            donateButton.UseVisualStyleBackColor = false;
            donateButton.Click += donateButton_Click;
            // 
            // MainFormMoreSimplified
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(535, 274);
            Controls.Add(discordButton);
            Controls.Add(donateButton);
            Controls.Add(previousButton);
            Controls.Add(nextButton);
            Controls.Add(wizardPages);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            MaximizeBox = false;
            Name = "MainFormMoreSimplified";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Loose Texture Compiler (Wizard)";
            FormClosed += MainFormMoreSimplified_FormClosed;
            Load += MainFormMoreSimplified_Load;
            wizardPages.ResumeLayout(false);
            one.ResumeLayout(false);
            one.PerformLayout();
            two.ResumeLayout(false);
            two.PerformLayout();
            four.ResumeLayout(false);
            four.PerformLayout();
            five.ResumeLayout(false);
            five.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TabControl wizardPages;
        private TabPage one;
        private ComboBox clan;
        private Label clanLabel;
        private TabPage two;
        private ComboBox gender;
        private Label genderLabel;
        private CheckedListBox itemChangeQuestions;
        private Label label3;
        private TabPage four;
        private FFXIVVoicePackCreator.FilePicker eyes;
        private Button nextButton;
        private Button previousButton;
        private FFXIVVoicePackCreator.FilePicker body;
        private FFXIVVoicePackCreator.FilePicker face;
        private Label label7;
        private TabPage five;
        private Button exportButton;
        private Label label8;
        private TextBox modNameTextBox;
        private Button discordButton;
        private Button donateButton;
        private ComboBox faceBox;
        private ComboBox bodyBox;
        private Label bodyLabel;
        private Label faceLabel;
        private Label noSelection;
        private Button universalPathingCompatibility;
    }
}