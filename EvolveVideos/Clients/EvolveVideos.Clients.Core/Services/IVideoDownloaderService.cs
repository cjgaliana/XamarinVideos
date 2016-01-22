using System;
using System.Threading.Tasks;

namespace EvolveVideos.Clients.Core.Services
{
    public interface IVideoDownloaderService
    {
        Task<Uri> GetStreamVideoUrlAsync(string youtubeId);
        Task<Uri> GetDownloadVideoUrlAsync(string youtubeId);
    }
}