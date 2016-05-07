using EvolveVideos.Clients.Core.Models;
using EvolveVideos.Clients.Core.Utils;
using EvolveVideos.Clients.Services;
using EvolveVideos.Clients.Services.Download;
using EvolveVideos.Data.Models;
using Nito.AsyncEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvolveVideos.Clients.UWP.Services
{
    public class DownloadManager : IDownloadManager
    {
        private readonly AsyncLock _fileMutex = new AsyncLock();
        private readonly AsyncLock _networkMutex = new AsyncLock();

        private readonly IVideoDownloaderService _downloaderService;
        private readonly INetworkService _networkService;
        private readonly ISettingsService _settingsService;
        private readonly IVideoDownloaderFactory _videoDownloaderFactory;
        private readonly IStorageService _storageService;

        public DownloadManager(
            INetworkService networkService,
            IStorageService storageService,
            IVideoDownloaderService downloaderService,
            ISettingsService settingsService,
            IVideoDownloaderFactory videoDownloaderFactory)
        {
            _networkService = networkService;
            _storageService = storageService;
            _downloaderService = downloaderService;
            _settingsService = settingsService;
            _videoDownloaderFactory = videoDownloaderFactory;

            _networkService.NetworkStatusChanged += OnNetworkStatusChanged;
        }

        public IList<IVideoDownload> Downloads { get; private set; }

        public async Task PauseAllDownloadsAsync()
        {
            foreach (var videoDowload in Downloads)
            {
                await videoDowload.PauseAsync();
            }

            await SaveDownloasdToStorageAsync();
        }

        public async Task ResumeAllDownloadsAsync()
        {
            if (Downloads == null || !Downloads.Any())
            {
                return;
            }

            var MaxConcurrentDownloads = 3;

            // Check Downlading
            var downloading = Downloads.Where(x => x.Status == DownloadStatus.Downloading).ToList();
            foreach (var videoDownload in downloading)
            {
                // Ensure is downloading
               await videoDownload.ResumeAsync();
            }

            var availableSlots = MaxConcurrentDownloads - downloading.Count;
            if (availableSlots > 0)
            {
                // Resume paused
                var paused = Downloads.Where(x => x.Status == DownloadStatus.Paused).ToList();
                foreach (var download in paused)
                {
                    if (availableSlots > 0)
                    {
                        await download.ResumeAsync();
                        availableSlots--;
                    }
                    else
                    {
                        break;
                    }
                }

                // Resume queued
                var queued = Downloads.Where(x => x.Status == DownloadStatus.Queue).ToList();
                foreach (var download in queued)
                {
                    if (availableSlots > 0)
                    {
                        await download.ResumeAsync();
                        availableSlots--;
                    }
                    else
                    {
                        break;
                    }
                }

                // Resume Error
                var errors = Downloads.Where(x => x.Status == DownloadStatus.Error).ToList();
                foreach (var download in errors)
                {
                    if (availableSlots > 0)
                    {
                        await download.ResumeAsync();
                        availableSlots--;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            await SaveDownloasdToStorageAsync();
        }

        public async Task DeleteAllDownloadsAsync()
        {
            foreach (var videoDowload in Downloads)
            {
                await videoDowload.DeleteAsync();
            }

            Downloads.Clear();
            await SaveDownloasdToStorageAsync();
        }

        public async Task QueueDownloadAsync(EvolveSession session)
        {
            var url = await _downloaderService.GetDownloadVideoUrlAsync(session.YoutubeID);

            var newDownload = this._videoDownloaderFactory.Create();

            newDownload.SessionId = session.Id;
            newDownload.DownloadUrl = url;
            newDownload.Status = DownloadStatus.Queue;
            newDownload.DownloadCompleted += OnDownloadCompleted;
            newDownload.DownloadStatusChanged += OnStatusChanged;
            newDownload.DownloadProgressChanged += OnProgressChanged;

            newDownload.StartDownlodAsync().FireAndForget();

            Downloads.Add(newDownload);

            await SaveDownloasdToStorageAsync();
        }

        private async void OnProgressChanged(object sender, DownloadProgressChangedArgs e)
        {
            await this.UpdateDownloadsAsync(e.Download);
        }

        private async void OnStatusChanged(object sender, DownloadStatusChangedArgs e)
        {
            await this.UpdateDownloadsAsync(e.Download);
        }

        private async void OnDownloadCompleted(object sender, DownloadCompetedArgs e)
        {
            await this.UpdateDownloadsAsync(e.Download);
        }

        private async Task UpdateDownloadsAsync(IVideoDownload download)
        {
            var localDownload = this.Downloads.FirstOrDefault(x => x.Id == download.Id);
            if (localDownload != null)
            {
                ////localDownload.Status = download.Status;
                //localDownload.LocalFileUrl = download.LocalFileUrl;
                ////localDownload.Percentage = download.Percentage;
                //localDownload.SessionId = download.SessionId;

                await SaveDownloasdToStorageAsync();
            }
        }

        private async Task SaveDownloasdToStorageAsync()
        {
            using (await _fileMutex.LockAsync())
            {
                await _storageService.SaveDownloadsAsync(Downloads);
            }
        }

        public async Task DeleteDownloadAsync(IVideoDownload download)
        {
            var selected = Downloads.FirstOrDefault(x => x.Id == download.Id);
            if (selected != null)
            {
                await selected.DeleteAsync();
                Downloads.Remove(selected);
                await this.SaveDownloasdToStorageAsync();
            }
        }

        public async Task PauseDownload(IVideoDownload download)
        {
            var selected = Downloads.FirstOrDefault(x => x.Id == download.Id);
            if (selected != null)
            {
                await download.PauseAsync();
                await this.SaveDownloasdToStorageAsync();
            }
        }

        public async Task ResumeDownload(IVideoDownload download)
        {
            var selected = Downloads.FirstOrDefault(x => x.Id == download.Id);
            if (selected != null)
            {
                await download.ResumeAsync();
                await this.SaveDownloasdToStorageAsync();
            }
        }

        public Task<IVideoDownload> GetDownloadForSessionAsync(EvolveSession session)
        {
            var download = Downloads.FirstOrDefault(x => x.SessionId == session.Id);
            return Task.FromResult(download);
        }

        public async Task InitializeAsync()
        {
            try
            {
                await LoadDownloadQueueAsync();

                var autoResume = await _settingsService.LoadSettingAsync<bool>(SettingsKeys.DownloaderAutoResume, true);
                if (autoResume)
                {
                    await ResumeAllDownloadsAsync();
                }
            }
            catch (Exception ex)
            {
                var a = 5;
            }
        }

        private async Task LoadDownloadQueueAsync()
        {
            var localDownloads = await _storageService.LoadDownloadsAsync() ?? new List<IVideoDownload>();
            Downloads = localDownloads;

            foreach (var download in Downloads)
            {
                download.DownloadCompleted += OnDownloadCompleted;
                download.DownloadProgressChanged += OnProgressChanged;
                download.DownloadStatusChanged += OnStatusChanged;
            }
        }

        private async void OnNetworkStatusChanged(object sender, NetworkStatusChangedEvents e)
        {
            using (await _networkMutex.LockAsync())
            {
                // It's safe to await while the lock is held
                if (e.IsOnline && e.NetworkType == NetworkType.Wifi)
                {
                    await ResumeAllDownloadsAsync();
                }
                else
                {
                    await PauseAllDownloadsAsync();
                }
            }
        }
    }
}