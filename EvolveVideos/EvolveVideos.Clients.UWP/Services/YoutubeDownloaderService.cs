using System;
using System.Threading.Tasks;
using EvolveVideos.Clients.Core.Services;
using EvolveVideos.Clients.Core.Services.Download;
using VideoLibrary;

namespace EvolveVideos.Clients.UWP.Services
{
    public class YoutubeDownloaderService : IVideoDownloaderService
    {
        public async Task<Uri> GetStreamVideoUrlAsync(string youtubeId)
        {
            return await Task.FromResult(new Uri("https://www.youtube.com/watch?v=" + youtubeId));
        }

        public async Task<Uri> GetDownloadVideoUrlAsync(string youtubeId)
        {
            var youtubeUrl = await this.GetStreamVideoUrlAsync(youtubeId);
            var video = YouTube.Default.GetVideo(youtubeUrl.AbsoluteUri);
            return await Task.FromResult(new Uri(video.Uri));
        }
    }
}