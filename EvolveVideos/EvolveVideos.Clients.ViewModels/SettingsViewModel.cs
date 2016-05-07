using System;
using System.Threading.Tasks;
using System.Windows.Input;
using EvolveVideos.Clients.Core.Utils;
using EvolveVideos.Clients.Services;
using EvolveVideos.Clients.Services.Download;
using GalaSoft.MvvmLight.Command;

namespace EvolveVideos.Clients.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private readonly IDownloadManager _downloadManager;
        private readonly IDialogService _dialogService;
        private readonly ISettingsService _settingsService;
        private readonly IStorageService _storageService;

        private bool _autoResumeDownloads;

        public SettingsViewModel(ISettingsService settingsService, IStorageService storageService,
            IDownloadManager downloaderService, IDialogService dialogService)
        {
            _settingsService = settingsService;
            _storageService = storageService;
            _downloadManager = downloaderService;
            _dialogService = dialogService;

            CreateCommands();
        }

        public ICommand DeleteAllDataCommand { get; private set; }

        public bool AutoResumeDownloads
        {
            get { return _autoResumeDownloads; }
            set
            {
                Set(() => AutoResumeDownloads, ref _autoResumeDownloads, value);
                _settingsService.SaveSetting<bool>(SettingsKeys.DownloaderAutoResume, value).FireAndForget();
            }
        }

        private void CreateCommands()
        {
            DeleteAllDataCommand = new RelayCommand(async () => await DeleteAllDataAsync());
        }

        private async Task DeleteAllDataAsync()
        {
            try
            {
                IsBusy = true;
                await _downloadManager.DeleteAllDownloadsAsync();
            }
            catch (Exception ex)
            {
                // TODO: Show error
            }
            finally
            {
                IsBusy = false;
                await this._dialogService.ShowMessageAsync("Done!", "All videos have been deleted from you device");
            }
        }

        public override async Task OnNavigateTo(object parameter)
        {
            await base.OnNavigateTo(parameter);

            await LoadSettingsAsync();
        }

        private async Task LoadSettingsAsync()
        {
            AutoResumeDownloads = await _settingsService.LoadSettingAsync(SettingsKeys.DownloaderAutoResume, true);
        }
    }
}