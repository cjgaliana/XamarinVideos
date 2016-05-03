using System;
using System.Threading.Tasks;

namespace EvolveVideos.Clients.Core.Services.Download
{
    public interface IVideoDownload
    {
        DownloadStatus Status { get; set; }
        Guid Id { get; set; }
        Uri DownloadUrl { get; set; }
        Guid SessionId { get; set; }
        Uri LocalFileUrl { get; set; }
        double Percentage { get; set; }

        Task StartDownlodAsync();

        Task PauseAsync();

        Task ResumeAsync();

        Task DeleteAsync();
    }
}