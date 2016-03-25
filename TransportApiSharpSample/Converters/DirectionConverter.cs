using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace TransportApiSharpSample.Converters
{
    public class DirectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var returnString = string.Empty;

            var valueString = value as string;

            if (valueString != null)
                returnString = $"Towards {valueString}";

            return returnString;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}