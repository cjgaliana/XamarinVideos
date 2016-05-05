using EvolveVideos.Clients.Services;
using System;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace EvolveVideos.Clients.UWP.Services
{
    public class DialogService : IDialogService
    {
        public async Task ShowMessageAsync(string caption, string message)
        {
            var dialog = new MessageDialog(message, caption);
            await dialog.ShowAsync();
        }
    }
}