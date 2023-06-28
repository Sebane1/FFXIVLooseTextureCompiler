using FFXIVLooseTextureCompiler.PathOrganization;
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
    public partial class BulkNameReplacement : Form {
        private List<TextureSet> _textureSets;

        public BulkNameReplacement(List<TextureSet> textureSets) {
            _textureSets = textureSets;
            InitializeComponent();
        }

        private void BulkNameReplacement_Load(object sender, EventArgs e) {
            replacementTypeComboBox.SelectedIndex = 0;
        }

        private void replacementTypeComboBox_SelectedIndexChanged(object sender, EventArgs e) {

        }

        private void replaceBox_TextChanged(object sender, EventArgs e) {

        }

        private void replaceAll_Click(object sender, EventArgs e) {
            switch (replacementTypeComboBox.SelectedIndex) {
                case 0:
                    foreach (TextureSet textureSet in _textureSets) {
                        if (textureSet.TextureSetName.Contains(findBox.Text)) {
                            textureSet.TextureSetName.Replace(findBox.Text, replaceBox.Text);
                        }
                    }
                    break;
                case 1:
                    foreach (TextureSet textureSet in _textureSets) {
                        if (textureSet.GroupName.Contains(findBox.Text)) {
                            textureSet.GroupName.Replace(findBox.Text, replaceBox.Text);
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
                            textureSet.TextureSetName = replaceBox.Text;
                        }
                    }
                    break;
            }
        }
    }
}
