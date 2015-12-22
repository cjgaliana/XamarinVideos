using Newtonsoft.Json;
using System;
using System.IO;

namespace Tools.Evolve2015Extractor
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Reading file");

            // Read HTML file
            var html = ReadFile("EvolveWebSiteData.txt");

            // Extract the data from the HTML
            var list = ExtractorHelper.ExtractSessions(html);

            // Save data in the new file
            var json = JsonConvert.SerializeObject(list);
            SaveFile("SerializedData.json", json);

            Console.WriteLine("Pres any key to continue...");
            Console.ReadKey();
        }

        private static string ReadFile(string fileName)
        {
            return File.ReadAllText(fileName);
        }

        private static void SaveFile(string fileName, string content)
        {
            File.WriteAllText(fileName, content);
        }
    }
}