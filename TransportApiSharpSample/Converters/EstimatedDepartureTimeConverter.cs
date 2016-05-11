using Exceptionless.DateTimeExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace TransportApiSharpSample.Converters
{
    public class EstimatedDepartureTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var dateNow = DateTime.Now;
            var timeNow = dateNow.TimeOfDay;
            var valueTime = (TimeSpan)value;

            // If the bus time is less than the time now, then it's one of today's buses, else it's
            // one of tomorrow's.
            if (valueTime < timeNow)
                dateNow.AddDays(1);

            var busDateTime = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, valueTime.Hours, valueTime.Minutes, 0);

            // ToApproximateAgeString() has an annoying minus sign in front...
            var ageString = busDateTime.ToApproximateAgeString().TrimStart('-');

            return $"Estimated actual arrival: {busDateTime.ToString(@"HH\:mm")} ({ageString})";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}