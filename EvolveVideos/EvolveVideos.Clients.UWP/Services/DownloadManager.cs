using EvolveVideos.Clients.Core.Services;
using EvolveVideos.Clients.Core.Services.Download;
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
        private readonly IVideoDownloaderService _downloaderService;
        private readonly AsyncLock _fileMutex = new AsyncLock();

        private readonly AsyncLock _networkMutex = new AsyncLock();
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

            InitilizeAsync();
        }

        public IList<IVideoDownload> Downloads { get; private set; }

        public async Task PauseAllDownloadsAsync()
        {
            foreach (var videoDowload in Downloads)
            {
                await videoDowload.PauseAsync();
            }

            using (await _fileMutex.LockAsync())
            {
                await _storageService.SaveDownloadsAsync(Downloads);
            }
        }

        public async Task ResumeAllDownloadsAsync()
        {
            var MaxConcurrentDownloads = 3;

            // Check Downlading
            var downloading = Downloads.Where(x => x.Status == DownloadStatus.Downloading).ToList();

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

            using (await _fileMutex.LockAsync())
            {
                await _storageService.SaveDownloadsAsync(Downloads);
            }
        }

        public async Task DeleteAllDownloadsAsync()
        {
            foreach (var videoDowload in Downloads)
            {
                await videoDowload.DeleteAsync();
            }

            Downloads.Clear();
            using (await _fileMutex.LockAsync())
            {
                await _storageService.SaveDownloadsAsync(Downloads);
            }
        }

        public async Task QueueDownloadAsync(EvolveSession session)
        {
            var url = await _downloaderService.GetDownloadVideoUrlAsync(session.YoutubeID);

            var newDownload = this._videoDownloaderFactory.Create();

            newDownload.Id = Guid.NewGuid();
            newDownload.SessionId = session.Id;
            newDownload.DownloadUrl = url;
            newDownload.Status = DownloadStatus.Queue;

            await newDownload.StartDownlodAsync();

            Downloads.Add(newDownload);

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
                using (await _fileMutex.LockAsync())
                {
                    await _storageService.SaveDownloadsAsync(Downloads);
                }
            }
        }

        public async Task PauseDownload(IVideoDownload download)
        {
            var selected = Downloads.FirstOrDefault(x => x.Id == download.Id);
            if (selected != null)
            {
                await download.PauseAsync();
                using (await _fileMutex.LockAsync())
                {
                    await _storageService.SaveDownloadsAsync(Downloads);
                }
            }
        }

        public async Task ResumeDownload(IVideoDownload download)
        {
            var selected = Downloads.FirstOrDefault(x => x.Id == download.Id);
            if (selected != null)
            {
                await download.ResumeAsync();
                using (await _fileMutex.LockAsync())
                {
                    await _storageService.SaveDownloadsAsync(Downloads);
                }
            }
        }

        public Task<IVideoDownload> GetDownloadForSessionAsync(EvolveSession session)
        {
            var download = Downloads.FirstOrDefault(x => x.SessionId == session.Id);
            return Task.FromResult(download);
        }

        public async Task InitilizeAsync()
        {
            await LoadDownloadQueueAsync();

            var autoResume = await _settingsService.LoadSettingAsync<bool>(SettingsKeys.DownloaderAutoResume);
            if (autoResume)
            {
                await ResumeAllDownloadsAsync();
            }
        }

        private async Task LoadDownloadQueueAsync()
        {
            var localDownloads = await _storageService.LoadDownloadsAsync() ?? new List<IVideoDownload>();
            Downloads = localDownloads;
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