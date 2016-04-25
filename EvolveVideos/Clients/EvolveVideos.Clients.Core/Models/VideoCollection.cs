using System;
using System.Collections.Generic;

namespace EvolveVideos.Clients.Core.Models
{
    public class VideoCollection
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Website { get; set; }

        public IEnumerable<EvolveSession> Videos { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime DateLastModification { get; set; }
    }
}