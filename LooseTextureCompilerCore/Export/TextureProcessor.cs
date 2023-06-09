using FFXIVLooseTextureCompiler.Export;
using FFXIVLooseTextureCompiler.ImageProcessing;
using FFXIVLooseTextureCompiler.PathOrganization;
using FFXIVLooseTextureCompiler.Racial;
using FFXIVVoicePackCreator.Json;
using Newtonsoft.Json;
using Penumbra.Import.Dds;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Color = System.Drawing.Color;
using Size = System.Drawing.Size;

namespace FFXIVLooseTextureCompiler {
    public class TextureProcessor {
        private Dictionary<string, Bitmap> normalCache;
        private Dictionary<string, Bitmap> multiCache;
        private Dictionary<string, string> xnormalCache;
        private XNormal xnormal;
        private List<KeyValuePair<string, string>> textureSetQueue;
        private int fileCount;

        private bool finalizeResults;
        private bool generateNormals;
        private bool generateMulti;

        public event EventHandler OnProgressChange;
        public event EventHandler OnStartedProcessing;
        public event EventHandler OnLaunchedXnormal;

        private Bitmap GetMergedBitmap(string file) {
            if (file.Contains("baseTexBaked") && (file.Contains("_d_") || file.Contains("_g_") || file.Contains("_n_"))) {
                Bitmap alpha = TexLoader.ResolveBitmap(file.Replace("baseTexBaked", "alpha_baseTexBaked"));
                Bitmap rgb = TexLoader.ResolveBitmap(file.Replace("baseTexBaked", "rgb_baseTexBaked"));
                Bitmap merged = ImageManipulation.MergeAlphaToRGB(alpha, rgb);
                merged.Save(file, ImageFormat.Png);
                return merged;
            } else {
                return TexLoader.ResolveBitmap(file);
            }
        }

        public void BatchTextureSet(TextureSet parent, TextureSet child) {
            if (!string.IsNullOrEmpty(child.Diffuse)) {
                if (!xnormalCache.ContainsKey(child.Diffuse)) {
                    string diffuseAlpha = parent.Diffuse.Replace(".", "_alpha.").Replace(".tex", ".png");
                    string diffuseRGB = parent.Diffuse.Replace(".", "_rgb.").Replace(".tex", ".png");
                    if (finalizeResults || !File.Exists(child.Diffuse.Replace("baseTexBaked", "rgb_baseTexBaked"))
                        || !File.Exists(child.Diffuse.Replace("baseTexBaked", "alpha_baseTexBaked"))) {
                        if (child.Diffuse.Contains("baseTexBaked")) {
                            xnormalCache.Add(child.Diffuse, child.Diffuse);
                            Bitmap diffuse = TexLoader.ResolveBitmap(parent.Diffuse);
                            if (Directory.Exists(Path.GetDirectoryName(diffuseAlpha))
                                && Directory.Exists(Path.GetDirectoryName(diffuseRGB))) {
                                string childAlpha = child.Diffuse.Replace("baseTexBaked", "alpha");
                                string childRGB = child.Diffuse.Replace("baseTexBaked", "rgb");
                                ImageManipulation.ExtractTransparency(diffuse).Save(diffuseAlpha, ImageFormat.Png);
                                ImageManipulation.ExtractRGB(diffuse).Save(diffuseRGB, ImageFormat.Png);
                                if (finalizeResults) {
                                    xnormal.AddToBatch(parent.InternalDiffusePath, diffuseAlpha, childAlpha, false);
                                    xnormal.AddToBatch(parent.InternalDiffusePath, diffuseRGB, childRGB, false);
                                } else {
                                    if (!File.Exists(childAlpha)) {
                                        new Bitmap(1024, 1024).Save(childAlpha.Replace(".", "_baseTexBaked."), ImageFormat.Png);
                                    }
                                    if (!File.Exists(childRGB)) {
                                        new Bitmap(1024, 1024).Save(childRGB.Replace(".", "_baseTexBaked."), ImageFormat.Png);
                                    }
                                }
                            } else {
                                //MessageBox.Show("Something has gone terribly wrong. " + parent.Diffuse + "is missing");
                            }
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(child.Normal)) {
                if (!xnormalCache.ContainsKey(child.Normal)) {
                    string normalAlpha = parent.Normal.Replace(".", "_alpha.");
                    string normalRGB = parent.Normal.Replace(".", "_rgb.");
                    if (finalizeResults || !File.Exists(child.Normal.Replace("baseTexBaked", "rgb_baseTexBaked"))
                        || !File.Exists(child.Normal.Replace("baseTexBaked", "alpha_baseTexBaked"))) {
                        if (child.Normal.Contains("baseTexBaked")) {
                            xnormalCache.Add(child.Normal, child.Normal);
                            Bitmap normal = TexLoader.ResolveBitmap(parent.Normal);
                            ImageManipulation.ExtractTransparency(normal).Save(normalAlpha, ImageFormat.Png);
                            ImageManipulation.ExtractRGB(normal, true).Save(normalRGB, ImageFormat.Png);
                            xnormal.AddToBatch(parent.InternalDiffusePath, normalAlpha, child.Normal.Replace("baseTexBaked", "alpha"), false);
                            xnormal.AddToBatch(parent.InternalDiffusePath, normalRGB, child.Normal.Replace("baseTexBaked", "rgb"), true);
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(child.Multi)) {
                if (!xnormalCache.ContainsKey(child.Multi)) {
                    if (finalizeResults || !File.Exists(child.Multi)) {
                        if (child.Multi.Contains("baseTexBaked")) {
                            xnormalCache.Add(child.Multi, child.Multi);
                            xnormal.AddToBatch(parent.InternalMultiPath, parent.Multi, child.Multi, false);
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(child.Glow)) {
                if (!xnormalCache.ContainsKey(child.Glow)) {
                    string glowAlpha = parent.Glow.Replace(".", "_alpha.");
                    string glowRGB = parent.Glow.Replace(".", "_rgb.");
                    if (finalizeResults || !File.Exists(child.Glow.Replace("baseTexBaked", "rgb_baseTexBaked"))
                        || !File.Exists(child.Glow.Replace("baseTexBaked", "alpha_baseTexBaked"))) {
                        if (child.Glow.Contains("baseTexBaked")) {
                            xnormalCache.Add(child.Glow, child.Glow);
                            Bitmap glow = TexLoader.ResolveBitmap(parent.Glow);
                            ImageManipulation.ExtractTransparency(glow).Save(glowAlpha, ImageFormat.Png);
                            ImageManipulation.ExtractRGB(glow).Save(glowRGB, ImageFormat.Png);
                            xnormal.AddToBatch(parent.InternalDiffusePath, glowAlpha, child.Glow.Replace("baseTexBaked", "alpha"), false);
                            xnormal.AddToBatch(parent.InternalDiffusePath, glowRGB, child.Glow.Replace("baseTexBaked", "rgb"), false);
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(child.NormalMask)) {
                if (!xnormalCache.ContainsKey(child.NormalMask)) {
                    string normalMaskAlpha = parent.NormalMask.Replace(".", "_alpha.");
                    string normalMaskRGB = parent.NormalMask.Replace(".", "_rgb.");
                    if (finalizeResults || !File.Exists(child.NormalMask.Replace("baseTexBaked", "rgb_baseTexBaked"))
                        || !File.Exists(child.NormalMask.Replace("baseTexBaked", "alpha_baseTexBaked"))) {
                        if (child.NormalMask.Contains("baseTexBaked")) {
                            xnormalCache.Add(child.NormalMask, child.NormalMask);
                            Bitmap normalMask = TexLoader.ResolveBitmap(parent.NormalMask);
                            ImageManipulation.ExtractTransparency(normalMask).Save(normalMaskAlpha, ImageFormat.Png);
                            ImageManipulation.ExtractRGB(normalMask).Save(normalMaskRGB, ImageFormat.Png);
                            xnormal.AddToBatch(parent.InternalDiffusePath, normalMaskAlpha, child.NormalMask.Replace("baseTexBaked", "alpha"), false);
                            xnormal.AddToBatch(parent.InternalDiffusePath, normalMaskRGB, child.NormalMask.Replace("baseTexBaked", "rgb"), false);
                        }
                    }
                }
            }
        }

        public void Export(List<TextureSet> textureSetList, Dictionary<string, int> groupOptionTypes, string modPath, int generationType,
            bool generateNormals, bool generateMulti, bool useXNormal, string xNormalPathOverride = "") {
            XNormal.XNormalPathOverride = xNormalPathOverride;
            int i = 0;
            fileCount = 0;
            finalizeResults = useXNormal;
            Dictionary<string, List<TextureSet>> groups = new Dictionary<string, List<TextureSet>>();
            normalCache?.Clear();
            multiCache?.Clear();
            xnormalCache?.Clear();
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
                int choiceOption = groupOptionTypes.ContainsKey(textureSets[0].MaterialGroupName)
                ? (groupOptionTypes[textureSets[0].MaterialGroupName] == 0
                ? generationType : groupOptionTypes[textureSets[0].MaterialGroupName] - 1)
                : generationType;
                Group group = new Group(textureSets[0].MaterialGroupName.Replace(@"/", "-").Replace(@"\", "-"), "", 0,
                    (choiceOption == 2 && textureSets.Count > 1) ? "Single" : "Multi", 0);
                Option option = null;
                Option diffuseOption = null;
                Option normalOption = null;
                Option multiOption = null;
                bool alreadySetOption = false;
                foreach (TextureSet textureSet in textureSets) {
                    string diffuseDiskPath = !string.IsNullOrEmpty(textureSet.InternalDiffusePath) ?
                        Path.Combine(modPath, textureSet.InternalDiffusePath.Replace("/", @"\")) : "";
                    string normalDiskPath = !string.IsNullOrEmpty(textureSet.InternalNormalPath) ?
                        Path.Combine(modPath, textureSet.InternalNormalPath.Replace("/", @"\")) : "";
                    string multiDiskPath = !string.IsNullOrEmpty(textureSet.InternalMultiPath) ?
                        Path.Combine(modPath, textureSet.InternalMultiPath.Replace("/", @"\")) : "";
                    switch (choiceOption) {
                        case 0:
                            if (!string.IsNullOrEmpty(textureSet.Diffuse) && !string.IsNullOrEmpty(textureSet.InternalDiffusePath)) {
                                if (DiffuseLogic(textureSet, diffuseDiskPath)) {
                                    AddDetailedGroupOption(textureSet.InternalDiffusePath, "Diffuse", "Normal", textureSet,
                                        textureSets, group, diffuseOption, out diffuseOption);
                                }
                            }
                            if (OnProgressChange != null) {
                                OnProgressChange.Invoke(this, EventArgs.Empty);
                            }
                            if (!string.IsNullOrEmpty(textureSet.InternalNormalPath)) {
                                if (NormalLogic(textureSet, normalDiskPath)) {
                                    AddDetailedGroupOption(textureSet.InternalNormalPath, "Normal", "Multi", textureSet,
                                           textureSets, group, normalOption, out normalOption);
                                }
                            }
                            if (OnProgressChange != null) {
                                OnProgressChange.Invoke(this, EventArgs.Empty);
                            }
                            if (!string.IsNullOrEmpty(textureSet.InternalMultiPath)) {
                                if (MultiLogic(textureSet, multiDiskPath)) {
                                    AddDetailedGroupOption(textureSet.InternalMultiPath, "Multi", "Catchlight", textureSet,
                                            textureSets, group, multiOption, out multiOption);
                                }
                            }
                            if (OnProgressChange != null) {
                                OnProgressChange.Invoke(this, EventArgs.Empty);
                            }
                            break;
                        case 1:
                        case 2:
                        case 3:
                            if ((!textureSet.IsChildSet && choiceOption != 3) || (choiceOption == 3 && !alreadySetOption)) {
                                if (!string.IsNullOrEmpty(textureSet.Diffuse) ||
                                    !string.IsNullOrEmpty(textureSet.Normal) ||
                                    !string.IsNullOrEmpty(textureSet.Multi)) {
                                    option = new Option(textureSet.MaterialSetName == textureSet.MaterialGroupName || choiceOption == 3 ? "Enable"
                                    : textureSet.MaterialSetName + (textureSet.ChildSets.Count > 0 ? " (Universal)" : ""), 0);
                                    group.Options.Add(option);
                                    alreadySetOption = true;
                                }
                            }
                            if (!string.IsNullOrEmpty(textureSet.Diffuse) && !string.IsNullOrEmpty(textureSet.InternalDiffusePath)) {
                                if (DiffuseLogic(textureSet, diffuseDiskPath)) {
                                    option.Files[textureSet.InternalDiffusePath] =
                                        AppendNumber(textureSet.InternalDiffusePath.Replace("/", @"\"), fileCount++);
                                }
                            }
                            if (OnProgressChange != null) {
                                OnProgressChange.Invoke(this, EventArgs.Empty);
                            }
                            if (!string.IsNullOrEmpty(textureSet.InternalNormalPath)) {
                                if (NormalLogic(textureSet, normalDiskPath)) {
                                    option.Files[textureSet.InternalNormalPath] =
                                        AppendNumber(textureSet.InternalNormalPath.Replace("/", @"\"), fileCount++);
                                }
                            }
                            if (OnProgressChange != null) {
                                OnProgressChange.Invoke(this, EventArgs.Empty);
                            }
                            if (!string.IsNullOrEmpty(textureSet.InternalMultiPath)) {
                                if (MultiLogic(textureSet, multiDiskPath)) {
                                    option.Files[textureSet.InternalMultiPath] =
                                        AppendNumber(textureSet.InternalMultiPath.Replace("/", @"\"), fileCount++);
                                }
                            }
                            if (OnProgressChange != null) {
                                OnProgressChange.Invoke(this, EventArgs.Empty);
                            }
                            break;
                    }
                }
                if (group.Options.Count > 0) {
                    string groupPath = Path.Combine(modPath, $"group_" + (i++).ToString().PadLeft(3) + $"_{group.Name.ToLower().Replace(" ", "_")}.json");
                    ExportGroup(groupPath, group);
                }
            }
            foreach (Bitmap value in normalCache.Values) {
                value.Dispose();
            }
            foreach (Bitmap value in multiCache.Values) {
                value.Dispose();
            }
        }
        public void AddDetailedGroupOption(string path, string name, string alternateName,
            TextureSet textureSet, List<TextureSet> textureSets, Group group, Option inputOption, out Option outputOption) {
            if (!textureSet.IsChildSet) {
                outputOption = new Option((textureSets.Count > 1 ? textureSet.MaterialSetName + " " : "")
                + (textureSet.InternalMultiPath.ToLower().Contains("catchlight") ? alternateName : name)
                + (textureSet.ChildSets.Count > 0 ? " (Universal)" : ""), 0);
                group.Options.Add(outputOption);
            } else {
                outputOption = inputOption;
            }
            outputOption.Files.Add(path,
                AppendNumber(path.Replace("/", @"\"),
                fileCount++));
        }
        private bool MultiLogic(TextureSet textureSet, string multiDiskPath) {
            bool outputGenerated = false;
            if (!string.IsNullOrEmpty(textureSet.Multi) && !string.IsNullOrEmpty(textureSet.InternalMultiPath)) {
                if (!string.IsNullOrEmpty(textureSet.Glow) && !textureSet.InternalMultiPath.Contains("catchlight")) {
                    ExportTex(textureSet.Multi, AppendNumber(multiDiskPath, fileCount), ExportType.GlowMulti, "", textureSet.Glow);
                } else {
                    ExportTex(textureSet.Multi, AppendNumber(multiDiskPath, fileCount), ExportType.None);
                }
                outputGenerated = true;
            } else if (!string.IsNullOrEmpty(textureSet.Diffuse) && !string.IsNullOrEmpty(textureSet.InternalMultiPath)
                && generateMulti && !(textureSet.InternalMultiPath.ToLower().Contains("catchlight"))) {
                if (!textureSet.IgnoreMultiGeneration) {
                    if (textureSet.InternalDiffusePath.Contains("b0001_b_d") || textureSet.InternalDiffusePath.Contains("b0101_b_d")) {
                        ExportTex(textureSet.Diffuse, AppendNumber(multiDiskPath, fileCount), ExportType.MultiTbse, "", textureSet.Glow,
                            textureSet.BackupTexturePaths != null ? textureSet.BackupTexturePaths.Diffuse : "");
                    } else if (textureSet.InternalDiffusePath.Contains("fac_b_d")) {
                        ExportTex(textureSet.Diffuse, AppendNumber(multiDiskPath, fileCount), ExportType.MultiFaceAsym, "", textureSet.Glow,
                            textureSet.BackupTexturePaths != null ? textureSet.BackupTexturePaths.Diffuse : "");
                    } else if (textureSet.InternalDiffusePath.Contains("fac_d")) {
                        ExportTex(textureSet.Diffuse, AppendNumber(multiDiskPath, fileCount), ExportType.MultiFace, "", textureSet.Glow,
                            textureSet.BackupTexturePaths != null ? textureSet.BackupTexturePaths.Diffuse : "");
                    }else {
                        ExportTex(textureSet.Diffuse, AppendNumber(multiDiskPath, fileCount), ExportType.Multi, "", textureSet.Glow,
                            textureSet.BackupTexturePaths != null ? textureSet.BackupTexturePaths.Diffuse : "");
                    }
                    outputGenerated = true;
                }
            }
            return outputGenerated;
        }

        private bool NormalLogic(TextureSet textureSet, string normalDiskPath) {
            bool outputGenerated = false;
            if (!string.IsNullOrEmpty(textureSet.Normal) && !string.IsNullOrEmpty(textureSet.InternalNormalPath)) {
                if (generateNormals && !textureSet.InternalMultiPath.ToLower().Contains("catchlight")) {
                    ExportTex(textureSet.Normal, AppendNumber(normalDiskPath, fileCount), ExportType.MergeNormal,
                        textureSet.Diffuse, textureSet.NormalMask,
                        textureSet.BackupTexturePaths != null ? textureSet.BackupTexturePaths.Normal : "", textureSet.NormalCorrection);
                    outputGenerated = true;
                } else {
                    if (!string.IsNullOrEmpty(textureSet.Glow) && (textureSet.InternalMultiPath.ToLower().Contains("catchlight"))) {
                        ExportTex(textureSet.Normal, AppendNumber(normalDiskPath, fileCount), ExportType.GlowEyeMulti, "",
                            textureSet.Glow);
                        outputGenerated = true;
                    } else {
                        ExportTex(textureSet.Normal, AppendNumber(normalDiskPath, fileCount), ExportType.None, "", "",
                            textureSet.BackupTexturePaths != null ? textureSet.BackupTexturePaths.Normal : "");
                        outputGenerated = true;
                    }
                }
            } else if (!string.IsNullOrEmpty(textureSet.Diffuse) && !string.IsNullOrEmpty(textureSet.InternalNormalPath)
            && generateNormals && !(textureSet.InternalMultiPath.ToLower().Contains("catchlight"))) {
                if (!textureSet.IgnoreNormalGeneration) {
                    if (textureSet.BackupTexturePaths != null) {
                        ExportTex((Path.Combine(AppDomain.CurrentDomain.BaseDirectory, textureSet.BackupTexturePaths.Normal)),
                            AppendNumber(normalDiskPath, fileCount), ExportType.MergeNormal, textureSet.Diffuse, textureSet.NormalMask,
                            (textureSet.BackupTexturePaths != null ? textureSet.BackupTexturePaths.Normal : ""),
                            textureSet.NormalCorrection, textureSet.InvertNormalGeneration);
                        outputGenerated = true;
                    } else {
                        ExportTex(textureSet.Diffuse, AppendNumber(normalDiskPath, fileCount),
                            ExportType.Normal, "", textureSet.NormalMask, textureSet.BackupTexturePaths != null ?
                            textureSet.BackupTexturePaths.Diffuse : "",
                            textureSet.NormalCorrection, textureSet.InvertNormalGeneration);
                        outputGenerated = true;
                    }
                }
            }
            return outputGenerated;
        }

        private bool DiffuseLogic(TextureSet textureSet, string diffuseDiskPath) {
            bool outputGenerated = false;
            if ((textureSet.InternalMultiPath.ToLower().Contains("catchlight") && generateNormals)) {
                ExportTex(textureSet.Diffuse, AppendNumber(diffuseDiskPath, fileCount), ExportType.Normal,
                    textureSet.Diffuse);
                outputGenerated = true;
            } else {
                string underlay = textureSet.BackupTexturePaths != null ? (RaceInfo.ReverseRaceLookup(textureSet.InternalDiffusePath) == 6 ?
                     textureSet.BackupTexturePaths.DiffuseRaen : textureSet.BackupTexturePaths.Diffuse) : "";
                if (string.IsNullOrEmpty(textureSet.Glow) || textureSet.InternalMultiPath.ToLower().Contains("catchlight")) {
                    ExportTex(textureSet.Diffuse, AppendNumber(diffuseDiskPath, fileCount),
                        ExportType.None, "", "", underlay);
                    outputGenerated = true;
                } else {
                    ExportTex(textureSet.Diffuse, AppendNumber(diffuseDiskPath, fileCount),
                        ExportType.Glow, "", textureSet.Glow, underlay);
                    outputGenerated = true;
                }
            }
            return outputGenerated;
        }
        private void ExportGroup(string path, Group group) {
            if (path != null) {
                if (group.Options.Count > 32) {
                    int groupsToSplitTo = group.Options.Count / 32;
                    for (int i = 0; i < groupsToSplitTo; i++) {
                        int rangeStartingPoint = 32 * i;
                        int maxRange = group.Options.Count - rangeStartingPoint;
                        Group newGroup = new Group(group.Name + $" ({i + 1})", group.Description,
                                        group.Priority, group.Type, group.DefaultSettings);
                        newGroup.Options = group.Options.GetRange(rangeStartingPoint, maxRange > 32 ? 32 : maxRange);
                        using (StreamWriter file = File.CreateText(path.Replace(".", $" ({i})."))) {
                            JsonSerializer serializer = new JsonSerializer();
                            serializer.Formatting = Formatting.Indented;
                            serializer.Serialize(file, newGroup);
                        }
                    }
                } else if (group.Options.Count > 0) {
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
            Multi,
            MergeNormal,
            Glow,
            GlowEyeMulti,
            XNormalImport,
            DontManipulate,
            GlowMulti,
            MultiTbse,
            MultiFace,
            MultiFaceAsym
        }
        public void ExportTex(string inputFile, string outputFile, ExportType exportType = ExportType.None,
            string diffuseNormal = "", string mask = "", string layeringImage = "", string normalCorrection = "", bool modifier = false) {
            byte[] data = new byte[0];
            int contrast = 500;
            int contrastFace = 100;
            bool skipPngTexConversion = false;
            using (MemoryStream stream = new MemoryStream()) {
                switch (exportType) {
                    case ExportType.None:
                        using (Bitmap bitmap = TexLoader.ResolveBitmap(inputFile)) {
                            if (bitmap != null) {
                                if (!string.IsNullOrEmpty(layeringImage)) {
                                    Bitmap image = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format32bppArgb);
                                    Bitmap layer = TexLoader.ResolveBitmap(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                        layeringImage));
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
                    case ExportType.DontManipulate:
                        data = TexLoader.GetTexBytes(inputFile);
                        skipPngTexConversion = true;
                        break;
                    case ExportType.Glow:
                        using (Bitmap bitmap = TexLoader.ResolveBitmap(inputFile)) {
                            if (bitmap != null) {
                                if (!string.IsNullOrEmpty(layeringImage)) {
                                    Bitmap image = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format32bppArgb);
                                    Bitmap layer = TexLoader.ResolveBitmap(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                        layeringImage));
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
                    case ExportType.GlowEyeMulti:
                        using (Bitmap bitmap = TexLoader.ResolveBitmap(inputFile)) {
                            if (bitmap != null) {
                                Bitmap glowBitmap = AtramentumLuminisGlow.CalculateEyeMulti(bitmap, TexLoader.ResolveBitmap(mask));
                                glowBitmap.Save(stream, ImageFormat.Png);
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
                        Bitmap output;
                        if (normalCache.ContainsKey(inputFile)) {
                            output = normalCache[inputFile];
                        } else {
                            using (Bitmap bitmap = TexLoader.ResolveBitmap(inputFile)) {
                                using (Bitmap target = new Bitmap(bitmap.Size.Width, bitmap.Size.Height, PixelFormat.Format32bppArgb)) {
                                    Graphics g = Graphics.FromImage(target);
                                    g.Clear(Color.Transparent);
                                    g.CompositingQuality = CompositingQuality.HighQuality;
                                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                    g.SmoothingMode = SmoothingMode.HighQuality;
                                    g.DrawImage(bitmap, 0, 0, bitmap.Width, bitmap.Height);
                                    if (File.Exists(mask)) {
                                        using (Bitmap normalMaskBitmap = TexLoader.ResolveBitmap(mask)) {
                                            output = Normal.Calculate(modifier ? ImageManipulation.InvertImage(target)
                                                : target, normalMaskBitmap);
                                        }
                                    } else {
                                        output = Normal.Calculate(modifier ? ImageManipulation.InvertImage(target) : target);
                                    }
                                    if (outputFile.Contains("fac_b_n")) {
                                        output = new Bitmap(output, new Size(1024, 1024));
                                    }
                                    normalCache.Add(inputFile, output);
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(normalCorrection)) {
                            output = ImageManipulation.ResizeAndMerge(output, TexLoader.ResolveBitmap(normalCorrection));
                        }
                        output.Save(stream, ImageFormat.Png);
                        break;
                    case ExportType.Multi:
                    case ExportType.MultiFace:
                    case ExportType.MultiFaceAsym:
                    case ExportType.MultiTbse:
                        if (multiCache.ContainsKey(inputFile)) {
                            multiCache[inputFile].Save(stream, ImageFormat.Png);
                        } else {
                            using (Bitmap bitmap = TexLoader.ResolveBitmap(inputFile)) {
                                if (bitmap != null) {
                                    Bitmap image;
                                    if (layeringImage != null) {
                                        image = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format32bppArgb);
                                        Bitmap layer = TexLoader.ResolveBitmap(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                            layeringImage));
                                        Graphics g = Graphics.FromImage(image);
                                        g.Clear(Color.Transparent);
                                        g.CompositingQuality = CompositingQuality.HighQuality;
                                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                        g.SmoothingMode = SmoothingMode.HighQuality;
                                        g.DrawImage(layer, 0, 0, bitmap.Width, bitmap.Height);
                                        g.DrawImage(GetMergedBitmap(inputFile), 0, 0, bitmap.Width, bitmap.Height);
                                    } else {
                                        image = bitmap;
                                    }
                                    Bitmap generatedMulti = exportType != ExportType.MultiTbse
                                    ? (exportType == ExportType.MultiFace || exportType == ExportType.MultiFaceAsym) ?
                                    ImageManipulation.GenerateFaceMulti(image, exportType == ExportType.MultiFaceAsym) :
                                    ImageManipulation.GenerateSkinMulti(image)
                                    : MultiplyFilter.MultiplyImage(Brightness.BrightenImage(Grayscale.MakeGrayscale(image)), 255,
                                    (byte)0, 0);
                                    Bitmap multi = !string.IsNullOrEmpty(mask)
                                        ? AtramentumLuminisGlow.CalculateMulti(generatedMulti, TexLoader.ResolveBitmap(mask))
                                        : generatedMulti;
                                    multi.Save(stream, ImageFormat.Png);
                                    multiCache.Add(inputFile, multi);
                                }
                            }
                        }
                        break;
                    case ExportType.MergeNormal:
                        if (!string.IsNullOrEmpty(diffuseNormal)) {
                            if (!normalCache.ContainsKey(diffuseNormal)) {
                                using (Bitmap diffuse = TexLoader.ResolveBitmap(diffuseNormal)) {
                                    if (diffuse != null) {
                                        using (Bitmap canvasImage = new Bitmap(diffuse.Size.Width,
                                            diffuse.Size.Height, PixelFormat.Format32bppArgb)) {
                                            output = null;
                                            if (File.Exists(mask)) {
                                                using (Bitmap normalMaskBitmap = TexLoader.ResolveBitmap(mask)) {
                                                    if (outputFile.Contains("fac_b_n")) {
                                                        Bitmap resize = new Bitmap(diffuse, new Size(1024, 1024));
                                                        output = ImageManipulation.MergeNormals(inputFile, resize,
                                                            canvasImage, normalMaskBitmap, diffuseNormal);
                                                    } else {
                                                        output = ImageManipulation.MergeNormals(inputFile, diffuse,
                                                            canvasImage, normalMaskBitmap, diffuseNormal);
                                                    }
                                                }
                                            } else {
                                                using (Bitmap bitmap = TexLoader.ResolveBitmap(inputFile)) {
                                                    if (bitmap != null) {
                                                        if (!string.IsNullOrEmpty(layeringImage)) {
                                                            Bitmap image = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format32bppArgb);
                                                            Bitmap layer = TexLoader.ResolveBitmap(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                                                layeringImage));
                                                            Graphics g = Graphics.FromImage(image);
                                                            g.Clear(Color.Transparent);
                                                            g.CompositingQuality = CompositingQuality.HighQuality;
                                                            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                                            g.SmoothingMode = SmoothingMode.HighQuality;
                                                            g.DrawImage(layer, 0, 0, bitmap.Width, bitmap.Height);
                                                            g.DrawImage(GetMergedBitmap(inputFile), 0, 0, bitmap.Width, bitmap.Height);
                                                            output = ImageManipulation.MergeNormals(image, diffuse, canvasImage, null, diffuseNormal);
                                                        } else {
                                                            output = ImageManipulation.MergeNormals(inputFile, diffuse, canvasImage, null, diffuseNormal);
                                                        }
                                                    }
                                                }
                                            }
                                            if (!string.IsNullOrEmpty(normalCorrection)) {
                                                output = ImageManipulation.ResizeAndMerge(output, TexLoader.ResolveBitmap(normalCorrection));
                                            }
                                            output.Save(stream, ImageFormat.Png);
                                            normalCache.Add(diffuseNormal, output);
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
                                Bitmap underlay = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format32bppArgb);
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
                if (!skipPngTexConversion) {
                    stream.Flush();
                    stream.Position = 0;
                    if (stream.Length > 0) {
                        TextureImporter.PngToTex(stream, out data);
                        stream.Position = 0;
                    }
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
