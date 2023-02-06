using FFBardMusicPlayer.FFXIV;
using FFXIVLooseTextureCompiler.DataTypes;
using FFXIVLooseTextureCompiler.ImageProcessing;
using FFXIVLooseTextureCompiler.PathOrganization;
using FFXIVVoicePackCreator.Json;
using Lumina.Data.Files;
using Newtonsoft.Json;
using OtterTex;
using Penumbra.Import.Dds;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using TypingConnector;

namespace FFXIVLooseTextureCompiler {
    public partial class MainWindow : Form {
        FFXIVHook Hook = new FFXIVHook();
        private RaceCode raceCodeBody;
        private RaceCode raceCodeFace;
        private List<RacialBodyIdentifiers> bodyIdentifiers = new List<RacialBodyIdentifiers>();
        private int lastRaceIndex;
        private string? penumbraModPath;
        private string jsonFilepath;
        private string metaFilePath;
        private bool enteredField;

        public readonly string _defaultModName = "";
        public string _defaultAuthor = "FFXIV Loose Texture Compiler";
        public readonly string _defaultDescription = "Exported by FFXIV Loose Texture Compiler";
        public string _defaultWebsite = "https://github.com/Sebane1/FFXIVLooseTextureCompiler";
        private string savePath;
        private bool hasSaved;
        private bool foundInstance;
        private bool generatedOnce;
        private int fileCount;
        private Dictionary<string, Bitmap> normalCache;
        private Dictionary<string, Bitmap> multiCache;
        Dictionary<string, FileSystemWatcher> watchers = new Dictionary<string, FileSystemWatcher>();
        private bool exportingTex;

        public bool HasSaved {
            get => hasSaved; set {
                hasSaved = value;
                if (!hasSaved) {
                    Text = Application.ProductName + " " + Application.ProductVersion + (!string.IsNullOrWhiteSpace(savePath) ? $" ({savePath})*" : "*");
                } else {
                    Text = Application.ProductName + " " + Application.ProductVersion + (!string.IsNullOrWhiteSpace(savePath) ? $" ({savePath})" : "");
                }
            }
        }
        public string VersionText { get; private set; }

        public MainWindow() {
            AutoScaleDimensions = new SizeF(96, 96);
            InitializeComponent();
            GetAuthorWebsite();
            GetAuthorName();
            GetPenumbraPath();
            Text += " " + Application.ProductVersion;
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void Form1_Load(object sender, EventArgs e) {
            VersionText = Application.ProductName + " " + Application.ProductVersion;
            AutoScaleDimensions = new SizeF(96, 96);
            diffuse.FilePath.Enabled = false;
            normal.FilePath.Enabled = false;
            multi.FilePath.Enabled = false;
            raceCodeBody = new RaceCode();
            raceCodeFace = new RaceCode();
            raceCodeBody.Masculine = new string[] {
            "0101","0301","0101","0101","0901","1101","1301","1301","1501","1701"};
            raceCodeBody.Feminine = new string[] {
            "0201","0401","0201","0201","0401","1101","1401","1401","0000","1801"};
            raceCodeFace.Masculine = new string[] {
                "0101", "0301", "0501", "0501", "0701",
                "0701", "0901", "0901", "1101", "1101",
                "1301", "1301", "1501", "1501", "1701", "1701" };
            raceCodeFace.Feminine = new string[] {
                "0201", "0401", "0601", "0601", "0801",
                "0801", "1001", "1001", "1201", "1201",
                "1401", "1401", "0000", "0000", "1801", "1801" };

            bodyIdentifiers.Add(new RacialBodyIdentifiers("VANILLA", new List<string>() { "", "", "", "", "", "", "", "", "", "" }));
            bodyIdentifiers.Add(new RacialBodyIdentifiers("BIBO+", new List<string>() { "midlander", "highlander", "midlander", "midlander", "highlander", "", "raen", "xaela", "", "viera" }));
            bodyIdentifiers.Add(new RacialBodyIdentifiers("EVE", new List<string>() { "middie", "buffie", "middie", "middie", "buffie", "", "lizard", "lizard2", "", "bunny" }));
            bodyIdentifiers.Add(new RacialBodyIdentifiers("GEN3", new List<string>() { "mid", "high", "mid", "mid", "high", "", "raen", "xaela", "", "viera" }));
            bodyIdentifiers.Add(new RacialBodyIdentifiers("SCALE+", new List<string>() { "", "", "", "", "", "", "raen", "xaela", "", "" }));
            bodyIdentifiers.Add(new RacialBodyIdentifiers("TBSE/HRBODY", new List<string>() { "", "", "", "", "", "", "", "", "", "" }));
            bodyIdentifiers.Add(new RacialBodyIdentifiers("TAIL", new List<string>() { "1", "2", "3", "4", "5", "6", "7", "8", "", "" }));
            baseBodyList.SelectedIndex = genderListBody.SelectedIndex = raceList.SelectedIndex = tailList.SelectedIndex = subRaceList.SelectedIndex = faceType.SelectedIndex = facePart.SelectedIndex = facePaint.SelectedIndex = generationType.SelectedIndex = 0;
            for (int i = 0; i < 99; i++) {
                facePaint.Items.Add((i + 1) + "");
            }
            CleanDirectory();
            CheckForCommandArguments();
        }
        private void RefreshFFXIVInstance() {
            var processes = new List<Process>(Process.GetProcessesByName("ffxiv_dx11"));
            foundInstance = false;
            if (processes.Count > 0) {
                foundInstance = true;
                Hook.Hook(processes[0], false);
            }
        }
        private void generateButton_Click(object sender, EventArgs e) {
            if (!exportingTex && !generationCooldown.Enabled) {
                exportingTex = true;
                exportPanel.Visible = true;
                exportPanel.BringToFront();
                if (string.IsNullOrEmpty(penumbraModPath)) {
                    ConfigurePenumbraModFolder();
                }
                if (!string.IsNullOrWhiteSpace(modNameTextBox.Text)) {
                    string modPath = Path.Combine(penumbraModPath, modNameTextBox.Text);
                    jsonFilepath = Path.Combine(modPath, "default_mod.json");
                    metaFilePath = Path.Combine(modPath, "meta.json");
                    if (Directory.Exists(modPath)) {
                        try {
                            Directory.Delete(modPath, true);
                        } catch {

                        }
                    } else {
                        generatedOnce = false;
                    }
                    Directory.CreateDirectory(modPath);
                    int i = 0;
                    fileCount = 0;
                    exportProgress.BringToFront();
                    exportProgress.Maximum = materialList.Items.Count * 3;
                    exportProgress.Visible = true;
                    Refresh();
                    Dictionary<string, List<MaterialSet>> groups = new Dictionary<string, List<MaterialSet>>();
                    normalCache = new Dictionary<string, Bitmap>();
                    multiCache = new Dictionary<string, Bitmap>();
                    foreach (MaterialSet materialSet in materialList.Items) {
                        if (!groups.ContainsKey(materialSet.MaterialGroupName)) {
                            groups.Add(materialSet.MaterialGroupName, new List<MaterialSet>() { materialSet });
                        } else {
                            groups[materialSet.MaterialGroupName].Add(materialSet);
                        }
                    }
                    foreach (List<MaterialSet> materialSets in groups.Values) {
                        Group group = new Group(materialSets[0].MaterialGroupName.Replace(@"/", "-").Replace(@"\", "-"), "", 0, "Multi", 0);
                        Option option = null;
                        foreach (MaterialSet materialSet in materialSets) {
                            string diffuseBodyDiskPath = !string.IsNullOrEmpty(materialSet.InternalDiffusePath) ? Path.Combine(modPath, materialSet.InternalDiffusePath.Replace("/", @"\")) : "";
                            string normalBodyDiskPath = !string.IsNullOrEmpty(materialSet.InternalNormalPath) ? Path.Combine(modPath, materialSet.InternalNormalPath.Replace("/", @"\")) : "";
                            string multiBodyDiskPath = !string.IsNullOrEmpty(materialSet.InternalMultiPath) ? Path.Combine(modPath, materialSet.InternalMultiPath.Replace("/", @"\")) : "";
                            switch (generationType.SelectedIndex) {
                                case 0:
                                    if (!string.IsNullOrEmpty(materialSet.Diffuse) && !string.IsNullOrEmpty(materialSet.InternalDiffusePath)) {
                                        option = new Option((materialSets.Count > 1 ? materialSet.MaterialSetName + " " : "") + (materialSet.MaterialSetName.ToLower().Contains("eye") ? "Normal" : "Diffuse"), 0);
                                        option.Files.Add(materialSet.InternalDiffusePath, AppendNumber(materialSet.InternalDiffusePath.Replace("/", @"\"), fileCount));
                                        group.Options.Add(option);
                                        if ((materialSet.MaterialSetName.ToLower().Contains("eye") && bakeNormals.Checked)) {
                                            ExportTex(materialSet.Diffuse, AppendNumber(diffuseBodyDiskPath, fileCount++), ExportType.Normal, materialSet.Diffuse);
                                        } else {
                                            ExportTex(materialSet.Diffuse, AppendNumber(diffuseBodyDiskPath, fileCount++));
                                        }
                                        exportProgress.Increment(1);
                                        Refresh();
                                        Application.DoEvents();
                                    } else {
                                        exportProgress.Maximum--;
                                    }
                                    if (!string.IsNullOrEmpty(materialSet.Normal) && !string.IsNullOrEmpty(materialSet.InternalNormalPath)) {
                                        option = new Option((materialSets.Count > 1 ? materialSet.MaterialSetName + " " : "") + (materialSet.MaterialSetName.ToLower().Contains("eye") ? "Multi" : "Normal"), 0);
                                        option.Files.Add(materialSet.InternalNormalPath, AppendNumber(materialSet.InternalNormalPath.Replace("/", @"\"), fileCount));
                                        group.Options.Add(option);
                                        if (bakeNormals.Checked && !materialSet.MaterialSetName.ToLower().Contains("eye")) {
                                            ExportTex(materialSet.Normal, AppendNumber(normalBodyDiskPath, fileCount++), ExportType.MergeNormal, materialSet.Diffuse, materialSet.NormalMask);
                                        } else {
                                            ExportTex(materialSet.Normal, AppendNumber(normalBodyDiskPath, fileCount++));
                                        }
                                        exportProgress.Increment(1);
                                        Refresh();
                                    } else if (!string.IsNullOrEmpty(materialSet.Diffuse) && !string.IsNullOrEmpty(materialSet.InternalNormalPath) && bakeNormals.Checked && !(materialSet.MaterialSetName.ToLower().Contains("eye"))) {
                                        option = new Option((materialSets.Count > 1 ? materialSet.MaterialSetName + " " : "") + (materialSet.MaterialSetName.ToLower().Contains("eye") ? "Multi" : "Normal"), 0);
                                        option.Files.Add(materialSet.InternalNormalPath, AppendNumber(materialSet.InternalNormalPath.Replace("/", @"\"), fileCount));
                                        group.Options.Add(option);
                                        ExportTex(materialSet.Diffuse, AppendNumber(normalBodyDiskPath, fileCount++), ExportType.Normal);
                                        exportProgress.Increment(1);
                                        Refresh();
                                        Application.DoEvents();
                                    } else {
                                        exportProgress.Maximum--;
                                    }
                                    if (!string.IsNullOrEmpty(materialSet.Multi) && !string.IsNullOrEmpty(materialSet.InternalMultiPath)) {
                                        option = new Option((materialSets.Count > 1 ? materialSet.MaterialSetName + " " : "") + (materialSet.MaterialSetName.ToLower().Contains("eye") ? "Catchlight" : "Multi"), 0);
                                        option.Files.Add(materialSet.InternalMultiPath, AppendNumber(materialSet.InternalMultiPath.Replace("/", @"\"), fileCount));
                                        group.Options.Add(option);
                                        ExportTex(materialSet.Multi, AppendNumber(multiBodyDiskPath, fileCount++));
                                        exportProgress.Increment(1);
                                        Refresh();
                                        Application.DoEvents();
                                    } else if (!string.IsNullOrEmpty(materialSet.Diffuse) && !string.IsNullOrEmpty(materialSet.InternalMultiPath) && generateMultiCheckBox.Checked && !(materialSet.MaterialSetName.ToLower().Contains("eye"))) {
                                        option = new Option((materialSets.Count > 1 ? materialSet.MaterialSetName + " " : "") + (materialSet.MaterialSetName.ToLower().Contains("eye") ? "Catchlight" : "Multi"), 0);
                                        option.Files.Add(materialSet.InternalMultiPath, AppendNumber(materialSet.InternalMultiPath.Replace("/", @"\"), fileCount));
                                        group.Options.Add(option);
                                        ExportTex(materialSet.Diffuse, AppendNumber(multiBodyDiskPath, fileCount), ExportType.MultiFace);
                                        exportProgress.Increment(1);
                                        Refresh();
                                        Application.DoEvents();
                                    } else {
                                        exportProgress.Maximum--;
                                    }
                                    break;
                                case 1:
                                    option = new Option(materialSet.MaterialSetName == materialSet.MaterialGroupName ? "Enable" : materialSet.MaterialSetName, 0);
                                    group.Options.Add(option);
                                    if (!string.IsNullOrEmpty(materialSet.Diffuse) && !string.IsNullOrEmpty(materialSet.InternalDiffusePath)) {
                                        ExportTex(materialSet.Diffuse, AppendNumber(diffuseBodyDiskPath, fileCount));
                                        option.Files.Add(materialSet.InternalDiffusePath, AppendNumber(materialSet.InternalDiffusePath.Replace("/", @"\"), fileCount++));
                                        exportProgress.Increment(1);
                                        Refresh();
                                        Application.DoEvents();
                                    } else {
                                        exportProgress.Maximum--;
                                    }
                                    if (!string.IsNullOrEmpty(materialSet.Normal) && !string.IsNullOrEmpty(materialSet.InternalNormalPath)) {
                                        if (!bakeNormals.Checked) {
                                            ExportTex(materialSet.Normal, AppendNumber(normalBodyDiskPath, fileCount));
                                        } else {
                                            ExportTex(materialSet.Normal, AppendNumber(normalBodyDiskPath, fileCount), ExportType.MergeNormal, materialSet.Diffuse, materialSet.NormalMask);
                                        }
                                        option.Files.Add(materialSet.InternalNormalPath, AppendNumber(materialSet.InternalNormalPath.Replace("/", @"\"), fileCount++));
                                        exportProgress.Increment(1);
                                        Refresh();
                                        Application.DoEvents();
                                    } else if (!string.IsNullOrEmpty(materialSet.Diffuse) && !string.IsNullOrEmpty(materialSet.InternalNormalPath) && bakeNormals.Checked) {
                                        ExportTex(materialSet.Diffuse, AppendNumber(normalBodyDiskPath, fileCount), ExportType.Normal);
                                        option.Files.Add(materialSet.InternalNormalPath, AppendNumber(materialSet.InternalNormalPath.Replace("/", @"\"), fileCount++));
                                        exportProgress.Increment(1);
                                        Refresh();
                                        Application.DoEvents();
                                    } else {
                                        exportProgress.Maximum--;
                                    }
                                    if (!string.IsNullOrEmpty(materialSet.Multi) && !string.IsNullOrEmpty(materialSet.InternalMultiPath)) {
                                        ExportTex(materialSet.Multi, AppendNumber(multiBodyDiskPath, fileCount));
                                        option.Files.Add(materialSet.InternalMultiPath, AppendNumber(materialSet.InternalMultiPath.Replace("/", @"\"), fileCount++));
                                        exportProgress.Increment(1);
                                        Refresh();
                                        Application.DoEvents();
                                    } else if (!string.IsNullOrEmpty(materialSet.Diffuse) && !string.IsNullOrEmpty(materialSet.InternalMultiPath) && generateMultiCheckBox.Checked) {
                                        ExportTex(materialSet.Diffuse, AppendNumber(multiBodyDiskPath, fileCount), ExportType.MultiFace);
                                        option.Files.Add(materialSet.InternalMultiPath, AppendNumber(materialSet.InternalMultiPath.Replace("/", @"\"), fileCount++));
                                        exportProgress.Increment(1);
                                        Refresh();
                                        Application.DoEvents();
                                    } else {
                                        exportProgress.Maximum--;
                                    }
                                    break;
                            }
                        }
                        if (group.Options.Count > 0) {
                            string groupPath = Path.Combine(modPath, $"group_" + i++ + $"_{group.Name.ToLower()}.json");
                            ExportGroup(groupPath, group);
                        }
                    }
                    foreach (Bitmap value in normalCache.Values) {
                        value.Dispose();
                    }
                    foreach (Bitmap value in multiCache.Values) {
                        value.Dispose();
                    }
                    ExportJson();
                    ExportMeta();
                    if (generatedOnce) {
                        Hook.SendSyncKey(Keys.Enter);
                        Thread.Sleep(500);
                        Hook.SendString(@"/penumbra redraw self");
                        Thread.Sleep(200);
                        Hook.SendSyncKey(Keys.Enter);
                    } else {
                        Hook.SendSyncKey(Keys.Enter);
                        Thread.Sleep(500);
                        Hook.SendString(@"/penumbra reload");
                        Thread.Sleep(200);
                        Hook.SendSyncKey(Keys.Enter);
                        Thread.Sleep(2000);
                        Hook.SendSyncKey(Keys.Enter);
                        Thread.Sleep(500);
                        Hook.SendString(@"/penumbra redraw self");
                        Thread.Sleep(200);
                        Hook.SendSyncKey(Keys.Enter);
                        generatedOnce = true;
                    }
                    TopMost = true;
                    BringToFront();
                    TopMost = false;
                    generateButton.Enabled = false;
                    generationCooldown.Start();
                    // generateButton.Text = "Success!";
                    exportProgress.Visible = false;
                    exportProgress.Value = 0;
                    exportingTex = false;
                    //MessageBox.Show("Export succeeded!");
                    exportPanel.Visible = false;
                } else {
                    MessageBox.Show("Please enter a mod name!");
                }
            }
        }
        private void MainWindow_KeyDown(object sender, KeyEventArgs e) {
            if (e.Control && e.KeyCode == Keys.S) {
                Save();
            }
        }
        protected override bool ProcessCmdKey(ref Message message, Keys keys) {
            switch (keys) {
                case Keys.S | Keys.Control:
                    // ... Process Shift+Ctrl+Alt+B ...
                    Save();
                    return true; // signal that we've processed this key
            }

            // run base implementation
            return base.ProcessCmdKey(ref message, keys);
        }
        public enum ExportType {
            None,
            Normal,
            Multi,
            MultiFace,
            MergeNormal
        }
        public void ExportTex(string inputFile, string outputFile, ExportType exportType = ExportType.None, string diffuseNormal = "", string normalMask = "") {
            byte[] data = new byte[0];
            int contrast = 500;
            int contrastFace = 100;
            using (MemoryStream stream = new MemoryStream()) {
                switch (exportType) {
                    case ExportType.None:
                        using (Bitmap bitmap = TexLoader.ResolveBitmap(inputFile)) {
                            if (bitmap != null) {
                                bitmap.Save(stream, ImageFormat.Png);
                                stream.Flush();
                                stream.Position = 0;
                                TextureImporter.PngToTex(stream, out data);
                            }
                        }
                        break;
                    case ExportType.Normal:
                        if (!normalCache.ContainsKey(inputFile)) {
                            using (Bitmap bitmap = TexLoader.ResolveBitmap(inputFile)) {
                                if (bitmap != null) {
                                    using (Bitmap target = new Bitmap(bitmap.Size.Width, bitmap.Size.Height)) {
                                        Graphics g = Graphics.FromImage(target);
                                        g.Clear(Color.White);
                                        g.DrawImage(bitmap, 0, 0, bitmap.Width, bitmap.Height);
                                        Bitmap output = null;
                                        if (File.Exists(normalMask)) {
                                            using (Bitmap normalMaskBitmap = TexLoader.ResolveBitmap(normalMask)) {
                                                output = Normal.Calculate(target, normalMaskBitmap);
                                            }
                                        } else {
                                            output = Normal.Calculate(target);
                                        }
                                        output.Save(stream, ImageFormat.Png);
                                        normalCache.Add(inputFile, output);
                                    }
                                }
                            }
                        } else {
                            normalCache[inputFile].Save(stream, ImageFormat.Png);
                        }
                        break;
                    case ExportType.MultiFace:
                        if (!multiCache.ContainsKey(inputFile)) {
                            using (Bitmap bitmap = TexLoader.ResolveBitmap(inputFile)) {
                                if (bitmap != null) {
                                    Bitmap multi = MultiplyFilter.MultiplyImage(Brightness.BrightenImage(Grayscale.MakeGrayscale3(bitmap)), 255, 126, 0);
                                    multi.Save(stream, ImageFormat.Png);
                                    multiCache.Add(inputFile, multi);
                                }
                            }
                        } else {
                            multiCache[inputFile].Save(stream, ImageFormat.Png);
                        }
                        stream.Position = 0;
                        TextureImporter.PngToTex(stream, out data);
                        break;
                    case ExportType.MergeNormal:
                        if (!normalCache.ContainsKey(diffuseNormal)) {
                            using (Bitmap bitmap = TexLoader.ResolveBitmap(diffuseNormal)) {
                                if (bitmap != null) {
                                    using (Bitmap target = new Bitmap(bitmap.Size.Width, bitmap.Size.Height)) {
                                        Bitmap output = null;
                                        if (File.Exists(normalMask)) {
                                            using (Bitmap normalMaskBitmap = TexLoader.ResolveBitmap(normalMask)) {
                                                output = ImageManipulation.MergeNormals(inputFile, bitmap, target, normalMaskBitmap, diffuseNormal);
                                            }
                                        } else {
                                            output = ImageManipulation.MergeNormals(inputFile, bitmap, target, null, diffuseNormal);
                                        }
                                        output.Save(stream, ImageFormat.Png);
                                        normalCache.Add(diffuseNormal, output);
                                    }
                                }
                            }
                        } else {
                            normalCache[diffuseNormal].Save(stream, ImageFormat.Png);
                        }
                        stream.Position = 0;
                        if (stream.Length > 0) {
                            TextureImporter.PngToTex(stream, out data);
                        }
                        break;
                }
            }
            if (data.Length > 0) {
                Directory.CreateDirectory(Path.GetDirectoryName(outputFile));
                File.WriteAllBytes(outputFile, data);
            }
        }
        private void ExportJson() {
            string jsonText = @"{
  ""Name"": """",
  ""Priority"": 0,
  ""Files"": { },
  ""FileSwaps"": { },
  ""Manipulations"": []
}";
            if (jsonFilepath != null) {
                using (StreamWriter writer = new StreamWriter(jsonFilepath)) {
                    writer.WriteLine(jsonText);
                }
            }
        }
        private void ExportGroup(string path, Group group) {
            if (path != null) {
                if (group.Options.Count > 0) {
                    using (StreamWriter file = File.CreateText(path)) {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Formatting = Formatting.Indented;
                        serializer.Serialize(file, group);
                    }
                }
            }
        }
        public string AppendNumber(string value, int number) {
            return value.Replace(".tex", number + ".tex");
        }
        private void CleanDirectory() {
            foreach (string file in Directory.GetFiles(Application.StartupPath)) {
                if (file.Contains("WebView2") || file.Contains(".zip") || file.Contains(".pdb") || file.Contains(".config") || file.Contains(".xml") || file.Contains(".log") || file.Contains(".tmp") || file.Contains("ZipExtractor")) {
                    try {
                        File.Delete(file);
                    } catch {

                    }
                }
            }
        }
        private void ExportMeta() {
            string metaText = @"{
  ""FileVersion"": 3,
  ""Name"": """ + (!string.IsNullOrEmpty(modNameTextBox.Text) ? modNameTextBox.Text : "") + @""",
  ""Author"": """ + (!string.IsNullOrEmpty(modAuthorTextBox.Text) ? modAuthorTextBox.Text : "FFXIV Loose Texture Compiler") + @""",
  ""Description"": """ + (!string.IsNullOrEmpty(modDescriptionTextBox.Text) ? modDescriptionTextBox.Text : "Exported by FFXIV Loose Texture Compiler") + @""",
  ""Version"": """ + modVersionTextBox.Text + @""",
  ""Website"": """ + modWebsiteTextBox.Text + @""",
  ""ModTags"": []
}";
            if (metaFilePath != null) {
                using (StreamWriter writer = new StreamWriter(metaFilePath)) {
                    writer.WriteLine(metaText);
                }
            }
        }
        private void ConfigurePenumbraModFolder() {
            MessageBox.Show("Please configure where your penumbra mods folder is, we will remember it for all future exports. This should be where you have penumbra set to use mods.\r\n\r\nNote:\r\nAVOID MANUALLY CREATING ANY NEW FOLDERS IN YOUR PENUMBRA FOLDER, ONLY SELECT THE BASE FOLDER!", VersionText);
            FolderBrowserDialog folderSelect = new FolderBrowserDialog();
            if (folderSelect.ShowDialog() == DialogResult.OK) {
                penumbraModPath = folderSelect.SelectedPath;
                WritePenumbraPath(penumbraModPath);
            }
        }
        private string GetBodyMaterialPath(int material) {
            string result = "";
            string unique = (((string)raceList.Items[raceList.SelectedIndex]).Contains("Xaela") ? "0101" : "0001");
            switch (baseBodyList.SelectedIndex) {
                case 0:
                    // Vanila
                    if (material != 2) {
                        string genderCode = (genderListBody.SelectedIndex == 0 ? raceCodeBody.Masculine[raceList.SelectedIndex] : raceCodeBody.Feminine[raceList.SelectedIndex]);
                        result = @"chara/human/c" + genderCode + @"/obj/body/b" + unique + @"/texture/--c" + genderCode + "b" + unique + GetTextureType(material) + ".tex";
                    } else {
                        result = @"chara/common/texture/skin_m.tex";
                    }
                    break;
                case 1:
                    // Bibo+
                    if (raceList.SelectedIndex != 5) {
                        if (genderListBody.SelectedIndex == 1) {
                            result = @"chara/bibo/" + bodyIdentifiers[baseBodyList.SelectedIndex].RaceIdentifiers[raceList.SelectedIndex] + GetTextureType(material) + ".tex";
                        } else {
                            result = "";
                            MessageBox.Show("Bibo+ is only compatible with feminine characters", VersionText);
                        }
                    } else {
                        MessageBox.Show("Bibo+ is not compatible with lalafells", VersionText);
                    }
                    break;
                case 2:
                    // Eve
                    if (raceList.SelectedIndex != 5) {
                        if (genderListBody.SelectedIndex == 1) {
                            result = @"chara/human/c" + (genderListBody.SelectedIndex == 0 ? raceCodeBody.Masculine[raceList.SelectedIndex] : raceCodeBody.Feminine[raceList.SelectedIndex]) + @"/obj/body/b" + "0001" + @"/texture/eve2" + bodyIdentifiers[baseBodyList.SelectedIndex].RaceIdentifiers[raceList.SelectedIndex] + GetTextureType(material) + ".tex";
                        } else {
                            result = "";
                            MessageBox.Show("Eve is only compatible with feminine characters", VersionText);
                        }
                    } else {
                        MessageBox.Show("Eve is not compatible with lalafells", VersionText);
                    }
                    break;
                case 3:
                    // Gen3 and T&F3
                    if (raceList.SelectedIndex != 5) {
                        if (genderListBody.SelectedIndex == 1) {
                            result = @"chara/human/c" + (genderListBody.SelectedIndex == 0 ? raceCodeBody.Masculine[raceList.SelectedIndex] : raceCodeBody.Feminine[raceList.SelectedIndex]) + @"/obj/body/b" + unique + @"/texture/tfgen3" + bodyIdentifiers[baseBodyList.SelectedIndex].RaceIdentifiers[raceList.SelectedIndex] + "f" + GetTextureType(material) + ".tex";
                        } else {
                            result = "";
                            MessageBox.Show("Gen3 and T&F3 are only compatible with feminine characters", VersionText);
                        }
                    } else {
                        MessageBox.Show("Gen3 and T&F3 are not compatible with lalafells", VersionText);
                    }
                    break;
                case 4:
                    // Scales+
                    if (raceList.SelectedIndex != 5) {
                        if (raceList.SelectedIndex == 6 || raceList.SelectedIndex == 7) {
                            if (genderListBody.SelectedIndex == 1) {
                                result = @"chara/bibo/" + bodyIdentifiers[baseBodyList.SelectedIndex].RaceIdentifiers[raceList.SelectedIndex] + GetTextureType(material) + ".tex";
                            } else {
                                result = "";
                                MessageBox.Show("Scales+ is only compatible with feminine Au Ra characters", VersionText);
                            }
                        } else {
                            MessageBox.Show("Scales+ is only compatible with feminine Au Ra characters", VersionText);
                        }
                    } else {
                        MessageBox.Show("Scales+ is not compatible with lalafells", VersionText);
                    }
                    break;
                case 5:
                    if (raceList.SelectedIndex != 5) {
                        if (genderListBody.SelectedIndex == 0) {
                            // TBSE and HRBODY
                            result = @"chara/human/c" + (genderListBody.SelectedIndex == 0 ? raceCodeBody.Masculine[raceList.SelectedIndex] : raceCodeBody.Feminine[raceList.SelectedIndex]) + @"/obj/body/b" + (uniqueAuRa.Checked ? "0101" : "0001") + @"/texture/--c" + raceCodeBody.Masculine[raceList.SelectedIndex] + "b0001_b" + GetTextureType(material) + ".tex";
                        } else {
                            result = "";
                            MessageBox.Show("TBSE and HRBODY are only compatible with masculine characters", VersionText);
                        }
                    } else {
                        MessageBox.Show("TBSE and HRBODY are not compatible with lalafells", VersionText);
                    }
                    break;
                case 6:
                    string xaelaCheck = (raceList.SelectedIndex == 7 ? "0004" : "0104");
                    result = @"chara/human/c" + (genderListBody.SelectedIndex == 0 ? raceCodeBody.Masculine[raceList.SelectedIndex] : raceCodeBody.Feminine[raceList.SelectedIndex]) + @"/obj/tail/t" + xaelaCheck + @"/texture/--c" + raceCodeBody.Feminine[raceList.SelectedIndex] + "_etc_" + xaelaCheck + GetTextureType(material) + ".tex";
                    break;
            }
            return result;
        }
        public string GetFaceMaterialPath(int material) {
            if (material != 3) {
                string faceIdCheck = "000";
                string selectedText = (string)subRaceList.Items[subRaceList.SelectedIndex];
                if (selectedText.ToLower() == "the lost" || selectedText.ToLower() == "hellsgaurd" || selectedText.ToLower() == "highlander" || selectedText.ToLower() == "duskwight" || selectedText.ToLower() == "keeper" || selectedText.ToLower() == "dunesfolk" || (selectedText.ToLower() == "veena" && facePart.SelectedIndex == 1)) {
                    faceIdCheck = "010";
                }
                string subRace = (genderListBody.SelectedIndex == 0 ? raceCodeFace.Masculine[subRaceList.SelectedIndex] : raceCodeFace.Feminine[subRaceList.SelectedIndex]);
                return "chara/human/c" + subRace + "/obj/face/f" + faceIdCheck + (faceType.SelectedIndex + 1) + "/texture/--c" + subRace + "f" + faceIdCheck + (faceType.SelectedIndex + 1) + GetFacePart(facePart.SelectedIndex) + GetTextureType(material, true) + ".tex";
            } else {
                return "chara/common/texture/catchlight_1.tex";
            }
        }

        public string GetTextureType(int material, bool isface = false) {
            switch (material) {
                case 0:
                    return "_d";
                case 1:
                    return "_n";
                case 2:
                    if (baseBodyList.SelectedIndex == 1 && !isface) {
                        return "_m";
                    } else {
                        return "_s";
                    }
            }
            return null;
        }
        public string GetFacePart(int material) {
            switch (material) {
                case 0:
                    if (!asymCheckbox.Checked) {
                        return "_fac";
                    } else {
                        return "_fac_b";
                    }
                case 1:
                    return "_etc";
                case 2:
                    return "_iri";
                case 3:
                    return "_etc";
            }
            return null;
        }

        private void filePicker1_Load(object sender, EventArgs e) {

        }

        private void filePicker1_Load_1(object sender, EventArgs e) {

        }

        private void baseBodyList_SelectedIndexChanged(object sender, EventArgs e) {
            switch (baseBodyList.SelectedIndex) {
                case 0:
                    // Vanila
                    genderListBody.Enabled = true;
                    tailList.Enabled = false;
                    break;
                case 1:
                    // Bibo+
                    genderListBody.SelectedIndex = 1;
                    genderListBody.Enabled = false;
                    tailList.Enabled = false;
                    if (raceList.SelectedIndex == 5) {
                        raceList.SelectedIndex = 0;
                    }
                    break;
                case 2:
                    // Eve
                    genderListBody.SelectedIndex = 1;
                    genderListBody.Enabled = false;
                    tailList.Enabled = false;
                    if (raceList.SelectedIndex == 5) {
                        raceList.SelectedIndex = 0;
                    }
                    break;
                case 3:
                    // Gen3 and T&F3
                    genderListBody.SelectedIndex = 1;
                    genderListBody.Enabled = false;
                    tailList.Enabled = false;
                    if (raceList.SelectedIndex == 5) {
                        raceList.SelectedIndex = 0;
                    }
                    break;
                case 4:
                    // Scales+
                    if (raceList.SelectedIndex != 6 && raceList.SelectedIndex != 7) {
                        raceList.SelectedIndex = 6;
                    }
                    genderListBody.SelectedIndex = 1;
                    genderListBody.Enabled = false;
                    tailList.Enabled = false;
                    if (raceList.SelectedIndex == 5) {
                        raceList.SelectedIndex = 0;
                    }
                    break;
                case 5:
                    // TBSE and HRBODY
                    genderListBody.SelectedIndex = 0;
                    genderListBody.Enabled = false;
                    tailList.Enabled = false;
                    if (raceList.SelectedIndex == 5) {
                        raceList.SelectedIndex = 0;
                    }
                    break;
                case 6:
                    // Tails
                    if (raceList.SelectedIndex != 6 && raceList.SelectedIndex != 7) {
                        raceList.SelectedIndex = 6;
                    }
                    genderListBody.Enabled = true;
                    tailList.Enabled = true;
                    if (raceList.SelectedIndex == 5) {
                        raceList.SelectedIndex = 0;
                    }
                    break;
            }
        }

        private void raceList_SelectedIndexChanged(object sender, EventArgs e) {
            if (baseBodyList.SelectedIndex == 6) {
                if (raceList.SelectedIndex != 3 && raceList.SelectedIndex != 6 && raceList.SelectedIndex != 7) {
                    raceList.SelectedIndex = 3;
                    MessageBox.Show("Tail is only compatible with Xaela, and Raen", VersionText);
                }
            } else if (baseBodyList.SelectedIndex == 4) {
                if (raceList.SelectedIndex != 6 && raceList.SelectedIndex != 7) {
                    raceList.SelectedIndex = 6;
                    MessageBox.Show("Scales+ is only compatible with Xaela, and Raen", VersionText);
                }
            }
            if (baseBodyList.SelectedIndex > 0) {
                if (raceList.SelectedIndex == 5) {
                    raceList.SelectedIndex = lastRaceIndex;
                    MessageBox.Show("Lalafels are not compatible with the selected body", VersionText);
                }
            }
            lastRaceIndex = raceList.SelectedIndex;
        }


        private void genderList_SelectedIndexChanged(object sender, EventArgs e) {

        }
        public void GetPenumbraPath() {
            string dataPath = Application.UserAppDataPath.Replace(Application.ProductVersion, null);
            string path = Path.Combine(dataPath, @"PenumbraPath.config");
            if (File.Exists(path)) {
                using (StreamReader reader = new StreamReader(path)) {
                    penumbraModPath = reader.ReadLine();
                }
            }
        }
        public void WritePenumbraPath(string path) {
            string dataPath = Application.UserAppDataPath.Replace(Application.ProductVersion, null);
            using (StreamWriter writer = new StreamWriter(Path.Combine(dataPath, @"PenumbraPath.config"))) {
                writer.WriteLine(path);
            }
        }

        private void changePenumbraPathToolStripMenuItem_Click(object sender, EventArgs e) {
            ConfigurePenumbraModFolder();
        }

        private void donateButton_Click(object sender, EventArgs e) {
            try {
                Process.Start(new System.Diagnostics.ProcessStartInfo() {
                    FileName = "https://ko-fi.com/sebastina",
                    UseShellExecute = true,
                    Verb = "OPEN"
                });
            } catch {

            }
        }

        private void addBodyEditButton_Click(object sender, EventArgs e) {
            generatedOnce = false;
            MaterialSet materialSet = new MaterialSet();
            materialSet.MaterialSetName = baseBodyList.Text + ", " + genderListBody.Text + ", " + raceList.Text;
            materialSet.InternalDiffusePath = GetBodyMaterialPath(0);
            materialSet.InternalNormalPath = GetBodyMaterialPath(1);
            materialSet.InternalMultiPath = GetBodyMaterialPath(2);
            materialList.Items.Add(materialSet);
            HasSaved = false;
        }

        private void addFaceButton_Click(object sender, EventArgs e) {
            generatedOnce = false;
            MaterialSet materialSet = new MaterialSet();
            materialSet.MaterialSetName = facePart.Text + (facePart.SelectedIndex == 4 ? " " + (facePaint.SelectedIndex + 1) : "") + ", " + (facePart.SelectedIndex != 4 ? genderListBody.Text : "Unisex") + ", " + (facePart.SelectedIndex != 4 ? subRaceList.Text : "Multi Race") + ", " + (facePart.SelectedIndex != 4 ? faceType.Text : "Multi Face");
            switch (facePart.SelectedIndex) {
                default:
                    materialSet.InternalDiffusePath = GetFaceMaterialPath(0);
                    materialSet.InternalNormalPath = GetFaceMaterialPath(1);
                    materialSet.InternalMultiPath = GetFaceMaterialPath(2);
                    break;
                case 2:
                    materialSet.InternalDiffusePath = GetFaceMaterialPath(1);
                    materialSet.InternalNormalPath = GetFaceMaterialPath(2);
                    materialSet.InternalMultiPath = GetFaceMaterialPath(3);
                    break;
                case 4:
                    materialSet.InternalDiffusePath = "chara/common/texture/decal_face/_decal_" + (facePaint.SelectedIndex + 1) + ".tex";
                    break;

            }
            materialList.Items.Add(materialSet);
            HasSaved = false;
        }

        private void materialList_SelectedIndexChanged(object sender, EventArgs e) {
            if (materialList.SelectedIndex == -1) {
                currentEditLabel.Text = "Please select a texture set to start importing";
                diffuse.Enabled = false;
                normal.Enabled = false;
                multi.Enabled = false;
                mask.Enabled = false;
            } else {
                MaterialSet materialSet = (materialList.Items[materialList.SelectedIndex] as MaterialSet);
                currentEditLabel.Text = "Editing: " + materialSet.MaterialSetName;
                diffuse.CurrentPath = materialSet.Diffuse;
                normal.CurrentPath = materialSet.Normal;
                multi.CurrentPath = materialSet.Multi;
                mask.CurrentPath = materialSet.NormalMask;

                diffuse.Enabled = !string.IsNullOrEmpty(materialSet.InternalDiffusePath);
                normal.Enabled = !string.IsNullOrEmpty(materialSet.InternalNormalPath);
                multi.Enabled = !string.IsNullOrEmpty(materialSet.InternalMultiPath);
                mask.Enabled = bakeNormals.Checked;

                if (materialSet.MaterialSetName.ToLower().Contains("eye")) {
                    diffuse.LabelName.Text = "normal";
                    normal.LabelName.Text = "multi";
                    multi.LabelName.Text = "catchlight";
                } else {
                    diffuse.LabelName.Text = "diffuse";
                    normal.LabelName.Text = "normal";
                    multi.LabelName.Text = "multi";
                }
            }
        }
        public void SetPaths() {
            if (materialList.SelectedIndex != -1) {
                MaterialSet materialSet = (materialList.Items[materialList.SelectedIndex] as MaterialSet);
                string directory = Path.GetDirectoryName(materialSet.Diffuse);
                if (!string.IsNullOrWhiteSpace(directory)) {
                    if (watchers.ContainsKey(directory)) {
                        if (materialSet.Diffuse != diffuse.CurrentPath) {
                            watchers[directory].Dispose();
                            watchers.Remove(directory);
                        }
                    }
                }
                materialSet.Diffuse = diffuse.CurrentPath;
                materialSet.Normal = normal.CurrentPath;
                materialSet.Multi = multi.CurrentPath;
                materialSet.NormalMask = mask.CurrentPath;

                directory = Path.GetDirectoryName(materialSet.Diffuse);
                if (!string.IsNullOrWhiteSpace(directory)) {
                    if (!watchers.ContainsKey(directory)) {
                        FileSystemWatcher fileSystemWatcher = new FileSystemWatcher();
                        fileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite;
                        fileSystemWatcher.Changed += delegate {
                            StartGeneration();
                            return;
                        };
                        fileSystemWatcher.Path = directory;
                        fileSystemWatcher.EnableRaisingEvents = !string.IsNullOrEmpty(materialSet.Diffuse);
                        watchers.Add(directory, fileSystemWatcher);
                    } else {
                        watchers[directory].Path = directory;
                        watchers[directory].EnableRaisingEvents = !string.IsNullOrEmpty(materialSet.Diffuse);
                    }
                }
            }
        }
        public void StartGeneration() {
            if (!exportingTex) {
                if (generateButton.InvokeRequired) {
                    Action safeWrite = delegate { StartGeneration(); };
                    generateButton.Invoke(safeWrite);
                } else {
                    if (autoGenerateTImer == null) {
                        autoGenerateTImer = new System.Windows.Forms.Timer();
                        autoGenerateTImer.Interval = 10;
                        autoGenerateTImer.Tick += autoGenerateTImer_Tick;
                    }
                    if (!autoGenerateTImer.Enabled) {
                        autoGenerateTImer.Stop();
                        autoGenerateTImer.Start();
                    }
                }
            }
        }
        private void newToolStripMenuItem_Click(object sender, EventArgs e) {
            CleanSlate();
        }
        private bool CleanSlate() {
            if (!HasSaved) {
                DialogResult dialogResult = MessageBox.Show("Save changes?", VersionText, MessageBoxButtons.YesNoCancel);
                switch (dialogResult) {
                    case DialogResult.Yes:
                        if (savePath == null) {
                            SaveFileDialog saveFileDialog = new SaveFileDialog();
                            saveFileDialog.Filter = "FFXIV Texture Project|*.ffxivtp;";
                            saveFileDialog.AddExtension = true;
                            if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                                savePath = saveFileDialog.FileName;
                                Text = Application.ProductName + " " + Application.ProductVersion + $" ({savePath})";
                            }
                        }
                        if (!string.IsNullOrEmpty(savePath)) {
                            SaveProject(savePath);
                            NewProject();
                            return true;
                        }
                        break;
                    case DialogResult.No:
                        NewProject();
                        return true;
                }
            } else {
                NewProject();
                return true;
            }
            return false;
        }
        private void NewProject() {
            generatedOnce = false;
            Text = Application.ProductName + " " + Application.ProductVersion;
            savePath = null;
            materialList.Items.Clear();
            modNameTextBox.Text = "";
            modAuthorTextBox.Text = _defaultAuthor;
            modVersionTextBox.Text = "1.0.0";
            modDescriptionTextBox.Text = _defaultDescription;
            modWebsiteTextBox.Text = _defaultWebsite;
            diffuse.CurrentPath = "";
            normal.CurrentPath = "";
            multi.CurrentPath = "";
            diffuse.Enabled = false;
            normal.Enabled = false;
            multi.Enabled = false;
            mask.Enabled = false;
            HasSaved = true;
            foreach (FileSystemWatcher watcher in watchers.Values) {
                watcher.Dispose();
            }
            watchers.Clear();
            currentEditLabel.Text = "Please select a texture set to start importing";
        }
        private void multi_Leave(object sender, EventArgs e) {
            SetPaths();
        }

        private void multi_Enter(object sender, EventArgs e) {
            enteredField = true;
        }

        private void removeSelectionButton_Click(object sender, EventArgs e) {
            generatedOnce = false;
            materialList.Items.RemoveAt(materialList.SelectedIndex);
            diffuse.CurrentPath = "";
            normal.CurrentPath = "";
            multi.CurrentPath = "";
        }

        private void clearList_Click(object sender, EventArgs e) {
            generatedOnce = false;
            if (MessageBox.Show("This will irriversably remove everything from the list, including any changes. Are you sure?", VersionText, MessageBoxButtons.YesNo) == DialogResult.Yes) {
                {
                    materialList.Items.Clear();
                }
            }
            diffuse.CurrentPath = "";
            normal.CurrentPath = "";
            multi.CurrentPath = "";
            diffuse.Enabled = false;
            normal.Enabled = false;
            multi.Enabled = false;
            mask.Enabled = false;
        }

        private void multi_OnFileSelected(object sender, EventArgs e) {
            SetPaths();
            HasSaved = false;
        }

        private void modDescriptionTextBox_TextChanged(object sender, EventArgs e) {
            HasSaved = false;
        }
        public void GetAuthorWebsite() {
            string dataPath = Application.UserAppDataPath.Replace(Application.ProductVersion, null);
            string path = Path.Combine(dataPath, @"AuthorWebsite.config");
            if (File.Exists(path)) {
                using (StreamReader reader = new StreamReader(path)) {
                    _defaultWebsite = reader.ReadLine();
                }
            }
        }
        public void GetAuthorName() {
            string dataPath = Application.UserAppDataPath.Replace(Application.ProductVersion, null);
            string path = Path.Combine(dataPath, @"AuthorName.config");
            if (File.Exists(path)) {
                using (StreamReader reader = new StreamReader(path)) {
                    _defaultAuthor = reader.ReadLine();
                }
            }
        }

        public void WriteAuthorWebsite(string path) {
            string dataPath = Application.UserAppDataPath.Replace(Application.ProductVersion, null);
            using (StreamWriter writer = new StreamWriter(Path.Combine(dataPath, @"AuthorWebsite.config"))) {
                writer.WriteLine(path);
            }
        }
        public void WriteAuthorName(string path) {
            string dataPath = Application.UserAppDataPath.Replace(Application.ProductVersion, null);
            using (StreamWriter writer = new StreamWriter(Path.Combine(dataPath, @"AuthorName.config"))) {
                writer.WriteLine(path);
            }
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e) {
            Save();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e) {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "FFXIV Texture Project|*.ffxivtp;";
            if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                savePath = saveFileDialog.FileName;
                SaveProject(savePath);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e) {
            if (CleanSlate()) {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "FFXIV Texture Project|*.ffxivtp;";
                if (openFileDialog.ShowDialog() == DialogResult.OK) {
                    savePath = openFileDialog.FileName;
                    OpenProject(savePath);
                }
                HasSaved = true;
            }
        }
        private void Save() {
            if (savePath == null) {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "FFXIV Texture Project|*.ffxivtp;";
                if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                    savePath = saveFileDialog.FileName;
                }
            }
            if (savePath != null) {
                SaveProject(savePath);
            }
        }
        public void OpenProject(string path) {
            using (StreamReader file = File.OpenText(path)) {
                JsonSerializer serializer = new JsonSerializer();
                ProjectFile projectFile = (ProjectFile)serializer.Deserialize(file, typeof(ProjectFile));
                modNameTextBox.Text = projectFile.Name;
                modAuthorTextBox.Text = projectFile.Author;
                modVersionTextBox.Text = projectFile.Version;
                modDescriptionTextBox.Text = projectFile.Description;
                modWebsiteTextBox.Text = projectFile.Website;
                generationType.SelectedIndex = projectFile.ExportType;
                bakeNormals.Checked = projectFile.BakeMissingNormals;
                generateMultiCheckBox.Checked = projectFile.GenerateMulti;
                materialList.Items.AddRange(projectFile.MaterialSets?.ToArray());

                foreach (MaterialSet materialSet in projectFile.MaterialSets) {
                    if (!string.IsNullOrEmpty(materialSet.Diffuse)) {
                        if (!watchers.ContainsKey(materialSet.Diffuse)) {
                            FileSystemWatcher fileSystemWatcher = new FileSystemWatcher();
                            fileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite;
                            fileSystemWatcher.Changed += delegate {
                                StartGeneration();
                            };
                            fileSystemWatcher.Path = Path.GetDirectoryName(materialSet.Diffuse);
                            fileSystemWatcher.EnableRaisingEvents = !string.IsNullOrEmpty(materialSet.Diffuse);
                            watchers.Add(materialSet.Diffuse, fileSystemWatcher);
                        } else {
                            watchers[materialSet.Diffuse].Path = Path.GetDirectoryName(materialSet.Diffuse);
                            watchers[materialSet.Diffuse].EnableRaisingEvents = !string.IsNullOrEmpty(materialSet.Diffuse);
                        }
                    }
                }
            }
            HasSaved = true;
        }
        public void SaveProject(string path) {
            using (StreamWriter writer = new StreamWriter(path)) {
                JsonSerializer serializer = new JsonSerializer();
                ProjectFile projectFile = new ProjectFile();
                projectFile.Name = modNameTextBox.Text;
                projectFile.Author = modAuthorTextBox.Text;
                projectFile.Version = modVersionTextBox.Text;
                projectFile.Description = modDescriptionTextBox.Text;
                projectFile.Website = modWebsiteTextBox.Text;
                projectFile.MaterialSets = new List<MaterialSet>();
                projectFile.ExportType = generationType.SelectedIndex;
                projectFile.BakeMissingNormals = bakeNormals.Checked;
                projectFile.GenerateMulti = generateMultiCheckBox.Checked;
                foreach (MaterialSet materialSet in materialList.Items) {
                    projectFile.MaterialSets.Add(materialSet);
                }
                serializer.Serialize(writer, projectFile);
            }
            HasSaved = true;
        }
        private void CheckForCommandArguments() {
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1) {
                if (!string.IsNullOrWhiteSpace(args[1])) {
                    if (File.Exists(args[1]) && args[1].Contains(".ffxivtp")) {
                        savePath = args[1];
                        OpenProject(savePath);
                    }
                }
            }
        }

        private void modAuthorTextBox_Leave(object sender, EventArgs e) {
            WriteAuthorName(modAuthorTextBox.Text);
        }

        private void modWebsiteTextBox_Leave(object sender, EventArgs e) {
            WriteAuthorWebsite(modWebsiteTextBox.Text);
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e) {
            if (!HasSaved) {
                DialogResult dialogResult = MessageBox.Show("Save changes?", Text, MessageBoxButtons.YesNoCancel);
                switch (dialogResult) {
                    case DialogResult.Yes:
                        if (savePath == null) {
                            SaveFileDialog saveFileDialog = new SaveFileDialog();
                            saveFileDialog.Filter = "FFXIV Sound Project|*.ffxivtp;";
                            if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                                savePath = saveFileDialog.FileName;
                            }
                        }
                        if (savePath != null) {
                            SaveProject(savePath);
                        }
                        break;
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;
                }
            }
        }

        private void addCustomPathButton_Click(object sender, EventArgs e) {
            CustomPathDialog customPathDialog = new CustomPathDialog();
            if (customPathDialog.ShowDialog() == DialogResult.OK) {
                materialList.Items.Add(customPathDialog.MaterialSet);
            }
        }

        private void materialListContextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e) {
            if (materialList.Items.Count < 1 || materialList.SelectedIndex < 0) {
                e.Cancel = true;
                materialListContextMenu.Close();
            }
        }

        private void editPathsToolStripMenuItem_Click(object sender, EventArgs e) {
            CustomPathDialog customPathDialog = new CustomPathDialog();
            if (materialList.SelectedIndex != -1) {
                customPathDialog.MaterialSet = (materialList.Items[materialList.SelectedIndex] as MaterialSet);
                if (customPathDialog.ShowDialog() == DialogResult.OK) {
                    MessageBox.Show("Material Set has been edited successfully", VersionText);
                }
            }
            RefreshList();
        }
        private void RefreshList() {
            for (int i = 0; i < materialList.Items.Count; i++) {
                materialList.Items[i] = materialList.Items[i];
            }
        }

        private void moveUpButton_Click(object sender, EventArgs e) {
            if (materialList.SelectedIndex > 0) {
                object object1 = materialList.Items[materialList.SelectedIndex - 1];
                object object2 = materialList.Items[materialList.SelectedIndex];

                materialList.Items[materialList.SelectedIndex] = object1;
                materialList.Items[materialList.SelectedIndex - 1] = object2;
                materialList.SelectedIndex -= 1;
            }
        }

        private void moveDownButton_Click(object sender, EventArgs e) {
            if (materialList.SelectedIndex + 1 < materialList.Items.Count && materialList.SelectedIndex != -1) {
                object object1 = materialList.Items[materialList.SelectedIndex + 1];
                object object2 = materialList.Items[materialList.SelectedIndex];

                materialList.Items[materialList.SelectedIndex] = object1;
                materialList.Items[materialList.SelectedIndex + 1] = object2;
                materialList.SelectedIndex += 1;
            }
        }

        private void ffxivRefreshTimer_Tick(object sender, EventArgs e) {
            RefreshFFXIVInstance();
        }

        private void generationCooldown_Tick(object sender, EventArgs e) {
            generationCooldown.Stop();
            generateButton.Enabled = true;
            generateButton.Text = "Generate";
        }

        private void bulkTexViewerToolStripMenuItem_Click(object sender, EventArgs e) {
            new BulkTexManager().Show();
        }

        private void generationType_SelectedIndexChanged(object sender, EventArgs e) {
            generatedOnce = false;
        }

        private void bakeMissingNormalsCheckbox_CheckedChanged(object sender, EventArgs e) {
            generatedOnce = false;
            mask.Enabled = bakeNormals.Checked && materialList.SelectedIndex > -1;
        }

        private void generateMultiCheckBox_CheckedChanged(object sender, EventArgs e) {
            generatedOnce = false;
        }

        private void bulkReplaceToolStripMenuItem_Click(object sender, EventArgs e) {
            FindAndReplace findAndReplace = new FindAndReplace();
            Tokenizer tokenizer = new Tokenizer((materialList.Items[materialList.SelectedIndex] as MaterialSet).MaterialSetName);
            findAndReplace.ReplacementString.Text = tokenizer.GetToken();
            findAndReplace.Diffuse.CurrentPath = diffuse.CurrentPath;
            findAndReplace.Normal.CurrentPath = normal.CurrentPath;
            findAndReplace.Multi.CurrentPath = multi.CurrentPath;

            findAndReplace.MaterialSets.AddRange(materialList.Items.Cast<MaterialSet>().ToArray());
            if (findAndReplace.ShowDialog() == DialogResult.OK) {
                MessageBox.Show("Replacement succeeded.", VersionText);
            }
        }

        private void findAndBulkReplaceToolStripMenuItem_Click(object sender, EventArgs e) {
            FindAndReplace findAndReplace = new FindAndReplace();
            findAndReplace.MaterialSets.AddRange(materialList.Items.Cast<MaterialSet>().ToArray());
            if (findAndReplace.ShowDialog() == DialogResult.OK) {
                MessageBox.Show("Replacement succeeded.", VersionText);
            }
        }

        private void exportProgress_Click(object sender, EventArgs e) {

        }

        private void diffuseMergerToolStripMenuItem_Click(object sender, EventArgs e) {
            new DiffuseMerger().Show();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e) {

        }

        private void discordButton_Click(object sender, EventArgs e) {
            try {
                Process.Start(new System.Diagnostics.ProcessStartInfo() {
                    FileName = "https://discord.gg/rtGXwMn7pX",
                    UseShellExecute = true,
                    Verb = "OPEN"
                });
            } catch {

            }
        }

        private void facePart_SelectedIndexChanged(object sender, EventArgs e) {
            if (facePart.SelectedIndex == 4) {
                asymCheckbox.Enabled = faceType.Enabled = subRaceList.Enabled = false;
                facePaint.Enabled = true;
            } else {
                asymCheckbox.Enabled = faceType.Enabled = subRaceList.Enabled = true;
                facePaint.Enabled = false;
            }
        }

        private void howToGetTexturesToolStripMenuItem_Click(object sender, EventArgs e) {
            new HelpWindow().Show();
        }

        private void autoGenerateTImer_Tick(object sender, EventArgs e) {
            generateButton_Click(this, EventArgs.Empty);
            autoGenerateTImer.Stop();
        }

        private void normal_Load(object sender, EventArgs e) {

        }
    }
}