using System.Drawing;
using System.Drawing.Imaging;
using ColorMatrix = System.Drawing.Imaging.ColorMatrix;
using Rectangle = System.Drawing.Rectangle;

namespace FFXIVLooseTextureCompiler.ImageProcessing {
    public class Brightness {
        public static Bitmap BrightenImage(Bitmap originalImage, float brightness = 1.5f, float contrast = 1.0f, float gamma = 1.0f) {
            Bitmap adjustedImage = new Bitmap(originalImage.Width, originalImage.Height);
            float adjustedBrightness = brightness - 1.0f;
            // create matrix that will brighten and contrast the image
            float[][] ptsArray ={
        new float[] {contrast, 0, 0, 0, 0}, // scale red
        new float[] {0, contrast, 0, 0, 0}, // scale green
        new float[] {0, 0, contrast, 0, 0}, // scale blue
        new float[] {0, 0, 0, 1.0f, 0}, // don't scale alpha
        new float[] {adjustedBrightness, adjustedBrightness, adjustedBrightness, 0, 1}};

            ImageAttributes imageAttributes = new ImageAttributes();
            imageAttributes.ClearColorMatrix();
            imageAttributes.SetColorMatrix(new ColorMatrix(ptsArray), ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            imageAttributes.SetGamma(gamma, ColorAdjustType.Bitmap);
            Graphics g = Graphics.FromImage(adjustedImage);
            g.DrawImage(originalImage, new Rectangle(0, 0, adjustedImage.Width, adjustedImage.Height)
                , 0, 0, originalImage.Width, originalImage.Height,
                GraphicsUnit.Pixel, imageAttributes);
            return adjustedImage;
        }
    }
}
