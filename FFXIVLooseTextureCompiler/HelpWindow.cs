using System.Diagnostics;

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
                    FileName = "https://drive.google.com/drive/folders/1pQoDREYQsvHFwoiEOggkSyjbQSjKcnpU",
                    UseShellExecute = true,
                    Verb = "OPEN"
                });
            } catch {

            }
        }

        private void getHRBODY_Click(object sender, EventArgs e) {
            try {
                Process.Start(new System.Diagnostics.ProcessStartInfo() {
                    FileName = "https://drive.google.com/drive/folders/1FfJm42VTncIOJWNWAH2L_JbGih9ZAae6",
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
    }
}
