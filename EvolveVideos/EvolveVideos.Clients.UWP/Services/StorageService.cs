using EvolveVideos.Clients.Core.Services;
using EvolveVideos.Clients.Core.Services.Download;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace EvolveVideos.Clients.UWP.Services
{
    internal class StorageFileKey
    {
        public static string DownloadsFile = "downloads.json";
    }

    public class StorageService : IStorageService
    {
        private readonly StorageFolder _rootFolder;

        public async Task<IList<IVideoDownload>> LoadDownloadsAsync()
        {
            var data = await this.LoadFile<IList<IVideoDownload>>(StorageFileKey.DownloadsFile);
            return data;
        }

        public async Task SaveDownloadsAsync(IList<IVideoDownload> downloads)
        {
            await this.SaveFile(StorageFileKey.DownloadsFile, downloads);
        }

        public StorageService()
        {
            this._rootFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
        }

        private async Task SaveFile(string filePath, object data)
        {
            var json = JsonConvert.SerializeObject(data);
            var file = await this._rootFolder.CreateFileAsync(filePath, CreationCollisionOption.OpenIfExists);
            if (file != null)
            {
                await FileIO.WriteTextAsync(file, json);
            }
        }

        private async Task<T> LoadFile<T>(string filePath)
        {
            var file = await this._rootFolder.CreateFileAsync(filePath, CreationCollisionOption.OpenIfExists);
            if (file != null)
            {
                var json = await FileIO.ReadTextAsync(file);
                var data = JsonConvert.DeserializeObject<T>(json);
                return data;
            }

            throw new FileNotFoundException($"File nor found in {this._rootFolder.Path} directoy", filePath);
        }
    }
}