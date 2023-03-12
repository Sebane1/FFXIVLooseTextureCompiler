using FFXIVLooseTextureCompiler.ImageProcessing;
using FFXIVLooseTextureCompiler.PathOrganization;
using FFXIVVoicePackCreator.Json;
using Newtonsoft.Json;
using Penumbra.Import.Dds;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FFXIVLooseTextureCompiler.MainWindow;

namespace FFXIVLooseTextureCompiler {
    public class TextureProcessor {
        private Dictionary<string, Bitmap> normalCache;
        private Dictionary<string, Bitmap> multiCache;
        private Dictionary<string, string> xnormalCache;
        private XNormal xnormal;
        private List<KeyValuePair<string, string>> childTextureSetQueue;
        private int fileCount;
        private bool finalizeResults;
        private bool generateNormals;
        private bool generateMulti;
        public event EventHandler OnProgressChange;
        public event EventHandler OnLaunchedXnormal;

        public void Export(List<TextureSet> textureSetList, string modPath, int generationType, bool generateNormals, bool generateMulti, bool useXNormal) {
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
            childTextureSetQueue = new List<KeyValuePair<string, string>>();
            foreach (TextureSet textureSet in textureSetList) {
                if (!groups.ContainsKey(textureSet.MaterialGroupName)) {
                    groups.Add(textureSet.MaterialGroupName, new List<TextureSet>() { textureSet });
                    foreach (TextureSet childSet in textureSet.ChildSets) {
                        childSet.MaterialGroupName = textureSet.MaterialGroupName;
                        groups[textureSet.MaterialGroupName].Add(childSet);
                    }
                } else {
                    groups[textureSet.MaterialGroupName].Add(textureSet);
                    foreach (TextureSet childSet in textureSet.ChildSets) {
                        childSet.MaterialGroupName = textureSet.MaterialGroupName;
                        groups[textureSet.MaterialGroupName].Add(childSet);
                    }
                }
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
            if (OnLaunchedXnormal != null) {
                OnLaunchedXnormal.Invoke(this, EventArgs.Empty);
            }
            xnormal.ProcessBatches();
            foreach (KeyValuePair<string, string> keyValuePair in childTextureSetQueue) {
                ExportTex(keyValuePair.Key, keyValuePair.Value);
            }
            foreach (Bitmap value in normalCache.Values) {
                value.Dispose();
            }
            foreach (Bitmap value in multiCache.Values) {
                value.Dispose();
            }
        }
        private bool MultiLogic(TextureSet textureSet, string multiDiskPath) {
            bool outputGenerated = false;
            if (!string.IsNullOrEmpty(textureSet.Multi) && !string.IsNullOrEmpty(textureSet.InternalMultiPath)) {
                if (!textureSet.IsChildSet) {
                    ExportTex(textureSet.Multi, AppendNumber(multiDiskPath, fileCount), ExportType.None,
                        "", "", textureSet.OmniExportMode ? textureSet.Multi.Replace(".", "_xnormal.") : "");
                } else {
                    childTextureSetQueue.Add(new KeyValuePair<string, string>(textureSet.Multi,
                        AppendNumber(multiDiskPath, fileCount)));
                }
                outputGenerated = true;
                foreach (TextureSet child in textureSet.ChildSets) {
                    if (!string.IsNullOrEmpty(child.Multi)) {
                        if (!xnormalCache.ContainsKey(child.Multi)) {
                            if (finalizeResults || !File.Exists(child.Multi)) {
                                if (child.Multi.Contains("baseTexBaked")) {
                                    xnormalCache.Add(child.Multi, child.Multi);
                                    xnormal.AddToBatch(textureSet.InternalMultiPath, textureSet.Multi.Replace(".", "_xnormal."), child.Multi);
                                }
                            }
                        }
                    }
                }
            } else if (!string.IsNullOrEmpty(textureSet.Diffuse) && !string.IsNullOrEmpty(textureSet.InternalMultiPath)
                && generateMulti && !(textureSet.MaterialSetName.ToLower().Contains("eyes"))) {
                if (!textureSet.IgnoreMultiGeneration) {
                    ExportTex(textureSet.Diffuse, AppendNumber(multiDiskPath, fileCount), ExportType.MultiFace);
                    outputGenerated = true;
                }
            }
            return outputGenerated;
        }

        private bool NormalLogic(TextureSet textureSet, string normalDiskPath) {
            bool outputGenerated = false;
            if (!string.IsNullOrEmpty(textureSet.Normal) && !string.IsNullOrEmpty(textureSet.InternalNormalPath)) {
                if (generateNormals && !textureSet.MaterialSetName.ToLower().Contains("eyes")) {
                    ExportTex(textureSet.Normal, AppendNumber(normalDiskPath, fileCount), ExportType.MergeNormal,
                        textureSet.Diffuse, textureSet.NormalMask, textureSet.OmniExportMode ?
                        textureSet.Normal.Replace(".", "_xnormal.") : "");
                    outputGenerated = true;
                    foreach (TextureSet child in textureSet.ChildSets) {
                        if (!xnormalCache.ContainsKey(child.Normal)) {
                            if (finalizeResults || !File.Exists(child.Normal)) {
                                if (child.Normal.Contains("baseTexBaked")) {
                                    xnormalCache.Add(child.Normal, child.Normal);
                                    xnormal.AddToBatch(textureSet.InternalNormalPath,
                                        textureSet.Normal.Replace(".", "_xnormal."), child.Normal);
                                }
                            }
                        }
                    }
                } else {
                    if (!string.IsNullOrEmpty(textureSet.Glow) && (textureSet.MaterialSetName.ToLower().Contains("eyes"))) {
                        ExportTex(textureSet.Normal, AppendNumber(normalDiskPath, fileCount), ExportType.GlowMulti, "",
                            textureSet.Glow);
                        outputGenerated = true;
                    } else {
                        if (!textureSet.IsChildSet) {
                            ExportTex(textureSet.Normal, AppendNumber(normalDiskPath, fileCount), ExportType.None, "",
                            "", textureSet.Normal.Replace(".", "_xnormal."));
                        } else {
                            childTextureSetQueue.Add(new KeyValuePair<string, string>(textureSet.Normal,
                                AppendNumber(normalDiskPath, fileCount)));
                        }
                        outputGenerated = true;
                        foreach (TextureSet child in textureSet.ChildSets) {
                            if (!xnormalCache.ContainsKey(child.Normal)) {
                                if (finalizeResults || !File.Exists(child.Normal)) {
                                    if (child.Normal.Contains("baseTexBaked")) {
                                        xnormalCache.Add(child.Normal, child.Normal);
                                        xnormal.AddToBatch(textureSet.InternalNormalPath,
                                            textureSet.Diffuse.Replace(".", "_xnormal."), child.Normal);
                                    }
                                }
                            }
                        }
                    }
                }
            } else if (!string.IsNullOrEmpty(textureSet.Diffuse) && !string.IsNullOrEmpty(textureSet.InternalNormalPath)
            && generateNormals && !(textureSet.MaterialSetName.ToLower().Contains("eyes"))) {
                if (!textureSet.IgnoreNormalGeneration) {
                    ExportTex(textureSet.Diffuse, AppendNumber(normalDiskPath, fileCount),
                        ExportType.Normal, "", "", textureSet.OmniExportMode ? textureSet.Diffuse.Replace(".", "_normal_xnormal.") : "");
                    outputGenerated = true;
                    foreach (TextureSet child in textureSet.ChildSets) {
                        if (!xnormalCache.ContainsKey(child.Normal)) {
                            if (finalizeResults || !File.Exists(child.Normal)) {
                                if (child.Normal.Contains("baseTexBaked")) {
                                    xnormalCache.Add(child.Normal, child.Normal);
                                    xnormal.AddToBatch(textureSet.InternalDiffusePath,
                                        textureSet.Diffuse.Replace(".", "_normal_xnormal."), child.Normal);
                                }
                            }
                        }
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
                    if (!textureSet.IsChildSet || textureSet.MaterialSetName.ToLower().Contains("eyes")) {
                        ExportTex(textureSet.Diffuse, AppendNumber(diffuseDiskPath, fileCount), ExportType.None,
                            "", "", textureSet.OmniExportMode ? textureSet.Diffuse.Replace(".", "_xnormal.") : "");
                    } else {
                        childTextureSetQueue.Add(new KeyValuePair<string, string>(textureSet.Diffuse,
                            AppendNumber(diffuseDiskPath, fileCount)));
                    }
                    outputGenerated = true;
                } else {
                    ExportTex(textureSet.Diffuse, AppendNumber(diffuseDiskPath, fileCount), ExportType.Glow, "",
                        textureSet.Glow, textureSet.OmniExportMode ? textureSet.Diffuse.Replace(".", "_xnormal.") : "");
                    outputGenerated = true;
                }
                if (outputGenerated) {
                    foreach (TextureSet child in textureSet.ChildSets) {
                        if (!xnormalCache.ContainsKey(child.Diffuse)) {
                            if (finalizeResults || !File.Exists(child.Diffuse)) {
                                if (child.Diffuse.Contains("baseTexBaked")) {
                                    xnormalCache.Add(child.Diffuse, child.Diffuse);
                                    xnormal.AddToBatch(textureSet.InternalDiffusePath,
                                        textureSet.Diffuse.Replace(".", "_xnormal."), child.Diffuse);
                                }
                            }
                        }
                    }
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
            GlowMulti
        }
        public void ExportTex(string inputFile, string outputFile, ExportType exportType = ExportType.None,
            string diffuseNormal = "", string mask = "", string rawDataExport = "") {
            byte[] data = new byte[0];
            int contrast = 500;
            int contrastFace = 100;
            using (MemoryStream stream = new MemoryStream()) {
                switch (exportType) {
                    case ExportType.None:
                        using (Bitmap bitmap = TexLoader.ResolveBitmap(inputFile)) {
                            if (bitmap != null) {
                                bitmap.Save(stream, ImageFormat.Png);
                            }
                        }
                        break;
                    case ExportType.Glow:
                        using (Bitmap bitmap = TexLoader.ResolveBitmap(inputFile)) {
                            if (bitmap != null) {
                                Bitmap glowBitmap = AtramentumLuminisGlow.CalculateDiffuse(bitmap, TexLoader.ResolveBitmap(mask));
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
                        break;
                    case ExportType.MergeNormal:
                        if (string.IsNullOrEmpty(diffuseNormal)) {

                        } else {
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
                }
                stream.Flush();
                stream.Position = 0;
                if (stream.Length > 0) {
                    TextureImporter.PngToTex(stream, out data);
                    stream.Position = 0;
                    if (!string.IsNullOrEmpty(rawDataExport)) {
                        if (!rawDataExport.Contains("baseTexBaked")) {
                            using (FileStream fileStream = new FileStream(rawDataExport, FileMode.Create, FileAccess.Write)) {
                                stream.CopyTo(fileStream);
                            }
                        }
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
