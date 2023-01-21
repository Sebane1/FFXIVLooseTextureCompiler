using FFBardMusicPlayer.FFXIV;
using FFXIVLooseTextureCompiler.DataTypes;
using FFXIVLooseTextureCompiler.PathOrganization;
using FFXIVVoicePackCreator;
using FFXIVVoicePackCreator.Json;
using Newtonsoft.Json;
using Penumbra.Import.Dds;
using System.Diagnostics;

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
            baseBodyList.SelectedIndex = genderListBody.SelectedIndex = raceList.SelectedIndex = tailList.SelectedIndex = subRaceList.SelectedIndex = faceType.SelectedIndex = facePart.SelectedIndex = 0;
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
            if (string.IsNullOrEmpty(penumbraModPath)) {
                ConfigurePenumbraModFolder();
            }
            if (!string.IsNullOrWhiteSpace(modNameTextBox.Text)) {
                string modPath = Path.Combine(penumbraModPath, modNameTextBox.Text);
                jsonFilepath = Path.Combine(modPath, "default_mod.json");
                metaFilePath = Path.Combine(modPath, "meta.json");
                if (Directory.Exists(modPath)) {
                    Directory.Delete(modPath, true);
                } else {
                    generatedOnce = false;
                }
                Directory.CreateDirectory(modPath);
                int i = 0;
                foreach (MaterialSet materialSet in materialList.Items) {
                    Group group = new Group(materialSet.MaterialSetName.Replace(@"/", "-").Replace(@"\", "-"), "", 0, "Multi", 0);
                    string diffuseBodyDiskPath = !string.IsNullOrEmpty(materialSet.InternalDiffusePath) ? Path.Combine(modPath, materialSet.InternalDiffusePath.Replace("/", @"\")) : "";
                    string normalBodyDiskPath = !string.IsNullOrEmpty(materialSet.InternalNormalPath) ? Path.Combine(modPath, materialSet.InternalNormalPath.Replace("/", @"\")) : "";
                    string multiBodyDiskPath = !string.IsNullOrEmpty(materialSet.InternalMultiPath) ? Path.Combine(modPath, materialSet.InternalMultiPath.Replace("/", @"\")) : "";
                    if (!string.IsNullOrEmpty(materialSet.Diffuse) && !string.IsNullOrEmpty(materialSet.InternalDiffusePath)) {
                        byte[] diffuseData = new byte[0];
                        TextureImporter.PngToTex(materialSet.Diffuse, out diffuseData);
                        Option option = new Option(materialSet.MaterialSetName.ToLower().Contains("eye") ? "Normal" : "Diffuse", 0);
                        option.Files.Add(materialSet.InternalDiffusePath, materialSet.InternalDiffusePath.Replace("/", @"\"));
                        Directory.CreateDirectory(Path.GetDirectoryName(diffuseBodyDiskPath));
                        group.Options.Add(option);
                        File.WriteAllBytes(diffuseBodyDiskPath, diffuseData);
                    }
                    if (!string.IsNullOrEmpty(materialSet.Normal) && !string.IsNullOrEmpty(materialSet.InternalNormalPath)) {
                        byte[] normalData = new byte[0];
                        TextureImporter.PngToTex(materialSet.Normal, out normalData);
                        Option option = new Option(materialSet.MaterialSetName.ToLower().Contains("eye") ? "Multi" : "Normal", 0);
                        option.Files.Add(materialSet.InternalNormalPath, materialSet.InternalNormalPath.Replace("/", @"\"));
                        Directory.CreateDirectory(Path.GetDirectoryName(normalBodyDiskPath));
                        group.Options.Add(option);
                        File.WriteAllBytes(normalBodyDiskPath, normalData);
                    }
                    if (!string.IsNullOrEmpty(materialSet.Multi) && !string.IsNullOrEmpty(materialSet.InternalMultiPath)) {
                        byte[] multiData = new byte[0];
                        TextureImporter.PngToTex(materialSet.Multi, out multiData);
                        Option option = new Option(materialSet.MaterialSetName.ToLower().Contains("eye") ? "Catchlight" : "Multi", 0);
                        option.Files.Add(materialSet.InternalMultiPath, materialSet.InternalMultiPath.Replace("/", @"\"));
                        Directory.CreateDirectory(Path.GetDirectoryName(multiBodyDiskPath));
                        group.Options.Add(option);
                        File.WriteAllBytes(multiBodyDiskPath, multiData);
                    }
                    if (group.Options.Count > 0) {
                        string groupPath = Path.Combine(modPath, $"group_" + i++ + $"_{group.Name.ToLower()}.json");
                        ExportGroup(groupPath, group);
                    }
                }

                ExportJson();
                ExportMeta();
                if (generatedOnce) {
                    Hook.SendSyncKey(Keys.Enter);
                    Thread.Sleep(500);
                    Hook.SendString(@"/penumbra redraw");
                    Thread.Sleep(100);
                    Hook.SendSyncKey(Keys.Enter);
                } else {
                    Hook.SendSyncKey(Keys.Enter);
                    Thread.Sleep(500);
                    Hook.SendString(@"/penumbra reload");
                    Thread.Sleep(100);
                    Hook.SendSyncKey(Keys.Enter);
                    generatedOnce = true;
                }
                TopMost = true;
                BringToFront();
                TopMost = false;
                generateButton.Enabled = false;
                generateButton.Text = "Generation Succeed";
                generationCooldown.Start();
                //MessageBox.Show("Export succeeded!");
            } else {
                MessageBox.Show("Please enter a mod name!");
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
            string unique = (uniqueAuRa.Checked ? "0101" : "0001");
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
                            result = @"chara/human/c" + (genderListBody.SelectedIndex == 0 ? raceCodeBody.Masculine[raceList.SelectedIndex] : raceCodeBody.Feminine[raceList.SelectedIndex]) + @"/obj/body/b" + "0001" + @"/texture/--c" + raceCodeBody.Masculine[raceList.SelectedIndex] + "b0001_b" + GetTextureType(material) + ".tex";
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
                if (subRaceList.SelectedText.ToLower() == "the lost" || subRaceList.SelectedText.ToLower() == "highlander" || subRaceList.SelectedText.ToLower() == "duskwight" || subRaceList.SelectedText.ToLower() == "moon") {
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
            MaterialSet materialSet = new MaterialSet();
            materialSet.MaterialSetName = baseBodyList.Text + ", " + genderListBody.Text + ", " + raceList.Text;
            materialSet.InternalDiffusePath = GetBodyMaterialPath(0);
            materialSet.InternalNormalPath = GetBodyMaterialPath(1);
            materialSet.InternalMultiPath = GetBodyMaterialPath(2);
            materialList.Items.Add(materialSet);
            HasSaved = false;
        }

        private void addFaceButton_Click(object sender, EventArgs e) {
            MaterialSet materialSet = new MaterialSet();
            materialSet.MaterialSetName = facePart.Text + ", " + genderListBody.Text + ", " + subRaceList.Text + ", " + faceType.Text;
            if (facePart.SelectedIndex != 2) {
                materialSet.InternalDiffusePath = GetFaceMaterialPath(0);
                materialSet.InternalNormalPath = GetFaceMaterialPath(1);
                materialSet.InternalMultiPath = GetFaceMaterialPath(2);
            } else {
                materialSet.InternalDiffusePath = GetFaceMaterialPath(1);
                materialSet.InternalNormalPath = GetFaceMaterialPath(2);
                materialSet.InternalMultiPath = GetFaceMaterialPath(3);
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
            } else {
                MaterialSet materialSet = (materialList.Items[materialList.SelectedIndex] as MaterialSet);
                currentEditLabel.Text = "Editing: " + materialSet.MaterialSetName;
                diffuse.FilePath.Text = materialSet.Diffuse;
                normal.FilePath.Text = materialSet.Normal;
                multi.FilePath.Text = materialSet.Multi;

                diffuse.Enabled = !string.IsNullOrEmpty(materialSet.InternalDiffusePath);
                normal.Enabled = !string.IsNullOrEmpty(materialSet.InternalNormalPath);
                multi.Enabled = !string.IsNullOrEmpty(materialSet.InternalDiffusePath);
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
                materialSet.Diffuse = diffuse.FilePath.Text;
                materialSet.Normal = normal.FilePath.Text;
                materialSet.Multi = multi.FilePath.Text;
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
            diffuse.FilePath.Text = "";
            normal.FilePath.Text = "";
            multi.FilePath.Text = "";
            diffuse.Enabled = false;
            normal.Enabled = false;
            multi.Enabled = false;
            HasSaved = true;
            currentEditLabel.Text = "Please select a texture set to start importing";
        }
        private void multi_Leave(object sender, EventArgs e) {
            SetPaths();
        }

        private void multi_Enter(object sender, EventArgs e) {
            enteredField = true;
        }

        private void removeSelectionButton_Click(object sender, EventArgs e) {
            materialList.Items.RemoveAt(materialList.SelectedIndex);
        }

        private void clearList_Click(object sender, EventArgs e) {
            if (MessageBox.Show("This will irriversably remove everything from the list, including any changes. Are you sure?", VersionText, MessageBoxButtons.YesNo) == DialogResult.Yes) {
                {
                    materialList.Items.Clear();
                }
            }
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
            string lastDataPath = Path.Combine(dataPath, @"0.0.1.5\");
            if (Directory.Exists(lastDataPath)) {
                dataPath = lastDataPath;
            }
            string path = Path.Combine(dataPath, @"AuthorWebsite.config");
            if (File.Exists(path)) {
                using (StreamReader reader = new StreamReader(path)) {
                    _defaultWebsite = reader.ReadLine();
                }
            }
        }
        public void GetAuthorName() {
            string dataPath = Application.UserAppDataPath.Replace(Application.ProductVersion, null);
            string lastDataPath = Path.Combine(dataPath, @"0.0.1.5\");
            if (Directory.Exists(lastDataPath)) {
                dataPath = lastDataPath;
            }
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
                materialList.Items.AddRange(projectFile.MaterialSets?.ToArray());
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
            if (materialList.Items.Count == 0) {
                materialListContextMenu.Close();
            }
        }

        private void editPathsToolStripMenuItem_Click(object sender, EventArgs e) {
            CustomPathDialog customPathDialog = new CustomPathDialog();
            customPathDialog.MaterialSet = (materialList.Items[materialList.SelectedIndex] as MaterialSet);
            if (customPathDialog.ShowDialog() == DialogResult.OK) {
                MessageBox.Show("Material Set has been edited successfully", VersionText);
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
    }
}