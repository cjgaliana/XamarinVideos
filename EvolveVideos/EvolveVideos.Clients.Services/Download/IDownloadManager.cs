using System.Collections.Generic;
using System.Threading.Tasks;
using EvolveVideos.Clients.Core.Models;
using EvolveVideos.Data.Models;

namespace EvolveVideos.Clients.Services.Download
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