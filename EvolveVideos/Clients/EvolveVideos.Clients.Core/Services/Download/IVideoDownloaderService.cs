using System;
using System.Threading.Tasks;

namespace EvolveVideos.Clients.Core.Services.Download
{
    public interface IVideoDownloaderService
    {
        Task<Uri> GetStreamVideoUrlAsync(string youtubeId);
        Task<Uri> GetDownloadVideoUrlAsync(string youtubeId);
    }
}