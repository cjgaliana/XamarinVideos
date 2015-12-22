using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using EvolveVideos.Clients.Core.Services;
using EvolveVideos.Clients.UWP.Helpers;
using EvolveVideos.Clients.UWP.Views;

namespace EvolveVideos.Clients.UWP.Services
{
    public class NavigationService : INavigationService
    {
        private readonly Dictionary<PageKey, Type> _pages;

        public NavigationService()
        {
            this._pages = new Dictionary<PageKey, Type>
            {
                {PageKey.MainPage, typeof (MainPage)},
                {PageKey.SessionDetailsPage, typeof (SessionDetailsView)}
            };

            this.CurrentFrame = (Frame) Window.Current.Content;
        }

        private Frame CurrentFrame { get; }

        public void GoBack()
        {
            if (this.CanGoBack)
            {
                this.CurrentFrame.GoBack();
            }
        }

        public bool CanGoBack => this.CurrentFrame.CanGoBack;

        public void NavigateTo(PageKey page)
        {
            this.NavigateTo(page, null);
        }

        public void NavigateTo(PageKey page, object parameters)
        {
            if (!this._pages.ContainsKey(page))
            {
                return;
            }

            var pageType = this._pages[page];
            this.NavigateToPage(pageType, parameters);
            TitleBarHelper.ShowBackButton();
        }

        public void ClearNavigationStack()
        {
            try
            {
                this.CurrentFrame.SetNavigationState("1,0");
                TitleBarHelper.HideBackButton();
            }
            catch (Exception ex)
            {
                // Catch the error
            }
        }

        private void NavigateToPage(Type page, object parameter = null)
        {
            this.CurrentFrame.Navigate(page, parameter);
        }
    }
}