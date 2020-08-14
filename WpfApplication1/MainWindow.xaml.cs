using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BasicGeometryApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Canvas canvas = new Canvas();

            canvas.Show();
        }

        private void PictureAnalizer_Click(object sender, RoutedEventArgs e)
        {
            Analizer analizer = new Analizer();

            analizer.Show();
        }

        private void PhotoGrammetry_Click(object sender, RoutedEventArgs e)
        {
            PhotoGrammetry photoGrammetry = new PhotoGrammetry();

            photoGrammetry.Show();
        }
    }
}
