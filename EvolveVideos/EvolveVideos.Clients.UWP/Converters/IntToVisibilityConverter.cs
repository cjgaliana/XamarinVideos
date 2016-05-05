using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace EvolveVideos.Clients.UWP.Converters
{
    public class IntToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var number = (int) value;
            var isVisible = number > 0;
            if (parameter!=null)
            {
                isVisible = !isVisible;
            }
            return isVisible ? Visibility.Visible : Visibility.Collapsed;
        }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new System.NotImplementedException();
        }
    }
}