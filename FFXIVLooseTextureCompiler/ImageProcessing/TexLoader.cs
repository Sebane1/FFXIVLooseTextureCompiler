using OtterTex;
using Penumbra.Import.Dds;
using Penumbra.Import.Textures;
using System.Diagnostics;
using System.Drawing.Imaging;

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
            bool failSafeTriggered = false;
            if (!string.IsNullOrEmpty(inputFile)) {
                if (File.Exists(inputFile)) {
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    while (IsFileLocked(inputFile)) {
                        Application.DoEvents();
                        Thread.Sleep(10);
                    }
                    if (!failSafeTriggered) {
                        try {
                            using (Bitmap bitmap = inputFile.EndsWith(".tex") ?
                                TexLoader.TexToBitmap(inputFile) : (inputFile.EndsWith(".dds") ?
                                TexLoader.DDSToBitmap(inputFile) : new Bitmap(inputFile))) {
                                return new Bitmap(bitmap);
                            }
                        } catch {
                            MessageBox.Show(inputFile + " failed to read. The tool will now skip it.");
                            return new Bitmap(1024, 1024);
                        }
                    } else {
                        MessageBox.Show(inputFile + " is missing. The tool will now skip it.");
                        return new Bitmap(1024, 1024);
                    }
                } else {
                    return new Bitmap(1024, 1024);
                }
            } else {
                return null;
            }
        }

        public static byte[] GetTexBytes(string inputFile) {
            byte[] data = new byte[0];
            KeyValuePair<Size, byte[]> keyValuePair = ResolveImageBytes(inputFile);
            TextureImporter.RgbaBytesToTex(keyValuePair.Value, keyValuePair.Key.Width, keyValuePair.Key.Height, out data);
            return data;
        }

        public static KeyValuePair<Size, byte[]> ResolveImageBytes(string inputFile) {
            KeyValuePair<Size, byte[]> data = new KeyValuePair<Size, byte[]>(new Size(1, 1), new byte[4]);
            bool failSafeTriggered = false;
            if (!string.IsNullOrEmpty(inputFile)) {
                if (File.Exists(inputFile)) {
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    while (IsFileLocked(inputFile)) {
                        Application.DoEvents();
                        Thread.Sleep(10);
                    }
                    if (!failSafeTriggered) {
                        //try {
                        data = inputFile.EndsWith(".tex") ?
                            TexToBytes(inputFile) : (inputFile.EndsWith(".dds") ?
                            DDSToBytes(inputFile) : PngToBytes(inputFile));
                        //} catch {
                        //    MessageBox.Show(inputFile + " failed to read. The tool will now skip it.");
                        //}
                    }
                }
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
