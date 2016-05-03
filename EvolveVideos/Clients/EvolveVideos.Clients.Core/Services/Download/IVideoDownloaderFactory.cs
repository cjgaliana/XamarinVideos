namespace EvolveVideos.Clients.Core.Services.Download
{
    public interface IVideoDownloaderFactory
    {
        IVideoDownload Create();
    }
}