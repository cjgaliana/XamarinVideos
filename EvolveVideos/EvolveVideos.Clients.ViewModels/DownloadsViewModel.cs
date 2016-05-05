using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using EvolveVideos.Clients.Core.Models;
using EvolveVideos.Clients.Services;
using EvolveVideos.Clients.Services.Download;
using EvolveVideos.Data;
using GalaSoft.MvvmLight.Command;

namespace EvolveVideos.Clients.ViewModels
{
    public class DownloadsViewModel : BaseViewModel
    {
        private readonly IDataService _dataService;
        private readonly IDownloadManager _downloadManager;
        private readonly INavigationService _navigationService;

        private ObservableCollection<DownloadSession> _completedSessions;
        private ObservableCollection<DownloadSession> _downloadingSessions;

        public DownloadsViewModel(INavigationService navigationService, IDownloadManager downloadManager,
            IDataService dataService)
        {
            _navigationService = navigationService;
            _downloadManager = downloadManager;
            _dataService = dataService;

            CreateCommands();
        }

        public ObservableCollection<DownloadSession> CompletedSessions
        {
            get { return _completedSessions; }
            set { Set(() => CompletedSessions, ref _completedSessions, value); }
        }

        public ObservableCollection<DownloadSession> DownloadingSessions
        {
            get { return _downloadingSessions; }
            set { Set(() => DownloadingSessions, ref _downloadingSessions, value); }
        }

        public ICommand OpenSessionCommand { get; private set; }

        private void CreateCommands()
        {
            OpenSessionCommand = new RelayCommand<DownloadSession>(OpenSessionDetails);
        }

        private void OpenSessionDetails(DownloadSession downloadedSession)
        {
            var session = downloadedSession.Session;
            _navigationService.NavigateTo(PageKey.SessionDetailsPage, session);
        }

        public override async Task OnNavigateTo(object parameter)
        {
            await base.OnNavigateTo(parameter);

            var completed = _downloadManager.Downloads.Where(x => x.Status == DownloadStatus.Completed).ToList();
            var downloading = _downloadManager.Downloads.Where(x => x.Status != DownloadStatus.Completed).ToList();

            CompletedSessions = new ObservableCollection<DownloadSession>();
            foreach (var videoDownload in completed)
            {
                var session = await _dataService.GetSessionAsync(videoDownload.SessionId);
                CompletedSessions.Add(new DownloadSession
                {
                    Session = session,
                    Download = videoDownload
                });
            }

            DownloadingSessions = new ObservableCollection<DownloadSession>();
            foreach (var videoDownload in downloading)
            {
                var session = await _dataService.GetSessionAsync(videoDownload.SessionId);
                DownloadingSessions.Add(new DownloadSession
                {
                    Session = session,
                    Download = videoDownload
                });
            }
        }
    }
}