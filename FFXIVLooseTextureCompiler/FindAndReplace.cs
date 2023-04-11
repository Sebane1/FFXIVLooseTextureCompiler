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
    public partial class FindAndReplace : Form {
        public FindAndReplace() {
            InitializeComponent();
        }
        List<TextureSet> textureSet = new List<TextureSet>();

        public List<TextureSet> TextureSets {
            get => textureSet;
            set {
                textureSet = value;
            }
        }

        private void acceptChangesButton_Click(object sender, EventArgs e) {
            if (!string.IsNullOrEmpty(replacementString.Text)) {
                foreach (TextureSet textureSet in textureSet) {
                    if (textureSet.MaterialSetName.ToLower().Contains(replacementString.Text.ToLower()) 
                        && textureSet.MaterialGroupName.ToLower().Contains(groupTextBox.Text.ToLower())) {
                        if (!string.IsNullOrEmpty(diffuse.FilePath.Text)) {
                            textureSet.Diffuse = diffuse.FilePath.Text;
                        }
                        if (!string.IsNullOrEmpty(normal.FilePath.Text)) {
                            textureSet.Normal = normal.FilePath.Text;
                        }
                        if (!string.IsNullOrEmpty(multi.FilePath.Text)) {
                            textureSet.Multi = multi.FilePath.Text;
                        }
                        if (!string.IsNullOrEmpty(mask.FilePath.Text)) {
                            textureSet.NormalMask = mask.FilePath.Text;
                        }
                        if (!string.IsNullOrEmpty(glow.FilePath.Text)) {
                            textureSet.Glow = glow.FilePath.Text;
                        }
                    }
                }
                DialogResult = DialogResult.OK;
            } else {
                MessageBox.Show("Search value cannot be empty!", Text);
            }
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
            if (replacementString.Text.ToLower().Contains("eyes")) {
                diffuse.LabelName.Text = "Normal";
                normal.LabelName.Text = "Multi";
                multi.LabelName.Text = "Catchlight";
            } else {
                diffuse.LabelName.Text = "Diffuse";
                normal.LabelName.Text = "Normal";
                multi.LabelName.Text = "Multi";
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            Close();
        }

        private void replacementString_TextChanged(object sender, EventArgs e) {
            if (replacementString.Text.ToLower().Contains("eyes")) {
                diffuse.LabelName.Text = "Normal";
                normal.LabelName.Text = "Multi";
                multi.LabelName.Text = "Catchlight";
            } else {
                diffuse.LabelName.Text = "Diffuse";
                normal.LabelName.Text = "Normal";
                multi.LabelName.Text = "Multi";
            }
        }
    }
}
