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
                    groupNameTextBox.Text = textureSet.GroupName;
                    textureSetNameTextBox.Text = textureSet.TextureSetName;
                    internalBasePathTextBox.Text = textureSet.InternalBasePath;
                    internalNormalPathTextBox.Text = textureSet.InternalNormalPath;
                    internalMaskPathTextbox.Text = textureSet.InternalMaskPath;
                    internalMaterialPathTextBox.Text = textureSet.InternalMaterialPath;
                    ignoreNormalsCheckbox.Checked = textureSet.IgnoreNormalGeneration;
                    ignoreMultiCheckbox.Checked = textureSet.IgnoreMaskGeneration;
                    usesAlternateTextures.Checked = textureSet.UsesScales;
                    material.CurrentPath = textureSet.Material;
                    invertNormals.Checked = textureSet.InvertNormalGeneration;
                    normalCorrection.Text = textureSet.NormalCorrection;
                    var skinTypes = UniversalTextureSetCreator.GetSkinTypeNames(textureSet);
                    if (skinTypes != null) {
                        skinTypeSelection.Items.AddRange(skinTypes.ToArray());
                        skinTypeSelection.SelectedIndex = textureSet.SkinType;
                    } else {
                        skinTypeSelection.Items.Clear();
                    }
                }
            }
        }

        private void acceptChangesButton_Click(object sender, EventArgs e) {
            if (!string.IsNullOrWhiteSpace(textureSetNameTextBox.Text)) {
                textureSet.GroupName = groupNameTextBox.Text;
                textureSet.TextureSetName = textureSetNameTextBox.Text;
                int validationCount = 0;
                if (IsValidGamePathFormat(internalBasePathTextBox.Text)) {
                    textureSet.InternalBasePath = internalBasePathTextBox.Text;
                    validationCount++;
                } else {
                    MessageBox.Show("Internal baseTexture path is invalid. Make sure an in game path format is being used and that it points to a .tex file!", Text);
                }
                if (IsValidGamePathFormat(internalNormalPathTextBox.Text)) {
                    textureSet.InternalNormalPath = internalNormalPathTextBox.Text;
                    validationCount++;
                } else {
                    MessageBox.Show("Internal normal path is invalid. Make sure an in game path format is being used and that it points to a .tex file!", Text);
                }
                if (IsValidGamePathFormat(internalMaskPathTextbox.Text)) {
                    textureSet.InternalMaskPath = internalMaskPathTextbox.Text;
                    validationCount++;
                } else {
                    MessageBox.Show("Internal multi path is invalid. Make sure an in game path format is being used and that it points to a .tex file!", Text);
                }
                if (IsValidGamePathFormatMaterial(internalMaterialPathTextBox.Text)) {
                    textureSet.InternalMaterialPath = internalMaterialPathTextBox.Text;
                    validationCount++;
                } else {
                    MessageBox.Show("Internal material path is invalid. Make sure an in game path format is being used and that it points to a .mtrl file!", Text);
                }
                if (File.Exists(normalCorrection.Text) || string.IsNullOrEmpty(normalCorrection.Text)) {
                    textureSet.NormalCorrection = normalCorrection.Text;
                    validationCount++;
                } else {
                    MessageBox.Show("Normal correction path is invalid.", Text);
                }
                if (validationCount == 5) {
                    DialogResult = DialogResult.OK;
                    Close();
                }
                textureSet.IgnoreNormalGeneration = ignoreNormalsCheckbox.Checked;
                textureSet.IgnoreMaskGeneration = ignoreMultiCheckbox.Checked;
                textureSet.InvertNormalGeneration = invertNormals.Checked;
                textureSet.Material = material.FilePath.Text;
                textureSet.SkinType = skinTypeSelection.SelectedIndex;
                textureSet.UsesScales = usesAlternateTextures.Checked;
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
        public bool IsValidGamePathFormatMaterial(string input) {
            if ((input.Contains(@"\") || !input.Contains(".mtrl")) && !string.IsNullOrWhiteSpace(input)) {
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
            baseTextureLabel.Text = "Internal Base";
            normalLabel.Text = "Internal Normal";
            multiLabel.Text = "Internal Mask";
        }

        private void groupChoiceType_SelectedIndexChanged(object sender, EventArgs e) {

        }

        private void invertNormals_CheckedChanged(object sender, EventArgs e) {

        }

        private void skinTypeSelection_SelectedIndexChanged(object sender, EventArgs e) {

        }
    }
}
