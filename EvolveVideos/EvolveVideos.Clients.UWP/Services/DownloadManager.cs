using System.Threading.Tasks;
using EvolveVideos.Clients.Core.Services;
using EvolveVideos.Data.Models;
using Nito.AsyncEx;

namespace EvolveVideos.Clients.UWP.Services
{
    public class DownloadManager : IDownloadManager
    {
        private readonly IVideoDownloaderService _downloaderService;

        private readonly AsyncLock _mutex = new AsyncLock();
        private readonly INetworkService _networkService;
        private readonly IStorageService _storageService;

        public DownloadManager(
            INetworkService networkService,
            IStorageService storageService,
            IVideoDownloaderService downloaderService)
        {
            _networkService = networkService;
            _storageService = storageService;
            _downloaderService = downloaderService;

            _networkService.NetworkStatusChanged += OnNetworkStatusChanged;
        }

        public Task PauseAllDownloadsAsync()
        {
            return Task.CompletedTask;
        }

        public Task ResumeAllDownloadsAsync()
        {
            return Task.CompletedTask;
        }

        public async Task DeleteAllDownloadsAsync()
        {
            throw new System.NotImplementedException();
        }

        public async Task QueueDownloadAsync(EvolveSession session)
        {
            var url = this._downloaderService.GetDownloadVideoUrlAsync(session.YoutubeID);
        }

        private async void OnNetworkStatusChanged(object sender, NetworkStatusChangedEvents e)
        {
            using (await _mutex.LockAsync())
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