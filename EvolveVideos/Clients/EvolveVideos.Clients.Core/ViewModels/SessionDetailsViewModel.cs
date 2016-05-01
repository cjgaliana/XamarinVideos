using EvolveVideos.Clients.Core.Services;
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
        private readonly INavigationService _navigationService;

        private EvolveSession _session;

        private Uri _videoUrl;

        public SessionDetailsViewModel(INavigationService navigationService, IDialogService dialogService, IDownloadManager downloadManager)
        {
            this._navigationService = navigationService;
            this._dialogService = dialogService;
            _downloadManager = downloadManager;

            this.CreateCommands();

            if (this.IsInDesignMode)
            {
                this.Session = DesignData.GetSession();
            }
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


        private async Task DownloadVideoAsync()
        {
            try
            {
                await this._downloadManager.QueueDownloadAsync(this.Session);
            }
            catch (Exception ex)
            {
                // TODO: Log error
                await this._dialogService.ShowMessageAsync("Error", "Is not possible download the video");
            }
        }
    }
}