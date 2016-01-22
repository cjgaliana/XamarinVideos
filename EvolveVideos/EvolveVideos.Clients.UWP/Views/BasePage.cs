using EvolveVideos.Clients.Core.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace EvolveVideos.Clients.UWP.Views
{
    public class BasePage : Page
    {
        private EvolveBaseViewModel BasePageViewModel => this.DataContext as EvolveBaseViewModel;

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            this.BasePageViewModel.OnNavigateFrom(e.Parameter);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.BasePageViewModel.OnNavigateTo(e.Parameter);
        }
    }
}