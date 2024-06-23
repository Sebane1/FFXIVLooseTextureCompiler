using FFXIVLooseTextureCompiler.ImageProcessing;
using FFXIVLooseTextureCompiler.Racial;
using KVImage;
using System.Diagnostics;
using System.Windows.Documents;


namespace FFXIVLooseTextureCompiler.Sub_Utilities {
    public partial class LegacyMakeupSalvager : Form {
        private string lastItem;

        public LegacyMakeupSalvager() {
            InitializeComponent();
        }

        private void convertMakeupButton_Click(object sender, EventArgs e) {
            ConvertMakeup();
            MessageBox.Show("Makeup Salvaging Operation Complete!");
            if (!string.IsNullOrEmpty(lastItem)) {
                OpenFolder(lastItem);
            }
        }

        private void ConvertMakeup(string makeupPath = null) {
            convertMakeupButton.Enabled = false;
            string lipCorrectionMap = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"res\textures\face\" + racialGender.SelectedItem.ToString().ToLower() + @"\correction.png");
            string eyeCorrectionMap = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"res\textures\face\" + racialGender.SelectedItem.ToString().ToLower() + @"\eyecorrection.png");
            string inputModel = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
            @"res\model\face\" + racialGender.SelectedItem.ToString().ToLower() + @"\" + RaceInfo.ModelRaces[RaceInfo.SubRaceToModelRace(subRaceListBox.SelectedIndex)].ToLower() + @"\input\"
            + (1 + faceNumberListBox.SelectedIndex) + ".fbx");
            string outputModel = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
            @"res\model\face\" + racialGender.SelectedItem.ToString().ToLower() + @"\" + RaceInfo.ModelRaces[RaceInfo.SubRaceToModelRace(subRaceListBox.SelectedIndex)].ToLower() + @"\output\"
            + (1 + faceNumberListBox.SelectedIndex) + ".fbx");
            string outputTexture = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
            @"res\textures\face\" + racialGender.SelectedItem.ToString().ToLower() + @"\" + RaceInfo.ModelRaces[RaceInfo.SubRaceToModelRace(subRaceListBox.SelectedIndex)].ToLower() + @"\"
            + (((subRaceListBox.SelectedIndex == 5 && racialGender.SelectedIndex == 0) || subRaceListBox.SelectedIndex == 11 ? 101 : 1)
            + faceNumberListBox.SelectedIndex) + (textureIsNormalMap.Checked ? "n" : "") + ".png");
            if (File.Exists(inputModel) && File.Exists(outputModel) && File.Exists(outputTexture)) {
                if (string.IsNullOrEmpty(makeupPath)) {
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.Filter = "Texture File|*.png;*.dds;*.bmp;**.tex;";
                    MessageBox.Show("Please select input texture");

                    if (openFileDialog.ShowDialog() == DialogResult.OK) {
                        makeupPath = openFileDialog.FileName;
                    }
                }
                if (!string.IsNullOrEmpty(makeupPath)) {
                    string path = ImageManipulation.AddSuffix(makeupPath, "_bake");
                    if (!textureIsNormalMap.Checked) {
                        XNormal.CallXNormal(inputModel, outputModel, makeupPath, path,
                        (subRaceListBox.SelectedIndex == 11 || subRaceListBox.SelectedIndex == 10) ? 2048 : 1024, 2048);
                    } else {
                        XNormal.CallXNormal(inputModel, outputModel, makeupPath, path,
                       (subRaceListBox.SelectedIndex == 11 || subRaceListBox.SelectedIndex == 10) ? 1024 : 512, 1024);
                    }

                    Bitmap bitmap = null;
                    Bitmap alpha = null;
                    if (!skipUnderlayCheckBox.Checked) {
                        bitmap = TexLoader.ResolveBitmap(outputTexture, textureIsNormalMap.Checked);
                        alpha = ImageManipulation.ExtractAlpha(TexLoader.ResolveBitmap(outputTexture));
                    } else {
                        bitmap = TexLoader.ResolveBitmap(path);
                    }

                    Graphics graphics = Graphics.FromImage(bitmap);
                    Bitmap makeup = new Bitmap(TexLoader.ResolveBitmap(path), bitmap.Width, bitmap.Height);
                    Bitmap lipCorrectionBitmap = TexLoader.ResolveBitmap(lipCorrectionMap);
                    Bitmap eyeCorrectionBitmap = TexLoader.ResolveBitmap(eyeCorrectionMap);
                    if (!skipUnderlayCheckBox.Checked) {
                        Bitmap teeth = bitmap.Clone(new Rectangle(0, 0, (int)((float)bitmap.Height * 0.24853515625f), (int)((float)bitmap.Height * 0.1372549019607843f)), bitmap.PixelFormat);
                        if ((subRaceListBox.SelectedIndex == 11 || subRaceListBox.SelectedIndex == 10)) {
                            ImageManipulation.ClearOldHorns(makeup);
                        }
                        graphics.DrawImage(makeup, new Point(0, 0));
                        graphics.DrawImage(teeth, new Point(0, 0));
                    } else {
                        ImageManipulation.EraseTeeth(bitmap);
                    }
                    if (!skipLipCorrection.Checked && !textureIsNormalMap.Checked) {
                        Bitmap lips = ImageManipulation.LipCorrection(lipCorrectionBitmap, new Bitmap(bitmap), true);
                        for (int i = 0; i < 4; i++) {
                            graphics.DrawImage(lips, new Point(0, 0));
                        }
                        Bitmap eyes = ImageManipulation.EyeCorrection(eyeCorrectionBitmap, new Bitmap(bitmap), true);
                        for (int i = 0; i < 4; i++) {
                            graphics.DrawImage(eyes, new Point(0, 0));
                        }
                    }
                    if (textureIsNormalMap.Checked) {
                        ImageManipulation.MergeAlphaToRGB(alpha, bitmap).Save(lastItem = ImageManipulation.AddSuffix(path, "_final_normal"));
                    } else {
                        ImageManipulation.MergeAlphaToRGB(alpha, bitmap).Save(lastItem = ImageManipulation.AddSuffix(path, "_final"));
                    }
                    File.Delete(path);

                }
            } else {
                MessageBox.Show("Assets for the current selection do no exist. Likely missing files from the base installation of Loose Texture Compiler");
            }
            convertMakeupButton.Enabled = true;
        }

        private void filePath_DragEnter(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void filePath_DragDrop(object sender, DragEventArgs e) {
            lastItem = null;
            foreach (string file in ((string[])e.Data.GetData(DataFormats.FileDrop, false))) {
                if (CheckExtentions(file)) {
                    ConvertMakeup(file);
                    lastItem = file;
                } else {
                    MessageBox.Show("This is not a media file this tool supports.", ParentForm.Text);
                }
            }
            MessageBox.Show("Makeup Salvaging Operation Complete!");
            if (!string.IsNullOrEmpty(lastItem)) {
                OpenFolder(lastItem);
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
        private void LegacyMakeupSalvager_Load(object sender, EventArgs e) {
            AutoScaleDimensions = new SizeF(96, 96);
            racialGender.SelectedIndex = faceNumberListBox.SelectedIndex = subRaceListBox.SelectedIndex = 0;
        }
        public void OpenFolder(string folder) {
            string path = Path.GetDirectoryName(folder);
            ProcessStartInfo ProcessInfo;
            Process Process; ;
            try {
                Directory.CreateDirectory(path);
            } catch {
            }
            ProcessInfo = new ProcessStartInfo("explorer.exe", @"""" + path + @"""");
            ProcessInfo.UseShellExecute = true;
            Process = Process.Start(ProcessInfo);
            lastItem = null;
        }

        private void textBox1_TextChanged(object sender, EventArgs e) {

        }
    }
}
