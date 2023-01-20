using FFXIVLooseTextureCompiler.DataTypes;
using FFXIVVoicePackCreator.Json;
using Newtonsoft.Json;
using Penumbra.Import.Dds;
using System.Diagnostics;

namespace FFXIVLooseTextureCompiler {
    public partial class MainWindow : Form {
        private RaceCode raceCodeBody;
        private RaceCode raceCodeFace;
        private List<RacialBodyIdentifiers> bodyIdentifiers = new List<RacialBodyIdentifiers>();
        private int lastRaceIndex;
        private string? penumbraModPath;
        private string jsonFilepath;
        private string metaFilePath;

        public MainWindow() {
            AutoScaleDimensions = new SizeF(96, 96);
            InitializeComponent();
            GetPenumbraPath();
            Text += " " + Application.ProductVersion;
        }

        private void Form1_Load(object sender, EventArgs e) {
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
        }

        private void generateButton_Click(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(penumbraModPath)) {
                ConfigurePenumbraModFolder();
            }
            if (!string.IsNullOrWhiteSpace(modNameTextBox.Text)) {
                string modPath = Path.Combine(penumbraModPath, modNameTextBox.Text);
                jsonFilepath = Path.Combine(modPath, "default_mod.json");
                metaFilePath = Path.Combine(modPath, "meta.json");

                string diffuseBodyPath = GetBodyMaterialPath(0);
                string normalBodyPath = GetBodyMaterialPath(1);
                string multiBodyPath = GetBodyMaterialPath(2);

                string diffuseFacePath = GetFaceMaterialPath(0);
                string normalFacePath = GetFaceMaterialPath(1);
                string multiFacePath = GetFaceMaterialPath(2);

                string diffuseBodyDiskPath = Path.Combine(modPath, diffuseBodyPath.Replace("/", @"\"));
                string normalBodyDiskPath = Path.Combine(modPath, normalBodyPath.Replace("/", @"\"));
                string multiBodyDiskPath = Path.Combine(modPath, multiBodyPath.Replace("/", @"\"));

                string diffuseFaceDiskPath = Path.Combine(modPath, diffuseFacePath.Replace("/", @"\"));
                string normalFaceDiskPath = Path.Combine(modPath, normalFacePath.Replace("/", @"\"));
                string multiFaceDiskPath = Path.Combine(modPath, multiFacePath.Replace("/", @"\"));

                Group group = new Group($"Body", "Body Textures", 0, "Multi", 0);
                Group group2 = new Group($"Face", "Face Textures", 0, "Multi", 0);
                if (Directory.Exists(modPath)) {
                    Directory.Delete(modPath, true);
                }
                Directory.CreateDirectory(modPath);
                if (!string.IsNullOrEmpty(diffuseB.FilePath.Text)) {
                    byte[] diffuseData = new byte[0];
                    TextureImporter.PngToTex(diffuseB.FilePath.Text, out diffuseData);
                    Option option = new Option("Diffuse", 0);
                    option.Files.Add(diffuseBodyPath, diffuseBodyPath.Replace("/", @"\"));
                    Directory.CreateDirectory(Path.GetDirectoryName(diffuseBodyDiskPath));
                    group.Options.Add(option);
                    File.WriteAllBytes(diffuseBodyDiskPath, diffuseData);
                }
                if (!string.IsNullOrEmpty(normalB.FilePath.Text)) {
                    byte[] normalData = new byte[0];
                    TextureImporter.PngToTex(normalB.FilePath.Text, out normalData);
                    Option option = new Option("Normal", 0);
                    option.Files.Add(diffuseBodyPath, diffuseBodyPath.Replace("/", @"\"));
                    Directory.CreateDirectory(Path.GetDirectoryName(normalBodyDiskPath));
                    group.Options.Add(option);
                    File.WriteAllBytes(normalBodyDiskPath, normalData);
                }
                if (!string.IsNullOrEmpty(multiB.FilePath.Text)) {
                    byte[] multiData = new byte[0];
                    TextureImporter.PngToTex(multiB.FilePath.Text, out multiData);
                    Option option = new Option("Multi", 0);
                    option.Files.Add(multiBodyPath, multiBodyPath.Replace("/", @"\"));
                    Directory.CreateDirectory(Path.GetDirectoryName(multiBodyDiskPath));
                    group.Options.Add(option);
                    File.WriteAllBytes(multiBodyDiskPath, multiData);
                }

                if (!string.IsNullOrEmpty(diffuseF.FilePath.Text)) {
                    byte[] diffuseData = new byte[0];
                    TextureImporter.PngToTex(diffuseF.FilePath.Text, out diffuseData);
                    Option option = new Option("Diffuse", 0);
                    option.Files.Add(diffuseFacePath, diffuseFacePath.Replace("/", @"\"));
                    Directory.CreateDirectory(Path.GetDirectoryName(diffuseFaceDiskPath));
                    group2.Options.Add(option);
                    File.WriteAllBytes(diffuseFaceDiskPath, diffuseData);
                }
                if (!string.IsNullOrEmpty(normalF.FilePath.Text)) {
                    byte[] normalData = new byte[0];
                    TextureImporter.PngToTex(normalF.FilePath.Text, out normalData);
                    Option option = new Option("Normal", 0);
                    option.Files.Add(normalFacePath, normalFacePath.Replace("/", @"\"));
                    Directory.CreateDirectory(Path.GetDirectoryName(normalFaceDiskPath));
                    group2.Options.Add(option);
                    File.WriteAllBytes(normalFaceDiskPath, normalData);
                }
                if (!string.IsNullOrEmpty(multiF.FilePath.Text)) {
                    byte[] multiData = new byte[0];
                    TextureImporter.PngToTex(multiF.FilePath.Text, out multiData);
                    Option option = new Option("Multi", 0);
                    option.Files.Add(multiFacePath, multiFacePath.Replace("/", @"\"));
                    Directory.CreateDirectory(Path.GetDirectoryName(multiFaceDiskPath));
                    group2.Options.Add(option);
                    File.WriteAllBytes(multiFaceDiskPath, multiData);
                }
                ExportJson();
                ExportMeta();
                int i = 0;
                if (group.Options.Count > 0) {
                    string groupPath = Path.Combine(modPath, $"group_" + i++ + $"_{group.Name.ToLower()}.json");
                    ExportGroup(groupPath, group);
                }
                if (group2.Options.Count > 0) {
                    string groupPath2 = Path.Combine(modPath, $"group_" + i + $"_{group2.Name.ToLower()}.json");
                    ExportGroup(groupPath2, group2);
                }
                MessageBox.Show("Export succeeded!");
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
            MessageBox.Show("Please configure where your penumbra mods folder is, we will remember it for all future exports. This should be where you have penumbra set to use mods.\r\n\r\nNote:\r\nAVOID MANUALLY CREATING ANY NEW FOLDERS IN YOUR PENUMBRA FOLDER, ONLY SELECT THE BASE FOLDER!", Text);
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
                        result = @"chara/human/c" + (genderListBody.SelectedIndex == 0 ? raceCodeBody.Masculine[raceList.SelectedIndex] : raceCodeBody.Feminine[raceList.SelectedIndex]) + @"/obj/body/b" + unique + @"/texture/--c" + raceCodeBody.Feminine[raceList.SelectedIndex] + "b" + unique + GetTextureType(material) + ".tex";
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
                            MessageBox.Show("Bibo+ is only compatible with feminine characters");
                        }
                    } else {
                        MessageBox.Show("Bibo+ is not compatible with lalafells");
                    }
                    break;
                case 2:
                    // Eve
                    if (raceList.SelectedIndex != 5) {
                        if (genderListBody.SelectedIndex == 1) {
                            result = @"chara/human/c" + (genderListBody.SelectedIndex == 0 ? raceCodeBody.Masculine[raceList.SelectedIndex] : raceCodeBody.Feminine[raceList.SelectedIndex]) + @"/obj/body/b" + "0001" + @"/texture/eve2" + bodyIdentifiers[baseBodyList.SelectedIndex].RaceIdentifiers[raceList.SelectedIndex] + GetTextureType(material) + ".tex";
                        } else {
                            result = "";
                            MessageBox.Show("Eve is only compatible with feminine characters");
                        }
                    } else {
                        MessageBox.Show("Eve is not compatible with lalafells");
                    }
                    break;
                case 3:
                    // Gen3 and T&F3
                    if (raceList.SelectedIndex != 5) {
                        if (genderListBody.SelectedIndex == 1) {
                            result = @"chara/human/c" + (genderListBody.SelectedIndex == 0 ? raceCodeBody.Masculine[raceList.SelectedIndex] : raceCodeBody.Feminine[raceList.SelectedIndex]) + @"/obj/body/b" + unique + @"/texture/tfgen3" + bodyIdentifiers[baseBodyList.SelectedIndex].RaceIdentifiers[raceList.SelectedIndex] + "f" + GetTextureType(material) + ".tex";
                        } else {
                            result = "";
                            MessageBox.Show("Gen3 and T&F3 are only compatible with feminine characters");
                        }
                    } else {
                        MessageBox.Show("Gen3 and T&F3 are not compatible with lalafells");
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
                                MessageBox.Show("Scales+ is only compatible with feminine Au Ra characters");
                            }
                        } else {
                            MessageBox.Show("Scales+ is only compatible with feminine Au Ra characters");
                        }
                    } else {
                        MessageBox.Show("Scales+ is not compatible with lalafells");
                    }
                    break;
                case 5:
                    if (raceList.SelectedIndex != 5) {
                        if (genderListBody.SelectedIndex == 0) {
                            // TBSE and HRBODY
                            result = @"chara/human/c" + (genderListBody.SelectedIndex == 0 ? raceCodeBody.Masculine[raceList.SelectedIndex] : raceCodeBody.Feminine[raceList.SelectedIndex]) + @"/obj/body/b" + "0001" + @"/texture/--c" + raceCodeBody.Masculine[raceList.SelectedIndex] + "b0001_b" + GetTextureType(material) + ".tex";
                        } else {
                            result = "";
                            MessageBox.Show("TBSE and HRBODY are only compatible with masculine characters");
                        }
                    } else {
                        MessageBox.Show("TBSE and HRBODY are not compatible with lalafells");
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
            string helionCheck = subRaceList.SelectedIndex == 12 ? "000" : "010";
            string subRace = (genderListBody.SelectedIndex == 0 ? raceCodeFace.Masculine[subRaceList.SelectedIndex] : raceCodeFace.Feminine[subRaceList.SelectedIndex]);
            return "chara/human/c" + subRace + "/obj/face/f" + helionCheck + (faceType.SelectedIndex + 1) + "/texture/--c" + subRace + "f" + helionCheck + (faceType.SelectedIndex + 1) + GetFacePart(facePart.SelectedIndex) + GetTextureType(material) + ".tex";
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
                    MessageBox.Show("Tail is only compatible with Xaela, and Raen");
                }
            } else if (baseBodyList.SelectedIndex == 4) {
                if (raceList.SelectedIndex != 6 && raceList.SelectedIndex != 7) {
                    raceList.SelectedIndex = 6;
                    MessageBox.Show("Scales+ is only compatible with Xaela, and Raen");
                }
            }
            if (baseBodyList.SelectedIndex > 0) {
                if (raceList.SelectedIndex == 5) {
                    raceList.SelectedIndex = lastRaceIndex;
                    MessageBox.Show("Lalafels are not compatible with the selected body");
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
    }
}