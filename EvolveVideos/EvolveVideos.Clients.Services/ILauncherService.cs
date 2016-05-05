using System.Threading.Tasks;

namespace EvolveVideos.Clients.Services
{
    public interface ILauncherService
    {
        Task OpenWebSiteAsync(string url);
    }
}