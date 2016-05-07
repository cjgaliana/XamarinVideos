using System;
using System.Threading.Tasks;
using EvolveVideos.Clients.Services;
using Microsoft.HockeyApp;

namespace EvolveVideos.Clients.UWP.Services
{
    public class HockeyAppLogService : ILogService
    {
        public  Task LogExceptionAsync(Exception exception)
        {
            HockeyClient.Current.TrackEvent("Exception: " + exception.ToString());
            return Task.CompletedTask;
        }
    }
}