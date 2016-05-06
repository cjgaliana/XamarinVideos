using EvolveVideos.Clients.Services;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Windows.Storage;

namespace EvolveVideos.Clients.UWP.Services
{
    public class RoamingSettingsService : ISettingsService
    {
        private readonly ApplicationDataContainer _roamingSettings;

        public RoamingSettingsService()
        {
            this._roamingSettings = ApplicationData.Current.RoamingSettings;
        }

        public Task SaveSetting<T>(string key, object data)
        {
            var json = JsonConvert.SerializeObject(data);
            this._roamingSettings.Values[key] = json;
            return Task.CompletedTask;
        }

        public Task SaveSetting<T>(object data)
        {
            var key = typeof(T).FullName;
            return this.SaveSetting<T>(key, data);
        }

        public Task<T> LoadSettingAsync<T>(string key)
        {
            return this.LoadSettingAsync<T>(key, default(T));
        }

        public Task<T> LoadSettingAsync<T>(string key, T defaultValue)
        {
            var data = defaultValue;
            var value = this._roamingSettings.Values[key];
            if (value != null)
            {
                var json = value.ToString();
                data = JsonConvert.DeserializeObject<T>(json);
            }

            return Task.FromResult(data);
        }

        public Task<T> LoadSettingAsync<T>()
        {
            var key = typeof(T).FullName;
            return this.LoadSettingAsync<T>(key);
        }

        public Task DeleteSettingAsync(string key)
        {
            this._roamingSettings.Values.Remove(key);
            return Task.CompletedTask;
        }

        public Task DeleteSettingAsync<T>()
        {
            var key = typeof(T).FullName;
            return this.DeleteSettingAsync(key);
        }
    }
}