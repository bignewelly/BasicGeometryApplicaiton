using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfApplication1.UI;

namespace BasicGeometryApp
{
    /// <summary>
    /// Interaction logic for Canvas.xaml
    /// </summary>
    public partial class PhotoGrammetry : Window
    {
        public PhotoGrammetry()
        {
            InitializeComponent();

            this.DataContext = new PhotoGrammetryModel();

        }

        private void CalibrateCamera_Click(object sender, RoutedEventArgs e)
        {
            ((PhotoGrammetryModel)this.DataContext).CalibrateCamera_Click(sender, e);
        }

        private void GetFiles_Click(object sender, RoutedEventArgs e)
        {
            ((PhotoGrammetryModel)this.DataContext).GetFiles_Click(sender, e);
        }
    }
}
