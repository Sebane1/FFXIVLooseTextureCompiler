using FFXIVLooseTextureCompiler.ImageProcessing;
using OtterTex;
using Penumbra.LTCImport.Textures;
using System.Drawing.Imaging;

namespace FFXIVLooseTextureCompiler {
    public partial class BulkTexManager : Form {
        private Point startPos;
        private bool canDoDragDrop;

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
                    texturePreview.BackgroundImage.Save(saveFileDialog.FileName);
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
            if (textureList.SelectedIndex != -1) {
                if (File.Exists(textureList.SelectedItem.ToString())) {
                    texturePreview.BackgroundImage = TexLoader.TexToBitmap(textureList.SelectedItem.ToString());
                } else {
                    textureList.Items.Remove(textureList.SelectedItem);
                }
            }
        }

        private void bulkImport_Click(object sender, EventArgs e) {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK) {
                AddFilesRecursively(folderBrowserDialog.SelectedPath, 0, 10);
            }
        }
        public void AddFilesRecursively(string path, int recursionCount, int recursionLimit) {
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
                    using (var stream = new FileStream(item.ToString(), FileMode.Open, FileAccess.Read)) {
                        var scratch = PenumbraTexFileParser.Parse(stream);
                        var rgba = scratch.GetRGBA(out var f).ThrowIfError(f);
                        RGBAPixels = rgba.Pixels[..(f.Meta.Width * f.Meta.Height * f.Meta.Format.BitsPerPixel() / 8)].ToArray();

                        Bitmap output = new Bitmap(scratch.Meta.Width, scratch.Meta.Height);
                        Rectangle rect = new Rectangle(0, 0, output.Width, output.Height);
                        BitmapData bmpData = output.LockBits(rect, ImageLockMode.ReadWrite, output.PixelFormat);
                        IntPtr ptr = bmpData.Scan0;
                        for (int i = 0; i < RGBAPixels.Length; i += 4) {
                            byte R = RGBAPixels[i];
                            byte G = RGBAPixels[i + 1];
                            byte B = RGBAPixels[i + 2];
                            byte A = RGBAPixels[i + 3];

                            RGBAPixels[i] = B;
                            RGBAPixels[i + 1] = G;
                            RGBAPixels[i + 2] = R;
                            RGBAPixels[i + 3] = A;

                        }
                        System.Runtime.InteropServices.Marshal.Copy(RGBAPixels, 0, ptr, RGBAPixels.Length);
                        output.UnlockBits(bmpData);
                        output.Save(Path.Combine(folderBrowserDialog.SelectedPath, Path.GetFileNameWithoutExtension(item) + ".png"));
                    }
                }
            }
            MessageBox.Show("Bulk Export Complete", Text);
        }

        private void textureList_Click(object sender, EventArgs e) {
            textureList.Items.Clear();
        }
    }
}
