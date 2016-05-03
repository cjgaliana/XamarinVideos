using System;
using System.Collections.Generic;

namespace EvolveVideos.Data.Models
{
    public class VideoCollection
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Website { get; set; }

        public IEnumerable<EvolveSession> Videos { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime DateLastModification { get; set; }

        public string FileName { get; set; }
    }
}