using FFXIVLooseTextureCompiler.ImageProcessing;
using FFXIVLooseTextureCompiler.PathOrganization;
using FFXIVLooseTextureCompiler.Racial;
using FFXIVVoicePackCreator;
using System.Diagnostics;

namespace FFXIVLooseTextureCompiler {
    public partial class MainFormSimplified : Form {
        public MainFormSimplified() {
            InitializeComponent();
        }
        List<TextureSet> textureSets = new List<TextureSet>();
        MainWindow mainWindow;
        TextureSet skinTextureSet = new TextureSet();
        TextureSet faceTextureSet = new TextureSet();
        TextureSet eyesTextureSet = new TextureSet();

        private bool ignoreTitleChange;
        private FilePicker _skin;
        private FilePicker _face;
        private FilePicker _eyes;
        private TextBox _modName;
        private ComboBox _faceType;
        private ComboBox _bodyType;
        private ComboBox _clanType;

        public MainWindow MainWindow { get => mainWindow; set => mainWindow = value; }
        public FilePicker Skin { get => _skin; set => _skin = value; }
        public FilePicker Face { get => _face; set => _face = value; }
        public FilePicker Eyes { get => _eyes; set => _eyes = value; }
        public TextBox ModName { get => _modName; set => _modName = value; }
        public ComboBox ClanType { get => _clanType; set => _clanType = value; }

        private void MainWindowSimplified_Load(object sender, EventArgs e) {
            RefreshValues();

            _skin = body;
            _face = face;
            _eyes = eyes;
            _modName = modNameTextBox;
            _faceType = faceType;
            _bodyType = bodyType;
            _clanType = subRace;
        }

        public void RefreshValues() {
            if (Visible) {
                mainWindow.IsSimpleMode = true;
                Text = mainWindow.VersionText;
                mainWindow.BaseBodyList.SelectedIndex = 1;
                if (mainWindow.TextureList.Items.Count == 0) {
                    mainWindow.TextureList.Items.Add(skinTextureSet);
                    mainWindow.TextureList.Items.Add(faceTextureSet);
                    mainWindow.TextureList.Items.Add(eyesTextureSet);
                } else if (mainWindow.TextureList.Items.Count == 3) {
                    skinTextureSet = mainWindow.TextureList.Items[0] as TextureSet;
                    faceTextureSet = mainWindow.TextureList.Items[1] as TextureSet;
                    eyesTextureSet = mainWindow.TextureList.Items[2] as TextureSet;
                } else {
                    return;
                }
                mainWindow.Hide();
                skinTextureSet.OmniExportMode = true;
                skinTextureSet.TextureSetName = "Skin";
                faceTextureSet.TextureSetName = "Face";
                eyesTextureSet.TextureSetName = "Eyes";

                skinTextureSet.GroupName = "Character Customization";
                faceTextureSet.GroupName = "Character Customization";
                eyesTextureSet.GroupName = "Character Customization";

                body.FilePath.Text = skinTextureSet.Base;
                face.FilePath.Text = faceTextureSet.Base;
                eyes.FilePath.Text = eyesTextureSet.Normal;

                ignoreTitleChange = true;
                modNameTextBox.Text = mainWindow.ModNameTextBox.Text;

                mainWindow.AddBodyPaths(skinTextureSet);
                mainWindow.FacePart.SelectedIndex = 0;
                mainWindow.AddFacePaths(faceTextureSet);
                mainWindow.FacePart.SelectedIndex = 2;
                mainWindow.AddEyePaths(eyesTextureSet);
                mainWindow.RefreshList();
                mainWindow.GenerationType.SelectedIndex = 1;
                mainWindow.ExportProgress.VisibleChanged += delegate {
                    exportProgress.Visible = mainWindow.ExportProgress.Visible;
                };
                progressChecker.Start();
            }
        }
        private void SetBodyTypeFromPath() {
            switch (RaceInfo.ReverseBodyLookup(skinTextureSet.Base)) {
                case "bibo":
                    bodyType.SelectedIndex = 0;
                    break;
                case "gen3":
                    bodyType.SelectedIndex = 1;
                    break;
                case "tbse":
                    bodyType.SelectedIndex = 2;
                    break;
                case "otopo":
                    bodyType.SelectedIndex = 3;
                    break;
            }
        }

        public void generateButton_Click(object sender, EventArgs e) {
            MainWindow.finalizeButton_Click(sender, e);
        }

        private void previewButton_Click(object sender, EventArgs e) {
            MainWindow.generateButton_Click(sender, e);
        }

        private void skin_OnFileSelected(object sender, EventArgs e) {
            skinTextureSet.Base = body.FilePath.Text;
            mainWindow.AddWatcher(skinTextureSet.Base);
            mainWindow.HasSaved = false;
        }

        private void face_OnFileSelected(object sender, EventArgs e) {
            faceTextureSet.Base = face.FilePath.Text;
            mainWindow.AddWatcher(faceTextureSet.Base);
            mainWindow.HasSaved = false;
        }

        private void eyes_OnFileSelected(object sender, EventArgs e) {
            eyesTextureSet.Normal = eyes.FilePath.Text;
            mainWindow.AddWatcher(eyesTextureSet.Base);
            mainWindow.HasSaved = false;
        }

        private void bodyType_SelectedIndexChanged(object sender, EventArgs e) {
            switch (bodyType.SelectedIndex) {
                case 0:
                    mainWindow.BaseBodyList.SelectedIndex = 1;
                    break;
                case 1:
                    mainWindow.BaseBodyList.SelectedIndex = 2;
                    break;
                case 2:
                    mainWindow.BaseBodyList.SelectedIndex = 3;
                    break;
                case 3:
                    mainWindow.BaseBodyList.SelectedIndex = 5;
                    break;
            }
            mainWindow.AddBodyPaths(skinTextureSet);
            mainWindow.HasSaved = false;
        }

        private void subRace_SelectedIndexChanged(object sender, EventArgs e) {
            mainWindow.SubRaceList.SelectedIndex = subRace.SelectedIndex;
            subRace.SelectedIndex = mainWindow.SubRaceList.SelectedIndex;
            mainWindow.HasSaved = false;
        }

        private void faceType_SelectedIndexChanged(object sender, EventArgs e) {
            mainWindow.FaceType.SelectedIndex = faceType.SelectedIndex;
            mainWindow.FacePart.SelectedIndex = 0;
            mainWindow.AddFacePaths(faceTextureSet);
            mainWindow.FacePart.SelectedIndex = 2;
            mainWindow.AddEyePaths(eyesTextureSet);
            mainWindow.HasSaved = false;
        }
        private void modNameTextBox_TextChanged(object sender, EventArgs e) {
            if (!ignoreTitleChange && Visible) {
                mainWindow.ModNameTextBox.Text = modNameTextBox.Text;
                mainWindow.HasSaved = false;
            } else {
                ignoreTitleChange = false;
            }
        }

        private void advancedModeButton_Click(object sender, EventArgs e) {
            mainWindow.IsSimpleMode = false;
            mainWindow.Show();
            Hide();
            mainWindow.WriteDefaultMode();
        }

        private void progressChecker_Tick(object sender, EventArgs e) {
            exportPanel.Visible = exportProgress.Visible = mainWindow.LockDuplicateGeneration;
            if (mainWindow.LockDuplicateGeneration) {
                exportPanel.BringToFront();
                exportProgress.BringToFront();
                exportProgress.Maximum = mainWindow.ExportProgress.Maximum;
                exportProgress.Value = mainWindow.ExportProgress.Value;
            }
        }

        private void normalGeneration_SelectedIndexChanged(object sender, EventArgs e) {
            switch (normalGeneration.SelectedIndex) {
                case 0:
                    mainWindow.BakeNormals.Checked = false;
                    skinTextureSet.InvertNormalGeneration = false;
                    faceTextureSet.InvertNormalGeneration = false;
                    break;
                case 1:
                    mainWindow.BakeNormals.Checked = true;
                    skinTextureSet.InvertNormalGeneration = false;
                    faceTextureSet.InvertNormalGeneration = false;
                    break;
                case 2:
                    mainWindow.BakeNormals.Checked = true;
                    skinTextureSet.InvertNormalGeneration = true;
                    faceTextureSet.InvertNormalGeneration = true;
                    break;
            }
            mainWindow.HasSaved = false;
        }
        public void ClearForm() {
            modNameTextBox.Text = "";
            body.FilePath.Text = "";
            face.FilePath.Text = "";
            eyes.FilePath.Text = "";

            body.ClearValue();
            face.ClearValue();
            eyes.ClearValue();
            normalGeneration.SelectedIndex = 0;
        }
        private void newToolStripMenuItem_Click(object sender, EventArgs e) {
            mainWindow.newToolStripMenuItem_Click(sender, e);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e) {
            mainWindow.openToolStripMenuItem_Click(sender, e);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e) {
            mainWindow.saveToolStripMenuItem_Click(sender, e);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e) {
            mainWindow.saveAsToolStripMenuItem_Click(sender, e);
        }

        private void enableModShareToolStripMenuItem_Click(object sender, EventArgs e) {
            mainWindow.enableModshareToolStripMenuItem_Click(sender, e);
        }

        private void changePenumbraPathToolStripMenuItem_Click(object sender, EventArgs e) {
            mainWindow.changePenumbraPathToolStripMenuItem_Click(sender, e);
        }

        private void creditsToolStripMenuItem_Click(object sender, EventArgs e) {
            mainWindow.creditsToolStripMenuItem_Click(sender, e);
        }

        private void MainFormSimplified_Load(object sender, EventArgs e) {
            AutoScaleDimensions = new SizeF(96, 96);
        }

        private void MainFormSimplified_FormClosing(object sender, FormClosingEventArgs e) {
            e.Cancel = true;
            mainWindow.Close();
        }

        public void discordButton_Click(object sender, EventArgs e) {
            mainWindow.discordButton_Click(sender, e);
        }

        public void donateButton_Click(object sender, EventArgs e) {
            mainWindow.donateButton_Click(sender, e);
        }
        private void MainWindow_KeyDown(object sender, KeyEventArgs e) {
            if (e.Control && e.KeyCode == Keys.S) {
                mainWindow.Save();
            }
        }
        protected override bool ProcessCmdKey(ref Message message, Keys keys) {
            switch (keys) {
                case Keys.S | Keys.Control:
                    // ... Process Shift+Ctrl+Alt+B ...
                    mainWindow.Save();
                    return true; // signal that we've processed this key
            }

            // run base implementation
            return base.ProcessCmdKey(ref message, keys);
        }

        private void convertPictureToEyeMultiToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Texture File|*.png;*.tga;*.dds;*.bmp;*.tex;";
            MessageBox.Show("Please select input texture");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                Bitmap image = ImageManipulation.ExtractRed(TexIO.ResolveBitmap(openFileDialog.FileName));
                Bitmap eyeMulti = ImageManipulation.BitmapToEyeBaseDawntrail(image, true);
                TexIO.SaveBitmap(eyeMulti, openFileDialog.FileName.Replace(".", "_eye_texture."));
                MessageBox.Show("Image successfully converted to eye multi", mainWindow.VersionText);
                try {
                    Process.Start(new System.Diagnostics.ProcessStartInfo() {
                        FileName = Path.GetDirectoryName(openFileDialog.FileName),
                        UseShellExecute = true,
                        Verb = "OPEN"
                    });
                } catch {

                }
            }
        }

        private void legacyMakeupSalvagerToolStripMenuItem_Click(object sender, EventArgs e) {
            mainWindow.legacyMakeupSalvagerToolStripMenuItem_Click(sender, e);
        }

        private void exportPanel_Paint(object sender, PaintEventArgs e) {

        }

        private void face_Load(object sender, EventArgs e) {

        }

        private void exportProgress_Click(object sender, EventArgs e) {

        }

        private void easyModeButton_Click(object sender, EventArgs e) {
            //Hide();
            var mainFormMoreSimplified = new MainFormMoreSimplified();
            mainFormMoreSimplified.MainFormSimplified = this;
            mainFormMoreSimplified.ShowDialog();
        }

        private void exportLabel_Click(object sender, EventArgs e) {

        }

        private void universalPathingCompatibilityToolStripMenuItem_Click(object sender, EventArgs e) {
            try {
                Process.Start(new System.Diagnostics.ProcessStartInfo() {
                    FileName = "https://docs.google.com/document/d/1X1VZ16cFFqtlpAodvdWgLKN35ZcZJWpXoKU_-g2vsvk/edit?usp=sharing",
                    UseShellExecute = true,
                    Verb = "OPEN"
                });
            } catch {

            }
        }
    }
}
