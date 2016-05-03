using EvolveVideos.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EvolveVideos.Clients.Core.Services.Download
{
    public interface IDownloadManager
    {
        Task InitializeAsync();

        IList<IVideoDownload> Downloads { get; }

        Task PauseAllDownloadsAsync();

        Task ResumeAllDownloadsAsync();

        Task DeleteAllDownloadsAsync();

        Task QueueDownloadAsync(EvolveSession session);

        Task DeleteDownloadAsync(IVideoDownload download);

        Task PauseDownload(IVideoDownload download);

        Task ResumeDownload(IVideoDownload download);

        Task<IVideoDownload> GetDownloadForSessionAsync(EvolveSession session);
    }
}