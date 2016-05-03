﻿using EvolveVideos.Clients.Core.Services.Download;
using EvolveVideos.Clients.Core.ViewModels;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;

namespace EvolveVideos.Clients.UWP.Services
{
    public class UWPBackgroundDowloader : BaseViewModel, IVideoDownload
    {
        private readonly string VideosFolderName = "Videos";

        private CancellationTokenSource _cts;
        private DownloadOperation _download;
        private Uri _downloadUrl;
        private Guid _id;
        private Uri _localFileUrl;
        private double _percentage;
        private StorageFolder _rootFolder;
        private Guid _sessionId;

        private DownloadStatus _status;

        public UWPBackgroundDowloader()
        {
            _cts = new CancellationTokenSource();
            CreateRootFolderAsync().Wait(TimeSpan.FromSeconds(1));
        }

        public double Percentage
        {
            get { return _percentage; }
            set { Set(() => Percentage, ref _percentage, value); }
        }

        public DownloadStatus Status
        {
            get { return _status; }
            set { Set(() => Status, ref _status, value); }
        }

        public Guid Id
        {
            get { return _id; }
            set { Set(() => Id, ref _id, value); }
        }

        public Uri DownloadUrl
        {
            get { return _downloadUrl; }
            set { Set(() => DownloadUrl, ref _downloadUrl, value); }
        }

        public Guid SessionId
        {
            get { return _sessionId; }
            set { Set(() => SessionId, ref _sessionId, value); }
        }

        public Uri LocalFileUrl
        {
            get { return _localFileUrl; }
            set { Set(() => LocalFileUrl, ref _localFileUrl, value); }
        }

        public async Task StartDownlodAsync()
        {
            try
            {
                _cts = new CancellationTokenSource();

                var source = DownloadUrl;
                var fileExtension = Path.GetExtension(DownloadUrl.AbsolutePath);
                var destination = Path.Combine(Id.ToString(), fileExtension);

                var destinationFile =
                    await _rootFolder.CreateFileAsync(
                        destination,
                        CreationCollisionOption.OpenIfExists);
                LocalFileUrl = new Uri(destinationFile.Path);

                var downloader = new BackgroundDownloader();
                _download = downloader.CreateDownload(source, destinationFile);

                // Attach progress and completion handlers.
                await HandleDownloadAsync(true);
            }
            catch (Exception ex)
            {
                var a = 5;
            }
        }

        public Task PauseAsync()
        {
            _download.Pause();
            return Task.CompletedTask;
        }

        public Task ResumeAsync()
        {
            _download.Resume();
            return Task.CompletedTask;
        }

        public Task DeleteAsync()
        {
            // Cancel download
            _cts.Cancel();
            _cts.Dispose();

            // TODO: Delete file?

            return Task.CompletedTask;
        }

        private async Task CreateRootFolderAsync()
        {
            var localFolder = ApplicationData.Current.LocalFolder;
            _rootFolder = await localFolder.CreateFolderAsync(VideosFolderName, CreationCollisionOption.OpenIfExists);
        }

        private async Task HandleDownloadAsync(bool start)
        {
            try
            {
                var progressCallback = new Progress<DownloadOperation>(DownloadProgress);
                if (start)
                {
                    // Start the download and attach a progress handler.
                    await _download.StartAsync().AsTask(_cts.Token, progressCallback);
                }
                else
                {
                    // The download was already running when the application started, re-attach the progress handler.
                    await _download.AttachAsync().AsTask(_cts.Token, progressCallback);
                }

                //ResponseInformation response = download.GetResponseInformation();
            }
            catch (Exception ex)
            {
                var a = 5;
            }
            finally
            {
            }
        }

        private void DownloadProgress(DownloadOperation download)
        {
            double percent = 100;
            if (download.Progress.TotalBytesToReceive > 0)
            {
                percent = download.Progress.BytesReceived * 100 / download.Progress.TotalBytesToReceive;
            }

            Percentage = percent;
        }
    }
}