using EvolveVideos.Clients.Core.Services;
using System;
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

        //public override async Task OnNavigateTo(object parameter)
        //{
        //    await base.OnNavigateTo(parameter);

        //}

        public async Task InitializeAsync()
        {
            // Do things,
            await Task.Delay(TimeSpan.FromSeconds(3));
        }
    }
}