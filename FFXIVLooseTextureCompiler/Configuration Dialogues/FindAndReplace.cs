using FFXIVLooseTextureCompiler.Configuration_Dialogues;
using FFXIVLooseTextureCompiler.PathOrganization;

namespace FFXIVLooseTextureCompiler {
    public partial class FindAndReplace : Form {
        public FindAndReplace() {
            InitializeComponent();
            AutoScaleDimensions = new SizeF(96, 96);

            _overlaySelectorBase = new OverlaySelector();
            _overlaySelectorBase.LayeredImages = new List<string>();

            _overlaySelectorNormal = new OverlaySelector();
            _overlaySelectorNormal.LayeredImages = new List<string>();

            _overlaySelectorMask = new OverlaySelector();
            _overlaySelectorMask.LayeredImages = new List<string>();

        }
        List<TextureSet> textureSet = new List<TextureSet>();
        private OverlaySelector _overlaySelectorBase;
        private OverlaySelector _overlaySelectorNormal;
        private OverlaySelector _overlaySelectorMask;

        public List<TextureSet> TextureSets {
            get => textureSet;
            set {
                textureSet = value;
            }
        }

        public bool IsForEyes { get; internal set; }
        public void AcceptChanges() {
            if (!string.IsNullOrEmpty(replacementString.Text)) {
                foreach (TextureSet textureSet in textureSet) {
                    if (textureSet.TextureSetName.ToLower().Contains(replacementString.Text.ToLower())
                        && textureSet.GroupName.ToLower().Contains(groupTextBox.Text.ToLower())) {
                        if (!string.IsNullOrEmpty(baseTexture.FilePath.Text)) {
                            textureSet.Base = baseTexture.FilePath.Text;
                        }
                        if (_overlaySelectorBase.LayeredImages.Count > 0) {
                            textureSet.BaseOverlays = _overlaySelectorBase.LayeredImages;
                        }

                        if (!string.IsNullOrEmpty(normal.FilePath.Text)) {
                            textureSet.Normal = normal.FilePath.Text;
                        }
                        if (_overlaySelectorNormal.LayeredImages.Count > 0) {
                            textureSet.NormalOverlays = _overlaySelectorNormal.LayeredImages;
                        }

                        if (!string.IsNullOrEmpty(mask.FilePath.Text)) {
                            textureSet.Mask = mask.FilePath.Text;
                        }
                        if (_overlaySelectorMask.LayeredImages.Count > 0) {
                            textureSet.MaskOverlays = _overlaySelectorMask.LayeredImages;
                        }

                        if (!string.IsNullOrEmpty(bounds.FilePath.Text)) {
                            textureSet.NormalMask = bounds.FilePath.Text;
                        }
                        if (!string.IsNullOrEmpty(glow.FilePath.Text)) {
                            textureSet.Glow = glow.FilePath.Text;
                        }
                        if (!string.IsNullOrEmpty(material.FilePath.Text)) {
                            textureSet.Material = material.FilePath.Text;
                        }
                    }
                }
                DialogResult = DialogResult.OK;
            } else {
                MessageBox.Show("Search value cannot be empty!", Text);
            }
        }
        private void acceptChangesButton_Click(object sender, EventArgs e) {
            AcceptChanges();
        }

        public bool IsValidGamePathFormat(string input) {
            if ((input.Contains(@"/"))) {
                return false;
            } else {
                return true;
            }
        }

        private void CustomPathDialog_Load(object sender, EventArgs e) {
            AutoScaleDimensions = new SizeF(96, 96);
            baseTexture.LabelName.Text = "Base";
            normal.LabelName.Text = "Normal";
            mask.LabelName.Text = "Mask";
            WFTranslator.TranslateControl(this);
        }

        private void button1_Click(object sender, EventArgs e) {
            Close();
        }

        private void replacementString_TextChanged(object sender, EventArgs e) {
            baseTexture.LabelName.Text = "Base";
            normal.LabelName.Text = "Normal";
            mask.LabelName.Text = "Mask";
        }

        private void multi_Load(object sender, EventArgs e) {

        }

        private void layerBaseButton_Click(object sender, EventArgs e) {

            _overlaySelectorBase.ShowDialog();
        }

        private void layerNormalButton_Click(object sender, EventArgs e) {
            _overlaySelectorNormal.ShowDialog();
        }

        private void layersMaskButton_Click(object sender, EventArgs e) {
            _overlaySelectorMask.ShowDialog();
        }
    }
}
