using System;

namespace EvolveVideos.Data.Models
{
    public class EvolveSession
    {
        public string Title { get; set; }
        public string Track { get; set; }
        public string Thumbnail { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string YoutubeID { get; set; }
        public string VideoUrl { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime DateLastModification { get; set; }
    }
}