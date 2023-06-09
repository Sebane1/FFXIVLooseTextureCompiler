using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Rectangle = System.Drawing.Rectangle;

namespace FFXIVLooseTextureCompiler.ImageProcessing {
    public static class MultiplyFilter {
        public static Bitmap MultiplyImage(this Bitmap bitmap, byte r, byte g, byte b, PixelFormat format = PixelFormat.Format32bppArgb) {
            var size = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            var bitmapData = bitmap.LockBits(size, ImageLockMode.ReadOnly, format);

            var buffer = new byte[bitmapData.Stride * bitmapData.Height];
            Marshal.Copy(bitmapData.Scan0, buffer, 0, buffer.Length);
            bitmap.UnlockBits(bitmapData);

            byte Calc(byte c1, byte c2) {
                var cr = c1 / 255d * c2 / 255d * 255d;
                return (byte)(cr > 255 ? 255 : cr);
            }

            for (var i = 0; i < buffer.Length; i += 4) {
                buffer[i] = Calc(buffer[i], b);
                buffer[i + 1] = Calc(buffer[i + 1], g);
                buffer[i + 2] = Calc(buffer[i + 2], r);
            }

            var result = new Bitmap(bitmap.Width, bitmap.Height);
            var resultData = result.LockBits(size, ImageLockMode.WriteOnly, format);

            Marshal.Copy(buffer, 0, resultData.Scan0, buffer.Length);
            result.UnlockBits(resultData);

            return result;
        }
    }
}
