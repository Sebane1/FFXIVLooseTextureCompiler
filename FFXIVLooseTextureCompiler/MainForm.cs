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

        private MainFormSimplified mainFormSimplified;
        private bool isSimpleMode = true;

        public string VersionText { get; private set; }
        public bool LockDuplicateGeneration { get => lockDuplicateGeneration; set => lockDuplicateGeneration = value; }
        public bool IsSimpleMode { get => isSimpleMode; set => isSimpleMode = value; }
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
        #region Export
        private void autoGenerateTImer_Tick(object sender, EventArgs e) {
            generateButton_Click(this, EventArgs.Empty);
            autoGenerateTImer.Stop();
        }

        public void finalizeButton_Click(object sender, EventArgs e) {
            finalizeResults = true;
            generateButton_Click(sender, e);
        }
        private void generationCooldown_Tick(object sender, EventArgs e) {
            generationCooldown.Stop();
            finalizeButton.Enabled = generateButton.Enabled = true;
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
            if (isNetworkSync) {
                SendModOverNetwork();
            }
        }

        private void SendModOverNetwork() {
            exportPanel.Visible = true;
            exportLabel.Text = "Sending Over Network";
            exportProgress.Visible = true;
            Refresh();
            Application.DoEvents();
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
        private void MainForm_Load(object sender, EventArgs e) {
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
            GetLastUsedOptions();
            if (IntegrityChecker.IntegrityCheck()) {
                IntegrityChecker.ShowRules();
            }
            originalDiffuseBoxColour = diffuse.BackColor;
            originalNormalBoxColour = normal.BackColor;
            originalMultiBoxColour = multi.BackColor;
            GetLastIP();
            LoadTemplates();
            mainFormSimplified = new MainFormSimplified();
            mainFormSimplified.MainWindow = this;
            GetDefaultMode();
            CheckForCommandArguments();
        }
        public void generateButton_Click(object sender, EventArgs e) {
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
            findAndReplace.IsForEyes = sourceTextureSet.InternalMultiPath.ToLower().Contains("catchlight");

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
            raceList.SelectedIndex = RaceInfo.SubRaceToMainRace(subRaceList.SelectedIndex);
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
                    isSimpleMode = false;
                }
            } else {
                path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "XIVLauncher\\pluginConfigs\\Penumbra.json");
                if (File.Exists(path)) {
                    using (StreamReader reader = new StreamReader(path)) {
                        for (int i = 0; i < 7; i++) {
                            penumbraModPath = reader.ReadLine()
                                .Replace("\"ModDirectory\": ", null)
                                .Replace(",", null)
                                .Replace(@"""", null)
                                .Replace(@"\\", @"\").Trim();
                        }
                    }
                }
            }
        }
        public void WritePenumbraPath(string path) {
            string dataPath = Application.UserAppDataPath.Replace(Application.ProductVersion, null);
            using (StreamWriter writer = new StreamWriter(Path.Combine(dataPath, @"PenumbraPath.config"))) {
                writer.WriteLine(path);
            }
        }
        public void GetDefaultMode() {
            string dataPath = Application.UserAppDataPath.Replace(Application.ProductVersion, null);
            string path = Path.Combine(dataPath, @"DefaultMode.config");
            if (File.Exists(path)) {
                using (StreamReader reader = new StreamReader(path)) {
                    isSimpleMode = bool.Parse(reader.ReadLine());
                }
            }
            if (isSimpleMode) {
                Hide();
                mainFormSimplified.Show();
            }
        }
        public void WriteDefaultMode() {
            string dataPath = Application.UserAppDataPath.Replace(Application.ProductVersion, null);
            using (StreamWriter writer = new StreamWriter(Path.Combine(dataPath, @"DefaultMode.config"))) {
                writer.WriteLine(isSimpleMode);
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
        public void changePenumbraPathToolStripMenuItem_Click(object sender, EventArgs e) {
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
                    if (raceList.SelectedIndex == 5) {
                        raceList.SelectedIndex = 0;
                    }
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
                    if (raceList.SelectedIndex == 5) {
                        raceList.SelectedIndex = 0;
                    }
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
            AddBodyPaths(textureSet);
            textureList.Items.Add(textureSet);
            HasSaved = false;
        }

        public void AddBodyPaths(TextureSet textureSet) {
            if (raceList.SelectedIndex != 3 || baseBodyList.SelectedIndex != 6) {
                textureSet.InternalDiffusePath = RacePaths.GetBodyTexturePath(0, genderListBody.SelectedIndex,
                    baseBodyList.SelectedIndex, raceList.SelectedIndex, tailList.SelectedIndex, uniqueAuRa.Checked);
            }
            textureSet.InternalNormalPath = RacePaths.GetBodyTexturePath(1, genderListBody.SelectedIndex,
                    baseBodyList.SelectedIndex, raceList.SelectedIndex, tailList.SelectedIndex, uniqueAuRa.Checked);

            textureSet.InternalMultiPath = RacePaths.GetBodyTexturePath(2, genderListBody.SelectedIndex,
                    baseBodyList.SelectedIndex, raceList.SelectedIndex, tailList.SelectedIndex, uniqueAuRa.Checked);
            AddBackupPaths(textureSet);
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
                    AddFacePaths(textureSet);
                    break;
                case 2:
                    AddEyePaths(textureSet);
                    break;
                case 4:
                    AddDecalPath(textureSet);
                    break;
                case 5:
                    AddHairPaths(textureSet);
                    break;

            }
            textureSet.IgnoreMultiGeneration = true;
            textureList.Items.Add(textureSet);
            HasSaved = false;
        }

        public void AddDecalPath(TextureSet textureSet) {
            textureSet.InternalDiffusePath = "chara/common/texture/decal_face/_decal_" + (faceExtra.SelectedIndex + 1) + ".tex";
        }

        public void AddHairPaths(TextureSet textureSet) {
            textureSet.MaterialSetName = facePart.Text + " " + (faceExtra.SelectedIndex + 1) + ", " + genderListBody.Text
      + ", " + raceList.Text;

            textureSet.InternalNormalPath = RacePaths.GetHairTexturePath(1, faceExtra.SelectedIndex,
                genderListBody.SelectedIndex, raceList.SelectedIndex, subRaceList.SelectedIndex);

            textureSet.InternalMultiPath = RacePaths.GetHairTexturePath(2, faceExtra.SelectedIndex,
                genderListBody.SelectedIndex, raceList.SelectedIndex, subRaceList.SelectedIndex);
        }

        public void AddEyePaths(TextureSet textureSet) {
            textureSet.InternalDiffusePath = RacePaths.GetFaceTexturePath(1, genderListBody.SelectedIndex, subRaceList.SelectedIndex,
            2, faceType.SelectedIndex, auraFaceScalesDropdown.SelectedIndex, asymCheckbox.Checked);

            textureSet.InternalNormalPath = RacePaths.GetFaceTexturePath(2, genderListBody.SelectedIndex, subRaceList.SelectedIndex,
            2, faceType.SelectedIndex, auraFaceScalesDropdown.SelectedIndex, asymCheckbox.Checked);

            textureSet.InternalMultiPath = RacePaths.GetFaceTexturePath(3, genderListBody.SelectedIndex, subRaceList.SelectedIndex,
            2, faceType.SelectedIndex, auraFaceScalesDropdown.SelectedIndex, asymCheckbox.Checked);
        }

        public void AddFacePaths(TextureSet textureSet) {
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
            if (materialSet.InternalMultiPath.ToLower().Contains("catchlight")) {
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
                DisposeWatcher(textureSet.Diffuse, diffuse);
                DisposeWatcher(textureSet.Normal, normal);
                DisposeWatcher(textureSet.Multi, multi);
                DisposeWatcher(textureSet.NormalMask, mask);
                DisposeWatcher(textureSet.Glow, glow);
                if (!string.IsNullOrWhiteSpace(textureSet.Glow)) {
                    generateMultiCheckBox.Checked = true;
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
        public void DisposeWatcher(string path, FilePicker filePicker) {
            if (!string.IsNullOrWhiteSpace(path)) {
                if (watchers.ContainsKey(path)) {
                    if (path != filePicker.CurrentPath) {
                        watchers[path].Dispose();
                        watchers.Remove(path);
                    }
                }
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
        public void RefreshList() {
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
        public void newToolStripMenuItem_Click(object sender, EventArgs e) {
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
            mainFormSimplified.ClearForm();
            if (IsSimpleMode) {
                mainFormSimplified.RefreshValues();
            }
        }
        public void saveToolStripMenuItem_Click(object sender, EventArgs e) {
            Save();
            if (IntegrityChecker.IntegrityCheck()) {
                IntegrityChecker.ShowSave();
            }
        }
        public void saveAsToolStripMenuItem_Click(object sender, EventArgs e) {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "FFXIV Texture Project|*.ffxivtp;";
            if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                savePath = saveFileDialog.FileName;
                SaveProject(savePath);
            }
        }
        public void openToolStripMenuItem_Click(object sender, EventArgs e) {
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
        public void Save() {
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
                textureList.Items.Clear();
                modNameTextBox.Text = projectFile.Name;
                modAuthorTextBox.Text = projectFile.Author;
                modVersionTextBox.Text = projectFile.Version;
                modDescriptionTextBox.Text = projectFile.Description;
                modWebsiteTextBox.Text = projectFile.Website;
                generationType.SelectedIndex = projectFile.ExportType;
                bakeNormals.Checked = projectFile.BakeMissingNormals;
                generateMultiCheckBox.Checked = projectFile.GenerateMulti;
                textureList.Items.AddRange(projectFile.MaterialSets?.ToArray());
                if (projectFile.SimpleMode) {
                    if (isSimpleMode) {
                        mainFormSimplified.BodyType.SelectedIndex = projectFile.SimpleBodyType;
                        mainFormSimplified.SubRace.SelectedIndex = projectFile.SimpleSubRaceType;
                        mainFormSimplified.FaceType.SelectedIndex = projectFile.SimpleFaceType;
                        if (!mainFormSimplified.Visible) {
                            mainFormSimplified.Show();
                        }
                        mainFormSimplified.ClearForm();
                        mainFormSimplified.RefreshValues();
                        mainFormSimplified.NormalGeneration.SelectedIndex = projectFile.SimpleNormalGeneration;
                        if (Visible) {
                            this.Hide();
                        }
                    }
                } else {
                    if (mainFormSimplified.Visible) {
                        mainFormSimplified.Hide();
                    }
                    if (!Visible) {
                        this.Show();
                    }
                }

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
                        if (!templateConfiguration.GroupName.Contains("Default")) {
                            textureSet.MaterialGroupName = templateConfiguration.GroupName;
                        }
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
                    textureSet.BackupTexturePaths = raceList.SelectedIndex == 5 ? 
                    BackupTexturePaths.VanillaLalaPath : BackupTexturePaths.Gen3Gen2Path;
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
                projectFile.SimpleMode = isSimpleMode;
                projectFile.SimpleBodyType = mainFormSimplified.BodyType.SelectedIndex;
                projectFile.SimpleFaceType = mainFormSimplified.FaceType.SelectedIndex;
                projectFile.SimpleSubRaceType = mainFormSimplified.SubRace.SelectedIndex;
                projectFile.SimpleNormalGeneration = mainFormSimplified.NormalGeneration.SelectedIndex;
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
                        Hide();
                        mainFormSimplified.Hide();
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
            openFileDialog.Filter = "Texture File|*.png;*.dds;*.bmp;";
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
            openFileDialog.Filter = "Texture File|*.png;*.dds;*.bmp;";
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
            openFileDialog.Filter = "Texture File|*.png;*.dds;*.bmp;";
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
            openFileDialog.Filter = "Texture File|*.png;*.dds;*.bmp;";
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
            openFileDialog.Filter = "Texture File|*.png;*.dds;*.bmp;";
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
            openFileDialog.Filter = "Texture File|*.png;*.dds;*.bmp;";
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
            openFileDialog.Filter = "Texture File|*.png;*.dds;*.bmp;";
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
            openFileDialog.Filter = "Texture File|*.png;*.dds;*.bmp;";
            saveFileDialog.Filter = "Texture File|*.png;";
            MessageBox.Show("Please select input texture");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                MessageBox.Show("Please select where you want to save the conversion");
                if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                    XNormal.VanillaLalaToOtopop(openFileDialog.FileName, saveFileDialog.FileName);
                }
            }
        }

        private void vanillaToAsymLalaToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            openFileDialog.Filter = "Texture File|*.png;*.dds;*.bmp;";
            saveFileDialog.Filter = "Texture File|*.png;";
            MessageBox.Show("Please select input texture");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                MessageBox.Show("Please select where you want to save the conversion");
                if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                    XNormal.VanillaLalaToAsymLala(openFileDialog.FileName, saveFileDialog.FileName);
                }
            }
        }

        private void otopopToAsymLalaToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            openFileDialog.Filter = "Texture File|*.png;*.dds;*.bmp;";
            saveFileDialog.Filter = "Texture File|*.png;";
            MessageBox.Show("Please select input texture");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                MessageBox.Show("Please select where you want to save the conversion");
                if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                    XNormal.OtopopToAsymLala(openFileDialog.FileName, saveFileDialog.FileName);
                }
            }
        }

        private void asymLalaToOtopopToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            openFileDialog.Filter = "Texture File|*.png;*.dds;*.bmp;";
            saveFileDialog.Filter = "Texture File|*.png;";
            MessageBox.Show("Please select input texture");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                MessageBox.Show("Please select where you want to save the conversion");
                if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                    XNormal.AsymLalaToOtopop(openFileDialog.FileName, saveFileDialog.FileName);
                }
            }
        }

        private void extractAtramentumLuminisGlowMapToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            openFileDialog.Filter = "Texture File|*.png;*.dds;*.bmp;";
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

        private void bulkImageToTexToolStripMenuItem_Click(object sender, EventArgs e) {
            FolderBrowserDialog openFileDialog = new FolderBrowserDialog();
            MessageBox.Show("Please select folder");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                foreach (string file in Directory.EnumerateFiles(openFileDialog.SelectedPath)) {
                    if (FilePicker.CheckExtentions(file)) {
                        textureProcessor.ExportTex(file, file.Replace(".png", ".tex").Replace(".dds", ".tex").Replace(".bmp", ".tex"));
                        MessageBox.Show("The operation succeeded!", VersionText);
                    }
                }
            }
        }

        private void recursiveBulkImageToTexToolStripMenuItem_Click(object sender, EventArgs e) {
            FolderBrowserDialog openFileDialog = new FolderBrowserDialog();
            MessageBox.Show("Please select folder");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                RecursiveImageToText(openFileDialog.SelectedPath);
                MessageBox.Show("The operation succeeded!", VersionText);
            }
        }
        private void multiMapToGrayscaleToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            openFileDialog.Filter = "Texture File|*.png;*.dds;*.bmp;**.tex;";
            MessageBox.Show("Please select input texture");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                Bitmap image = TexLoader.ResolveBitmap(openFileDialog.FileName);
                ImageManipulation.ExtractRed(image).Save(openFileDialog.FileName.Replace(".", "_grayscale."));
                MessageBox.Show("Multi successfully converted to grayscale", VersionText);
            }
        }
        public void RecursiveImageToText(string filePath, int layer = 0, int maxLayer = int.MaxValue) {
            foreach (string file in Directory.EnumerateFiles(filePath)) {
                if (FilePicker.CheckExtentions(file)) {
                    textureProcessor.ExportTex(file, file.Replace(".png", ".tex").Replace(".dds", ".tex").Replace(".bmp", ".tex"));
                }
            }
            if (layer < maxLayer) {
                foreach (string directory in Directory.EnumerateDirectories(filePath)) {
                    RecursiveImageToText(directory, layer + 1, maxLayer);
                }
            }
        }
        private void convertImageToEyeMultiToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Texture File|*.png;*.dds;*.bmp;**.tex;";
            MessageBox.Show("Please select input texture");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                ImageManipulation.ConvertToEyeMaps(openFileDialog.FileName);
                MessageBox.Show("Image successfully converted to eye maps", VersionText);
                try {
                    Process.Start(new System.Diagnostics.ProcessStartInfo() {
                        FileName = Path.GetDirectoryName(openFileDialog.FileName),
                        UseShellExecute = true,
                        Verb = "OPEN"
                    });
                } catch {

                }
            }
        }

        private void convertImagesToAsymEyeMapsToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Texture File|*.png;*.dds;*.bmp;**.tex;";
            OpenFileDialog openFileDialog2 = new OpenFileDialog();
            openFileDialog2.Filter = "Texture File|*.png;*.dds;*.bmp;**.tex;";
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Texture File|*.png;";

            MessageBox.Show("Please select left input texture (left side of image, not left side of face)");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                MessageBox.Show("Please select right input texture (right side of image, not right side of face)");
                if (openFileDialog2.ShowDialog() == DialogResult.OK) {
                    MessageBox.Show("Please pick a file name for the merged result");
                    if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                        ImageManipulation.ConvertToAsymEyeMaps(openFileDialog.FileName, openFileDialog2.FileName, saveFileDialog.FileName);
                        MessageBox.Show("Image successfully converted to asym eye multi", VersionText);
                        try {
                            Process.Start(new System.Diagnostics.ProcessStartInfo() {
                                FileName = Path.GetDirectoryName(saveFileDialog.FileName),
                                UseShellExecute = true,
                                Verb = "OPEN"
                            });
                        } catch {

                        }
                    }
                }
            }
        }

        private void convertFolderToEyeMapsToolStripMenuItem_Click(object sender, EventArgs e) {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK) {
                foreach (string file in Directory.EnumerateFiles(folderBrowserDialog.SelectedPath, "*.*", SearchOption.AllDirectories)
                .Where(s => s.EndsWith(".png") || s.EndsWith(".bmp") || s.EndsWith(".dds") || s.EndsWith(".tex"))) {
                    ImageManipulation.ConvertToEyeMaps(file);
                }
                MessageBox.Show("Images successfully converted to eye maps", VersionText);
                try {
                    Process.Start(new System.Diagnostics.ProcessStartInfo() {
                        FileName = Path.GetDirectoryName(folderBrowserDialog.SelectedPath),
                        UseShellExecute = true,
                        Verb = "OPEN"
                    });
                } catch {

                }
            }
        }
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
        private void generateXNormalTranslationMapToolStripMenuItem_Click(object sender, EventArgs e) {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Texture File|*.png;";
            MessageBox.Show("Please select where you want to save the result", VersionText);
            if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                ImageManipulation.GenerateXNormalTranslationMap().Save(saveFileDialog.FileName, ImageFormat.Png);
            }
        }
        #endregion
        #region Mod Share
        public void enableModshareToolStripMenuItem_Click(object sender, EventArgs e) {
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
                if (Directory.Exists(Path.Combine(penumbraModPath, modNameTextBox.Text))) {
                    SendModOverNetwork();
                } else {
                    isNetworkSync = true;
                    generateButton_Click(this, EventArgs.Empty);
                }
            } else {
                MessageBox.Show("No mod is loaded to send", VersionText);
            }
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
        public void donateButton_Click(object sender, EventArgs e) {
            try {
                Process.Start(new System.Diagnostics.ProcessStartInfo() {
                    FileName = "https://ko-fi.com/sebastina",
                    UseShellExecute = true,
                    Verb = "OPEN"
                });
            } catch {

            }
        }

        public void discordButton_Click(object sender, EventArgs e) {
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
        public void creditsToolStripMenuItem_Click(object sender, EventArgs e) {
            MessageBox.Show("Credits for the body textures used in this tool:\r\n\r\nThe creators of Bibo+\r\nThe creators of Tight&Firm (Gen3)\r\nThe creators of TBSE\r\nThe creator of Otopop.\r\n\r\nTake care to read the terms and permissions for each body type when releasing public mods.\r\n\r\nSpecial thanks to Zatori for all their help with testing, and all of you for using the tool!", VersionText);
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
        private void thisToolIsTooHardMakeItSimplerToolStripMenuItem_Click(object sender, EventArgs e) {
            if (textureList.Items.Count == 0 || textureList.Items.Count == 3) {
                isSimpleMode = true;
                this.Hide();
                mainFormSimplified.Show();
                WriteDefaultMode();
            } else {
                MessageBox.Show("This project is too complex for simple mode", VersionText);
            }
        }
        #endregion
    }
}