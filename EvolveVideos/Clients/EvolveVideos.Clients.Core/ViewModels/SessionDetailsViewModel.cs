using EvolveVideos.Clients.Core.Models;
using EvolveVideos.Clients.Core.Services;
using EvolveVideos.Clients.Core.Services.Download;
using EvolveVideos.Clients.UWP.DesignData;
using EvolveVideos.Data.Models;
using GalaSoft.MvvmLight.Command;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EvolveVideos.Clients.Core.ViewModels
{
    public class SessionDetailsViewModel : BaseViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly IDownloadManager _downloadManager;
        private readonly IVideoDownloaderService _videoDownloaderService;
        private readonly INavigationService _navigationService;
        private bool _hasDownload;

        private EvolveSession _session;

        private IVideoDownload _videoDownload;

        private Uri _videoUrl;

        public SessionDetailsViewModel(INavigationService navigationService, IDialogService dialogService,
            IDownloadManager downloadManager, IVideoDownloaderService videoDownloaderService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _downloadManager = downloadManager;
            _videoDownloaderService = videoDownloaderService;

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
            }
        }

        public bool HasDownload => VideoDownload != null;

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
            var parameters = new PlayerParameters
            {
                Title = Session.Title,
                Url = await this._videoDownloaderService.GetDownloadVideoUrlAsync(this.Session.YoutubeID)
            };

            // Play video
            if (HasDownload)
            {
                if (VideoDownload.Status == DownloadStatus.Completed)
                {
                    parameters.Url = VideoDownload.LocalFileUrl;
                }
            }

            _navigationService.NavigateTo(PageKey.PlayerPage, parameters);
        }

        private async Task DownloadVideoAsync()
        {
            try
            {
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