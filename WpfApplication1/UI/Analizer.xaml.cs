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
    public partial class Analizer : Window
    {
        public Analizer()
        {
            InitializeComponent();

            this.DataContext = new AnalizerViewModel();
        }

        private void GetImage_Click(object sender, RoutedEventArgs e)
        {
            ((AnalizerViewModel)this.DataContext).GetImage_Click(sender, e);
        }

        private void BlurImage_Click(object sender, RoutedEventArgs e)
        {
            ((AnalizerViewModel)this.DataContext).BlurImage_Click(sender, e);
        }
    }
}
