using System.Drawing;
using Bitmap = System.Drawing;

namespace FFXIVLooseTextureCompiler.ImageProcessing {
    public class AtramentumLuminisGlow {
        public static Bitmap.Bitmap CalculateDiffuse(Bitmap.Bitmap file, Bitmap.Bitmap  glow) {
            Bitmap.Bitmap  image = new Bitmap.Bitmap (glow, file.Width, file.Height);
            Bitmap.Bitmap  diffuse = new Bitmap.Bitmap (file);
            Bitmap.Bitmap  mergedImage = new Bitmap.Bitmap (diffuse);
            //Bitmap.Bitmap  debugImage = new Bitmap.Bitmap (file.Width, file.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            Bitmap.Bitmap  glowMultiply = new Bitmap.Bitmap (mergedImage);
            Graphics g = Graphics.FromImage(glowMultiply);
            g.Clear(Bitmap.Color.White);
            g.DrawImage(glow, 0, 0, glow.Width, glow.Height);
            new KVImage.ImageBlender().BlendImages(mergedImage, glowMultiply, KVImage.ImageBlender.BlendOperation.Blend_Multiply);

            LockBitmap source = new LockBitmap(image);
            LockBitmap destination = new LockBitmap(diffuse);
            LockBitmap mergedImagePixels = new LockBitmap(mergedImage);
            //LockBitmap debug = new LockBitmap(debugImage);

            source.LockBits();
            destination.LockBits();
            //debug.LockBits();
            mergedImagePixels.LockBits();
            for (int y = 0; y < image.Height; y++) {
                for (int x = 0; x < image.Width; x++) {
                    Bitmap.Color sourcePixel = source.GetPixel(x, y);
                    Bitmap.Color mergedPixel = mergedImagePixels.GetPixel(x, y);
                    Bitmap.Color comparisonColour = FlattenColours(sourcePixel);
                    if (!(comparisonColour.R == 0 && comparisonColour.G == 0 && comparisonColour.B == 0)) {
                        if (sourcePixel.A > 20) {
                            Bitmap.Color col = Bitmap.Color.FromArgb(255 - sourcePixel.A, sourcePixel.R, sourcePixel.G, sourcePixel.B);
                            destination.SetPixel(x, y, col);
                        } else if (sourcePixel.A > 10) {
                            Bitmap.Color col = Bitmap.Color.FromArgb(255 - sourcePixel.A, mergedPixel.R, mergedPixel.G, mergedPixel.B);
                            destination.SetPixel(x, y, col);
                        } else if (sourcePixel.A > 0) {
                            Bitmap.Color col = Bitmap.Color.FromArgb(mergedPixel.A, mergedPixel.R, mergedPixel.G, mergedPixel.B);
                            destination.SetPixel(x, y, col);
                        }
                        //debug.SetPixel(x, y, sourcePixel);
                    } else {
                        Bitmap.Color col = Bitmap.Color.FromArgb(mergedPixel.A, mergedPixel.R, mergedPixel.G, mergedPixel.B);
                        destination.SetPixel(x, y, col);
                    }
                }
            }
            destination.UnlockBits();
            source.UnlockBits();
            mergedImagePixels.UnlockBits();
            //debug.UnlockBits();
            //debugImage.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "atramentumTest.png"));
            return diffuse;
        }
        public static Bitmap.Color FlattenColours(Bitmap.Color colour, int minBrightness = 90) {
            return Bitmap.Color.FromArgb(colour.A,
                colour.R > minBrightness ? colour.R : 0,
                colour.G > minBrightness ? colour.G : 0,
                colour.B > minBrightness ? colour.B : 0);
        }
        public static Bitmap.Bitmap  CalculateEyeMulti(Bitmap.Bitmap  file, Bitmap.Bitmap  glow) {
            Bitmap.Bitmap  image = new Bitmap.Bitmap (glow, file.Width, file.Height);
            Bitmap.Bitmap  multi = new Bitmap.Bitmap (file);
            LockBitmap source = new LockBitmap(image);
            LockBitmap destination = new LockBitmap(multi);
            source.LockBits();
            destination.LockBits();
            for (int y = 0; y < image.Height; y++) {
                for (int x = 0; x < image.Width; x++) {
                    Bitmap.Color sourcePixel = source.GetPixel(x, y);
                    Bitmap.Color destinationPixel = destination.GetPixel(x, y);
                    if (sourcePixel.A > 0) {
                        Bitmap.Color col = Bitmap.Color.FromArgb(255 - sourcePixel.A, destinationPixel.R, destinationPixel.G, destinationPixel.B);
                        destination.SetPixel(x, y, col);
                    }
                }
            }
            destination.UnlockBits();
            source.UnlockBits();
            return multi;
        }

        public static Bitmap.Bitmap  CalculateMulti(Bitmap.Bitmap  file, Bitmap.Bitmap  glow) {
            Bitmap.Bitmap  image = new Bitmap.Bitmap (glow, file.Width, file.Height);
            Bitmap.Bitmap  multi = new Bitmap.Bitmap (file);
            LockBitmap source = new LockBitmap(image);
            LockBitmap destination = new LockBitmap(multi);
            source.LockBits();
            destination.LockBits();
            for (int y = 0; y < image.Height; y++) {
                for (int x = 0; x < image.Width; x++) {
                    Bitmap.Color sourcePixel = source.GetPixel(x, y);
                    Bitmap.Color destinationPixel = destination.GetPixel(x, y);
                    Bitmap.Color comparisonColour = FlattenColours(sourcePixel, 90);
                    if (!(comparisonColour.R == 0 && comparisonColour.G == 0 && comparisonColour.B == 0)) {
                        if (sourcePixel.A > 20) {
                            Bitmap.Color col = Bitmap.Color.FromArgb(destinationPixel.A, destinationPixel.R, destinationPixel.G, 255 - sourcePixel.A);
                            destination.SetPixel(x, y, col);
                        }
                    }
                }
            }
            destination.UnlockBits();
            source.UnlockBits();
            return multi;
        }

        public static Bitmap.Bitmap  TransplantData(Bitmap.Bitmap  file, Bitmap.Bitmap  glow) {
            Bitmap.Bitmap  image = glow;
            Bitmap.Bitmap  diffuse = new Bitmap.Bitmap (file);
            Bitmap.Bitmap  mergedImage = new Bitmap.Bitmap (diffuse);

            Bitmap.Bitmap  glowMultiply = new Bitmap.Bitmap (mergedImage);
            Graphics g = Graphics.FromImage(glowMultiply);
            g.Clear(Bitmap.Color.White);
            g.DrawImage(glow, 0, 0, glow.Width, glow.Height);
            new KVImage.ImageBlender().BlendImages(mergedImage, glowMultiply, KVImage.ImageBlender.BlendOperation.Blend_Multiply);

            LockBitmap source = new LockBitmap(image);
            LockBitmap destination = new LockBitmap(diffuse);
            LockBitmap mergedImagePixels = new LockBitmap(mergedImage);
            source.LockBits();
            destination.LockBits();
            mergedImagePixels.LockBits();
            if (file.Width == glow.Width && file.Height == glow.Height) {
                for (int y = 0; y < image.Height; y++) {
                    for (int x = 0; x < image.Width; x++) {
                        Bitmap.Color sourcePixel = source.GetPixel(x, y);
                        Bitmap.Color mergedPixel = mergedImagePixels.GetPixel(x, y);
                        if (sourcePixel.A > 0) {
                            Bitmap.Color col = Bitmap.Color.FromArgb(sourcePixel.A, sourcePixel.R, sourcePixel.G, sourcePixel.B);
                            destination.SetPixel(x, y, col);
                        }
                    }
                }
            }
            destination.UnlockBits();
            source.UnlockBits();
            mergedImagePixels.UnlockBits();
            return diffuse;
        }

        public static Bitmap.Bitmap  ExtractGlowMapFormLegacyDiffuse(Bitmap.Bitmap  file) {
            Bitmap.Bitmap  image = new Bitmap.Bitmap (file);
            LockBitmap source = new LockBitmap(image);
            source.LockBits();
            for (int y = 0; y < image.Height; y++) {
                for (int x = 0; x < image.Width; x++) {
                    Bitmap.Color sourcePixel = source.GetPixel(x, y);
                    Bitmap.Color col = Bitmap.Color.FromArgb(255 - sourcePixel.A, sourcePixel.R, sourcePixel.G, sourcePixel.B);
                    source.SetPixel(x, y, col);
                }
            }
            source.UnlockBits();
            return image;
        }

        byte Calc(byte c1, byte c2) {
            var cr = c1 / 255d * c2 / 255d * 255d;
            return (byte)(cr > 255 ? 255 : cr);
        }
    }
}
