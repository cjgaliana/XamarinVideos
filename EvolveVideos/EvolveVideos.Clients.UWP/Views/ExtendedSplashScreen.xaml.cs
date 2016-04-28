using EvolveVideos.Clients.Core.ViewModels;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace EvolveVideos.Clients.UWP.Views
{
    public sealed partial class ExtendedSplashScreen : Page
    {
        private readonly Frame _rootFrame;
        private readonly SplashScreen _splash; // Variable to hold the splash screen object.
        private bool _dismissed; // Variable to track splash screen dismissal status.

        private Rect _splashImageRect; // Rect to store splash screen image coordinates.
        private SplashScreenViewModel _viewModel;

        public ExtendedSplashScreen(SplashScreen splashscreen)
        {
            InitializeComponent();

            // Listen for window resize events to reposition the extended splash screen image accordingly.
            // This ensures that the extended splash screen formats properly in response to window resizing.
            Window.Current.SizeChanged += ExtendedSplash_OnResize;

            _splash = splashscreen;
            if (_splash != null)
            {
                // Register an event handler to be executed when the splash screen has been dismissed.
                _splash.Dismissed += DismissedEventHandler;

                // Retrieve the window coordinates of the splash screen image.
                _splashImageRect = _splash.ImageLocation;
                PositionImage();

                // If applicable, include a method for positioning a progress control.
                PositionRing();
            }

            // Create a Frame to act as the navigation context
            _rootFrame = new Frame();

            this.Loaded += async (sender, args) => { await this.AppBootstrapper(); };
        }

        private async Task AppBootstrapper()
        {
            this._viewModel = this.DataContext as SplashScreenViewModel;
            if (this._viewModel != null)
            {
                await this._viewModel.InitializeAsync();
                this.DismissExtendedSplash();
            }
        }

        private void PositionImage()
        {
            extendedSplashImage.SetValue(Canvas.LeftProperty, _splashImageRect.X);
            extendedSplashImage.SetValue(Canvas.TopProperty, _splashImageRect.Y);
            extendedSplashImage.Height = _splashImageRect.Height;
            extendedSplashImage.Width = _splashImageRect.Width;
        }

        private void PositionRing()
        {
            splashProgressRing.SetValue(Canvas.LeftProperty,
                _splashImageRect.X + _splashImageRect.Width * 0.5 - splashProgressRing.Width * 0.5);
            splashProgressRing.SetValue(Canvas.TopProperty,
                _splashImageRect.Y + _splashImageRect.Height + _splashImageRect.Height * 0.1);
        }

        // Include code to be executed when the system has transitioned from the splash screen to the extended splash screen (application's first view).
        private void DismissedEventHandler(SplashScreen sender, object e)
        {
            _dismissed = true;

            // Complete app setup operations here...
        }

        private void DismissExtendedSplash()
        {
            // Navigate to mainpage
            _rootFrame.Navigate(typeof(Shell));
            // Place the frame in the current Window
            Window.Current.Content = _rootFrame;
        }

        private void ExtendedSplash_OnResize(object sender, WindowSizeChangedEventArgs e)
        {
            // Safely update the extended splash screen image coordinates. This function will be executed when a user resizes the window.
            if (_splash != null)
            {
                // Update the coordinates of the splash screen image.
                _splashImageRect = _splash.ImageLocation;
                PositionImage();

                // If applicable, include a method for positioning a progress control.
                PositionRing();
            }
        }
    }
}