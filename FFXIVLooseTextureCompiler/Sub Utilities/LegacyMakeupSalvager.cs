using FFXIVLooseTextureCompiler.ImageProcessing;
using FFXIVLooseTextureCompiler.Racial;
using KVImage;
using LooseTextureCompilerCore;
using System.Diagnostics;
using System.Drawing.Imaging;
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
            string lipCorrectionMap = Path.Combine(GlobalPathStorage.OriginalBaseDirectory,
            @"res\textures\face\" + racialGender.SelectedItem.ToString().ToLower() + @"\correction.png");

            string eyeCorrectionMap = Path.Combine(GlobalPathStorage.OriginalBaseDirectory,
            @"res\textures\face\" + racialGender.SelectedItem.ToString().ToLower() + @"\eyecorrection.png");

            string inputModel = Path.Combine(GlobalPathStorage.OriginalBaseDirectory,
            @"res\model\face\" + racialGender.SelectedItem.ToString().ToLower() + @"\" +
            RaceInfo.ModelRaces[RaceInfo.SubRaceToModelRace(subRaceListBox.SelectedIndex)].ToLower() + @"\input\"
            + (1 + faceNumberListBox.SelectedIndex) + ".fbx");

            string outputModel = Path.Combine(GlobalPathStorage.OriginalBaseDirectory,
            @"res\model\face\" + racialGender.SelectedItem.ToString().ToLower() + @"\" +
            RaceInfo.ModelRaces[RaceInfo.SubRaceToModelRace(subRaceListBox.SelectedIndex)].ToLower() + @"\output\"
            + (1 + faceNumberListBox.SelectedIndex) + ".fbx");

            string outputTexture = Path.Combine(GlobalPathStorage.OriginalBaseDirectory,
            @"res\textures\face\" + racialGender.SelectedItem.ToString().ToLower() + @"\" +
            RaceInfo.ModelRaces[RaceInfo.SubRaceToModelRace(subRaceListBox.SelectedIndex)].ToLower() + @"\"
            + (((subRaceListBox.SelectedIndex == 7 && racialGender.SelectedIndex == 0) || subRaceListBox.SelectedIndex == 11 ? (textureIsNormalMap.Checked ? 1 : 101) : 1)
            + faceNumberListBox.SelectedIndex) + (textureIsNormalMap.Checked ? "n" : "") + ".png");

            if (File.Exists(inputModel) && File.Exists(outputModel) && File.Exists(outputTexture)) {
                if (string.IsNullOrEmpty(makeupPath)) {
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.Filter = "Texture File|*.png;*.tga;*.dds;*.bmp;*.tex;";
                    MessageBox.Show("Please select input texture");

                    if (openFileDialog.ShowDialog() == DialogResult.OK) {
                        makeupPath = openFileDialog.FileName;
                    }
                }
                string tempPath = ImageManipulation.ReplaceExtension(ImageManipulation.AddSuffix(makeupPath, "_backup"), ".png");
                TexIO.SaveBitmap(TexIO.ResolveBitmap(makeupPath), tempPath);
                makeupPath = tempPath;
                if (!string.IsNullOrEmpty(makeupPath)) {
                    string path = ImageManipulation.AddSuffix(makeupPath, "_bake");
                    if (!textureIsNormalMap.Checked) {
                        XNormal.CallXNormal(inputModel, outputModel, makeupPath, path, false,
                        (subRaceListBox.SelectedIndex == 11 || subRaceListBox.SelectedIndex == 10) ? 2048 : 1024, 2048);
                    } else {
                        XNormal.CallXNormal(inputModel, outputModel, makeupPath, path, false,
                       (subRaceListBox.SelectedIndex == 11 || subRaceListBox.SelectedIndex == 10) ? 1024 : 512, 1024);
                    }
                    Bitmap bitmap = null;
                    Bitmap alpha = null;
                    if (!skipUnderlayCheckBox.Checked) {
                        bitmap = TexIO.ResolveBitmap(outputTexture, textureIsNormalMap.Checked);
                        alpha = ImageManipulation.ExtractAlpha(TexIO.ResolveBitmap(outputTexture));
                    } else {
                        bitmap = TexIO.ResolveBitmap(path);
                    }
                    Bitmap canvas = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format32bppArgb);
                    Graphics graphics = Graphics.FromImage(canvas);
                    graphics.Clear(Color.Transparent);
                    graphics.DrawImage(bitmap, new Point(0, 0));
                    Bitmap makeup = ImageManipulation.Resize(TexIO.ResolveBitmap(path), bitmap.Width, bitmap.Height);
                    Bitmap lipCorrectionBitmap = TexIO.ResolveBitmap(lipCorrectionMap);
                    Bitmap eyeCorrectionBitmap = TexIO.ResolveBitmap(eyeCorrectionMap);
                    if (!skipUnderlayCheckBox.Checked) {
                        Bitmap teeth = TexIO.Clone(bitmap, new Rectangle(0, 0, (int)((float)bitmap.Height * 0.24853515625f), (int)((float)bitmap.Height * 0.1372549019607843f)));
                        //if ((subRaceListBox.SelectedIndex == 11 || subRaceListBox.SelectedIndex == 10)) {
                        //    makeup = ImageManipulation.ClearOldHorns(makeup);
                        //}
                        graphics.DrawImage(makeup, new Point(0, 0));
                        graphics.DrawImage(teeth, new Point(0, 0));
                    } else {
                        ImageManipulation.EraseTeeth(canvas);
                    }
                    if (!skipLipCorrection.Checked && !textureIsNormalMap.Checked) {
                        Bitmap lips = ImageManipulation.LipCorrection(lipCorrectionBitmap, TexIO.NewBitmap(canvas), true);
                        for (int i = 0; i < 4; i++) {
                            graphics.DrawImage(lips, new Point(0, 0));
                        }
                        Bitmap eyes = ImageManipulation.EyeCorrection(eyeCorrectionBitmap, TexIO.NewBitmap(canvas), true);
                        for (int i = 0; i < 4; i++) {
                            graphics.DrawImage(eyes, new Point(0, 0));
                        }
                    }
                    if (!textureIsNormalMap.Checked) {
                        if (alpha != null) {
                            TexIO.SaveBitmap(ImageManipulation.MergeAlphaToRGB(alpha, canvas), lastItem = ImageManipulation.ReplaceExtension(ImageManipulation.AddSuffix(path, textureIsNormalMap.Checked ? "_final_normal" : "_final"), ".png"));
                        } else {
                            TexIO.SaveBitmap(canvas, lastItem = ImageManipulation.ReplaceExtension(ImageManipulation.AddSuffix(path, textureIsNormalMap.Checked ? "_final_normal" : "_final"), ".png"));
                        }
                    } else {
                        TexIO.SaveBitmap(ImageManipulation.MergeGrayscalesToRGBA(TexIO.NewBitmap(canvas), TexIO.NewBitmap(canvas), TexIO.NewBitmap(bitmap), alpha), lastItem = ImageManipulation.ReplaceExtension(ImageManipulation.AddSuffix(path, textureIsNormalMap.Checked ? "_final_normal" : "_final"), ".png"));
                    }
                    //File.Delete(path);
                }
            } else {
                MessageBox.Show("Assets for the current selection do no exist. If this is a valid selection, its possible Loose Texture Compiler needs to be re-installed.");
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
            WFTranslator.TranslateControl(this);
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

        private void skipLipCorrection_CheckedChanged(object sender, EventArgs e) {

        }
    }
}
