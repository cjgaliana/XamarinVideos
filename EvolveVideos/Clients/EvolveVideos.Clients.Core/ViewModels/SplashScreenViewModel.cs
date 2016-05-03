using EvolveVideos.Clients.Core.Services;
using System.Threading.Tasks;
using EvolveVideos.Clients.Core.Services.Download;

namespace EvolveVideos.Clients.Core.ViewModels
{
    public class SplashScreenViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IDownloadManager _downloadManager;

        public SplashScreenViewModel(INavigationService navigationService, IDownloadManager downloadManager)
        {
            _navigationService = navigationService;
            _downloadManager = downloadManager;
        }

        public async Task InitializeAsync()
        {
            // Do things,
            await this._downloadManager.InitializeAsync();
        }
    }
}