﻿using EvolveVideos.Clients.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using EvolveVideos.Clients.Services;

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
            var uwPdata = await this.LoadFile<IList<UWPBackgroundDowloader>>(StorageFileKey.DownloadsFile);
            var data = uwPdata?.Cast<IVideoDownload>().ToList();
            return data;
        }

        public async Task SaveDownloadsAsync(IList<IVideoDownload> downloads)
        {
            await this.SaveFile(StorageFileKey.DownloadsFile, downloads);
        }

        public StorageService()
        {
            this._rootFolder = ApplicationData.Current.LocalFolder;
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
            try
            {
                var file = await this._rootFolder.CreateFileAsync(filePath, CreationCollisionOption.OpenIfExists);
                if (file != null)
                {
                    var json = await FileIO.ReadTextAsync(file);
                    var data = JsonConvert.DeserializeObject<T>(json);
                    return data;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            throw new FileNotFoundException($"File nor found in {this._rootFolder.Path} directoy", filePath);
        }
    }
}