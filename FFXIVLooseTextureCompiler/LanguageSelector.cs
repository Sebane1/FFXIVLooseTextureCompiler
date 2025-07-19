using LanguageConversionProxy;
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
    public partial class LanguageSelector : Form {
        public LanguageEnum Language { get; set; }
        public LanguageSelector() {
            InitializeComponent();
        }

        private void englishButton_Click(object sender, EventArgs e) {
            Language = LanguageEnum.English;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void frenchButton_Click(object sender, EventArgs e) {
            Language = LanguageEnum.French;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void germanButton_Click(object sender, EventArgs e) {
            Language = LanguageEnum.German;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void japaneseButton_Click(object sender, EventArgs e) {
            Language = LanguageEnum.Japanese;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void chineseButton_Click(object sender, EventArgs e) {
            Language = LanguageEnum.Chinese;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void swedishButton_Click(object sender, EventArgs e) {
            Language = LanguageEnum.Swedish;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void koreanButton_Click(object sender, EventArgs e) {
            Language = LanguageEnum.Korean;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
