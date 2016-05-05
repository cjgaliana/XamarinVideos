using System.Threading.Tasks;
using EvolveVideos.Clients.Core.Models;
using EvolveVideos.Clients.Core.Utils;
using EvolveVideos.Clients.Services;
using EvolveVideos.Clients.Services.Download;

namespace EvolveVideos.Clients.ViewModels
{
    public class PlayerViewModel : BaseViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly IDownloadManager _downloadManager;
        private readonly INavigationService _navigationService;
        private readonly INetworkService _networkService;
        private readonly IVideoDownloaderService _videoDownloaderService;

        private PlayerParameters _playerParameters;
        private string _videoUrl;

        public PlayerViewModel(
            INavigationService navigationService,
            INetworkService networkService,
            IDownloadManager downloadManager,
            IDialogService dialogService,
            IVideoDownloaderService videoDownloaderService)
        {
            this._navigationService = navigationService;
            this._networkService = networkService;
            this._downloadManager = downloadManager;
            this._dialogService = dialogService;
            this._videoDownloaderService = videoDownloaderService;
        }

        public string VideoUrl
        {
            get { return this._videoUrl; }
            set { this.Set(() => this.VideoUrl, ref this._videoUrl, value); }
        }

        public PlayerParameters PlayerParameters
        {
            get { return _playerParameters; }
            set { this.Set(() => this.PlayerParameters, ref this._playerParameters, value); }
        }

        public override async Task OnNavigateTo(object parameter)
        {
            await base.OnNavigateTo(parameter);

            var playerParameters = parameter as PlayerParameters;
            if (playerParameters == null)
            {
                await this._dialogService.ShowMessageAsync("Error", "Incorrect navigation parameters");
                this._navigationService.GoBack();
                return;
            }

            this.PlayerParameters = playerParameters;

            var streamUri = this.PlayerParameters.Url;
            this.VideoUrl = streamUri.AbsoluteUri;
        }

        public async Task OnMediaFailedAsync(string message = null)
        {
            var errorMessage = "Is not possible to play this video";
            if (!string.IsNullOrWhiteSpace(message))
            {
                errorMessage = $"{errorMessage} {message}";
            }
            await this._dialogService.ShowMessageAsync("Error", errorMessage);
            this._navigationService.GoBack();
        }

        public Task OnMediaEndedAsync()
        {
            this._navigationService.GoBack();
            return TaskUtils.CompletedTask;
        }
    }
}