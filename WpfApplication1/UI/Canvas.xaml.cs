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
    public partial class Canvas : Window
    {
        public Canvas()
        {
            InitializeComponent();

            this.DataContext = new CanvasViewModel((System.Windows.Controls.Canvas)this.FindName("drawingCanvas"));
        }

        public void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ((CanvasViewModel)this.DataContext).Canvas_MouseDown(sender, e);
        }

        public void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ((CanvasViewModel)this.DataContext).Canvas_MouseUp(sender, e);
        }

        public void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            ((CanvasViewModel)this.DataContext).Canvas_MouseMove(sender, e);
        }


    }
}
