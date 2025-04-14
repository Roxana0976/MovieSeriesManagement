using System;
using System.Collections.Generic;
using System.Linq;
using MovieSeriesManagement.Models;
using MovieSeriesManagement.Services;

namespace MovieSeriesManagement.Controllers
{
    public class UserController
    {
        private readonly JsonDataService _dataService;

        public UserController()
        {
            _dataService = new JsonDataService();
        }

        public bool Login(string username, string password, out User loggedInUser)
        {
            loggedInUser = null;

            var user = _dataService.GetUserByUsername(username);
            if (user != null && user.Password == password) // In a real app, use proper password comparison
            {
                user.LastLoginDate = DateTime.Now;
                _dataService.UpdateUser(user);

                loggedInUser = user;
                return true;
            }

            return false;
        }

        public bool Register(string username, string password, string email)
        {
            // Check if username already exists
            if (_dataService.GetUserByUsername(username) != null)
                return false;

            var newUser = new User
            {
                Username = username,
                Password = password, // In a real app, use proper password hashing
                Email = email,
                Role = UserRole.Regular,
                RegistrationDate = DateTime.Now,
                LastLoginDate = DateTime.Now
            };

            _dataService.AddUser(newUser);
            return true;
        }

        public List<User> GetAllUsers()
        {
            return _dataService.GetAllUsers();
        }

        public void UpdateUserWatchHistory(int userId, int contentId, double progress)
        {
            var user = _dataService.GetUserById(userId);
            if (user == null) return;

            // Update progress
            user.WatchProgress[contentId] = progress;

            // Add to watched list if not already there and progress is complete
            if (progress >= 0.95 && !user.WatchedContentIds.Contains(contentId))
            {
                user.WatchedContentIds.Add(contentId);
            }

            _dataService.UpdateUser(user);

            // Update content view count
            var content = _dataService.GetContentById(contentId);
            if (content != null && progress >= 0.95)
            {
                content.ViewCount++;
                _dataService.UpdateContent(content);
            }
        }

        public void RateContent(int userId, int contentId, int rating)
        {
            if (rating < 1 || rating > 10) return;

            var user = _dataService.GetUserById(userId);
            if (user == null) return;

            // Add or update rating
            user.ContentRatings[contentId] = rating;
            _dataService.UpdateUser(user);

            // Update content average rating
            var content = _dataService.GetContentById(contentId);
            if (content != null)
            {
                // Get all ratings for this content
                var allUsers = _dataService.GetAllUsers();
                var ratings = new List<int>();

                foreach (var u in allUsers)
                {
                    if (u.ContentRatings.TryGetValue(contentId, out int r))
                    {
                        ratings.Add(r);
                    }
                }

                if (ratings.Count > 0)
                {
                    content.AverageRating = Math.Round(ratings.Average(), 1);
                    _dataService.UpdateContent(content);
                }
            }
        }
    }
}
