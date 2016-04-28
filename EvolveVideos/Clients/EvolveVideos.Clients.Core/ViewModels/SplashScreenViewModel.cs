using EvolveVideos.Clients.Core.Services;
using System.Threading.Tasks;

namespace EvolveVideos.Clients.Core.ViewModels
{
    public class SplashScreenViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;

        public SplashScreenViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public Task InitializeAsync()
        {
            // Do things,
            return Task.CompletedTask;
        }
    }
}