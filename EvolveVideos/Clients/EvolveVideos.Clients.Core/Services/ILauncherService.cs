using System.Threading.Tasks;

namespace EvolveVideos.Clients.Core.Services
{
    public interface ILauncherService
    {
        Task OpenWebSiteAsync(string url);
    }
}