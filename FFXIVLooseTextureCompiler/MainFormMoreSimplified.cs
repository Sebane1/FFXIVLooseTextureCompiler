using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FFXIVLooseTextureCompiler {
    public partial class MainFormMoreSimplified : Form {
        private MainFormSimplified _mainFormSimplified;

        public MainFormSimplified MainFormSimplified {
            get {
                return _mainFormSimplified;
            }
            set {
                _mainFormSimplified = value;
                bodyBox.Items.Clear();
                bodyBox.Items.AddRange(_mainFormSimplified.BodyType.Items.OfType<string>().ToArray<string>());
                faceBox.Items.Clear();
                faceBox.Items.AddRange(_mainFormSimplified.FaceType.Items.OfType<string>().ToArray<string>());
                gender.Items.Clear();
                gender.Items.AddRange(_mainFormSimplified.MainWindow.Gender.Items.OfType<string>().ToArray<string>());
                clan.Items.Clear();
                clan.Items.AddRange(_mainFormSimplified.ClanType.Items.OfType<string>().ToArray<string>());
                Text = _mainFormSimplified.MainWindow.VersionText;
            }
        }
        public MainFormMoreSimplified() {
            InitializeComponent();
        }

        private void eye_OnFileSelected(object sender, EventArgs e) {
            MainFormSimplified.Eyes.CurrentPath = eyes.FilePath.Text;
            MainFormSimplified.Eyes.TriggerFileSelected();
        }

        private void face_OnFileSelected(object sender, EventArgs e) {
            MainFormSimplified.Face.CurrentPath = face.FilePath.Text;
            MainFormSimplified.Face.TriggerFileSelected();
        }

        private void body_OnFileSelected(object sender, EventArgs e) {
            MainFormSimplified.Skin.CurrentPath = body.FilePath.Text;
            MainFormSimplified.Skin.TriggerFileSelected();
        }

        private void modNameTextBox_TextChanged(object sender, EventArgs e) {
            MainFormSimplified.ModName.Text = modNameTextBox.Text;
        }

        private void bodyBox_SelectedIndexChanged(object sender, EventArgs e) {
            MainFormSimplified.BodyType.SelectedIndex = bodyBox.SelectedIndex;
        }

        private void faceBox_SelectedIndexChanged(object sender, EventArgs e) {
            MainFormSimplified.FaceType.SelectedIndex = faceBox.SelectedIndex;
        }

        private void clan_SelectedIndexChanged(object sender, EventArgs e) {
            MainFormSimplified.ClanType.SelectedIndex = clan.SelectedIndex;
        }

        private void gender_SelectedIndexChanged(object sender, EventArgs e) {
            MainFormSimplified.MainWindow.Gender.SelectedIndex = gender.SelectedIndex;
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e) {

        }

        private void exportButton_Click(object sender, EventArgs e) {
            MainFormSimplified.Show();
            MainFormSimplified.generateButton_Click(sender, e);
            Close();
        }

        private void MainFormMoreSimplified_FormClosed(object sender, FormClosedEventArgs e) {
            MainFormSimplified.Show();
        }

        private void itemChangeQuestions_SelectedIndexChanged(object sender, EventArgs e) {
            RefreshValues();
        }

        private void RefreshValues() {
            eyes.Visible = itemChangeQuestions.CheckedIndices.Contains(0);
            face.Visible = itemChangeQuestions.CheckedIndices.Contains(1);
            faceLabel.Visible = faceBox.Visible = itemChangeQuestions.CheckedIndices.Contains(0) || itemChangeQuestions.CheckedIndices.Contains(1);
            bodyLabel.Visible = body.Visible = bodyBox.Visible = itemChangeQuestions.CheckedIndices.Contains(2);

            noSelection.Visible = !itemChangeQuestions.CheckedIndices.Contains(0)
                && !itemChangeQuestions.CheckedIndices.Contains(1)
                && !itemChangeQuestions.CheckedIndices.Contains(2);
        }

        private void nextButton_Click(object sender, EventArgs e) {
            wizardPages.SelectedIndex = Math.Clamp(wizardPages.SelectedIndex + 1, 0, wizardPages.TabCount);
        }

        private void previousButton_Click(object sender, EventArgs e) {
            wizardPages.SelectedIndex = Math.Clamp(wizardPages.SelectedIndex - 1, 0, wizardPages.TabCount);
        }

        private void MainFormMoreSimplified_Load(object sender, EventArgs e) {
            RefreshValues();
            WFTranslator.TranslateControl(this);
        }

        private void itemChangeQuestions_ItemCheck(object sender, ItemCheckEventArgs e) {
            RefreshValues();
        }

        private void wizardPages_SelectedIndexChanged(object sender, EventArgs e) {
            RefreshValues();
        }

        private void discordButton_Click(object sender, EventArgs e) {
            MainFormSimplified.discordButton_Click(sender, e);
        }

        private void donateButton_Click(object sender, EventArgs e) {
            MainFormSimplified.donateButton_Click(sender, e);
        }

        private void label3_Click(object sender, EventArgs e) {

        }

        private void eye_Load(object sender, EventArgs e) {

        }

        private void universalPathingCompatibility_Click(object sender, EventArgs e) {
            try {
                Process.Start(new System.Diagnostics.ProcessStartInfo() {
                    FileName = "https://docs.google.com/document/d/1X1VZ16cFFqtlpAodvdWgLKN35ZcZJWpXoKU_-g2vsvk/edit?usp=sharing",
                    UseShellExecute = true,
                    Verb = "OPEN"
                });
            } catch {

            }
        }

        private void label_Resize(object sender, EventArgs e) {

        }

        private void genderLabel_Click(object sender, EventArgs e) {

        }

        private void clanLabel_Click(object sender, EventArgs e) {

        }
    }
}
