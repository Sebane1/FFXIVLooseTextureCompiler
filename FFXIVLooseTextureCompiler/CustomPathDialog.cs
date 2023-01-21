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
    public partial class CustomPathDialog : Form {
        public CustomPathDialog() {
            InitializeComponent();
        }
        MaterialSet materialSet = new MaterialSet();

        public MaterialSet MaterialSet {
            get => materialSet;
            set {
                materialSet = value;
                if (materialSet != null) {
                    materialSetNameTextBox.Text = materialSet.MaterialSetName;
                    internalDiffusePathTextBox.Text = materialSet.InternalDiffusePath;
                    internalNormalPathTextBox.Text = materialSet.InternalNormalPath;
                    internalMultiPathTextbox.Text = materialSet.InternalMultiPath;
                }
            }
        }

        private void acceptChangesButton_Click(object sender, EventArgs e) {
            if (!string.IsNullOrWhiteSpace(materialSetNameTextBox.Text)) {
                materialSet.MaterialSetName = materialSetNameTextBox.Text;
                int validationCount = 0;
                if (IsValidGamePathFormat(internalDiffusePathTextBox.Text)) {
                    materialSet.InternalDiffusePath = internalDiffusePathTextBox.Text;
                    validationCount++;
                } else {
                    MessageBox.Show("Internal diffuse path is invalid. Make sure an in game path format is being used and that it points to a .tex file!", Text);
                }
                if (IsValidGamePathFormat(internalNormalPathTextBox.Text)) {
                    materialSet.InternalNormalPath = internalNormalPathTextBox.Text;
                    validationCount++;
                } else {
                    MessageBox.Show("Internal normal path is invalid. Make sure an in game path format is being used and that it points to a .tex file!", Text);
                }
                if (IsValidGamePathFormat(internalMultiPathTextbox.Text)) {
                    materialSet.InternalMultiPath = internalMultiPathTextbox.Text;
                    validationCount++;
                } else {
                    MessageBox.Show("Internal multi path is invalid. Make sure an in game path format is being used and that it points to a .tex file!", Text);
                }
                if (validationCount == 3) {
                    DialogResult = DialogResult.OK;
                    Close();
                }
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
    }
}
