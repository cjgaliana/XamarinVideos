using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using EvolveVideos.Data.Models;
using Newtonsoft.Json;

namespace EvolveVideos.Data
{
    public interface IDataService
    {
        Task<List<EvolveSession>> GetLatestAsync();

        Task<List<VideoCollection>> GetVideoCollectionsAsync();

        Task<List<EvolveSession>> GetVideosAsync(VideoCollection collection);
    }

    public class LocalResourcesDataService : IDataService
    {
        public async Task<List<EvolveSession>> GetLatestAsync()
        {
            var json = await LoadFileContentAsync("Evolve2015.json");
            var data = JsonConvert.DeserializeObject<List<EvolveSession>>(json);
            return data;
        }

        public async Task<List<VideoCollection>> GetVideoCollectionsAsync()
        {
            var json = await LoadFileContentAsync("Collections.json");
            var data = JsonConvert.DeserializeObject<List<VideoCollection>>(json);
            return data;
        }

        public async Task<List<EvolveSession>> GetVideosAsync(VideoCollection collection)
        {
            var json = await LoadFileContentAsync(collection.Name);
            var data = JsonConvert.DeserializeObject<List<EvolveSession>>(json);
            return data;
        }

        public async Task<string> LoadFileContentAsync(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                throw new ArgumentNullException(nameof(filename));
            }

            var assembly = typeof(IDataService).GetTypeInfo().Assembly;

            // Use this help aid to figure out what the actual manifest resource name is.
            var resources = assembly.GetManifestResourceNames();

            var fullname = resources.FirstOrDefault(x => x.Contains(filename));
            if (string.IsNullOrWhiteSpace(fullname))
            {
                throw new FileNotFoundException($"Resource not found in the PCL library", filename);
            }
            // Once you figure out the name, pass it in as the argument here.
            var stream = assembly.GetManifestResourceStream(fullname);
            using (var reader = new StreamReader(stream))
            {
                var text = await reader.ReadToEndAsync();
                return text;
            }
        }
    }
}