using EvolveVideos.Clients.Core.Models;

namespace EvolveVideos.Clients.Services.Download
{
    public interface IVideoDownloaderFactory
    {
        IVideoDownload Create();
    }
}