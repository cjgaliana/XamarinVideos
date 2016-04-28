using EvolveVideos.Clients.Core.Services;
using EvolveVideos.Clients.Core.ViewModels;
using EvolveVideos.Clients.UWP.Views;
using System;
using System.Collections.Generic;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace EvolveVideos.Clients.UWP.Services
{
    public class NavigationService : INavigationService
    {
        private readonly Dictionary<PageKey, Type> _pages;

        private readonly SystemNavigationManager _systemNavManager;

        private Frame _currentFrame;

        public NavigationService()
        {
            _pages = new Dictionary<PageKey, Type>
            {
                {PageKey.SplashScreenPage, typeof(ExtendedSplashScreen)},
                {PageKey.MainPage, typeof(MainPage)},
                {PageKey.SessionDetailsPage, typeof(SessionDetailsView)},
                {PageKey.VideoCollectionsPage, typeof(VideoCollectionsPage)},
                {PageKey.VideoCollectionDetailsPage, typeof(VideoCollectionDetailsPage)}
            };

            SetFrame((Frame)Window.Current.Content);

            _systemNavManager = SystemNavigationManager.GetForCurrentView();
            _systemNavManager.BackRequested += SystemNavManager_BackRequested;
        }

        public void GoBack()
        {
            if (CanGoBack)
            {
                _currentFrame.GoBack();
                UpdateBackButtonVisibility();
            }
        }

        public bool CanGoBack => _currentFrame.CanGoBack;

        public void NavigateTo(PageKey page)
        {
            NavigateTo(page, null);
        }

        public void NavigateTo(PageKey page, object parameters)
        {
            if (!_pages.ContainsKey(page))
            {
                return;
            }

            var pageType = _pages[page];
            NavigateToPage(pageType, parameters);
            UpdateBackButtonVisibility();
        }

        public void ClearNavigationStack()
        {
            try
            {
                //_currentFrame.SetNavigationState("1,0");
                _currentFrame?.BackStack.Clear();
                UpdateBackButtonVisibility();
            }
            catch (Exception ex)
            {
                // Catch the error
            }
        }

        public void SetFrame(Frame newFrame)
        {
            _currentFrame = newFrame;
            if (_currentFrame != null)
            {
                Window.Current.Content = _currentFrame;
                _currentFrame.Navigated += OnNavigatedTo;
                _currentFrame.Navigating += OnNavigatedFrom;
            }
        }

        private async void OnNavigatedTo(object sender, NavigationEventArgs e)
        {
            var page = e.Content as Page;
            var viewModel = page?.DataContext as BaseViewModel;
            if (viewModel != null)
            {
                await viewModel.OnNavigateTo(e.Parameter);
            }
        }

        private async void OnNavigatedFrom(object sender, NavigatingCancelEventArgs e)
        {
            var frame = sender as Frame;
            if (frame != null)
            {
                var page = frame.Content as Page;
                var viewModel = page?.DataContext as BaseViewModel;
                if (viewModel != null)
                {
                    await viewModel.OnNavigateFrom(e.Parameter);
                }
            }
        }

        private void SystemNavManager_BackRequested(object sender, BackRequestedEventArgs e)
        {
            GoBack();
            e.Handled = true;
        }

        private void NavigateToPage(Type page, object parameter = null)
        {
            _currentFrame.Navigate(page, parameter);
        }

        private void UpdateBackButtonVisibility()
        {
            if (_systemNavManager != null)
            {
                _systemNavManager.AppViewBackButtonVisibility = _currentFrame.BackStackDepth > 0
                    ? AppViewBackButtonVisibility.Visible
                    : AppViewBackButtonVisibility.Collapsed;
            }
        }
    }
}