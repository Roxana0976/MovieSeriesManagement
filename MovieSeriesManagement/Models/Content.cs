using System;
using System.Collections.Generic;

namespace MovieSeriesManagement.Models
{
    public enum ContentType
    {
        Movie,
        Series
    }

    public class Content
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<string> Genres { get; set; } = new List<string>();
        public string Platform { get; set; }
        public int ReleaseYear { get; set; }
        public double AverageRating { get; set; }
        public ContentType Type { get; set; }
        public int ViewCount { get; set; }
        public int Duration { get; set; } // In minutes for movies, total minutes for series

        // Specific for Series
        public List<Season> Seasons { get; set; } = new List<Season>();
    }

    public class Season
    {
        public int Number { get; set; }
        public string Title { get; set; }
        public List<Episode> Episodes { get; set; } = new List<Episode>();
    }

    public class Episode
    {
        public int Number { get; set; }
        public string Title { get; set; }
        public int Duration { get; set; } // In minutes
    }
}
