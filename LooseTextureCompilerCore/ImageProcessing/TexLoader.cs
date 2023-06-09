using OtterTex;
using Penumbra.Import.Dds;
using Penumbra.Import.Textures;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using Rectangle = System.Drawing.Rectangle;
using Size = System.Drawing.Size;

namespace FFXIVLooseTextureCompiler.ImageProcessing {
    public static class TexLoader {
        public static Bitmap DDSToBitmap(string inputFile) {
            using (var scratch = ScratchImage.LoadDDS(inputFile)) {
                var rgba = scratch.GetRGBA(out var f).ThrowIfError(f);
                byte[] ddsFile = rgba.Pixels[..(f.Meta.Width * f.Meta.Height * f.Meta.Format.BitsPerPixel() / 8)].ToArray();
                return RGBAToBitmap(ddsFile, scratch.Meta.Width, scratch.Meta.Height);
            }
        }

        public static KeyValuePair<Size, byte[]> DDSToBytes(string inputFile) {
            using (var scratch = ScratchImage.LoadDDS(inputFile)) {
                var rgba = scratch.GetRGBA(out var f).ThrowIfError(f);
                return new KeyValuePair<Size, byte[]>(new Size(f.Meta.Width, f.Meta.Height),
                rgba.Pixels[..(f.Meta.Width * f.Meta.Height * f.Meta.Format.BitsPerPixel() / 8)].ToArray());
            }
        }

        public static KeyValuePair<Size, byte[]> PngToBytes(string inputFile) {
            byte[] output = new byte[0];
            using (Bitmap bitmap = new Bitmap(inputFile)) {
                MemoryStream stream = new MemoryStream();
                bitmap.Save(stream, ImageFormat.Png);
                stream.Position = 0;
                TextureImporter.PngToTex(stream, out output);
                return TexToBytes(new MemoryStream(output));
            }
        }


        public static Bitmap RGBAToBitmap(byte[] RGBAPixels, int width, int height) {
            Bitmap output = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            Rectangle rect = new Rectangle(0, 0, output.Width, output.Height);
            BitmapData bmpData = output.LockBits(rect, ImageLockMode.ReadWrite, output.PixelFormat);
            IntPtr ptr = bmpData.Scan0;
            for (int i = 0; i < RGBAPixels.Length; i += 4) {
                byte R = RGBAPixels[i];
                byte G = RGBAPixels[i + 1];
                byte B = RGBAPixels[i + 2];
                byte A = RGBAPixels[i + 3];

                RGBAPixels[i] = B;
                RGBAPixels[i + 1] = G;
                RGBAPixels[i + 2] = R;
                RGBAPixels[i + 3] = A;

            }
            System.Runtime.InteropServices.Marshal.Copy(RGBAPixels, 0, ptr, RGBAPixels.Length);
            output.UnlockBits(bmpData);
            return output;
        }

        //Optimized

        public static byte[] BitmapToRGBA(Bitmap bitmap) {
            byte[] RGBAPixels = (byte[])new ImageConverter().ConvertTo(new ImageConverter(), typeof(byte[]));
            for (int i = 0; i < RGBAPixels.Length; i += 4) {
                byte B = RGBAPixels[i];
                byte G = RGBAPixels[i + 1];
                byte R = RGBAPixels[i + 2];
                byte A = RGBAPixels[i + 3];

                RGBAPixels[i] = R;
                RGBAPixels[i + 1] = G;
                RGBAPixels[i + 2] = B;
                RGBAPixels[i + 3] = A;
            }
            return RGBAPixels;
        }

        public static byte[] BitmapToRGBA(Stream stream) {
            MemoryStream memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            byte[] RGBAPixels = memoryStream.ToArray();
            for (int i = 0; i < RGBAPixels.Length; i += 4) {
                byte B = RGBAPixels[i];
                byte G = RGBAPixels[i + 1];
                byte R = RGBAPixels[i + 2];
                byte A = RGBAPixels[i + 3];

                RGBAPixels[i] = R;
                RGBAPixels[i + 1] = G;
                RGBAPixels[i + 2] = B;
                RGBAPixels[i + 3] = A;

            }
            return RGBAPixels;
        }

        public static Bitmap TexToBitmap(string path) {
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read)) {
                var scratch = TexFileParser.Parse(stream);
                var rgba = scratch.GetRGBA(out var f).ThrowIfError(f);
                byte[] RGBAPixels = rgba.Pixels[..(f.Meta.Width * f.Meta.Height * f.Meta.Format.BitsPerPixel() / 8)].ToArray();
                return RGBAToBitmap(RGBAPixels, f.Meta.Width, f.Meta.Height);
            }
        }

        public static KeyValuePair<Size, byte[]> TexToBytes(string path) {
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read)) {
                var scratch = TexFileParser.Parse(stream);
                var rgba = scratch.GetRGBA(out var f).ThrowIfError(f);
                return new KeyValuePair<Size, byte[]>(new Size(f.Meta.Width, f.Meta.Height),
                rgba.Pixels[..(f.Meta.Width * f.Meta.Height * f.Meta.Format.BitsPerPixel() / 8)].ToArray());
            }
        }

        public static KeyValuePair<Size, byte[]> TexToBytes(Stream stream) {
            var scratch = TexFileParser.Parse(stream);
            var rgba = scratch.GetRGBA(out var f).ThrowIfError(f);
            return new KeyValuePair<Size, byte[]>(new Size(f.Meta.Width, f.Meta.Height),
            rgba.Pixels[..(f.Meta.Width * f.Meta.Height * f.Meta.Format.BitsPerPixel() / 8)].ToArray());
        }

        public static Bitmap ResolveBitmap(string inputFile) {
            if (string.IsNullOrEmpty(inputFile) || !File.Exists(inputFile)) {
                return new Bitmap(1024, 1024);
            }

            while (IsFileLocked(inputFile)) {
                Thread.Sleep(100);
            }

            try {
                using (Bitmap bitmap =
                    inputFile.EndsWith(".tex") ? TexToBitmap(inputFile) :
                    inputFile.EndsWith(".dds") ? DDSToBitmap(inputFile) :
                    inputFile.EndsWith(".ltct") ? OpenImageFromXOR(inputFile) :
                    new Bitmap(inputFile)) {
                    return new Bitmap(bitmap);
                }
            } catch {
                return new Bitmap(1024, 1024);
            }
        }

        public static byte[] GetTexBytes(string inputFile) {
            byte[] data = new byte[0];
            KeyValuePair<Size, byte[]> keyValuePair = ResolveImageBytes(inputFile);
            TextureImporter.RgbaBytesToTex(keyValuePair.Value, keyValuePair.Key.Width, keyValuePair.Key.Height, out data);
            return data;
        }
        public static void ObfuscateOrDeobfuscate(byte[] blob) {
            for (int i = 0; i < blob.Length; ++i) {
                blob[i] ^= 0x2A;
            }
        }

        public static void WriteImageToXOR(Bitmap data, string filename) {
            MemoryStream memoryStream = new MemoryStream();
            data.Save(memoryStream, ImageFormat.Png);
            byte[] bytes = memoryStream.ToArray();
            ObfuscateOrDeobfuscate(bytes);
            File.WriteAllBytes(filename, bytes);
        }
        public static void WriteImageToXOR(string input, string filename) {
            byte[] bytes = File.ReadAllBytes(input);
            ObfuscateOrDeobfuscate(bytes);
            File.WriteAllBytes(filename, bytes);
        }

        public static Bitmap OpenImageFromXOR(string filename) {
            byte[] file = File.ReadAllBytes(filename);
            ObfuscateOrDeobfuscate(file);
            MemoryStream memoryStream = new MemoryStream(file);
            return new Bitmap(memoryStream);
        }

        public static void ConvertToLtct(string rootDirectory) {
            foreach (string file in Directory.GetFiles(rootDirectory)) {
                if (file.EndsWith(".tex") || file.EndsWith(".png") || file.EndsWith(".bmp") || file.EndsWith(".dds")) {
                    WriteImageToXOR(ResolveBitmap(file), file
                        .Replace(".tex", ".ltct")
                        .Replace(".png", ".ltct")
                        .Replace(".bmp", ".ltct")
                        .Replace(".dds", ".ltct"));
                }
            }
            foreach (string directory in Directory.GetDirectories(rootDirectory)) {
                ConvertToLtct(directory);
            }
        }

        public static void ConvertPngToLtct(string rootDirectory) {
            foreach (string file in Directory.GetFiles(rootDirectory)) {
                if (file.EndsWith(".png")) {
                    WriteImageToXOR(file, file.Replace(".png", ".ltct"));
                }
            }
            foreach (string directory in Directory.GetDirectories(rootDirectory)) {
                ConvertPngToLtct(directory);
            }
        }
        public static void ConvertLtctToPng(string rootDirectory) {
            foreach (string file in Directory.GetFiles(rootDirectory)) {
                if (file.EndsWith(".ltct")) {
                    OpenImageFromXOR(file).Save(file.Replace(".ltct", ".png"));
                }
            }
            foreach (string directory in Directory.GetDirectories(rootDirectory)) {
                ConvertLtctToPng(directory);
            }
        }

        public static void RunOptiPNG(string rootDirectory) {
            foreach (string file in Directory.GetFiles(rootDirectory)) {
                if (file.EndsWith(".png")) {
                    ProcessStartInfo info = new ProcessStartInfo {
                        FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "optipng.exe"),
                        Arguments = "-clobber " + @"""" + file + @"""",
                        WorkingDirectory = rootDirectory,
                        UseShellExecute = false
                    };
                    info.RedirectStandardOutput = true;
                    Process process = new Process();
                    process.StartInfo = info;
                    process.OutputDataReceived += Process_OutputDataReceived;
                    process.Start();
                }
            }
            foreach (string directory in Directory.GetDirectories(rootDirectory)) {
                RunOptiPNG(directory);
            }
        }

        private static void Process_OutputDataReceived(object sender, DataReceivedEventArgs e) {

        }

        public static KeyValuePair<Size, byte[]> ResolveImageBytes(string inputFile) {
            KeyValuePair<Size, byte[]> data = new KeyValuePair<Size, byte[]>(new Size(1, 1), new byte[4]);
            if (!string.IsNullOrEmpty(inputFile) && File.Exists(inputFile)) {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                while (IsFileLocked(inputFile)) {
                    Thread.Sleep(10);
                }
                data = inputFile.EndsWith(".tex") ?
                    TexToBytes(inputFile) : (inputFile.EndsWith(".dds") ?
                    DDSToBytes(inputFile) : PngToBytes(inputFile));
            }
            return data;
        }

        public static bool IsFileLocked(string file) {
            try {
                using (FileStream stream = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.None)) {
                    stream.Close();
                }
            } catch (IOException) {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }

            //file is not locked
            return false;
        }
    }
}
