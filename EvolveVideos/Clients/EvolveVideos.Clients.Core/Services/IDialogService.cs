using System.Threading.Tasks;

namespace EvolveVideos.Clients.Core.Services
{
    public interface IDialogService
    {
        Task ShowMessageAsync(string caption, string message);
    }
}