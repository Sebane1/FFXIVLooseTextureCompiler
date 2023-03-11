using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FFXIVLooseTextureCompiler {
    public partial class HelpWindow : Form {
        public HelpWindow() {
            InitializeComponent();
        }

        private void getBibo_Click(object sender, EventArgs e) {
            try {
                Process.Start(new System.Diagnostics.ProcessStartInfo() {
                    FileName = "https://cdn.discordapp.com/attachments/1062028973053325412/1064005923351310376/Bibo_Textures.rar",
                    UseShellExecute = true,
                    Verb = "OPEN"
                });
            } catch {

            }
        }

        private void getGen3_Click(object sender, EventArgs e) {
            try {
                Process.Start(new System.Diagnostics.ProcessStartInfo() {
                    FileName = "https://www.xivmodarchive.com/private/d461db42-d408-4728-b3b1-2169f0de7516/files/Tight%26Firm-Gen3-RawTextures.7z",
                    UseShellExecute = true,
                    Verb = "OPEN"
                });
            } catch {

            }
        }

        private void getScalesPlus_Click(object sender, EventArgs e) {
            try {
                Process.Start(new System.Diagnostics.ProcessStartInfo() {
                    FileName = "https://drive.google.com/file/d/1yokE7nZlRQSXK3TZyOSH7o6HngfC0WJ2/view",
                    UseShellExecute = true,
                    Verb = "OPEN"
                });
            } catch {

            }
        }

        private void getTBSE_Click(object sender, EventArgs e) {
            try {
                Process.Start(new System.Diagnostics.ProcessStartInfo() {
                    FileName = "https://www.nexusmods.com/finalfantasy14/mods/1255?tab=files",
                    UseShellExecute = true,
                    Verb = "OPEN"
                });
            } catch {

            }
        }

        private void getHRBODY_Click(object sender, EventArgs e) {
            try {
                Process.Start(new System.Diagnostics.ProcessStartInfo() {
                    FileName = "https://www.nexusmods.com/finalfantasy14/mods/1052?tab=files",
                    UseShellExecute = true,
                    Verb = "OPEN"
                });
            } catch {

            }
        }

        private void HelpWindow_Load(object sender, EventArgs e) {

        }

        private void getOtopop_Click(object sender, EventArgs e) {
            try {
                Process.Start(new System.Diagnostics.ProcessStartInfo() {
                    FileName = "https://drive.google.com/drive/u/1/folders/1glbwYcLHcYiO5HykCAHYRgkUM2K6ieOx",
                    UseShellExecute = true,
                    Verb = "OPEN"
                });
            } catch {

            }
        }

        private void getRedefinedLala_Click(object sender, EventArgs e) {
            try {
                Process.Start(new System.Diagnostics.ProcessStartInfo() {
                    FileName = "https://mega.nz/folder/7QMSTQbB#n_9788MFTEYfOk4VPRusGQ/folder/3MkCGbxa",
                    UseShellExecute = true,
                    Verb = "OPEN"
                });
            } catch {

            }
        }
    }
}
