using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace TransportApiSharpSample.Converters
{
    public class OperatorCodeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string returnVal = "Unknown Operator";

            var valueString = value as string;

            if (App.OperatorCodes != null)
            {
                if (valueString != null)
                {
                    var operatorRecord = App.OperatorCodes
                        .Where(operators => operators.Code == valueString)
                        .ToList();

                    if (operatorRecord != null)
                        returnVal = operatorRecord[0].ShortName;
                }
            }

            return returnVal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}