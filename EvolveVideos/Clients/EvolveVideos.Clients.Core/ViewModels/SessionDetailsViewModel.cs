using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using EvolveVideos.Clients.Core.Models;
using EvolveVideos.Clients.Core.Services;
using GalaSoft.MvvmLight.Command;

namespace EvolveVideos.Clients.Core.ViewModels
{
    public class SessionDetailsViewModel : EvolveBaseViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly INavigationService _navigationService;
        private readonly IVideoDownloaderService _videoDownloaderService;

        private EvolveSession _session;

        private Uri _videoUrl;

        public SessionDetailsViewModel(INavigationService navigationService, IDialogService dialogService,
            IVideoDownloaderService videoDownloaderService)
        {
            this._navigationService = navigationService;
            this._dialogService = dialogService;
            this._videoDownloaderService = videoDownloaderService;

            this.CreateCommands();
        }

        public EvolveSession Session
        {
            get { return this._session; }
            set { this.Set(() => this.Session, ref this._session, value); }
        }

        public Uri VideoUrl
        {
            get { return this._videoUrl; }
            set { this.Set(() => this.VideoUrl, ref this._videoUrl, value); }
        }

        public ICommand PlayCommand { get; private set; }

        public ICommand DownloadCommand { get; private set; }

        private void CreateCommands()
        {
            this.PlayCommand = new RelayCommand(this.PlayVideo);
            this.DownloadCommand = new RelayCommand(async () => { await this.DownloadVideoAsync(); });
        }

        public override async Task OnNavigateTo(object parameter)
        {
            await base.OnNavigateTo(parameter);

            try
            {
                var session = parameter as EvolveSession;
                if (session != null)
                {
                    this.IsBusy = true;
                    this.Session = session;
                    this.VideoUrl = await this._videoDownloaderService.GetStreamVideoUrlAsync(session.YoutubeID);
                }
            }
            catch (Exception ex)
            {
                // TODO: Log error
                await this._dialogService.ShowMessageAsync("Error", "Is not possible load the item");
                this._navigationService.GoBack();
            }
        }

        private void PlayVideo()
        {
            // Play video
        }

        /// <summary>
        /// TODO: Create a DownloadManager
        /// </summary>
        /// <returns></returns>
        private async Task DownloadVideoAsync()
        {
            try
            {
                // Test method
                // Replace it by a background downloader
                var downloadUrl = await this._videoDownloaderService.GetDownloadVideoUrlAsync(this.Session.YoutubeID);
                if (!string.IsNullOrWhiteSpace(downloadUrl.AbsoluteUri))
                {
                    var client = new HttpClient();
                    var data = await client.GetByteArrayAsync(downloadUrl);

                    // Save data[] in localstorage
                    var a = 4;
                }
            }
            catch (Exception ex)
            {
                // TODO: Log error
                await this._dialogService.ShowMessageAsync("Error", "Is not possible download the video");
            }
        }
    }
}