using FFXIVLooseTextureCompiler.PathOrganization;
namespace FFXIVLooseTextureCompiler {
    public partial class BulkNameReplacement : Form {
        private TextureSet[] _textureSets;

        public BulkNameReplacement(TextureSet[] textureSets) {
            _textureSets = textureSets;
            InitializeComponent();
            AutoScaleDimensions = new SizeF(96, 96);
        }

        private void BulkNameReplacement_Load(object sender, EventArgs e) {
            replacementTypeComboBox.SelectedIndex = 0;
            WFTranslator.TranslateControl(this);
        }

        private void replacementTypeComboBox_SelectedIndexChanged(object sender, EventArgs e) {

        }

        private void replaceBox_TextChanged(object sender, EventArgs e) {

        }

        private void replaceAll_Click(object sender, EventArgs e) {
            if (!string.IsNullOrEmpty(findBox.Text)) {
                switch (replacementTypeComboBox.SelectedIndex) {
                    case 0:
                        foreach (TextureSet textureSet in _textureSets) {
                            if (textureSet.TextureSetName.Contains(findBox.Text)) {
                                textureSet.TextureSetName = textureSet.TextureSetName.Replace(findBox.Text, replaceBox.Text);
                            }
                        }
                        break;
                    case 1:
                        foreach (TextureSet textureSet in _textureSets) {
                            if (textureSet.GroupName.Contains(findBox.Text)) {
                                textureSet.GroupName = textureSet.GroupName.Replace(findBox.Text, replaceBox.Text);
                            }
                        }
                        break;
                    case 2:
                        foreach (TextureSet textureSet in _textureSets) {
                            if (textureSet.TextureSetName.Contains(findBox.Text)) {
                                textureSet.GroupName = replaceBox.Text;
                            }
                        }
                        break;
                    case 3:
                        foreach (TextureSet textureSet in _textureSets) {
                            if (textureSet.GroupName.Contains(findBox.Text)) {
                                textureSet.TextureSetName = textureSet.TextureSetName = replaceBox.Text;
                            }
                        }
                        break;
                    case 4:
                        foreach (TextureSet textureSet in _textureSets) {
                            if (textureSet.TextureSetName.Contains(findBox.Text)) {
                                textureSet.TextureSetName = textureSet.TextureSetName = replaceBox.Text;
                            }
                        }
                        break;
                    case 5:
                        foreach (TextureSet textureSet in _textureSets) {
                            if (textureSet.GroupName.Contains(findBox.Text)) {
                                textureSet.GroupName = replaceBox.Text;
                            }
                        }
                        break;
                }
                DialogResult = DialogResult.OK;
                Close();
            } else {
                MessageBox.Show("You must enter a name or group to search and replace.", ProductName);
            }
        }

        private void cancel_Click(object sender, EventArgs e) {
            Close();
        }
    }
}
