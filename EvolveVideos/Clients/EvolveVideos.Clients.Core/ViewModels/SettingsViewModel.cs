using EvolveVideos.Clients.Core.Services;
using GalaSoft.MvvmLight.Command;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EvolveVideos.Clients.Core.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private readonly IDownloadManager _downloadManager;
        private readonly ISettingsService _settingsService;
        private readonly IStorageService _storageService;

        public SettingsViewModel(ISettingsService settingsService, IStorageService storageService,
            IDownloadManager downloaderService)
        {
            _settingsService = settingsService;
            _storageService = storageService;
            _downloadManager = downloaderService;

            CreateCommands();
        }

        public ICommand DeleteAllDataCommand { get; private set; }

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
            }
        }
    }
}