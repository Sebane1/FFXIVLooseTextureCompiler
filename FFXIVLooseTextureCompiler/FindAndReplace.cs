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
        List<TextureSet> materialSets = new List<TextureSet>();

        public List<TextureSet> MaterialSets {
            get => materialSets;
            set {
                materialSets = value;
            }
        }

        private void acceptChangesButton_Click(object sender, EventArgs e) {
            if (!string.IsNullOrEmpty(replacementString.Text)) {
                foreach (TextureSet materialSet in materialSets) {
                    if (materialSet.MaterialSetName.ToLower().Contains(replacementString.Text.ToLower())) {
                        if (!string.IsNullOrEmpty(diffuse.FilePath.Text)) {
                            materialSet.Diffuse = diffuse.FilePath.Text;
                        }
                        if (!string.IsNullOrEmpty(normal.FilePath.Text)) {
                            materialSet.Normal = normal.FilePath.Text;
                        }
                        if (!string.IsNullOrEmpty(multi.FilePath.Text)) {
                            materialSet.Multi = multi.FilePath.Text;
                        }
                        if (!string.IsNullOrEmpty(mask.FilePath.Text)) {
                            materialSet.NormalMask = mask.FilePath.Text;
                        }
                        if (!string.IsNullOrEmpty(glow.FilePath.Text)) {
                            materialSet.Glow = glow.FilePath.Text;
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
                diffuse.LabelName.Text = "normal";
                normal.LabelName.Text = "multi";
                multi.LabelName.Text = "catchlight";
            } else {
                diffuse.LabelName.Text = "diffuse";
                normal.LabelName.Text = "normal";
                multi.LabelName.Text = "multi";
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            Close();
        }

        private void replacementString_TextChanged(object sender, EventArgs e) {
            if (replacementString.Text.ToLower().Contains("eyes")) {
                diffuse.LabelName.Text = "normal";
                normal.LabelName.Text = "multi";
                multi.LabelName.Text = "catchlight";
            } else {
                diffuse.LabelName.Text = "diffuse";
                normal.LabelName.Text = "normal";
                multi.LabelName.Text = "multi";
            }
        }
    }
}
