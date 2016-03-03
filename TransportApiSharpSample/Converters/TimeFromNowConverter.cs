using Exceptionless.DateTimeExtensions;
using System;
using Windows.UI.Xaml.Data;

namespace TransportApiSharpSample.Converters
{
    public class TimeFromNowConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string responseString = string.Empty;
            if (value != null)
            {
                var valueDT = (DateTime)value;

                // ToApproximateAgeString() has an annoying minus sign in front...
                var ageString = valueDT.ToApproximateAgeString().TrimStart('-');

                responseString = $"{valueDT.ToString(@"HH\:mm")} ({ageString})";
            }
            return responseString;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}