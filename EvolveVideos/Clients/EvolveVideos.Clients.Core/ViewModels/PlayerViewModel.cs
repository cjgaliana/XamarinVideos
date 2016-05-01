using System.Threading.Tasks;
using EvolveVideos.Clients.Core.Services;
using EvolveVideos.Data.Models;

namespace EvolveVideos.Clients.Core.ViewModels
{
    public class PlayerViewModel : BaseViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly IDownloadManager _downloadManager;
        private readonly INavigationService _navigationService;
        private readonly INetworkService _networkService;
        private readonly IVideoDownloaderService _videoDownloaderService;

        private EvolveSession _session;
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

        public EvolveSession Session
        {
            get { return this._session; }
            set { this.Set(() => this.Session, ref this._session, value); }
        }

        public string VideoUrl
        {
            get { return this._videoUrl; }
            set { this.Set(() => this.VideoUrl, ref this._videoUrl, value); }
        }

        public override async Task OnNavigateTo(object parameter)
        {
            await base.OnNavigateTo(parameter);

            var session = parameter as EvolveSession;
            if (session == null)
            {
                await this._dialogService.ShowMessageAsync("Error", "Incorrect navigation parameters");
                this._navigationService.GoBack();
                return;
            }

            this.Session = session;

            var streamUri = await this._videoDownloaderService.GetDownloadVideoUrlAsync(this.Session.YoutubeID);
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
            return Task.CompletedTask;
        }
    }
}