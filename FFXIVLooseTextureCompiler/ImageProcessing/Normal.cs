using KVImage;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                for (int y = 0; y < h; y++) {
                    for (int x = 0; x < w; x++) {
                        if (x > 0) { sample_l = image.GetPixel(x - 1, y).GetBrightness(); } else { sample_l = image.GetPixel(x, y).GetBrightness(); }
                        if (x < w) { sample_r = image.GetPixel(x + 1, y).GetBrightness(); } else { sample_r = image.GetPixel(x, y).GetBrightness(); }
                        if (y > 1) { sample_u = image.GetPixel(x, y - 1).GetBrightness(); } else { sample_u = image.GetPixel(x, y).GetBrightness(); }
                        if (y < h) { sample_d = image.GetPixel(x, y + 1).GetBrightness(); } else { sample_d = image.GetPixel(x, y).GetBrightness(); }
                        x_vector = (((sample_l - sample_r) + 1) * .5f) * 255;
                        y_vector = (((sample_u - sample_d) + 1) * .5f) * 255;
                        Color col = Color.FromArgb(255, (int)x_vector, (int)y_vector, 255);
                        normal.SetPixel(x, y, col);
                    }
                }
                return normal;
            }
        }
        public static Bitmap Calculate(Bitmap file) {
            Bitmap image = file;
            image.RotateFlip(RotateFlipType.RotateNoneFlipX);
            #region Global Variables
            int w = image.Width - 1;
            int h = image.Height - 1;
            float sample_l;
            float sample_r;
            float sample_u;
            float sample_d;
            float x_vector;
            float y_vector;
            Bitmap finalNormal = new Bitmap(image.Width, image.Height);
            Bitmap normal = new Bitmap(image.Width, image.Height);
            #endregion
            for (int y = 0; y < h + 1; y++) {
                for (int x = 0; x < w + 1; x++) {
                    if (x > 0) { sample_l = image.GetPixel(x - 1, y).GetBrightness(); } else { sample_l = image.GetPixel(x, y).GetBrightness(); }
                    if (x < w) { sample_r = image.GetPixel(x + 1, y).GetBrightness(); } else { sample_r = image.GetPixel(x, y).GetBrightness(); }
                    if (y > 1) { sample_u = image.GetPixel(x, y - 1).GetBrightness(); } else { sample_u = image.GetPixel(x, y).GetBrightness(); }
                    if (y < h) { sample_d = image.GetPixel(x, y + 1).GetBrightness(); } else { sample_d = image.GetPixel(x, y).GetBrightness(); }
                    x_vector = (((sample_l - sample_r) + 1) * .5f) * 255;
                    y_vector = (((sample_u - sample_d) + 1) * .5f) * 255;
                    Color col = Color.FromArgb(255, (int)x_vector, (int)y_vector, 255);
                    normal.SetPixel(x, y, col);
                }
            }
            normal.RotateFlip(RotateFlipType.RotateNoneFlipX);
            Bitmap normal2 = new Bitmap(normal);
            KVImage.ImageBlender imageBlender = new KVImage.ImageBlender();
            return imageBlender.BlendImages(normal, 0, 0, normal.Width, normal.Height, Contrast.AdjustContrast(normal2, 120), 0, 0, KVImage.ImageBlender.BlendOperation.Blend_Overlay); ;
        }
    }
}
