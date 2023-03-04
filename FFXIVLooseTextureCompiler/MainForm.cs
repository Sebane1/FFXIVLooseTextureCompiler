using Anamnesis.Penumbra;
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
using System.Windows.Interop;
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
        private bool hasDoneReload;
        private int fileCount;
        private Dictionary<string, Bitmap> normalCache;
        private Dictionary<string, Bitmap> multiCache;
        Dictionary<string, FileSystemWatcher> watchers = new Dictionary<string, FileSystemWatcher>();
        private bool lockDuplicateGeneration;

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
            mask.FilePath.Enabled = false;
            glow.FilePath.Enabled = false;
            raceCodeBody = new RaceCode();
            raceCodeFace = new RaceCode();
            for (int i = 0; i < 300; i++) {
                faceExtra.Items.Add((i + 1) + "");
            }
            uniqueAuRa.Enabled = false;
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
            baseBodyList.SelectedIndex = genderListBody.SelectedIndex = raceList.SelectedIndex = tailList.SelectedIndex =
                subRaceList.SelectedIndex = faceType.SelectedIndex = facePart.SelectedIndex = faceExtra.SelectedIndex = generationType.SelectedIndex = 0;
            CleanDirectory();
            CheckForCommandArguments();
            if (IntegrityChecker.IntegrityCheck()) {
                IntegrityChecker.ShowRules();
            }
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
            if (!lockDuplicateGeneration && !generationCooldown.Enabled) {
                lockDuplicateGeneration = true;
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
                        hasDoneReload = false;
                    }
                    Directory.CreateDirectory(modPath);
                    int i = 0;
                    fileCount = 0;
                    exportProgress.BringToFront();
                    exportProgress.Maximum = textureList.Items.Count * 3;
                    exportProgress.Visible = true;
                    Refresh();
                    Dictionary<string, List<TextureSet>> groups = new Dictionary<string, List<TextureSet>>();
                    normalCache = new Dictionary<string, Bitmap>();
                    multiCache = new Dictionary<string, Bitmap>();
                    foreach (TextureSet materialSet in textureList.Items) {
                        if (!groups.ContainsKey(materialSet.MaterialGroupName)) {
                            groups.Add(materialSet.MaterialGroupName, new List<TextureSet>() { materialSet });
                        } else {
                            groups[materialSet.MaterialGroupName].Add(materialSet);
                        }
                    }
                    foreach (List<TextureSet> materialSets in groups.Values) {
                        Group group = new Group(materialSets[0].MaterialGroupName.Replace(@"/", "-").Replace(@"\", "-"), "", 0, "Multi", 0);
                        Option option = null;
                        foreach (TextureSet materialSet in materialSets) {
                            string diffuseDiskPath = !string.IsNullOrEmpty(materialSet.InternalDiffusePath) ?
                                Path.Combine(modPath, materialSet.InternalDiffusePath.Replace("/", @"\")) : "";
                            string normalDiskPath = !string.IsNullOrEmpty(materialSet.InternalNormalPath) ?
                                Path.Combine(modPath, materialSet.InternalNormalPath.Replace("/", @"\")) : "";
                            string multiDiskPath = !string.IsNullOrEmpty(materialSet.InternalMultiPath) ?
                                Path.Combine(modPath, materialSet.InternalMultiPath.Replace("/", @"\")) : "";
                            switch (generationType.SelectedIndex) {
                                case 0:
                                    if (!string.IsNullOrEmpty(materialSet.Diffuse) && !string.IsNullOrEmpty(materialSet.InternalDiffusePath)) {
                                        option = new Option((materialSets.Count > 1 ? materialSet.MaterialSetName + " " : "")
                                            + (materialSet.MaterialSetName.ToLower().Contains("eyes") ? "Normal" : "Diffuse"), 0);
                                        option.Files.Add(materialSet.InternalDiffusePath, AppendNumber(materialSet.InternalDiffusePath.Replace("/", @"\"),
                                            fileCount));
                                        group.Options.Add(option);
                                        if ((materialSet.MaterialSetName.ToLower().Contains("eyes") && bakeNormals.Checked)) {
                                            ExportTex(materialSet.Diffuse, AppendNumber(diffuseDiskPath, fileCount++), ExportType.Normal,
                                                materialSet.Diffuse);
                                        } else {
                                            if (string.IsNullOrEmpty(materialSet.Glow)) {
                                                ExportTex(materialSet.Diffuse, AppendNumber(diffuseDiskPath, fileCount++));
                                            } else {
                                                ExportTex(materialSet.Diffuse, AppendNumber(diffuseDiskPath, fileCount++), ExportType.Glow, "",
                                                    materialSet.Glow);
                                            }
                                        }
                                        exportProgress.Increment(1);
                                        Refresh();
                                        Application.DoEvents();
                                    } else {
                                        exportProgress.Maximum--;
                                    }
                                    if (!string.IsNullOrEmpty(materialSet.Normal) && !string.IsNullOrEmpty(materialSet.InternalNormalPath)) {
                                        option = new Option((materialSets.Count > 1 ? materialSet.MaterialSetName + " " : "")
                                            + (materialSet.MaterialSetName.ToLower().Contains("eyes") ? "Multi" : "Normal"), 0);
                                        option.Files.Add(materialSet.InternalNormalPath, AppendNumber(materialSet.InternalNormalPath.Replace("/", @"\"),
                                            fileCount));
                                        group.Options.Add(option);
                                        if (bakeNormals.Checked && !materialSet.MaterialSetName.ToLower().Contains("eyes")) {
                                            ExportTex(materialSet.Normal, AppendNumber(normalDiskPath, fileCount++), ExportType.MergeNormal, materialSet.Diffuse, materialSet.NormalMask);
                                        } else {
                                            if (!string.IsNullOrEmpty(materialSet.Glow) && (materialSet.MaterialSetName.ToLower().Contains("eyes"))) {
                                                ExportTex(materialSet.Normal, AppendNumber(normalDiskPath, fileCount++), ExportType.GlowMulti, "",
                                                    materialSet.Glow);
                                            } else {
                                                ExportTex(materialSet.Normal, AppendNumber(normalDiskPath, fileCount++));
                                            }
                                        }
                                        exportProgress.Increment(1);
                                        Refresh();
                                    } else if (!string.IsNullOrEmpty(materialSet.Diffuse) && !string.IsNullOrEmpty(materialSet.InternalNormalPath)
                                        && bakeNormals.Checked && !(materialSet.MaterialSetName.ToLower().Contains("eyes"))) {
                                        if (!materialSet.IgnoreNormalGeneration) {
                                            option = new Option((materialSets.Count > 1 ? materialSet.MaterialSetName + " " : "") +
                                                (materialSet.MaterialSetName.ToLower().Contains("eyes") ? "Multi" : "Normal"), 0);
                                            option.Files.Add(materialSet.InternalNormalPath, AppendNumber(materialSet.InternalNormalPath.Replace("/", @"\"),
                                                fileCount));
                                            group.Options.Add(option);
                                            ExportTex(materialSet.Diffuse, AppendNumber(normalDiskPath, fileCount++), ExportType.Normal);
                                            exportProgress.Increment(1);
                                            Refresh();
                                            Application.DoEvents();
                                        } else {
                                            exportProgress.Maximum--;
                                        }
                                    } else {
                                        exportProgress.Maximum--;
                                    }
                                    if (!string.IsNullOrEmpty(materialSet.Multi) && !string.IsNullOrEmpty(materialSet.InternalMultiPath)) {
                                        option = new Option((materialSets.Count > 1 ? materialSet.MaterialSetName + " " : "") +
                                            (materialSet.MaterialSetName.ToLower().Contains("eyes") ? "Catchlight" : "Multi"), 0);
                                        option.Files.Add(materialSet.InternalMultiPath, AppendNumber(materialSet.InternalMultiPath.Replace("/", @"\"),
                                            fileCount));
                                        group.Options.Add(option);
                                        ExportTex(materialSet.Multi, AppendNumber(multiDiskPath, fileCount++));
                                        exportProgress.Increment(1);
                                        Refresh();
                                        Application.DoEvents();
                                    } else if (!string.IsNullOrEmpty(materialSet.Diffuse) && !string.IsNullOrEmpty(materialSet.InternalMultiPath)
                                        && generateMultiCheckBox.Checked && !(materialSet.MaterialSetName.ToLower().Contains("eyes"))) {
                                        if (!materialSet.IgnoreMultiGeneration) {
                                            option = new Option((materialSets.Count > 1 ? materialSet.MaterialSetName + " " : "") +
                                                (materialSet.MaterialSetName.ToLower().Contains("eyes") ? "Catchlight" : "Multi"), 0);
                                            option.Files.Add(materialSet.InternalMultiPath, AppendNumber(materialSet.InternalMultiPath.Replace("/", @"\"),
                                                fileCount));
                                            group.Options.Add(option);
                                            ExportTex(materialSet.Diffuse, AppendNumber(multiDiskPath, fileCount), ExportType.MultiFace);
                                            exportProgress.Increment(1);
                                            Refresh();
                                            Application.DoEvents();
                                        } else {
                                            exportProgress.Maximum--;
                                        }
                                    } else {
                                        exportProgress.Maximum--;
                                    }
                                    break;
                                case 1:
                                    option = new Option(materialSet.MaterialSetName == materialSet.MaterialGroupName ? "Enable"
                                        : materialSet.MaterialSetName, 0);
                                    group.Options.Add(option);
                                    if (!string.IsNullOrEmpty(materialSet.Diffuse) && !string.IsNullOrEmpty(materialSet.InternalDiffusePath)) {
                                        if ((materialSet.MaterialSetName.ToLower().Contains("eyes") && bakeNormals.Checked)) {
                                            ExportTex(materialSet.Diffuse, AppendNumber(diffuseDiskPath, fileCount), ExportType.Normal, materialSet.Diffuse);
                                        } else {
                                            if (string.IsNullOrEmpty(materialSet.Glow)) {
                                                ExportTex(materialSet.Diffuse, AppendNumber(diffuseDiskPath, fileCount));
                                            } else {
                                                ExportTex(materialSet.Diffuse, AppendNumber(diffuseDiskPath, fileCount), ExportType.Glow, "", materialSet.Glow);
                                            }
                                        }
                                        option.Files.Add(materialSet.InternalDiffusePath, AppendNumber(materialSet.InternalDiffusePath.Replace("/", @"\"),
                                            fileCount++));
                                        exportProgress.Increment(1);
                                        Refresh();
                                        Application.DoEvents();
                                    } else {
                                        exportProgress.Maximum--;
                                    }
                                    if (!string.IsNullOrEmpty(materialSet.Normal) && !string.IsNullOrEmpty(materialSet.InternalNormalPath)) {
                                        if (!bakeNormals.Checked || materialSet.MaterialSetName.ToLower().Contains("eyes")) {
                                            if (!string.IsNullOrEmpty(materialSet.Glow) && (materialSet.MaterialSetName.ToLower().Contains("eyes"))) {
                                                ExportTex(materialSet.Normal, AppendNumber(normalDiskPath, fileCount), ExportType.GlowMulti, "",
                                                    materialSet.Glow);
                                            } else {
                                                ExportTex(materialSet.Normal, AppendNumber(normalDiskPath, fileCount));
                                            }
                                        } else {
                                            ExportTex(materialSet.Normal, AppendNumber(normalDiskPath, fileCount), ExportType.MergeNormal,
                                                materialSet.Diffuse, materialSet.NormalMask);
                                        }
                                        option.Files.Add(materialSet.InternalNormalPath, AppendNumber(materialSet.InternalNormalPath.Replace("/", @"\"),
                                            fileCount++));
                                        exportProgress.Increment(1);
                                        Refresh();
                                        Application.DoEvents();
                                    } else if (!string.IsNullOrEmpty(materialSet.Diffuse) && !string.IsNullOrEmpty(materialSet.InternalNormalPath)
                                        && bakeNormals.Checked) {
                                        if (!materialSet.IgnoreNormalGeneration) {
                                            ExportTex(materialSet.Diffuse, AppendNumber(normalDiskPath, fileCount), ExportType.Normal);
                                            option.Files.Add(materialSet.InternalNormalPath, AppendNumber(materialSet.InternalNormalPath.Replace("/", @"\"),
                                                fileCount++));
                                            exportProgress.Increment(1);
                                            Refresh();
                                            Application.DoEvents();
                                        } else {
                                            exportProgress.Maximum--;
                                        }
                                    } else {
                                        exportProgress.Maximum--;
                                    }
                                    if (!string.IsNullOrEmpty(materialSet.Multi) && !string.IsNullOrEmpty(materialSet.InternalMultiPath)) {
                                        ExportTex(materialSet.Multi, AppendNumber(multiDiskPath, fileCount));
                                        option.Files.Add(materialSet.InternalMultiPath, AppendNumber(materialSet.InternalMultiPath.Replace("/", @"\"),
                                            fileCount++));
                                        exportProgress.Increment(1);
                                        Refresh();
                                        Application.DoEvents();
                                    } else if (!string.IsNullOrEmpty(materialSet.Diffuse) && !string.IsNullOrEmpty(materialSet.InternalMultiPath)
                                        && generateMultiCheckBox.Checked && !(materialSet.MaterialSetName.ToLower().Contains("eyes"))) {
                                        if (!materialSet.IgnoreMultiGeneration) {
                                            ExportTex(materialSet.Diffuse, AppendNumber(multiDiskPath, fileCount), ExportType.MultiFace);
                                            option.Files.Add(materialSet.InternalMultiPath, AppendNumber(materialSet.InternalMultiPath.Replace("/", @"\"),
                                                fileCount++));
                                            exportProgress.Increment(1);
                                            Refresh();
                                            Application.DoEvents();
                                        } else {
                                            exportProgress.Maximum--;
                                        }
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
                    if (hasDoneReload) {
                        PenumbraHttpApi.Redraw(0, Hook);
                    } else {
                        modNameTextBox.Enabled = modAuthorTextBox.Enabled
                        = modWebsiteTextBox.Enabled = modVersionTextBox.Enabled
                        = modVersionTextBox.Enabled = modDescriptionTextBox.Enabled = false;
                        diffuse.FilePath.Enabled = false;
                        normal.FilePath.Enabled = false;
                        multi.FilePath.Enabled = false;
                        mask.FilePath.Enabled = false;
                        glow.FilePath.Enabled = false;
                        PenumbraHttpApi.Reload(modPath, modNameTextBox.Text, Hook);
                        PenumbraHttpApi.Redraw(0, Hook);
                        if (IntegrityChecker.IntegrityCheck()) {
                            IntegrityChecker.ShowConsolation();
                        }
                        hasDoneReload = true;
                        materialList_SelectedIndexChanged(this, EventArgs.Empty);
                    }
                    generateButton.Enabled = false;
                    generationCooldown.Start();
                    exportProgress.Visible = false;
                    exportProgress.Value = 0;
                    lockDuplicateGeneration = false;
                    modNameTextBox.Enabled = modAuthorTextBox.Enabled
                    = modWebsiteTextBox.Enabled = modVersionTextBox.Enabled
                    = modVersionTextBox.Enabled = modDescriptionTextBox.Enabled = true;
                    exportPanel.Visible = false;
                } else {
                    exportPanel.Visible = false;
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
            MultiFace,
            MergeNormal,
            Glow,
            GlowMulti
        }
        public void ExportTex(string inputFile, string outputFile, ExportType exportType = ExportType.None, string diffuseNormal = "", string mask = "") {
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
                    case ExportType.Glow:
                        using (Bitmap bitmap = TexLoader.ResolveBitmap(inputFile)) {
                            if (bitmap != null) {
                                Bitmap glowBitmap = AtramentumLuminisGlow.CalculateDiffuse(bitmap, TexLoader.ResolveBitmap(mask));
                                glowBitmap.Save(stream, ImageFormat.Png);
                                stream.Flush();
                                stream.Position = 0;
                                TextureImporter.PngToTex(stream, out data);
                            }
                        }
                        break;
                    case ExportType.GlowMulti:
                        using (Bitmap bitmap = TexLoader.ResolveBitmap(inputFile)) {
                            if (bitmap != null) {
                                Bitmap glowBitmap = AtramentumLuminisGlow.CalculateMulti(bitmap, TexLoader.ResolveBitmap(mask));
                                glowBitmap.Save(stream, ImageFormat.Png);
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
                                        if (File.Exists(mask)) {
                                            using (Bitmap normalMaskBitmap = TexLoader.ResolveBitmap(mask)) {
                                                output = Normal.Calculate(target, normalMaskBitmap);
                                            }
                                        } else {
                                            output = Normal.Calculate(target);
                                        }
                                        if (outputFile.Contains("fac_b_n")) {
                                            Bitmap resize = new Bitmap(output, new Size(1024, 1024));
                                            resize.Save(stream, ImageFormat.Png);
                                            normalCache.Add(inputFile, resize);
                                        } else {
                                            output.Save(stream, ImageFormat.Png);
                                            normalCache.Add(inputFile, output);
                                        }
                                    }
                                }
                            }
                        } else {
                            normalCache[inputFile].Save(stream, ImageFormat.Png);
                        }
                        stream.Position = 0;
                        TextureImporter.PngToTex(stream, out data);
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
                                        if (File.Exists(mask)) {
                                            using (Bitmap normalMaskBitmap = TexLoader.ResolveBitmap(mask)) {
                                                if (outputFile.Contains("fac_b_n")) {
                                                    Bitmap resize = new Bitmap(bitmap, new Size(1024, 1024));
                                                    output = ImageManipulation.MergeNormals(inputFile, resize, target, normalMaskBitmap, diffuseNormal);
                                                    output.Save(stream, ImageFormat.Png);
                                                    normalCache.Add(inputFile, resize);
                                                } else {
                                                    output = ImageManipulation.MergeNormals(inputFile, bitmap, target, normalMaskBitmap, diffuseNormal);
                                                    normalCache.Add(inputFile, output);
                                                }
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
                if (file.Contains("WebView2") || file.Contains(".zip") || file.Contains(".pdb") || file.Contains(".config")
                    || file.Contains(".xml") || file.Contains(".log") || file.Contains(".tmp") || file.Contains("ZipExtractor")) {
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
        private string GetBodyTexturePath(int texture) {
            string result = "";
            string unique = (((string)raceList.Items[raceList.SelectedIndex]).Contains("Xaela") ? "0101" : "0001");
            switch (baseBodyList.SelectedIndex) {
                case 0:
                    // Vanila
                    if (texture == 2 && raceList.SelectedIndex == 5) {
                        result = @"chara/common/texture/skin_m.tex";
                    } else {
                        string genderCode = (genderListBody.SelectedIndex == 0 ? raceCodeBody.Masculine[raceList.SelectedIndex]
                            : raceCodeBody.Feminine[raceList.SelectedIndex]);
                        result = @"chara/human/c" + genderCode + @"/obj/body/b" + unique
                            + @"/texture/--c" + genderCode + "b" + unique + GetTextureType(texture) + ".tex";
                    }
                    break;
                case 1:
                    // Bibo+
                    if (raceList.SelectedIndex != 5) {
                        if (genderListBody.SelectedIndex == 1) {
                            result = @"chara/bibo/" + bodyIdentifiers[baseBodyList.SelectedIndex].RaceIdentifiers[raceList.SelectedIndex]
                                + GetTextureType(texture) + ".tex";
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
                            if (texture != 2) {
                                result = @"chara/human/c" + (genderListBody.SelectedIndex == 0 ? raceCodeBody.Masculine[raceList.SelectedIndex]
                                    : raceCodeBody.Feminine[raceList.SelectedIndex]) + @"/obj/body/b" + "0001" + @"/texture/eve2" +
                                    bodyIdentifiers[baseBodyList.SelectedIndex].RaceIdentifiers[raceList.SelectedIndex] + GetTextureType(texture) + ".tex";
                            } else {
                                if (raceList.SelectedIndex == 6) {
                                    result = "chara/human/c1401/obj/body/b0001/texture/eve2lizard_m.tex";
                                } else if (raceList.SelectedIndex == 7) {
                                    result = "chara/human/c1401/obj/body/b0001/texture/eve2lizard2_m.tex";
                                } else {
                                    result = "chara/common/texture/skin_gen3.tex";
                                }
                            }
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
                            result = @"chara/human/c" + (genderListBody.SelectedIndex == 0 ? raceCodeBody.Masculine[raceList.SelectedIndex]
                                : raceCodeBody.Feminine[raceList.SelectedIndex]) + @"/obj/body/b" + unique + @"/texture/tfgen3" +
                                bodyIdentifiers[baseBodyList.SelectedIndex].RaceIdentifiers[raceList.SelectedIndex] + "f" + GetTextureType(texture) + ".tex";
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
                                result = @"chara/bibo/" + bodyIdentifiers[baseBodyList.SelectedIndex].RaceIdentifiers[raceList.SelectedIndex] +
                                    GetTextureType(texture) + ".tex";
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
                            result = @"chara/human/c" + (genderListBody.SelectedIndex == 0 ? raceCodeBody.Masculine[raceList.SelectedIndex]
                                : raceCodeBody.Feminine[raceList.SelectedIndex]) + @"/obj/body/b" + (uniqueAuRa.Checked ? "0101" : "0001")
                                + @"/texture/--c" + raceCodeBody.Masculine[raceList.SelectedIndex] + "b0001_b" + GetTextureType(texture) + ".tex";
                        } else {
                            result = "";
                            MessageBox.Show("TBSE and HRBODY are only compatible with masculine characters", VersionText);
                        }
                    } else {
                        MessageBox.Show("TBSE and HRBODY are not compatible with lalafells", VersionText);
                    }
                    break;
                case 6:
                    string xaelaCheck = (raceList.SelectedIndex == 7 ? "010" : "000") + (tailList.SelectedIndex + 1);
                    string gender = (genderListBody.SelectedIndex == 0 ? raceCodeBody.Masculine[raceList.SelectedIndex]
                        : raceCodeBody.Feminine[raceList.SelectedIndex]);
                    result = @"chara/human/c" + gender + @"/obj/tail/t" + xaelaCheck + @"/texture/--c" + gender + "t" +
                        xaelaCheck + "_etc" + GetTextureType(texture) + ".tex";
                    break;
            }
            return result;
        }
        public string GetFaceTexturePath(int material) {
            if (material != 3) {
                string faceIdCheck = "000";
                string selectedText = (string)subRaceList.Items[subRaceList.SelectedIndex];
                if (selectedText.ToLower() == "the lost" || selectedText.ToLower() == "hellsgaurd" || selectedText.ToLower() == "highlander"
                    || selectedText.ToLower() == "duskwight" || selectedText.ToLower() == "keeper" || selectedText.ToLower() == "dunesfolk"
                    || selectedText.ToLower() == "xaela" || (selectedText.ToLower() == "veena" && facePart.SelectedIndex == 1)
                    || (selectedText.ToLower() == "veena" && facePart.SelectedIndex == 2 && material == 2)) {
                    faceIdCheck = "010";
                }
                string subRace = (genderListBody.SelectedIndex == 0 ? raceCodeFace.Masculine[subRaceList.SelectedIndex]
                    : raceCodeFace.Feminine[subRaceList.SelectedIndex]);
                return "chara/human/c" + subRace + "/obj/face/f" + faceIdCheck + (faceType.SelectedIndex + 1) + "/texture/--c"
                    + subRace + "f" + faceIdCheck + (faceType.SelectedIndex + 1)
                    + GetFacePart(facePart.SelectedIndex) + GetTextureType(material, true) + ".tex";
            } else {
                return "chara/common/texture/catchlight_1.tex";
            }
        }
        public string GetHairTexturePath(int material) {
            string hairValue = NumberPadder(faceExtra.SelectedIndex + 1);
            string genderCode = (genderListBody.SelectedIndex == 0 ? raceCodeBody.Masculine[raceList.SelectedIndex]
                : raceCodeBody.Feminine[raceList.SelectedIndex]);
            string subRace = (genderListBody.SelectedIndex == 0 ? raceCodeFace.Masculine[subRaceList.SelectedIndex]
                : raceCodeFace.Feminine[subRaceList.SelectedIndex]);
            return "chara/human/c" + genderCode + "/obj/hair/h" + hairValue + "/texture/--c"
                + genderCode + "h" + hairValue + "_hir" + GetTextureType(material, true) + ".tex";
        }

        public string NumberPadder(int value) {
            int numbersToPad = 4 - value.ToString().Length;
            string result = "";
            for (int i = 0; i < numbersToPad; i++) {
                result += "0";
            }
            result += value;
            return result;
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
                    uniqueAuRa.Enabled = false;
                    break;
                case 1:
                    // Bibo+
                    genderListBody.SelectedIndex = 1;
                    genderListBody.Enabled = false;
                    tailList.Enabled = false;
                    if (raceList.SelectedIndex == 5) {
                        raceList.SelectedIndex = 0;
                    }
                    uniqueAuRa.Enabled = false;
                    break;
                case 2:
                    // Eve
                    genderListBody.SelectedIndex = 1;
                    genderListBody.Enabled = false;
                    tailList.Enabled = false;
                    if (raceList.SelectedIndex == 5) {
                        raceList.SelectedIndex = 0;
                    }
                    uniqueAuRa.Enabled = false;
                    break;
                case 3:
                    // Gen3 and T&F3
                    genderListBody.SelectedIndex = 1;
                    genderListBody.Enabled = false;
                    tailList.Enabled = false;
                    if (raceList.SelectedIndex == 5) {
                        raceList.SelectedIndex = 0;
                    }
                    uniqueAuRa.Enabled = false;
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
                    uniqueAuRa.Enabled = false;
                    break;
                case 5:
                    // TBSE and HRBODY
                    genderListBody.SelectedIndex = 0;
                    genderListBody.Enabled = false;
                    tailList.Enabled = false;
                    if (raceList.SelectedIndex == 5) {
                        raceList.SelectedIndex = 0;
                    }
                    uniqueAuRa.Enabled = true;
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
                    uniqueAuRa.Enabled = false;
                    break;
            }
        }

        private void raceList_SelectedIndexChanged(object sender, EventArgs e) {
            if (baseBodyList.SelectedIndex == 6) {
                if (raceList.SelectedIndex != 3 && raceList.SelectedIndex != 6 && raceList.SelectedIndex != 7) {
                    raceList.SelectedIndex = 3;
                    MessageBox.Show("Tail is only compatible with Miqo'te Xaela, and Raen", VersionText);
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
            hasDoneReload = false;
            TextureSet materialSet = new TextureSet();
            materialSet.MaterialSetName = baseBodyList.Text + (baseBodyList.Text.ToLower().Contains("tail") ? " " + (tailList.SelectedIndex + 1) : "") + ", " + genderListBody.Text + ", " + raceList.Text;
            if (raceList.SelectedIndex != 3 || baseBodyList.SelectedIndex != 6) {
                materialSet.InternalDiffusePath = GetBodyTexturePath(0);
            }
            materialSet.InternalNormalPath = GetBodyTexturePath(1);
            materialSet.InternalMultiPath = GetBodyTexturePath(2);
            textureList.Items.Add(materialSet);
            HasSaved = false;
        }

        private void addFaceButton_Click(object sender, EventArgs e) {
            hasDoneReload = false;
            TextureSet materialSet = new TextureSet();
            materialSet.MaterialSetName = facePart.Text + (facePart.SelectedIndex == 4 ? " " + (faceExtra.SelectedIndex + 1) : "") + ", " + (facePart.SelectedIndex != 4 ? genderListBody.Text : "Unisex") + ", " + (facePart.SelectedIndex != 4 ? subRaceList.Text : "Multi Race") + ", " + (facePart.SelectedIndex != 4 ? faceType.Text : "Multi Face");
            switch (facePart.SelectedIndex) {
                default:
                    materialSet.InternalDiffusePath = GetFaceTexturePath(0);
                    materialSet.InternalNormalPath = GetFaceTexturePath(1);
                    materialSet.InternalMultiPath = GetFaceTexturePath(2);
                    break;
                case 2:
                    materialSet.InternalDiffusePath = GetFaceTexturePath(1);
                    materialSet.InternalNormalPath = GetFaceTexturePath(2);
                    materialSet.InternalMultiPath = GetFaceTexturePath(3);
                    break;
                case 4:
                    materialSet.InternalDiffusePath = "chara/common/texture/decal_face/_decal_" + (faceExtra.SelectedIndex + 1) + ".tex";
                    break;
                case 5:
                    materialSet.MaterialSetName = facePart.Text + " " + (faceExtra.SelectedIndex + 1) + ", " + genderListBody.Text + ", " + subRaceList.Text;
                    materialSet.InternalNormalPath = GetHairTexturePath(1);
                    materialSet.InternalMultiPath = GetHairTexturePath(2);
                    break;

            }
            materialSet.IgnoreMultiGeneration = true;
            textureList.Items.Add(materialSet);
            HasSaved = false;
        }

        private void materialList_SelectedIndexChanged(object sender, EventArgs e) {
            if (textureList.SelectedIndex == -1) {
                currentEditLabel.Text = "Please select a texture set to start importing";
                diffuse.Enabled = false;
                normal.Enabled = false;
                multi.Enabled = false;
                mask.Enabled = false;
                glow.Enabled = false;
            } else {
                TextureSet materialSet = (textureList.Items[textureList.SelectedIndex] as TextureSet);
                currentEditLabel.Text = "Editing: " + materialSet.MaterialSetName;
                diffuse.CurrentPath = materialSet.Diffuse;
                normal.CurrentPath = materialSet.Normal;
                multi.CurrentPath = materialSet.Multi;
                mask.CurrentPath = materialSet.NormalMask;
                glow.CurrentPath = materialSet.Glow;

                diffuse.Enabled = !string.IsNullOrEmpty(materialSet.InternalDiffusePath);
                normal.Enabled = !string.IsNullOrEmpty(materialSet.InternalNormalPath);
                multi.Enabled = !string.IsNullOrEmpty(materialSet.InternalMultiPath);
                mask.Enabled = bakeNormals.Checked;
                glow.Enabled = !materialSet.MaterialSetName.ToLower().Contains("face paint") && !materialSet.MaterialSetName.ToLower().Contains("hair") && diffuse.Enabled;

                if (materialSet.MaterialSetName.ToLower().Contains("eyes")) {
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
            if (textureList.SelectedIndex != -1) {
                TextureSet materialSet = (textureList.Items[textureList.SelectedIndex] as TextureSet);
                string directoryDiffuse = Path.GetDirectoryName(materialSet.Diffuse);
                if (!string.IsNullOrWhiteSpace(directoryDiffuse)) {
                    if (watchers.ContainsKey(directoryDiffuse)) {
                        if (materialSet.Diffuse != diffuse.CurrentPath) {
                            watchers[directoryDiffuse].Dispose();
                            watchers.Remove(directoryDiffuse);
                        }
                    }
                }
                string directoryNormal = Path.GetDirectoryName(materialSet.Normal);
                if (!string.IsNullOrWhiteSpace(directoryNormal)) {
                    if (watchers.ContainsKey(directoryNormal)) {
                        if (materialSet.Normal != normal.CurrentPath) {
                            watchers[directoryNormal].Dispose();
                            watchers.Remove(directoryNormal);
                        }
                    }
                }
                string directoryMulti = Path.GetDirectoryName(materialSet.Multi);
                if (!string.IsNullOrWhiteSpace(directoryMulti)) {
                    if (watchers.ContainsKey(directoryMulti)) {
                        if (materialSet.Multi != multi.CurrentPath) {
                            watchers[directoryMulti].Dispose();
                            watchers.Remove(directoryMulti);
                        }
                    }
                }
                string directoryMask = Path.GetDirectoryName(materialSet.NormalMask);
                if (!string.IsNullOrWhiteSpace(directoryMask)) {
                    if (watchers.ContainsKey(directoryMask)) {
                        if (materialSet.NormalMask != mask.CurrentPath) {
                            watchers[directoryMask].Dispose();
                            watchers.Remove(directoryMask);
                        }
                    }
                }
                string directoryGlow = Path.GetDirectoryName(materialSet.Glow);
                if (!string.IsNullOrWhiteSpace(directoryGlow)) {
                    if (watchers.ContainsKey(directoryGlow)) {
                        if (materialSet.Glow != glow.CurrentPath) {
                            watchers[directoryGlow].Dispose();
                            watchers.Remove(directoryGlow);
                        }
                    }
                }
                materialSet.Diffuse = diffuse.CurrentPath;
                materialSet.Normal = normal.CurrentPath;
                materialSet.Multi = multi.CurrentPath;
                materialSet.NormalMask = mask.CurrentPath;
                materialSet.Glow = glow.CurrentPath;

                AddWatcher(materialSet.Diffuse);
                AddWatcher(materialSet.Normal);
                AddWatcher(materialSet.Multi);
                AddWatcher(materialSet.NormalMask);
                AddWatcher(materialSet.Glow);
            }
        }

        public void AddWatcher(string path) {
            string directory = Path.GetDirectoryName(path);
            if (Directory.Exists(directory)) {
                if (!string.IsNullOrWhiteSpace(directory)) {
                    if (!watchers.ContainsKey(directory)) {
                        FileSystemWatcher fileSystemWatcher = new FileSystemWatcher();
                        fileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite;
                        fileSystemWatcher.Changed += delegate {
                            StartGeneration();
                            return;
                        };
                        fileSystemWatcher.Path = directory;
                        fileSystemWatcher.EnableRaisingEvents = !string.IsNullOrEmpty(path);
                        watchers.Add(directory, fileSystemWatcher);
                    } else {
                        watchers[directory].Path = directory;
                        watchers[directory].EnableRaisingEvents = !string.IsNullOrEmpty(path);
                    }
                }
            }
        }

        public void StartGeneration() {
            if (!lockDuplicateGeneration) {
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
            lockDuplicateGeneration = true;
            hasDoneReload = false;
            Text = Application.ProductName + " " + Application.ProductVersion;
            savePath = null;
            textureList.Items.Clear();
            modNameTextBox.Text = "";
            modAuthorTextBox.Text = _defaultAuthor;
            modVersionTextBox.Text = "1.0.0";
            modDescriptionTextBox.Text = _defaultDescription;
            modWebsiteTextBox.Text = _defaultWebsite;
            diffuse.CurrentPath = "";
            normal.CurrentPath = "";
            multi.CurrentPath = "";
            glow.CurrentPath = "";

            diffuse.Enabled = false;
            normal.Enabled = false;
            multi.Enabled = false;
            mask.Enabled = false;
            glow.Enabled = false;
            HasSaved = true;
            foreach (FileSystemWatcher watcher in watchers.Values) {
                watcher.Dispose();
            }
            watchers.Clear();
            currentEditLabel.Text = "Please select a texture set to start importing";
            lockDuplicateGeneration = false;
        }
        private void multi_Leave(object sender, EventArgs e) {
            SetPaths();
        }

        private void multi_Enter(object sender, EventArgs e) {
            enteredField = true;
        }

        private void removeSelectionButton_Click(object sender, EventArgs e) {
            hasDoneReload = false;
            if (textureList.SelectedIndex > -1) {
                textureList.Items.RemoveAt(textureList.SelectedIndex);
                diffuse.CurrentPath = "";
                normal.CurrentPath = "";
                multi.CurrentPath = "";
                glow.CurrentPath = "";
            }
        }

        private void clearList_Click(object sender, EventArgs e) {
            hasDoneReload = false;
            if (MessageBox.Show("This will irriversably remove everything from the list, including any changes. Are you sure?", VersionText, MessageBoxButtons.YesNo) == DialogResult.Yes) {
                {
                    textureList.Items.Clear();
                }
            }
            diffuse.CurrentPath = "";
            normal.CurrentPath = "";
            multi.CurrentPath = "";
            mask.CurrentPath = "";
            glow.CurrentPath = "";

            diffuse.Enabled = false;
            normal.Enabled = false;
            multi.Enabled = false;
            mask.Enabled = false;
            glow.Enabled = false;
        }

        private void multi_OnFileSelected(object sender, EventArgs e) {
            SetPaths();
            HasSaved = false;
            hasDoneReload = false;
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
            if (IntegrityChecker.IntegrityCheck()) {
                IntegrityChecker.ShowSave();
            }
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
            lockDuplicateGeneration = true;
            if (CleanSlate()) {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "FFXIV Texture Project|*.ffxivtp;";
                if (openFileDialog.ShowDialog() == DialogResult.OK) {
                    savePath = openFileDialog.FileName;
                    OpenProject(savePath);
                    generationCooldown.Start();
                    if (IntegrityChecker.IntegrityCheck()) {
                        IntegrityChecker.ShowOpen();
                    }
                }
                HasSaved = true;
            }
            lockDuplicateGeneration = false;
        }
        private void Save() {
            lockDuplicateGeneration = true;
            if (savePath == null) {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "FFXIV Texture Project|*.ffxivtp;";
                if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                    savePath = saveFileDialog.FileName;
                }
            }
            if (savePath != null) {
                SaveProject(savePath);
                generationCooldown.Start();
            }
            lockDuplicateGeneration = false;
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
                textureList.Items.AddRange(projectFile.MaterialSets?.ToArray());

                foreach (TextureSet materialSet in projectFile.MaterialSets) {
                    AddWatcher(materialSet.Diffuse);
                    AddWatcher(materialSet.Normal);
                    AddWatcher(materialSet.Multi);
                    AddWatcher(materialSet.NormalMask);
                    AddWatcher(materialSet.Glow);
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
                projectFile.MaterialSets = new List<TextureSet>();
                projectFile.ExportType = generationType.SelectedIndex;
                projectFile.BakeMissingNormals = bakeNormals.Checked;
                projectFile.GenerateMulti = generateMultiCheckBox.Checked;
                foreach (TextureSet materialSet in textureList.Items) {
                    projectFile.MaterialSets.Add(materialSet);
                }
                serializer.Serialize(writer, projectFile);
            }
            HasSaved = true;
            MessageBox.Show("Save successfull", VersionText);
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
                textureList.Items.Add(customPathDialog.MaterialSet);
            }
        }

        private void materialListContextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e) {
            if (textureList.Items.Count < 1 || textureList.SelectedIndex < 0) {
                e.Cancel = true;
                materialListContextMenu.Close();
            }
        }

        private void editPathsToolStripMenuItem_Click(object sender, EventArgs e) {
            CustomPathDialog customPathDialog = new CustomPathDialog();
            if (textureList.SelectedIndex != -1) {
                customPathDialog.MaterialSet = (textureList.Items[textureList.SelectedIndex] as TextureSet);
                if (customPathDialog.ShowDialog() == DialogResult.OK) {
                    MessageBox.Show("Texture Set has been edited successfully", VersionText);
                    hasDoneReload = false;
                }
            }
            RefreshList();
        }
        private void RefreshList() {
            for (int i = 0; i < textureList.Items.Count; i++) {
                textureList.Items[i] = textureList.Items[i];
            }
        }

        private void moveUpButton_Click(object sender, EventArgs e) {
            if (textureList.SelectedIndex > 0) {
                object object1 = textureList.Items[textureList.SelectedIndex - 1];
                object object2 = textureList.Items[textureList.SelectedIndex];

                textureList.Items[textureList.SelectedIndex] = object1;
                textureList.Items[textureList.SelectedIndex - 1] = object2;
                textureList.SelectedIndex -= 1;
            }
        }

        private void moveDownButton_Click(object sender, EventArgs e) {
            if (textureList.SelectedIndex + 1 < textureList.Items.Count && textureList.SelectedIndex != -1) {
                object object1 = textureList.Items[textureList.SelectedIndex + 1];
                object object2 = textureList.Items[textureList.SelectedIndex];

                textureList.Items[textureList.SelectedIndex] = object1;
                textureList.Items[textureList.SelectedIndex + 1] = object2;
                textureList.SelectedIndex += 1;
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
            hasDoneReload = false;
        }

        private void bakeMissingNormalsCheckbox_CheckedChanged(object sender, EventArgs e) {
            hasDoneReload = false;
            mask.Enabled = bakeNormals.Checked && textureList.SelectedIndex > -1;
        }

        private void generateMultiCheckBox_CheckedChanged(object sender, EventArgs e) {
            hasDoneReload = false;
        }

        private void bulkReplaceToolStripMenuItem_Click(object sender, EventArgs e) {
            FindAndReplace findAndReplace = new FindAndReplace();
            Tokenizer tokenizer = new Tokenizer((textureList.Items[textureList.SelectedIndex] as TextureSet).MaterialSetName);
            findAndReplace.ReplacementString.Text = tokenizer.GetToken();
            findAndReplace.Diffuse.CurrentPath = diffuse.CurrentPath;
            findAndReplace.Normal.CurrentPath = normal.CurrentPath;
            findAndReplace.Multi.CurrentPath = multi.CurrentPath;
            findAndReplace.Mask.CurrentPath = mask.CurrentPath;
            findAndReplace.Glow.CurrentPath = glow.CurrentPath;

            findAndReplace.MaterialSets.AddRange(textureList.Items.Cast<TextureSet>().ToArray());
            if (findAndReplace.ShowDialog() == DialogResult.OK) {
                MessageBox.Show("Replacement succeeded.", VersionText);
            }
        }

        private void findAndBulkReplaceToolStripMenuItem_Click(object sender, EventArgs e) {
            FindAndReplace findAndReplace = new FindAndReplace();
            findAndReplace.MaterialSets.AddRange(textureList.Items.Cast<TextureSet>().ToArray());
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
            if (facePart.SelectedIndex == 4 || facePart.SelectedIndex == 5) {
                asymCheckbox.Enabled = faceType.Enabled = subRaceList.Enabled = false;
                faceExtra.Enabled = true;
            } else {
                asymCheckbox.Enabled = faceType.Enabled = subRaceList.Enabled = true;
                faceExtra.Enabled = false;
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