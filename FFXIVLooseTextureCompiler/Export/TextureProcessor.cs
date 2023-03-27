using FFXIVLooseTextureCompiler.Export;
using FFXIVLooseTextureCompiler.ImageProcessing;
using FFXIVLooseTextureCompiler.PathOrganization;
using FFXIVVoicePackCreator.Json;
using Newtonsoft.Json;
using Penumbra.Import.Dds;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FFXIVLooseTextureCompiler.MainWindow;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace FFXIVLooseTextureCompiler {
    public class TextureProcessor {
        private Dictionary<string, Bitmap> normalCache;
        private Dictionary<string, Bitmap> multiCache;
        private Dictionary<string, string> xnormalCache;
        private XNormal xnormal;
        private List<KeyValuePair<string, string>> textureSetQueue;
        private int fileCount;

        BackupTexturePaths biboPath = new BackupTexturePaths(@"res\textures\bibo\bibo\");
        BackupTexturePaths biboGen3Path = new BackupTexturePaths(@"res\textures\bibo\gen3\");
        BackupTexturePaths biboGen2Path = new BackupTexturePaths(@"res\textures\bibo\gen2\");

        BackupTexturePaths gen3BiboPath = new BackupTexturePaths(@"res\textures\gen3\bibo\");
        BackupTexturePaths gen3Path = new BackupTexturePaths(@"res\textures\gen3\gen3\");
        BackupTexturePaths gen3Gen2Path = new BackupTexturePaths(@"res\textures\gen3\gen2\");

        BackupTexturePaths tbsePath = new BackupTexturePaths(@"res\textures\tbse\tbse\");
        BackupTexturePaths tbsePathHighlander = new BackupTexturePaths(@"res\textures\tbse\highlander\");
        BackupTexturePaths tbsePathViera = new BackupTexturePaths(@"res\textures\tbse\viera\");

        BackupTexturePaths otopopLalaPath = new BackupTexturePaths(@"res\textures\otopop\otopop\");
        BackupTexturePaths vanillaLalaPath = new BackupTexturePaths(@"res\textures\otopop\vanilla\");

        private bool finalizeResults;
        private bool generateNormals;
        private bool generateMulti;

        public BackupTexturePaths BiboPath { get => biboPath; set => biboPath = value; }
        public BackupTexturePaths BiboGen3Path { get => biboGen3Path; set => biboGen3Path = value; }
        public BackupTexturePaths BiboGen2Path { get => biboGen2Path; set => biboGen2Path = value; }
        public BackupTexturePaths Gen3BiboPath { get => gen3BiboPath; set => gen3BiboPath = value; }
        public BackupTexturePaths Gen3Path { get => gen3Path; set => gen3Path = value; }
        public BackupTexturePaths Gen3Gen2Path { get => gen3Gen2Path; set => gen3Gen2Path = value; }
        public BackupTexturePaths OtopopLalaPath { get => otopopLalaPath; set => otopopLalaPath = value; }

        public BackupTexturePaths TbsePath { get => tbsePath; set => tbsePath = value; }
        public BackupTexturePaths VanillaLalaPath { get => vanillaLalaPath; set => vanillaLalaPath = value; }
        public BackupTexturePaths TbsePathHighlander { get => tbsePathHighlander; set => tbsePathHighlander = value; }
        public BackupTexturePaths TbsePathViera { get => tbsePathViera; set => tbsePathViera = value; }

        public event EventHandler OnProgressChange;
        public event EventHandler OnStartedProcessing;
        public event EventHandler OnLaunchedXnormal;

        Bitmap GetMergedBitmap(string file) {
            if (file.Contains("baseTexBaked") && (file.Contains("_d_") || file.Contains("_g_"))) {
                Bitmap alpha = TexLoader.ResolveBitmap(file.Replace("baseTexBaked", "alpha_baseTexBaked"));
                Bitmap rgb = TexLoader.ResolveBitmap(file.Replace("baseTexBaked", "rgb_baseTexBaked"));
                Bitmap merged = ImageManipulation.MergeAlphaToRRGB(alpha, rgb);
                merged.Save(file, ImageFormat.Png);
                return merged;
            } else {
                return TexLoader.ResolveBitmap(file);
            }
        }

        public void BatchTextureSet(TextureSet parent, TextureSet child) {
            if (!string.IsNullOrEmpty(child.Diffuse)) {
                if (!xnormalCache.ContainsKey(child.Diffuse)) {
                    string diffuseAlpha = parent.Diffuse.Replace(".", "_alpha.");
                    string diffuseRGB = parent.Diffuse.Replace(".", "_rgb.");
                    if (finalizeResults || !File.Exists(child.Diffuse.Replace("baseTexBaked", "rgb_baseTexBaked"))
                        || !File.Exists(child.Diffuse.Replace("baseTexBaked", "alpha_baseTexBaked"))) {
                        if (child.Diffuse.Contains("baseTexBaked")) {
                            xnormalCache.Add(child.Diffuse, child.Diffuse);
                            Bitmap diffuse = TexLoader.ResolveBitmap(parent.Diffuse);
                            ImageManipulation.ExtractTransparency(diffuse).Save(diffuseAlpha, ImageFormat.Png);
                            ImageManipulation.ExtractRGB(diffuse).Save(diffuseRGB, ImageFormat.Png);
                            xnormal.AddToBatch(parent.InternalDiffusePath, diffuseAlpha, child.Diffuse.Replace("baseTexBaked", "alpha"));
                            xnormal.AddToBatch(parent.InternalDiffusePath, diffuseRGB, child.Diffuse.Replace("baseTexBaked", "rgb"));
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(child.Normal)) {
                if (!xnormalCache.ContainsKey(child.Normal)) {
                    if (finalizeResults || !File.Exists(child.Normal)) {
                        if (child.Normal.Contains("baseTexBaked")) {
                            if (child.BackupTexturePaths != null) {
                                child.Normal = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, child.BackupTexturePaths.Normal);
                            }
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(child.Multi)) {
                if (!xnormalCache.ContainsKey(child.Multi)) {
                    if (finalizeResults || !File.Exists(child.Multi)) {
                        if (child.Multi.Contains("baseTexBaked")) {
                            xnormalCache.Add(child.Multi, child.Multi);
                            xnormal.AddToBatch(parent.InternalMultiPath, parent.Multi, child.Multi);
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(child.Glow)) {
                if (!xnormalCache.ContainsKey(child.Glow)) {
                    string diffuseAlpha = parent.Glow.Replace(".", "_alpha.");
                    string diffuseRGB = parent.Glow.Replace(".", "_rgb.");
                    if (finalizeResults || !File.Exists(child.Glow.Replace("baseTexBaked", "rgb_baseTexBaked"))
                        || !File.Exists(child.Glow.Replace("baseTexBaked", "alpha_baseTexBaked"))) {
                        if (child.Glow.Contains("baseTexBaked")) {
                            xnormalCache.Add(child.Glow, child.Glow);
                            Bitmap diffuse = TexLoader.ResolveBitmap(parent.Glow);
                            ImageManipulation.ExtractTransparency(diffuse).Save(diffuseAlpha, ImageFormat.Png);
                            ImageManipulation.ExtractRGB(diffuse).Save(diffuseRGB, ImageFormat.Png);
                            xnormal.AddToBatch(parent.InternalDiffusePath, diffuseAlpha, child.Glow.Replace("baseTexBaked", "alpha"));
                            xnormal.AddToBatch(parent.InternalDiffusePath, diffuseRGB, child.Glow.Replace("baseTexBaked", "rgb"));
                        }
                    }
                }
            }
        }
        public void Export(List<TextureSet> textureSetList, string modPath, int generationType,
            bool generateNormals, bool generateMulti, bool useXNormal) {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            int i = 0;
            fileCount = 0;
            finalizeResults = useXNormal;
            Dictionary<string, List<TextureSet>> groups = new Dictionary<string, List<TextureSet>>();
            normalCache = new Dictionary<string, Bitmap>();
            multiCache = new Dictionary<string, Bitmap>();
            xnormalCache = new Dictionary<string, string>();
            xnormal = new XNormal();
            this.generateNormals = generateNormals;
            this.generateMulti = generateMulti;
            foreach (TextureSet textureSet in textureSetList) {
                if (!groups.ContainsKey(textureSet.MaterialGroupName)) {
                    groups.Add(textureSet.MaterialGroupName, new List<TextureSet>() { textureSet });
                    foreach (TextureSet childSet in textureSet.ChildSets) {
                        childSet.MaterialGroupName = textureSet.MaterialGroupName;
                        groups[textureSet.MaterialGroupName].Add(childSet);
                        BatchTextureSet(textureSet, childSet);
                    }
                } else {
                    groups[textureSet.MaterialGroupName].Add(textureSet);
                    foreach (TextureSet childSet in textureSet.ChildSets) {
                        childSet.MaterialGroupName = textureSet.MaterialGroupName;
                        groups[textureSet.MaterialGroupName].Add(childSet);
                        BatchTextureSet(textureSet, childSet);
                    }
                }
            }
            if (OnLaunchedXnormal != null) {
                OnLaunchedXnormal.Invoke(this, EventArgs.Empty);
            }
            xnormal.ProcessBatches();
            if (OnStartedProcessing != null) {
                OnStartedProcessing.Invoke(this, EventArgs.Empty);
            }
            foreach (List<TextureSet> textureSets in groups.Values) {
                Group group = new Group(textureSets[0].MaterialGroupName.Replace(@"/", "-").Replace(@"\", "-"), "", 0, "Multi", 0);
                Option option = null;
                foreach (TextureSet textureSet in textureSets) {
                    string diffuseDiskPath = !string.IsNullOrEmpty(textureSet.InternalDiffusePath) ?
                        Path.Combine(modPath, textureSet.InternalDiffusePath.Replace("/", @"\")) : "";
                    string normalDiskPath = !string.IsNullOrEmpty(textureSet.InternalNormalPath) ?
                        Path.Combine(modPath, textureSet.InternalNormalPath.Replace("/", @"\")) : "";
                    string multiDiskPath = !string.IsNullOrEmpty(textureSet.InternalMultiPath) ?
                        Path.Combine(modPath, textureSet.InternalMultiPath.Replace("/", @"\")) : "";
                    switch (generationType) {
                        case 0:
                            if (!string.IsNullOrEmpty(textureSet.Diffuse) && !string.IsNullOrEmpty(textureSet.InternalDiffusePath)) {
                                if (DiffuseLogic(textureSet, diffuseDiskPath)) {
                                    option = new Option((textureSets.Count > 1 ? textureSet.MaterialSetName + " " : "")
                                        + (textureSet.MaterialSetName.ToLower().Contains("eyes") ? "Normal" : "Diffuse"), 0);
                                    option.Files.Add(textureSet.InternalDiffusePath, AppendNumber(textureSet.InternalDiffusePath.Replace("/", @"\"),
                                        fileCount++));
                                    group.Options.Add(option);
                                }
                            }
                            if (OnProgressChange != null) {
                                OnProgressChange.Invoke(this, EventArgs.Empty);
                            }
                            if (!string.IsNullOrEmpty(textureSet.InternalNormalPath)) {
                                if (NormalLogic(textureSet, normalDiskPath)) {
                                    option = new Option((textureSets.Count > 1 ? textureSet.MaterialSetName + " " : "")
                                        + (textureSet.MaterialSetName.ToLower().Contains("eyes") ? "Multi" : "Normal"), 0);
                                    option.Files.Add(textureSet.InternalNormalPath, AppendNumber(textureSet.InternalNormalPath.Replace("/", @"\"),
                                        fileCount++));
                                    group.Options.Add(option);
                                }
                            }
                            if (OnProgressChange != null) {
                                OnProgressChange.Invoke(this, EventArgs.Empty);
                            }
                            Application.DoEvents();
                            if (!string.IsNullOrEmpty(textureSet.InternalMultiPath)) {
                                if (MultiLogic(textureSet, multiDiskPath)) {
                                    option = new Option((textureSets.Count > 1 ? textureSet.MaterialSetName + " " : "") +
                                    (textureSet.MaterialSetName.ToLower().Contains("eyes") ? "Catchlight" : "Multi"), 0);
                                    option.Files.Add(textureSet.InternalMultiPath, AppendNumber(textureSet.InternalMultiPath.Replace("/", @"\"),
                                        fileCount++));
                                    group.Options.Add(option);
                                }
                            }
                            if (OnProgressChange != null) {
                                OnProgressChange.Invoke(this, EventArgs.Empty);
                            }
                            Application.DoEvents();
                            break;
                        case 1:
                            option = new Option(textureSet.MaterialSetName == textureSet.MaterialGroupName ? "Enable"
                                : textureSet.MaterialSetName, 0);
                            group.Options.Add(option);
                            if (!string.IsNullOrEmpty(textureSet.Diffuse) && !string.IsNullOrEmpty(textureSet.InternalDiffusePath)) {
                                if (DiffuseLogic(textureSet, diffuseDiskPath)) {
                                    option.Files.Add(textureSet.InternalDiffusePath,
                                        AppendNumber(textureSet.InternalDiffusePath.Replace("/", @"\"), fileCount++));
                                }
                            }
                            if (OnProgressChange != null) {
                                OnProgressChange.Invoke(this, EventArgs.Empty);
                            }
                            Application.DoEvents();
                            if (!string.IsNullOrEmpty(textureSet.InternalNormalPath)) {
                                if (NormalLogic(textureSet, normalDiskPath)) {
                                    option.Files.Add(textureSet.InternalNormalPath,
                                        AppendNumber(textureSet.InternalNormalPath.Replace("/", @"\"), fileCount++));
                                }
                            }
                            if (OnProgressChange != null) {
                                OnProgressChange.Invoke(this, EventArgs.Empty);
                            }
                            Application.DoEvents();
                            if (!string.IsNullOrEmpty(textureSet.InternalMultiPath)) {
                                if (MultiLogic(textureSet, multiDiskPath)) {
                                    option.Files.Add(textureSet.InternalMultiPath,
                                        AppendNumber(textureSet.InternalMultiPath.Replace("/", @"\"), fileCount++));
                                }
                            }
                            if (OnProgressChange != null) {
                                OnProgressChange.Invoke(this, EventArgs.Empty);
                            }
                            Application.DoEvents();
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
            if (finalizeResults) {
                MessageBox.Show("Export completed in " + stopwatch.Elapsed + "!");
            }
        }
        void ExportPaths(string input, string output, BackupTexturePaths backupTexturePaths) {
            if (output.Contains("_d")) {
                if (File.Exists(input)) {
                    ExportTex(input, output, ExportType.XNormalImport, Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                         backupTexturePaths.Diffuse));
                }
            } else if (output.Contains("_n")) {
                if (File.Exists(input)) {
                    ExportTex(backupTexturePaths.Normal, output,
                        ExportType.MergeNormal, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, input), "");
                } else {
                    ExportTex(backupTexturePaths.Normal, output,
                        ExportType.None, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, input), "");
                }
            } else if (output.Contains("_m") || output.Contains("_s")) {
                if (File.Exists(input)) {
                    ExportTex(input, output, ExportType.XNormalImport);
                }
            }
        }
        private bool MultiLogic(TextureSet textureSet, string multiDiskPath) {
            bool outputGenerated = false;
            if (!string.IsNullOrEmpty(textureSet.Multi) && !string.IsNullOrEmpty(textureSet.InternalMultiPath)) {
                ExportTex(textureSet.Multi, AppendNumber(multiDiskPath, fileCount));
                outputGenerated = true;
            } else if (!string.IsNullOrEmpty(textureSet.Diffuse) && !string.IsNullOrEmpty(textureSet.InternalMultiPath)
                && generateMulti && !(textureSet.MaterialSetName.ToLower().Contains("eyes"))) {
                if (!textureSet.IgnoreMultiGeneration) {
                    ExportTex(textureSet.Diffuse, AppendNumber(multiDiskPath, fileCount), ExportType.MultiFace, "", "", textureSet.BackupTexturePaths != null ? textureSet.BackupTexturePaths.Diffuse : "");
                    outputGenerated = true;
                }
            }
            return outputGenerated;
        }

        private bool NormalLogic(TextureSet textureSet, string normalDiskPath) {
            bool outputGenerated = false;
            if (!string.IsNullOrEmpty(textureSet.Normal) && !string.IsNullOrEmpty(textureSet.InternalNormalPath)) {
                if (generateNormals && !textureSet.MaterialSetName.ToLower().Contains("eyes")) {
                    ExportTex(textureSet.Normal, AppendNumber(normalDiskPath, fileCount), ExportType.MergeNormal, textureSet.Diffuse, textureSet.NormalMask, textureSet.BackupTexturePaths != null ? textureSet.BackupTexturePaths.Diffuse : "", textureSet.NormalCorrection);
                    outputGenerated = true;
                } else {
                    if (!string.IsNullOrEmpty(textureSet.Glow) && (textureSet.MaterialSetName.ToLower().Contains("eyes"))) {
                        ExportTex(textureSet.Normal, AppendNumber(normalDiskPath, fileCount), ExportType.GlowMulti, "",
                            textureSet.Glow);
                        outputGenerated = true;
                    } else {
                        ExportTex(textureSet.Normal, AppendNumber(normalDiskPath, fileCount), ExportType.None);
                        outputGenerated = true;
                    }
                }
            } else if (!string.IsNullOrEmpty(textureSet.Diffuse) && !string.IsNullOrEmpty(textureSet.InternalNormalPath)
            && generateNormals && !(textureSet.MaterialSetName.ToLower().Contains("eyes"))) {
                if (!textureSet.IgnoreNormalGeneration) {
                    if (textureSet.BackupTexturePaths != null) {
                        ExportTex((Path.Combine(AppDomain.CurrentDomain.BaseDirectory, textureSet.BackupTexturePaths.Normal)), AppendNumber(normalDiskPath, fileCount), ExportType.MergeNormal, textureSet.Diffuse, textureSet.NormalMask, textureSet.BackupTexturePaths != null ? textureSet.BackupTexturePaths.Diffuse : "");
                        outputGenerated = true;
                    } else {
                        ExportTex(textureSet.Diffuse, AppendNumber(normalDiskPath, fileCount),
                            ExportType.Normal, "", "", textureSet.BackupTexturePaths != null ? textureSet.BackupTexturePaths.Diffuse : "", textureSet.NormalCorrection);
                        outputGenerated = true;
                    }
                }
            }
            return outputGenerated;
        }

        private bool DiffuseLogic(TextureSet textureSet, string diffuseDiskPath) {
            bool outputGenerated = false;
            if ((textureSet.MaterialSetName.ToLower().Contains("eyes") && generateNormals)) {
                ExportTex(textureSet.Diffuse, AppendNumber(diffuseDiskPath, fileCount), ExportType.Normal,
                    textureSet.Diffuse);
                outputGenerated = true;
            } else {
                if (string.IsNullOrEmpty(textureSet.Glow) || textureSet.MaterialSetName.ToLower().Contains("eyes")) {
                    ExportTex(textureSet.Diffuse, AppendNumber(diffuseDiskPath, fileCount), ExportType.None, "", "",
                        textureSet.BackupTexturePaths != null ? textureSet.BackupTexturePaths.Diffuse : "");
                    outputGenerated = true;
                } else {
                    ExportTex(textureSet.Diffuse, AppendNumber(diffuseDiskPath, fileCount), ExportType.Glow, "",
                        textureSet.Glow, textureSet.BackupTexturePaths != null ? textureSet.BackupTexturePaths.Diffuse : "");
                    outputGenerated = true;
                }
            }
            return outputGenerated;
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
        public enum ExportType {
            None,
            Normal,
            MultiFace,
            MergeNormal,
            Glow,
            GlowMulti,
            XNormalImport
        }
        public void ExportTex(string inputFile, string outputFile, ExportType exportType = ExportType.None,
            string diffuseNormal = "", string mask = "", string layeringImage = "", string normalCorrection = "") {
            byte[] data = new byte[0];
            int contrast = 500;
            int contrastFace = 100;
            using (MemoryStream stream = new MemoryStream()) {
                switch (exportType) {
                    case ExportType.None:
                        using (Bitmap bitmap = TexLoader.ResolveBitmap(inputFile)) {
                            if (bitmap != null) {
                                if (!string.IsNullOrEmpty(layeringImage)) {
                                    Bitmap image = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format32bppArgb);
                                    Bitmap layer = TexLoader.ResolveBitmap(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, layeringImage));
                                    Graphics g = Graphics.FromImage(image);
                                    g.Clear(Color.Transparent);
                                    g.CompositingQuality = CompositingQuality.HighQuality;
                                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                    g.SmoothingMode = SmoothingMode.HighQuality;
                                    g.DrawImage(layer, 0, 0, bitmap.Width, bitmap.Height);
                                    g.DrawImage(GetMergedBitmap(inputFile), 0, 0, bitmap.Width, bitmap.Height);
                                    image.Save(stream, ImageFormat.Png);
                                } else {
                                    bitmap.Save(stream, ImageFormat.Png);
                                }
                            }
                        }
                        break;
                    case ExportType.Glow:
                        using (Bitmap bitmap = TexLoader.ResolveBitmap(inputFile)) {
                            if (bitmap != null) {
                                if (!string.IsNullOrEmpty(layeringImage)) {
                                    Bitmap image = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format32bppArgb);
                                    Bitmap layer = TexLoader.ResolveBitmap(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, layeringImage));
                                    Graphics g = Graphics.FromImage(image);
                                    g.Clear(Color.Transparent);
                                    g.CompositingQuality = CompositingQuality.HighQuality;
                                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                    g.SmoothingMode = SmoothingMode.HighQuality;
                                    g.DrawImage(layer, 0, 0, bitmap.Width, bitmap.Height);
                                    g.DrawImage(GetMergedBitmap(inputFile), 0, 0, bitmap.Width, bitmap.Height);
                                    Bitmap glowBitmap = AtramentumLuminisGlow.CalculateDiffuse(image,
                                        ImageManipulation.Resize(GetMergedBitmap(mask), bitmap.Width, bitmap.Height));
                                    glowBitmap.Save(stream, ImageFormat.Png);
                                } else {
                                    Bitmap glowBitmap = AtramentumLuminisGlow.CalculateDiffuse(bitmap, TexLoader.ResolveBitmap(mask));
                                    glowBitmap.Save(stream, ImageFormat.Png);
                                }
                            }
                        }
                        break;
                    case ExportType.GlowMulti:
                        using (Bitmap bitmap = TexLoader.ResolveBitmap(inputFile)) {
                            if (bitmap != null) {
                                Bitmap glowBitmap = AtramentumLuminisGlow.CalculateMulti(bitmap, TexLoader.ResolveBitmap(mask));
                                glowBitmap.Save(stream, ImageFormat.Png);
                            }
                        }
                        break;
                    case ExportType.Normal:
                        if (!normalCache.ContainsKey(inputFile)) {
                            using (Bitmap bitmap = TexLoader.ResolveBitmap(inputFile)) {
                                if (bitmap != null) {
                                    using (Bitmap target = new Bitmap(bitmap.Size.Width, bitmap.Size.Height, PixelFormat.Format32bppArgb)) {
                                        Graphics g = Graphics.FromImage(target);
                                        g.Clear(Color.Transparent);
                                        g.CompositingQuality = CompositingQuality.HighQuality;
                                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                        g.SmoothingMode = SmoothingMode.HighQuality;
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
                                            normalCache.Add(inputFile, resize);
                                            if (!string.IsNullOrEmpty(normalCorrection)) {
                                                output = ImageManipulation.ResizeAndMerge(resize, TexLoader.ResolveBitmap(normalCorrection));
                                            }
                                            output.Save(stream, ImageFormat.Png);
                                        } else {
                                            normalCache.Add(inputFile, output);
                                            if (!string.IsNullOrEmpty(normalCorrection)) {
                                                output = ImageManipulation.ResizeAndMerge(output, TexLoader.ResolveBitmap(normalCorrection));
                                            }
                                            output.Save(stream, ImageFormat.Png);
                                        }
                                    }
                                }
                            }
                        } else {
                            if (!string.IsNullOrEmpty(normalCorrection)) {
                                ImageManipulation.ResizeAndMerge(normalCache[inputFile], TexLoader.ResolveBitmap(normalCorrection)).Save(stream, ImageFormat.Png); ;
                            } else {
                                normalCache[inputFile].Save(stream, ImageFormat.Png);
                            }
                        }
                        break;
                    case ExportType.MultiFace:
                        if (!multiCache.ContainsKey(inputFile)) {
                            using (Bitmap bitmap = TexLoader.ResolveBitmap(inputFile)) {
                                if (bitmap != null) {
                                    if (layeringImage != null) {
                                        Bitmap image = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format32bppArgb);
                                        Bitmap layer = TexLoader.ResolveBitmap(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, layeringImage));
                                        Graphics g = Graphics.FromImage(image);
                                        g.Clear(Color.Transparent);
                                        g.CompositingQuality = CompositingQuality.HighQuality;
                                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                        g.SmoothingMode = SmoothingMode.HighQuality;
                                        g.DrawImage(layer, 0, 0, bitmap.Width, bitmap.Height);
                                        g.DrawImage(GetMergedBitmap(inputFile), 0, 0, bitmap.Width, bitmap.Height);
                                        Bitmap multi = MultiplyFilter.MultiplyImage(
                                            Brightness.BrightenImage(Grayscale.MakeGrayscale3(image)), 255, 126, 0);
                                        multi.Save(stream, ImageFormat.Png);
                                        multiCache.Add(inputFile, multi);
                                    } else {
                                        Bitmap multi = MultiplyFilter.MultiplyImage(
                                        Brightness.BrightenImage(Grayscale.MakeGrayscale3(bitmap)), 255, 126, 0);
                                        multi.Save(stream, ImageFormat.Png);
                                        multiCache.Add(inputFile, multi);
                                    }
                                }
                            }
                        } else {
                            multiCache[inputFile].Save(stream, ImageFormat.Png);
                        }
                        break;
                    case ExportType.MergeNormal:
                        if (!string.IsNullOrEmpty(diffuseNormal)) {
                            if (!normalCache.ContainsKey(diffuseNormal)) {
                                using (Bitmap diffuse = TexLoader.ResolveBitmap(diffuseNormal)) {
                                    if (diffuse != null) {
                                        using (Bitmap canvasImage = new Bitmap(diffuse.Size.Width, diffuse.Size.Height, PixelFormat.Format32bppArgb)) {
                                            Bitmap output = null;
                                            if (File.Exists(mask)) {
                                                using (Bitmap normalMaskBitmap = TexLoader.ResolveBitmap(mask)) {
                                                    if (outputFile.Contains("fac_b_n")) {
                                                        Bitmap resize = new Bitmap(diffuse, new Size(1024, 1024));
                                                        output = ImageManipulation.MergeNormals(
                                                            inputFile, resize, canvasImage,
                                                            normalMaskBitmap, diffuseNormal);
                                                        normalCache.Add(inputFile, resize);
                                                        if (!string.IsNullOrEmpty(normalCorrection)) {
                                                            output = ImageManipulation.ResizeAndMerge(output, TexLoader.ResolveBitmap(normalCorrection));
                                                        }
                                                    } else {
                                                        output = ImageManipulation.MergeNormals(
                                                            inputFile, diffuse, canvasImage,
                                                            normalMaskBitmap, diffuseNormal);
                                                        normalCache.Add(inputFile, output);
                                                        if (!string.IsNullOrEmpty(normalCorrection)) {
                                                            output = ImageManipulation.ResizeAndMerge(output, TexLoader.ResolveBitmap(normalCorrection));
                                                        }
                                                    }
                                                }
                                            } else {
                                                output = ImageManipulation.MergeNormals(inputFile, diffuse, canvasImage, null, diffuseNormal);
                                            }
                                            output.Save(stream, ImageFormat.Png);
                                            if (!normalCache.ContainsKey(diffuseNormal)) {
                                                normalCache.Add(diffuseNormal, output);
                                            }
                                        }
                                    }
                                }
                            } else {
                                normalCache[diffuseNormal].Save(stream, ImageFormat.Png);
                            }
                        }
                        break;
                    case ExportType.XNormalImport:
                        using (Bitmap bitmap = TexLoader.ResolveBitmap(inputFile)) {
                            if (bitmap != null) {
                                Bitmap underlay = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format32bppArgb));
                                Graphics g = Graphics.FromImage(underlay);
                                g.Clear(Color.FromArgb(255, 160, 113, 94));
                                if (!string.IsNullOrEmpty(diffuseNormal)) {
                                    g.CompositingQuality = CompositingQuality.HighQuality;
                                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                    g.SmoothingMode = SmoothingMode.HighQuality;
                                    g.DrawImage(TexLoader.ResolveBitmap(diffuseNormal), 0, 0, bitmap.Width, bitmap.Height);
                                }
                                AtramentumLuminisGlow.TransplantData(underlay, bitmap).Save(stream, ImageFormat.Png);
                            }
                        }
                        break;
                }
                stream.Flush();
                stream.Position = 0;
                if (stream.Length > 0) {
                    TextureImporter.PngToTex(stream, out data);
                    stream.Position = 0;
                }
            }
            if (data.Length > 0) {
                Directory.CreateDirectory(Path.GetDirectoryName(outputFile));
                File.WriteAllBytes(outputFile, data);
            }
        }

        public string AppendNumber(string value, int number) {
            return value.Replace(".tex", number + ".tex");
        }
    }
}
