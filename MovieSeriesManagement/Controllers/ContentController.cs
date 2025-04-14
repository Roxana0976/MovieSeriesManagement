using System;
using System.Collections.Generic;
using System.Linq;
using MovieSeriesManagement.Models;
using MovieSeriesManagement.Services;
using ContentTypeEnum = MovieSeriesManagement.Models.ContentType;

namespace MovieSeriesManagement.Controllers
{
    public class ContentController
    {
        private readonly JsonDataService _dataService;

        public ContentController()
        {
            _dataService = new JsonDataService();
        }

        public List<Content> GetAllContent()
        {
            return _dataService.GetAllContent();
        }

        public List<Content> GetAllMovies()
        {
            return _dataService.GetAllMovies();
        }

        public List<Content> GetAllSeries()
        {
            return _dataService.GetAllSeries();
        }

        public Content GetContentById(int id)
        {
            return _dataService.GetContentById(id);
        }

        public List<Content> SearchContent(string searchTerm, List<string> genres = null, string platform = null, ContentTypeEnum? type = null)
        {
            var allContent = _dataService.GetAllContent();

            // Filter by search term
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                allContent = allContent.Where(c =>
                    c.Title.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    c.Description.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            }

            // Filter by genres
            if (genres != null && genres.Count > 0)
            {
                allContent = allContent.Where(c =>
                    c.Genres.Any(g => genres.Contains(g, StringComparer.OrdinalIgnoreCase))).ToList();
            }

            // Filter by platform
            if (!string.IsNullOrWhiteSpace(platform))
            {
                allContent = allContent.Where(c =>
                    c.Platform.Equals(platform, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Filter by type
            if (type.HasValue)
            {
                allContent = allContent.Where(c => c.Type == type.Value).ToList();
            }

            return allContent;
        }

        public List<string> GetAllGenres()
        {
            var allContent = _dataService.GetAllContent();
            return allContent.SelectMany(c => c.Genres).Distinct().ToList();
        }

        public List<string> GetAllPlatforms()
        {
            var allContent = _dataService.GetAllContent();
            return allContent.Select(c => c.Platform).Distinct().ToList();
        }

        public void AddContent(Content content)
        {
            var allMovies = _dataService.GetAllMovies();
            var allSeries = _dataService.GetAllSeries();

            // Generate ID
            if (content.Type == ContentTypeEnum.Movie)
            {
                if (allMovies.Count > 0)
                    content.Id = allMovies.Max(m => m.Id) + 1;
                else
                    content.Id = 1;

                allMovies.Add(content);
                _dataService.SaveMovies(allMovies);
            }
            else // Series
            {
                if (allSeries.Count > 0)
                    content.Id = allSeries.Max(s => s.Id) + 1;
                else
                    content.Id = 51; // Series start from ID 51

                allSeries.Add(content);
                _dataService.SaveSeries(allSeries);
            }
        }

        public void UpdateContent(Content content)
        {
            _dataService.UpdateContent(content);
        }

        public void DeleteContent(int id)
        {
            var content = _dataService.GetContentById(id);
            if (content == null) return;

            if (content.Type == ContentTypeEnum.Movie)
            {
                var movies = _dataService.GetAllMovies();
                movies.RemoveAll(m => m.Id == id);
                _dataService.SaveMovies(movies);
            }
            else // Series
            {
                var series = _dataService.GetAllSeries();
                series.RemoveAll(s => s.Id == id);
                _dataService.SaveSeries(series);
            }
        }

        public List<Content> GetRecommendedContent(int userId, int limit = 10)
        {
            var user = _dataService.GetUserById(userId);
            if (user == null) return new List<Content>();

            // Get all content the user has watched
            var watchedContent = new List<Content>();
            foreach (var contentId in user.WatchedContentIds)
            {
                var content = _dataService.GetContentById(contentId);
                if (content != null)
                {
                    watchedContent.Add(content);
                }
            }

            // Get favorite genres based on watched content
            var genreCounts = new Dictionary<string, int>();
            foreach (var content in watchedContent)
            {
                foreach (var genre in content.Genres)
                {
                    if (genreCounts.ContainsKey(genre))
                        genreCounts[genre]++;
                    else
                        genreCounts[genre] = 1;
                }
            }

            // Get top genres
            var favoriteGenres = genreCounts.OrderByDescending(kv => kv.Value)
                                           .Take(3)
                                           .Select(kv => kv.Key)
                                           .ToList();

            // Get all content
            var allContent = _dataService.GetAllContent();

            // Filter out already watched content
            allContent = allContent.Where(c => !user.WatchedContentIds.Contains(c.Id)).ToList();

            // Score content based on genre matching and rating
            var scoredContent = allContent.Select(c => new
            {
                Content = c,
                GenreScore = c.Genres.Count(g => favoriteGenres.Contains(g)),
                RatingScore = c.AverageRating
            }).ToList();

            // Order by genre score (primary) and rating (secondary)
            var recommendations = scoredContent.OrderByDescending(s => s.GenreScore)
                                              .ThenByDescending(s => s.RatingScore)
                                              .Take(limit)
                                              .Select(s => s.Content)
                                              .ToList();

            return recommendations;
        }

        public Dictionary<string, int> GetGenreStatistics()
        {
            var allContent = _dataService.GetAllContent();
            var genreCounts = new Dictionary<string, int>();

            foreach (var content in allContent)
            {
                foreach (var genre in content.Genres)
                {
                    if (genreCounts.ContainsKey(genre))
                        genreCounts[genre]++;
                    else
                        genreCounts[genre] = 1;
                }
            }

            return genreCounts;
        }

        public Dictionary<string, int> GetPlatformStatistics()
        {
            var allContent = _dataService.GetAllContent();
            var platformCounts = new Dictionary<string, int>();

            foreach (var content in allContent)
            {
                if (platformCounts.ContainsKey(content.Platform))
                    platformCounts[content.Platform]++;
                else
                    platformCounts[content.Platform] = 1;
            }

            return platformCounts;
        }
    }
}
