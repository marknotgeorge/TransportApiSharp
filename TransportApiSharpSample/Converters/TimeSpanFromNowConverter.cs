using Exceptionless.DateTimeExtensions;
using System;
using Windows.UI.Xaml.Data;

namespace TransportApiSharpSample.Converters
{
    public class TimeSpanFromNowConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var valueSpan = (TimeSpan)value;

            var dateNow = DateTime.Now;
            var timeNow = dateNow.TimeOfDay;

            // If the bus time is less than the time now, then it's one of today's buses, else it's
            // one of tomorrow's.
            if (valueSpan < timeNow)
                dateNow.AddDays(1);

            var busDateTime = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, valueSpan.Hours, valueSpan.Minutes, 0);

            // ToApproximateAgeString() has an annoying minus sign in front...
            var ageString = busDateTime.ToApproximateAgeString().TrimStart('-');

            return $"Aimed departure time: {busDateTime.ToString(@"HH\:mm")} ({ageString})";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}