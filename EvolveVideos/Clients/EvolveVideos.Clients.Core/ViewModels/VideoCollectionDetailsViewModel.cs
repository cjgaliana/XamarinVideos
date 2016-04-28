using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using EvolveVideos.Clients.Core.Services;
using EvolveVideos.Data;
using EvolveVideos.Data.Models;
using GalaSoft.MvvmLight.Command;

namespace EvolveVideos.Clients.Core.ViewModels
{
    public class VideoCollectionDetailsViewModel : BaseViewModel
    {
        private readonly IDataService _dataService;
        private readonly IDialogService _dialogService;
        private readonly INavigationService _navigationService;
        private readonly IStorageService _storageService;
        private IList<EvolveSession> _sessions;

        private VideoCollection _videoCollection;

        public VideoCollectionDetailsViewModel(INavigationService navigationService, IStorageService storageService,
            IDialogService dialogService, IDataService dataService)
        {
            _navigationService = navigationService;
            _storageService = storageService;
            _dialogService = dialogService;
            _dataService = dataService;

            CreateCommands();
        }

        public ICommand NavigateToSessionCommand { get; private set; }

        public VideoCollection VideoCollection
        {
            get { return _videoCollection; }
            set { Set(() => VideoCollection, ref _videoCollection, value); }
        }

        public IList<EvolveSession> Sessions
        {
            get { return _sessions; }
            set
            {
                Set(() => Sessions, ref _sessions, value);
                RaisePropertyChanged(() => TotalVideos);
            }
        }

        public int TotalVideos => Sessions.Count;

        private void CreateCommands()
        {
            NavigateToSessionCommand = new RelayCommand<EvolveSession>(NavigateToSessionDetails);
        }

        private void NavigateToSessionDetails(EvolveSession session)
        {
            _navigationService.NavigateTo(PageKey.SessionDetailsPage, session);
        }

        public override async Task OnNavigateTo(object parameter)
        {
            await base.OnNavigateTo(parameter);

            var collection = parameter as VideoCollection;
            if (collection == null)
            {
                // Show error
                await _dialogService.ShowMessageAsync("Incorrect parameter", "Is not possible to open this page");
                _navigationService.GoBack();
                return;
            }
            try
            {
                IsBusy = true;
                VideoCollection = collection;
                var videos = await _dataService.GetVideosAsync(VideoCollection);
                Sessions = videos;
            }
            catch (Exception ex)
            {
                // TODO: Handle exception
                throw;
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}