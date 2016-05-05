using EvolveVideos.Data.Models;

namespace EvolveVideos.Clients.Core.Models
{
    public class DownloadSession
    {
        public IVideoDownload Download { get; set; }
        public EvolveSession Session { get; set; }
    }
}