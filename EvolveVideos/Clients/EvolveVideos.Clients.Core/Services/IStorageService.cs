using EvolveVideos.Clients.Core.Services.Download;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EvolveVideos.Clients.Core.Services
{
    public interface IStorageService
    {
        Task<IList<IVideoDownload>> LoadDownloadsAsync();

        Task SaveDownloadsAsync(IList<IVideoDownload> downloads);
    }
}