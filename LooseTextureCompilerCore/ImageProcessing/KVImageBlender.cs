using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Bitmap = System.Drawing.Bitmap;
using ColorMatrix = System.Drawing.Imaging.ColorMatrix;
using Image = System.Drawing.Image;
using Rectangle = System.Drawing.Rectangle;

namespace KVImage {
    public class ImageBlender {
        public enum BlendOperation : int {
            SourceCopy = 1,
            ROP_MergePaint,
            ROP_NOTSourceErase,
            ROP_SourceAND,
            ROP_SourceErase,
            ROP_SourceInvert,
            ROP_SourcePaint,
            Blend_Darken,
            Blend_Multiply,
            Blend_ColorBurn,
            Blend_Lighten,
            Blend_Screen,
            Blend_ColorDodge,
            Blend_Overlay,
            Blend_SoftLight,
            Blend_HardLight,
            Blend_PinLight,
            Blend_Difference,
            Blend_Exclusion,
            Blend_Hue,
            Blend_Saturation,
            Blend_Color,
            Blend_Luminosity
        }

        // NTSC defined color weights
        public const float R_WEIGHT = 0.299f;
        public const float G_WEIGHT = 0.587f;
        public const float B_WEIGHT = 0.144f;

        public const ushort HLSMAX = 360;
        public const byte RGBMAX = 255;
        public const byte HUNDEFINED = 0;

        private delegate byte PerChannelProcessDelegate(ref byte nSrc, ref byte nDst);
        private delegate void RGBProcessDelegate(byte sR, byte sG, byte sB, ref byte dR, ref byte dG, ref byte dB);

        // Invert image
        public void Invert(Image img) {
            if (img == null)
                throw new Exception("Image must be provided");

            ColorMatrix cMatrix = new ColorMatrix(new float[][] {
                        new float[] {-1.0f, 0.0f, 0.0f, 0.0f, 0.0f },
                        new float[] { 0.0f,-1.0f, 0.0f, 0.0f, 0.0f },
                        new float[] { 0.0f, 0.0f,-1.0f, 0.0f, 0.0f },
                        new float[] { 0.0f, 0.0f, 0.0f, 1.0f, 0.0f },
                        new float[] { 1.0f, 1.0f, 1.0f, 0.0f, 1.0f }
                    });
            ApplyColorMatrix(ref img, cMatrix);
        }

        // Adjustment values are between -1.0 and 1.0
        public void AdjustBrightness(Image img, float adjValueR, float adjValueG, float adjValueB) {
            if (img == null)
                throw new Exception("Image must be provided");

            ColorMatrix cMatrix = new ColorMatrix(new float[][] {
                        new float[] { 1.0f, 0.0f, 0.0f, 0.0f, 0.0f },
                        new float[] { 0.0f, 1.0f, 0.0f, 0.0f, 0.0f },
                        new float[] { 0.0f, 0.0f, 1.0f, 0.0f, 0.0f },
                        new float[] { 0.0f, 0.0f, 0.0f, 1.0f, 0.0f },
                        new float[] { adjValueR, adjValueG, adjValueB, 0.0f, 1.0f }
                    });
            ApplyColorMatrix(ref img, cMatrix);
        }


        // Adjustment values are between -1.0 and 1.0
        public void AdjustBrightness(Image img, float adjValue) {
            AdjustBrightness(img, adjValue, adjValue, adjValue);
        }


        // Saturation. 0.0 = desaturate, 1.0 = identity, -1.0 = complementary colors
        public void AdjustSaturation(Image img, float sat, float rweight, float gweight, float bweight) {
            if (img == null)
                throw new Exception("Image must be provided");

            ColorMatrix cMatrix = new ColorMatrix(new float[][] {
                        new float[] { (1.0f-sat)*rweight+sat, (1.0f-sat)*rweight, (1.0f-sat)*rweight, 0.0f, 0.0f },
                        new float[] { (1.0f-sat)*gweight, (1.0f-sat)*gweight+sat, (1.0f-sat)*gweight, 0.0f, 0.0f },
                        new float[] { (1.0f-sat)*bweight, (1.0f-sat)*bweight, (1.0f-sat)*bweight+sat, 0.0f, 0.0f },
                        new float[] { 0.0f, 0.0f, 0.0f, 1.0f, 0.0f },
                        new float[] { 0.0f, 0.0f, 0.0f, 0.0f, 1.0f }
                    });
            ApplyColorMatrix(ref img, cMatrix);
        }


        // Saturation. 0.0 = desaturate, 1.0 = identity, -1.0 = complementary colors
        public void AdjustSaturation(Image img, float sat) {
            AdjustSaturation(img, sat, R_WEIGHT, G_WEIGHT, B_WEIGHT);
        }


        // Weights between 0.0 and 1.0
        public void Desaturate(Image img, float RWeight, float GWeight, float BWeight) {
            AdjustSaturation(img, 0.0f, RWeight, GWeight, BWeight);
        }


        // Desaturate using "default" NTSC defined color weights
        public void Desaturate(Image img) {
            AdjustSaturation(img, 0.0f, R_WEIGHT, G_WEIGHT, B_WEIGHT);
        }


        public void ApplyColorMatrix(ref Image img, ColorMatrix colMatrix) {
            Graphics gr = Graphics.FromImage(img);
            ImageAttributes attrs = new ImageAttributes();
            attrs.SetColorMatrix(colMatrix);
            gr.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height),
                0, 0, img.Width, img.Height, GraphicsUnit.Pixel, attrs);
            gr.Dispose();
        }


        #region BlendImages functions ...
        /* 
			destImage - image that will be used as background
			destX, destY - define position on destination image where to start applying blend operation
			destWidth, destHeight - width and height of the area to apply blending
			srcImage - image to use as foreground (source of blending)	
			srcX, srcY - starting position of the source image 	  
		*/
        public Bitmap BlendImages(Image destImage, int destX, int destY, int destWidth, int destHeight,
                                Image srcImage, int srcX, int srcY, BlendOperation BlendOp) {
            if (destImage == null)
                throw new Exception("Destination image must be provided");

            if (destImage.Width < destX + destWidth || destImage.Height < destY + destHeight)
                throw new Exception("Destination image is smaller than requested dimentions");

            if (srcImage == null)
                throw new Exception("Source image must be provided");

            if (srcImage.Width < srcX + destWidth || srcImage.Height < srcY + destHeight) {
                throw new Exception("Source image is smaller than requested dimentions");
            }

            Bitmap tempBmp = null;
            Graphics gr = Graphics.FromImage(destImage);
            gr.CompositingMode = CompositingMode.SourceCopy;

            switch (BlendOp) {
                case BlendOperation.SourceCopy:
                    gr.DrawImage(srcImage, new Rectangle(destX, destY, destWidth, destHeight),
                            srcX, srcY, destWidth, destHeight, GraphicsUnit.Pixel);
                    break;

                case BlendOperation.ROP_MergePaint:
                    tempBmp = PerChannelProcess(ref destImage, destX, destY, destWidth, destHeight,
                                ref srcImage, srcX, srcY, new PerChannelProcessDelegate(MergePaint));
                    break;

                case BlendOperation.ROP_NOTSourceErase:
                    tempBmp = PerChannelProcess(ref destImage, destX, destY, destWidth, destHeight,
                        ref srcImage, srcX, srcY, new PerChannelProcessDelegate(NOTSourceErase));
                    break;

                case BlendOperation.ROP_SourceAND:
                    tempBmp = PerChannelProcess(ref destImage, destX, destY, destWidth, destHeight,
                        ref srcImage, srcX, srcY, new PerChannelProcessDelegate(SourceAND));
                    break;

                case BlendOperation.ROP_SourceErase:
                    tempBmp = PerChannelProcess(ref destImage, destX, destY, destWidth, destHeight,
                        ref srcImage, srcX, srcY, new PerChannelProcessDelegate(SourceErase));
                    break;

                case BlendOperation.ROP_SourceInvert:
                    tempBmp = PerChannelProcess(ref destImage, destX, destY, destWidth, destHeight,
                        ref srcImage, srcX, srcY, new PerChannelProcessDelegate(SourceInvert));
                    break;

                case BlendOperation.ROP_SourcePaint:
                    tempBmp = PerChannelProcess(ref destImage, destX, destY, destWidth, destHeight,
                        ref srcImage, srcX, srcY, new PerChannelProcessDelegate(SourcePaint));
                    break;

                case BlendOperation.Blend_Darken:
                    tempBmp = PerChannelProcess(ref destImage, destX, destY, destWidth, destHeight,
                        ref srcImage, srcX, srcY, new PerChannelProcessDelegate(BlendDarken));
                    break;

                case BlendOperation.Blend_Multiply:
                    tempBmp = PerChannelProcess(ref destImage, destX, destY, destWidth, destHeight,
                        ref srcImage, srcX, srcY, new PerChannelProcessDelegate(BlendMultiply));
                    break;

                case BlendOperation.Blend_Screen:
                    tempBmp = PerChannelProcess(ref destImage, destX, destY, destWidth, destHeight,
                        ref srcImage, srcX, srcY, new PerChannelProcessDelegate(BlendScreen));
                    break;

                case BlendOperation.Blend_Lighten:
                    tempBmp = PerChannelProcess(ref destImage, destX, destY, destWidth, destHeight,
                        ref srcImage, srcX, srcY, new PerChannelProcessDelegate(BlendLighten));
                    break;

                case BlendOperation.Blend_HardLight:
                    tempBmp = PerChannelProcess(ref destImage, destX, destY, destWidth, destHeight,
                        ref srcImage, srcX, srcY, new PerChannelProcessDelegate(BlendHardLight));
                    break;

                case BlendOperation.Blend_Difference:
                    tempBmp = PerChannelProcess(ref destImage, destX, destY, destWidth, destHeight,
                        ref srcImage, srcX, srcY, new PerChannelProcessDelegate(BlendDifference));
                    break;

                case BlendOperation.Blend_PinLight:
                    tempBmp = PerChannelProcess(ref destImage, destX, destY, destWidth, destHeight,
                        ref srcImage, srcX, srcY, new PerChannelProcessDelegate(BlendPinLight));
                    break;

                case BlendOperation.Blend_Overlay:
                    tempBmp = PerChannelProcess(ref destImage, destX, destY, destWidth, destHeight,
                        ref srcImage, srcX, srcY, new PerChannelProcessDelegate(BlendOverlay));
                    break;

                case BlendOperation.Blend_Exclusion:
                    tempBmp = PerChannelProcess(ref destImage, destX, destY, destWidth, destHeight,
                        ref srcImage, srcX, srcY, new PerChannelProcessDelegate(BlendExclusion));
                    break;

                case BlendOperation.Blend_SoftLight:
                    tempBmp = PerChannelProcess(ref destImage, destX, destY, destWidth, destHeight,
                        ref srcImage, srcX, srcY, new PerChannelProcessDelegate(BlendSoftLight));
                    break;

                case BlendOperation.Blend_ColorBurn:
                    tempBmp = PerChannelProcess(ref destImage, destX, destY, destWidth, destHeight,
                        ref srcImage, srcX, srcY, new PerChannelProcessDelegate(BlendColorBurn));
                    break;

                case BlendOperation.Blend_ColorDodge:
                    tempBmp = PerChannelProcess(ref destImage, destX, destY, destWidth, destHeight,
                        ref srcImage, srcX, srcY, new PerChannelProcessDelegate(BlendColorDodge));
                    break;

                case BlendOperation.Blend_Hue:
                    tempBmp = RGBProcess(ref destImage, destX, destY, destWidth, destHeight,
                        ref srcImage, srcX, srcY, new RGBProcessDelegate(BlendHue));
                    break;

                case BlendOperation.Blend_Saturation:
                    tempBmp = RGBProcess(ref destImage, destX, destY, destWidth, destHeight,
                        ref srcImage, srcX, srcY, new RGBProcessDelegate(BlendSaturation));
                    break;

                case BlendOperation.Blend_Color:
                    tempBmp = RGBProcess(ref destImage, destX, destY, destWidth, destHeight,
                        ref srcImage, srcX, srcY, new RGBProcessDelegate(BlendColor));
                    break;

                case BlendOperation.Blend_Luminosity:
                    tempBmp = RGBProcess(ref destImage, destX, destY, destWidth, destHeight,
                        ref srcImage, srcX, srcY, new RGBProcessDelegate(BlendLuminosity));
                    break;
            }

            if (tempBmp != null) {
                gr.DrawImage(tempBmp, 0, 0, tempBmp.Width, tempBmp.Height);
                return tempBmp;
            }

            gr.Dispose();
            gr = null;
            return null;
        }


        public void BlendImages(Image destImage, Image srcImage, BlendOperation BlendOp) {
            BlendImages(destImage, 0, 0, destImage.Width, destImage.Height, srcImage, 0, 0, BlendOp);
        }

        public void BlendImages(Image destImage, BlendOperation BlendOp) {
            BlendImages(destImage, 0, 0, destImage.Width, destImage.Height, null, 0, 0, BlendOp);
        }

        public void BlendImages(Image destImage, int destX, int destY, BlendOperation BlendOp) {
            BlendImages(destImage, destX, destY, destImage.Width - destX, destImage.Height - destY, null, 0, 0, BlendOp);
        }

        public void BlendImages(Image destImage, int destX, int destY, int destWidth, int destHeight, BlendOperation BlendOp) {
            BlendImages(destImage, destX, destY, destWidth, destHeight, null, 0, 0, BlendOp);
        }
        #endregion

        #region Private Blending Functions ...

        private Bitmap PerChannelProcess(ref Image destImg, int destX, int destY, int destWidth, int destHeight,
                                ref Image srcImg, int srcX, int srcY,
                                PerChannelProcessDelegate ChannelProcessFunction) {
            Bitmap dst = new Bitmap(destImg);
            Bitmap src = new Bitmap(srcImg);

            BitmapData dstBD = dst.LockBits(new Rectangle(destX, destY, destWidth, destHeight), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData srcBD = src.LockBits(new Rectangle(srcX, srcY, destWidth, destHeight), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int dstStride = dstBD.Stride;
            int srcStride = srcBD.Stride;

            System.IntPtr dstScan0 = dstBD.Scan0;
            System.IntPtr srcScan0 = srcBD.Scan0;

            unsafe {
                byte* pDst = (byte*)(void*)dstScan0;
                byte* pSrc = (byte*)(void*)srcScan0;

                for (int y = 0; y < destHeight; y++) {
                    for (int x = 0; x < destWidth * 3; x++) {
                        pDst[x + y * dstStride] = ChannelProcessFunction(ref pSrc[x + y * srcStride], ref pDst[x + y * dstStride]);
                    }
                }
            }

            src.UnlockBits(srcBD);
            dst.UnlockBits(dstBD);

            src.Dispose();

            return dst;
        }

        private Bitmap RGBProcess(ref Image destImg, int destX, int destY, int destWidth, int destHeight,
            ref Image srcImg, int srcX, int srcY,
            RGBProcessDelegate RGBProcessFunction) {
            Bitmap dst = new Bitmap(destImg);
            Bitmap src = new Bitmap(srcImg);

            BitmapData dstBD = dst.LockBits(new Rectangle(destX, destY, destWidth, destHeight), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData srcBD = src.LockBits(new Rectangle(srcX, srcY, destWidth, destHeight), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int dstStride = dstBD.Stride;
            int srcStride = srcBD.Stride;

            System.IntPtr dstScan0 = dstBD.Scan0;
            System.IntPtr srcScan0 = srcBD.Scan0;

            unsafe {
                byte* pDst = (byte*)(void*)dstScan0;
                byte* pSrc = (byte*)(void*)srcScan0;

                for (int y = 0; y < destHeight; y++) {
                    for (int x = 0; x < destWidth; x++) {
                        RGBProcessFunction(
                                pSrc[x * 3 + 2 + y * srcStride], pSrc[x * 3 + 1 + y * srcStride], pSrc[x * 3 + y * srcStride],
                                ref pDst[x * 3 + 2 + y * dstStride], ref pDst[x * 3 + 1 + y * dstStride], ref pDst[x * 3 + y * dstStride]
                            );
                    }
                }
            }

            src.UnlockBits(srcBD);
            dst.UnlockBits(dstBD);

            src.Dispose();

            return dst;
        }

        #endregion

        #region HLS Conversion Functions ...

        public void RGBToHLS(byte R, byte G, byte B, out ushort H, out ushort L, out ushort S) {
            byte cMax, cMin;      /* max and min RGB values */
            float Rdelta, Gdelta, Bdelta; /* intermediate value: % of spread from max */

            /* calculate lightness */
            cMax = Math.Max(Math.Max(R, G), B);
            cMin = Math.Min(Math.Min(R, G), B);
            L = (ushort)((((cMax + cMin) * HLSMAX) + RGBMAX) / (2 * RGBMAX));

            if (cMax == cMin) {/* r=g=b --> achromatic case */
                S = 0;                     /* saturation */
                H = HUNDEFINED;             /* hue */
            } else {/* chromatic case */
                /* saturation */
                if (L <= (HLSMAX / 2))
                    S = (ushort)((((cMax - cMin) * HLSMAX) + ((cMax + cMin) / 2)) / (cMax + cMin));
                else
                    S = (ushort)((((cMax - cMin) * HLSMAX) + ((2 * RGBMAX - cMax - cMin) / 2)) / (2 * RGBMAX - cMax - cMin));

                /* hue */
                Rdelta = (float)((((cMax - R) * (HLSMAX / 6)) + ((cMax - cMin) / 2)) / (cMax - cMin));
                Gdelta = (float)((((cMax - G) * (HLSMAX / 6)) + ((cMax - cMin) / 2)) / (cMax - cMin));
                Bdelta = (float)((((cMax - B) * (HLSMAX / 6)) + ((cMax - cMin) / 2)) / (cMax - cMin));

                if (R == cMax)
                    H = (ushort)(Bdelta - Gdelta);
                else if (G == cMax)
                    H = (ushort)((HLSMAX / 3) + Rdelta - Bdelta);
                else /* B == cMax */
                    H = (ushort)(((2 * HLSMAX) / 3) + Gdelta - Rdelta);

                if (H < 0)
                    H += HLSMAX;
                if (H > HLSMAX)
                    H -= HLSMAX;
            }
        }

        public void HLSToRGB(ushort H, ushort L, ushort S, out byte R, out byte G, out byte B) {
            float Magic1, Magic2;       /* calculated magic numbers (really!) */

            if (S == 0) {/* achromatic case */
                R = G = B = (byte)((L * RGBMAX) / HLSMAX);
            } else {/* chromatic case */
                /* set up magic numbers */
                if (L <= (HLSMAX / 2))
                    Magic2 = (float)((L * (HLSMAX + S) + (HLSMAX / 2)) / HLSMAX);
                else
                    Magic2 = (float)(L + S - ((L * S) + (HLSMAX / 2)) / HLSMAX);

                Magic1 = (float)(2 * L - Magic2);

                /* get RGB, change units from HLSMAX to RGBMAX */
                R = (byte)((HueToRGB(Magic1, Magic2, H + (HLSMAX / 3)) * RGBMAX + (HLSMAX / 2)) / HLSMAX);
                G = (byte)((HueToRGB(Magic1, Magic2, H) * RGBMAX + (HLSMAX / 2)) / HLSMAX);
                B = (byte)((HueToRGB(Magic1, Magic2, H - (HLSMAX / 3)) * RGBMAX + (HLSMAX / 2)) / HLSMAX);
            }
        }

        /* utility routine for HLStoRGB */
        private float HueToRGB(float n1, float n2, float hue) {
            /* range check: note values passed add/subtract thirds of range */
            if (hue < 0)
                hue += HLSMAX;

            if (hue > HLSMAX)
                hue -= HLSMAX;

            /* return r,g, or b value from this tridrant */
            if (hue < (HLSMAX / 6))
                return (float)(n1 + (((n2 - n1) * hue + (HLSMAX / 12)) / (HLSMAX / 6)));
            if (hue < (HLSMAX / 2))
                return (float)(n2);
            if (hue < ((HLSMAX * 2) / 3))
                return (float)(n1 + (((n2 - n1) * (((HLSMAX * 2) / 3) - hue) + (HLSMAX / 12)) / (HLSMAX / 6)));
            else
                return (float)(n1);
        }
        #endregion

        #region Raster Operation Functions ...

        // (NOT Source) OR Destination
        private byte MergePaint(ref byte Src, ref byte Dst) {
            return (byte)Math.Max(Math.Min((255 - Src) | Dst, 255), 0);
        }

        // NOT (Source OR Destination)
        private byte NOTSourceErase(ref byte Src, ref byte Dst) {
            return (byte)Math.Max(Math.Min(255 - (Src | Dst), 255), 0);
        }

        // Source AND Destination
        private byte SourceAND(ref byte Src, ref byte Dst) {
            return (byte)Math.Max(Math.Min(Src & Dst, 255), 0);
        }

        // Source AND (NOT Destination)
        private byte SourceErase(ref byte Src, ref byte Dst) {
            return (byte)Math.Max(Math.Min(Src & (255 - Dst), 255), 0);
        }

        // Source XOR Destination
        private byte SourceInvert(ref byte Src, ref byte Dst) {
            return (byte)Math.Max(Math.Min(Src ^ Dst, 255), 0);
        }

        // Source OR Destination
        private byte SourcePaint(ref byte Src, ref byte Dst) {
            return (byte)Math.Max(Math.Min(Src | Dst, 255), 0);
        }

        #endregion

        #region Blend Pixels Functions ...
        // Choose darkest color 
        private byte BlendDarken(ref byte Src, ref byte Dst) {
            return ((Src < Dst) ? Src : Dst);
        }

        // Multiply
        private byte BlendMultiply(ref byte Src, ref byte Dst) {
            return (byte)Math.Max(Math.Min((Src / 255.0f * Dst / 255.0f) * 255.0f, 255), 0);
        }

        // Screen
        private byte BlendScreen(ref byte Src, ref byte Dst) {
            return (byte)Math.Max(Math.Min(255 - ((255 - Src) / 255.0f * (255 - Dst) / 255.0f) * 255.0f, 255), 0);
        }

        // Choose lightest color 
        private byte BlendLighten(ref byte Src, ref byte Dst) {
            return ((Src > Dst) ? Src : Dst);
        }

        // hard light 
        private byte BlendHardLight(ref byte Src, ref byte Dst) {
            return ((Src < 128) ? (byte)Math.Max(Math.Min((Src / 255.0f * Dst / 255.0f) * 255.0f * 2, 255), 0) : (byte)Math.Max(Math.Min(255 - ((255 - Src) / 255.0f * (255 - Dst) / 255.0f) * 255.0f * 2, 255), 0));
        }

        // difference 
        private byte BlendDifference(ref byte Src, ref byte Dst) {
            return (byte)((Src > Dst) ? Src - Dst : Dst - Src);
        }

        // pin light 
        private byte BlendPinLight(ref byte Src, ref byte Dst) {
            return (Src < 128) ? ((Dst > Src) ? Src : Dst) : ((Dst < Src) ? Src : Dst);
        }

        // overlay 
        private byte BlendOverlay(ref byte Src, ref byte Dst) {
            return ((Dst < 128) ? (byte)Math.Max(Math.Min((Src / 255.0f * Dst / 255.0f) * 255.0f * 2, 255), 0) : (byte)Math.Max(Math.Min(255 - ((255 - Src) / 255.0f * (255 - Dst) / 255.0f) * 255.0f * 2, 255), 0));
        }

        // exclusion 
        private byte BlendExclusion(ref byte Src, ref byte Dst) {
            return (byte)(Src + Dst - 2 * (Dst * Src) / 255f);
        }

        // Soft Light (XFader formula)  
        private byte BlendSoftLight(ref byte Src, ref byte Dst) {
            return (byte)Math.Max(Math.Min((Dst * Src / 255f) + Dst * (255 - ((255 - Dst) * (255 - Src) / 255f) - (Dst * Src / 255f)) / 255f, 255), 0);
        }

        // Color Burn 
        private byte BlendColorBurn(ref byte Src, ref byte Dst) {
            return (Src == 0) ? (byte)0 : (byte)Math.Max(Math.Min(255 - (((255 - Dst) * 255) / Src), 255), 0);
        }

        // Color Dodge 
        private byte BlendColorDodge(ref byte Src, ref byte Dst) {
            return (Src == 255) ? (byte)255 : (byte)Math.Max(Math.Min((Dst * 255) / (255 - Src), 255), 0);
        }

        // use source Hue
        private void BlendHue(byte sR, byte sG, byte sB, ref byte dR, ref byte dG, ref byte dB) {
            ushort sH, sL, sS, dH, dL, dS;
            RGBToHLS(sR, sG, sB, out sH, out sL, out sS);
            RGBToHLS(dR, dG, dB, out dH, out dL, out dS);
            HLSToRGB(sH, dL, dS, out dR, out dG, out dB);
        }

        // use source Saturation
        private void BlendSaturation(byte sR, byte sG, byte sB, ref byte dR, ref byte dG, ref byte dB) {
            ushort sH, sL, sS, dH, dL, dS;
            RGBToHLS(sR, sG, sB, out sH, out sL, out sS);
            RGBToHLS(dR, dG, dB, out dH, out dL, out dS);
            HLSToRGB(dH, dL, sS, out dR, out dG, out dB);
        }

        // use source Color
        private void BlendColor(byte sR, byte sG, byte sB, ref byte dR, ref byte dG, ref byte dB) {
            ushort sH, sL, sS, dH, dL, dS;
            RGBToHLS(sR, sG, sB, out sH, out sL, out sS);
            RGBToHLS(dR, dG, dB, out dH, out dL, out dS);
            HLSToRGB(sH, dL, sS, out dR, out dG, out dB);
        }

        // use source Luminosity
        private void BlendLuminosity(byte sR, byte sG, byte sB, ref byte dR, ref byte dG, ref byte dB) {
            ushort sH, sL, sS, dH, dL, dS;
            RGBToHLS(sR, sG, sB, out sH, out sL, out sS);
            RGBToHLS(dR, dG, dB, out dH, out dL, out dS);
            HLSToRGB(dH, sL, dS, out dR, out dG, out dB);
        }

        #endregion
    }
}
