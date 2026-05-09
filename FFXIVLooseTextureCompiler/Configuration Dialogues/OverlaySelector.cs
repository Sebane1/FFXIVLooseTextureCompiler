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
        List<string> _layeredUVs = new List<string>();
        List<ComboBox> _uvSelectors = new List<ComboBox>();

        private string _targetUV = "";
        public string TargetUV {
            get => _targetUV;
            set => _targetUV = value.ToLower();
        }

        public List<string> LayeredImages {
            get => _layeredImages; set {
                _layeredImages = value;
                foreach (var image in value) {
                    AddSelector(image, true);
                }
            }
        }
        public List<string> LayeredUVs {
            get => _layeredUVs; set {
                _layeredUVs = value;
                for (int i = 0; i < value.Count && i < _uvSelectors.Count; i++) {
                    string uv = value[i];
                    if (string.IsNullOrEmpty(uv)) _uvSelectors[i].SelectedIndex = 0;
                    else if (uv.ToLower() == "bibo") _uvSelectors[i].SelectedIndex = 1;
                    else if (uv.ToLower() == "gen3") _uvSelectors[i].SelectedIndex = 2;
                    else if (uv.ToLower() == "gen2") _uvSelectors[i].SelectedIndex = 3;
                    else if (uv.ToLower() == "tbse") _uvSelectors[i].SelectedIndex = 4;
                    else if (uv.ToLower() == "otopop") _uvSelectors[i].SelectedIndex = 5;
                    else if (uv.ToLower() == "vanillalala") _uvSelectors[i].SelectedIndex = 6;
                    else if (uv.ToLower() == "asymlala") _uvSelectors[i].SelectedIndex = 7;
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
            WFTranslator.TranslateControl(this);
        }

        private void label1_Click(object sender, EventArgs e) {

        }
        void AddSelector(string path, bool skipAddingEntry = false) {
            var filePicker = new FilePicker();
            filePicker.Parent = this;
            filePicker.Location = new Point(addLayerButton.Location.Y - addLayerButton.Height - (filePicker.Height * _filePickers.Count), 0);
            filePicker.Width = this.Width - 160;
            filePicker.Name = "Layer" + _filePickers.Count;
            filePicker.LabelName.Text = "Layer " + _filePickers.Count;
            filePicker.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            filePicker.CurrentPath = path;
            filePicker.FilePath.Text = path;
            
            ComboBox uvComboBox = new ComboBox();
            uvComboBox.Parent = this;
            uvComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            List<string> options = new List<string>() { "Auto" };
            if (_targetUV == "bibo") options.AddRange(new string[] { "Bibo", "Gen3", "Gen2" });
            else if (_targetUV == "gen3") options.AddRange(new string[] { "Gen3", "Bibo", "Gen2" });
            else if (_targetUV == "gen2") options.AddRange(new string[] { "Gen2", "Bibo", "Gen3" });
            else if (_targetUV == "otopop") options.AddRange(new string[] { "Otopop", "Asym Lala", "Vanilla Lala" });
            else if (_targetUV == "asymlala") options.AddRange(new string[] { "Asym Lala", "Otopop", "Vanilla Lala" });
            else if (_targetUV == "vanillalala") options.AddRange(new string[] { "Vanilla Lala", "Otopop", "Asym Lala" });
            else if (_targetUV == "tbse") options.AddRange(new string[] { "TBSE", "Vanilla Male" });
            else if (_targetUV == "vanillamale") options.AddRange(new string[] { "Vanilla Male", "TBSE" });
            else options.AddRange(new string[] { "Bibo", "Gen3", "Gen2", "TBSE", "Otopop", "Vanilla Lala", "Asym Lala", "Vanilla Male" });

            uvComboBox.Items.AddRange(options.ToArray());
            uvComboBox.Width = 100;
            uvComboBox.Location = new Point(filePicker.Location.X + filePicker.Width + 5, filePicker.Location.Y + 2);
            uvComboBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            uvComboBox.SelectedIndex = 0;
            _uvSelectors.Add(uvComboBox);

            this.Height += filePicker.Height;
            _filePickers.Add(filePicker);
            filePicker.Refresh();
            if (!skipAddingEntry) {
                _layeredImages.Add(path);
                _layeredUVs.Add("");
            }
            filePicker.Filter = "Texture File|*.png;*.tga;*.dds;*.bmp;*.tex;";
            int index = _layeredImages.Count - 1;
            filePicker.OnFileSelected += delegate {
                var i = index;
                var selector = filePicker;
                _layeredImages[i] = selector.FilePath.Text;
            };
            uvComboBox.SelectedIndexChanged += delegate {
                var i = index;
                if (uvComboBox.SelectedItem != null) {
                    string selected = uvComboBox.SelectedItem.ToString();
                    if (selected == "Auto") _layeredUVs[i] = "";
                    else if (selected == "Vanilla Lala") _layeredUVs[i] = "vanillalala";
                    else if (selected == "Vanilla Male") _layeredUVs[i] = "vanillamale";
                    else if (selected == "Asym Lala") _layeredUVs[i] = "asymlala";
                    else _layeredUVs[i] = selected.ToLower();
                }
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
            
            ComboBox uvComboBox = _uvSelectors[index];
            uvComboBox.Parent = null;
            uvComboBox.Dispose();

            _filePickers.RemoveAt(index);
            _uvSelectors.RemoveAt(index);
            _layeredImages.RemoveAt(index);
            _layeredUVs.RemoveAt(index);
        }
        private void addLayerButton_Click(object sender, EventArgs e) {
            AddSelector("");
        }
    }
}
