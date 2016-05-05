using System.Threading.Tasks;

namespace EvolveVideos.Clients.Services
{
    public interface IDialogService
    {
        Task ShowMessageAsync(string caption, string message);
    }
}