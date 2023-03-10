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
        private Dictionary<string, string> xnormalCache;
        Dictionary<string, FileSystemWatcher> watchers = new Dictionary<string, FileSystemWatcher>();
        private bool lockDuplicateGeneration;
        private bool finalizeResults;
        private TextureProcessor textureProcessor;

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
            textureProcessor = new TextureProcessor();
            textureProcessor.OnProgressChange += TextureProcessor_OnProgressChange;
            textureProcessor.OnLaunchedXnormal += TextureProcessor_OnLaunchedXnormal;
        }

        private void TextureProcessor_OnLaunchedXnormal(object? sender, EventArgs e) {
            exportLabel.Text = "Wait For xNormal";
            Refresh();
            Application.DoEvents();
        }

        private void TextureProcessor_OnProgressChange(object? sender, EventArgs e) {
            exportProgress.Increment(1);
            Refresh();
            Application.DoEvents();
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

            bodyIdentifiers.Add(new RacialBodyIdentifiers("VANILLA", new List<string>() { "201", "401", "201", "201", "401", "1101", "1401", "1401", "Invalid", "1801" }));
            bodyIdentifiers.Add(new RacialBodyIdentifiers("BIBO+", new List<string>() { "midlander", "highlander", "midlander", "midlander", "highlander", "Invalid", "raen", "xaela", "Invalid", "viera" }));
            bodyIdentifiers.Add(new RacialBodyIdentifiers("EVE", new List<string>() { "middie", "buffie", "middie", "middie", "buffie", "Invalid", "lizard", "lizard2", "Invalid", "bunny" }));
            bodyIdentifiers.Add(new RacialBodyIdentifiers("GEN3", new List<string>() { "mid", "high", "mid", "mid", "high", "Invalid", "raen", "xaela", "Invalid", "viera" }));
            bodyIdentifiers.Add(new RacialBodyIdentifiers("SCALE+", new List<string>() { "Invalid", "Invalid", "Invalid", "Invalid", "Invalid", "Invalid", "raen", "xaela", "Invalid", "Invalid" }));
            bodyIdentifiers.Add(new RacialBodyIdentifiers("TBSE/HRBODY", new List<string>() { "Invalid", "Invalid", "Invalid", "Invalid", "Invalid", "Invalid", "Invalid", "Invalid", "Invalid", "Invalid" }));
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
            exportLabel.Text = "Exporting";
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
                    exportProgress.BringToFront();
                    exportProgress.Maximum = textureList.Items.Count * 3;
                    exportProgress.Visible = true;
                    Refresh();
                    List<TextureSet> textureSets = new List<TextureSet>();
                    foreach (TextureSet item in textureList.Items) {
                        if (item.OmniExportMode) {
                            ConfigureOmniConfiguration(item);
                        }
                        textureSets.Add(item);
                    }
                    textureProcessor.Export(textureSets, modPath, generationType.SelectedIndex, bakeNormals.Checked, generateMultiCheckBox.Checked, finalizeResults);
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
                    finalizeButton.Enabled = generateButton.Enabled = false;
                    generationCooldown.Start();
                    exportProgress.Visible = false;
                    exportProgress.Value = 0;
                    lockDuplicateGeneration = false;
                    modNameTextBox.Enabled = modAuthorTextBox.Enabled
                    = modWebsiteTextBox.Enabled = modVersionTextBox.Enabled
                    = modVersionTextBox.Enabled = modDescriptionTextBox.Enabled = true;
                    exportPanel.Visible = false;
                    finalizeResults = false;
                } else {
                    exportPanel.Visible = false;
                    lockDuplicateGeneration = false;
                    finalizeResults = false;
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

        private void ConfigurePenumbraModFolder() {
            MessageBox.Show("Please configure where your penumbra mods folder is, we will remember it for all future exports. This should be where you have penumbra set to use mods.\r\n\r\nNote:\r\nAVOID MANUALLY CREATING ANY NEW FOLDERS IN YOUR PENUMBRA FOLDER, ONLY SELECT THE BASE FOLDER!", VersionText);
            FolderBrowserDialog folderSelect = new FolderBrowserDialog();
            if (folderSelect.ShowDialog() == DialogResult.OK) {
                penumbraModPath = folderSelect.SelectedPath;
                WritePenumbraPath(penumbraModPath);
            }
        }
        private string GetBodyTexturePath(int texture, int genderValue, int baseBody, int race) {
            string result = "";
            string unique = (((string)raceList.Items[race]).Contains("Xaela") ? "0101" : "0001");
            switch (baseBody) {
                case 0:
                    // Vanila
                    if (texture == 2 && race == 5) {
                        result = @"chara/common/texture/skin_m.tex";
                    } else {
                        string genderCode = (genderValue == 0 ? raceCodeBody.Masculine[race]
                            : raceCodeBody.Feminine[race]);
                        result = @"chara/human/c" + genderCode + @"/obj/body/b" + unique
                            + @"/texture/--c" + genderCode + "b" + unique + GetTextureType(texture) + ".tex";
                    }
                    break;
                case 1:
                    // Bibo+
                    if (race != 5) {
                        if (genderValue == 1) {
                            result = @"chara/bibo/" + bodyIdentifiers[baseBody].RaceIdentifiers[race]
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
                    if (race != 5) {
                        if (genderValue == 1) {
                            if (texture != 2) {
                                result = @"chara/human/c" + (genderValue == 0 ? raceCodeBody.Masculine[race]
                                    : raceCodeBody.Feminine[race]) + @"/obj/body/b" + "0001" + @"/texture/eve2" +
                                    bodyIdentifiers[baseBody].RaceIdentifiers[race] + GetTextureType(texture) + ".tex";
                            } else {
                                if (race == 6) {
                                    result = "chara/human/c1401/obj/body/b0001/texture/eve2lizard_m.tex";
                                } else if (race == 7) {
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
                    if (race != 5) {
                        if (genderValue == 1) {
                            result = @"chara/human/c" + (genderValue == 0 ? raceCodeBody.Masculine[race]
                                : raceCodeBody.Feminine[race]) + @"/obj/body/b" + unique + @"/texture/tfgen3" +
                                bodyIdentifiers[baseBody].RaceIdentifiers[race] + "f" + GetTextureType(texture) + ".tex";
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
                    if (race != 5) {
                        if (race == 6 || race == 7) {
                            if (genderValue == 1) {
                                result = @"chara/bibo/" + bodyIdentifiers[baseBody].RaceIdentifiers[race] +
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
                    if (race != 5) {
                        if (genderValue == 0) {
                            // TBSE and HRBODY
                            result = @"chara/human/c" + (genderValue == 0 ? raceCodeBody.Masculine[race]
                                : raceCodeBody.Feminine[race]) + @"/obj/body/b" + (uniqueAuRa.Checked ? "0101" : "0001")
                                + @"/texture/--c" + raceCodeBody.Masculine[race] + "b0001_b" + GetTextureType(texture) + ".tex";
                        } else {
                            result = "";
                            MessageBox.Show("TBSE and HRBODY are only compatible with masculine characters", VersionText);
                        }
                    } else {
                        MessageBox.Show("TBSE and HRBODY are not compatible with lalafells", VersionText);
                    }
                    break;
                case 6:
                    // Tails
                    string xaelaCheck = (race == 7 ? "010" : "000") + (tailList.SelectedIndex + 1);
                    string gender = (genderValue == 0 ? raceCodeBody.Masculine[race]
                        : raceCodeBody.Feminine[race]);
                    result = @"chara/human/c" + gender + @"/obj/tail/t" + xaelaCheck + @"/texture/--c" + gender + "t" +
                        xaelaCheck + "_etc" + GetTextureType(texture) + ".tex";
                    break;
                case 7:
                    // Otopop
                    if (race == 5) {
                        if (texture == 0) {
                            return "chara/common/texture/skin_otopop.tex";
                        } else {
                            result = @"chara/human/c1101/obj/body/b0001/texture/v01_c1101b0001_g" + GetTextureType(texture) + ".tex";
                        }
                    } else {
                        MessageBox.Show("Otopop is only compatible with lalafells", VersionText);
                    }
                    break;
                case 8:
                    // Redefined Lala A
                    if (race == 5) {
                        result = @"chara/human/c1101/obj/body/b0001/texture/v01_c1101b0001" + GetTextureType(texture) + ".tex";
                    } else {
                        MessageBox.Show("Redefined Lala A is only compatible with lalafells", VersionText);
                    }
                    break;
                case 9:
                    // Redefined Lala B
                    if (race == 5) {
                        result = @"chara/human/c1101/obj/body/b0001/texture/v01_c1101b0001_b" + GetTextureType(texture) + ".tex";
                    } else {
                        MessageBox.Show("Redefined Lala B is only compatible with lalafells", VersionText);
                    }
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
                    || (selectedText.ToLower() == "xaela" && facePart.SelectedIndex != 2) || (selectedText.ToLower() == "veena" && facePart.SelectedIndex == 1)
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
                case 6:
                    return "_fac_b";
                case 7:
                    return "_etc_b";
            }
            return null;
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
                case 7:
                case 8:
                case 9:
                    genderListBody.Enabled = false;
                    if (raceList.SelectedIndex != 5) {
                        raceList.SelectedIndex = 5;
                    }
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
            if (baseBodyList.SelectedIndex > 0 && baseBodyList.SelectedIndex < 7) {
                if (raceList.SelectedIndex == 5) {
                    raceList.SelectedIndex = lastRaceIndex;
                    MessageBox.Show("Lalafells are not compatible with the selected body", VersionText);
                }
            }
            if (baseBodyList.SelectedIndex > 6) {
                if (raceList.SelectedIndex != 5) {
                    raceList.SelectedIndex = 5;
                    MessageBox.Show("Only Lalafells are compatible with the selected body", VersionText);
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
            TextureSet textureSet = new TextureSet();
            textureSet.MaterialSetName = baseBodyList.Text + (baseBodyList.Text.ToLower().Contains("tail") ? " " + (tailList.SelectedIndex + 1) : "") + ", " + (raceList.SelectedIndex == 5 ? "Unisex" : genderListBody.Text) + ", " + raceList.Text;
            if (raceList.SelectedIndex != 3 || baseBodyList.SelectedIndex != 6) {
                textureSet.InternalDiffusePath = GetBodyTexturePath(0, genderListBody.SelectedIndex, baseBodyList.SelectedIndex, raceList.SelectedIndex);
            }
            textureSet.InternalNormalPath = GetBodyTexturePath(1, genderListBody.SelectedIndex, baseBodyList.SelectedIndex, raceList.SelectedIndex);
            textureSet.InternalMultiPath = GetBodyTexturePath(2, genderListBody.SelectedIndex, baseBodyList.SelectedIndex, raceList.SelectedIndex);
            textureList.Items.Add(textureSet);
            HasSaved = false;
        }

        private void addFaceButton_Click(object sender, EventArgs e) {
            hasDoneReload = false;
            TextureSet textureSet = new TextureSet();
            textureSet.MaterialSetName = facePart.Text + (facePart.SelectedIndex == 4 ? " " + (faceExtra.SelectedIndex + 1) : "") + ", " + (facePart.SelectedIndex != 4 ? genderListBody.Text : "Unisex") + ", " + (facePart.SelectedIndex != 4 ? subRaceList.Text : "Multi Race") + ", " + (facePart.SelectedIndex != 4 ? faceType.Text : "Multi Face");
            switch (facePart.SelectedIndex) {
                default:
                    textureSet.InternalDiffusePath = GetFaceTexturePath(0);
                    textureSet.InternalNormalPath = GetFaceTexturePath(1);
                    textureSet.InternalMultiPath = GetFaceTexturePath(2);
                    break;
                case 2:
                    textureSet.InternalDiffusePath = GetFaceTexturePath(1);
                    textureSet.InternalNormalPath = GetFaceTexturePath(2);
                    textureSet.InternalMultiPath = GetFaceTexturePath(3);
                    break;
                case 4:
                    textureSet.InternalDiffusePath = "chara/common/texture/decal_face/_decal_" + (faceExtra.SelectedIndex + 1) + ".tex";
                    break;
                case 5:
                    textureSet.MaterialSetName = facePart.Text + " " + (faceExtra.SelectedIndex + 1) + ", " + genderListBody.Text + ", " + subRaceList.Text;
                    textureSet.InternalNormalPath = GetHairTexturePath(1);
                    textureSet.InternalMultiPath = GetHairTexturePath(2);
                    break;

            }
            textureSet.IgnoreMultiGeneration = true;
            textureList.Items.Add(textureSet);
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
            } else {
                omniExportModeToolStripMenuItem.Text = (textureList.SelectedItem as TextureSet).OmniExportMode ? "Disable Universal Compatibility" : "Enable Universal Compatibility";
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
            finalizeButton.Enabled = generateButton.Enabled = true;
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
                textureList.SelectedIndex = -1;
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

        private void omniExportModeToolStripMenuItem_Click(object sender, EventArgs e) {
            TextureSet textureSet = (textureList.Items[textureList.SelectedIndex] as TextureSet);
            if (textureSet != null) {
                if (!textureSet.OmniExportMode) {
                    ConfigureOmniConfiguration(textureSet);
                    MessageBox.Show("Enabling universal compatibility mode allows your currently selected body textures to be compatible with other bodies on a best effort basis.\r\n\r\nWarning: this slows down the generation process, so you will need to click the finalize button to update changes on bodies that arent this one.", VersionText);
                } else {
                    textureSet.OmniExportMode = false;
                    textureSet.ChildSets.Clear();
                }
            }
        }

        public int ReverseRaceLookup(string path) {
            for (int i = 1; i < raceList.Items.Count; i++) {
                string vanilla = bodyIdentifiers[0].RaceIdentifiers[i];
                string bibo = bodyIdentifiers[1].RaceIdentifiers[i];
                string eve = bodyIdentifiers[2].RaceIdentifiers[i];
                string tnf = bodyIdentifiers[3].RaceIdentifiers[i];
                if (path.Contains("c" + vanilla)
                        || path.Contains(bibo)
                        || path.Contains(eve)
                        || path.Contains(tnf)) {
                    return i;
                }
            }
            if (path.Contains("1101") || path.Contains("otopop")) {
                return 5;
            }
            return -1;
        }

        private void ConfigureOmniConfiguration(TextureSet textureSet) {
            textureSet.OmniExportMode = true;
            textureSet.ChildSets.Clear();
            int race = ReverseRaceLookup(textureSet.InternalDiffusePath);
            if (textureSet.InternalDiffusePath.Contains("bibo")) {
                TextureSet vanilla = new TextureSet();
                vanilla.MaterialSetName = "Vanilla Compatibility";
                vanilla.InternalDiffusePath = GetBodyTexturePath(0, 1, 0, race);
                vanilla.InternalNormalPath = GetBodyTexturePath(1, 1, 0, race);
                vanilla.InternalMultiPath = GetBodyTexturePath(2, 1, 0, race);
                vanilla.Diffuse = textureSet.Diffuse.Replace(".", "_gen2_baseTexBaked.");
                vanilla.Normal = textureSet.Normal.Replace(".", "_gen2_baseTexBaked.");
                vanilla.Multi = textureSet.Multi.Replace(".", "_gen2_baseTexBaked.");

                TextureSet eve = new TextureSet();
                eve.MaterialSetName = "Eve Compatibility";
                eve.InternalDiffusePath = GetBodyTexturePath(0, 1, 2, race);
                eve.InternalNormalPath = GetBodyTexturePath(1, 1, 2, race);
                eve.InternalMultiPath = GetBodyTexturePath(2, 1, 2, race);
                eve.Diffuse = textureSet.Diffuse.Replace(".", "_gen3_baseTexBaked.");
                eve.Normal = textureSet.Normal.Replace(".", "_gen3_baseTexBaked.");
                eve.Multi = textureSet.Multi.Replace(".", "_gen3_baseTexBaked.");

                TextureSet gen3 = new TextureSet();
                gen3.MaterialSetName = "Tight & Firm Compatibility";
                gen3.InternalDiffusePath = GetBodyTexturePath(0, 1, 3, race);
                gen3.InternalNormalPath = GetBodyTexturePath(1, 1, 3, race);
                gen3.InternalMultiPath = GetBodyTexturePath(2, 1, 3, race);
                gen3.Diffuse = textureSet.Diffuse.Replace(".", "_gen3_baseTexBaked.");
                gen3.Normal = textureSet.Normal.Replace(".", "_gen3_baseTexBaked.");
                gen3.Multi = textureSet.Multi.Replace(".", "_gen3_baseTexBaked.");

                textureSet.ChildSets.Add(vanilla);
                textureSet.ChildSets.Add(eve);
                textureSet.ChildSets.Add(gen3);
            } else if (textureSet.InternalDiffusePath.Contains("eve")) {
                TextureSet vanilla = new TextureSet();
                vanilla.MaterialSetName = "Vanilla Compatibility";
                vanilla.InternalDiffusePath = GetBodyTexturePath(0, 1, 0, race);
                vanilla.InternalNormalPath = GetBodyTexturePath(1, 1, 0, race);
                vanilla.InternalMultiPath = GetBodyTexturePath(2, 1, 0, race);
                vanilla.Diffuse = textureSet.Diffuse.Replace(".", "_gen2_baseTexBaked.");
                vanilla.Normal = textureSet.Normal.Replace(".", "_gen2_baseTexBaked.");
                vanilla.Multi = textureSet.Multi.Replace(".", "_gen2_baseTexBaked.");

                TextureSet bibo = new TextureSet();
                bibo.MaterialSetName = "Bibo+ Compatibility";
                bibo.InternalDiffusePath = GetBodyTexturePath(0, 1, 1, race);
                bibo.InternalNormalPath = GetBodyTexturePath(1, 1, 1, race);
                bibo.InternalMultiPath = GetBodyTexturePath(2, 1, 1, race);
                bibo.Diffuse = textureSet.Diffuse.Replace(".", "_bibo_baseTexBaked.");
                bibo.Normal = textureSet.Normal.Replace(".", "_bibo_baseTexBaked.");
                bibo.Multi = textureSet.Multi.Replace(".", "_bibo_baseTexBaked.");

                TextureSet gen3 = new TextureSet();
                gen3.MaterialSetName = "Tight & Firm Compatibility";
                gen3.InternalDiffusePath = GetBodyTexturePath(0, 1, 3, race);
                gen3.InternalNormalPath = GetBodyTexturePath(1, 1, 3, race);
                gen3.InternalMultiPath = GetBodyTexturePath(2, 1, 3, race);
                gen3.Diffuse = textureSet.Diffuse;
                gen3.Normal = textureSet.Normal;
                gen3.Multi = textureSet.Multi;
                gen3.Glow = textureSet.Glow;
                gen3.NormalMask = textureSet.NormalMask;

                textureSet.ChildSets.Add(vanilla);
                textureSet.ChildSets.Add(bibo);
                textureSet.ChildSets.Add(gen3);
            } else if (textureSet.InternalDiffusePath.Contains("gen3")) {
                TextureSet vanilla = new TextureSet();
                vanilla.MaterialSetName = "Vanilla Compatibility";
                vanilla.InternalDiffusePath = GetBodyTexturePath(0, 1, 0, race);
                vanilla.InternalNormalPath = GetBodyTexturePath(1, 1, 0, race);
                vanilla.InternalMultiPath = GetBodyTexturePath(2, 1, 0, race);
                vanilla.Diffuse = textureSet.Diffuse.Replace(".", "_gen2_baseTexBaked.");
                vanilla.Normal = textureSet.Normal.Replace(".", "_gen2_baseTexBaked.");
                vanilla.Multi = textureSet.Multi.Replace(".", "_gen2_baseTexBaked.");

                TextureSet bibo = new TextureSet();
                bibo.MaterialSetName = "Bibo+ Compatibility";
                bibo.InternalDiffusePath = GetBodyTexturePath(0, 1, 1, race);
                bibo.InternalNormalPath = GetBodyTexturePath(1, 1, 1, race);
                bibo.InternalMultiPath = GetBodyTexturePath(2, 1, 1, race);
                bibo.Diffuse = textureSet.Diffuse.Replace(".", "_bibo_baseTexBaked.");
                bibo.Normal = textureSet.Normal.Replace(".", "_bibo_baseTexBaked.");
                bibo.Multi = textureSet.Multi.Replace(".", "_bibo_baseTexBaked.");

                TextureSet eve = new TextureSet();
                eve.MaterialSetName = "Eve Compatibility";
                eve.InternalDiffusePath = GetBodyTexturePath(0, 1, 2, race);
                eve.InternalNormalPath = GetBodyTexturePath(1, 1, 2, race);
                eve.InternalMultiPath = GetBodyTexturePath(2, 1, 2, race);
                eve.Diffuse = textureSet.Diffuse;
                eve.Normal = textureSet.Normal;
                eve.Multi = textureSet.Multi;
                eve.Glow = textureSet.Glow;
                eve.NormalMask = textureSet.NormalMask;

                textureSet.ChildSets.Add(vanilla);
                textureSet.ChildSets.Add(bibo);
                textureSet.ChildSets.Add(eve);
            } else if (textureSet.InternalDiffusePath.Contains("skin_otopop")) {
                TextureSet vanilla = new TextureSet();
                TextureSet redefinedLalaA = new TextureSet();
                redefinedLalaA.MaterialSetName = "Redefined Lala A Compatibility";
                vanilla.MaterialSetName = "Vanilla Compatibility";
                vanilla.InternalDiffusePath = GetBodyTexturePath(0, 1, 0, race);
                vanilla.InternalNormalPath = GetBodyTexturePath(1, 1, 0, race);
                vanilla.InternalMultiPath = GetBodyTexturePath(2, 1, 0, race);
                redefinedLalaA.InternalDiffusePath = GetBodyTexturePath(0, 0, 8, race);
                redefinedLalaA.InternalNormalPath = GetBodyTexturePath(1, 0, 8, race);
                redefinedLalaA.InternalMultiPath = GetBodyTexturePath(2, 0, 8, race);
                redefinedLalaA.Diffuse = vanilla.Diffuse = textureSet.Diffuse.Replace(".", "_vanilla_lala_baseTexBaked.");
                redefinedLalaA.Normal = vanilla.Normal = textureSet.Normal.Replace(".", "_vanilla_lala_baseTexBaked.");
                redefinedLalaA.Multi = vanilla.Multi = textureSet.Multi.Replace(".", "_vanilla_lala_baseTexBaked.");

                TextureSet redefinedLalaB = new TextureSet();
                redefinedLalaB.MaterialSetName = "Redefined Lala B Compatibility";
                redefinedLalaB.InternalDiffusePath = GetBodyTexturePath(0, 0, 9, race);
                redefinedLalaB.InternalNormalPath = GetBodyTexturePath(1, 0, 9, race);
                redefinedLalaB.InternalMultiPath = GetBodyTexturePath(2, 0, 9, race);
                redefinedLalaB.Diffuse = textureSet.Diffuse.Replace(".", "_redefined_lala_baseTexBaked.");
                redefinedLalaB.Normal = textureSet.Normal.Replace(".", "redefined_lala_baseTexBaked.");
                redefinedLalaB.Multi = textureSet.Multi.Replace(".", "redefined_lala_baseTexBaked.");

                textureSet.ChildSets.Add(vanilla);
                textureSet.ChildSets.Add(redefinedLalaA);
                textureSet.ChildSets.Add(redefinedLalaB);
            } else if (textureSet.InternalDiffusePath.Contains("v01_c1101b0001_b_")) {
                TextureSet vanilla = new TextureSet();
                TextureSet redefinedLalaA = new TextureSet();
                redefinedLalaA.MaterialSetName = "Redefined Lala A Compatibility";
                vanilla.MaterialSetName = "Vanilla Compatibility";
                vanilla.InternalDiffusePath = GetBodyTexturePath(0, 0, 0, race);
                vanilla.InternalNormalPath = GetBodyTexturePath(1, 0, 0, race);
                vanilla.InternalMultiPath = GetBodyTexturePath(2, 0, 0, race);
                redefinedLalaA.InternalDiffusePath = GetBodyTexturePath(0, 0, 8, race);
                redefinedLalaA.InternalNormalPath = GetBodyTexturePath(1, 0, 8, race);
                redefinedLalaA.InternalMultiPath = GetBodyTexturePath(2, 0, 8, race);
                redefinedLalaA.Diffuse = vanilla.Diffuse = textureSet.Diffuse.Replace(".", "_vanilla_lala_baseTexBaked.");
                redefinedLalaA.Normal = vanilla.Normal = textureSet.Normal.Replace(".", "_vanilla_lala_baseTexBaked.");
                redefinedLalaA.Multi = vanilla.Multi = textureSet.Multi.Replace(".", "_vanilla_lala_baseTexBaked.");

                TextureSet otopop = new TextureSet();
                otopop.MaterialSetName = "Otopop Compatibility";
                otopop.InternalDiffusePath = GetBodyTexturePath(0, 1, 7, race);
                otopop.InternalNormalPath = GetBodyTexturePath(1, 1, 7, race);
                otopop.InternalMultiPath = GetBodyTexturePath(2, 1, 7, race);
                otopop.Diffuse = textureSet.Diffuse.Replace(".", "_otopop_baseTexBaked.");
                otopop.Normal = textureSet.Normal.Replace(".", "otopop_baseTexBaked.");
                otopop.Multi = textureSet.Multi.Replace(".", "otopop_baseTexBaked.");

                textureSet.ChildSets.Add(vanilla);
                textureSet.ChildSets.Add(redefinedLalaA);
                textureSet.ChildSets.Add(otopop);
            } else if (textureSet.InternalDiffusePath.Contains("--c1101b0001_")) {
                TextureSet redefinedLalaA = new TextureSet();
                redefinedLalaA.MaterialSetName = "Redefined Lala A Compatibility";
                redefinedLalaA.InternalDiffusePath = GetBodyTexturePath(0, 0, 8, race);
                redefinedLalaA.InternalNormalPath = GetBodyTexturePath(1, 0, 8, race);
                redefinedLalaA.InternalMultiPath = GetBodyTexturePath(2, 0, 8, race);
                redefinedLalaA.Diffuse = textureSet.Diffuse;
                redefinedLalaA.Normal = textureSet.Normal;
                redefinedLalaA.Multi = textureSet.Multi;
                redefinedLalaA.Glow = textureSet.Glow;
                redefinedLalaA.NormalMask = textureSet.NormalMask;

                TextureSet otopop = new TextureSet();
                otopop.MaterialSetName = "Otopop Compatibility";
                otopop.InternalDiffusePath = GetBodyTexturePath(0, 1, 7, race);
                otopop.InternalNormalPath = GetBodyTexturePath(1, 1, 7, race);
                otopop.InternalMultiPath = GetBodyTexturePath(2, 1, 7, race);
                otopop.Diffuse = textureSet.Diffuse.Replace(".", "_otopop_baseTexBaked.");
                otopop.Normal = textureSet.Normal.Replace(".", "otopop_baseTexBaked.");
                otopop.Multi = textureSet.Multi.Replace(".", "otopop_baseTexBaked.");

                TextureSet redefinedLalaB = new TextureSet();
                redefinedLalaB.MaterialSetName = "Redefined Lala B Compatibility";
                redefinedLalaB.InternalDiffusePath = GetBodyTexturePath(0, 1, 9, race);
                redefinedLalaB.InternalNormalPath = GetBodyTexturePath(1, 1, 9, race);
                redefinedLalaB.InternalMultiPath = GetBodyTexturePath(2, 1, 9, race);
                redefinedLalaB.Diffuse = textureSet.Diffuse.Replace(".", "redefined_lala_baseTexBaked.");
                redefinedLalaB.Normal = textureSet.Normal.Replace(".", "redefined_lala_baseTexBaked.");
                redefinedLalaB.Multi = textureSet.Multi.Replace(".", "redefined_lala_baseTexBaked.");

                textureSet.ChildSets.Add(otopop);
                textureSet.ChildSets.Add(redefinedLalaA);
                textureSet.ChildSets.Add(redefinedLalaB);
            } else if (textureSet.InternalDiffusePath.Contains("v01_c1101b0001_")) {
                TextureSet vanilla = new TextureSet();
                vanilla.MaterialSetName = "Vanilla Compatibility";
                vanilla.InternalDiffusePath = GetBodyTexturePath(0, 0, 0, race);
                vanilla.InternalNormalPath = GetBodyTexturePath(1, 0, 0, race);
                vanilla.InternalMultiPath = GetBodyTexturePath(2, 0, 0, race);
                vanilla.Diffuse = textureSet.Diffuse;
                vanilla.Normal = textureSet.Normal;
                vanilla.Multi = textureSet.Multi;
                vanilla.Glow = textureSet.Glow;
                vanilla.NormalMask = textureSet.NormalMask;

                TextureSet otopop = new TextureSet();
                otopop.MaterialSetName = "Otopop Compatibility";
                otopop.InternalDiffusePath = GetBodyTexturePath(0, 1, 7, race);
                otopop.InternalNormalPath = GetBodyTexturePath(1, 1, 7, race);
                otopop.InternalMultiPath = GetBodyTexturePath(2, 1, 7, race);
                otopop.Diffuse = textureSet.Diffuse.Replace(".", "_otopop_baseTexBaked.");
                otopop.Normal = textureSet.Normal.Replace(".", "otopop_baseTexBaked.");
                otopop.Multi = textureSet.Multi.Replace(".", "otopop_baseTexBaked.");

                TextureSet redefinedLalaB = new TextureSet();
                redefinedLalaB.MaterialSetName = "Redefined Lala B Compatibility";
                redefinedLalaB.InternalDiffusePath = GetBodyTexturePath(0, 1, 8, race);
                redefinedLalaB.InternalNormalPath = GetBodyTexturePath(1, 1, 8, race);
                redefinedLalaB.InternalMultiPath = GetBodyTexturePath(2, 1, 8, race);
                redefinedLalaB.Diffuse = textureSet.Diffuse.Replace(".", "redefined_lala_baseTexBaked.");
                redefinedLalaB.Normal = textureSet.Normal.Replace(".", "redefined_lala_baseTexBaked.");
                redefinedLalaB.Multi = textureSet.Multi.Replace(".", "redefined_lala_baseTexBaked.");

                textureSet.ChildSets.Add(otopop);
                textureSet.ChildSets.Add(vanilla);
                textureSet.ChildSets.Add(redefinedLalaB);
            }
        }

        private void finalizeButton_Click(object sender, EventArgs e) {
            finalizeResults = true;
            generateButton_Click(sender, e);
        }

        private void xNormalToolStripMenuItem_Click(object sender, EventArgs e) {
            XNormal.OpenXNormal();
        }

        private void biboToGen3ToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            openFileDialog.Filter = "Texture File|*.png;*.dds;*.bmp;**.tex;";
            saveFileDialog.Filter = "Texture File|*.png;*.dds;*.bmp;**.tex;";
            MessageBox.Show("Please select input texture");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                MessageBox.Show("Please select where you want to save the conversion");
                if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                    XNormal.BiboToGen3(openFileDialog.FileName, saveFileDialog.FileName);
                }
            }
        }

        private void gen3ToBiboToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            openFileDialog.Filter = "Texture File|*.png;*.dds;*.bmp;**.tex;";
            saveFileDialog.Filter = "Texture File|*.png;*.dds;*.bmp;**.tex;";
            MessageBox.Show("Please select input texture");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                MessageBox.Show("Please select where you want to save the conversion");
                if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                    XNormal.Gen3ToBibo(openFileDialog.FileName, saveFileDialog.FileName);
                }
            }
        }

        private void gen3ToGen2ToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            openFileDialog.Filter = "Texture File|*.png;*.dds;*.bmp;**.tex;";
            saveFileDialog.Filter = "Texture File|*.png;*.dds;*.bmp;**.tex;";
            MessageBox.Show("Please select input texture");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                MessageBox.Show("Please select where you want to save the conversion");
                if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                    XNormal.Gen3ToGen2(openFileDialog.FileName, saveFileDialog.FileName);
                }
            }
        }

        private void gen2ToGen3ToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            openFileDialog.Filter = "Texture File|*.png;*.dds;*.bmp;**.tex;";
            saveFileDialog.Filter = "Texture File|*.png;*.dds;*.bmp;**.tex;";
            MessageBox.Show("Please select input texture");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                MessageBox.Show("Please select where you want to save the conversion");
                if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                    XNormal.Gen2ToGen3(openFileDialog.FileName, saveFileDialog.FileName);
                }
            }
        }

        private void gen2ToBiboToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            openFileDialog.Filter = "Texture File|*.png;*.dds;*.bmp;**.tex;";
            saveFileDialog.Filter = "Texture File|*.png;*.dds;*.bmp;**.tex;";
            MessageBox.Show("Please select input texture");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                MessageBox.Show("Please select where you want to save the conversion");
                if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                    XNormal.Gen2ToBibo(openFileDialog.FileName, saveFileDialog.FileName);
                }
            }
        }

        private void toolsToolStripMenuItem_Click(object sender, EventArgs e) {

        }

        private void biboToGen2ToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            openFileDialog.Filter = "Texture File|*.png;*.dds;*.bmp;**.tex;";
            saveFileDialog.Filter = "Texture File|*.png;*.dds;*.bmp;**.tex;";
            MessageBox.Show("Please select input texture");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                MessageBox.Show("Please select where you want to save the conversion");
                if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                    XNormal.BiboToGen2(openFileDialog.FileName, saveFileDialog.FileName);
                }
            }
        }

        private void otopopToRedefinedLalaToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            openFileDialog.Filter = "Texture File|*.png;*.dds;*.bmp;**.tex;";
            saveFileDialog.Filter = "Texture File|*.png;*.dds;*.bmp;**.tex;";
            MessageBox.Show("Please select input texture");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                MessageBox.Show("Please select where you want to save the conversion");
                if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                    XNormal.OtopopToRedefinedLala(openFileDialog.FileName, saveFileDialog.FileName);
                }
            }
        }

        private void otopopToVanillaToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            openFileDialog.Filter = "Texture File|*.png;*.dds;*.bmp;**.tex;";
            saveFileDialog.Filter = "Texture File|*.png;*.dds;*.bmp;**.tex;";
            MessageBox.Show("Please select input texture");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                MessageBox.Show("Please select where you want to save the conversion");
                if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                    XNormal.OtopopToVanillaLala(openFileDialog.FileName, saveFileDialog.FileName);
                }
            }
        }

        private void redefinedLalaToOtopopToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            openFileDialog.Filter = "Texture File|*.png;*.dds;*.bmp;**.tex;";
            saveFileDialog.Filter = "Texture File|*.png;*.dds;*.bmp;**.tex;";
            MessageBox.Show("Please select input texture");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                MessageBox.Show("Please select where you want to save the conversion");
                if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                    XNormal.RedefinedLalaToOtopop(openFileDialog.FileName, saveFileDialog.FileName);
                }
            }
        }

        private void redefinedLalaToVanillaToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            openFileDialog.Filter = "Texture File|*.png;*.dds;*.bmp;**.tex;";
            saveFileDialog.Filter = "Texture File|*.png;*.dds;*.bmp;**.tex;";
            MessageBox.Show("Please select input texture");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                MessageBox.Show("Please select where you want to save the conversion");
                if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                    XNormal.RedefinedLalaToVanillaLala(openFileDialog.FileName, saveFileDialog.FileName);
                }
            }
        }

        private void vanillaToOtopopToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            openFileDialog.Filter = "Texture File|*.png;*.dds;*.bmp;**.tex;";
            saveFileDialog.Filter = "Texture File|*.png;*.dds;*.bmp;**.tex;";
            MessageBox.Show("Please select input texture");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                MessageBox.Show("Please select where you want to save the conversion");
                if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                    XNormal.VanillaLalaToOtopop(openFileDialog.FileName, saveFileDialog.FileName);
                }
            }
        }

        private void vanillaToRedefinedLalaToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            openFileDialog.Filter = "Texture File|*.png;*.dds;*.bmp;**.tex;";
            saveFileDialog.Filter = "Texture File|*.png;*.dds;*.bmp;**.tex;";
            MessageBox.Show("Please select input texture");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                MessageBox.Show("Please select where you want to save the conversion");
                if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                    XNormal.VanillaLalaToRedefinedLala(openFileDialog.FileName, saveFileDialog.FileName);
                }
            }
        }

        private void howDoIUseThisToolStripMenuItem_Click(object sender, EventArgs e) {
            try {
                Process.Start(new System.Diagnostics.ProcessStartInfo() {
                    FileName = "https://docs.google.com/document/d/1AR53LNy0dQ6X7L6NSfQY4PkZoUFgqfEYPzHPCcnW_YY/edit?usp=share_link",
                    UseShellExecute = true,
                    Verb = "OPEN"
                });
            } catch {

            }
        }

        private void howDoIMakeStuffBumpyToolStripMenuItem_Click(object sender, EventArgs e) {
            try {
                Process.Start(new System.Diagnostics.ProcessStartInfo() {
                    FileName = "https://docs.google.com/document/d/1UMmHVM2Iqvw7jPQ1Ff3MIy_-Cqwam1dcywBcOdyrp8E/edit?usp=share_link",
                    UseShellExecute = true,
                    Verb = "OPEN"
                });
            } catch {

            }
        }

        private void howDoIMakeStuffGlowToolStripMenuItem_Click(object sender, EventArgs e) {
            try {
                Process.Start(new System.Diagnostics.ProcessStartInfo() {
                    FileName = "https://docs.google.com/document/d/1G5Qu6cywPPdsdc-LUyfVGg8pvTITTHL_U8peI8G8yvI/edit?usp=share_link",
                    UseShellExecute = true,
                    Verb = "OPEN"
                });
            } catch {

            }
        }

        private void canIMakeMyBiboOrGen3BodyWorkOnAnotherBodyToolStripMenuItem_Click(object sender, EventArgs e) {
            try {
                Process.Start(new System.Diagnostics.ProcessStartInfo() {
                    FileName = "https://docs.google.com/document/d/1jXWL5cE9bQL5KPbIzAXKdM7_UgSz8zz2hO9fj5xysHg/edit?usp=share_link",
                    UseShellExecute = true,
                    Verb = "OPEN"
                });
            } catch {

            }
        }

        private void canIReplaceABunchOfStuffAtOnceToolStripMenuItem_Click(object sender, EventArgs e) {
            try {
                Process.Start(new System.Diagnostics.ProcessStartInfo() {
                    FileName = "https://docs.google.com/document/d/1Va4rhnhemJirHVHj4-ZZbvNbeQnCHJHEF_uoc3kb1jA/edit?usp=sharing",
                    UseShellExecute = true,
                    Verb = "OPEN"
                });
            } catch {

            }
        }

        private void canICustomizeTheGroupsThisToolExporrtsToolStripMenuItem_Click(object sender, EventArgs e) {
            try {
                Process.Start(new System.Diagnostics.ProcessStartInfo() {
                    FileName = "https://docs.google.com/document/d/1ISKq1k7ebK7x8XkTvCUWWERaHCjFWodJb1kQncOPLwc/edit?usp=sharing",
                    UseShellExecute = true,
                    Verb = "OPEN"
                });
            } catch {

            }
        }
    }
}