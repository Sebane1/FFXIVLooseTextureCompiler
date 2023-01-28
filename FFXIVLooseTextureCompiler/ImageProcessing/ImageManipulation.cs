using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFXIVLooseTextureCompiler.ImageProcessing {
    public class ImageManipulation {
        public static Bitmap MergeNormals(string inputFile, Bitmap bitmap, Bitmap target, Bitmap normalMask, string diffuseNormal) {
            Graphics g = Graphics.FromImage(target);
            g.Clear(Color.White);
            g.DrawImage(bitmap, 0, 0, bitmap.Width, bitmap.Height);
            Bitmap normal = Normal.Calculate(target, normalMask);
            using (Bitmap originalNormal = (inputFile.EndsWith(".dds") ? TexLoader.DDSToBitmap(inputFile) : new Bitmap(inputFile))) {
                using (Bitmap destination = new Bitmap(originalNormal, originalNormal.Width, originalNormal.Height)) {
                    try {
                        KVImage.ImageBlender imageBlender = new KVImage.ImageBlender();
                        return imageBlender.BlendImages(destination, 0, 0, destination.Width, destination.Height, normal, 0, 0, KVImage.ImageBlender.BlendOperation.Blend_Overlay);
                    } catch {
                        return normal;
                    }
                }
            }
        }
    }
}
