using System;
using System.Threading.Tasks;
using Windows.System;
using EvolveVideos.Clients.Core.Services;

namespace EvolveVideos.Clients.UWP.Services
{
    public class LauncherService : ILauncherService
    {
        public async Task OpenWebSiteAsync(string url)
        {
            var success = await Launcher.LaunchUriAsync(new Uri(url));
            if (success)
            {
                // URI launched
            }
            else
            {
                // URI launch failed
            }
        }
    }
}