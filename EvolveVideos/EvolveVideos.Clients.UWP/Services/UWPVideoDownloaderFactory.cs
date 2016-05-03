using EvolveVideos.Clients.Core.Services.Download;

namespace EvolveVideos.Clients.UWP.Services
{
    public class UWPVideoDownloaderFactory : IVideoDownloaderFactory
    {
        public IVideoDownload Create()
        {
            return new UWPBackgroundDowloader();
        }
    }
}