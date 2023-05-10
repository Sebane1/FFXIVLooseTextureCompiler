using FFXIVLooseTextureCompiler.PathOrganization;
using FFXIVLooseTextureCompiler.Racial;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        public MainWindow MainWindow { get => mainWindow; set => mainWindow = value; }

        private void MainWindowSimplified_Load(object sender, EventArgs e) {
            RefreshValues();
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
                    //mainWindow.Show();
                    //this.Hide();
                    return;
                }
                mainWindow.Hide();
                skinTextureSet.OmniExportMode = true;
                skinTextureSet.MaterialSetName = "Skin";
                faceTextureSet.MaterialSetName = "Face";
                eyesTextureSet.MaterialSetName = "Eyes";

                skinTextureSet.MaterialGroupName = "Character Customization";
                faceTextureSet.MaterialGroupName = "Character Customization";
                eyesTextureSet.MaterialGroupName = "Character Customization";

                skin.FilePath.Text = skinTextureSet.Diffuse;
                face.FilePath.Text = faceTextureSet.Diffuse;
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
            switch (RaceInfo.ReverseBodyLookup(skinTextureSet.Diffuse)) {
                case "bibo":
                    bodyType.SelectedIndex = 0;
                    break;
                case "gen3":
                    bodyType.SelectedIndex = 1;
                    break;
                case "tbse":
                    bodyType.SelectedIndex = 2;
                    break;
                case "otopoo":
                    bodyType.SelectedIndex = 3;
                    break;
            }
        }

        private void generateButton_Click(object sender, EventArgs e) {
            MainWindow.finalizeButton_Click(sender, e);
        }

        private void previewButton_Click(object sender, EventArgs e) {
            MainWindow.generateButton_Click(sender, e);
        }

        private void skin_OnFileSelected(object sender, EventArgs e) {
            skinTextureSet.Diffuse = skin.FilePath.Text;
            mainWindow.AddWatcher(skinTextureSet.Diffuse);
            mainWindow.HasSaved = false;
        }

        private void face_OnFileSelected(object sender, EventArgs e) {
            faceTextureSet.Diffuse = face.FilePath.Text;
            mainWindow.AddWatcher(faceTextureSet.Diffuse);
            mainWindow.HasSaved = false;
        }

        private void eyes_OnFileSelected(object sender, EventArgs e) {
            eyesTextureSet.Normal = eyes.FilePath.Text;
            mainWindow.AddWatcher(eyesTextureSet.Diffuse);
            mainWindow.HasSaved = false;
        }

        private void bodyType_SelectedIndexChanged(object sender, EventArgs e) {
            switch (bodyType.SelectedIndex) {
                case 0:
                    mainWindow.BaseBodyList.SelectedIndex = 1;
                    break;
                case 1:
                    mainWindow.BaseBodyList.SelectedIndex = 3;
                    break;
                case 2:
                    mainWindow.BaseBodyList.SelectedIndex = 5;
                    break;
                case 3:
                    mainWindow.BaseBodyList.SelectedIndex = 7;
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
            exportProgress.Maximum = mainWindow.ExportProgress.Maximum;
            exportProgress.Value = mainWindow.ExportProgress.Value;
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
            skin.FilePath.Text = "";
            face.FilePath.Text = "";
            eyes.FilePath.Text = "";

            skin.ClearValue();
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

        private void discordButton_Click(object sender, EventArgs e) {
            mainWindow.discordButton_Click(sender, e);
        }

        private void donateButton_Click(object sender, EventArgs e) {
            mainWindow.donateButton_Click(sender, e);
        }
    }
}
