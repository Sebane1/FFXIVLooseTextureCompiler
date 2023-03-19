using SixLabors.ImageSharp.ColorSpaces;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFXIVLooseTextureCompiler.ImageProcessing {
    public class ImageManipulation {
        public static Bitmap SaniitizeArtifacts(Bitmap file) {
            Bitmap image = new Bitmap(file);
            LockBitmap source = new LockBitmap(image);
            source.LockBits();
            for (int y = 0; y < image.Height; y++) {
                for (int x = 0; x < image.Width; x++) {
                    Color sourcePixel = source.GetPixel(x, y);
                    if (sourcePixel.A < 255) {
                        Color col = Color.FromArgb(0, sourcePixel.R, sourcePixel.G, sourcePixel.B);
                        source.SetPixel(x, y, col);
                    }
                }
            };
            source.UnlockBits();
            return image;
        }

        public static Bitmap Resize(Bitmap file, int width, int height) {
            Bitmap image = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(image);
            g.Clear(Color.Transparent);
            g.DrawImage(file, 0, 0, width, height);
            return image;
        }

        public static Bitmap ResizeAndMerge(Bitmap target, Bitmap source) {
            Bitmap image = new Bitmap(target);
            Graphics g = Graphics.FromImage(image);
            g.DrawImage(source, 0, 0, target.Width, target.Height);
            return image;
        }

        public static Bitmap ExtractTransparency(Bitmap file) {
            Bitmap image = new Bitmap(file);
            LockBitmap source = new LockBitmap(image);
            source.LockBits();
            for (int y = 0; y < image.Height; y++) {
                for (int x = 0; x < image.Width; x++) {
                    Color sourcePixel = source.GetPixel(x, y);
                    Color col = Color.FromArgb(255, sourcePixel.A, sourcePixel.A, sourcePixel.A);
                    source.SetPixel(x, y, col);
                }
            };
            source.UnlockBits();
            return image;
        }
        public static Bitmap ExtractRGB(Bitmap file) {
            Bitmap image = new Bitmap(file);
            LockBitmap source = new LockBitmap(image);
            source.LockBits();
            for (int y = 0; y < image.Height; y++) {
                for (int x = 0; x < image.Width; x++) {
                    Color sourcePixel = source.GetPixel(x, y);
                    Color col = Color.FromArgb(255, sourcePixel.R, sourcePixel.G, sourcePixel.B);
                    source.SetPixel(x, y, col);
                }
            };
            source.UnlockBits();
            return image;
        }

        public static Bitmap MergeAlphaToRRGB(Bitmap alpha, Bitmap rgb) {
            Bitmap image = new Bitmap(rgb.Width, rgb.Height, PixelFormat.Format32bppArgb);
            LockBitmap destination = new LockBitmap(image);
            LockBitmap alphaBits = new LockBitmap(alpha);
            LockBitmap rgbBits = new LockBitmap(rgb);
            alphaBits.LockBits();
            rgbBits.LockBits();
            destination.LockBits();
            for (int y = 0; y < image.Height; y++) {
                for (int x = 0; x < image.Width; x++) {
                    Color alphaPixel = alphaBits.GetPixel(x, y);
                    Color rgbPixel = rgbBits.GetPixel(x, y);
                    Color col = Color.FromArgb(alphaPixel.R, rgbPixel.R, rgbPixel.G, rgbPixel.B);
                    destination.SetPixel(x, y, col);
                }
            };
            alphaBits.UnlockBits();
            rgbBits.UnlockBits();
            destination.UnlockBits();
            return image;
        }

        public static Bitmap MergeNormals(string inputFile, Bitmap diffuse, Bitmap canvasImage, Bitmap normalMask, string diffuseNormal) {
            Graphics g = Graphics.FromImage(canvasImage);
            g.Clear(Color.White);
            g.DrawImage(diffuse, 0, 0, diffuse.Width, diffuse.Height);
            Bitmap normal = Normal.Calculate(canvasImage, normalMask);
            using (Bitmap originalNormal = TexLoader.ResolveBitmap(inputFile)) {
                using (Bitmap destination = new Bitmap(originalNormal, originalNormal.Width, originalNormal.Height)) {
                    try {
                        Bitmap resize = new Bitmap(originalNormal.Width, originalNormal.Height);
                        g = Graphics.FromImage(resize);
                        g.Clear(Color.White);
                        g.DrawImage(normal, 0, 0, originalNormal.Width, originalNormal.Height);
                        KVImage.ImageBlender imageBlender = new KVImage.ImageBlender();
                        return imageBlender.BlendImages(destination, 0, 0, destination.Width, destination.Height, resize, 0, 0, KVImage.ImageBlender.BlendOperation.Blend_Overlay);
                    } catch {
                        return normal;
                    }
                }
            }
        }
    }
}
