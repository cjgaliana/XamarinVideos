using EvolveVideos.Clients.Core.Services;

namespace EvolveVideos.Clients.Core.ViewModels
{
    public class DownloadsViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IDownloadManager _downloadManager;

        public DownloadsViewModel(INavigationService navigationService, IDownloadManager downloadManager)
        {
            _navigationService = navigationService;
            _downloadManager = downloadManager;
        }
    }
}