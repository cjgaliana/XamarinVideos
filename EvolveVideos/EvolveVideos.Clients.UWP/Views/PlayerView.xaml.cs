using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using EvolveVideos.Clients.ViewModels;
using Windows.System.Display;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace EvolveVideos.Clients.UWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PlayerView : Page
    {
        private PlayerViewModel ViewModel => this.DataContext as PlayerViewModel;

		
		DisplayRequest _displayRequest;
		
        public PlayerView()
        {
            this.InitializeComponent();

            this.MediaElement.MediaFailed += OnMediaFailed;
            this.MediaElement.MediaEnded += OnMediaEnded;
			
			_displayRequest = new DisplayRequest();
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            this._displayRequest?.RequestActive();
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            this._displayRequest?.RequestRelease();
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