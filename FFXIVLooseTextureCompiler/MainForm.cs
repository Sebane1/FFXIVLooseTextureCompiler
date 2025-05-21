#region Libraries
using Anamnesis.Penumbra;
using FFXIVLooseTextureCompiler.Configuration_Dialogues;
using FFXIVLooseTextureCompiler.Export;
using FFXIVLooseTextureCompiler.ImageProcessing;
using FFXIVLooseTextureCompiler.Networking;
using FFXIVLooseTextureCompiler.PathOrganization;
using FFXIVLooseTextureCompiler.Racial;
using FFXIVLooseTextureCompiler.Sub_Utilities;
using FFXIVVoicePackCreator;
using LooseTextureCompilerCore;
using LooseTextureCompilerCore.Json;
using LooseTextureCompilerCore.Racial;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Compression;
using System.Windows.Forms;
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
        private Color originalBaseBoxColour;
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
        private Dictionary<string, int> groupOptionTypes = new Dictionary<string, int>();
        Stopwatch stopwatch = new Stopwatch();

        public bool HasSaved {
            get => hasSaved; set {
                hasSaved = value;
                if (!hasSaved) {
                    Text = Application.ProductName + " " + Program.Version +
                        (!string.IsNullOrWhiteSpace(savePath) ? $" ({savePath})*" : "*");
                } else {
                    Text = Application.ProductName + " " + Program.Version +
                        (!string.IsNullOrWhiteSpace(savePath) ? $" ({savePath})" : "");
                }
            }
        }

        private MainFormSimplified mainFormSimplified;
        private bool isSimpleMode = true;
        private BulkTexManager _exportTexManager;
        private string _lastGroupName;

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
            Text += " " + Program.Version;
            textureProcessor = new TextureProcessor();
            textureProcessor.OnProgressChange += TextureProcessor_OnProgressChange;
            textureProcessor.OnLaunchedXnormal += TextureProcessor_OnLaunchedXnormal;
            textureProcessor.OnStartedProcessing += TextureProcessor_OnStartedProcessing;
            textureProcessor.OnError += TextureProcessor_OnError;
            Racial.RacePaths.otopopNoticeTriggered += RacePaths_otopopNoticeTriggered;
        }

        private void TextureProcessor_OnError(object? sender, string e) {
            MessageBox.Show(e);
        }

        private void RacePaths_otopopNoticeTriggered(object? sender, EventArgs e) {
            MessageBox.Show("By using Otopop you agree to the following:\r\n\r\nYou are not making a NSFW mod. \r\n\r\nCommercial mod releases require a commercial license from the Otopop creator.");
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e) {
            if (!HasSaved) {
                DialogResult dialogResult = MessageBox.Show("Save changes?", VersionText, MessageBoxButtons.YesNoCancel);
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
            try {
                processGeneration.ReportProgress(generationProgress++);
            } catch {

            }
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

        private async void processGeneration_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e) {
            ExportJson();
            ExportMeta();
            Thread.Sleep(100);
            await PenumbraHttpApi.Reload(modPath, modNameTextBox.Text);
            Thread.Sleep(100);
            await PenumbraHttpApi.Redraw(0);
            Thread.Sleep(100);
            await PenumbraHttpApi.Redraw(0);
            hasDoneReload = true;
            textureSetList_SelectedIndexChanged(this, EventArgs.Empty);
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
            string path = Path.Combine(modPath, @"do_not_edit\textures\");

            stopwatch.Reset();
            if (isNetworkSync) {
                SendModOverNetwork();
            }
            if (!isSimpleMode) {
                if (_exportTexManager == null || _exportTexManager.IsDisposed) {
                    _exportTexManager = new BulkTexManager();
                } else {
                    _exportTexManager.ClearList();
                }
                _exportTexManager.AddFilesRecursively(path, 0, 10);
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
            textureProcessor.Export(textureSets, groupOptionTypes, modPath, choiceTypeIndex,
                bakeNormalsChecked, generatingMulti, finalizeResults);
            processGeneration.CancelAsync();
        }
        private void processGeneration_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e) {
            exportProgress.Value = Math.Clamp(textureProcessor.ExportCompletion, 0, exportProgress.Maximum);
            Console.WriteLine(exportProgress.Value + "% Complete");
        }
        private void MainForm_Load(object sender, EventArgs e) {
            VersionText = Application.ProductName + " " + Program.Version;
            if (DateTime.Now < new DateTime(2024, 06, 28)) {
                MessageBox.Show("This version of Loose texture Compiler is designed to generate early texture previews for the Dawntrail Benchmark and test automated map conversions.\r\n\r\n" +
        "You will need a copy of CursedTools from the FFXIV Textools discords 'dev_room' to manually import the exports made by this tool into the benchmark.\r\n\r\n" +
        "Exports made by this version of Loose Texture Compiler are not compatible with Endwalker or any current version of Penumbra.", VersionText);
            }
            RacePaths.VersionText = VersionText;
            AutoScaleDimensions = new SizeF(96, 96);
            Base.FilePath.Enabled = false;
            normal.FilePath.Enabled = false;
            mask.FilePath.Enabled = false;
            bounds.FilePath.Enabled = false;
            glow.FilePath.Enabled = false;
            for (int i = 0; i < 300; i++) {
                faceExtraList.Items.Add((i + 1) + "");
            }
            uniqueAuRa.Enabled = false;
            auraFaceScalesDropdown.SelectedIndex = baseBodyList.SelectedIndex = genderList.SelectedIndex =
                 raceList.SelectedIndex = tailList.SelectedIndex =
                 subRaceList.SelectedIndex = faceTypeList.SelectedIndex = facePart.SelectedIndex =
                 faceExtraList.SelectedIndex = generationType.SelectedIndex = 0;
            CleanDirectory();
            GetLastUsedOptions();
            originalBaseBoxColour = Base.BackColor;
            originalNormalBoxColour = normal.BackColor;
            originalMultiBoxColour = mask.BackColor;
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
                        exportProgress.Maximum = textureList.Items.Count * 4;
                        exportProgress.Visible = true;
                        modPath = Path.Combine(penumbraModPath, modNameTextBox.Text);
                        jsonFilepath = Path.Combine(modPath, "default_mod.json");
                        metaFilePath = Path.Combine(modPath, "meta.json");
                        if (Directory.Exists(modPath)) {
                            try {
                                if (!File.Exists("project.ffxivtp")) {
                                    Directory.Delete(modPath, true);
                                } else {
                                    textureProcessor.CleanGeneratedAssets(modPath);
                                }
                            } catch {

                            }
                        } else {
                            hasDoneReload = false;
                        }
                        Directory.CreateDirectory(modPath);
                        SaveProject(Path.Combine(penumbraModPath, modNameTextBox.Text + ".ffxivtp"), true);
                        textureSets = new List<TextureSet>();
                        foreach (TextureSet item in textureList.Items) {
                            if (item.OmniExportMode) {
                                UniversalTextureSetCreator.ConfigureTextureSet(item);
                                exportProgress.Maximum += (item.ChildSets.Count * 4);
                            }
                            textureSets.Add(item);
                        }
                        choiceTypeIndex = generationType.SelectedIndex;
                        bakeNormalsChecked = bakeNormals.Checked;
                        generatingMulti = generatMaskCheckBox.Checked;
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
                    AddWatchersToTextureSet(textureSet);
                }
                textureList.SelectedIndex = -1;
                MessageBox.Show("Replacement succeeded.", VersionText);
            }
        }
        private void AddWatchersToTextureSet(TextureSet textureSet) {
            AddWatcher(textureSet.Base);
            AddWatcher(textureSet.Normal);
            AddWatcher(textureSet.Mask);
            AddWatcher(textureSet.NormalMask);
            AddWatcher(textureSet.Glow);

            foreach (var item in textureSet.BaseOverlays) {
                AddWatcher(item);
            }
            foreach (var item in textureSet.NormalOverlays) {
                AddWatcher(item);
            }
            foreach (var item in textureSet.MaskOverlays) {
                AddWatcher(item);
            }
        }

        private void RemovedWatchersFromTextureSet(TextureSet textureSet) {
            DisposeWatcher(textureSet.Base, Base);
            DisposeWatcher(textureSet.Normal, normal);
            DisposeWatcher(textureSet.Mask, mask);
            DisposeWatcher(textureSet.NormalMask, bounds);
            DisposeWatcher(textureSet.Glow, glow);
            DisposeWatcher(textureSet.Material, null);

            foreach (var item in textureSet.BaseOverlays) {
                DisposeWatcher(item, null);
            }
            foreach (var item in textureSet.NormalOverlays) {
                DisposeWatcher(item, null);
            }
            foreach (var item in textureSet.MaskOverlays) {
                DisposeWatcher(item, null);
            }
        }
        private void bulkReplaceToolStripMenuItem_Click(object sender, EventArgs e) {
            FindAndReplace findAndReplace = new FindAndReplace();
            TextureSet sourceTextureSet = (textureList.Items[textureList.SelectedIndex] as TextureSet);
            Tokenizer tokenizer = new Tokenizer(sourceTextureSet.TextureSetName);
            findAndReplace.ReplacementString.Text = tokenizer.GetToken();
            findAndReplace.ReplacementGroup.Text = sourceTextureSet.GroupName
                != sourceTextureSet.TextureSetName ? sourceTextureSet.GroupName : "";
            findAndReplace.Base.CurrentPath = Base.CurrentPath;
            findAndReplace.Normal.CurrentPath = normal.CurrentPath;
            findAndReplace.Mask.CurrentPath = mask.CurrentPath;
            findAndReplace.Bounds.CurrentPath = bounds.CurrentPath;
            findAndReplace.Glow.CurrentPath = glow.CurrentPath;

            findAndReplace.TextureSets.AddRange(textureList.Items.Cast<TextureSet>().ToArray());
            if (findAndReplace.ShowDialog() == DialogResult.OK) {
                foreach (TextureSet textureSet in textureList.Items) {
                    AddWatchersToTextureSet(textureSet);
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
            string templatePath = Path.Combine(GlobalPathStorage.OriginalBaseDirectory, @"res\templates");
            Directory.CreateDirectory(templatePath);
            foreach (string file in Directory.GetFiles(templatePath)) {
                if (file.ToLower().EndsWith(".ffxivtp")) {
                    ToolStripMenuItem menuItem = new ToolStripMenuItem();
                    menuItem.Text = Path.GetFileNameWithoutExtension(file);
                    menuItem.Click += (s, e) => {
                        string value = file;
                        OpenLoadTemplate(value);
                    };
                    templatesToolStripMenuItem.DropDownItems.Add(menuItem);
                }
            }
        }

        private void OpenLoadTemplate(string templatePaths, string overrideGroupName = "", TextureSet paths = null) {
            OpenTemplate(templatePaths, overrideGroupName);
            if (paths == null) {
                MessageBox.Show("You can now select textures to bulk replace in the template.", VersionText);
            }
            FindAndReplace findAndReplace = new FindAndReplace();
            textureList.SelectedIndex = textureList.Items.Count - 1;
            TextureSet sourceTextureSet = (textureList.Items[textureList.SelectedIndex] as TextureSet);
            Tokenizer tokenizer = new Tokenizer(sourceTextureSet.TextureSetName);
            findAndReplace.ReplacementString.Text = tokenizer.GetToken();
            findAndReplace.ReplacementGroup.Text = "";
            findAndReplace.Base.CurrentPath = paths != null ? paths.Base : Base.CurrentPath;
            findAndReplace.Normal.CurrentPath = paths != null ? paths.Normal : normal.CurrentPath;
            findAndReplace.Mask.CurrentPath = paths != null ? paths.Mask : mask.CurrentPath;
            findAndReplace.Glow.CurrentPath = paths != null ? paths.Glow : glow.CurrentPath;
            findAndReplace.Bounds.CurrentPath = bounds.CurrentPath;

            findAndReplace.TextureSets.AddRange(textureList.Items.Cast<TextureSet>().ToArray());
            if (_lastGroupName != "Default") {
                findAndReplace.ReplacementGroup.Text = sourceTextureSet.GroupName
                != sourceTextureSet.TextureSetName ? sourceTextureSet.GroupName : "";
            }
            if (paths == null && findAndReplace.ShowDialog() == DialogResult.OK) {
                foreach (TextureSet textureSet in textureList.Items) {
                    AddWatchersToTextureSet(textureSet);
                }
                textureList.SelectedIndex = -1;
                MessageBox.Show("Replacement succeeded.", VersionText);
            } else {
                findAndReplace.AcceptChanges();
            }
        }

        private void ConfigurePenumbraModFolder() {
            MessageBox.Show("Please configure where your penumbra mods folder is, we will remember it for all future exports. " +
                "This should be where you have penumbra set to use mods.\r\n\r\n" +
                "Note:\r\nAVOID MANUALLY CREATING ANY NEW FOLDERS IN YOUR PENUMBRA FOLDER, ONLY SELECT THE BASE FOLDER!", VersionText);
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
                        PenumbraModPath modPath = JsonConvert.DeserializeObject<PenumbraModPath>(reader.ReadToEnd());
                        if (modPath != null) {
                            penumbraModPath = modPath.ModDirectory;
                        }
                    }
                } else {
                    path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "XIVLauncherCN\\pluginConfigs\\Penumbra.json");
                    if (File.Exists(path)) {
                        using (StreamReader reader = new StreamReader(path)) {
                            PenumbraModPath modPath = JsonConvert.DeserializeObject<PenumbraModPath>(reader.ReadToEnd());
                            if (modPath != null) {
                                penumbraModPath = modPath.ModDirectory;
                            }
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
                Hide();
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
                    try {
                        genderList.SelectedIndex = Math.Clamp(int.Parse(reader.ReadLine()), 0, int.MaxValue); ;
                        raceList.SelectedIndex = Math.Clamp(int.Parse(reader.ReadLine()), 0, int.MaxValue);
                        subRaceList.SelectedIndex = Math.Clamp(int.Parse(reader.ReadLine()), 0, int.MaxValue);
                        int value = Math.Clamp(int.Parse(reader.ReadLine()), 0, int.MaxValue);
                        if (value < 8) {
                            baseBodyList.SelectedIndex = value;
                        } else {
                            MessageBox.Show("Previously selected body type is not valid");
                        }
                    } catch {
                    }
                }
            }
        }
        public void WriteLastUsedOptions() {
            string dataPath = Application.UserAppDataPath.Replace(Application.ProductVersion, null);
            try {
                using (StreamWriter writer = new StreamWriter(Path.Combine(dataPath, @"UsedOptions.config"))) {
                    writer.WriteLine(genderList.SelectedIndex);
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
                auraFaceScalesDropdown.Enabled = asymCheckbox.Enabled = faceTypeList.Enabled = subRaceList.Enabled = false;
                faceExtraList.Enabled = true;
            } else if (facePart.SelectedIndex == 2) {
                auraFaceScalesDropdown.Enabled = false;
            } else if (facePart.SelectedIndex == 5) {
                auraFaceScalesDropdown.Enabled = false;
                asymCheckbox.Enabled = faceTypeList.Enabled;
                faceExtraList.Enabled = true;
            } else {
                asymCheckbox.Enabled = faceTypeList.Enabled = subRaceList.Enabled = true;
                if (subRaceList.SelectedIndex == 10 || subRaceList.SelectedIndex == 11) {
                    auraFaceScalesDropdown.Enabled = true;
                }
                faceExtraList.Enabled = false;
            }
            addFaceButton.Text = "Add " + (facePart.SelectedItem as string);
        }
        private void baseBodyList_SelectedIndexChanged(object sender, EventArgs e) {
            switch (baseBodyList.SelectedIndex) {
                case 0:
                    //genderList.Enabled = true;
                    tailList.Enabled = false;
                    uniqueAuRa.Enabled = false;
                    break;
                case 1:
                case 2:
                    genderList.SelectedIndex = 1;
                    //genderList.Enabled = false;
                    tailList.Enabled = false;
                    if (raceList.SelectedIndex == 3) {
                        subRaceList.SelectedIndex = raceList.SelectedIndex = 0;
                    }
                    uniqueAuRa.Enabled = false;
                    break;
                case 3:
                    genderList.SelectedIndex = 0;
                    //genderList.Enabled = false;
                    tailList.Enabled = false;
                    if (raceList.SelectedIndex == 3) {
                        subRaceList.SelectedIndex = raceList.SelectedIndex = 0;
                    }
                    uniqueAuRa.Enabled = true;
                    break;
                case 4:
                    if (raceList.SelectedIndex != 6 && raceList.SelectedIndex != 4) {
                        raceList.SelectedIndex = 6;
                        //genderList.Enabled = true;
                    }
                    tailList.Enabled = true;
                    uniqueAuRa.Enabled = false;
                    break;
                case 5:
                    //genderList.Enabled = false;
                    raceList.SelectedIndex = 3;
                    tailList.Enabled = false;
                    break;
            }
        }

        private void raceList_SelectedIndexChanged(object sender, EventArgs e) {
            if (baseBodyList.SelectedIndex == 4) {
                if (raceList.SelectedIndex != 4 && raceList.SelectedIndex != 6 && raceList.SelectedIndex != 7) {
                    baseBodyList.SelectedIndex = 0;
                }
            } else if (baseBodyList.SelectedIndex == 5) {
                if (raceList.SelectedIndex != 3) {
                    if (genderList.SelectedIndex == 0) {
                        baseBodyList.SelectedIndex = 3;
                    } else {
                        baseBodyList.SelectedIndex = 1;
                    }
                }
            } else if (baseBodyList.SelectedIndex > 0 && baseBodyList.SelectedIndex < 5) {
                if (raceList.SelectedIndex == 3) {
                    baseBodyList.SelectedIndex = 5;
                }
            } else if (baseBodyList.SelectedIndex > 4) {
                if (raceList.SelectedIndex != 3) {
                    if (genderList.SelectedIndex == 0) {
                        baseBodyList.SelectedIndex = 3;
                    } else {
                        baseBodyList.SelectedIndex = 1;
                    }
                }
            }
            lastRaceIndex = raceList.SelectedIndex;
        }
        private void omniExportModeToolStripMenuItem_Click(object sender, EventArgs e) {
            TextureSet textureSet = (textureList.Items[textureList.SelectedIndex] as TextureSet);
            if (textureSet != null) {
                if (!textureSet.OmniExportMode) {
                    textureSet.OmniExportMode = true;
                    UniversalTextureSetCreator.ConfigureTextureSet(textureSet);
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
            textureSet.TextureSetName = baseBodyList.Text + (baseBodyList.Text.ToLower().Contains("tail") ? " " +
                (tailList.SelectedIndex + 1) : "") + ", " + (raceList.SelectedIndex == 3 ? "Unisex" : genderList.Text)
                + ", " + raceList.Text;
            AddBodyPaths(textureSet);
            textureList.Items.Add(textureSet);
            textureList.SelectedIndex = textureList.Items.Count - 1;
            HasSaved = false;
        }

        public void AddBodyPaths(TextureSet textureSet) {
            if (raceList.SelectedIndex != 3 || baseBodyList.SelectedIndex != 6) {
                textureSet.InternalBasePath = RacePaths.GetBodyTexturePath(0, genderList.SelectedIndex,
                    baseBodyList.SelectedIndex, raceList.SelectedIndex, tailList.SelectedIndex, uniqueAuRa.Checked);
            }
            textureSet.InternalNormalPath = RacePaths.GetBodyTexturePath(1, genderList.SelectedIndex,
                    baseBodyList.SelectedIndex, raceList.SelectedIndex, tailList.SelectedIndex, uniqueAuRa.Checked);

            textureSet.InternalMaskPath = RacePaths.GetBodyTexturePath(2, genderList.SelectedIndex,
                    baseBodyList.SelectedIndex, raceList.SelectedIndex, tailList.SelectedIndex, uniqueAuRa.Checked);
            textureSet.InternalMaterialPath = RacePaths.GetBodyMaterialPath(genderList.SelectedIndex, baseBodyList.SelectedIndex, raceList.SelectedIndex, tailList.SelectedIndex);
            BackupTexturePaths.AddBodyBackupPaths(genderList.SelectedIndex, raceList.SelectedIndex, textureSet);

        }

        private void addFaceButton_Click(object sender, EventArgs e) {
            hasDoneReload = false;
            TextureSet textureSet = new TextureSet();
            textureSet.TextureSetName = facePart.Text + (facePart.SelectedIndex == 4 ? " "
                + (faceExtraList.SelectedIndex + 1) : "") + ", " + (facePart.SelectedIndex != 4 ? genderList.Text : "Unisex")
                + ", " + (facePart.SelectedIndex != 4 ? subRaceList.Text : "Multi Race") + ", "
                + (facePart.SelectedIndex != 4 ? faceTypeList.Text : "Multi Face");
            textureSet.BackupTexturePaths = null;
            switch (facePart.SelectedIndex) {
                default:
                    AddFacePaths(textureSet);
                    if (facePart.SelectedIndex == 0) {
                        textureSet.UsesScales = auraFaceScalesDropdown.SelectedIndex == 1;
                        BackupTexturePaths.AddFaceBackupPaths(genderList.SelectedIndex, subRaceList.SelectedIndex, faceTypeList.SelectedIndex, textureSet);
                    }
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
            textureSet.InternalMaterialPath = RacePaths.GetFacePath(0, genderList.SelectedIndex, subRaceList.SelectedIndex,
            facePart.SelectedIndex, faceTypeList.SelectedIndex, auraFaceScalesDropdown.SelectedIndex, asymCheckbox.Checked, true);
            textureSet.IgnoreMaskGeneration = true;
            textureList.Items.Add(textureSet);
            textureList.SelectedIndex = textureList.Items.Count - 1;
            HasSaved = false;
        }

        public void AddDecalPath(TextureSet textureSet) {
            textureSet.InternalBasePath = "chara/common/texture/decal_face/_decal_" + (faceExtraList.SelectedIndex + 1) + ".tex";
        }

        public void AddHairPaths(TextureSet textureSet) {
            textureSet.TextureSetName = facePart.Text + " " + (faceExtraList.SelectedIndex + 1) + ", " + genderList.Text
      + ", " + raceList.Text;

            textureSet.InternalNormalPath = RacePaths.GetHairTexturePath(1, faceExtraList.SelectedIndex,
                genderList.SelectedIndex, raceList.SelectedIndex, subRaceList.SelectedIndex);

            textureSet.InternalMaskPath = RacePaths.GetHairTexturePath(2, faceExtraList.SelectedIndex,
                genderList.SelectedIndex, raceList.SelectedIndex, subRaceList.SelectedIndex);
        }

        public void AddEyePaths(TextureSet textureSet) {
            if (asymCheckbox.Checked) {
                textureSet.InternalBasePath = RacePaths.GetFacePath(0, genderList.SelectedIndex, subRaceList.SelectedIndex,
                2, faceTypeList.SelectedIndex, auraFaceScalesDropdown.SelectedIndex, asymCheckbox.Checked);
                textureSet.InternalNormalPath = RacePaths.GetFacePath(1, genderList.SelectedIndex, subRaceList.SelectedIndex,
                2, faceTypeList.SelectedIndex, auraFaceScalesDropdown.SelectedIndex, asymCheckbox.Checked);
                textureSet.InternalMaskPath = RacePaths.GetFacePath(2, genderList.SelectedIndex, subRaceList.SelectedIndex,
                2, faceTypeList.SelectedIndex, auraFaceScalesDropdown.SelectedIndex, asymCheckbox.Checked);
            } else {
                RaceEyePaths.GetEyeTextureSet(subRaceList.SelectedIndex, faceTypeList.SelectedIndex, Convert.ToBoolean(genderList.SelectedIndex), textureSet);
            }
        }

        public void AddEyePathsLegacy(TextureSet textureSet) {
            textureSet.InternalBasePath = RacePaths.GetFacePath(0, genderList.SelectedIndex, subRaceList.SelectedIndex,
            2, faceTypeList.SelectedIndex, auraFaceScalesDropdown.SelectedIndex, asymCheckbox.Checked);

            textureSet.InternalNormalPath = RacePaths.GetFacePath(1, genderList.SelectedIndex, subRaceList.SelectedIndex,
            2, faceTypeList.SelectedIndex, auraFaceScalesDropdown.SelectedIndex, asymCheckbox.Checked);

            textureSet.InternalMaskPath = RacePaths.GetFacePath(2, genderList.SelectedIndex, subRaceList.SelectedIndex,
            2, faceTypeList.SelectedIndex, auraFaceScalesDropdown.SelectedIndex, asymCheckbox.Checked);
        }

        public void AddFacePaths(TextureSet textureSet) {
            if (facePart.SelectedIndex != 1) {
                textureSet.InternalBasePath = RacePaths.GetFacePath(0, genderList.SelectedIndex, subRaceList.SelectedIndex,
                    facePart.SelectedIndex, faceTypeList.SelectedIndex, auraFaceScalesDropdown.SelectedIndex, asymCheckbox.Checked);
            }

            textureSet.InternalNormalPath = RacePaths.GetFacePath(1, genderList.SelectedIndex, subRaceList.SelectedIndex,
            facePart.SelectedIndex, faceTypeList.SelectedIndex, auraFaceScalesDropdown.SelectedIndex, asymCheckbox.Checked);

            textureSet.InternalMaskPath = RacePaths.GetFacePath(2, genderList.SelectedIndex, subRaceList.SelectedIndex,
            facePart.SelectedIndex, faceTypeList.SelectedIndex, auraFaceScalesDropdown.SelectedIndex, asymCheckbox.Checked);
        }

        private void textureSetList_SelectedIndexChanged(object sender, EventArgs e) {
            if (textureList.SelectedIndex == -1) {
                currentEditLabel.Text = "Locked:";
                textureSetName.Text = "Please select a texture set to start importing";
                textureSetName.Enabled = false;
                SetControlsEnabled(false);
            } else {
                TextureSet textureSet = textureList.Items[textureList.SelectedIndex] as TextureSet;
                currentEditLabel.Text = "Editing:";
                textureSetName.Text = textureSet.TextureSetName;
                SetControlsEnabled(true, textureSet);
                SetControlsPaths(textureSet);
                SetControlsColors(textureSet);
                textureSetName.Enabled = true;
            }
        }

        private void SetControlsEnabled(bool enabled, TextureSet textureSet = null) {
            if (textureSet == null) {
                enabled = false;
            }
            Base.Enabled = enabled && !string.IsNullOrEmpty(textureSet.InternalBasePath);
            normal.Enabled = enabled && !string.IsNullOrEmpty(textureSet.InternalNormalPath);
            mask.Enabled = enabled && !string.IsNullOrEmpty(textureSet.InternalMaskPath);

            layerBaseButton.Enabled = Base.Enabled;
            layerNormalButton.Enabled = normal.Enabled;
            layerMaskButton.Enabled = mask.Enabled;

            bounds.Enabled = enabled && bakeNormals.Checked;
            glow.Enabled = enabled && !textureSet.TextureSetName.ToLower().Contains("face paint")
                    && !textureSet.TextureSetName.ToLower().Contains("hair") && Base.Enabled;
        }

        private void SetControlsPaths(TextureSet textureSet) {
            Base.CurrentPath = textureSet.Base;
            normal.CurrentPath = textureSet.Normal;
            mask.CurrentPath = textureSet.Mask;
            bounds.CurrentPath = textureSet.NormalMask;
            glow.CurrentPath = textureSet.Glow;
        }

        private void SetControlsColors(TextureSet texturelSet) {
            Base.LabelName.Text = "Base";
            normal.LabelName.Text = "Normal";
            mask.LabelName.Text = "Mask";
            Base.BackColor = originalBaseBoxColour;
            normal.BackColor = originalNormalBoxColour;
            mask.BackColor = originalMultiBoxColour;
        }
        public void SetPaths() {
            if (textureList.SelectedIndex != -1) {
                TextureSet textureSet = (textureList.Items[textureList.SelectedIndex] as TextureSet);
                RemovedWatchersFromTextureSet(textureSet);
                if (!string.IsNullOrWhiteSpace(textureSet.Glow) && !textureSet.InternalBasePath.Contains("eye")) {
                    bakeNormals.Checked = true;
                }
                textureSet.Base = Base.CurrentPath;
                textureSet.Normal = normal.CurrentPath;
                textureSet.Mask = mask.CurrentPath;
                textureSet.NormalMask = bounds.CurrentPath;
                textureSet.Glow = glow.CurrentPath;
                AddWatchersToTextureSet(textureSet);
            }
        }
        public void DisposeWatcher(string path, FilePicker filePicker) {
            if (!string.IsNullOrWhiteSpace(path)) {
                if (watchers.ContainsKey(path)) {
                    if (filePicker == null || path != filePicker.CurrentPath) {
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
                Base.CurrentPath = "";
                normal.CurrentPath = "";
                mask.CurrentPath = "";
                glow.CurrentPath = "";
                textureSetName.Text = "Please select a texture set to start importing";
                currentEditLabel.Text = "Locked:";
                textureSetName.Enabled = false;
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
            Base.CurrentPath = "";
            normal.CurrentPath = "";
            mask.CurrentPath = "";
            bounds.CurrentPath = "";
            glow.CurrentPath = "";

            Base.Enabled = false;
            normal.Enabled = false;
            mask.Enabled = false;
            bounds.Enabled = false;
            glow.Enabled = false;

        }

        public void multi_OnFileSelected(object sender, EventArgs e) {
            SetPaths();
            HasSaved = false;
            hasDoneReload = false;
        }
        private void addCustomPathButton_Click(object sender, EventArgs e) {
            CustomPathDialog customPathDialog = new CustomPathDialog();
            foreach (string option in generationType.Items) {
                customPathDialog.GroupingType.Items.Add(option);
                customPathDialog.GroupingType.SelectedIndex = 0;
            }
            if (customPathDialog.ShowDialog() == DialogResult.OK) {
                textureList.Items.Add(customPathDialog.TextureSet);
                textureList.SelectedIndex = textureList.Items.Count - 1;
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
            if (textureList.SelectedIndex != -1) {
                CustomPathDialog customPathDialog = new CustomPathDialog();
                customPathDialog.TextureSet = (textureList.Items[textureList.SelectedIndex] as TextureSet);
                foreach (string option in generationType.Items) {
                    customPathDialog.GroupingType.Items.Add(option);
                }
                customPathDialog.GroupingType.SelectedIndex = (
                groupOptionTypes.ContainsKey(customPathDialog.TextureSet.GroupName) ?
                groupOptionTypes[customPathDialog.TextureSet.GroupName] : 0);
                if (customPathDialog.ShowDialog() == DialogResult.OK) {
                    groupOptionTypes[customPathDialog.TextureSet.GroupName] = customPathDialog.GroupingType.SelectedIndex;
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
                                Text = Application.ProductName + " " + Program.Version + $" ({savePath})";
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
            Text = Application.ProductName + " " + Program.Version;
            savePath = null;
            textureList.Items.Clear();
            modNameTextBox.Text = "";
            modAuthorTextBox.Text = _defaultAuthor;
            modVersionTextBox.Text = "1.0.0";
            modDescriptionTextBox.Text = _defaultDescription;
            modWebsiteTextBox.Text = _defaultWebsite;
            Base.CurrentPath = "";
            normal.CurrentPath = "";
            mask.CurrentPath = "";
            glow.CurrentPath = "";

            Base.Enabled = false;
            normal.Enabled = false;
            mask.Enabled = false;
            bounds.Enabled = false;
            glow.Enabled = false;
            HasSaved = true;
            foreach (FileSystemWatcher watcher in watchers.Values) {
                watcher.Dispose();
            }
            watchers.Clear();
            currentEditLabel.Text = "Locked:";
            textureSetName.Text = "Please select a texture set to start importing";
            textureSetName.Enabled = false;
            lockDuplicateGeneration = false;
            mainFormSimplified.ClearForm();
            if (IsSimpleMode) {
                mainFormSimplified.RefreshValues();
            }
        }
        public void saveToolStripMenuItem_Click(object sender, EventArgs e) {
            Save();
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
                    //if (IntegrityChecker.IntegrityCheck()) {
                    //    IntegrityChecker.ShowOpen();
                    //}
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
                int missingFiles = 0;
                bool foundFaceMod = false;
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
                generatMaskCheckBox.Checked = projectFile.GenerateMulti;
                if (projectFile.GroupOptionTypes != null) {
                    groupOptionTypes = projectFile.GroupOptionTypes;
                }
                if (projectFile.ProjectVersion < 5) {
                    ProjectUpgrade(projectFile, ref missingFiles, ref foundFaceMod);
                }
                textureList.Items.AddRange(projectFile.TextureSets?.ToArray());
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
                foreach (TextureSet textureSet in projectFile.TextureSets) {
                    AddWatchersToTextureSet(textureSet);
                }
                if (missingFiles > 0) {
                    MessageBox.Show($"{missingFiles} texture sets are missing original files. Please update the file paths", VersionText);
                }
                if (foundFaceMod) {
                    MessageBox.Show($"Face mods created before Dawntrail are no longer compatible with the game due to UV changes." +
                        $" You will need to use the Face Salvager to attempt to recover it." +
                        $"\r\n\"Tools -> Face Tools -> Legacy Face Salvager\"", VersionText);
                }
            }
            HasSaved = true;
        }

        private void ProjectUpgrade(ProjectFile projectFile, ref int missingFiles, ref bool foundFaceMod) {
            foreach (TextureSet textureSet in projectFile.TextureSets) {
                if (textureSet.InternalMaskPath.Contains("catchlight")) {
                    textureSet.InternalMaskPath = RacePaths.OldEyePathToNewEyeMultiPath(textureSet.InternalNormalPath);
                    textureSet.InternalNormalPath = RacePaths.OldEyePathToNewEyeNormalPath(textureSet.InternalBasePath);
                    textureSet.InternalBasePath = RacePaths.OldEyePathToNewEyeBasePath(textureSet.InternalBasePath);
                    if (File.Exists(textureSet.Normal)) {
                        var strings = ImageManipulation.ConvertImageToEyeMapsDawntrail(textureSet.Normal, true, null, true);
                        textureSet.Base = strings[0];
                        textureSet.Normal = strings[1];
                        textureSet.Mask = strings[2];
                    } else {
                        textureSet.Mask = textureSet.Normal;
                        textureSet.Normal = textureSet.Base;
                        textureSet.Base = "";
                        missingFiles++;
                    }
                }
                if (textureSet.InternalMaskPath.Contains("hair")) {
                    if (File.Exists(textureSet.Mask)) {
                        string newMultiPath = ImageManipulation.AddSuffix(ImageManipulation.ReplaceExtension(textureSet.Mask, ".png"), "_dawntrail");
                        if (!File.Exists(newMultiPath)) {
                            TexIO.SaveBitmap(ImageManipulation.LegacyHairMultiToDawntrailMulti(
                           TexIO.ResolveBitmap(textureSet.Mask)), newMultiPath);
                        }
                        textureSet.Mask = newMultiPath;
                        if (File.Exists(textureSet.Normal)) {
                            string newNormalPath = ImageManipulation.AddSuffix(ImageManipulation.ReplaceExtension(textureSet.Normal, ".png"), "_dawntrail");
                            if (!File.Exists(newNormalPath)) {
                                TexIO.SaveBitmap(ImageManipulation.LegacyHairNormalToDawntrailNormal(
                            TexIO.ResolveBitmap(textureSet.Mask),
                            TexIO.ResolveBitmap(textureSet.Normal)), newNormalPath);
                            }
                            textureSet.Normal = newMultiPath;
                        }
                    } else {
                        textureSet.Mask = textureSet.Normal;
                        textureSet.Normal = textureSet.Base;
                        textureSet.Base = "";
                        missingFiles++;
                    }
                }
                textureSet.InternalMaskPath = RacePaths.PathCorrector(textureSet.InternalMaskPath);
                textureSet.InternalNormalPath = RacePaths.PathCorrector(textureSet.InternalNormalPath);
                textureSet.InternalBasePath = RacePaths.PathCorrector(textureSet.InternalBasePath);
                if (textureSet.InternalMaskPath.Contains("fac_d")) {
                    foundFaceMod = true;
                }
            }
        }

        public void OpenTemplate(string path, string overridePath = "") {
            using (StreamReader file = File.OpenText(path)) {
                JsonSerializer serializer = new JsonSerializer();
                ProjectFile projectFile = (ProjectFile)serializer.Deserialize(file, typeof(ProjectFile));
                TemplateConfiguration templateConfiguration = new TemplateConfiguration();
                if (!string.IsNullOrEmpty(overridePath) || templateConfiguration.ShowDialog() == DialogResult.OK) {
                    BringToFront();
                    int missingFiles = 0;
                    bool foundFaceMod = false;
                    if (projectFile.ProjectVersion < 5) {
                        ProjectUpgrade(projectFile, ref missingFiles, ref foundFaceMod);
                    }
                    foreach (TextureSet textureSet in projectFile.TextureSets) {
                        if (!string.IsNullOrEmpty(overridePath)) {
                            _lastGroupName = overridePath;
                            if (!overridePath.Contains("Default")) {
                                textureSet.GroupName = overridePath;
                            }
                        } else {
                            _lastGroupName = templateConfiguration.GroupName;
                            if (!templateConfiguration.GroupName.Contains("Default")) {
                                textureSet.GroupName = templateConfiguration.GroupName;
                            }
                        }
                        AddWatchersToTextureSet(textureSet);
                    }
                    textureList.Items.AddRange(projectFile.TextureSets?.ToArray());
                }
            }
            HasSaved = false;
        }



        public void SaveProject(string path, bool ignoreSaveAlert = false) {
            using (StreamWriter writer = new StreamWriter(path)) {
                JsonSerializer serializer = new JsonSerializer();
                ProjectFile projectFile = new ProjectFile();
                projectFile.ProjectVersion = 5;
                projectFile.Name = modNameTextBox.Text;
                projectFile.Author = modAuthorTextBox.Text;
                projectFile.Version = modVersionTextBox.Text;
                projectFile.Description = modDescriptionTextBox.Text;
                projectFile.Website = modWebsiteTextBox.Text;
                projectFile.GroupOptionTypes = groupOptionTypes;
                projectFile.TextureSets = new List<TextureSet>();
                projectFile.ExportType = generationType.SelectedIndex;
                projectFile.BakeMissingNormals = bakeNormals.Checked;
                projectFile.GenerateMulti = generatMaskCheckBox.Checked;
                projectFile.SimpleMode = isSimpleMode;
                projectFile.SimpleBodyType = mainFormSimplified.BodyType.SelectedIndex;
                projectFile.SimpleFaceType = mainFormSimplified.FaceType.SelectedIndex;
                projectFile.SimpleSubRaceType = mainFormSimplified.SubRace.SelectedIndex;
                projectFile.SimpleNormalGeneration = mainFormSimplified.NormalGeneration.SelectedIndex;
                foreach (TextureSet textureSet in textureList.Items) {
                    projectFile.TextureSets.Add(textureSet);
                }
                serializer.Serialize(writer, projectFile);
            }
            HasSaved = true;
            if (!ignoreSaveAlert) {
                MessageBox.Show("Save successfull", VersionText);
            }
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
        private void baseTextureMergerToolStripMenuItem_Click(object sender, EventArgs e) {
            new BaseMerger().Show();
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
            bounds.Enabled = bakeNormals.Checked && textureList.SelectedIndex > -1;
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
                    MessageBox.Show("Conversion successful!");
                    NavigateToFolder(saveFileDialog.FileName);
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
                    MessageBox.Show("Conversion successful!");
                    NavigateToFolder(saveFileDialog.FileName);
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
                    MessageBox.Show("Conversion successful!");
                    NavigateToFolder(saveFileDialog.FileName);
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
                    MessageBox.Show("Conversion successful!");
                    NavigateToFolder(saveFileDialog.FileName);
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
                    MessageBox.Show("Conversion successful!");
                    NavigateToFolder(saveFileDialog.FileName);
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
                    MessageBox.Show("Conversion successful!");
                    NavigateToFolder(saveFileDialog.FileName);
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
                    MessageBox.Show("Conversion successful!");
                    NavigateToFolder(saveFileDialog.FileName);
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
                    MessageBox.Show("Conversion successful!");
                    NavigateToFolder(saveFileDialog.FileName);
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
                    MessageBox.Show("Conversion successful!");
                    NavigateToFolder(saveFileDialog.FileName);
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
                    MessageBox.Show("Conversion successful!");
                    NavigateToFolder(saveFileDialog.FileName);
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
                    MessageBox.Show("Conversion successful!");
                    NavigateToFolder(saveFileDialog.FileName);
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
                    TexIO.SaveBitmap(MapWriting.ExtractGlowMapFromBase(
                        TexIO.ResolveBitmap(openFileDialog.FileName)), ImageManipulation.ReplaceExtension(saveFileDialog.FileName, ".png"));
                }
            }
        }
        #endregion
        #region Image Conversion Utilities

        private void bulkImageToTexToolStripMenuItem_Click(object sender, EventArgs e) {
            FolderBrowserDialog openFileDialog = new FolderBrowserDialog();
            MessageBox.Show("Please select folder");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                var files = Directory.GetFiles(openFileDialog.SelectedPath);
                foreach (string file in files) {
                    if (FilePicker.CheckExtentions(file) && !file.ToLower().EndsWith(".tex")) {
                        textureProcessor.ExportTex(file, ImageManipulation.ReplaceExtension(file, ".tex"));
                    }
                }
                MessageBox.Show("The operation succeeded!", VersionText);
            }
        }

        private void recursiveBulkImageToTexToolStripMenuItem_Click(object sender, EventArgs e) {
            FolderBrowserDialog openFileDialog = new FolderBrowserDialog();
            MessageBox.Show("Please select folder");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                RecursiveImageToTex(openFileDialog.SelectedPath);
                MessageBox.Show("The operation succeeded!", VersionText);
            }
        }
        private void multiMapToGrayscaleToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Texture File|*.png;*.tga;*.dds;*.bmp;*.tex;";
            MessageBox.Show("Please select input texture");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                Bitmap image = TexIO.ResolveBitmap(openFileDialog.FileName);
                TexIO.SaveBitmap(ImageManipulation.ExtractRed(image), ImageManipulation.AddSuffix(openFileDialog.FileName, "_grayscale"));
                MessageBox.Show("Multi successfully converted to grayscale", VersionText);
            }
        }
        public void RecursiveImageToTex(string filePath, int layer = 0, int maxLayer = int.MaxValue) {
            var files = Directory.GetFiles(filePath);
            foreach (string file in files) {
                if (FilePicker.CheckExtentions(file)) {
                    textureProcessor.ExportTex(file, ImageManipulation.ReplaceExtension(file, ".tex"));
                }
            }
            if (layer < maxLayer) {
                foreach (string directory in Directory.EnumerateDirectories(filePath)) {
                    RecursiveImageToTex(directory, layer + 1, maxLayer);
                }
            }
        }
        private void convertImageToEyeMultiToolStripMenuItem_Click(object sender, EventArgs e) {

        }

        private void convertImageToEyeMultiDawntrailToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Texture File|*.png;*.tga;*.dds;*.bmp;*.tex;";
            MessageBox.Show("Please select input texture");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                ImageManipulation.ConvertImageToEyeMapsDawntrail(openFileDialog.FileName, true);
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

        private void convertOldImageToEyeMultiDawntrailToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Texture File|*.png;*.tga;*.dds;*.bmp;*.tex;";
            MessageBox.Show("Please select input texture");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                ImageManipulation.ConvertOldEyeMultiToDawntrailEyeMaps(openFileDialog.FileName, true);
                MessageBox.Show("Image successfully converted to eye maps", VersionText);
                AutoModPackingPrompt(openFileDialog.FileName);
            }
        }

        private void convertImagesToAsymEyeMapsToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Texture File|*.png;*.tga;*.dds;*.bmp;*.tex;";
            OpenFileDialog openFileDialog2 = new OpenFileDialog();
            openFileDialog2.Filter = "Texture File|*.png;*.tga;*.dds;*.bmp;*.tex;";
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Texture File|*.png;";

            MessageBox.Show("Please select left input texture (left side of image, not left side of face)");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                MessageBox.Show("Please select right input texture (right side of image, not right side of face)");
                if (openFileDialog2.ShowDialog() == DialogResult.OK) {
                    MessageBox.Show("Please pick a file name for the merged result");
                    if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                        ImageManipulation.ConvertImageToAsymEyeMaps(openFileDialog.FileName, openFileDialog2.FileName, saveFileDialog.FileName);
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
        private void convertImageToDawntrailEyeMapsToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Texture File|*.png;*.tga;*.dds;*.bmp;*.tex;";
            MessageBox.Show("Please select input texture");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                ImageManipulation.ConvertImageToEyeMapsDawntrail(openFileDialog.FileName, true);
                MessageBox.Show("Image successfully converted to eye maps", VersionText);
                AutoModPackingPrompt(openFileDialog.FileName);
            }
        }
        public void AutoModPackingPrompt(string filePath) {
            if (MessageBox.Show("Would you like to export these textures as a mod right now?", VersionText, MessageBoxButtons.YesNo) == DialogResult.Yes) {
                string path1 = ImageManipulation.ReplaceExtension(ImageManipulation.AddSuffix(filePath, "_eye_base"), ".png");
                string path2 = ImageManipulation.ReplaceExtension(ImageManipulation.AddSuffix(filePath, "_eye_norm"), ".png");
                string path3 = ImageManipulation.ReplaceExtension(ImageManipulation.AddSuffix(filePath, "_eye_mask"), ".png");
                string modPackPackageFolder = Path.Combine(Path.GetDirectoryName(path1), "Mod Packs");
                ExportEyePackList(new string[] { path1, path2, path3 }, modPackPackageFolder);
            } else {
                try {
                    Process.Start(new System.Diagnostics.ProcessStartInfo() {
                        FileName = Path.GetDirectoryName(filePath),
                        UseShellExecute = true,
                        Verb = "OPEN"
                    });
                } catch {

                }
            }
        }

        public void AutoModPackingPromptContacts(string filePath) {
            if (MessageBox.Show("Would you like to export these textures as a mod right now?", VersionText, MessageBoxButtons.YesNo) == DialogResult.Yes) {
                string path1 = ImageManipulation.ReplaceExtension(ImageManipulation.AddSuffix(filePath, "_contactBase"), ".png");
                string modPackPackageFolder = Path.Combine(Path.GetDirectoryName(path1), "Mod Packs");
                ExportContactPackList(new string[] { path1 }, modPackPackageFolder);
            } else {
                try {
                    Process.Start(new System.Diagnostics.ProcessStartInfo() {
                        FileName = Path.GetDirectoryName(filePath),
                        UseShellExecute = true,
                        Verb = "OPEN"
                    });
                } catch {

                }
            }
        }

        private void convertFolderOfLegacyEyeMultiMapsToolStripMenuItem_Click(object sender, EventArgs e) {
            MessageBox.Show("Pick folder of generic eye textures to generate maps for.\r\nWARNING: This process may take a while if the folder has lots of eye textures.", VersionText);
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            int maxItems = 0;
            int itemsCounted = 0;
            int calculatingItems = 0;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK) {
                foreach (string file in Directory.EnumerateFiles(folderBrowserDialog.SelectedPath, "*.*", SearchOption.AllDirectories)
                .Where(s => s.EndsWith(".png") || s.EndsWith(".bmp") || s.EndsWith(".dds") || s.EndsWith(".tex"))) {
                    if (!file.Contains("_base") && !file.Contains("_norm") && !file.Contains("_mask")) {
                        maxItems++;
                        while (calculatingItems > Environment.ProcessorCount) {
                            Thread.Sleep(3000);
                        }
                        calculatingItems++;
                        Task.Run(() => {
                            try {
                                ImageManipulation.ConvertOldEyeMultiToDawntrailEyeMaps(file, true);
                            } catch {

                            }
                            itemsCounted++;
                            calculatingItems--;
                        });
                    }
                }
                while (itemsCounted < maxItems) {
                    Thread.Sleep(5000);
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
        private void convertFolderToEyeMapsToolStripMenuItem_Click(object sender, EventArgs e) {
            MessageBox.Show("Pick folder of generic eye textures to generate maps for.\r\nWARNING: This process may take a while if the folder has lots of eye textures.", VersionText);
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            int maxItems = 0;
            int itemsCounted = 0;
            int calculatingItems = 0;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK) {
                foreach (string file in Directory.EnumerateFiles(folderBrowserDialog.SelectedPath, "*.*", SearchOption.AllDirectories)
                .Where(s => s.EndsWith(".png") || s.EndsWith(".bmp") || s.EndsWith(".dds") || s.EndsWith(".tex"))) {
                    if (!file.Contains("_base") && !file.Contains("_norm") && !file.Contains("_mask")) {
                        maxItems++;
                        while (calculatingItems > Environment.ProcessorCount) {
                            Thread.Sleep(3000);
                        }
                        calculatingItems++;
                        Task.Run(() => {
                            try {
                                ImageManipulation.ConvertImageToEyeMapsDawntrail(file, true);
                            } catch {

                            }
                            itemsCounted++;
                            calculatingItems--;
                        });
                    }
                }
                while (itemsCounted < maxItems) {
                    Thread.Sleep(5000);
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
            openFileDialog.Filter = "Texture File|*.png;*.tga;*.dds;*.bmp;*.tex;";
            MessageBox.Show("Please select input texture");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                ImageManipulation.SplitImageToRGBA(openFileDialog.FileName);
                MessageBox.Show("Image successfully split into seperate channels", VersionText);
            }
        }

        private void multiCreatorToolStripMenuItem_Click(object sender, EventArgs e) {
            new MaskCreator().Show();
        }
        private void NavigateToFolder(string folder) {
            Process.Start(new System.Diagnostics.ProcessStartInfo() {
                FileName = Path.GetDirectoryName(folder),
                UseShellExecute = true,
                Verb = "OPEN"
            });
        }
        private void splitImageToRGBAndAlphaToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Texture File|*.png;*.tga;*.dds;*.bmp;*.tex;";
            MessageBox.Show("Please select input texture");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                ImageManipulation.SplitRGBAndAlpha(openFileDialog.FileName);
                MessageBox.Show("Image successfully split into RGB and Alpha", VersionText);
            }
        }

        private void mergeRGBAndAlphaImagesToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialogRGB = new OpenFileDialog();
            OpenFileDialog openFileDialogAlpha = new OpenFileDialog();
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            openFileDialogRGB.Filter = "Texture File|*.png;*.tga;*.dds;*.bmp;*.tex;";
            openFileDialogAlpha.Filter = "Texture File|*.png;*.tga;*.dds;*.bmp;*.tex;";
            saveFileDialog.Filter = "Texture File|*.png;";
            MessageBox.Show("Please select RGB texture");
            if (openFileDialogRGB.ShowDialog() == DialogResult.OK) {
                MessageBox.Show("Please select alpha texture");
                if (openFileDialogAlpha.ShowDialog() == DialogResult.OK) {
                    MessageBox.Show("Please select where you want to save the conversion", VersionText);
                    if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                        TexIO.SaveBitmap(ImageManipulation.MergeAlphaToRGB(TexIO.ResolveBitmap(openFileDialogAlpha.FileName),
                            TexIO.ResolveBitmap(openFileDialogRGB.FileName)), ImageManipulation.ReplaceExtension(saveFileDialog.FileName, ".png"));
                    }
                }
            }
        }

        private void bulkConvertImagesToLTCTToolStripMenuItem_Click(object sender, EventArgs e) {
            FolderBrowserDialog openFileDialog = new FolderBrowserDialog();
            MessageBox.Show("Please select input folder");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                TexIO.ConvertToLtct(openFileDialog.SelectedPath);
            }
        }

        private void bulkConvertLTCTToPNGToolStripMenuItem_Click(object sender, EventArgs e) {
            FolderBrowserDialog openFileDialog = new FolderBrowserDialog();
            MessageBox.Show("Please select input folder");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                TexIO.ConvertLtctToPng(openFileDialog.SelectedPath);
            }
        }

        private void optimizePNGToolStripMenuItem_Click(object sender, EventArgs e) {
            FolderBrowserDialog openFileDialog = new FolderBrowserDialog();
            MessageBox.Show("Please select input folder");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                TexIO.RunOptiPNG(openFileDialog.SelectedPath);
            }
        }

        private void convertPNGToLTCTToolStripMenuItem_Click(object sender, EventArgs e) {
            FolderBrowserDialog openFileDialog = new FolderBrowserDialog();
            MessageBox.Show("Please select input folder");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                TexIO.ConvertPngToLtct(openFileDialog.SelectedPath);
            }
        }
        private void generateXNormalTranslationMapToolStripMenuItem_Click(object sender, EventArgs e) {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Texture File|*.png;";
            MessageBox.Show("Please select where you want to save the result", VersionText);
            if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                TexIO.SaveBitmap(ImageManipulation.GenerateXNormalTranslationMap(), ImageManipulation.ReplaceExtension(saveFileDialog.FileName, ".png"));
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
        private void howDoIMakeEyesToolStripMenuItem_Click(object sender, EventArgs e) {
            try {
                Process.Start(new System.Diagnostics.ProcessStartInfo() {
                    FileName = "https://docs.google.com/document/d/1Smef3rexDHoRQSV1ZjT6R20EeIIyVK4W73wjPZDcys4/edit?usp=sharing",
                    UseShellExecute = true,
                    Verb = "OPEN"
                });
            } catch {

            }
        }
        public void creditsToolStripMenuItem_Click(object sender, EventArgs e) {
            MessageBox.Show("Credits for the resources used in this tool:\r\n\r\nThe creators of Bibo+\r\nThe creators of Tight&Firm (Gen3)\r\nThe creators of TBSE\r\nThe creator of Otopop.\r\nThe creators of Pythia\r\nThe creators of Freyja\r\nThe creators of Eve\r\nThe creators of EXQB\r\n\r\nTake care to read the terms and permissions for each body type when releasing public mods.\r\n\r\nThanks to Yuria and KZ for helping improve a portion of the face bake models!\r\n\r\nSpecial thanks to Zatori for all their help with testing, and all of you for using the tool!", VersionText);
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

        private void textureToBodyMultiToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Texture File|*.png;*.tga;*.dds;*.bmp;*.tex;";
            MessageBox.Show("Please select input texture");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                Bitmap image = TexIO.ResolveBitmap(openFileDialog.FileName);
                TexIO.SaveBitmap(ImageManipulation.GenerateSkinMulti(image),
                    ImageManipulation.ReplaceExtension(ImageManipulation.AddSuffix(openFileDialog.FileName, "_multi."), ".png"));
                MessageBox.Show("Texture converted to body multi", VersionText);
            }
        }
        private void baseTextureToDawntrailSkinMultiToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Texture File|*.png;*.tga;*.dds;*.bmp;*.tex;";
            MessageBox.Show("Please select input texture");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                Bitmap image = TexIO.ResolveBitmap(openFileDialog.FileName);
                TexIO.SaveBitmap(ImageManipulation.ConvertBaseToDawntrailSkinMulti(image)
                    , ImageManipulation.ReplaceExtension(ImageManipulation.AddSuffix(openFileDialog.FileName, "_multi."), ".png"));
                MessageBox.Show("Texture converted to skin multi", VersionText);
            }
        }
        private void textureToFaceMultiToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Texture File|*.png;*.tga;*.dds;*.bmp;*.tex;";
            MessageBox.Show("Please select input texture");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                Bitmap image = TexIO.ResolveBitmap(openFileDialog.FileName);
                TexIO.SaveBitmap(ImageManipulation.GenerateFaceMulti(image, false)
                    , ImageManipulation.ReplaceExtension(ImageManipulation.AddSuffix(openFileDialog.FileName, "_multi."), ".png"));
                MessageBox.Show("Texture converted to body multi", VersionText);
            }
        }

        private void textureToAsymFaceMultiToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Texture File|*.png;*.tga;*.dds;*.bmp;*.tex;";
            MessageBox.Show("Please select input texture");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                Bitmap image = TexIO.ResolveBitmap(openFileDialog.FileName);
                TexIO.SaveBitmap(ImageManipulation.GenerateFaceMulti(image, true)
                    , ImageManipulation.ReplaceExtension(ImageManipulation.AddSuffix(openFileDialog.FileName, "_multi."), ".png"));
                MessageBox.Show("Texture converted to body multi", VersionText);
            }
        }

        private void duplicateToolStripMenuItem_Click(object sender, EventArgs e) {
            TextureSet newTextureSet = new TextureSet();
            TextureSet textureSet = (textureList.SelectedItem as TextureSet);
            newTextureSet.Base = textureSet.Base;
            newTextureSet.Normal = textureSet.Normal;
            newTextureSet.Mask = textureSet.Mask;
            newTextureSet.Glow = newTextureSet.Glow;
            newTextureSet.Material = textureSet.Material;
            newTextureSet.NormalCorrection = newTextureSet.NormalCorrection;
            newTextureSet.InternalBasePath = textureSet.InternalBasePath;
            newTextureSet.InternalNormalPath = textureSet.InternalNormalPath;
            newTextureSet.InternalMaskPath = textureSet.InternalMaskPath;
            newTextureSet.InternalMaterialPath = textureSet.InternalMaterialPath;
            newTextureSet.BackupTexturePaths = textureSet.BackupTexturePaths;
            newTextureSet.ChildSets = textureSet.ChildSets;
            newTextureSet.GroupName = textureSet.GroupName;
            newTextureSet.TextureSetName = textureSet.TextureSetName;
            newTextureSet.InvertNormalGeneration = textureSet.InvertNormalGeneration;
            newTextureSet.IgnoreNormalGeneration = textureSet.IgnoreNormalGeneration;
            newTextureSet.IgnoreMaskGeneration = textureSet.IgnoreMaskGeneration;
            newTextureSet.OmniExportMode = textureSet.OmniExportMode;
            textureList.Items.Add(newTextureSet);
            textureList.SelectedIndex = textureList.Items.Count - 1;
        }

        private void bulkNameReplacement_Click(object sender, EventArgs e) {
            BulkNameReplacement bulkNameReplacement = new BulkNameReplacement(textureList.Items.Cast<TextureSet>().ToArray());
            if (bulkNameReplacement.ShowDialog() == DialogResult.OK) {
                MessageBox.Show("Replacement Operation Succeeded!", VersionText);
                RefreshList();
            }
        }

        private void hairBaseToFFXIVHairMapsToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Texture File|*.png;*.tga;*.dds;*.bmp;*.tex;";
            MessageBox.Show("Please select input texture");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                ImageManipulation.HairBaseToHairMaps(openFileDialog.FileName);
                MessageBox.Show("Hair Base Converted To FFXIV Maps!", VersionText);
            }
        }

        private void legacyHairMapsToDawntrailHairMapsToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Texture File|*.png;*.tga;*.dds;*.bmp;*.tex;";
            MessageBox.Show("Please select legacy hair multi.");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                string multiPath = openFileDialog.FileName;
                TexIO.SaveBitmap(ImageManipulation.LegacyHairMultiToDawntrailMulti(TexIO.ResolveBitmap(multiPath)),
                ImageManipulation.ReplaceExtension(
                ImageManipulation.AddSuffix(multiPath, "_dawntrail"), ".png"));

                MessageBox.Show("Please select legacy hair normal.");
                if (openFileDialog.ShowDialog() == DialogResult.OK) {
                    TexIO.SaveBitmap(ImageManipulation.LegacyHairNormalToDawntrailNormal(
                    TexIO.ResolveBitmap(multiPath),
                    TexIO.ResolveBitmap(openFileDialog.FileName)),
                    ImageManipulation.ReplaceExtension(
                    ImageManipulation.AddSuffix(openFileDialog.FileName, "_dawntrail"), ".png"));
                    MessageBox.Show("Hair Maps Converted To FFXIV Maps!", VersionText);
                }
            }
        }


        private void convertBaseToNormalAndMultiToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Texture File|*.png;*.tga;*.dds;*.bmp;*.tex;";
            MessageBox.Show("Please select input texture");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                ImageManipulation.ClothingBaseToClothingMultiAndNormalMaps(openFileDialog.FileName);
                MessageBox.Show("Clothing Base Converted To FFXIV Maps!", VersionText);
            }
        }

        public void legacyMakeupSalvagerToolStripMenuItem_Click(object sender, EventArgs e) {
            new LegacyMakeupSalvager().Show();
        }

        private void bulkDDSToPNGToolStripMenuItem_Click(object sender, EventArgs e) {
            MessageBox.Show("Please select a folder with .dds images");
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK) {
                ImageManipulation.BulkDDSToPng(Directory.EnumerateFiles(folderBrowserDialog.SelectedPath));
                MessageBox.Show("DDS converted to PNG.", VersionText);
            }
        }

        private void textureToTexToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Texture File|*.png;*.tga;*.dds;*.bmp;*.tex;";
            MessageBox.Show("Please select input texture to convert to .tex");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                textureProcessor.ExportTex(openFileDialog.FileName, ImageManipulation.ReplaceExtension(openFileDialog.FileName, ".tex"));
                MessageBox.Show("Texture converted to TEX.", VersionText);
            }
        }

        private void currentEditLabel_Click(object sender, EventArgs e) {

        }

        private void textureSetName_TextChanged(object sender, EventArgs e) {
            if (textureList.SelectedIndex > -1) {
                TextureSet item = textureList.Items[textureList.SelectedIndex] as TextureSet;
                if (item != null) {
                    item.TextureSetName = textureSetName.Text;
                }
                textureSetNameRefreshTimer.Stop();
                textureSetNameRefreshTimer.Start();
            }
        }

        private void textureSetName_Leave(object sender, EventArgs e) {
            int index = textureList.SelectedIndex;
            RefreshList();
        }

        private void textureSetName_KeyUp(object sender, KeyEventArgs e) {

        }

        private void textureSetNameRefreshTimer_Tick(object sender, EventArgs e) {
            textureSetNameRefreshTimer.Stop();
            textureList.Focus();
        }

        private void textureSetName_MouseMove(object sender, MouseEventArgs e) {
        }

        private void autoPrepareNormalMapsFromTexToolsDumpToolStripMenuItem_Click(object sender, EventArgs e) {
            string paths = "";
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK) {
                foreach (string directory in Directory.EnumerateDirectories(folderBrowserDialog.SelectedPath)) {
                    string directoryName = Path.GetFileNameWithoutExtension(directory).Replace("Hyur_", null);
                    string[] splitString = directoryName.Split("_");
                    string race = splitString[0];
                    string gender = splitString[1].Replace("Female", "feminine").Replace("Male", "masculine");
                    foreach (string subDirectory in Directory.EnumerateDirectories(directory)) {
                        foreach (string file in Directory.EnumerateFiles(subDirectory)) {
                            if (file.Contains("_norm") && !file.Contains("eye") && !file.Contains("etc")) {
                                //try {
                                string faceNumber = Path.GetFileNameWithoutExtension(file).Replace("000", "").Replace("010", "").Replace("_", "").Split("f")[1];
                                string outputTexture = Path.Combine(GlobalPathStorage.OriginalBaseDirectory, @"res\textures\face\"
                                + gender + @"\" + race.ToLower() + @"\" + faceNumber + "n.png");
                                Directory.CreateDirectory(Path.GetDirectoryName(outputTexture));
                                TexIO.SaveBitmap(TexIO.ResolveBitmap(file), outputTexture);
                                paths += outputTexture + "\r\n";
                                //} catch { }
                            }
                        }
                    }
                }
                MessageBox.Show("Operation Completed Successfully");
            }
        }

        private void autoFinalizeNormalMapsFromTexToolsDumpToolStripMenuItem_Click(object sender, EventArgs e) {
            string paths = "";
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK) {
                foreach (string directory in Directory.EnumerateDirectories(folderBrowserDialog.SelectedPath)) {
                    string directoryName = Path.GetFileNameWithoutExtension(directory);
                    string[] splitString = directoryName.Split("_");
                    string race = splitString[0];
                    string gender = splitString[1].Replace("Female", "feminine").Replace("Male", "masculine");
                    foreach (string subDirectory in Directory.EnumerateDirectories(directory)) {
                        foreach (string faceDirectory in Directory.EnumerateDirectories(subDirectory)) {
                            foreach (string file in Directory.EnumerateFiles(faceDirectory)) {
                                if (file.Contains("_norm") && !file.Contains("eye") && !file.Contains("etc")) {
                                    try {
                                        string faceNumber = Path.GetFileNameWithoutExtension(file).Replace("000", "").Replace("010", "").Replace("_", "").Split("f")[1];
                                        string outputTexture = Path.Combine(GlobalPathStorage.OriginalBaseDirectory, @"res\textures\face\"
                                        + gender + @"\" + race.ToLower() + @"\" + faceNumber + "n.png");
                                        Directory.CreateDirectory(Path.GetDirectoryName(outputTexture));
                                        TexIO.SaveBitmap(TexIO.ResolveBitmap(file), outputTexture);
                                        paths += outputTexture + "\r\n";
                                    } catch { }
                                }
                            }
                        }
                    }
                }
                MessageBox.Show(paths);
            }
        }

        private void genderList_SelectedIndexChanged(object sender, EventArgs e) {
            switch (baseBodyList.SelectedIndex) {
                case 1:
                case 2:
                    if (genderList.SelectedIndex != 1) {
                        baseBodyList.SelectedIndex = 0;
                    }
                    break;
                case 4:
                    if (genderList.SelectedIndex != 1) {
                        baseBodyList.SelectedIndex = 0;
                    }
                    break;
                case 3:
                    if (genderList.SelectedIndex != 0) {
                        baseBodyList.SelectedIndex = 0;
                    }
                    break;
            }
        }

        private void baseTextureToNormalMapToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Texture File|*.png;*.tga;*.dds;*.bmp;*.tex;";
            MessageBox.Show("Please select input texture");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                ImageManipulation.BaseToNormaMap(openFileDialog.FileName);
                MessageBox.Show("Base converted to normal map!", VersionText);
            }
        }

        private void baseTextureToInvertedNormalMapToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Texture File|*.png;*.tga;*.dds;*.bmp;*.tex;";
            MessageBox.Show("Please select input texture");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                ImageManipulation.BaseToInvertedNormaMap(openFileDialog.FileName);
                MessageBox.Show("Base converted to inverted normal map!", VersionText);
            }
        }

        private void colourChannelSplittingToolStripMenuItem_Click(object sender, EventArgs e) {

        }

        private void generateMapsForDawntrailEyeDiffuseToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Texture File|*.png;*.tga;*.dds;*.bmp;*.tex;";
            MessageBox.Show("Please select input texture");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                ImageManipulation.ConvertImageToEyeMapsDawntrail(openFileDialog.FileName, false);
                MessageBox.Show("Image successfully converted to eye maps", VersionText);
                AutoModPackingPrompt(openFileDialog.FileName);
            }
        }

        private void fullTattooToOverlayToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Texture File|*.png;*.tga;*.dds;*.bmp;*.tex;";
            MessageBox.Show("Please select input texture");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                TexIO.SaveBitmap(ImageManipulation.SeperateTattoo(TexIO.ResolveBitmap(openFileDialog.FileName)),
                ImageManipulation.AddSuffix(openFileDialog.FileName, "_separated"));
                MessageBox.Show("Tattoo has attempted to be separated.", VersionText);
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
        private void fullTattooToOverlayBodyToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Texture File|*.png;*.tga;*.dds;*.bmp;*.tex;";
            MessageBox.Show("Please select input texture");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                foreach (var skin in BackupTexturePaths.BiboSkinTypes) {
                    TexIO.SaveBitmap(ImageManipulation.SeperateTattooByDifference(TexIO.ResolveBitmap(openFileDialog.FileName), skin, null, openFileDialog.FileName.ToLower().Contains("raen")),
                    ImageManipulation.AddSuffix(openFileDialog.FileName, "_" + skin.Name + "_separated"));
                }
                MessageBox.Show("Tattoo has attempted to be separated.", VersionText);
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

        private void seperateTextureByDifferenceToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Texture File|*.png;*.tga;*.dds;*.bmp;*.tex;";
            OpenFileDialog openFileDialog2 = new OpenFileDialog();
            openFileDialog2.Filter = "Texture File|*.png;*.tga;*.dds;*.bmp;*.tex;";
            MessageBox.Show("Please select an input texture with the element you want to separate.");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                MessageBox.Show("Please select an input texture that has the same underlying texture but without the element you want to separate.");
                if (openFileDialog2.ShowDialog() == DialogResult.OK) {
                    TexIO.SaveBitmap(ImageManipulation.SeperateByDifference(
                    TexIO.ResolveBitmap(openFileDialog.FileName), TexIO.ResolveBitmap(openFileDialog2.FileName)),
                    ImageManipulation.AddSuffix(openFileDialog.FileName, "_separated"));
                    MessageBox.Show("Texture has attempted to be separated.", VersionText);
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
        }

        private void createMapsForFolderOfExistingDawntrailEyeMapsToolStripMenuItem_Click(object sender, EventArgs e) {
            MessageBox.Show("Pick folder of dawntrail eye maps to generate missing maps for.\r\nWARNING: This process may take a while if the folder has lots of eye textures.", VersionText);
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            int maxItems = 0;
            int itemsCounted = 0;
            int calculatingItems = 0;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK) {
                foreach (string file in Directory.EnumerateFiles(folderBrowserDialog.SelectedPath, "*.*", SearchOption.AllDirectories)
                .Where(s => s.EndsWith(".png") || s.EndsWith(".bmp") || s.EndsWith(".dds") || s.EndsWith(".tex"))) {
                    if (file.Contains("_base") && !file.Contains("_norm") && !file.Contains("_mask")) {
                        maxItems++;
                        while (calculatingItems > Environment.ProcessorCount) {
                            Thread.Sleep(3000);
                        }
                        calculatingItems++;
                        Task.Run(() => {
                            try {
                                ImageManipulation.ConvertImageToEyeMapsDawntrail(file, false);
                            } catch {

                            }
                            itemsCounted++;
                            calculatingItems--;
                        });
                    }
                }
                while (itemsCounted < maxItems) {
                    Thread.Sleep(5000);
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

        private void convertFolderToEyeMapsToolStripMenuItem1_Click(object sender, EventArgs e) {

        }

        private void autoAssembleAndExportEyeModsFromFolderToolStripMenuItem_Click(object sender, EventArgs e) {
            MessageBox.Show("Pick folder of eye maps to auto assemble.\r\nWARNING: This process may take a while if the folder has lots of eye textures.", VersionText);
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK) {
                var items = Directory.EnumerateFiles(folderBrowserDialog.SelectedPath, "*.*", SearchOption.AllDirectories)
                .Where(s => s.EndsWith(".png") || s.EndsWith(".bmp") || s.EndsWith(".dds") || s.EndsWith(".tga") || s.EndsWith(".tex"));
                modPath = Path.Combine(penumbraModPath, modNameTextBox.Text);
                string modPackPackageFolder = Path.Combine(folderBrowserDialog.SelectedPath, "Mod Packs");
                ExportEyePackList(items, modPackPackageFolder);
            }
        }
        public void ExportEyePackList(IEnumerable<string> files, string modPackPackageFolder) {
            Dictionary<string, TextureSet> keyValuePairs = new Dictionary<string, TextureSet>();
            string templatePath = Path.Combine(GlobalPathStorage.OriginalBaseDirectory, @"res\templates");
            foreach (string file in files) {
                if (file.Contains("_eye")) {
                    try {
                        string cleanedName = Path.GetFileNameWithoutExtension(file)
                        .Replace("_eye", "").Replace("_base", "").Replace("_norm", "").Replace("_mask", "");
                        if (!keyValuePairs.ContainsKey(cleanedName)) {
                            keyValuePairs[cleanedName] = new TextureSet();
                        }
                        if (file.Contains("_base")) {
                            keyValuePairs[cleanedName].Base = file;
                        }
                        if (file.Contains("_norm")) {
                            keyValuePairs[cleanedName].Normal = file;
                        }
                        if (file.Contains("_mask")) {
                            keyValuePairs[cleanedName].Mask = file;
                        }
                    } catch (Exception weee) {
                        MessageBox.Show(weee.Message, "Error");
                    }
                }
            }
            Task.Run(() => {
                foreach (var item in keyValuePairs) {
                    Thread.Sleep(500);
                    bool executed = false;
                    string modName1 = item.Key + " Eye Mod";
                    string modName2 = item.Key + " Right Eye Mod";
                    string modPath1 = "";
                    string modPath2 = "";
                    generateButton.Invoke(() => {
                        try {
                            NewProject();
                            OpenLoadTemplate(templatePath + "\\" + "- Eye Pack Template.ffxivtp", "Symmetrical Eyes And Left Eye", item.Value);
                            OpenLoadTemplate(templatePath + "\\" + "- Eye Pack Template.ffxivtp", "Right Eye", item.Value);
                            foreach (TextureSet textureSet in textureList.Items) {
                                if (textureSet.GroupName == "Right Eye") {
                                    textureSet.InternalBasePath = textureSet.InternalBasePath.Replace("_", "_b_");
                                    textureSet.InternalNormalPath = textureSet.InternalNormalPath.Replace("_", "_b_");
                                    textureSet.InternalMaskPath = textureSet.InternalMaskPath.Replace("_", "_b_");
                                    textureSet.InternalMaterialPath = textureSet.InternalMaterialPath.Replace("_a", "_b");
                                }
                                textureSet.Glow = "c:\\dontRemoveButYouCanReplace.png";
                            }
                            generationType.SelectedIndex = 3;
                            choiceTypeIndex = 3;
                            modNameTextBox.Text = modName1;
                            generateButton_Click(this, EventArgs.Empty);
                            modPath1 = modPath;
                            executed = true;
                        } catch (Exception e) {
                            MessageBox.Show(e.Message, "Error");
                        }
                    });
                    while (lockDuplicateGeneration) {
                        Thread.Sleep(2000);
                    }
                    Task.Run(() => {
                        Directory.CreateDirectory(modPackPackageFolder);
                        string packagingFolder = Path.Combine(modPackPackageFolder, modName1);
                        string path1 = Path.Combine(modPackPackageFolder, modName1 + ".pmp");
                        if (File.Exists(path1)) {
                            File.Delete(path1);
                        }
                        ZipFile.CreateFromDirectory(modPath1, path1);
                    });
                }
                generateButton.Invoke(() => {
                    MessageBox.Show("Your .pmp archives have been exported, but are also already in Penumbra.", VersionText);
                    try {
                        Process.Start(new System.Diagnostics.ProcessStartInfo() {
                            FileName = modPackPackageFolder,
                            UseShellExecute = true,
                            Verb = "OPEN"
                        });
                    } catch {

                    }
                });
            }
            );
        }

        public void ExportContactPackList(IEnumerable<string> files, string modPackPackageFolder) {
            Dictionary<string, TextureSet> keyValuePairs = new Dictionary<string, TextureSet>();
            string templatePath = Path.Combine(GlobalPathStorage.OriginalBaseDirectory, @"res\templates");
            foreach (string file in files) {
                if (file.Contains("_contactBase")) {
                    try {
                        string cleanedName = Path.GetFileNameWithoutExtension(file)
                        .Replace("_contactBase", "");
                        if (!keyValuePairs.ContainsKey(cleanedName)) {
                            keyValuePairs[cleanedName] = new TextureSet();
                        }
                        if (file.Contains("_contactBase")) {
                            keyValuePairs[cleanedName].Base = file;
                        }
                    } catch (Exception weee) {
                        MessageBox.Show(weee.Message, "Error");
                    }
                }
            }
            Task.Run(() => {
                foreach (var item in keyValuePairs) {
                    Thread.Sleep(500);
                    bool executed = false;
                    string modName1 = item.Key + " Contact Mod";
                    string modPath1 = "";
                    generateButton.Invoke(() => {
                        try {
                            NewProject();
                            OpenLoadTemplate(templatePath + "\\" + "- Animated Contact Lense.ffxivtp", "Default", item.Value);
                            generationType.SelectedIndex = 3;
                            choiceTypeIndex = 3;
                            modNameTextBox.Text = modName1;
                            generateButton_Click(this, EventArgs.Empty);
                            modPath1 = modPath;
                            executed = true;
                        } catch (Exception e) {
                            MessageBox.Show(e.Message, "Error");
                        }
                    });
                    while (lockDuplicateGeneration) {
                        Thread.Sleep(2000);
                    }
                    Task.Run(() => {
                        Directory.CreateDirectory(modPackPackageFolder);
                        string packagingFolder = Path.Combine(modPackPackageFolder, modName1);
                        string path1 = Path.Combine(modPackPackageFolder, modName1 + ".pmp");
                        if (File.Exists(path1)) {
                            File.Delete(path1);
                        }
                        ZipFile.CreateFromDirectory(modPath1, path1);
                    });
                }
                generateButton.Invoke(() => {
                    MessageBox.Show("Your .pmp archives have been exported, but are also already in Penumbra.", VersionText);
                    try {
                        Process.Start(new System.Diagnostics.ProcessStartInfo() {
                            FileName = modPackPackageFolder,
                            UseShellExecute = true,
                            Verb = "OPEN"
                        });
                    } catch {

                    }
                });
            }
            );
        }

        private void createAnimatedContactLensesToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Texture File|*.png;*.tga;*.dds;*.bmp;*.tex;";
            MessageBox.Show("Please select input texture");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                ImageManipulation.CreateContact(GlobalPathStorage.OriginalBaseDirectory, openFileDialog.FileName);
                MessageBox.Show("Image successfully converted to contact maps.", VersionText);
                AutoModPackingPromptContacts(openFileDialog.FileName);
            }
        }

        private void convertFolderOfGenericEyeTexturesToAnimatedContactLensesToolStripMenuItem_Click(object sender, EventArgs e) {
            MessageBox.Show("Pick folder of generic eye textures to generate maps for.\r\nWARNING: This process may take a while if the folder has lots of eye textures.", VersionText);
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            int maxItems = 0;
            int itemsCounted = 0;
            int calculatingItems = 0;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK) {
                foreach (string file in Directory.EnumerateFiles(folderBrowserDialog.SelectedPath, "*.*", SearchOption.AllDirectories)
                .Where(s => s.EndsWith(".png") || s.EndsWith(".bmp") || s.EndsWith(".dds") || s.EndsWith(".tex"))) {
                    if (!file.Contains("_contactBase") && !file.Contains("_base") && !file.Contains("_norm") && !file.Contains("_mask")) {
                        maxItems++;
                        while (calculatingItems > Environment.ProcessorCount) {
                            Thread.Sleep(3000);
                        }
                        calculatingItems++;
                        Task.Run(() => {
                            try {
                                ImageManipulation.CreateContact(GlobalPathStorage.OriginalBaseDirectory, file);
                            } catch {

                            }
                            itemsCounted++;
                            calculatingItems--;
                        });
                    }
                }
                while (itemsCounted < maxItems) {
                    Thread.Sleep(5000);
                }
                MessageBox.Show("Images successfully converted to contact maps", VersionText);
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

        private void autoAssembleAndExportEyeContactModsFromFolderToolStripMenuItem1_Click(object sender, EventArgs e) {
            MessageBox.Show("Pick folder of eye maps to auto assemble.\r\nWARNING: This process may take a while if the folder has lots of eye textures.", VersionText);
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK) {
                var items = Directory.EnumerateFiles(folderBrowserDialog.SelectedPath, "*.*", SearchOption.AllDirectories)
                .Where(s => s.EndsWith(".png") || s.EndsWith(".bmp") || s.EndsWith(".dds") || s.EndsWith(".tga") || s.EndsWith(".tex"));
                modPath = Path.Combine(penumbraModPath, modNameTextBox.Text);
                string modPackPackageFolder = Path.Combine(folderBrowserDialog.SelectedPath, "Mod Packs");
                ExportContactPackList(items, modPackPackageFolder);
            }
        }

        private void label8_Click(object sender, EventArgs e) {

        }

        private void Base_Load(object sender, EventArgs e) {

        }

        private void layerBaseButton_Click(object sender, EventArgs e) {
            TextureSet textureSet = textureList.Items[textureList.SelectedIndex] as TextureSet;
            OverlaySelector overlaySelector = new OverlaySelector();
            overlaySelector.LayeredImages = textureSet.BaseOverlays;
            overlaySelector.OnSelectedEventHandler = multi_OnFileSelected;
            overlaySelector.ShowDialog();
        }

        private void layerNormalButton_Click(object sender, EventArgs e) {
            TextureSet textureSet = textureList.Items[textureList.SelectedIndex] as TextureSet;
            OverlaySelector overlaySelector = new OverlaySelector();
            overlaySelector.LayeredImages = textureSet.NormalOverlays;
            overlaySelector.ShowDialog();
        }

        private void layersMaskButton_Click(object sender, EventArgs e) {
            TextureSet textureSet = textureList.Items[textureList.SelectedIndex] as TextureSet;
            OverlaySelector overlaySelector = new OverlaySelector();
            overlaySelector.LayeredImages = textureSet.MaskOverlays;
            overlaySelector.ShowDialog();
        }

        private void tail1FemaleToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Texture File|*.png;*.tga;*.dds;*.bmp;*.tex;";
            MessageBox.Show("Please select input texture");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                ImageManipulation.ConvertLegacyAuRaTail(openFileDialog.FileName, 1, true);
                MessageBox.Show("Tail texture successfully converted!", VersionText);
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

        private void tail2FemaleToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Texture File|*.png;*.tga;*.dds;*.bmp;*.tex;";
            MessageBox.Show("Please select input texture");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                ImageManipulation.ConvertLegacyAuRaTail(openFileDialog.FileName, 2, true);
                MessageBox.Show("Tail texture successfully converted!", VersionText);
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

        private void tail3FemaleToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Texture File|*.png;*.tga;*.dds;*.bmp;*.tex;";
            MessageBox.Show("Please select input texture");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                ImageManipulation.ConvertLegacyAuRaTail(openFileDialog.FileName, 3, true);
                MessageBox.Show("Tail texture successfully converted!", VersionText);
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

        private void tail4FemaleToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Texture File|*.png;*.tga;*.dds;*.bmp;*.tex;";
            MessageBox.Show("Please select input texture");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                ImageManipulation.ConvertLegacyAuRaTail(openFileDialog.FileName, 4, true);
                MessageBox.Show("Tail texture successfully converted!", VersionText);
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

        private void tail1MaleToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Texture File|*.png;*.tga;*.dds;*.bmp;*.tex;";
            MessageBox.Show("Please select input texture");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                ImageManipulation.ConvertLegacyAuRaTail(openFileDialog.FileName, 1, false);
                MessageBox.Show("Tail texture successfully converted!", VersionText);
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

        private void tail2MaleToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Texture File|*.png;*.tga;*.dds;*.bmp;*.tex;";
            MessageBox.Show("Please select input texture");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                ImageManipulation.ConvertLegacyAuRaTail(openFileDialog.FileName, 2, false);
                MessageBox.Show("Tail texture successfully converted!", VersionText);
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

        private void tail3MaleToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Texture File|*.png;*.tga;*.dds;*.bmp;*.tex;";
            MessageBox.Show("Please select input texture");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                ImageManipulation.ConvertLegacyAuRaTail(openFileDialog.FileName, 3, false);
                MessageBox.Show("Tail texture successfully converted!", VersionText);
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

        private void tail4MaleToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Texture File|*.png;*.tga;*.dds;*.bmp;*.tex;";
            MessageBox.Show("Please select input texture");
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                ImageManipulation.ConvertLegacyAuRaTail(openFileDialog.FileName, 4, false);
                MessageBox.Show("Tail texture successfully converted!", VersionText);
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

        private void eyeToolsToolStripMenuItem_Click(object sender, EventArgs e) {

        }
    }
}