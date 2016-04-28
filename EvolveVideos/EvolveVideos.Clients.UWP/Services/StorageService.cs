using EvolveVideos.Clients.Core.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using EvolveVideos.Data.Models;

namespace EvolveVideos.Clients.UWP.Services
{
    public class StorageService : IStorageService
    {
        public async Task<List<EvolveSession>> LoadSessions()
        {
            try
            {
                var folder = Package.Current.InstalledLocation;
                var file = await folder.GetFileAsync(Path.Combine("Resources", "EvolveVideos.json"));
                var content = await FileIO.ReadTextAsync(file);

                var data = JsonConvert.DeserializeObject<List<EvolveSession>>(content);
                return data;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return new List<EvolveSession>();
            }
        }
    }
}