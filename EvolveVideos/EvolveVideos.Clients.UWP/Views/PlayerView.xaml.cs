using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using EvolveVideos.Clients.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace EvolveVideos.Clients.UWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PlayerView : Page
    {
        private PlayerViewModel ViewModel => this.DataContext as PlayerViewModel;

        public PlayerView()
        {
            this.InitializeComponent();

            this.MediaElement.MediaFailed += OnMediaFailed;
            this.MediaElement.MediaEnded += OnMediaEnded;
        }

        private async void OnMediaEnded(object sender, RoutedEventArgs e)
        {
            if (this.ViewModel != null)
            {
                await this.ViewModel.OnMediaEndedAsync();
            }
        }

        private async void OnMediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            if (this.ViewModel != null)
            {
                await this.ViewModel.OnMediaFailedAsync(e.ErrorMessage);
            }
        }
    }
}