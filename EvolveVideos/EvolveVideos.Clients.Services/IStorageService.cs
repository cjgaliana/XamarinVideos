using System.Collections.Generic;
using System.Threading.Tasks;
using EvolveVideos.Clients.Core.Models;

namespace EvolveVideos.Clients.Services
{
    public interface IStorageService
    {
        Task<IList<IVideoDownload>> LoadDownloadsAsync();

        Task SaveDownloadsAsync(IList<IVideoDownload> downloads);
    }
}