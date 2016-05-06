using System.Threading.Tasks;

namespace EvolveVideos.Clients.Services
{
    public class SettingsKeys
    {
        public static string DownloaderAutoResume = "DownloaderAutoResume_key";
    }

    public interface ISettingsService
    {
        Task SaveSetting<T>(string key, object data);

        Task SaveSetting<T>(object data);

        Task<T> LoadSettingAsync<T>(string key);

        Task<T> LoadSettingAsync<T>(string key, T defaultValue);

        Task<T> LoadSettingAsync<T>();

        Task DeleteSettingAsync(string key);

        Task DeleteSettingAsync<T>();
    }
}