using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace EvolveVideos.Clients.UWP.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var isVisible = (bool) value;
            if (parameter != null)
            {
                isVisible = !isVisible;
            }

            return isVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var visibility = (Visibility) value;
            var isVisible = visibility == Visibility.Visible;
            if (parameter != null)
            {
                isVisible = !isVisible;
            }

            return isVisible;
        }
    }
}