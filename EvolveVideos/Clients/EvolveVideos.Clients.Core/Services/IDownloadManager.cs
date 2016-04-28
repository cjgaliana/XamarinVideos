using System.Threading.Tasks;
using EvolveVideos.Data.Models;

namespace EvolveVideos.Clients.Core.Services
{
    public interface IDownloadManager
    {
        Task PauseAllDownloadsAsync();

        Task ResumeAllDownloadsAsync();

        Task DeleteAllDownloadsAsync();
        Task QueueDownloadAsync(EvolveSession session);
    }
}