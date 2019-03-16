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
            System.Windows.Controls.Canvas canvas = Canvas_Get();

            LastMouseLocation = e.GetPosition(canvas);

            Transform transform = new TranslateTransform(LastMouseLocation.Value.X, LastMouseLocation.Value.Y);

            Canvas_ReDraw(transform);
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Point currentMouseLocation = e.GetPosition(Canvas_Get());

            TransformGroup transfromGroup = new TransformGroup();
            transfromGroup.Children.Add(ScaleTransform_Get(currentMouseLocation));
            transfromGroup.Children.Add(TranslateTransform_Get(currentMouseLocation));

            LastMouseLocation = null;

            Canvas_ReDraw(transfromGroup);
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (LastMouseLocation != null)
            {
                Point currentMouseLocation = e.GetPosition(Canvas_Get());

                TransformGroup transfromGroup = new TransformGroup();
                transfromGroup.Children.Add(ScaleTransform_Get(currentMouseLocation));
                transfromGroup.Children.Add(TranslateTransform_Get(currentMouseLocation));

                Canvas_ReDraw(transfromGroup);
            }
        }

        private Transform ScaleTransform_Get(Point CurrentPoint)
        {
            double diffX = Math.Abs(CurrentPoint.X - LastMouseLocation.Value.X);
            double diffY = Math.Abs(CurrentPoint.Y - LastMouseLocation.Value.Y);

            return new ScaleTransform(diffX, diffY);
        }

        private Transform TranslateTransform_Get(Point CurrentPoint)
        {
            double locX = Math.Min(LastMouseLocation.Value.X, CurrentPoint.X);
            double locY = Math.Min(LastMouseLocation.Value.Y, CurrentPoint.Y);

            return new TranslateTransform(locX, locY);
        }

        private void Canvas_ReDraw(Transform transform)
        {
            RectangleGeometry rectGeo = RectangleGeometry_Get();

            rectGeo.Transform = transform;

        }

        private RectangleGeometry RectangleGeometry_Get()
        {
            if (RectGeo == null)
            {
                System.Windows.Controls.Canvas drawingCanvas = Canvas_Get();

                RectGeo = new RectangleGeometry(new Rect(new Point(0, 0), new Point(1, 1)));

                GeometryGroup group = new GeometryGroup();
                group.Children.Add(RectGeo);

                Path rectPath = new Path();

                rectPath.Stroke = Brushes.Black;
                rectPath.StrokeThickness = 1;
                rectPath.Fill = Brushes.Aqua;
                rectPath.Data = RectGeo;

                drawingCanvas.Children.Add(rectPath);
            }

            return RectGeo;
        }

        private System.Windows.Controls.Canvas Canvas_Get()
        {
            return (System.Windows.Controls.Canvas)this.FindName("drawingCanvas");
        }

        private Point? LastMouseLocation { get; set; }
        private RectangleGeometry RectGeo { get; set;}
    }
}
