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

        private void BlurImage(System.Drawing.Bitmap Image)
        {
            for (int x = 0; x < Image.Width; x++)
            {
                for (int y = 0; y < Image.Height; x++)
                {
                    //Todo: average pixel with its neighbors.
                }
            }
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

                if (System.IO.File.Exists(ImageFile))
                {
                    System.Drawing.Image image = System.Drawing.Image.FromFile(System.IO.Path.GetFullPath(ImageFile));

                    TransformMatrix = GetTransformMatrixForImage(image);
                }

                this.PropertyChanged(this, new PropertyChangedEventArgs(nameof(ImageFile)));
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
