using Cimbalino.Toolkit.Controls;
using EvolveVideos.Clients.Core.Services;
using EvolveVideos.Clients.UWP.Controls;
using EvolveVideos.Clients.UWP.Services;
using Microsoft.Practices.ServiceLocation;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace EvolveVideos.Clients.UWP.Views
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Shell : Page
    {
        public Shell()
        {
            InitializeComponent();

            Loaded += (sender, args) =>
            {
                // Initialize Navigation Service
                var navService = ServiceLocator.Current.GetInstance<INavigationService>() as NavigationService;
                if (navService != null)
                {
                    // Create the Hamburger menu
                    var hamburguerFrame = new HamburgerFrame
                    {
                        Header = new HamburgerTitleBar
                        {
                            Title = "Evolve Videos"
                        },
                        Pane = new HamburgerPaneControl()
                    };

                    // Set the new hamburguer menu as a new Frame
                    navService.SetFrame(hamburguerFrame);

                    // And go to main page
                    navService.NavigateTo(PageKey.MainPage);
                }
            };
        }
    }
}