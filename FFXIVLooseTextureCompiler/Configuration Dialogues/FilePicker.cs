using System.ComponentModel;

namespace FFXIVVoicePackCreator {
    public partial class FilePicker : UserControl {
        public FilePicker() {
            InitializeComponent();
        }

        int index = 0;
        bool isSaveMode = false;
        bool isSwappable = true;
        bool isPlayable = true;
        public event EventHandler OnFileSelected;

        string filter;
        private Point startPos;
        private bool canDoDragDrop;
        private bool ignoreClear;
        private bool muteState;
        private int maxTime = 4000;
        private Color color;
        private string currentPath;

        [Category("Filter"), Description("Changes what type of selection is made")]
        public string Filter { get => filter; set => filter = value; }
        public bool Enabled {
            get {
                return filePath.Enabled;
            }
            set {
                clearButton.Enabled = openButton.Enabled = filePath.Enabled = value;
                if (!value) {
                    filePath.Text = "";
                }
            }
        }

        public string CurrentPath { get => currentPath; set { currentPath = value; filePath.Text = value; } }

        private void filePicker_Load(object sender, EventArgs e) {
            color = BackColor;
            AutoScaleDimensions = new SizeF(96, 96);
            labelName.Text = (index == -1 ? FirstCharToUpper(Name) : ($"({index})  " + FirstCharToUpper(Name)));
            filePath.AllowDrop = true;
        }
        public string FirstCharToUpper(string input) {
            switch (input) {
                case null: return "";
                case "": return "";
                default: return input[0].ToString().ToUpper() + input.Substring(1);
            }
        }
        private void filePicker_MouseDown(object sender, MouseEventArgs e) {
            startPos = e.Location;
            canDoDragDrop = true;
        }

        private void filePicker_MouseMove(object sender, MouseEventArgs e) {
            if ((e.X != startPos.X || startPos.Y != e.Y) && canDoDragDrop) {
                this.ParentForm.TopMost = true;
                List<string> fileList = new List<string>();
                if (!string.IsNullOrEmpty(filePath.Text)) {
                    fileList.Add(filePath.Text);
                }
                if (fileList.Count > 0) {
                    DataObject fileDragData = new DataObject(DataFormats.FileDrop, fileList.ToArray());
                    DoDragDrop(fileDragData, DragDropEffects.Copy);
                }
                canDoDragDrop = false;
                this.ParentForm.BringToFront();
            }
            this.ParentForm.TopMost = false;
        }
        private void openButton_Click(object sender, EventArgs e) {
            if (!isSaveMode) {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = filter;
                if (openFileDialog.ShowDialog() == DialogResult.OK) {
                    filePath.Text = openFileDialog.FileName;
                    currentPath = openFileDialog.FileName;
                }
            } else {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = filter;
                if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                    filePath.Text = saveFileDialog.FileName;
                    currentPath = saveFileDialog.FileName;
                }
            }
            if (OnFileSelected != null) {
                OnFileSelected.Invoke(this, EventArgs.Empty);
            }
        }

        private void useGameFileCheckBox_CheckedChanged(object sender, EventArgs e) {

        }

        private void filePath_TextChanged(object sender, EventArgs e) {

        }

        private void filePath_DragEnter(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void filePath_DragDrop(object sender, DragEventArgs e) {
            string file = ((string[])e.Data.GetData(DataFormats.FileDrop, false))[0];
            if (CheckExtentions(file)) {
                filePath.Text = file;
                currentPath = file;
                if (OnFileSelected != null) {
                    OnFileSelected.Invoke(this, EventArgs.Empty);
                }
            } else {
                MessageBox.Show("This is not a media file this tool supports.", ParentForm.Text);
            }
        }
        public static bool CheckExtentions(string file) {
            string[] extentions = new string[] { ".png", ".dds", ".bmp", ".tex" };
            foreach (string extention in extentions) {
                if (file.EndsWith(extention)) {
                    return true;
                }
            }
            return false;
        }

        private void playButton_Click(object sender, EventArgs e) {

        }

        private void filePath_Enter(object sender, EventArgs e) {

        }

        private void filePath_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == 13) {
                if (CheckExtentions(FilePath.Text) || string.IsNullOrEmpty(filePath.Text)) {
                    currentPath = FilePath.Text;
                    if (OnFileSelected != null) {
                        OnFileSelected.Invoke(this, EventArgs.Empty);
                    }
                } else if (string.IsNullOrWhiteSpace(FilePath.SelectedText)) {
                    currentPath = null;
                }
            }
        }

        private void labelName_Click(object sender, EventArgs e) {

        }

        private void filePath_Leave(object sender, EventArgs e) {
            if (CheckExtentions(filePath.Text) || string.IsNullOrEmpty(filePath.Text)) {
                filePath.Text = filePath.Text;
                currentPath = filePath.Text;
                if (OnFileSelected != null) {
                    OnFileSelected.Invoke(this, EventArgs.Empty);
                }
            } else {
                filePath.Text = CurrentPath;
            }
        }

        private void clearButton_Click(object sender, EventArgs e) {
            currentPath = "";
            filePath.Text = "";
        }
    }
}


