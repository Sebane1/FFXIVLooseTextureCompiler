﻿using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFXIVLooseTextureCompiler.ImageProcessing {
    public class ImageManipulation {
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
