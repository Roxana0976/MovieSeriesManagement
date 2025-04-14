using System;
using System.Collections.Generic;

namespace MovieSeriesManagement.Models
{
    public enum UserRole
    {
        Regular,
        Administrator
    }

    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } // In a real app, use proper password hashing
        public string Email { get; set; }
        public UserRole Role { get; set; }
        public List<int> WatchedContentIds { get; set; } = new List<int>();
        public Dictionary<int, int> ContentRatings { get; set; } = new Dictionary<int, int>(); // ContentId, Rating
        public Dictionary<int, double> WatchProgress { get; set; } = new Dictionary<int, double>(); // ContentId, Progress (0-1)
        public DateTime RegistrationDate { get; set; }
        public DateTime LastLoginDate { get; set; }

        public User()
        {
            RegistrationDate = DateTime.Now;
            LastLoginDate = DateTime.Now;
        }
    }
}
