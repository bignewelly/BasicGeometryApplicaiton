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
    public partial class CanvasViewModel : INotifyPropertyChanged
    {
        public CanvasViewModel(System.Windows.Controls.Canvas Canvas)
        {
            this.DrawingCanvas = Canvas;
            // initialize our base matrix
            TransformMatrix = new Matrix();
            SelectedTransformationType = WpfApplication1.Geometry.TransformationTypes.Unselected;

            TransformTypesData = new ObservableCollection<KeyValuePair<WpfApplication1.Geometry.TransformationTypes, String>>();

            foreach (WpfApplication1.Geometry.TransformationTypes type in Enum.GetValues(typeof(WpfApplication1.Geometry.TransformationTypes)))
            {
                TransformTypesData.Add(new KeyValuePair<WpfApplication1.Geometry.TransformationTypes, string>(type, type.ToString()));
            }

        }

        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        public void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {

            switch (SelectedTransformationType)
            {
                case WpfApplication1.Geometry.TransformationTypes.Translation:
                    LastMouseLocation = e.GetPosition(DrawingCanvas);
                    break;

                case WpfApplication1.Geometry.TransformationTypes.Similarity:
                //TODO: Update the rotational transform
                case WpfApplication1.Geometry.TransformationTypes.Rigid:
                //Todo: update botht he scale and transform transforms
                case WpfApplication1.Geometry.TransformationTypes.Projective:
                // todo: update the projective transform (this might need to be done with a matrix)
                case WpfApplication1.Geometry.TransformationTypes.Affine:
                // todo: update the affine transform  (this might need to be with a matrix)
                case WpfApplication1.Geometry.TransformationTypes.Unselected:
                default:

                    Matrix matrix = new Matrix();
                    LastMouseLocation = e.GetPosition(DrawingCanvas);
                    matrix.Translate(LastMouseLocation.Value.X, LastMouseLocation.Value.Y);

                    TransformMatrix = matrix;
                    break;
            }

            //Canvas_ReDraw();
        }

        public void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Point currentMouseLocation = e.GetPosition(DrawingCanvas);

            switch (SelectedTransformationType)
            {
                case WpfApplication1.Geometry.TransformationTypes.Translation:
                    Matrix matrix = new Matrix();
                    matrix.Translate(LastMouseLocation.Value.X - currentMouseLocation.X, LastMouseLocation.Value.Y - currentMouseLocation.Y);
                    break;

                case WpfApplication1.Geometry.TransformationTypes.Similarity:
                //TODO: Update the rotational transform
                case WpfApplication1.Geometry.TransformationTypes.Rigid:
                //Todo: update botht he scale and transform transforms
                case WpfApplication1.Geometry.TransformationTypes.Projective:
                // todo: update the projective transform (this might need to be done with a matrix)
                case WpfApplication1.Geometry.TransformationTypes.Affine:
                // todo: update the affine transform  (this might need to be with a matrix)
                case WpfApplication1.Geometry.TransformationTypes.Unselected:
                default:
                    TransformGroup transfromGroup = new TransformGroup();
                    transfromGroup.Children.Add(ScaleTransform_Get(currentMouseLocation));
                    transfromGroup.Children.Add(TranslateTransform_Get(currentMouseLocation));

                    TransformMatrix = transfromGroup.Value;

                    LastMouseLocation = null;
                    break;
            }

            LastMouseLocation = null;

            //Canvas_ReDraw();
        }

        public void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            Point currentMouseLocation = e.GetPosition(DrawingCanvas);

            switch (SelectedTransformationType)
            {
                case WpfApplication1.Geometry.TransformationTypes.Translation:

                    Matrix matrix = new Matrix();
                    matrix.Translate(LastMouseLocation.Value.X - currentMouseLocation.X, LastMouseLocation.Value.Y - currentMouseLocation.Y);


                    LastMouseLocation = currentMouseLocation;
                    break;

                case WpfApplication1.Geometry.TransformationTypes.Similarity:
                //TODO: Update the rotational transform
                case WpfApplication1.Geometry.TransformationTypes.Rigid:
                //Todo: update botht he scale and transform transforms
                case WpfApplication1.Geometry.TransformationTypes.Projective:
                // todo: update the projective transform (this might need to be done with a matrix)
                case WpfApplication1.Geometry.TransformationTypes.Affine:
                // todo: update the affine transform  (this might need to be with a matrix)
                case WpfApplication1.Geometry.TransformationTypes.Unselected:
                default:
                    if (LastMouseLocation != null)
                    {
                        TransformGroup transfromGroup = new TransformGroup();
                        transfromGroup.Children.Add(ScaleTransform_Get(currentMouseLocation));
                        transfromGroup.Children.Add(TranslateTransform_Get(currentMouseLocation));

                        TransformMatrix = transfromGroup.Value;
                    }
                    break;
            }
            //Canvas_ReDraw();
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

        //private void Canvas_ReDraw()
        //{
        //    RectangleGeometry rectGeo = RectangleGeometry_Get();

        //    rectGeo.Transform = Transform;

        //}

        //private RectangleGeometry RectangleGeometry_Get()
        //{
        //    if (RectGeo == null)
        //    {
        //        GeometryGroup group = new GeometryGroup();
        //        group.Children.Add(RectGeo);

        //        Path rectPath = new Path();

        //        rectPath.Stroke = Brushes.Black;
        //        rectPath.StrokeThickness = 1;
        //        rectPath.Fill = Brushes.Aqua;
        //        rectPath.Data = RectGeo;

        //        DrawingCanvas.Children.Add(rectPath);
        //    }

        //    return RectGeo;
        //}


        private System.Windows.Controls.Canvas _DrawingCanvas;

        public System.Windows.Controls.Canvas DrawingCanvas
        {
            get
            {
                return _DrawingCanvas;
            }

            set
            {
                if (_DrawingCanvas != value)
                {
                    _DrawingCanvas = value;
                    this.PropertyChanged(this, new PropertyChangedEventArgs(nameof(DrawingCanvas)));
                }
            }
        }

        private Point? _LastMouseLocation;
        private Point? LastMouseLocation
        {
            get
            {
                return _LastMouseLocation;
            }
            set
            {
                _LastMouseLocation = value;
                this.PropertyChanged(this, new PropertyChangedEventArgs(nameof(LastMouseLocation)));
            }
        }

        private ObservableCollection<KeyValuePair<WpfApplication1.Geometry.TransformationTypes, String>> _TransformTypesData;
        public ObservableCollection<KeyValuePair<WpfApplication1.Geometry.TransformationTypes, String>> TransformTypesData
        {
            get
            {
                return _TransformTypesData;
            }
            set
            {
                _TransformTypesData = value;
                this.PropertyChanged(this, new PropertyChangedEventArgs(nameof(TransformTypesData)));
            }
        }

        private WpfApplication1.Geometry.TransformationTypes _SelectedTransformationType;
        public WpfApplication1.Geometry.TransformationTypes SelectedTransformationType
        {
            get
            {
                return _SelectedTransformationType;
            }
            set
            {
                _SelectedTransformationType = value;
                this.PropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedTransformationType)));
            }
        }

        ///// <summary>
        ///// Transform Matrix transform.
        ///// </summary>
        //public MatrixTransform Transform
        //{
        //    get
        //    {
        //        AssertExists();
        //        return RectangleObjects[0].Transform;
        //    }
        //    set
        //    {
        //        AssertExists();
        //        RectangleObjects[0].Transform = value;
        //        this.PropertyChanged(this, new PropertyChangedEventArgs(nameof(RectangleObjects)));
        //        this.PropertyChanged(this, new PropertyChangedEventArgs(nameof(Transform)));
        //    }
        //}



        private Matrix _TransformMatrix;
        /// <summary>
        /// Transform Matrix .
        /// </summary>
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
                //this.PropertyChanged(this, new PropertyChangedEventArgs(nameof(RectangleObjects)));
                //this.PropertyChanged(this, new PropertyChangedEventArgs(nameof(Transform)));
            }
        }
        //private void AssertExists()
        //{
        //    if (RectangleObjects == null)
        //    {
        //        RectangleObjects = new ObservableCollection<Geometry.RectObject>();
        //    }
        //    if (!RectangleObjects.Any())
        //    {
        //        Geometry.RectObject rectObj = new Geometry.RectObject();
        //        rectObj.Visible = true;
        //        RectangleObjects.Add(rectObj);
        //    }
        //}

        //private ObservableCollection<WpfApplication1.Geometry.RectObject> _RectangleObjects;
        //public ObservableCollection<WpfApplication1.Geometry.RectObject> RectangleObjects
        //{
        //    get
        //    {
        //        return _RectangleObjects;
        //    }
        //    set
        //    {
        //        _RectangleObjects = value;
        //        this.PropertyChanged(this, new PropertyChangedEventArgs(nameof(RectangleObjects)));
        //    }
        //}



    }
}
