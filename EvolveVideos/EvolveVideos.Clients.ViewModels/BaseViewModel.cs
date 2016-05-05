using EvolveVideos.Clients.Core.Utils;
using GalaSoft.MvvmLight;
using System.Threading.Tasks;

namespace EvolveVideos.Clients.ViewModels
{
    public class BaseViewModel : ViewModelBase
    {
        private bool _isBusy;

        public bool IsBusy
        {
            get { return this._isBusy; }
            set { this.Set(() => this.IsBusy, ref this._isBusy, value); }
        }

        public virtual Task OnNavigateTo(object parameter)
        {
            return TaskUtils.CompletedTask;
        }

        public virtual Task OnNavigateFrom(object parameter)
        {
            return TaskUtils.CompletedTask;
        }
    }
}