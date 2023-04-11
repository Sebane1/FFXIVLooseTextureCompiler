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
    public partial class TemplateConfiguration : Form {
        private string groupName;

        public TemplateConfiguration() {
            InitializeComponent();
        }

        public string GroupName { get => groupName; set => groupName = value; }

        private void confirmButton_Click(object sender, EventArgs e) {
            groupName = groupNameTextBox.Text;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
