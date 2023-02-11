using OtterTex;
using Penumbra.Import.Textures;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFXIVLooseTextureCompiler.ImageProcessing {
    public static class TexLoader {
        public static Bitmap DDSToBitmap(string inputFile) {
            using (var scratch = ScratchImage.LoadDDS(inputFile)) {
                var rgba = scratch.GetRGBA(out var f).ThrowIfError(f);
                byte[] ddsFile = rgba.Pixels[..(f.Meta.Width * f.Meta.Height * f.Meta.Format.BitsPerPixel() / 8)].ToArray();
                return RGBAToBitmap(ddsFile, scratch.Meta.Width, scratch.Meta.Height);
            }
        }
        public static Bitmap RGBAToBitmap(byte[] RGBAPixels, int width, int height) {
            Bitmap output = new Bitmap(width, height);
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
        public static Bitmap TexToBitmap(string path) {
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read)) {
                var scratch = TexFileParser.Parse(stream);
                var rgba = scratch.GetRGBA(out var f).ThrowIfError(f);
                byte[] RGBAPixels = rgba.Pixels[..(f.Meta.Width * f.Meta.Height * f.Meta.Format.BitsPerPixel() / 8)].ToArray();
                return RGBAToBitmap(RGBAPixels, f.Meta.Width, f.Meta.Height);
            }
        }
        public static Bitmap ResolveBitmap(string inputFile) {
            bool failSafeTriggered = false;
            if (!string.IsNullOrEmpty(inputFile)) {
                if (File.Exists(inputFile)) {
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    while (IsFileLocked(inputFile)) {
                        Application.DoEvents();
                        if (stopwatch.ElapsedMilliseconds > 15000 && stopwatch.IsRunning) {
                            MessageBox.Show(inputFile + " is taking an abnormal amount of time to be released. The tool will now skip it.");
                            stopwatch.Stop();
                            stopwatch.Reset();
                            failSafeTriggered = true;
                            break;
                        }
                    }
                    if (!failSafeTriggered) {
                        using (Bitmap bitmap = inputFile.EndsWith(".tex") ?
                            TexLoader.TexToBitmap(inputFile) : (inputFile.EndsWith(".dds") ?
                            TexLoader.DDSToBitmap(inputFile) : new Bitmap(inputFile))) {
                            return new Bitmap(bitmap);
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
