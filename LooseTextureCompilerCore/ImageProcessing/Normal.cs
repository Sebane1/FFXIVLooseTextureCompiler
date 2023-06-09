using System.Drawing;
using Color = System.Drawing.Color;

namespace FFXIVLooseTextureCompiler.ImageProcessing {
    public class Normal {

        public static Bitmap Calculate(string file) {
            using (Bitmap image = (Bitmap)Bitmap.FromFile(file)) {
                int w = image.Width - 1;
                int h = image.Height - 1;
                float sample_l;
                float sample_r;
                float sample_u;
                float sample_d;
                float x_vector;
                float y_vector;
                Bitmap normal = new Bitmap(image.Width, image.Height);
                LockBitmap source = new LockBitmap(image);
                LockBitmap destination = new LockBitmap(normal);
                source.LockBits();
                destination.LockBits();
                float brightness_difference = 255 * 0.5f;
                for (int y = 0; y < h; y++) {
                    for (int x = 0; x < w; x++) {
                        sample_l = x > 0 ? source.GetPixel(x - 1, y).GetBrightness() : source.GetPixel(x, y).GetBrightness();
                        sample_r = x < w ? source.GetPixel(x + 1, y).GetBrightness() : source.GetPixel(x, y).GetBrightness();
                        sample_u = y > 1 ? source.GetPixel(x, y - 1).GetBrightness() : source.GetPixel(x, y).GetBrightness();
                        sample_d = y < h ? source.GetPixel(x, y + 1).GetBrightness() : source.GetPixel(x, y).GetBrightness();
                        x_vector = (((sample_l - sample_r) + 1) * brightness_difference);
                        y_vector = (((sample_u - sample_d) + 1) * brightness_difference);
                        Color col = Color.FromArgb(255, (int)x_vector, (int)y_vector, 255);
                        destination.SetPixel(x, y, col);
                    }
                }
                destination.UnlockBits();
                source.UnlockBits();
                return normal;
            }
        }
        //Optimized

        public static Bitmap Calculate(Bitmap file, Bitmap normalMask = null) {
            Bitmap image = file;
            #region Global Variables
            int w = image.Width - 1;
            int h = image.Height - 1;
            float sample_l;
            float sample_r;
            float sample_u;
            float sample_d;
            float x_vector;
            float y_vector;
            image.RotateFlip(RotateFlipType.RotateNoneFlipX);
            Bitmap normal = new Bitmap(image.Width, image.Height);
            LockBitmap source = new LockBitmap(image);
            LockBitmap destination = new LockBitmap(normal);
            LockBitmap maskReference = null;
            if (normalMask != null) {
                normalMask.RotateFlip(RotateFlipType.RotateNoneFlipX);
                maskReference = new LockBitmap(new Bitmap(normalMask, image.Width, image.Height));
                maskReference.LockBits();
            }
            source.LockBits();
            destination.LockBits();
            #endregion
            for (int y = 0; y < h + 1; y++) {
                for (int x = 0; x < w + 1; x++) {
                    if (normalMask == null || maskReference?.GetPixel(x, y).A == 0) {
                        sample_l = x > 0 ? source.GetPixel(x - 1, y).GetBrightness() : source.GetPixel(x, y).GetBrightness();
                        sample_r = x < w ? source.GetPixel(x + 1, y).GetBrightness() : source.GetPixel(x, y).GetBrightness();
                        sample_u = y > 1 ? source.GetPixel(x, y - 1).GetBrightness() : source.GetPixel(x, y).GetBrightness();
                        sample_d = y < h ? source.GetPixel(x, y + 1).GetBrightness() : source.GetPixel(x, y).GetBrightness();
                        x_vector = (((sample_l - sample_r) + 1) * .5f) * 255;
                        y_vector = (((sample_u - sample_d) + 1) * .5f) * 255;
                        Color col = Color.FromArgb(255, (int)x_vector, (int)y_vector, 255);
                        destination.SetPixel(x, y, col);
                    } else {
                        destination.SetPixel(x, y, Color.FromArgb(255, 128, 127, 255));
                    }
                }
            }
            destination.UnlockBits();
            source.UnlockBits();
            maskReference?.UnlockBits();
            normal.RotateFlip(RotateFlipType.RotateNoneFlipX);
            Bitmap normal2 = new Bitmap(normal);
            KVImage.ImageBlender imageBlender = new KVImage.ImageBlender();
            return imageBlender.BlendImages(normal, 0, 0, normal.Width, normal.Height,
                Contrast.AdjustContrast(normal2, 120), 0, 0, KVImage.ImageBlender.BlendOperation.Blend_Overlay);
        }
    }
}
