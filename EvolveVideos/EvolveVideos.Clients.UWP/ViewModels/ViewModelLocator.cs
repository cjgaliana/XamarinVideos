using EvolveVideos.Clients.Core.Services;
using EvolveVideos.Clients.Core.ViewModels;
using EvolveVideos.Clients.UWP.Services;
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
            this._container = new UnityContainer();

            this.RegisterServices();
            this.RegisterViewModels();
        }

        public MainViewModel MainViewModel => this._container.Resolve<MainViewModel>();

        public SessionDetailsViewModel SessionDetailsViewModel => this._container.Resolve<SessionDetailsViewModel>();

        private void RegisterServices()
        {
            this._container.RegisterType<IDialogService, DialogService>()
                .RegisterType<INavigationService, NavigationService>()
                .RegisterType<INetworkService, NetworkService>()
                .RegisterType<ILauncherService, LauncherService>()
                .RegisterType<IStorageService, StorageService>();
        }

        private void RegisterViewModels()
        {
            this._container.RegisterType<MainViewModel>()
                .RegisterType<SessionDetailsViewModel>();
        }
    }
}