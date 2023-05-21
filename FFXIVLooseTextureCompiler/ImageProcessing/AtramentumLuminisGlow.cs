namespace FFXIVLooseTextureCompiler.ImageProcessing {
    public class AtramentumLuminisGlow {
        public static Bitmap CalculateDiffuse(Bitmap file, Bitmap glow) {
            Bitmap image = new Bitmap(glow, file.Width, file.Height);
            Bitmap diffuse = new Bitmap(file);
            Bitmap mergedImage = new Bitmap(diffuse);

            Bitmap glowMultiply = new Bitmap(mergedImage);
            Graphics g = Graphics.FromImage(glowMultiply);
            g.Clear(Color.White);
            g.DrawImage(glow, 0, 0, glow.Width, glow.Height);
            new KVImage.ImageBlender().BlendImages(mergedImage, glowMultiply, KVImage.ImageBlender.BlendOperation.Blend_Multiply);

            LockBitmap source = new LockBitmap(image);
            LockBitmap destination = new LockBitmap(diffuse);
            LockBitmap mergedImagePixels = new LockBitmap(mergedImage);
            source.LockBits();
            destination.LockBits();
            mergedImagePixels.LockBits();
            for (int y = 0; y < image.Height; y++) {
                for (int x = 0; x < image.Width; x++) {
                    Color sourcePixel = source.GetPixel(x, y);
                    Color mergedPixel = mergedImagePixels.GetPixel(x, y);
                    if (sourcePixel.A > 20) {
                        Color col = Color.FromArgb(255 - sourcePixel.A, sourcePixel.R, sourcePixel.G, sourcePixel.B);
                        destination.SetPixel(x, y, col);
                    } else if (sourcePixel.A > 10) {
                        Color col = Color.FromArgb(255 - sourcePixel.A, mergedPixel.R, mergedPixel.G, mergedPixel.B);
                        destination.SetPixel(x, y, col);
                    } else if (sourcePixel.A > 0) {
                        Color col = Color.FromArgb(mergedPixel.A, mergedPixel.R, mergedPixel.G, mergedPixel.B);
                        destination.SetPixel(x, y, col);
                    }
                }
            }
            destination.UnlockBits();
            source.UnlockBits();
            mergedImagePixels.UnlockBits();
            return diffuse;
        }

        public static Bitmap CalculateEyeMulti(Bitmap file, Bitmap glow) {
            Bitmap image = new Bitmap(glow, file.Width, file.Height);
            Bitmap multi = new Bitmap(file);
            LockBitmap source = new LockBitmap(image);
            LockBitmap destination = new LockBitmap(multi);
            source.LockBits();
            destination.LockBits();
            for (int y = 0; y < image.Height; y++) {
                for (int x = 0; x < image.Width; x++) {
                    Color sourcePixel = source.GetPixel(x, y);
                    Color destinationPixel = destination.GetPixel(x, y);
                    if (sourcePixel.A > 0) {
                        Color col = Color.FromArgb(255 - sourcePixel.A, destinationPixel.R, destinationPixel.G, destinationPixel.B);
                        destination.SetPixel(x, y, col);
                    }
                }
            }
            destination.UnlockBits();
            source.UnlockBits();
            return multi;
        }

        public static Bitmap CalculateMulti(Bitmap file, Bitmap glow) {
            Bitmap image = new Bitmap(glow, file.Width, file.Height);
            Bitmap multi = new Bitmap(file);
            LockBitmap source = new LockBitmap(image);
            LockBitmap destination = new LockBitmap(multi);
            source.LockBits();
            destination.LockBits();
            for (int y = 0; y < image.Height; y++) {
                for (int x = 0; x < image.Width; x++) {
                    Color sourcePixel = source.GetPixel(x, y);
                    if (sourcePixel.A > 0) {
                        Color col = Color.FromArgb(sourcePixel.A, sourcePixel.R, sourcePixel.G, 255 - sourcePixel.A);
                        destination.SetPixel(x, y, col);
                    }
                }
            }
            destination.UnlockBits();
            source.UnlockBits();
            return multi;
        }

        public static Bitmap TransplantData(Bitmap file, Bitmap glow) {
            Bitmap image = glow;
            Bitmap diffuse = new Bitmap(file);
            Bitmap mergedImage = new Bitmap(diffuse);

            Bitmap glowMultiply = new Bitmap(mergedImage);
            Graphics g = Graphics.FromImage(glowMultiply);
            g.Clear(Color.White);
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
                        Color sourcePixel = source.GetPixel(x, y);
                        Color mergedPixel = mergedImagePixels.GetPixel(x, y);
                        if (sourcePixel.A > 0) {
                            Color col = Color.FromArgb(sourcePixel.A, sourcePixel.R, sourcePixel.G, sourcePixel.B);
                            destination.SetPixel(x, y, col);
                        }
                    }
                }
            } else {
                MessageBox.Show("Glow merging failed with diffuse. Images are not the same size.");
            }
            destination.UnlockBits();
            source.UnlockBits();
            mergedImagePixels.UnlockBits();
            return diffuse;
        }

        public static Bitmap ExtractGlowMapFormLegacyDiffuse(Bitmap file) {
            Bitmap image = new Bitmap(file);
            LockBitmap source = new LockBitmap(image);
            source.LockBits();
            for (int y = 0; y < image.Height; y++) {
                for (int x = 0; x < image.Width; x++) {
                    Color sourcePixel = source.GetPixel(x, y);
                    Color col = Color.FromArgb(255 - sourcePixel.A, sourcePixel.R, sourcePixel.G, sourcePixel.B);
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
