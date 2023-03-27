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
    public partial class ConnectionDisplay : Form {
        string id;
        bool ignoreEvent;
        public ConnectionDisplay(string id) {
            InitializeComponent();
            Id = id;
            AutoScaleDimensions = new SizeF(96, 96);
            TopMost = true;
        }
        public event EventHandler RequestedToSendCurrentMod;
        public string Id { get => id; set { ignoreEvent = true; sharingTextbox.Text = id = value; ignoreEvent = false; } }
        public string SendId { get => sendId.Text; set { sendId.Text = value; } }

        private void sharingTextbox_TextChanged(object sender, EventArgs e) {
            if (!ignoreEvent) {
                sharingTextbox.Text = id;
            }
        }

        private void sendModButton_Click(object sender, EventArgs e) {
            if (RequestedToSendCurrentMod != null) {
                RequestedToSendCurrentMod.Invoke(this, EventArgs.Empty);
            }
        }

        private void ConnectionDisplay_FormClosing(object sender, FormClosingEventArgs e) {
           // e.Cancel = true;
        }
    }
}
