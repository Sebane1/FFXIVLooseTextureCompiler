using FFXIVLooseTextureCompiler.PathOrganization;

namespace FFXIVLooseTextureCompiler {
    public partial class CustomPathDialog : Form {
        public CustomPathDialog() {
            InitializeComponent();
        }
        TextureSet textureSet = new TextureSet();
        public ComboBox GroupingType {
            get { return groupChoiceType; }
        }
        public TextureSet TextureSet {
            get => textureSet;
            set {
                textureSet = value;
                if (textureSet != null) {
                    groupNameTextBox.Text = textureSet.MaterialGroupName;
                    materialSetNameTextBox.Text = textureSet.MaterialSetName;
                    internalDiffusePathTextBox.Text = textureSet.InternalDiffusePath;
                    internalNormalPathTextBox.Text = textureSet.InternalNormalPath;
                    internalMultiPathTextbox.Text = textureSet.InternalMultiPath;
                    ignoreNormalsCheckbox.Checked = textureSet.IgnoreNormalGeneration;
                    ignoreMultiCheckbox.Checked = textureSet.IgnoreMultiGeneration;
                    invertNormals.Checked = textureSet.InvertNormalGeneration;
                    normalCorrection.Text = textureSet.NormalCorrection;
                }
            }
        }

        private void acceptChangesButton_Click(object sender, EventArgs e) {
            if (!string.IsNullOrWhiteSpace(materialSetNameTextBox.Text)) {
                textureSet.MaterialGroupName = groupNameTextBox.Text;
                textureSet.MaterialSetName = materialSetNameTextBox.Text;
                int validationCount = 0;
                if (IsValidGamePathFormat(internalDiffusePathTextBox.Text)) {
                    textureSet.InternalDiffusePath = internalDiffusePathTextBox.Text;
                    validationCount++;
                } else {
                    MessageBox.Show("Internal diffuse path is invalid. Make sure an in game path format is being used and that it points to a .tex file!", Text);
                }
                if (IsValidGamePathFormat(internalNormalPathTextBox.Text)) {
                    textureSet.InternalNormalPath = internalNormalPathTextBox.Text;
                    validationCount++;
                } else {
                    MessageBox.Show("Internal normal path is invalid. Make sure an in game path format is being used and that it points to a .tex file!", Text);
                }
                if (IsValidGamePathFormat(internalMultiPathTextbox.Text)) {
                    textureSet.InternalMultiPath = internalMultiPathTextbox.Text;
                    validationCount++;
                } else {
                    MessageBox.Show("Internal multi path is invalid. Make sure an in game path format is being used and that it points to a .tex file!", Text);
                }
                if (File.Exists(normalCorrection.Text) || string.IsNullOrEmpty(normalCorrection.Text)) {
                    textureSet.NormalCorrection = normalCorrection.Text;
                    validationCount++;
                } else {
                    MessageBox.Show("Normal correction path is invalid.", Text);
                }
                if (validationCount == 4) {
                    DialogResult = DialogResult.OK;
                    Close();
                }
                textureSet.IgnoreNormalGeneration = ignoreNormalsCheckbox.Checked;
                textureSet.IgnoreMultiGeneration = ignoreMultiCheckbox.Checked;
                textureSet.InvertNormalGeneration = invertNormals.Checked;
            } else {
                MessageBox.Show("Please enter a name for your custom material set!", Text);
            }
        }

        public bool IsValidGamePathFormat(string input) {
            if ((input.Contains(@"\") || !input.Contains(".tex")) && !string.IsNullOrWhiteSpace(input)) {
                return false;
            } else {
                return true;
            }
        }

        private void CustomPathDialog_Load(object sender, EventArgs e) {
            AutoScaleDimensions = new SizeF(96, 96);
        }

        private void button1_Click(object sender, EventArgs e) {
            Close();
        }

        private void materialSetNameTextBox_TextChanged(object sender, EventArgs e) {
            if (textureSet.InternalMultiPath != null && textureSet.InternalMultiPath.ToLower().Contains("catchlight")) {
                diffuseLabel.Text = "Internal Normal";
                normalLabel.Text = "Internal Multi";
                multiLabel.Text = "Internal Catchlight";
            } else {
                diffuseLabel.Text = "Internal Diffuse";
                normalLabel.Text = "Internal Normal";
                multiLabel.Text = "Internal Multi";
            }
        }

        private void groupChoiceType_SelectedIndexChanged(object sender, EventArgs e) {

        }
    }
}
