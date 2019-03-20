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

            // initialize our base matrix
            Transform = new MatrixTransform();
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            switch (selectedTransformationType)
            {
                case WpfApplication1.Geometry.TransformationTypes.Translation:
                    LastMouseLocation = e.GetPosition(Canvas_Get());
                    break;

                case WpfApplication1.Geometry.TransformationTypes.Similarity:
                //TODO: Update the rotational transform
                case WpfApplication1.Geometry.TransformationTypes.Rigid:
                //Todo: update botht he scale and transform transforms
                case WpfApplication1.Geometry.TransformationTypes.Projective:
                // todo: update the projective transform (this might need to be done with a matrix)
                case WpfApplication1.Geometry.TransformationTypes.Affine:
                // todo: update the affine transform  (this might need to be with a matrix)
                default:
                    Matrix matrix = new Matrix();
                    LastMouseLocation = e.GetPosition(Canvas_Get());
                    matrix.Translate(LastMouseLocation.Value.X, LastMouseLocation.Value.Y);

                    Transform.Matrix = matrix;
                    break;
            }

            Canvas_ReDraw();
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Point currentMouseLocation = e.GetPosition(Canvas_Get());

            TransformGroup transfromGroup = new TransformGroup();
            transfromGroup.Children.Add(ScaleTransform_Get(currentMouseLocation));
            transfromGroup.Children.Add(TranslateTransform_Get(currentMouseLocation));

            Transform.Matrix = transfromGroup.Value;

            LastMouseLocation = null;

            Canvas_ReDraw();
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (LastMouseLocation != null)
            {
                Point currentMouseLocation = e.GetPosition(Canvas_Get());

                TransformGroup transfromGroup = new TransformGroup();
                transfromGroup.Children.Add(ScaleTransform_Get(currentMouseLocation));
                transfromGroup.Children.Add(TranslateTransform_Get(currentMouseLocation));

                Transform.Matrix = transfromGroup.Value;

                Canvas_ReDraw();
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

        private void Canvas_ReDraw()
        {
            RectangleGeometry rectGeo = RectangleGeometry_Get();

            rectGeo.Transform = Transform;

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
        private RectangleGeometry RectGeo { get; set; }
        private WpfApplication1.Geometry.TransformationTypes selectedTransformationType { get; set; }

        /// <summary>
        /// Transform Matrix transform.
        /// </summary>
        private MatrixTransform Transform { get; set; }

    }
}
