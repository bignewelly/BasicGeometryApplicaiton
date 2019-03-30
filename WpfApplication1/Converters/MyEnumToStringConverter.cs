using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace WpfApplication1.Converters
{
    [ValueConversion(typeof(Geometry.TransformationTypes), typeof(string))]
    public class MyEnumToStringConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                return value.ToString();
            }

            public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                return (SelectionMode)Enum.Parse(typeof(SelectionMode), value.ToString(), true);
            }
        }
}
