using System.Threading.Tasks;

namespace EvolveVideos.Clients.Core.Services
{
    public interface ISettingsService
    {
        Task SaveSetting<T>(string key, object data);

        Task SaveSetting<T>(object data);

        Task<T> LoadSettingAsync<T>(string key);

        Task<T> LoadSettingAsync<T>();

        Task DeleteSettingAsync(string key);
        Task DeleteSettingAsync<T>();
    }
}