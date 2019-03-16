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
using System.Windows.Shapes;

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
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Controls.Canvas canvas = GetCanvas();

            MouseStartPoint = e.GetPosition(canvas);
            MouseEndPoint = e.GetPosition(canvas);

            Transform transform = new TranslateTransform(MouseStartPoint.X, MouseStartPoint.Y);

            Canvas_ReDraw(transform);
        }

        private void Canvas_ReDraw(Transform transform)
        {
            RectangleGeometry rectGeo = GetRectangleGeometry();

            rectGeo.Transform = transform;

            //rectGeo.Bounds.TopLeft = MouseStartPoint;
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private RectangleGeometry GetRectangleGeometry()
        {
            RectangleGeometry rectGeo = (RectangleGeometry)this.FindName("rectGeo");

            if (rectGeo == null)
            {
                System.Windows.Controls.Canvas drawingCanvas = GetCanvas();

                rectGeo = new RectangleGeometry(new Rect(new Point(-1000, -1000), new Point(1000, 1000)));

                GeometryGroup group = new GeometryGroup();
                group.Children.Add(rectGeo);

                Path rectPath = new Path();

                rectPath.Stroke = Brushes.Black;
                rectPath.StrokeThickness = 1;
                rectPath.Data = rectGeo;
            }

            return rectGeo;
        }

        private System.Windows.Controls.Canvas GetCanvas()
        {
            return (System.Windows.Controls.Canvas)this.FindName("drawingCanvas");
        }

        protected Point MouseStartPoint { get; set; }
        protected Point MouseEndPoint { get; set; }
    }
}
