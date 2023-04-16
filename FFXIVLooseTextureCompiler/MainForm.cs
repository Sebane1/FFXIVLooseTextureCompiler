#region Libraries
using Anamnesis.Penumbra;
using FFXIVLooseTextureCompiler.Export;
using FFXIVLooseTextureCompiler.ImageProcessing;
using FFXIVLooseTextureCompiler.Networking;
using FFXIVLooseTextureCompiler.PathOrganization;
using FFXIVLooseTextureCompiler.Racial;
using FFXIVVoicePackCreator;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Drawing.Imaging;
using TypingConnector;
#endregion
namespace FFXIVLooseTextureCompiler {
    public partial class MainWindow : Form {
        #region Variables
        private int lastRaceIndex;
        private string? penumbraModPath;
        private string modPath;
        private string jsonFilepath;
        private string metaFilePath;
        private string _defaultAuthor = "FFXIV Loose Texture Compiler";
        private readonly string _defaultDescription = "Exported by FFXIV Loose Texture Compiler";
        private string _defaultWebsite = "https://github.com/Sebane1/FFXIVLooseTextureCompiler";
        private string savePath;
        private bool hasSaved;
        private Dictionary<string, FileSystemWatcher> watchers = new Dictionary<string, FileSystemWatcher>();
        private bool lockDuplicateGeneration;
        private bool finalizeResults;
        private TextureProcessor textureProcessor;
        private Color originalDiffuseBoxColour;
        private Color originalNormalBoxColour;
        private Color originalMultiBoxColour;
        private bool isNetworkSync;
        private NetworkedClient networkedClient;
        private ConnectionDisplay connectionDisplay;
        private List<TextureSet> textureSets;
        private int choiceTypeIndex;
        private bool bakeNormalsChecked;
        private bool generatingMulti;
        private int generationProgress;
        private bool hasDoneReload;
        private bool willCloseWhenComplete;
        Stopwatch stopwatch = new Stopwatch();

        public bool HasSaved {
            get => hasSaved; set {
                hasSaved = value;
                if (!hasSaved) {
                    Text = Application.ProductName + " " + Application.ProductVersion +
                        (!string.IsNullOrWhiteSpace(savePath) ? $" ({savePath})*" : "*");
                } else {
                    Text = Application.ProductName + " " + Application.ProductVersion +
                        (!string.IsNullOrWhiteSpace(savePath) ? $" ({savePath})" : "");
                }
            }
        }
        public string VersionText { get; private set; }
        #endregion
        #region Main Window Events
        public MainWindow() {
            AutoScaleDimensions = new SizeF(96, 96);
            InitializeComponent();
            GetAuthorWebsite();
            GetAuthorName();
            GetPenumbraPath();
            Text += " " + Application.ProductVersion;
            textureProcessor = new TextureProcessor();
            textureProcessor.OnProgressChange += TextureProcessor_OnProgressChange;
            textureProcessor.OnLaunchedXnormal += TextureProcessor_OnLaunchedXnormal;
            textureProcessor.OnStartedProcessing += TextureProcessor_OnStartedProcessing;
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
                        networkedClient?.Dispose();
                        break;
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;
                }
            } else {
                networkedClient?.Dispose();
            }
        }
        #endregion
        #region Generation
        private void autoGenerateTImer_Tick(object sender, EventArgs e) {
            generateButton_Click(this, EventArgs.Empty);
            autoGenerateTImer.Stop();
        }

        private void finalizeButton_Click(object sender, EventArgs e) {
            finalizeResults = true;
            generateButton_Click(sender, e);
        }
        private void generationCooldown_Tick(object sender, EventArgs e) {
            generationCooldown.Stop();
            finalizeButton.Enabled = generateButton.Enabled = true;
            generateButton.Text = "Generate";
        }
        private void TextureProcessor_OnStartedProcessing(object? sender, EventArgs e) {
            StartedProcessing();
        }

        private void TextureProcessor_OnLaunchedXnormal(object? sender, EventArgs e) {
            LaunchingXnormal();
        }

        private void TextureProcessor_OnProgressChange(object? sender, EventArgs e) {
            processGeneration.ReportProgress(generationProgress++);
        }

        public void LaunchingXnormal() {
            if (generateButton.InvokeRequired) {
                Action safeWrite = delegate { LaunchingXnormal(); };
                generateButton.Invoke(safeWrite);
            } else {
                exportLabel.Text = "Wait For xNormal";
                Console.WriteLine(exportLabel.Text);
            }
        }
        public void StartedProcessing() {
            if (generateButton.InvokeRequired) {
                Action safeWrite = delegate { StartedProcessing(); };
                generateButton.Invoke(safeWrite);
            } else {
                exportLabel.Text = "Exporting";
                Console.WriteLine(exportLabel.Text);
            }
        }

        private void processGeneration_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e) {
            ExportJson();
            ExportMeta();
            if (hasDoneReload) {
                PenumbraHttpApi.Redraw(0);
            } else {
                PenumbraHttpApi.Reload(modPath, modNameTextBox.Text);
                PenumbraHttpApi.Redraw(0);
                if (IntegrityChecker.IntegrityCheck() && !willCloseWhenComplete) {
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
            if (!isNetworkSync) {
                exportPanel.Visible = false;
                finalizeResults = false;
            }
            stopwatch.Stop();
            if (willCloseWhenComplete) {
                hasSaved = true;
                Close();
            } else if (finalizeResults) {
                MessageBox.Show("Export completed in " + stopwatch.Elapsed + "!");
            }
            stopwatch.Reset();
        }
        private void processGeneration_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e) {
            textureProcessor.Export(textureSets, modPath, choiceTypeIndex,
                bakeNormalsChecked, generatingMulti, finalizeResults);
            processGeneration.CancelAsync();
        }
        private void processGeneration_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e) {
            exportProgress.Value = Math.Clamp(e.ProgressPercentage, 0, exportProgress.Maximum);
            Console.WriteLine(exportProgress.Value + "% Complete");
        }
        private void Form1_Load(object sender, EventArgs e) {
            VersionText = Application.ProductName + " " + Application.ProductVersion;
            RacePaths.VersionText = VersionText;
            AutoScaleDimensions = new SizeF(96, 96);
            diffuse.FilePath.Enabled = false;
            normal.FilePath.Enabled = false;
            multi.FilePath.Enabled = false;
            mask.FilePath.Enabled = false;
            glow.FilePath.Enabled = false;
            for (int i = 0; i < 300; i++) {
                faceExtra.Items.Add((i + 1) + "");
            }
            uniqueAuRa.Enabled = false;
            auraFaceScalesDropdown.SelectedIndex = baseBodyList.SelectedIndex = genderListBody.SelectedIndex =
                 raceList.SelectedIndex = tailList.SelectedIndex =
                 subRaceList.SelectedIndex = faceType.SelectedIndex = facePart.SelectedIndex =
                 faceExtra.SelectedIndex = generationType.SelectedIndex = 0;
            CleanDirectory();
            CheckForCommandArguments();
            GetLastUsedOptions();
            if (IntegrityChecker.IntegrityCheck()) {
                IntegrityChecker.ShowRules();
            }
            originalDiffuseBoxColour = diffuse.BackColor;
            originalNormalBoxColour = normal.BackColor;
            originalMultiBoxColour = multi.BackColor;
            GetLastIP();
            LoadTemplates();
        }
        private void generateButton_Click(object sender, EventArgs e) {
            exportLabel.Text = "Exporting";
            if (!lockDuplicateGeneration && !generationCooldown.Enabled) {
                if (!string.IsNullOrWhiteSpace(modNameTextBox.Text)) {
                    if (string.IsNullOrEmpty(penumbraModPath)) {
                        ConfigurePenumbraModFolder();
                    }
                    if (!string.IsNullOrEmpty(penumbraModPath)) {
                        generationProgress = 0;
                        lockDuplicateGeneration = true;
                        exportPanel.Visible = true;
                        exportPanel.BringToFront();
                        exportProgress.BringToFront();
                        exportProgress.Maximum = textureList.Items.Count * 3;
                        exportProgress.Visible = true;
                        modPath = Path.Combine(penumbraModPath, modNameTextBox.Text);
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
                        textureSets = new List<TextureSet>();
                        foreach (TextureSet item in textureList.Items) {
                            if (item.OmniExportMode) {
                                UniversalTextureSetCreator.ConfigureOmniConfiguration(item);
                                exportProgress.Maximum += (item.ChildSets.Count * 3);
                            }
                            textureSets.Add(item);
                        }
                        choiceTypeIndex = generationType.SelectedIndex;
                        bakeNormalsChecked = bakeNormals.Checked;
                        generatingMulti = generateMultiCheckBox.Checked;
                        stopwatch.Start();
                        processGeneration.RunWorkerAsync();
                    } else {
                        MessageBox.Show("No root penumbra path has been set!");
                    }
                } else {
                    lockDuplicateGeneration = false;
                    finalizeResults = false;
                    MessageBox.Show("Please enter a mod name!");
                }
            }
        }
        #endregion
        #region Path Generation
        private void findAndBulkReplaceToolStripMenuItem_Click(object sender, EventArgs e) {
            FindAndReplace findAndReplace = new FindAndReplace();
            findAndReplace.TextureSets.AddRange(textureList.Items.Cast<TextureSet>().ToArray());
            if (findAndReplace.ShowDialog() == DialogResult.OK) {
                foreach (TextureSet textureSet in textureList.Items) {
                    AddWatcher(textureSet.Diffuse);
                    AddWatcher(textureSet.Normal);
                    AddWatcher(textureSet.Multi);
                    AddWatcher(textureSet.NormalMask);
                    AddWatcher(textureSet.Glow);
                }
                textureList.SelectedIndex = -1;
                MessageBox.Show("Replacement succeeded.", VersionText);
            }
        }

        private void bulkReplaceToolStripMenuItem_Click(object sender, EventArgs e) {
            FindAndReplace findAndReplace = new FindAndReplace();
            TextureSet sourceTextureSet = (textureList.Items[textureList.SelectedIndex] as TextureSet);
            Tokenizer tokenizer = new Tokenizer(sourceTextureSet.MaterialSetName);
            findAndReplace.ReplacementString.Text = tokenizer.GetToken();
            findAndReplace.ReplacementGroup.Text = sourceTextureSet.MaterialGroupName != sourceTextureSet.MaterialSetName ? sourceTextureSet.MaterialGroupName : "";
            findAndReplace.Diffuse.CurrentPath = diffuse.CurrentPath;
            findAndReplace.Normal.CurrentPath = normal.CurrentPath;
            findAndReplace.Multi.CurrentPath = multi.CurrentPath;
            findAndReplace.Mask.CurrentPath = mask.CurrentPath;
            findAndReplace.Glow.CurrentPath = glow.CurrentPath;

            findAndReplace.TextureSets.AddRange(textureList.Items.Cast<TextureSet>().ToArray());
            if (findAndReplace.ShowDialog() == DialogResult.OK) {
                foreach (TextureSet textureSet in textureList.Items) {
                    AddWatcher(textureSet.Diffuse);
                    AddWatcher(textureSet.Normal);
                    AddWatcher(textureSet.Multi);
                    AddWatcher(textureSet.NormalMask);
                    AddWatcher(textureSet.Glow);
                }
                textureList.SelectedIndex = -1;
                MessageBox.Show("Replacement succeeded.", VersionText);
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
        private void subRaceList_SelectedIndexChanged(object sender, EventArgs e) {
            if (subRaceList.SelectedIndex == 10 || subRaceList.SelectedIndex == 11) {
                auraFaceScalesDropdown.Enabled = true;
            } else {
                auraFaceScalesDropdown.Enabled = false;
            }
            switch (subRaceList.SelectedIndex) {
                case 0:
                    raceList.SelectedIndex = 0;
                    break;
                case 1:
                    raceList.SelectedIndex = 1;
                    break;
                case 2:
                case 3:
                    raceList.SelectedIndex = 2;
                    break;
                case 4:
                case 5:
                    raceList.SelectedIndex = 3;
                    break;
                case 6:
                case 7:
                    raceList.SelectedIndex = 4;
                    break;
                case 8:
                case 9:
                    raceList.SelectedIndex = 5;
                    break;
                case 10:
                    raceList.SelectedIndex = 6;
                    break;
                case 11:
                    raceList.SelectedIndex = 7;
                    break;
                case 12:
                case 13:
                    raceList.SelectedIndex = 8;
                    break;
                case 14:
                case 15:
                    raceList.SelectedIndex = 9;
                    break;

            }
        }

        #endregion
        #region State Persistence
        private void ffxivRefreshTimer_Tick(object sender, EventArgs e) {
            WriteLastUsedOptions();
        }
        private void modAuthorTextBox_Leave(object sender, EventArgs e) {
            WriteAuthorName(modAuthorTextBox.Text);
        }

        private void modWebsiteTextBox_Leave(object sender, EventArgs e) {
            WriteAuthorWebsite(modWebsiteTextBox.Text);
        }
        private void LoadTemplates() {
            string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"res\templates");
            Directory.CreateDirectory(templatePath);
            foreach (string file in Directory.GetFiles(templatePath)) {
                if (file.ToLower().EndsWith(".ffxivtp")) {
                    ToolStripMenuItem menuItem = new ToolStripMenuItem();
                    menuItem.Text = Path.GetFileNameWithoutExtension(file);
                    menuItem.Click += (s, e) => {
                        string value = file;
                        OpenTemplate(value);
                    };
                    templatesToolStripMenuItem.DropDownItems.Add(menuItem);
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

        public void GetLastIP() {
            string dataPath = Application.UserAppDataPath.Replace(Application.ProductVersion, null);
            string path = Path.Combine(dataPath, @"IPConfig.config");
            if (File.Exists(path)) {
                using (StreamReader reader = new StreamReader(path)) {
                    ipBox.Text = reader.ReadLine();
                }
            }
        }
        public void WriteLastIP(string path) {
            string dataPath = Application.UserAppDataPath.Replace(Application.ProductVersion, null);
            using (StreamWriter writer = new StreamWriter(Path.Combine(dataPath, @"IPConfig.config"))) {
                writer.WriteLine(path);
            }
        }

        public void GetLastUsedOptions() {
            string dataPath = Application.UserAppDataPath.Replace(Application.ProductVersion, null);
            string path = Path.Combine(dataPath, @"UsedOptions.config");
            if (File.Exists(path)) {
                using (StreamReader reader = new StreamReader(path)) {
                    genderListBody.SelectedIndex = Math.Clamp(int.Parse(reader.ReadLine()), 0, int.MaxValue); ;
                    raceList.SelectedIndex = Math.Clamp(int.Parse(reader.ReadLine()), 0, int.MaxValue);
                    subRaceList.SelectedIndex = Math.Clamp(int.Parse(reader.ReadLine()), 0, int.MaxValue);
                    int value = Math.Clamp(int.Parse(reader.ReadLine()), 0, int.MaxValue);
                    if (value < 8) {
                        baseBodyList.SelectedIndex = value;
                    } else {
                        MessageBox.Show("Previously selected body type is not valid");
                    }
                }
            }
        }
        public void WriteLastUsedOptions() {
            string dataPath = Application.UserAppDataPath.Replace(Application.ProductVersion, null);
            try {
                using (StreamWriter writer = new StreamWriter(Path.Combine(dataPath, @"UsedOptions.config"))) {
                    writer.WriteLine(genderListBody.SelectedIndex);
                    writer.WriteLine(raceList.SelectedIndex);
                    writer.WriteLine(subRaceList.SelectedIndex);
                    writer.WriteLine(baseBodyList.SelectedIndex);
                }
            } catch {

            }
        }
        private void changePenumbraPathToolStripMenuItem_Click(object sender, EventArgs e) {
            ConfigurePenumbraModFolder();
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
        #endregion
        #region Path Management
        private void facePart_SelectedIndexChanged(object sender, EventArgs e) {
            if (facePart.SelectedIndex == 4) {
                auraFaceScalesDropdown.Enabled = asymCheckbox.Enabled = faceType.Enabled = subRaceList.Enabled = false;
                faceExtra.Enabled = true;
            } else if (facePart.SelectedIndex == 5) {
                auraFaceScalesDropdown.Enabled = asymCheckbox.Enabled = faceType.Enabled;
                faceExtra.Enabled = true;
            } else {
                asymCheckbox.Enabled = faceType.Enabled = subRaceList.Enabled = true;
                if (subRaceList.SelectedIndex == 10 || subRaceList.SelectedIndex == 11) {
                    auraFaceScalesDropdown.Enabled = true;
                }
                faceExtra.Enabled = false;
            }
        }
        private void baseBodyList_SelectedIndexChanged(object sender, EventArgs e) {
            switch (baseBodyList.SelectedIndex) {
                case 0:
                    genderListBody.Enabled = true;
                    tailList.Enabled = false;
                    uniqueAuRa.Enabled = false;
                    break;
                case 1:
                case 2:
                case 3:
                    genderListBody.SelectedIndex = 1;
                    genderListBody.Enabled = false;
                    tailList.Enabled = false;
                    raceList.SelectedIndex = 0;
                    uniqueAuRa.Enabled = false;
                    break;
                case 4:
                    raceList.SelectedIndex = 6;
                    genderListBody.SelectedIndex = 1;
                    genderListBody.Enabled = false;
                    tailList.Enabled = false;
                    uniqueAuRa.Enabled = false;
                    break;
                case 5:
                    genderListBody.SelectedIndex = 0;
                    genderListBody.Enabled = false;
                    tailList.Enabled = false;
                    raceList.SelectedIndex = 0;
                    uniqueAuRa.Enabled = true;
                    break;
                case 6:
                    raceList.SelectedIndex = 6;
                    genderListBody.Enabled = true;
                    tailList.Enabled = true;
                    uniqueAuRa.Enabled = false;
                    break;
                case 7:
                case 8:
                case 9:
                    genderListBody.Enabled = false;
                    raceList.SelectedIndex = 5;
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
            } else if (baseBodyList.SelectedIndex > 0 && baseBodyList.SelectedIndex < 7) {
                if (raceList.SelectedIndex == 5) {
                    raceList.SelectedIndex = lastRaceIndex;
                    MessageBox.Show("Lalafells are not compatible with the selected body", VersionText);
                }
            } else if (baseBodyList.SelectedIndex > 6) {
                if (raceList.SelectedIndex != 5) {
                    raceList.SelectedIndex = 5;
                    MessageBox.Show("Only Lalafells are compatible with the selected body", VersionText);
                }
            }
            lastRaceIndex = raceList.SelectedIndex;
        }
        private void omniExportModeToolStripMenuItem_Click(object sender, EventArgs e) {
            TextureSet textureSet = (textureList.Items[textureList.SelectedIndex] as TextureSet);
            if (textureSet != null) {
                if (!textureSet.OmniExportMode) {
                    UniversalTextureSetCreator.ConfigureOmniConfiguration(textureSet);
                    MessageBox.Show("Enabling universal compatibility mode allows your currently selected body or face textures to be compatible with other body/face configurations on a best effort basis.\r\n\r\nWarning: this slows down the generation process, so you will need to click the finalize button to update changes on bodies that arent this one.", VersionText);
                } else {
                    textureSet.OmniExportMode = false;
                    textureSet.ChildSets.Clear();
                }
            }
        }

        private void addBodyEditButton_Click(object sender, EventArgs e) {
            hasDoneReload = false;
            TextureSet textureSet = new TextureSet();
            textureSet.MaterialSetName = baseBodyList.Text + (baseBodyList.Text.ToLower().Contains("tail") ? " " +
                (tailList.SelectedIndex + 1) : "") + ", " + (raceList.SelectedIndex == 5 ? "Unisex" : genderListBody.Text)
                + ", " + raceList.Text;
            if (raceList.SelectedIndex != 3 || baseBodyList.SelectedIndex != 6) {
                textureSet.InternalDiffusePath = RacePaths.GetBodyTexturePath(0, genderListBody.SelectedIndex,
                    baseBodyList.SelectedIndex, raceList.SelectedIndex, tailList.SelectedIndex, uniqueAuRa.Checked);
            }
            textureSet.InternalNormalPath = RacePaths.GetBodyTexturePath(1, genderListBody.SelectedIndex,
                    baseBodyList.SelectedIndex, raceList.SelectedIndex, tailList.SelectedIndex, uniqueAuRa.Checked);

            textureSet.InternalMultiPath = RacePaths.GetBodyTexturePath(2, genderListBody.SelectedIndex,
                    baseBodyList.SelectedIndex, raceList.SelectedIndex, tailList.SelectedIndex, uniqueAuRa.Checked);
            AddBackupPaths(textureSet);
            textureList.Items.Add(textureSet);
            HasSaved = false;
        }

        private void addFaceButton_Click(object sender, EventArgs e) {
            hasDoneReload = false;
            TextureSet textureSet = new TextureSet();
            textureSet.MaterialSetName = facePart.Text + (facePart.SelectedIndex == 4 ? " "
                + (faceExtra.SelectedIndex + 1) : "") + ", " + (facePart.SelectedIndex != 4 ? genderListBody.Text : "Unisex")
                + ", " + (facePart.SelectedIndex != 4 ? subRaceList.Text : "Multi Race") + ", "
                + (facePart.SelectedIndex != 4 ? faceType.Text : "Multi Face");
            switch (facePart.SelectedIndex) {
                default:
                    if (facePart.SelectedIndex != 1) {
                        textureSet.InternalDiffusePath = RacePaths.GetFaceTexturePath(0, genderListBody.SelectedIndex, subRaceList.SelectedIndex,
                            facePart.SelectedIndex, faceType.SelectedIndex, auraFaceScalesDropdown.SelectedIndex, asymCheckbox.Checked);
                    }

                    textureSet.InternalNormalPath = RacePaths.GetFaceTexturePath(1, genderListBody.SelectedIndex, subRaceList.SelectedIndex,
                    facePart.SelectedIndex, faceType.SelectedIndex, auraFaceScalesDropdown.SelectedIndex, asymCheckbox.Checked);

                    textureSet.InternalMultiPath = RacePaths.GetFaceTexturePath(2, genderListBody.SelectedIndex, subRaceList.SelectedIndex,
                    facePart.SelectedIndex, faceType.SelectedIndex, auraFaceScalesDropdown.SelectedIndex, asymCheckbox.Checked);

                    if (facePart.SelectedIndex == 0) {
                        if (subRaceList.SelectedIndex == 10 || subRaceList.SelectedIndex == 11) {
                            if (auraFaceScalesDropdown.SelectedIndex > 0) {
                                if (faceType.SelectedIndex < 4) {
                                    if (asymCheckbox.Checked) {
                                        textureSet.NormalCorrection = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                              @"res\textures\s" + (genderListBody.SelectedIndex == 0 ? "m" : "f") + faceType.SelectedIndex + "a.png");
                                    } else {
                                        textureSet.NormalCorrection = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                            @"res\textures\s" + (genderListBody.SelectedIndex == 0 ? "m" : "f") + faceType.SelectedIndex + ".png");
                                    }
                                }
                            }
                        }
                    }
                    break;
                case 2:
                    textureSet.InternalDiffusePath = RacePaths.GetFaceTexturePath(1, genderListBody.SelectedIndex, subRaceList.SelectedIndex,
                    facePart.SelectedIndex, faceType.SelectedIndex, auraFaceScalesDropdown.SelectedIndex, asymCheckbox.Checked);

                    textureSet.InternalNormalPath = RacePaths.GetFaceTexturePath(2, genderListBody.SelectedIndex, subRaceList.SelectedIndex,
                    facePart.SelectedIndex, faceType.SelectedIndex, auraFaceScalesDropdown.SelectedIndex, asymCheckbox.Checked);

                    textureSet.InternalMultiPath = RacePaths.GetFaceTexturePath(3, genderListBody.SelectedIndex, subRaceList.SelectedIndex,
                    facePart.SelectedIndex, faceType.SelectedIndex, auraFaceScalesDropdown.SelectedIndex, asymCheckbox.Checked);
                    break;
                case 4:
                    textureSet.InternalDiffusePath = "chara/common/texture/decal_face/_decal_" + (faceExtra.SelectedIndex + 1) + ".tex";
                    break;
                case 5:
                    textureSet.MaterialSetName = facePart.Text + " " + (faceExtra.SelectedIndex + 1) + ", " + genderListBody.Text
                        + ", " + raceList.Text;
                    textureSet.InternalNormalPath = RacePaths.GetHairTexturePath(1, faceExtra.SelectedIndex, genderListBody.SelectedIndex, raceList.SelectedIndex, subRaceList.SelectedIndex);
                    textureSet.InternalMultiPath = RacePaths.GetHairTexturePath(2, faceExtra.SelectedIndex, genderListBody.SelectedIndex, raceList.SelectedIndex, subRaceList.SelectedIndex);
                    break;

            }
            textureSet.IgnoreMultiGeneration = true;
            textureList.Items.Add(textureSet);
            HasSaved = false;
        }

        private void materialList_SelectedIndexChanged(object sender, EventArgs e) {
            if (textureList.SelectedIndex == -1) {
                currentEditLabel.Text = "Please select a texture set to start importing";
                SetControlsEnabled(false);
            } else {
                TextureSet materialSet = textureList.Items[textureList.SelectedIndex] as TextureSet;
                currentEditLabel.Text = "Editing: " + materialSet.MaterialSetName;
                SetControlsEnabled(true, materialSet);
                SetControlsPaths(materialSet);
                SetControlsColors(materialSet);
            }
        }

        private void SetControlsEnabled(bool enabled, TextureSet textureSet = null) {
            if (textureSet == null) {
                enabled = false;
            }
            diffuse.Enabled = enabled;
            normal.Enabled = enabled;
            multi.Enabled = enabled;
            mask.Enabled = enabled && bakeNormals.Checked;
            glow.Enabled = enabled && !textureSet.MaterialSetName.ToLower().Contains("face paint")
                    && !textureSet.MaterialSetName.ToLower().Contains("hair") && diffuse.Enabled;
        }

        private void SetControlsPaths(TextureSet materialSet) {
            diffuse.CurrentPath = materialSet.Diffuse;
            normal.CurrentPath = materialSet.Normal;
            multi.CurrentPath = materialSet.Multi;
            mask.CurrentPath = materialSet.NormalMask;
            glow.CurrentPath = materialSet.Glow;
        }

        private void SetControlsColors(TextureSet materialSet) {
            if (materialSet.MaterialSetName.ToLower().Contains("eyes")) {
                diffuse.LabelName.Text = "Normal";
                normal.LabelName.Text = "Multi";
                multi.LabelName.Text = "Catchlight";
                diffuse.BackColor = originalNormalBoxColour;
                normal.BackColor = Color.Lavender;
                multi.BackColor = Color.LightGray;
            } else {
                diffuse.LabelName.Text = "Diffuse";
                normal.LabelName.Text = "Normal";
                multi.LabelName.Text = "Multi";
                diffuse.BackColor = originalDiffuseBoxColour;
                normal.BackColor = originalNormalBoxColour;
                multi.BackColor = originalMultiBoxColour;
            }
        }
        public void SetPaths() {
            if (textureList.SelectedIndex != -1) {
                TextureSet textureSet = (textureList.Items[textureList.SelectedIndex] as TextureSet);
                string directoryDiffuse = Path.GetDirectoryName(textureSet.Diffuse);
                if (!string.IsNullOrWhiteSpace(directoryDiffuse)) {
                    if (watchers.ContainsKey(directoryDiffuse)) {
                        if (textureSet.Diffuse != diffuse.CurrentPath) {
                            watchers[directoryDiffuse].Dispose();
                            watchers.Remove(directoryDiffuse);
                        }
                    }
                }
                string directoryNormal = Path.GetDirectoryName(textureSet.Normal);
                if (!string.IsNullOrWhiteSpace(directoryNormal)) {
                    if (watchers.ContainsKey(directoryNormal)) {
                        if (textureSet.Normal != normal.CurrentPath) {
                            watchers[directoryNormal].Dispose();
                            watchers.Remove(directoryNormal);
                        }
                    }
                }
                string directoryMulti = Path.GetDirectoryName(textureSet.Multi);
                if (!string.IsNullOrWhiteSpace(directoryMulti)) {
                    if (watchers.ContainsKey(directoryMulti)) {
                        if (textureSet.Multi != multi.CurrentPath) {
                            watchers[directoryMulti].Dispose();
                            watchers.Remove(directoryMulti);
                        }
                    }
                }
                string directoryMask = Path.GetDirectoryName(textureSet.NormalMask);
                if (!string.IsNullOrWhiteSpace(directoryMask)) {
                    if (watchers.ContainsKey(directoryMask)) {
                        if (textureSet.NormalMask != mask.CurrentPath) {
                            watchers[directoryMask].Dispose();
                            watchers.Remove(directoryMask);
                        }
                    }
                }
                string directoryGlow = Path.GetDirectoryName(textureSet.Glow);
                if (!string.IsNullOrWhiteSpace(directoryGlow)) {
                    if (watchers.ContainsKey(directoryGlow)) {
                        if (textureSet.Glow != glow.CurrentPath) {
                            watchers[directoryGlow].Dispose();
                            watchers.Remove(directoryGlow);
                        }
                    }
                }
                textureSet.Diffuse = diffuse.CurrentPath;
                textureSet.Normal = normal.CurrentPath;
                textureSet.Multi = multi.CurrentPath;
                textureSet.NormalMask = mask.CurrentPath;
                textureSet.Glow = glow.CurrentPath;

                AddWatcher(textureSet.Diffuse);
                AddWatcher(textureSet.Normal);
                AddWatcher(textureSet.Multi);
                AddWatcher(textureSet.NormalMask);
                AddWatcher(textureSet.Glow);
            }
        }
        public void AddWatcher(string path) {
            string directory = Path.GetDirectoryName(path);
            if (Directory.Exists(directory) && !string.IsNullOrWhiteSpace(path)) {
                FileSystemWatcher fileSystemWatcher = watchers.ContainsKey(path) ? watchers[path] : new FileSystemWatcher();
                fileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite;
                fileSystemWatcher.Changed += delegate (object sender, FileSystemEventArgs e) {
                    if (e.Name.Contains(Path.GetFileName(path)) && !string.IsNullOrEmpty(modNameTextBox.Text)) {
                        StartGeneration();
                    }
                    return;
                };
                fileSystemWatcher.Path = directory;
                fileSystemWatcher.EnableRaisingEvents = !string.IsNullOrEmpty(path);
                watchers[path] = fileSystemWatcher;
            }
        }
        private void multi_Leave(object sender, EventArgs e) {
            SetPaths();
        }

        private void multi_Enter(object sender, EventArgs e) {

        }

        private void removeSelectionButton_Click(object sender, EventArgs e) {
            hasDoneReload = false;
            if (textureList.SelectedIndex > -1) {
                textureList.Items.RemoveAt(textureList.SelectedIndex);
                diffuse.CurrentPath = "";
                normal.CurrentPath = "";
                multi.CurrentPath = "";
                glow.CurrentPath = "";
                currentEditLabel.Text = "Please select a texture set to start importing";
            }
        }

        private void clearList_Click(object sender, EventArgs e) {
            hasDoneReload = false;
            if (MessageBox.Show("This will irriversably remove everything from the list, including any changes. Are you sure?",
                VersionText, MessageBoxButtons.YesNo) == DialogResult.Yes) {
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
                omniExportModeToolStripMenuItem.Text = (textureList.SelectedItem as TextureSet).OmniExportMode
                    ? "Disable Universal Compatibility" : "Enable Universal Compatibility";
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
        #endregion
        #region File Management
        private void modDescriptionTextBox_TextChanged(object sender, EventArgs e) {
            HasSaved = false;
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
                        }
                        break;
                    case DialogResult.No:
                        break;
                    default:
                        return false;
                }
            }
            NewProject();
            return true;
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

                foreach (TextureSet textureSet in projectFile.MaterialSets) {
                    AddWatcher(textureSet.Diffuse);
                    AddWatcher(textureSet.Normal);
                    AddWatcher(textureSet.Multi);
                    AddWatcher(textureSet.NormalMask);
                    AddWatcher(textureSet.Glow);
                    AddBackupPaths(textureSet);
                }
            }
            HasSaved = true;
        }

        public void OpenTemplate(string path) {
            using (StreamReader file = File.OpenText(path)) {
                JsonSerializer serializer = new JsonSerializer();
                ProjectFile projectFile = (ProjectFile)serializer.Deserialize(file, typeof(ProjectFile));
                TemplateConfiguration templateConfiguration = new TemplateConfiguration();
                if (templateConfiguration.ShowDialog() == DialogResult.OK) {
                    BringToFront();
                    foreach (TextureSet textureSet in projectFile.MaterialSets) {
                        textureSet.MaterialGroupName = templateConfiguration.GroupName;
                        AddWatcher(textureSet.Diffuse);
                        AddWatcher(textureSet.Normal);
                        AddWatcher(textureSet.Multi);
                        AddWatcher(textureSet.NormalMask);
                        AddWatcher(textureSet.Glow);
                        AddBackupPaths(textureSet);
                    }
                    textureList.Items.AddRange(projectFile.MaterialSets?.ToArray());
                }
            }
            HasSaved = false;
        }

        private void AddBackupPaths(TextureSet textureSet) {
            if (genderListBody.SelectedIndex != 0) {
                if (textureSet.InternalDiffusePath.Contains("bibo")) {
                    textureSet.BackupTexturePaths = BackupTexturePaths.BiboPath;
                } else if (textureSet.InternalDiffusePath.Contains("gen3") || textureSet.InternalDiffusePath.Contains("eve")) {
                    textureSet.BackupTexturePaths = BackupTexturePaths.Gen3Path;
                } else if (textureSet.InternalDiffusePath.Contains("v01_c1101b0001_g")) {
                    textureSet.BackupTexturePaths = BackupTexturePaths.OtopopLalaPath;
                } else {
                    textureSet.BackupTexturePaths = raceList.SelectedIndex == 5 ? BackupTexturePaths.VanillaLalaPath : BackupTexturePaths.Gen3Gen2Path;
                }
            } else {
                if (raceList.SelectedIndex == 5) {
                    textureSet.BackupTexturePaths = BackupTexturePaths.VanillaLalaPath;
                } else {
                    // Midlander, Elezen, Miqo'te
                    if (textureSet.InternalDiffusePath.Contains("--c0101b0001_b_d")) {
                        textureSet.BackupTexturePaths = BackupTexturePaths.TbsePath;
                    } else
                    // Highlander
                    if (textureSet.InternalDiffusePath.Contains("--c0301b0001_b_d")) {
                        textureSet.BackupTexturePaths = BackupTexturePaths.TbsePathHighlander;
                    } else
                    // Viera
                    if (textureSet.InternalDiffusePath.Contains("--c1701b0001_b_d")) {
                        textureSet.BackupTexturePaths = BackupTexturePaths.TbsePathViera;
                    } else if (textureSet.InternalDiffusePath.Contains("_b_d")) {
                        textureSet.BackupTexturePaths = BackupTexturePaths.TbsePath;
                    }
                }
            }
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

                        if (args.Length > 2) {
                            if (args[2].Contains("-process")) {
                                willCloseWhenComplete = true;
                                finalizeButton_Click(this, EventArgs.Empty);
                            }
                        }
                    }
                }
            }
        }
        #endregion
        #region Sub Programs
        private void diffuseMergerToolStripMenuItem_Click(object sender, EventArgs e) {
            new DiffuseMerger().Show();
        }
        private void bulkTexViewerToolStripMenuItem_Click(object sender, EventArgs e) {
            new BulkTexManager().Show();
        }
        #endregion
        #region Project Config
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
        #endregion
        #region Json Export
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
  ""Author"": """ + (!string.IsNullOrEmpty(modAuthorTextBox.Text) ? modAuthorTextBox.Text :
    "FFXIV Loose Texture Compiler") + @""",
  ""Description"": """ + (!string.IsNullOrEmpty(modDescriptionTextBox.Text) ? modDescriptionTextBox.Text :
    "Exported by FFXIV Loose Texture Compiler") + @""",
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
        #endregion
        #region Standalone XNormal Conversion
        private void xNormalToolStripMenuItem_Click(object sender, EventArgs e) {
            XNormal.OpenXNormal();
        }
        private void biboToGen3ToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            openFileDialog.Filter = "Texture File|*.png;*.dds;*.bmp;**.tex;";
            saveFileDialog.Filter = "Texture File|*.png;";
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
            saveFileDialog.Filter = "Texture File|*.png;";
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
            saveFileDialog.Filter = "Texture File|*.png;";
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
            saveFileDialog.Filter = "Texture File|*.png;";
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
            saveFileDialog.Filter = "Texture File|*.png;";
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
            saveFileDialog.Filter = "Texture File|*.png;";
            MessageBox.Show("Please select input texture");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                MessageBox.Show("Please select where you want to save the conversion");
                if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                    XNormal.BiboToGen2(openFileDialog.FileName, saveFileDialog.FileName);
                }
            }
        }


        private void otopopToVanillaToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            openFileDialog.Filter = "Texture File|*.png;*.dds;*.bmp;**.tex;";
            saveFileDialog.Filter = "Texture File|*.png;";
            MessageBox.Show("Please select input texture");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                MessageBox.Show("Please select where you want to save the conversion");
                if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                    XNormal.OtopopToVanillaLala(openFileDialog.FileName, saveFileDialog.FileName);
                }
            }
        }


        private void vanillaToOtopopToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            openFileDialog.Filter = "Texture File|*.png;*.dds;*.bmp;**.tex;";
            saveFileDialog.Filter = "Texture File|*.png;";
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
            saveFileDialog.Filter = "Texture File|*.png;";
            MessageBox.Show("Please select input texture");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                MessageBox.Show("Please select where you want to save the conversion");
                if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                    XNormal.VanillaLalaToRedefinedLala(openFileDialog.FileName, saveFileDialog.FileName);
                }
            }
        }

        private void extractAtramentumLuminisGlowMapToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            openFileDialog.Filter = "Texture File|*.png;*.dds;*.bmp;**.tex;";
            saveFileDialog.Filter = "Texture File|*.png;";
            MessageBox.Show("Please select input texture");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                MessageBox.Show("Please select where you want to save the conversion", VersionText);
                if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                    AtramentumLuminisGlow.ExtractGlowMapFormLegacyDiffuse(
                        TexLoader.ResolveBitmap(openFileDialog.FileName)).Save(saveFileDialog.FileName, ImageFormat.Png);
                }
            }
        }
        #endregion
        #region Image Conversion Utilities
        private void imageToRGBChannelsToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            openFileDialog.Filter = "Texture File|*.png;*.dds;*.bmp;**.tex;";
            MessageBox.Show("Please select input texture");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                Bitmap image = TexLoader.ResolveBitmap(openFileDialog.FileName);
                ImageManipulation.ExtractRed(image).Save(openFileDialog.FileName.Replace(".", "_R."));
                ImageManipulation.ExtractGreen(image).Save(openFileDialog.FileName.Replace(".", "_G."));
                ImageManipulation.ExtractBlue(image).Save(openFileDialog.FileName.Replace(".", "_B."));
                ImageManipulation.ExtractAlpha(image).Save(openFileDialog.FileName.Replace(".", "_A."));
                MessageBox.Show("Image successfully split into seperate channels", VersionText);
            }
        }

        private void multiCreatorToolStripMenuItem_Click(object sender, EventArgs e) {
            new MultiCreator().Show();
        }

        private void splitImageToRGBAndAlphaToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            openFileDialog.Filter = "Texture File|*.png;*.dds;*.bmp;**.tex;";
            MessageBox.Show("Please select input texture");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                Bitmap image = TexLoader.ResolveBitmap(openFileDialog.FileName);
                ImageManipulation.ExtractTransparency(image).Save(openFileDialog.FileName.Replace(".", "_RGB."));
                ImageManipulation.ExtractAlpha(image).Save(openFileDialog.FileName.Replace(".", "_Alpha."));
                MessageBox.Show("Image successfully split into RGB and Alpha", VersionText);
            }
        }

        private void mergeRGBAndAlphaImagesToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialogRGB = new OpenFileDialog();
            OpenFileDialog openFileDialogAlpha = new OpenFileDialog();
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            openFileDialogRGB.Filter = "Texture File|*.png;*.dds;*.bmp;**.tex;";
            openFileDialogAlpha.Filter = "Texture File|*.png;*.dds;*.bmp;**.tex;";
            saveFileDialog.Filter = "Texture File|*.png;";
            MessageBox.Show("Please select RGB texture");
            if (openFileDialogRGB.ShowDialog() == DialogResult.OK) {
                MessageBox.Show("Please select alpha texture");
                if (openFileDialogAlpha.ShowDialog() == DialogResult.OK) {
                    MessageBox.Show("Please select where you want to save the conversion", VersionText);
                    if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                        ImageManipulation.MergeAlphaToRGB(TexLoader.ResolveBitmap(openFileDialogAlpha.FileName),
                            TexLoader.ResolveBitmap(openFileDialogRGB.FileName)).Save(saveFileDialog.FileName);
                    }
                }
            }
        }

        private void bulkConvertImagesToLTCTToolStripMenuItem_Click(object sender, EventArgs e) {
            FolderBrowserDialog openFileDialog = new FolderBrowserDialog();
            MessageBox.Show("Please select input folder");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                TexLoader.ConvertToLtct(openFileDialog.SelectedPath);
            }
        }

        private void bulkConvertLTCTToPNGToolStripMenuItem_Click(object sender, EventArgs e) {
            FolderBrowserDialog openFileDialog = new FolderBrowserDialog();
            MessageBox.Show("Please select input folder");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                TexLoader.ConvertLtctToPng(openFileDialog.SelectedPath);
            }
        }

        private void optimizePNGToolStripMenuItem_Click(object sender, EventArgs e) {
            FolderBrowserDialog openFileDialog = new FolderBrowserDialog();
            MessageBox.Show("Please select input folder");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                TexLoader.RunOptiPNG(openFileDialog.SelectedPath);
            }
        }

        private void convertPNGToLTCTToolStripMenuItem_Click(object sender, EventArgs e) {
            FolderBrowserDialog openFileDialog = new FolderBrowserDialog();
            MessageBox.Show("Please select input folder");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                TexLoader.ConvertPngToLtct(openFileDialog.SelectedPath);
            }
        }
        #endregion
        #region Mod Share
        private void enableModshareToolStripMenuItem_Click(object sender, EventArgs e) {
            if (MessageBox.Show("By enabling this feature you understand that we hold no responsibility for what data may be sent to you by other users. Only use this feature with people you trust.",
                VersionText, MessageBoxButtons.OKCancel) == DialogResult.OK) {
                if (networkedClient == null) {
                    networkedClient = new NetworkedClient((ipBox.Text.Contains("0.0.0.0")
                        || string.IsNullOrEmpty(ipBox.Text)) ? "50.70.229.19" : ipBox.Text);
                }
                networkedClient.Start();
                if (networkedClient.Connected) {
                    enableModshareToolStripMenuItem.Enabled = false;
                    sendCurrentModToolStripMenuItem.Enabled = true;

                    if (!listenForFiles.IsBusy) {
                        if (connectionDisplay == null) {
                            connectionDisplay = new ConnectionDisplay(networkedClient.Id);
                            connectionDisplay.RequestedToSendCurrentMod += delegate {
                                sendCurrentModToolStripMenuItem_Click(sender, e);
                            };
                        }
                        listenForFiles.RunWorkerAsync();
                        connectionDisplay.Show();
                    }
                }
            }
        }

        private void sendCurrentModToolStripMenuItem_Click(object sender, EventArgs e) {
            if (!string.IsNullOrEmpty(modNameTextBox.Text)) {
                isNetworkSync = true;
                generateButton_Click(this, EventArgs.Empty);
                exportPanel.Visible = true;
                exportLabel.Text = "Sending Over Network";
                exportProgress.Visible = true;
                Application.DoEvents();
                Refresh();
                networkedClient.SendModFolder(connectionDisplay.SendId, modNameTextBox.Text, penumbraModPath);
                exportProgress.Value = exportProgress.Maximum;
                exportPanel.Visible = false;
                exportProgress.Visible = false;
                exportLabel.Text = "Send Attempt Finished";
                isNetworkSync = false;
                if (!networkedClient.Connected) {
                    enableModshareToolStripMenuItem.Enabled = true;
                    sendCurrentModToolStripMenuItem.Enabled = false;
                    MessageBox.Show("Sending Mod failed!", VersionText);
                } else {
                    MessageBox.Show("Mod files sent!", VersionText);
                }
            } else {
                MessageBox.Show("No mod is loaded to send", VersionText);
            }
        }

        private void listenForFilesToolStripMenuItem_Click(object sender, EventArgs e) {
        }

        private void listenForFiles_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e) {
            networkedClient.ListenForFiles(penumbraModPath, connectionDisplay);
            if (!networkedClient.Connected) {
                enableModshareToolStripMenuItem.Enabled = true;
                sendCurrentModToolStripMenuItem.Enabled = false;
                listenForFiles.CancelAsync();
            }
        }

        private void ipBox_TextChanged(object sender, EventArgs e) {
            WriteLastIP(ipBox.Text);
        }

        private void ipBox_KeyUp(object sender, KeyEventArgs e) {
            WriteLastIP(ipBox.Text);
        }
        #endregion
        #region Hotkeys
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
        #endregion
        #region Links
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
        #endregion
        #region Help
        private void creditsToolStripMenuItem_Click(object sender, EventArgs e) {
            MessageBox.Show("Credits for the body textures used in this tool:\r\n\r\nThe creators of Bibo+\r\nThe creators of Tight&Firm (Gen3)\r\nThe creators of TBSE\r\nThe creator of Otopop.\r\n\r\nTake care to read the terms and permissions for each body type when releasing public mods.", VersionText);
        }
        private void howToGetTexturesToolStripMenuItem_Click(object sender, EventArgs e) {
            new HelpWindow().Show();
        }
        private void importCustomTemplateToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "FFXIV Texture Project|*.ffxivtp;";
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                OpenTemplate(openFileDialog.FileName);
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

        private void bulkImageToTexToolStripMenuItem_Click(object sender, EventArgs e) {
            FolderBrowserDialog openFileDialog = new FolderBrowserDialog();
            MessageBox.Show("Please select folder");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                foreach (string file in Directory.GetFiles(openFileDialog.SelectedPath)) {
                    if (FilePicker.CheckExtentions(file)) {
                        textureProcessor.ExportTex(file, file.Replace(".png", ".tex").Replace(".dds", ".tex").Replace(".bmp", ".tex"));
                    }
                }
            }
        }

        private void whatIsModshareAndCanIQuicklySendAModToSomebodyElseToolStripMenuItem_Click(object sender, EventArgs e) {
            try {
                Process.Start(new System.Diagnostics.ProcessStartInfo() {
                    FileName = "https://docs.google.com/document/d/1Mwg_qe9UvY5J4FwsQ8eP-Q1GuuoFfzgFMlXpwk0N_Bg/edit?usp=sharing",
                    UseShellExecute = true,
                    Verb = "OPEN"
                });
            } catch {

            }
        }

        private void whatAreTemplatesAndHowDoIUseThemToolStripMenuItem_Click(object sender, EventArgs e) {
            try {
                Process.Start(new System.Diagnostics.ProcessStartInfo() {
                    FileName = "https://docs.google.com/document/d/10bBarOZ18DZjqRug4rISqoIlfQxCTmCkLRCE_QZH0do/edit?usp=sharing",
                    UseShellExecute = true,
                    Verb = "OPEN"
                });
            } catch {

            }
        }
        #endregion
    }
}