using System.Drawing;
using System.Drawing.Imaging;
using Rectangle = System.Drawing.Rectangle;

namespace FFXIVLooseTextureCompiler.ImageProcessing {
    public class Contrast {
        //This code is a method for adjusting the contrast of a bitmap image. It takes in a bitmap image and a float value as parameters. The float value is used to adjust
        // the contrast of the image. The code then clones the bitmap image and locks the bits of the new bitmap. It then iterates through each pixel of the image and adjusts
        // the red, green, and blue values of the pixel according to the float value. The red, green, and blue values are adjusted by subtracting 0.5 from them, multiplying
        // them by the float value, and then adding 0.5 back to them. The adjusted values are then multiplied by 255 and cast to integers. The adjusted values are then set
        // as the red, green, and blue values of the pixel. The code then unlocks the bits of the new bitmap and returns it.
        //Optimized

        public static Bitmap AdjustContrast(Bitmap Image, float Value) {
            Value = (100.0f + Value) / 100.0f;
            Value *= Value;
            Bitmap NewBitmap = (Bitmap)Image.Clone();
            BitmapData data = NewBitmap.LockBits(
                new Rectangle(0, 0, NewBitmap.Width, NewBitmap.Height),
                ImageLockMode.ReadWrite,
                NewBitmap.PixelFormat);
            int Height = NewBitmap.Height;
            int Width = NewBitmap.Width;

            unsafe {
                for (int y = 0; y < Height; ++y) {
                    byte* row = (byte*)data.Scan0 + (y * data.Stride);
                    int columnOffset = 0;
                    for (int x = 0; x < Width; ++x) {
                        byte B = row[columnOffset];
                        byte G = row[columnOffset + 1];
                        byte R = row[columnOffset + 2];

                        float Red = R / 255.0f;
                        float Green = G / 255.0f;
                        float Blue = B / 255.0f;
                        Red = (((Red - 0.5f) * Value) + 0.5f) * 255.0f;
                        Green = (((Green - 0.5f) * Value) + 0.5f) * 255.0f;
                        Blue = (((Blue - 0.5f) * Value) + 0.5f) * 255.0f;

                        int iR = (int)Red;
                        iR = Math.Min(255, Math.Max(0, iR));
                        int iG = (int)Green;
                        iG = Math.Min(255, Math.Max(0, iG));
                        int iB = (int)Blue;
                        iB = Math.Min(255, Math.Max(0, iB));

                        row[columnOffset] = (byte)iB;
                        row[columnOffset + 1] = (byte)iG;
                        row[columnOffset + 2] = (byte)iR;

                        columnOffset += 4;
                    }
                }
            }

            NewBitmap.UnlockBits(data);

            return NewBitmap;
        }
    }
}
