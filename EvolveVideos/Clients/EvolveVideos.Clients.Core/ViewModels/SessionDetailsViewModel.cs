using EvolveVideos.Clients.Core.Models;
using EvolveVideos.Clients.Core.Services;
using EvolveVideos.Clients.Core.Youtube;
using GalaSoft.MvvmLight.Command;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EvolveVideos.Clients.Core.ViewModels
{
    public class SessionDetailsViewModel : EvolveBaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;

        private EvolveSession _session;

        public EvolveSession Session
        {
            get { return this._session; }
            set { this.Set(() => this.Session, ref this._session, value); }
        }

        private Uri _videoUrl;

        public Uri VideoUrl
        {
            get { return this._videoUrl; }
            set { this.Set(() => this.VideoUrl, ref this._videoUrl, value); }
        }

        public ICommand PlayCommand
        {
            get;
            private set;
        }

        public SessionDetailsViewModel(INavigationService navigationService, IDialogService dialogService)
        {
            this._navigationService = navigationService;
            this._dialogService = dialogService;

            this.CreateCommands();
        }

        private void CreateCommands()
        {
            this.PlayCommand = new RelayCommand(this.PlayVideo);
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
                    this.VideoUrl = (await YouTube.GetVideoUriAsync(session.YoutubeID, YouTubeQuality.Quality720P)).Uri;
                }
            }
            catch (Exception ex)
            {
                // Log error
                await this._dialogService.ShowMessageAsync("Error", "Is not possible load the item");
                this._navigationService.GoBack();
            }
        }

        private void PlayVideo()
        {
            // Play video
        }
    }
}