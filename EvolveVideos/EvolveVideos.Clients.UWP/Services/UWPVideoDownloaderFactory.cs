using EvolveVideos.Clients.Core.Models;
using EvolveVideos.Clients.Services.Download;

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