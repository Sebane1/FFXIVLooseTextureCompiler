using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FFXIVLooseTextureCompiler.Configuration_Dialogues {
    public partial class CustomUnderlayForm : Form {
        public CustomUnderlayForm() {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e) {

        }

        private void CustomUnderlayForm_Load(object sender, EventArgs e) {
            WFTranslator.TranslateControl(this);
        }
    }
}
