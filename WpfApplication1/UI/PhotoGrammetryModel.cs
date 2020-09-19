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
using OpenCvSharp;
using WpfApplication1.Properties;

namespace WpfApplication1.UI
{
    public partial class PhotoGrammetryModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        public void CalibrateCamera_Click(object sender, RoutedEventArgs e)
        {
            Mat imageMat = new Mat(ImageFiles);
            InputArray image = new Mat(ImageFiles);
            OpenCvSharp.Size patternSize = new OpenCvSharp.Size(PatternWidth, PatternHeight);
            OutputArray corners = OutputArray.Create(imageMat);

            var result = Cv2.FindChessboardCorners(image, patternSize, corners);

            ErrorMsg += result.ToString();

        }

        private String _ImageFile = "/Images/TestImage.jpg" ;

        public String ImageFiles
        {
            get
            {
                return _ImageFile;
            }
            set
            {
                _ImageFile = value;


                this.PropertyChanged(this, new PropertyChangedEventArgs(nameof(ImageFiles)));
            }
        }


        private int _PatternWidth = 6;

        public int PatternWidth
        {
            get
            {
                return _PatternWidth;
            }
            set
            {
                _PatternWidth = value;


                this.PropertyChanged(this, new PropertyChangedEventArgs(nameof(PatternWidth)));
            }
        }

        private int _PatternHeight = 9;

        public int PatternHeight
        {
            get
            {
                return _PatternHeight;
            }
            set
            {
                _PatternHeight = value;


                this.PropertyChanged(this, new PropertyChangedEventArgs(nameof(PatternHeight)));
            }
        }

        public void GetFiles_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.DefaultExt = ".jpg";
            dlg.Filter = "All Types (*.*)|*.*|JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
            dlg.Multiselect = true;

            bool? result = dlg.ShowDialog();

            if (result.HasValue)
            {
                ImageFiles = dlg.FileName;
            }
        }

        private string _ErrorMsg = String.Empty;
        public string ErrorMsg
        {
            get
            {
                return _ErrorMsg;
            }
            set
            {
                _ErrorMsg = value;
                this.PropertyChanged(this, new PropertyChangedEventArgs(nameof(ErrorMsg)));
            }
        }
    }

}
