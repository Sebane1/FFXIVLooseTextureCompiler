using FFXIVLooseTextureCompiler.ImageProcessing;
using KVImage;
using SixLabors.ImageSharp.Formats;
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
    public partial class MultiCreator : Form {
        public MultiCreator() {
            InitializeComponent();
            AutoScaleDimensions = new SizeF(96, 96);
            ((Control)redImage).AllowDrop = true;
            ((Control)greenImage).AllowDrop = true;
            ((Control)blueImage).AllowDrop = true;
            ((Control)alphaImage).AllowDrop = true;
        }

        public void ImportImage(string path, PictureBox output) {
            switch (Path.GetExtension(path)) {
                case ".png":
                case ".bmp":
                    using (Bitmap source = new Bitmap(path)) {
                        Bitmap target = new Bitmap(source.Size.Width, source.Size.Height);
                        Graphics g = Graphics.FromImage(target);
                        g.Clear(Color.White);
                        g.DrawImage(source, 0, 0, source.Width, source.Height);
                        output.BackgroundImage = target;
                    }
                    break;
                case ".dds":
                    using (Bitmap source = TexLoader.DDSToBitmap(path)) {
                        Bitmap target = new Bitmap(source.Size.Width, source.Size.Height);
                        Graphics g = Graphics.FromImage(target);
                        g.Clear(Color.White);
                        g.DrawImage(source, 0, 0, source.Width, source.Height);
                        output.BackgroundImage = target;
                    }
                    break;
            }
            if (redImage.BackgroundImage != null && greenImage.BackgroundImage != null
                && blueImage.BackgroundImage != null && alphaImage.BackgroundImage != null) {
                result.BackgroundImage = ImageManipulation.MergeGrayscalesToARGB(
                   (Bitmap)redImage.BackgroundImage, (Bitmap)greenImage.BackgroundImage,
                   (Bitmap)blueImage.BackgroundImage, (Bitmap)alphaImage.BackgroundImage);
            }
        }

        private void importButton1_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Texture Files|*.png;*.dds;*.bmp";
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                ImportImage(openFileDialog.FileName, redImage);
            }
        }

        private void importButton2_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Texture Files|*.png;*.dds;*.bmp";
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                ImportImage(openFileDialog.FileName, greenImage);
            }
        }

        private void importButton3_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Texture Files|*.png;*.dds;*.bmp";
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                ImportImage(openFileDialog.FileName, blueImage);
            }
        }

        private void importButton4_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Texture Files|*.png;*.dds;*.bmp";
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                ImportImage(openFileDialog.FileName, alphaImage);
            }
        }
        private void import1_DragEnter(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void import1_DragDrop(object sender, DragEventArgs e) {
            string file = ((string[])e.Data.GetData(DataFormats.FileDrop, false))[0];
            if (CheckExtentions(file)) {
                ImportImage(file, redImage);
            } else {
                MessageBox.Show("This is not a media file this window supports.", Text);
            }
        }
        private void import2_DragEnter(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void import2_DragDrop(object sender, DragEventArgs e) {
            string file = ((string[])e.Data.GetData(DataFormats.FileDrop, false))[0];
            if (CheckExtentions(file)) {
                ImportImage(file, greenImage);
            } else {
                MessageBox.Show("This is not a media file this window supports.", Text);
            }
        }
        private void import3_DragEnter(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void import3_DragDrop(object sender, DragEventArgs e) {
            string file = ((string[])e.Data.GetData(DataFormats.FileDrop, false))[0];
            if (CheckExtentions(file)) {
                ImportImage(file, blueImage);
            } else {
                MessageBox.Show("This is not a media file this window supports.", Text);
            }
        }
        private void import4_DragEnter(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void import4_DragDrop(object sender, DragEventArgs e) {
            string file = ((string[])e.Data.GetData(DataFormats.FileDrop, false))[0];
            if (CheckExtentions(file)) {
                ImportImage(file, alphaImage);
            } else {
                MessageBox.Show("This is not a media file this window supports.", Text);
            }
        }
        private bool CheckExtentions(string file) {
            string[] extentions = new string[] { ".png", ".dds", ".bmp" };
            foreach (string extention in extentions) {
                if (file.Contains(extention)) {
                    return true;
                }
            }
            return false;
        }

        private void exportButton_Click(object sender, EventArgs e) {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Texture Files|*.png;";
            if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                result.BackgroundImage.Save(saveFileDialog.FileName);
                MessageBox.Show("Texture saved!", Text);
            }
        }
    }
}
