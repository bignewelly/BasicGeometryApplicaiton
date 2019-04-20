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

namespace WpfApplication1.UI
{
    public partial class AnalizerViewModel : INotifyPropertyChanged
    {
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
            BlurImage(GetPixels(new System.Drawing.Bitmap(ImageFile)));
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

        private void BlurImage(System.Drawing.Color[,] Image)
        {

            int width = Image.GetLength(0);
            int height = Image.GetLength(1);

            uint[] pixels = new uint[Image.Length];
            WriteableBitmap bitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgra32, null);

            int xDist = 5;
            int yDist = xDist;

            int xCount = (xDist * 2) + 1;
            int yCount = (yDist * 2) + 1;

            double[] inverses = new double[(xCount * yCount) + 1];

            Parallel.For(1, inverses.Length, i =>
           {
               inverses[i] = 1.0 / i;
           });

            Parallel.For(0, width, x =>
            {
                for (int y = 0; y < height; y++)
                {

                    int pixelCount = 0;
                    int r = 0;
                    int g = 0;
                    int b = 0;
                    int a = 0;

                    for (int i = -xDist; i <= xDist; i++)
                    {
                        if (x + i >= 0 && x + i < width)
                        {
                            for (int j = -yDist; j <= yDist; j++)
                            {
                                if (y + j >= 0 && y + j < height)
                                {
                                    System.Drawing.Color pixel = Image[x + i, y + j];

                                    r += (int)pixel.R;
                                    g += (int)pixel.G;
                                    b += (int)pixel.B;
                                    a += (int)pixel.A;
                                    pixelCount++;
                                }
                            }
                        }
                    }

                    if (pixelCount > 0)
                    {
                        System.Drawing.Color currentPixel = System.Drawing.Color.FromArgb((int)(a * inverses[pixelCount]), (int)(r * inverses[pixelCount]), (int)(g * inverses[pixelCount]), (int)(b * inverses[pixelCount]));
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
    }
}
