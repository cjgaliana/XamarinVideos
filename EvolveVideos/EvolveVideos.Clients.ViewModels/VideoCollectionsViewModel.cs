using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using EvolveVideos.Clients.Services;
using EvolveVideos.Data;
using EvolveVideos.Data.Models;
using GalaSoft.MvvmLight.Command;

namespace EvolveVideos.Clients.ViewModels
{
    public class VideoCollectionsViewModel : BaseViewModel
    {
        private readonly IDataService _dataService;
        private readonly INavigationService _navigationService;
        private readonly IStorageService _storageService;

        private List<VideoCollection> _collections;

        public VideoCollectionsViewModel(
            INavigationService navigationService, IStorageService storageService,
            IDataService dataService)
        {
            _navigationService = navigationService;
            _storageService = storageService;
            _dataService = dataService;

            CreateCommands();
        }

        public List<VideoCollection> Collections
        {
            get { return _collections; }
            set { Set(() => Collections, ref _collections, value); }
        }

        public ICommand NavigateToCollectionCommand { get; private set; }

        private void CreateCommands()
        {
            NavigateToCollectionCommand = new RelayCommand<VideoCollection>(NavigateToSession);
        }

        public override async Task OnNavigateTo(object parameter)
        {
            await base.OnNavigateTo(parameter);

            Collections = await _dataService.GetVideoCollectionsAsync();
        }

        private void NavigateToSession(VideoCollection collection)
        {
            _navigationService.NavigateTo(PageKey.VideoCollectionDetailsPage, collection);
        }
    }
}