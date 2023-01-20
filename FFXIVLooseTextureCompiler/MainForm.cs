using FFXIVLooseTextureCompiler.DataTypes;
using FFXIVVoicePackCreator.Json;
using Newtonsoft.Json;
using Penumbra.Import.Dds;

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
            baseBodyList.SelectedIndex = genderList.SelectedIndex = raceList.SelectedIndex = tailList.SelectedIndex = 0;
            CleanDirectory();
        }

        private void generateButton_Click(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(penumbraModPath)) {
                ConfigurePenumbraModFolder();
            }
            if (!string.IsNullOrWhiteSpace(modNameTextBox.Text)) {
                string diffusePath = GetMaterialPath(0);
                string normalPath = GetMaterialPath(1);
                string multiPath = GetMaterialPath(2);
                string modPath = Path.Combine(penumbraModPath, modNameTextBox.Text);
                jsonFilepath = Path.Combine(modPath, "default_mod.json");
                metaFilePath = Path.Combine(modPath, "meta.json");
                string diffusePathDisk = Path.Combine(modPath, diffusePath.Replace("/", @"\"));
                string normalPathDisk = Path.Combine(modPath, normalPath.Replace("/", @"\"));
                string multiPathDisk = Path.Combine(modPath, multiPath.Replace("/", @"\"));
                Group group = new Group($"Texture File", "Imported Textures", 0, "Multi", 0);
                string groupPath = Path.Combine(modPath, $"group_0" + $"_{group.Name.ToLower()}.json");
                Directory.Delete(modPath, true);
                if (!string.IsNullOrEmpty(diffuse.FilePath.Text)) {
                    byte[] diffuseData = new byte[0];
                    TextureImporter.PngToTex(diffuse.FilePath.Text, out diffuseData);
                    Option option = new Option("Diffuse", 0);
                    option.Files.Add(diffusePath, diffusePath.Replace("/", @"\"));
                    Directory.CreateDirectory(Path.GetDirectoryName(diffusePathDisk));
                    group.Options.Add(option);
                    File.WriteAllBytes(diffusePathDisk, diffuseData);
                }
                if (!string.IsNullOrEmpty(normal.FilePath.Text)) {
                    byte[] normalData = new byte[0];
                    TextureImporter.PngToTex(normal.FilePath.Text, out normalData);
                    Option option = new Option("Normal", 0);
                    option.Files.Add(diffusePath, diffusePath.Replace("/", @"\"));
                    Directory.CreateDirectory(Path.GetDirectoryName(normalPathDisk));
                    group.Options.Add(option);
                    File.WriteAllBytes(normalPathDisk, normalData);
                }
                if (!string.IsNullOrEmpty(normal.FilePath.Text)) {
                    byte[] multiData = new byte[0];
                    TextureImporter.PngToTex(multi.FilePath.Text, out multiData);
                    Option option = new Option("Multi", 0);
                    option.Files.Add(multiPath, multiPath.Replace("/", @"\"));
                    Directory.CreateDirectory(Path.GetDirectoryName(multiPathDisk));
                    group.Options.Add(option);
                    File.WriteAllBytes(multiPathDisk, multiData);
                }
                ExportJson();
                ExportMeta();
                ExportGroup(groupPath, group);
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
        private string GetMaterialPath(int material) {
            string result = "";
            string uniqueAuRa = (raceList.SelectedIndex == 7 ? "0101" : "0001");
            switch (baseBodyList.SelectedIndex) {
                case 0:
                    // Vanila
                    if (material != 2) {
                        result = @"chara/human/c" + (genderList.SelectedIndex == 0 ? raceCodeBody.Masculine[raceList.SelectedIndex] : raceCodeBody.Feminine[raceList.SelectedIndex]) + @"/obj/body/b" + uniqueAuRa + @"/texture/--c" + raceCodeBody.Feminine[raceList.SelectedIndex] + "b" + uniqueAuRa + GetTextureType(material) + ".tex";
                    } else {
                        result = @"chara/common/texture/skin_m.tex";
                    }
                    break;
                case 1:
                    // Bibo+
                    if (raceList.SelectedIndex != 5) {
                        if (genderList.SelectedIndex == 1) {
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
                        if (genderList.SelectedIndex == 1) {
                            result = @"chara/human/c" + (genderList.SelectedIndex == 0 ? raceCodeBody.Masculine[raceList.SelectedIndex] : raceCodeBody.Feminine[raceList.SelectedIndex]) + @"/obj/body/b" + "0001" + @"/texture/eve2" + bodyIdentifiers[baseBodyList.SelectedIndex].RaceIdentifiers[raceList.SelectedIndex] + GetTextureType(material) + ".tex";
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
                        if (genderList.SelectedIndex == 1) {
                            result = @"chara/human/c" + (genderList.SelectedIndex == 0 ? raceCodeBody.Masculine[raceList.SelectedIndex] : raceCodeBody.Feminine[raceList.SelectedIndex]) + @"/obj/body/b" + uniqueAuRa + @"/texture/tfgen3" + bodyIdentifiers[baseBodyList.SelectedIndex].RaceIdentifiers[raceList.SelectedIndex] + "f" + GetTextureType(material) + ".tex";
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
                            if (genderList.SelectedIndex == 1) {
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
                        if (genderList.SelectedIndex == 0) {
                            // TBSE and HRBODY
                            result = @"chara/human/c" + (genderList.SelectedIndex == 0 ? raceCodeBody.Masculine[raceList.SelectedIndex] : raceCodeBody.Feminine[raceList.SelectedIndex]) + @"/obj/body/b" + "0001" + @"/texture/--c" + raceCodeBody.Masculine[raceList.SelectedIndex] + "b0001_b" + GetTextureType(material) + ".tex";
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
                    result = @"chara/human/c" + (genderList.SelectedIndex == 0 ? raceCodeBody.Masculine[raceList.SelectedIndex] : raceCodeBody.Feminine[raceList.SelectedIndex]) + @"/obj/tail/t" + xaelaCheck + @"/texture/--c" + raceCodeBody.Feminine[raceList.SelectedIndex] + "_etc_" + xaelaCheck + GetTextureType(material) + ".tex";
                    break;
            }
            return result;
        }

        public string GetTextureType(int material) {
            switch (material) {
                case 0:
                    return "_d";
                case 1:
                    return "_n";
                case 2:
                    if (baseBodyList.SelectedIndex == 1) {
                        return "_m";
                    } else {
                        return "_s";
                    }
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
                    genderList.Enabled = true;
                    tailList.Enabled = false;
                    break;
                case 1:
                    // Bibo+
                    genderList.SelectedIndex = 1;
                    genderList.Enabled = false;
                    tailList.Enabled = false;
                    if (raceList.SelectedIndex == 5) {
                        raceList.SelectedIndex = 0;
                    }
                    break;
                case 2:
                    // Eve
                    genderList.SelectedIndex = 1;
                    genderList.Enabled = false;
                    tailList.Enabled = false;
                    if (raceList.SelectedIndex == 5) {
                        raceList.SelectedIndex = 0;
                    }
                    break;
                case 3:
                    // Gen3 and T&F3
                    genderList.SelectedIndex = 1;
                    genderList.Enabled = false;
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
                    genderList.SelectedIndex = 1;
                    genderList.Enabled = false;
                    tailList.Enabled = false;
                    if (raceList.SelectedIndex == 5) {
                        raceList.SelectedIndex = 0;
                    }
                    break;
                case 5:
                    // TBSE and HRBODY
                    genderList.SelectedIndex = 0;
                    genderList.Enabled = false;
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
                    genderList.Enabled = true;
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
    }
}