using FFXIVVoicePackCreator;
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
    public partial class OverlaySelector : Form {
        List<FilePicker> _filePickers = new List<FilePicker>();
        public EventHandler OnSelectedEventHandler;
        public OverlaySelector() {
            InitializeComponent();
            AutoScaleDimensions = new SizeF(96, 96);
        }
        List<string> _layeredImages = new List<string>();

        public List<string> LayeredImages {
            get => _layeredImages; set {
                _layeredImages = value;
                foreach (var image in value) {
                    AddSelector(image, true);
                }
            }
        }
        public void RefreshLayeredImages() {

        }
        private void filePicker2_Load(object sender, EventArgs e) {

        }

        private void filePicker3_Load(object sender, EventArgs e) {

        }

        private void OverlaySelector_Load(object sender, EventArgs e) {

        }

        private void label1_Click(object sender, EventArgs e) {

        }
        void AddSelector(string path, bool skipAddingEntry = false) {
            var filePicker = new FilePicker();
            filePicker.Parent = this;
            filePicker.Location = new Point(addLayerButton.Location.Y - addLayerButton.Height - (filePicker.Height * _filePickers.Count), 0);
            filePicker.Width = this.Width - 50;
            filePicker.Name = "Layer" + _filePickers.Count;
            filePicker.LabelName.Text = "Layer " + _filePickers.Count;
            filePicker.Anchor = AnchorStyles.Bottom;
            filePicker.CurrentPath = path;
            filePicker.FilePath.Text = path;
            this.Height += filePicker.Height;
            _filePickers.Add(filePicker);
            filePicker.Refresh();
            if (!skipAddingEntry) {
                _layeredImages.Add(path);
            }
            filePicker.Filter = "Texture File|*.png;*.tga;*.dds;*.bmp;*.tex;";
            int index = _layeredImages.Count - 1;
            filePicker.OnFileSelected += delegate {
                var i = index;
                var selector = filePicker;
                _layeredImages[i] = selector.FilePath.Text;
                //if (string.IsNullOrEmpty(selector.FilePath.Text)) {
                //    RemoveSelector(selector, index);
                //}
            };
            if (OnSelectedEventHandler != null) {
                filePicker.OnFileSelected += OnSelectedEventHandler;
            }
        }
        void RemoveSelector(FilePicker filePicker, int index) {
            this.Height -= filePicker.Height;
            filePicker.CurrentPath = "";
            filePicker.FilePath.Text = "";
            filePicker.Parent = null;
            filePicker.Dispose();
            _filePickers.Remove(filePicker);
            _layeredImages.RemoveAt(index);
        }
        private void addLayerButton_Click(object sender, EventArgs e) {
            AddSelector("");
        }
    }
}
