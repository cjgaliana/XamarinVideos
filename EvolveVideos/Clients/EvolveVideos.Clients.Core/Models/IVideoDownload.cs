using System;
using System.Threading.Tasks;

namespace EvolveVideos.Clients.Core.Models
{
    public class DownloadCompetedArgs
    {
        public IVideoDownload Download { get; set; }
    }

    public class DownloadProgressChangedArgs
    {
        public IVideoDownload Download { get; set; }
        public double Progress { get; set; }
    }
    public class DownloadStatusChangedArgs
    {
        public IVideoDownload Download { get; set; }
        public DownloadStatus Status { get; set; }
        
    }

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

        event EventHandler<DownloadCompetedArgs> DownloadCompleted;

        event EventHandler<DownloadProgressChangedArgs> DownloadProgressChanged;

        event EventHandler<DownloadStatusChangedArgs> DownloadStatusChanged;
    }
}