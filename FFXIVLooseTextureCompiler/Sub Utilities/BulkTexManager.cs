﻿using FFXIVLooseTextureCompiler.ImageProcessing;
using Penumbra.LTCImport.Textures;
using System.Diagnostics;
using System.Drawing.Imaging;

namespace FFXIVLooseTextureCompiler {
    public partial class BulkTexManager : Form {
        private Point startPos;
        private bool canDoDragDrop;
        private string _lastPath;

        public byte[] RGBAPixels { get; private set; }

        public BulkTexManager() {
            InitializeComponent();
            TopMost = true;
        }

        private void pictureBox1_Click(object sender, EventArgs e) {

        }

        private void exportPNGButton_Click(object sender, EventArgs e) {
            if (texturePreview.BackgroundImage != null) {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = ".png files|*.png";
                if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                    TexIO.SaveBitmap(texturePreview.BackgroundImage as Bitmap, saveFileDialog.FileName);
                    MessageBox.Show("Texture saved to .png", Text);
                }
            } else {
                MessageBox.Show("Please select a valid file!", Text);
            }
        }
        private void texList_DragEnter(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void texList_DragDrop(object sender, DragEventArgs e) {
            string file = ((string[])e.Data.GetData(DataFormats.FileDrop, false))[0];
            if (CheckExtentions(file)) {
                if (!textureList.Items.Contains(file)) {
                    textureList.Items.Add(file);
                }
            } else {
                MessageBox.Show("This is not a media file this window supports.", Text);
            }
        }
        private bool CheckExtentions(string file) {
            string[] extentions = new string[] { ".tex" };
            foreach (string extention in extentions) {
                if (file.Contains(extention)) {
                    return true;
                }
            }
            return false;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e) {
            try {
                if (textureList.SelectedIndex != -1) {
                    if (File.Exists(textureList.SelectedItem.ToString())) {
                        texturePreview.BackgroundImage = TexIO.TexToBitmap(textureList.SelectedItem.ToString());
                    } else {
                        textureList.Items.Remove(textureList.SelectedItem);
                    }
                }
            } catch {

            }
        }

        private void bulkImport_Click(object sender, EventArgs e) {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK) {
                AddFilesRecursively(folderBrowserDialog.SelectedPath, 0, 10);
            }
        }
        public void AddFilesRecursively(string path, int recursionCount, int recursionLimit) {
            if (recursionCount == 0) {
                _lastPath = path;
                try {
                    pathTextBox.Text = _lastPath;
                } catch {

                }
            }
            try {
                foreach (string file in Directory.GetFiles(path, "*.tex")) {
                    if (File.Exists(file)) {
                        if (!textureList.Items.Contains(file)) {
                            textureList.Items.Add(file);
                        }
                    }
                }
                foreach (string newPath in Directory.GetDirectories(path)) {
                    AddFilesRecursively(newPath, recursionCount++, recursionLimit);
                }
            } catch {

            }
        }
        private void texList_MouseDown(object sender, MouseEventArgs e) {
            startPos = e.Location;
            canDoDragDrop = true;
        }

        private void texList_MouseMove(object sender, MouseEventArgs e) {
            if ((e.X != startPos.X || startPos.Y != e.Y) && canDoDragDrop) {
                List<string> fileList = new List<string>();
                foreach (int item in textureList.SelectedIndices) {
                    if (!string.IsNullOrEmpty(textureList.Items[item].ToString())) {
                        fileList.Add(textureList.Items[item].ToString());
                    }
                }
                if (fileList.Count > 0) {
                    DataObject fileDragData = new DataObject(DataFormats.FileDrop, fileList.ToArray());
                    DoDragDrop(fileDragData, DragDropEffects.Copy);
                }
                canDoDragDrop = false;
            }
        }
        private void exportAllButton_Click(object sender, EventArgs e) {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog(this) == DialogResult.OK) {
                foreach (string item in textureList.Items) {
                    var output = TexIO.TexToBitmap(item.ToString());
                    TexIO.SaveBitmap(output, Path.Combine(folderBrowserDialog.SelectedPath, Path.GetFileNameWithoutExtension(item) + ".png"));
                }
            }
            MessageBox.Show("Bulk Export Complete", Text);
        }

        private void textureList_Click(object sender, EventArgs e) {
            textureList.Items.Clear();
        }
        public void ClearList() {
            try {
                textureList.Items.Clear();
            } catch {

            }
        }

        private void pathTextBox_TextChanged(object sender, EventArgs e) {
            pathTextBox.Text = _lastPath;
        }

        private void openPathButton_Click(object sender, EventArgs e) {
            if (!string.IsNullOrEmpty(_lastPath)) {
                try {
                    Process.Start(new System.Diagnostics.ProcessStartInfo() {
                        FileName = Path.GetDirectoryName(_lastPath),
                        UseShellExecute = true,
                        Verb = "OPEN"
                    });
                } catch {

                }
            }
        }

        private void newFileCheck_Tick(object sender, EventArgs e) {
            if (!string.IsNullOrEmpty(_lastPath)) {
                AddFilesRecursively(_lastPath, 0, 10);
            }
        }

        private void BulkTexManager_Load(object sender, EventArgs e) {
            WFTranslator.TranslateControl(this);
        }
    }
}
