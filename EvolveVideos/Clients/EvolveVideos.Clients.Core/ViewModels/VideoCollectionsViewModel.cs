using System.Collections.ObjectModel;
using System.Threading.Tasks;
using EvolveVideos.Clients.Core.Models;
using EvolveVideos.Clients.Core.Services;

namespace EvolveVideos.Clients.Core.ViewModels
{
    public class VideoCollectionsViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IStorageService _storageService;

        private ObservableCollection<VideoCollection> _collections;

        public VideoCollectionsViewModel(INavigationService navigationService, IStorageService storageService)
        {
            _navigationService = navigationService;
            _storageService = storageService;
        }

        public ObservableCollection<VideoCollection> Collections
        {
            get { return _collections; }
            set { Set(() => Collections, ref _collections, value); }
        }

        public override async Task OnNavigateTo(object parameter)
        {
            await base.OnNavigateTo(parameter);

            Collections = new ObservableCollection<VideoCollection>
            {
                new VideoCollection
                {
                    Name = "Evolve 2015",
                    Description = "Evolve 2015 Videos",
                    Videos = await _storageService.LoadSessions()
                },
                new VideoCollection
                {
                    Name = "Evolve 2016",
                    Description = "Evolve 2016 Videos",
                    Videos = await _storageService.LoadSessions()
                }
            };
        }
    }
}