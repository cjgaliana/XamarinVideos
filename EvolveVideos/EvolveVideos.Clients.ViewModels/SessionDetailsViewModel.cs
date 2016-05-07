using EvolveVideos.Clients.Core.Models;
using EvolveVideos.Clients.Services;
using EvolveVideos.Clients.Services.Download;
using EvolveVideos.Clients.UWP.DesignData;
using EvolveVideos.Data.Models;
using GalaSoft.MvvmLight.Command;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EvolveVideos.Clients.ViewModels
{
    public class SessionDetailsViewModel : BaseViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly IDownloadManager _downloadManager;
        private readonly IVideoDownloaderService _videoDownloaderService;
        private readonly INetworkService _networkService;
        private readonly INavigationService _navigationService;
        private bool _hasDownload;

        private EvolveSession _session;

        private IVideoDownload _videoDownload;

        private Uri _videoUrl;

        public SessionDetailsViewModel(INavigationService navigationService, IDialogService dialogService,
            IDownloadManager downloadManager, IVideoDownloaderService videoDownloaderService, INetworkService networkService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _downloadManager = downloadManager;
            _videoDownloaderService = videoDownloaderService;
            _networkService = networkService;

            CreateCommands();

            if (IsInDesignMode)
            {
                Session = DesignData.GetSession();
            }
        }

        public EvolveSession Session
        {
            get { return _session; }
            set { Set(() => Session, ref _session, value); }
        }

        public Uri VideoUrl
        {
            get { return _videoUrl; }
            set { Set(() => VideoUrl, ref _videoUrl, value); }
        }

        public ICommand PlayCommand { get; private set; }

        public ICommand DownloadCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }

        public IVideoDownload VideoDownload
        {
            get { return _videoDownload; }
            set
            {
                Set(() => VideoDownload, ref _videoDownload, value);
                RaisePropertyChanged(() => HasDownload);
                RaisePropertyChanged(() => IsDownloading);
            }
        }

        public bool HasDownload => VideoDownload != null;
        public bool IsDownloading => this.HasDownload && this.VideoDownload.Status == DownloadStatus.Completed;

        private void CreateCommands()
        {
            PlayCommand = new RelayCommand(async () => { await this.PlayVideoAsync(); });
            DownloadCommand = new RelayCommand(async () => { await DownloadVideoAsync(); });
            DeleteCommand = new RelayCommand(async () => { await DeleteDownloadAsync(); });
        }

        public override async Task OnNavigateTo(object parameter)
        {
            await base.OnNavigateTo(parameter);

            try
            {
                var session = parameter as EvolveSession;
                if (session != null)
                {
                    IsBusy = true;
                    Session = session;

                    VideoDownload = await _downloadManager.GetDownloadForSessionAsync(Session);
                }
            }
            catch (Exception ex)
            {
                // TODO: Log error
                await _dialogService.ShowMessageAsync("Error", "Is not possible load the item");
                _navigationService.GoBack();
            }
        }

        private async Task PlayVideoAsync()
        {
            try
            {
                var parameters = new PlayerParameters
                {
                    Title = Session.Title,
                };

                // Play video
                if (HasDownload)
                {
                    if (VideoDownload.Status == DownloadStatus.Completed)
                    {
                        parameters.Url = VideoDownload.LocalFileUrl;
                    }
                }
                if (parameters.Url == null || string.IsNullOrWhiteSpace(parameters.Url.AbsoluteUri))
                {
                    if (!this._networkService.IsOnline)
                    {
                        await this._dialogService.ShowMessageAsync("No internet access", "When offline, you only can play downloaded videos");
                        return;
                    }
                    parameters.Url = await this._videoDownloaderService.GetDownloadVideoUrlAsync(this.Session.YoutubeID);
                }

                _navigationService.NavigateTo(PageKey.PlayerPage, parameters);
            }
            catch (Exception ex)
            {
                await this._dialogService.ShowMessageAsync("Error", "Is not possible to load the video URL");
            }
        }

        private async Task DownloadVideoAsync()
        {
            try
            {
                if (!this._networkService.IsOnline)
                {
                    await this._dialogService.ShowMessageAsync("No internet access", "A network connection is neccessary to download the video");
                    return;
                }

                await _downloadManager.QueueDownloadAsync(Session);
                VideoDownload = await _downloadManager.GetDownloadForSessionAsync(Session);
            }
            catch (Exception ex)
            {
                // TODO: Log error
                await _dialogService.ShowMessageAsync("Error", "Is not possible download the video");
            }
        }

        private async Task DeleteDownloadAsync()
        {
            if (HasDownload)
            {
                await this._downloadManager.DeleteDownloadAsync(this.VideoDownload);
                this.VideoDownload = null;
            }
        }
    }
}