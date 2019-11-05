using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using System.IO;

namespace WpfApplication1.UI
{
    public partial class AnalizerViewModel : INotifyPropertyChanged
    {
        const int HALF = 255 / 2;

        public AnalizerViewModel()
        {
            ImageFile = "/Images/TestImage.jpg";
        }

        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        // Orientations.
        public const int OrientationId = 0x0112;
        public enum ExifOrientations
        {
            Unknown = 0,
            Normal = 1,
            Flipped = 2,
            Normal180 = 3,
            Flipped180 = 4,
            Flipped270 = 5,
            Normal270 = 6,
            Flipped90 = 7,
            Normal90 = 8,
        }

        public void GetImage_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.DefaultExt = ".jpg";
            dlg.Filter = "All Types (*.*)|*.*|JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";

            bool? result = dlg.ShowDialog();

            if (result.HasValue)
            {
                ImageFile = dlg.FileName;
            }
        }

        public void BlurImage_Click(object sender, RoutedEventArgs e)
        {
            // get sigma values for bluring
            int sigma1 = 1;
            //int sigma2 = 10;

            // get file names
            string file1 = ProcessingFolder + string.Format("Test{0}.jpg", sigma1);
            string grayScale = ProcessingFolder + string.Format("GraySacale{0}.jpg", sigma1);
            string xLOG = ProcessingFolder + string.Format("xLOG{0}.jpg", sigma1);
            string yLOG = ProcessingFolder + string.Format("yLOG{0}.jpg", sigma1);
            string lOG = ProcessingFolder + string.Format("lOG{0}.jpg", sigma1);
            string ssd = ProcessingFolder + string.Format("ssd{0}.jpg", sigma1);
            string xBlur = ProcessingFolder + string.Format("xBlur{0}.jpg", sigma1);
            string yBlur = ProcessingFolder + string.Format("yBlur{0}.jpg", sigma1);
            string blur = ProcessingFolder + string.Format("blur{0}.jpg", sigma1);
            //string file2 = ProcessingFolder + string.Format("Test{0}.jpg", sigma2);
            string xDiff = ProcessingFolder + string.Format("XDiff{0}.jpg", sigma1);
            string yDiff = ProcessingFolder + string.Format("YDiff{0}.jpg", sigma1);
            string xxDiff = ProcessingFolder + string.Format("XXDiff{0}.jpg", sigma1);
            string yyDiff = ProcessingFolder + string.Format("YYDiff{0}.jpg", sigma1);
            string xyComb = ProcessingFolder + string.Format("XYComb{0}.jpg", sigma1);
            string xxyyComb = ProcessingFolder + string.Format("XXYYComb{0}.jpg", sigma1);
            string ZeroCross = ProcessingFolder + string.Format("ZeroCross{0}.jpg", sigma1);
            string ZeroCrossXXYY = ProcessingFolder + string.Format("ZeroCrossXXYY{0}.jpg", sigma1);
            string GetOboveTreshold = ProcessingFolder + string.Format("Threshold{0}.jpg", sigma1);

            //GrayScale(GetPixels(new System.Drawing.Bitmap(ImageFile)), grayScale);
            //var pixels = GetPixels(new System.Drawing.Bitmap(ImageFile));
            //LOGX(pixels, sigma1, xLOG);
            //LOGY(pixels, sigma1, yLOG);
            //LOG(GetPixels(new System.Drawing.Bitmap(ImageFile)), sigma1, true, true, lOG);
            SSD(GetPixels(new System.Drawing.Bitmap(ImageFile)), sigma1, ssd);
            //BlurImageX(GetPixels(new System.Drawing.Bitmap(grayScale)), sigma1, xBlur);
            //BlurImageY(GetPixels(new System.Drawing.Bitmap(xBlur)), sigma1, blur);
            //BlurImage(GetPixels(new System.Drawing.Bitmap(ImageFile)), sigma2, file2);
            //SubtractImages(GetPixels(new System.Drawing.Bitmap(file1)), GetPixels(new System.Drawing.Bitmap(file2)), 5, ProcessingFolder + "Edges.jpg");
            //GetXDifferencial(GetPixels(new System.Drawing.Bitmap(blur)), xDiff);
            //GetyDifferencial(GetPixels(new System.Drawing.Bitmap(blur)), yDiff);
            ////GetXDifferencial(GetPixels(new System.Drawing.Bitmap(xDiff)), xxDiff);
            ////GetyDifferencial(GetPixels(new System.Drawing.Bitmap(yDiff)), yyDiff);
            ////CombineDirectionalImages(GetPixels(new System.Drawing.Bitmap(xxDiff)), GetPixels(new System.Drawing.Bitmap(yyDiff)), xxyyComb);
            //CombineDirectionalImages(GetPixels(new System.Drawing.Bitmap(xDiff)), GetPixels(new System.Drawing.Bitmap(yDiff)), xyComb);
            //GetDifferencial(GetPixels(new System.Drawing.Bitmap(blur)), xyComb);
            //FindZeroCrossings(GetPixels(new System.Drawing.Bitmap(xyComb)), ZeroCross, 3);
            //FindZeroCrossings(GetPixels(new System.Drawing.Bitmap(xxyyComb)), ZeroCrossXXYY, 2);

            //for (int i = 0; i< 7; i++)
            //{
            //    string GetOboveTreshold = ProcessingFolder + string.Format("Threshold{0}-{1}.jpg", sigma1, i);
            //    string GetOboveTreshold2nd = ProcessingFolder + string.Format("Threshold2nd{0}-{1}.jpg", sigma1, i);
            //    //FindGreatestGradiant(GetPixels(new System.Drawing.Bitmap(xxyyComb)), GetOboveTreshold2nd, i);
            //    FindGreatestGradiant(GetPixels(new System.Drawing.Bitmap(lOG)), GetOboveTreshold, i);
            //}
            //FindGreatestGradiant(GetPixels(new System.Drawing.Bitmap(lOG)), GetOboveTreshold, 0);
            //FindZeroCrossings2(GetPixels(new System.Drawing.Bitmap(lOG)), ZeroCross, 0);
        }

        public void FindEdges_Click(object sender, RoutedEventArgs e)
        {

        }

        protected Matrix GetTransformMatrixForImage(System.Drawing.Image ImageFile)
        {
            return GetTransformFromOrientationIndex(Array.IndexOf(ImageFile.PropertyIdList, OrientationId), ImageFile.Width, ImageFile.Height);
        }

        protected Matrix GetTransformFromOrientationIndex(int OrientationIndex, int Width, int Height)
        {
            Matrix matrix = new Matrix();

            //switch(OrientationIndex)
            //{
            //    case (int)ExifOrientations.Flipped180:
            //        matrix.Translate(0, Height);
            //        matrix.Scale(1, -1);
            //        break;
            //    case (int)ExifOrientations.Normal180:
            //        matrix.Scale(-1, -1);
            //        break;
            //    case (int)ExifOrientations.Normal90:
            //        matrix.Rotate(90);
            //        break;
            //    case (int)ExifOrientations.Flipped90:
            //        matrix.Scale(-1, 1);
            //        matrix.Rotate(90);
            //        break;
            //    case (int)ExifOrientations.Normal270:
            //        matrix.Rotate(-90);
            //        break;
            //    case (int)ExifOrientations.Flipped:
            //        matrix.Scale(-1, 1);
            //        break;
            //    case (int)ExifOrientations.Flipped270:
            //        matrix.Scale(-1, 1);
            //        matrix.Rotate(-90);
            //        break;
            //    case (int)ExifOrientations.Normal:
            //    case (int)ExifOrientations.Unknown:
            //    default:
            //        break;

            //}

            return matrix;
        }

        private void BlurImageX(System.Drawing.Color[,] Image, int Sigma, String ProcessFile)
        {
            BlurImage(Image, Sigma, true, false, ProcessFile);
        }

        private void BlurImageY(System.Drawing.Color[,] Image, int Sigma, String ProcessFile)
        {
            BlurImage(Image, Sigma, false, true, ProcessFile);
        }

        private void BlurImage(System.Drawing.Color[,] Image, int Sigma, bool DoX, bool DoY, String ProcessFile)
        {

            int width = Image.GetLength(0);
            int height = Image.GetLength(1);

            uint[] pixels = new uint[Image.Length];
            WriteableBitmap bitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgra32, null);

            int xDist = Sigma * 3;
            //if (Sigma % 2 != 0) xDist += 1;
            int yDist = xDist;

            double topValue = 0;

            if (!DoY)
            {
                yDist = 0;
            }
            else
            {
                topValue = 3;
            }
            if (!DoX)
            {
                xDist = 0;
            }
            else
            {
                topValue += 3;
            }

            if (xDist + yDist == 0) topValue = 1;

            int xCount = (xDist * 2) + 1;
            int yCount = (yDist * 2) + 1;

            double inverse = topValue / (double)(xCount * yCount);

           // double[] inverses = new double[(xCount * yCount) + 1];


           // Parallel.For(1, inverses.Length, i =>
           //{
           //    inverses[i] = 1.0 / i;
           //});

            double[,] filter = GetGaussianFilter(xDist, yDist, Sigma);

            Parallel.For(0, width, x =>
            {
                for (int y = 0; y < height; y++)
                {

                    int pixelCount = (xDist * yCount) + 1;
                    double r = 0;
                    double g = 0;
                    double b = 0;
                    double a = 0;

                    for (int i = -xDist; i <= xDist; i++)
                    {
                        if (x + i >= 0 && x + i < width)
                        {
                            for (int j = -yDist; j <= yDist; j++)
                            {
                                if (y + j >= 0 && y + j < height)
                                {
                                    System.Drawing.Color pixel = Image[x + i, y + j];

                                    double gauss = filter[i + xDist, j + yDist];

                                    r += pixel.R * gauss;
                                    //g += pixel.G * gauss;
                                    //b += pixel.B * gauss;
                                    //a += pixel.A * gauss;
                                }
                            }
                        }
                    }

                    if (pixelCount > 0)
                    {
                        int p = Math.Min((int)(r * inverse), 255);
                        System.Drawing.Color currentPixel = System.Drawing.Color.FromArgb(255, p, p, p);
                        pixels[y * width + x] = (uint)((currentPixel.A << 24) | (currentPixel.R << 16) | (currentPixel.G << 8) |(currentPixel.B << 0));
                    }
                    else
                    {
                        //This shouldn't ever happen
                        throw new Exception("Pixel count 0.");
                    }
                }
            });

            bitmap.WritePixels(new Int32Rect(0, 0, width, height), pixels, width * 4, 0);

            SaveImage(ProcessFile, bitmap.Clone());

            ImageBitMap = bitmap;

            //PopupMessage("Done.");
        }


        private void LOGX(System.Drawing.Color[,] Image, int Sigma, String ProcessFile)
        {
            LOG(Image, Sigma, true, false, ProcessFile);
        }

        private void LOGY(System.Drawing.Color[,] Image, int Sigma, String ProcessFile)
        {
            LOG(Image, Sigma, false, true, ProcessFile);
        }


        private void LOG(System.Drawing.Color[,] Image, int Sigma, bool DoX, bool DoY, String ProcessFile)
        {

            int width = Image.GetLength(0);
            int height = Image.GetLength(1);

            uint[] pixels = new uint[Image.Length];
            WriteableBitmap bitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgra32, null);

            int xDist = Sigma * 3;
            //if (Sigma % 2 != 0) xDist += 1;
            int yDist = xDist;

            double topValue = 0;

            if (!DoY)
            {
                yDist = 0;
            }
            else
            {
                topValue = 3;
            }
            if (!DoX)
            {
                xDist = 0;
            }
            else
            {
                topValue += 3;
            }

            if (xDist + yDist == 0) topValue = 1;

            int xCount = (xDist * 2) + 1;
            int yCount = (yDist * 2) + 1;

            double inverse = 1;
            if (DoX && DoY)
            {
                inverse = 1.0;
            } else
            {
                inverse = 3.0 / Math.Sqrt(Sigma);
            }

            // double[] inverses = new double[(xCount * yCount) + 1];


            // Parallel.For(1, inverses.Length, i =>
            //{
            //    inverses[i] = 1.0 / i;
            //});

            double[,] filter = GetLoGFilter(xDist, yDist, Sigma);

            Parallel.For(0, width, x =>
            {
                for (int y = 0; y < height; y++)
                {

                    int pixelCount = (xDist * yCount) + 1;
                    double r = HALF;
                    double g = HALF;
                    double b = HALF;
                    double a = 255;

                    for (int i = -xDist; i <= xDist; i++)
                    {
                        if (x + i >= 0 && x + i < width)
                        {
                            for (int j = -yDist; j <= yDist; j++)
                            {
                                if (y + j >= 0 && y + j < height)
                                {
                                    System.Drawing.Color pixel = Image[x + i, y + j];

                                    double log = filter[i + xDist, j + yDist];

                                    r += pixel.R * log;
                                    g += pixel.G * log;
                                    b += pixel.B * log;
                                    //a += pixel.A * gauss;
                                }
                            }
                        }
                    }

                    if (pixelCount > 0)
                    {
                        int r2 = Math.Min((int)(r), 255);
                        int g2 = Math.Min((int)(g), 255);
                        int b2 = Math.Min((int)(b), 255);
                        System.Drawing.Color currentPixel = System.Drawing.Color.FromArgb(255, r2, g2, b2);
                        pixels[y * width + x] = (uint)((currentPixel.A << 24) | (currentPixel.R << 16) | (currentPixel.G << 8) | (currentPixel.B << 0));
                    }
                    else
                    {
                        //This shouldn't ever happen
                        throw new Exception("Pixel count 0.");
                    }
                }
            });

            bitmap.WritePixels(new Int32Rect(0, 0, width, height), pixels, width * 4, 0);

            SaveImage(ProcessFile, bitmap.Clone());

            ImageBitMap = bitmap;

            //PopupMessage("Done.");
        }

        private void SSD(System.Drawing.Color[,] Image, int Sigma, String ProcessFile)
        {

            int width = Image.GetLength(0);
            int height = Image.GetLength(1);

            uint[] pixels = new uint[Image.Length];
            WriteableBitmap bitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgra32, null);

            int xDist = Sigma * 3;
            //if (Sigma % 2 != 0) xDist += 1;
            int yDist = xDist;

            int xCount = (xDist * 2) + 1;
            int yCount = (yDist * 2) + 1;

            double inv = 1.0 / (double)(xCount * yCount);

            double[,] filter = GetGaussianFilter(xDist, yDist, Sigma);

            Parallel.For(0, width, x =>
            {
                for (int y = 0; y < height; y++)
                {

                    int pixelCount = (xDist * yCount) + 1;
                    double r = HALF;
                    double g = HALF;
                    double b = HALF;
                    double a = 255;

                    for (int i = -xDist; i <= xDist; i++)
                    {
                        if (x + i >= 0 && x + i < width)
                        {
                            System.Drawing.Color pixel = Image[x, y];
                            for (int j = -yDist; j <= yDist; j++)
                            {
                                if (y + j >= 0 && y + j < height)
                                {
                                    System.Drawing.Color pixel2 = Image[x + i, y + j];

                                    double gauss = filter[i + xDist, j + yDist];

                                    // Get pixel differences
                                    int rDiff = pixel2.R - pixel.R;
                                    int gDiff = pixel2.G - pixel.G;
                                    int bDiff = pixel2.B - pixel.B;

                                    r += rDiff * rDiff * gauss;
                                    g += gDiff * gDiff * gauss;
                                    b += bDiff * bDiff * gauss;
                                    //a += pixel.A * gauss;
                                }
                            }
                        }
                    }

                    if (pixelCount > 0)
                    {
                        int r2 = Math.Min((int)(r * inv), 255);
                        int g2 = Math.Min((int)(g * inv), 255);
                        int b2 = Math.Min((int)(b * inv), 255);
                        System.Drawing.Color currentPixel = System.Drawing.Color.FromArgb(255, r2, g2, b2);
                        pixels[y * width + x] = (uint)((currentPixel.A << 24) | (currentPixel.R << 16) | (currentPixel.G << 8) | (currentPixel.B << 0));
                    }
                    else
                    {
                        //This shouldn't ever happen
                        throw new Exception("Pixel count 0.");
                    }
                }
            });

            bitmap.WritePixels(new Int32Rect(0, 0, width, height), pixels, width * 4, 0);

            SaveImage(ProcessFile, bitmap.Clone());

            ImageBitMap = bitmap;

            //PopupMessage("Done.");
        }

        private void GrayScale(System.Drawing.Color[,] Image, String ProcessFile)
        {

            int width = Image.GetLength(0);
            int height = Image.GetLength(1);

            System.Drawing.Color OriginPixel = System.Drawing.Color.FromArgb(255, 0, 0, 0);
            System.Drawing.Color MaxPixel = System.Drawing.Color.FromArgb(255, 255, 255, 255);
            int maxDist = GetPixelDist(MaxPixel, OriginPixel);
            double pixelInverse = (double)255 / (double)maxDist;

            uint[] pixels = new uint[Image.Length];
            WriteableBitmap bitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgra32, null);

            Parallel.For(0, width, x =>
            {
                for (int y = 0; y < height; y++)
                {
                    System.Drawing.Color pixel = Image[x, y];

                    int p = (int) ((double)GetPixelDist(pixel, OriginPixel) * pixelInverse);

                    System.Drawing.Color currentPixel = System.Drawing.Color.FromArgb(255, p, p, p);
                    pixels[y * width + x] = (uint)((currentPixel.A << 24) | (currentPixel.R << 16) | (currentPixel.G << 8) | (currentPixel.B << 0));
                }
            });

            bitmap.WritePixels(new Int32Rect(0, 0, width, height), pixels, width * 4, 0);

            SaveImage(ProcessFile, bitmap.Clone());

            ImageBitMap = bitmap;

            //PopupMessage("Done.");
        }

        private void GetXDifferencial(System.Drawing.Color[,] Image, String OutPutFile)
        {
            int width = Image.GetLength(0);
            int height = Image.GetLength(1);

            uint[] pixels = new uint[Image.Length];
            WriteableBitmap bitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgra32, null);

            Parallel.For(0, width, x =>
            {
                for (int y = 0; y < height; y++)
                {
                    int p = 0;
                    int a = 255;

                    if (x + 1 < width)
                    {
                        System.Drawing.Color pixel1 = Image[x, y];
                        System.Drawing.Color pixel2 = Image[x + 1, y];

                        p = Math.Max(Math.Min(HALF + (pixel2.R - pixel1.R), 255), 0);
                    }

                    System.Drawing.Color currentPixel = System.Drawing.Color.FromArgb(a, p, p, p);
                    pixels[y * width + x] = (uint)((currentPixel.A << 24) | (currentPixel.R << 16) | (currentPixel.G << 8) | (currentPixel.B << 0));
                }
            });

            bitmap.WritePixels(new Int32Rect(0, 0, width, height), pixels, width * 4, 0);

            SaveImage(OutPutFile, bitmap.Clone());

            ImageBitMap = bitmap;

        }

        private void GetyDifferencial(System.Drawing.Color[,] Image, String OutPutFile)
        {
            int width = Image.GetLength(0);
            int height = Image.GetLength(1);

            uint[] pixels = new uint[Image.Length];
            WriteableBitmap bitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgra32, null);


            Parallel.For(0, width, x =>
            {
                for (int y = 0; y < height; y++)
                {
                    int p = 0;
                    int a = 255;

                    if (y + 1 < height)
                    {
                        System.Drawing.Color pixel1 = Image[x, y];
                        System.Drawing.Color pixel2 = Image[x, y + 1];

                        p = Math.Max(Math.Min(HALF + (pixel2.R - pixel1.R), 255), 0);
                    }

                    System.Drawing.Color currentPixel = System.Drawing.Color.FromArgb(a, p, p, p);
                    pixels[y * width + x] = (uint)((currentPixel.A << 24) | (currentPixel.R << 16) | (currentPixel.G << 8) | (currentPixel.B << 0));
                }
            });

            bitmap.WritePixels(new Int32Rect(0, 0, width, height), pixels, width * 4, 0);

            SaveImage(OutPutFile, bitmap.Clone());

            ImageBitMap = bitmap;

        }

        private void GetDifferencial(System.Drawing.Color[,] Image, String OutPutFile)
        {
            int width = Image.GetLength(0);
            int height = Image.GetLength(1);

            uint[] pixels = new uint[Image.Length];
            WriteableBitmap bitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgra32, null);


            Parallel.For(0, width, x =>
            {
                for (int y = 0; y < height; y++)
                {
                    int r = 0;
                    int g = 0;
                    int b = HALF;
                    int a = 255;

                    System.Drawing.Color pixel1 = Image[x, y];

                    if (y + 1 < height)
                    {
                        System.Drawing.Color pixel2 = Image[x, y + 1];

                        g = Math.Max(Math.Min(HALF + (pixel2.R - pixel1.R), 255), 0);
                    }

                    if (x + 1 < width)
                    {
                        System.Drawing.Color pixel3 = Image[x + 1, y];

                        r = Math.Max(Math.Min(HALF + (pixel3.R - pixel1.R), 255), 0);
                    }
                    System.Drawing.Color currentPixel = System.Drawing.Color.FromArgb(a, r, g, b);
                    pixels[y * width + x] = (uint)((currentPixel.A << 24) | (currentPixel.R << 16) | (currentPixel.G << 8) | (currentPixel.B << 0));
                }
            });

            bitmap.WritePixels(new Int32Rect(0, 0, width, height), pixels, width * 4, 0);

            SaveImage(OutPutFile, bitmap.Clone());

            ImageBitMap = bitmap;

        }

        private void FindZeroCrossings(System.Drawing.Color[,] Image, String OutPutFile, int Threshold = 1)
        {
            int width = Image.GetLength(0);
            int height = Image.GetLength(1);

            uint[] pixels = new uint[Image.Length];
            WriteableBitmap bitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgra32, null);

            const double oneThird = 1.0 / 3.0;


            Parallel.For(0, width, x =>
            {
                for (int y = 0; y < height; y++)
                {
                    int p = 0;
                    int a = 255;

                    // Get these values for the second pixel direction
                    int x2 = x;
                    int y2 = y;

                    System.Drawing.Color Pixel = Image[x, y];

                    int xdir = (int)Pixel.R - HALF;
                    int ydir = (int)Pixel.G - HALF;

                    //System.Drawing.Color xPixel1 = Image[x, y];
                    //System.Drawing.Color xPixel2 = Image[x, y];
                    //System.Drawing.Color yPixel1 = Image[x, y];
                    //System.Drawing.Color yPixel2 = Image[x, y];

                    //if (x - 1 >= 0)
                    //{
                    //    xPixel1 = Image[x - 1, y];
                    //}


                    if (xdir != 0)
                    {
                        if (xdir < 0 && x - 1 >= 0)
                        {
                            x2 = x - 1;
                        } else if (xdir > 0 && x + 1 < width)
                        {
                            x2 = x + 1;
                        }
                    }

                    //if (x + 1 < width)
                    //{
                    //    xPixel2 = Image[x + 1, y];
                    //}

                    //if (y - 1 >= 0)
                    //{
                    //    yPixel1 = Image[x, y - 1];
                    //}

                    if (ydir != 0)
                    {
                        if (ydir < 0 && y - 1 >= 0)
                        {
                            y2 = y - 1;
                        }
                        else if (ydir > 0 && y + 1 < height)
                        {
                            y2 = y + 1;
                        }
                    }

                    System.Drawing.Color Pixel2;

                    int totalDir = ydir + xdir;

                    double xPercent = (double)totalDir / (double)xdir;

                    if (xPercent > 2* oneThird)
                    {
                        Pixel2 = Image[x2, y];
                    } else if(xPercent < oneThird)
                    {
                        Pixel2 = Image[x, y2];
                    } else
                    {
                        Pixel2 = Image[x2, y2];
                    }


                    int ydir2 = (int)Pixel2.G - HALF;

                    if (Math.Abs(ydir - ydir2) >= Threshold
                    && ((ydir <= 0 && ydir2 >= 0)))
                    {
                        p = 255;
                    }

                    int xdir2 = (int)Pixel2.G - HALF;

                    if (Math.Abs(xdir - xdir2) >= Threshold
                    && ((xdir >= 0 && xdir2 <= 0)))
                    {
                        p = 255;
                    }

                    //if (Pixel.R != HALF && Pixel2.R != HALF && Pixel.G != HALF && Pixel2.G != HALF)
                    //{
                    //    int xdir2 = (int)Pixel2.R - HALF;
                    //    int ydir2 = (int)Pixel2.G - HALF;


                    //    double slope1 = (double)ydir / (double)xdir;
                    //    double slope2 = (double)ydir2 / (double)xdir2;

                    //    double ratio = slope1 / slope2;

                    //    if (ratio >= 0.9 || ratio <= 1.1)
                    //    {
                    //        if (Math.Abs(xdir - xdir2) >= Threshold
                    //            && Math.Abs(ydir - ydir2) >= Threshold
                    //            && ((xdir <= 0 && xdir2 >= 0) || (xdir >= 0 && xdir2 <= 0))
                    //            && ((ydir <= 0 && ydir2 >= 0) || (ydir >= 0 && ydir2 <= 0)))
                    //        {
                    //            p = 255;
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    //int ydir2 = (int)Pixel2.G - HALF;

                    //    //if (Math.Abs(ydir - ydir2) >= Threshold
                    //    //&& ((ydir <= 0 && ydir2 >= 0) || (ydir >= 0 && ydir2 <= 0)))
                    //    //{
                    //    //    p = 255;
                    //    //}
                    //}

                    //if (
                    //    (
                    //        (Math.Abs((int)Pixel.R - (int)Pixel2.R) > Threshold)
                    //        && ((Pixel.R >= HALF && Pixel.R <= HALF)))
                    //        ||
                    //    (
                    //        (Math.Abs((int)Pixel.G - (int)Pixel2.G) > Threshold)
                    //        && ((Pixel.R >= HALF && Pixel.R <= HALF))
                    //    )
                    //)
                    //{
                    //    p = 255;
                    //}

                    //if (
                    //    (
                    //        (xPixel1.R != xPixel2.R)
                    //        && ((xPixel1.R >= HALF && xPixel1.R <= HALF)
                    //            || (xPixel1.R >= HALF && xPixel1.R <= HALF)))
                    //        ||
                    //    (
                    //        (yPixel1.G != yPixel2.G)
                    //        && ((xPixel1.R >= HALF && xPixel1.R <= HALF)
                    //            || (xPixel1.R >= HALF && xPixel1.R <= HALF))
                    //    )
                    //)
                    //{
                    //    p = 255;
                    //}

                    System.Drawing.Color currentPixel = System.Drawing.Color.FromArgb(a, p, p, p);
                    pixels[y * width + x] = (uint)((currentPixel.A << 24) | (currentPixel.R << 16) | (currentPixel.G << 8) | (currentPixel.B << 0));
                }
            });

            bitmap.WritePixels(new Int32Rect(0, 0, width, height), pixels, width * 4, 0);

            SaveImage(OutPutFile, bitmap.Clone());

            ImageBitMap = bitmap;

        }

        private void FindZeroCrossings2(System.Drawing.Color[,] Image, String OutPutFile, int Threshold = 1)
        {
            int width = Image.GetLength(0);
            int height = Image.GetLength(1);

            uint[] pixels = new uint[Image.Length];
            WriteableBitmap bitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgra32, null);

            Parallel.For(0, width, x =>
            {
                for (int y = 0; y < height; y++)
                {
                    int p = 0;
                    int a = 255;

                    System.Drawing.Color Pixel = Image[x, y];

                    int dxMin = 0, dyMin = 0, dxMax = 0, dyMax = 0;

                    if (x - 1 >= 0)
                    {
                        dxMin = -1;
                    }
                    if (y - 1 >= 0)
                    {
                        dyMin = -1;
                    }
                    if (x + 1 < width)
                    {
                        dxMax = 1;
                    }
                    if (y + 1 < height)
                    {
                        dyMax = 1;
                    }

                    for (int dy = dyMin; dy <= dyMax && p == 0; dy++)
                    {
                        for (int dx = dxMin; dx <= dxMax && p == 0; dx++)
                        {
                            if (dy != 0 && dx != 0)
                            {
                                System.Drawing.Color Pixel2 = Image[x + dx, y + dy];

                                if ((Pixel2.R > HALF && Pixel.R < HALF) || (Pixel2.G > HALF && Pixel.G < HALF) || (Pixel2.B > HALF && Pixel.B < HALF)
                                    || (Pixel2.R < HALF && Pixel.R > HALF) || (Pixel2.G < HALF && Pixel.G > HALF) || (Pixel2.B < HALF && Pixel.B > HALF))
                                {
                                    p = 255;
                                }

                            }
                        }
                    }

                    System.Drawing.Color currentPixel = System.Drawing.Color.FromArgb(a, p, p, p);
                    pixels[y * width + x] = (uint)((currentPixel.A << 24) | (currentPixel.R << 16) | (currentPixel.G << 8) | (currentPixel.B << 0));
                }
            });

            bitmap.WritePixels(new Int32Rect(0, 0, width, height), pixels, width * 4, 0);

            SaveImage(OutPutFile, bitmap.Clone());

            ImageBitMap = bitmap;

        }

        private void FindGreatestGradiant(System.Drawing.Color[,] Image, String OutPutFile, int Threshold = 1)
        {
            int width = Image.GetLength(0);
            int height = Image.GetLength(1);

            uint[] pixels = new uint[Image.Length];
            WriteableBitmap bitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgra32, null);

            System.Drawing.Color halfPix = System.Drawing.Color.FromArgb(255, HALF, HALF, HALF);
            System.Drawing.Color orig = System.Drawing.Color.FromArgb(255, 0, 0, 0);

            Parallel.For(0, width, x =>
            {
            for (int y = 0; y < height; y++)
            {
                int p = 0;
                int a = 255;

                System.Drawing.Color Pixel = Image[x, y];

                    if (GetPixel2DDist(Pixel, halfPix) >= Threshold)
                    {
                        bool isExtrema = false;

                        if (y - 1 >= 0)
                        {
                            System.Drawing.Color pixel2 = Image[x, y - 1];

                            if (Pixel.G >= HALF)
                            {
                                if (pixel2.G < Pixel.G)
                                {
                                    isExtrema = true;
                                }
                            } else
                            {
                                if (pixel2.G > Pixel.G)
                                {
                                    isExtrema = true;
                                };
                            }
                        }
                        if (y + 1 < height)
                        {
                            System.Drawing.Color pixel2 = Image[x, y + 1];

                            if (Pixel.G >= HALF)
                            {
                                if (pixel2.G < Pixel.G)
                                {
                                    isExtrema = true;
                                } else
                                {
                                    isExtrema = false;
                                }
                            }
                            else
                            {
                                if (pixel2.G > Pixel.G)
                                {
                                    isExtrema = true;
                                } else
                                {
                                    isExtrema = false;
                                }
                            }
                        }

                        if (!isExtrema)
                        {
                            if (x - 1 >= 0)
                            {
                                System.Drawing.Color pixel2 = Image[x - 1, y];

                                if (Pixel.R >= HALF)
                                {
                                    if (pixel2.R < Pixel.R)
                                    {
                                        isExtrema = true;
                                    }
                                }
                                else
                                {
                                    if (pixel2.R > Pixel.R)
                                    {
                                        isExtrema = true;
                                    }
                                }

                            }
                            if (x + 1 < width)
                            {
                                System.Drawing.Color pixel2 = Image[x + 1, y];

                                if (Pixel.R >= HALF)
                                {
                                    if (pixel2.R < Pixel.R)
                                    {
                                        isExtrema = true;
                                    } else
                                    {
                                        isExtrema = false;
                                    }
                                }
                                else
                                {
                                    if (pixel2.R > Pixel.R)
                                    {
                                        isExtrema = true;
                                    } else
                                    {
                                        isExtrema = false;
                                    }
                                }

                            }
                        }

                        if (isExtrema) p = 255;
                    }

                        System.Drawing.Color currentPixel = System.Drawing.Color.FromArgb(a, p, p, p);
                    pixels[y * width + x] = (uint)((currentPixel.A << 24) | (currentPixel.R << 16) | (currentPixel.G << 8) | (currentPixel.B << 0));
                }
            });

            bitmap.WritePixels(new Int32Rect(0, 0, width, height), pixels, width * 4, 0);

            SaveImage(OutPutFile, bitmap.Clone());

            ImageBitMap = bitmap;

        }

        private void CombineDirectionalImages(System.Drawing.Color[,] ImageX, System.Drawing.Color[,] ImageY, String OutPutFile)
        {

            int widthX = ImageX.GetLength(0);
            int heightX = ImageX.GetLength(1);

            int widthY = ImageY.GetLength(0);
            int heightY = ImageY.GetLength(1);

            uint[] pixels = new uint[ImageX.Length];
            WriteableBitmap bitmap = new WriteableBitmap(widthX, heightX, 96, 96, PixelFormats.Bgra32, null);

            Parallel.For(0, Math.Min(widthX, widthY), x =>
            {
                for (int y = 0; y < heightX && y < heightY; y++)
                {
                    System.Drawing.Color pixelX = ImageX[x, y];
                    System.Drawing.Color pixelY = ImageY[x, y];

                    // averaget two pixels together
                    int r = (int)pixelX.R;
                    int g = (int)pixelY.G;
                    int b = HALF;

                    byte a = 255;

                    System.Drawing.Color currentPixel = System.Drawing.Color.FromArgb((int)(a), r, g, b);
                    pixels[y * widthX + x] = (uint)((currentPixel.A << 24) | (currentPixel.R << 16) | (currentPixel.G << 8) | (currentPixel.B << 0));
                }
            });

            bitmap.WritePixels(new Int32Rect(0, 0, widthX, heightX), pixels, widthX * 4, 0);

            SaveImage(OutPutFile, bitmap.Clone());

            ImageBitMap = bitmap;

            //PopupMessage("Done.");
        }

        private void CombineImages(System.Drawing.Color[,] Image1, System.Drawing.Color[,] Image2, byte Threshold, String OutPutFile)
        {

            int width1 = Image1.GetLength(0);
            int height1 = Image1.GetLength(1);

            int width2 = Image2.GetLength(0);
            int height2 = Image2.GetLength(1);

            uint[] pixels = new uint[Image1.Length];
            WriteableBitmap bitmap = new WriteableBitmap(width1, height1, 96, 96, PixelFormats.Bgra32, null);

            Parallel.For(0, Math.Min(width1, width2), x =>
            {
                for (int y = 0; y < height1 && y < height2; y++)
                {
                    System.Drawing.Color pixel1 = Image1[x, y];
                    System.Drawing.Color pixel2 = Image2[x, y];

                    // averaget two pixels together
                    int r = ((int)pixel1.R + (int)pixel2.R)/ 2;
                    int g = ((int)pixel1.G + (int)pixel2.G) / 2;
                    int b = ((int)pixel1.B + (int)pixel2.B) / 2;

                    // Get value of pixel centered at 0
                    r = HALF - r;
                    g = HALF - g;
                    b = HALF - b;

                    // get square value with value capped at half possible value
                    int r2 = Math.Min(r * r, HALF);
                    int g2 = Math.Min(g * g, HALF);
                    int b2 = Math.Min(b * b, HALF);

                    // if the origianl was negative, supbract it, otherwise add it.
                    r = (r < 0) ? HALF - r2 : HALF + r2;
                    g = (g < 0) ? HALF - g2 : HALF + g2;
                    b = (b < 0) ? HALF - b2 : HALF + b2;

                    byte a = 255;

                    System.Drawing.Color currentPixel = System.Drawing.Color.FromArgb((int)(a), r, g, b);
                    pixels[y * width1 + x] = (uint)((currentPixel.A << 24) | (currentPixel.R << 16) | (currentPixel.G << 8) | (currentPixel.B << 0));
                }
            });

            bitmap.WritePixels(new Int32Rect(0, 0, width1, height1), pixels, width1 * 4, 0);

            SaveImage(OutPutFile, bitmap.Clone());

            ImageBitMap = bitmap;

            //PopupMessage("Done.");
        }

        private void SubtractImages(System.Drawing.Color[,] Image1, System.Drawing.Color[,] Image2, byte Threshold, String OutPutFile)
        {

            int width1 = Image1.GetLength(0);
            int height1 = Image1.GetLength(1);

            int width2 = Image2.GetLength(0);
            int height2 = Image2.GetLength(1);

            uint[] pixels = new uint[Image1.Length];
            WriteableBitmap bitmap = new WriteableBitmap(width1, height1, 96, 96, PixelFormats.Bgra32, null);

            Parallel.For(0, Math.Min(width1, width2), x =>
            {
                for (int y = 0; y < height1 && y < height2; y++)
                {
                    System.Drawing.Color pixel1 = Image1[x, y];
                    System.Drawing.Color pixel2 = Image2[x, y];

                    int r = (Math.Max(pixel1.R, pixel2.R) - Math.Min(pixel1.R, pixel2.R));
                    int g = (Math.Max(pixel1.G, pixel2.G) - Math.Min(pixel1.G, pixel2.G));
                    int b = (Math.Max(pixel1.B, pixel2.B) - Math.Min(pixel1.B, pixel2.B));
                    byte a = 255;

                    byte r2 = CheckThreshold((byte)Math.Min(255, r * r), Threshold);
                    byte g2 = CheckThreshold((byte)Math.Min(255, g * g), Threshold);
                    byte b2 = CheckThreshold((byte)Math.Min(255, b * b), Threshold);

                    int p = Math.Max(Math.Max(r2, g2), b2);

                    //byte r = pixel2.R;
                    //byte g = pixel2.G;
                    //byte b = pixel2.B;
                    //byte a = pixel2.A;


                    System.Drawing.Color currentPixel = System.Drawing.Color.FromArgb((int)(a), (int)(p), (int)(p), (int)(p));
                    pixels[y * width1 + x] = (uint)((currentPixel.A << 24) | (currentPixel.R << 16) | (currentPixel.G << 8) | (currentPixel.B << 0));
                }
            });

            bitmap.WritePixels(new Int32Rect(0, 0, width1, height1), pixels, width1 * 4, 0);

            SaveImage(OutPutFile, bitmap.Clone());

            ImageBitMap = bitmap;

            //PopupMessage("Done.");
        }

        private void PopupMessage(string message)
        {
            Popup codePopup = new Popup();
            TextBlock text = new TextBlock();
            text.Text = message;
            codePopup.Child = text;
            codePopup.IsOpen = true;
        }

        private System.Drawing.Color[,] GetPixels(System.Drawing.Bitmap Image)
        {
            System.Drawing.Color[,] pixels = new System.Drawing.Color[Image.Width, Image.Height];

            for (int x = 0; x < Image.Width; x++)
            {
                for (int y = 0; y < Image.Height; y++)
                {
                    pixels[x, y] = Image.GetPixel(x, y);
                }
            }

            return pixels;
        }

        private String _ImageFile = "";

        public String ImageFile
        {
            get
            {
                return _ImageFile;
            }
            set
            {
                _ImageFile = value;

                if (System.IO.File.Exists(System.IO.Path.GetFullPath(ImageFile)))
                {
                    //ImageBitMap = new BitmapImage(new Uri(System.IO.Path.GetFullPath(ImageFile)));

                    BitmapSource source = new BitmapImage(new Uri(System.IO.Path.GetFullPath(ImageFile)));

                    ImageBitMap = new WriteableBitmap(source);

                    //TransformMatrix = GetTransformMatrixForImage(image);
                }

                this.PropertyChanged(this, new PropertyChangedEventArgs(nameof(ImageFile)));
            }
        }

        private WriteableBitmap _ImageBitMap;

        public WriteableBitmap ImageBitMap
        {
            get
            {
                return _ImageBitMap;
            }
            set
            {
                _ImageBitMap = value;

                this.PropertyChanged(this, new PropertyChangedEventArgs(nameof(ImageBitMap)));
            }
        }

        private Matrix _TransformMatrix;
        public Matrix TransformMatrix
        {
            get
            {
                return _TransformMatrix;
            }
            set
            {
                _TransformMatrix = value;
                this.PropertyChanged(this, new PropertyChangedEventArgs(nameof(TransformMatrix)));
            }
        }

        private String _ProcessingFolder = "D:/ProcessFolder/";
        public String ProcessingFolder
        {
            get
            {
                return _ProcessingFolder;
            }
            set
            {
                _ProcessingFolder = value;
                this.PropertyChanged(this, new PropertyChangedEventArgs(nameof(ProcessingFolder)));
            }
        }

        // Got this from a stack overflow user for saving an image: https://stackoverflow.com/questions/11212771/save-writeablebitmap-to-file-using-wpf
        void SaveImage(string filename, BitmapSource image)
        {
            if (filename != string.Empty)
            {
                using (FileStream stream5 = new FileStream(filename, FileMode.Create))
                {
                    PngBitmapEncoder encoder5 = new PngBitmapEncoder();
                    encoder5.Frames.Add(BitmapFrame.Create(image));
                    encoder5.Save(stream5);
                }
            }
        }

        byte CheckThreshold(byte Value, byte Threshold)
        {
            if (Value >= Threshold)
            {
                return 255;
            } else
            {
                return 0;
            }
        }

        double[,] GetGaussianFilter(int XDist, int YDist, double o)
        {
            double[,] filter = new double[XDist*2 + 1,YDist*2 + 1];
            double inv = 0.5 / (o * o);
            Parallel.For(-XDist, XDist + 1, x =>
            {
                for (int y = -YDist; y <= YDist; y++)
                {
                    //get gausian for x and y
                    filter[XDist + x, YDist + y] = GetGaussian(x, y, inv);
                }
            });

            return filter;
        }

        double[,] GetLoGFilter(int XDist, int YDist, double o)
        {
            double[,] filter = new double[XDist * 2 + 1, YDist * 2 + 1];
            double inv = 0.5 / (o * o);
            double piInv = -1 / (Math.PI * o * o * o * o);
            Parallel.For(-XDist, XDist + 1, x =>
            {
                for (int y = -YDist; y <= YDist; y++)
                {
                    double quotient = (double)(-(x * x + y * y)) * inv;

                    //get laubplassian of gausian for x and y
                    filter[XDist + x, YDist + y] = GetLoG(x, y, piInv, quotient);
                }
            });

            return filter;
        }

        double GetGaussian(int x, int y, double inverse)
        {
            return Math.Pow(Math.E, (double)(-(x * x + y * y)) * inverse);
        }

        double GetLoG(int x, int y, double piInv, double quotient)
        {
            return piInv * (1 + quotient) * Math.Pow(Math.E, quotient);
        }

        int GetPixelDist(System.Drawing.Color p1, System.Drawing.Color p2, bool getSign = false)
        {
            int r = (int)p1.R - (int)p2.R;
            int g = (int)p1.G - (int)p2.G;
            int b = (int)p1.B - (int)p2.B;

            int sign = 1;

            if (getSign && (r + g + b) < 0) sign = -1;

            return sign * (int)Math.Sqrt(r * r + g * g + b * b);
        }

        int GetPixel2DDist(System.Drawing.Color p1, System.Drawing.Color p2)
        {
            int r = (int)p1.R - (int)p2.R;
            int g = (int)p1.G - (int)p2.G;

            return (int)Math.Sqrt(r * r + g * g);
        }

        double GetDeterminate(double[,] matrix)
        {
            if (matrix.GetLength(0) != 2 || matrix.GetLength(1) != 2)
            {
                throw new Exception("Expected 2X2 matrix.");
            }

            return (matrix[0, 0] * matrix[1, 1] - matrix[1, 0] * matrix[0, 1]);
        } 
    }
}
