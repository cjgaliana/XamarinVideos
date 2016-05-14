using EvolveVideos.Clients.Services;
using Microsoft.Practices.ServiceLocation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace EvolveVideos.Clients.UWP.Controls
{
    public sealed partial class HamburgerPaneControl : UserControl
    {
        public HamburgerPaneControl()
        {
            this.InitializeComponent();
        }

        private void HamburgerMenuButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
        }

        private void OpenAppStrech(object sender, TappedRoutedEventArgs e)
        {
            var launcherService = ServiceLocator.Current.GetInstance<ILauncherService>();
            if (launcherService != null)
            {
                launcherService.OpenWebSiteAsync("https://appstretch.com/App/35483/Videos-for-Xamarin-Evolve");
            }
        }
    }
}