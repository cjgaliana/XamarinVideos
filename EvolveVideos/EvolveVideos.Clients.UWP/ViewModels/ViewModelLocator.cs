using EvolveVideos.Clients.Services;
using EvolveVideos.Clients.Services.Download;
using EvolveVideos.Clients.UWP.Services;
using EvolveVideos.Clients.ViewModels;
using EvolveVideos.Data;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

namespace EvolveVideos.Clients.UWP.ViewModels
{
    /// <summary>
    ///     This class contains static references to all the view models in the
    ///     application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        private readonly UnityContainer _container;

        /// <summary>
        ///     Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            _container = new UnityContainer();

            ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(_container));

            RegisterServices();
            RegisterViewModels();
        }

        public SettingsViewModel SettingsViewModel => _container.Resolve<SettingsViewModel>();
        public DownloadsViewModel DownloadsViewModel => _container.Resolve<DownloadsViewModel>();

        public MainViewModel MainViewModel => _container.Resolve<MainViewModel>();

        public SplashScreenViewModel SplashScreenViewModel => _container.Resolve<SplashScreenViewModel>();

        public SessionDetailsViewModel SessionDetailsViewModel => _container.Resolve<SessionDetailsViewModel>();

        public PlayerViewModel PlayerViewModel => _container.Resolve<PlayerViewModel>();

        public VideoCollectionsViewModel VideoCollectionsViewModel
            => _container.Resolve<VideoCollectionsViewModel>();

        public VideoCollectionDetailsViewModel VideoCollectionDetailsViewModel
            => _container.Resolve<VideoCollectionDetailsViewModel>();

        private void RegisterServices()
        {
            _container
                .RegisterType<ILogService, HockeyAppLogService>()
                .RegisterType<IDialogService, DialogService>()
                .RegisterType<INavigationService, NavigationService>(new ContainerControlledLifetimeManager())
                .RegisterType<INetworkService, NetworkService>(new ContainerControlledLifetimeManager())
                .RegisterType<ISettingsService, RoamingSettingsService>()
                .RegisterType<ILauncherService, LauncherService>()
                .RegisterType<IStorageService, StorageService>()
                .RegisterType<IDataService, LocalResourcesDataService>()
                .RegisterType<IVideoDownloaderService, YoutubeDownloaderService>()
                .RegisterType<IVideoDownloaderFactory, UWPVideoDownloaderFactory>()
                .RegisterType<IDownloadManager, DownloadManager>(new ContainerControlledLifetimeManager());
        }

        private void RegisterViewModels()
        {
            _container
                .RegisterType<SplashScreenViewModel>()
                .RegisterType<SettingsViewModel>()
                .RegisterType<MainViewModel>()
                .RegisterType<SessionDetailsViewModel>()
                .RegisterType<VideoCollectionsViewModel>()
                .RegisterType<VideoCollectionDetailsViewModel>()
                .RegisterType<DownloadsViewModel>()
                .RegisterType<PlayerViewModel>();
        }
    }
}