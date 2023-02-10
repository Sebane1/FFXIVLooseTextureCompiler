﻿using Lumina.Data.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFXIVLooseTextureCompiler.ImageProcessing {
    public class AtramentumLuminisGlow {
        public static Bitmap CalculateDiffuse(Bitmap file, Bitmap glow) {
            Bitmap image = glow;
            Bitmap diffuse = new Bitmap(file);
            LockBitmap source = new LockBitmap(image);
            LockBitmap destination = new LockBitmap(diffuse);
            source.LockBits();
            destination.LockBits();
            if (file.Width == glow.Width && file.Height == glow.Height) {
                for (int y = 0; y < image.Height; y++) {
                    for (int x = 0; x < image.Width; x++) {
                        Color sourcePixel = source.GetPixel(x, y);
                        if (sourcePixel.A > 0) {
                            Color col = Color.FromArgb(255 - sourcePixel.A, sourcePixel.R, sourcePixel.G, sourcePixel.B);
                            destination.SetPixel(x, y, col);
                        }
                    }
                }
            } else {
                MessageBox.Show("Glow merging failed. Images are not the same size.");
            }
            destination.UnlockBits();
            source.UnlockBits();
            return diffuse;
        }
        public static Bitmap CalculateMulti(Bitmap file, Bitmap glow) {
            Bitmap image = glow;
            Bitmap multi = new Bitmap(file);
            LockBitmap source = new LockBitmap(image);
            LockBitmap destination = new LockBitmap(multi);
            source.LockBits();
            destination.LockBits();
            if (file.Width == glow.Width && file.Height == glow.Height) {
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
            } else {
                MessageBox.Show("Glow merging failed. Images are not the same size.");
            }
            destination.UnlockBits();
            source.UnlockBits();
            return multi;
        }
    }
}
