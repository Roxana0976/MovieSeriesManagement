using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using MovieSeriesManagement.Models;

namespace MovieSeriesManagement.Services
{
    public class JsonDataService
    {
        private readonly string _dataFolderPath;
        private readonly string _usersFilePath;
        private readonly string _moviesFilePath;
        private readonly string _seriesFilePath;

        public JsonDataService()
        {
            _dataFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            _usersFilePath = Path.Combine(_dataFolderPath, "users.json");
            _moviesFilePath = Path.Combine(_dataFolderPath, "movies.json");
            _seriesFilePath = Path.Combine(_dataFolderPath, "series.json");

            // Create data folder if it doesn't exist
            if (!Directory.Exists(_dataFolderPath))
            {
                Directory.CreateDirectory(_dataFolderPath);
            }

            // Initialize data files if they don't exist
            InitializeDataFiles();
        }

        private void InitializeDataFiles()
        {
            // Initialize users.json with 100 users and 1 admin if it doesn't exist
            if (!File.Exists(_usersFilePath))
            {
                var users = GenerateSampleUsers();
                SaveUsers(users);
            }

            // Initialize movies.json with 50 movies if it doesn't exist
            if (!File.Exists(_moviesFilePath))
            {
                var movies = GenerateSampleMovies();
                SaveMovies(movies);
            }

            // Initialize series.json with 50 series if it doesn't exist
            if (!File.Exists(_seriesFilePath))
            {
                var series = GenerateSampleSeries();
                SaveSeries(series);
            }
        }

        // Users CRUD operations
        public List<User> GetAllUsers()
        {
            if (!File.Exists(_usersFilePath))
                return new List<User>();

            string json = File.ReadAllText(_usersFilePath);
            return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
        }

        public void SaveUsers(List<User> users)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(users, options);
            File.WriteAllText(_usersFilePath, json);
        }

        public User GetUserByUsername(string username)
        {
            var users = GetAllUsers();
            return users.Find(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        public User GetUserById(int id)
        {
            var users = GetAllUsers();
            return users.Find(u => u.Id == id);
        }

        public void AddUser(User user)
        {
            var users = GetAllUsers();

            // Generate ID
            if (users.Count > 0)
                user.Id = users.Max(u => u.Id) + 1;
            else
                user.Id = 1;

            users.Add(user);
            SaveUsers(users);
        }

        public void UpdateUser(User user)
        {
            var users = GetAllUsers();
            int index = users.FindIndex(u => u.Id == user.Id);

            if (index >= 0)
            {
                users[index] = user;
                SaveUsers(users);
            }
        }

        // Movies CRUD operations
        public List<Content> GetAllMovies()
        {
            if (!File.Exists(_moviesFilePath))
                return new List<Content>();

            string json = File.ReadAllText(_moviesFilePath);
            return JsonSerializer.Deserialize<List<Content>>(json) ?? new List<Content>();
        }

        public void SaveMovies(List<Content> movies)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(movies, options);
            File.WriteAllText(_moviesFilePath, json);
        }

        // Series CRUD operations
        public List<Content> GetAllSeries()
        {
            if (!File.Exists(_seriesFilePath))
                return new List<Content>();

            string json = File.ReadAllText(_seriesFilePath);
            return JsonSerializer.Deserialize<List<Content>>(json) ?? new List<Content>();
        }

        public void SaveSeries(List<Content> series)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(series, options);
            File.WriteAllText(_seriesFilePath, json);
        }

        // Get all content (movies and series)
        public List<Content> GetAllContent()
        {
            var allContent = new List<Content>();
            allContent.AddRange(GetAllMovies());
            allContent.AddRange(GetAllSeries());
            return allContent;
        }

        // Get content by id
        public Content GetContentById(int id)
        {
            var allContent = GetAllContent();
            return allContent.Find(c => c.Id == id);
        }

        // Update content (movie or series)
        public void UpdateContent(Content content)
        {
            if (content.Type == ContentType.Movie)
            {
                var movies = GetAllMovies();
                int index = movies.FindIndex(m => m.Id == content.Id);

                if (index >= 0)
                {
                    movies[index] = content;
                    SaveMovies(movies);
                }
            }
            else // Series
            {
                var series = GetAllSeries();
                int index = series.FindIndex(s => s.Id == content.Id);

                if (index >= 0)
                {
                    series[index] = content;
                    SaveSeries(series);
                }
            }
        }

        // Sample data generation methods
        private List<User> GenerateSampleUsers()
        {
            var users = new List<User>();

            // Add admin user
            users.Add(new User
            {
                Id = 1,
                Username = "admin",
                Password = "admin", // In a real app, use proper password hashing
                Email = "admin@example.com",
                Role = UserRole.Administrator,
                RegistrationDate = DateTime.Now.AddYears(-1),
                LastLoginDate = DateTime.Now
            });

            // Add 100 regular users
            for (int i = 2; i <= 101; i++)
            {
                users.Add(new User
                {
                    Id = i,
                    Username = $"user{i - 1}",
                    Password = $"password{i - 1}", // In a real app, use proper password hashing
                    Email = $"user{i - 1}@example.com",
                    Role = UserRole.Regular,
                    RegistrationDate = DateTime.Now.AddDays(-Random.Shared.Next(1, 365)),
                    LastLoginDate = DateTime.Now.AddDays(-Random.Shared.Next(0, 30))
                });
            }

            return users;
        }

        private List<Content> GenerateSampleMovies()
        {
            string[] genres = { "Action", "Comedy", "Drama", "Sci-Fi", "Horror", "Thriller", "Romance", "Fantasy", "Adventure", "Animation" };
            string[] platforms = { "Netflix", "Amazon Prime", "Disney+", "HBO Max", "Hulu", "Apple TV+", "Paramount+" };

            var movies = new List<Content>();

            for (int i = 1; i <= 50; i++)
            {
                var movie = new Content
                {
                    Id = i,
                    Title = $"Movie {i}",
                    Description = $"This is a description for Movie {i}. It's a very interesting movie that everyone should watch.",
                    Type = ContentType.Movie,
                    ReleaseYear = Random.Shared.Next(1980, 2024),
                    Duration = Random.Shared.Next(80, 180),
                    Platform = platforms[Random.Shared.Next(platforms.Length)],
                    ViewCount = Random.Shared.Next(100, 10000),
                    AverageRating = Math.Round(Random.Shared.NextDouble() * 5 + 3, 1) // Rating between 3 and 8 with one decimal
                };

                // Add 1-3 random genres
                int genreCount = Random.Shared.Next(1, 4);
                for (int j = 0; j < genreCount; j++)
                {
                    string genre = genres[Random.Shared.Next(genres.Length)];
                    if (!movie.Genres.Contains(genre))
                        movie.Genres.Add(genre);
                }

                movies.Add(movie);
            }

            return movies;
        }

        private List<Content> GenerateSampleSeries()
        {
            string[] genres = { "Action", "Comedy", "Drama", "Sci-Fi", "Horror", "Thriller", "Romance", "Fantasy", "Adventure", "Animation" };
            string[] platforms = { "Netflix", "Amazon Prime", "Disney+", "HBO Max", "Hulu", "Apple TV+", "Paramount+" };

            var series = new List<Content>();

            for (int i = 51; i <= 100; i++) // IDs from 51 to 100 for series
            {
                var show = new Content
                {
                    Id = i,
                    Title = $"Series {i - 50}",
                    Description = $"This is a description for Series {i - 50}. It's a very engaging series with multiple seasons.",
                    Type = ContentType.Series,
                    ReleaseYear = Random.Shared.Next(1990, 2024),
                    Platform = platforms[Random.Shared.Next(platforms.Length)],
                    ViewCount = Random.Shared.Next(500, 20000),
                    AverageRating = Math.Round(Random.Shared.NextDouble() * 5 + 3, 1) // Rating between 3 and 8 with one decimal
                };

                // Add 1-3 random genres
                int genreCount = Random.Shared.Next(1, 4);
                for (int j = 0; j < genreCount; j++)
                {
                    string genre = genres[Random.Shared.Next(genres.Length)];
                    if (!show.Genres.Contains(genre))
                        show.Genres.Add(genre);
                }

                // Add 1-5 seasons
                int seasonCount = Random.Shared.Next(1, 6);
                int totalDuration = 0;

                for (int j = 1; j <= seasonCount; j++)
                {
                    var season = new Season
                    {
                        Number = j,
                        Title = $"Season {j}"
                    };

                    // Add 8-15 episodes per season
                    int episodeCount = Random.Shared.Next(8, 16);

                    for (int k = 1; k <= episodeCount; k++)
                    {
                        int duration = Random.Shared.Next(20, 61); // 20-60 minutes
                        totalDuration += duration;

                        season.Episodes.Add(new Episode
                        {
                            Number = k,
                            Title = $"Episode {k}",
                            Duration = duration
                        });
                    }

                    show.Seasons.Add(season);
                }

                show.Duration = totalDuration;
                series.Add(show);
            }

            return series;
        }
    }
}
