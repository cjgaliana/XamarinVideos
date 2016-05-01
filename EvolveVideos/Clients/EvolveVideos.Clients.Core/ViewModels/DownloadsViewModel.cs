﻿using EvolveVideos.Clients.Core.Services;

namespace EvolveVideos.Clients.Core.ViewModels
{
    public class DownloadsViewModel : BaseViewModel
    {
        private readonly IDownloadManager _downloadManager;
        private readonly INavigationService _navigationService;

        public DownloadsViewModel(INavigationService navigationService, IDownloadManager downloadManager)
        {
            this._navigationService = navigationService;
            this._downloadManager = downloadManager;
        }
    }
}