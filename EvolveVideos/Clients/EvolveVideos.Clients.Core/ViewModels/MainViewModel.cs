﻿using EvolveVideos.Clients.Core.Services;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using EvolveVideos.Data;
using EvolveVideos.Data.Models;

namespace EvolveVideos.Clients.Core.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly INavigationService _navigationService;
        private readonly IStorageService _storageService;
        private readonly IDataService _dataService;

        private List<EvolveSession> _sessions = new List<EvolveSession>();

        public List<EvolveSession> Sessions
        {
            get { return this._sessions; }
            set { this.Set(() => this.Sessions, ref this._sessions, value); }
        }

        public ICommand LoadCommand
        {
            get;
            private set;
        }

        public ICommand NavigateToSessionCommand
        {
            get;
            private set;
        }

        public MainViewModel(
            INavigationService navigationService,
            IDialogService dialogService,
            IStorageService storageService,
            IDataService dataService)
        {
            this._navigationService = navigationService;
            this._dialogService = dialogService;
            this._storageService = storageService;
            _dataService = dataService;

            this.CreateCommands();
        }

        public override async Task OnNavigateTo(object parameter)
        {
            await base.OnNavigateTo(parameter);
            await this.LoadEvolveSessions();
        }

        private void CreateCommands()
        {
            this.LoadCommand = new RelayCommand(async () => { await this.LoadEvolveSessions(); });
            this.NavigateToSessionCommand = new RelayCommand<EvolveSession>(this.NavigateToSession);
        }

        private async Task LoadEvolveSessions()
        {
            //this.Sessions = await this._storageService.LoadSessions();
            var latestVideos = await this._dataService.GetLatestAsync();
            this.Sessions = latestVideos;
        }

        private void NavigateToSession(EvolveSession session)
        {
            this._navigationService.NavigateTo(PageKey.SessionDetailsPage, session);
        }
    }
}