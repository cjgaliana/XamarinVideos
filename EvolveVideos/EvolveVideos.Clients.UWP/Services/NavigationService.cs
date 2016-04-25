using System;
using System.Collections.Generic;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Cimbalino.Toolkit.Controls;
using EvolveVideos.Clients.Core.Services;
using EvolveVideos.Clients.UWP.Views;

namespace EvolveVideos.Clients.UWP.Services
{
    public class NavigationService : INavigationService
    {
        private readonly Dictionary<PageKey, Type> _pages;

        private readonly SystemNavigationManager _systemNavManager;

        public NavigationService()
        {
            _pages = new Dictionary<PageKey, Type>
            {
                {PageKey.MainPage, typeof(MainPage)},
                {PageKey.SessionDetailsPage, typeof(SessionDetailsView)},
                {PageKey.SplashScreenPage, typeof(SplashScreenPage)},
                {PageKey.VideoCollectionsPage, typeof(VideoCollectionsPage)},
                {PageKey.VideoCollectionDetailsPage, typeof(VideoCollectionDetailsPage)}
            };

            CurrentFrame = (HamburgerFrame) Window.Current.Content;

            _systemNavManager = SystemNavigationManager.GetForCurrentView();
            _systemNavManager.BackRequested += SystemNavManager_BackRequested;
        }

        private Frame CurrentFrame { get; }

        public void GoBack()
        {
            if (CanGoBack)
            {
                CurrentFrame.GoBack();
                UpdateBackButtonVisibility();
            }
        }

        public bool CanGoBack => CurrentFrame.CanGoBack;

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
                CurrentFrame.SetNavigationState("1,0");
                UpdateBackButtonVisibility();
            }
            catch (Exception ex)
            {
                // Catch the error
            }
        }

        private void SystemNavManager_BackRequested(object sender, BackRequestedEventArgs e)
        {
            GoBack();
            e.Handled = true;
        }

        private void NavigateToPage(Type page, object parameter = null)
        {
            CurrentFrame.Navigate(page, parameter);
        }

        private void UpdateBackButtonVisibility()
        {
            if (_systemNavManager != null)
            {
                _systemNavManager.AppViewBackButtonVisibility = CurrentFrame.BackStackDepth > 0 
                    ? AppViewBackButtonVisibility.Visible 
                    : AppViewBackButtonVisibility.Collapsed;
            }
        }
    }
}