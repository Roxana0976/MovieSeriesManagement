using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MovieSeriesManagement.Controllers;
using MovieSeriesManagement.Models;

namespace MovieSeriesManagement.Views
{
    public partial class UserDashboardForm : Form
    {
        private readonly User _currentUser;
        private readonly ContentController _contentController;
        private readonly UserController _userController;
        private List<Content> _currentContentList;

        public UserDashboardForm(User user)
        {
            InitializeComponent();
            _currentUser = user;
            _contentController = new ContentController();
            _userController = new UserController();

            // Set welcome message
            lblWelcome.Text = $"Welcome, {_currentUser.Username}!";

            // Load genres for filter
            LoadGenreFilters();

            // Load platforms for filter
            LoadPlatformFilters();

            // Load content
            LoadAllContent();

            // Load recommendations
            LoadRecommendations();
        }

        private void InitializeComponent()
        {
            this.tabControl = new TabControl();
            this.tabAllContent = new TabPage();
            this.tabRecommendations = new TabPage();
            this.tabWatchHistory = new TabPage();
            this.tabStats = new TabPage();
            this.lblWelcome = new Label();
            this.lblSearch = new Label();
            this.txtSearch = new TextBox();
            this.btnSearch = new Button();
            this.lblGenreFilter = new Label();
            this.cmbGenre = new ComboBox();
            this.lblPlatformFilter = new Label();
            this.cmbPlatform = new ComboBox();
            this.rbAllContent = new RadioButton();
            this.rbMoviesOnly = new RadioButton();
            this.rbSeriesOnly = new RadioButton();
            this.dgvContent = new DataGridView();
            this.btnViewDetails = new Button();
            this.dgvRecommendations = new DataGridView();
            this.dgvWatchHistory = new DataGridView();
            this.lblStats = new Label();
            this.panel1 = new Panel();
            this.btnLogout = new Button();

            // Set up form
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Name = "UserDashboardForm";
            this.Text = "User Dashboard - Series & Movies Manager";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormClosed += new FormClosedEventHandler(this.UserDashboardForm_FormClosed);

            // panel1 - Header
            this.panel1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panel1.Dock = DockStyle.Top;
            this.panel1.Height = 60;
            this.panel1.Padding = new Padding(10);

            // lblWelcome
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.lblWelcome.Location = new Point(20, 15);
            this.lblWelcome.Text = "Welcome, User!";

            // btnLogout
            this.btnLogout.Text = "Logout";
            this.btnLogout.Location = new Point(900, 15);
            this.btnLogout.Size = new Size(100, 30);
            this.btnLogout.Click += new EventHandler(this.btnLogout_Click);

            // Add controls to panel
            this.panel1.Controls.Add(this.lblWelcome);
            this.panel1.Controls.Add(this.btnLogout);

            // tabControl
            this.tabControl.Dock = DockStyle.Fill;
            this.tabControl.Location = new Point(0, 60);
            this.tabControl.Size = new Size(1024, 708);

            // tabAllContent
            this.tabAllContent.Text = "Browse Content";
            this.tabAllContent.Padding = new Padding(10);

            // Search controls
            this.lblSearch.Text = "Search:";
            this.lblSearch.AutoSize = true;
            this.lblSearch.Location = new Point(15, 15);

            this.txtSearch.Location = new Point(80, 12);
            this.txtSearch.Size = new Size(200, 23);
            this.txtSearch.KeyDown += new KeyEventHandler(this.txtSearch_KeyDown);

            this.btnSearch.Text = "Search";
            this.btnSearch.Location = new Point(290, 11);
            this.btnSearch.Size = new Size(75, 25);
            this.btnSearch.Click += new EventHandler(this.btnSearch_Click);

            // Filter controls
            this.lblGenreFilter.Text = "Genre:";
            this.lblGenreFilter.AutoSize = true;
            this.lblGenreFilter.Location = new Point(380, 15);

            this.cmbGenre.Location = new Point(430, 12);
            this.cmbGenre.Size = new Size(150, 23);
            this.cmbGenre.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbGenre.SelectedIndexChanged += new EventHandler(this.cmbGenre_SelectedIndexChanged);

            this.lblPlatformFilter.Text = "Platform:";
            this.lblPlatformFilter.AutoSize = true;
            this.lblPlatformFilter.Location = new Point(590, 15);

            this.cmbPlatform.Location = new Point(650, 12);
            this.cmbPlatform.Size = new Size(150, 23);
            this.cmbPlatform.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbPlatform.SelectedIndexChanged += new EventHandler(this.cmbPlatform_SelectedIndexChanged);

            // Type filter radio buttons
            this.rbAllContent.Text = "All";
            this.rbAllContent.AutoSize = true;
            this.rbAllContent.Location = new Point(820, 5);
            this.rbAllContent.Checked = true;
            this.rbAllContent.CheckedChanged += new EventHandler(this.rbTypeFilter_CheckedChanged);

            this.rbMoviesOnly.Text = "Movies";
            this.rbMoviesOnly.AutoSize = true;
            this.rbMoviesOnly.Location = new Point(870, 5);
            this.rbMoviesOnly.CheckedChanged += new EventHandler(this.rbTypeFilter_CheckedChanged);

            this.rbSeriesOnly.Text = "Series";
            this.rbSeriesOnly.AutoSize = true;
            this.rbSeriesOnly.Location = new Point(930, 5);
            this.rbSeriesOnly.CheckedChanged += new EventHandler(this.rbTypeFilter_CheckedChanged);

            // Content DataGridView
            this.dgvContent.Location = new Point(15, 50);
            this.dgvContent.Size = new Size(970, 550);
            this.dgvContent.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvContent.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvContent.MultiSelect = false;
            this.dgvContent.ReadOnly = true;
            this.dgvContent.RowHeadersVisible = false;
            this.dgvContent.CellDoubleClick += new DataGridViewCellEventHandler(this.dgvContent_CellDoubleClick);

            // View Details button
            this.btnViewDetails.Text = "View Details";
            this.btnViewDetails.Location = new Point(870, 610);
            this.btnViewDetails.Size = new Size(115, 30);
            this.btnViewDetails.Click += new EventHandler(this.btnViewDetails_Click);

            // Add controls to tabAllContent
            this.tabAllContent.Controls.Add(this.lblSearch);
            this.tabAllContent.Controls.Add(this.txtSearch);
            this.tabAllContent.Controls.Add(this.btnSearch);
            this.tabAllContent.Controls.Add(this.lblGenreFilter);
            this.tabAllContent.Controls.Add(this.cmbGenre);
            this.tabAllContent.Controls.Add(this.lblPlatformFilter);
            this.tabAllContent.Controls.Add(this.cmbPlatform);
            this.tabAllContent.Controls.Add(this.rbAllContent);
            this.tabAllContent.Controls.Add(this.rbMoviesOnly);
            this.tabAllContent.Controls.Add(this.rbSeriesOnly);
            this.tabAllContent.Controls.Add(this.dgvContent);
            this.tabAllContent.Controls.Add(this.btnViewDetails);

            // tabRecommendations
            this.tabRecommendations.Text = "Recommendations";
            this.tabRecommendations.Padding = new Padding(10);

            // Recommendations DataGridView
            this.dgvRecommendations.Location = new Point(15, 15);
            this.dgvRecommendations.Size = new Size(970, 600);
            this.dgvRecommendations.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvRecommendations.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvRecommendations.MultiSelect = false;
            this.dgvRecommendations.ReadOnly = true;
            this.dgvRecommendations.RowHeadersVisible = false;
            this.dgvRecommendations.CellDoubleClick += new DataGridViewCellEventHandler(this.dgvRecommendations_CellDoubleClick);

            // Add controls to tabRecommendations
            this.tabRecommendations.Controls.Add(this.dgvRecommendations);

            // tabWatchHistory
            this.tabWatchHistory.Text = "Watch History";
            this.tabWatchHistory.Padding = new Padding(10);

            // Watch History DataGridView
            this.dgvWatchHistory.Location = new Point(15, 15);
            this.dgvWatchHistory.Size = new Size(970, 600);
            this.dgvWatchHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvWatchHistory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvWatchHistory.MultiSelect = false;
            this.dgvWatchHistory.ReadOnly = true;
            this.dgvWatchHistory.RowHeadersVisible = false;
            this.dgvWatchHistory.CellDoubleClick += new DataGridViewCellEventHandler(this.dgvWatchHistory_CellDoubleClick);

            // Add controls to tabWatchHistory
            this.tabWatchHistory.Controls.Add(this.dgvWatchHistory);

            // tabStats
            this.tabStats.Text = "Statistics";
            this.tabStats.Padding = new Padding(10);

            // Stats label
            this.lblStats.Text = "Your Viewing Statistics";
            this.lblStats.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.lblStats.AutoSize = true;
            this.lblStats.Location = new Point(15, 15);

            // Add controls to tabStats
            this.tabStats.Controls.Add(this.lblStats);

            // Add tabs to tabControl
            this.tabControl.TabPages.Add(this.tabAllContent);
            this.tabControl.TabPages.Add(this.tabRecommendations);
            this.tabControl.TabPages.Add(this.tabWatchHistory);
            this.tabControl.TabPages.Add(this.tabStats);
            this.tabControl.SelectedIndexChanged += new EventHandler(this.tabControl_SelectedIndexChanged);

            // Add controls to form
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tabControl);

            this.ResumeLayout(false);
        }

        private void LoadGenreFilters()
        {
            var genres = _contentController.GetAllGenres();
            cmbGenre.Items.Clear();
            cmbGenre.Items.Add("All Genres");
            foreach (var genre in genres)
            {
                cmbGenre.Items.Add(genre);
            }
            cmbGenre.SelectedIndex = 0;
        }

        private void LoadPlatformFilters()
        {
            var platforms = _contentController.GetAllPlatforms();
            cmbPlatform.Items.Clear();
            cmbPlatform.Items.Add("All Platforms");
            foreach (var platform in platforms)
            {
                cmbPlatform.Items.Add(platform);
            }
            cmbPlatform.SelectedIndex = 0;
        }

        private void LoadAllContent()
        {
            _currentContentList = _contentController.GetAllContent();
            DisplayContentInGrid(_currentContentList, dgvContent);
        }

        private void LoadRecommendations()
        {
            var recommendations = _contentController.GetRecommendedContent(_currentUser.Id);
            DisplayContentInGrid(recommendations, dgvRecommendations);
        }

        private void LoadWatchHistory()
        {
            var allContent = _contentController.GetAllContent();
            var watchedContent = new List<Content>();

            foreach (var contentId in _currentUser.WatchedContentIds)
            {
                var content = allContent.FirstOrDefault(c => c.Id == contentId);
                if (content != null)
                {
                    watchedContent.Add(content);
                }
            }

            DisplayContentInGrid(watchedContent, dgvWatchHistory);
        }

        private void DisplayContentInGrid(List<Content> contentList, DataGridView grid)
        {
            grid.DataSource = null;

            if (contentList.Count == 0)
            {
                grid.DataSource = null;
                return;
            }

            var displayData = contentList.Select(c => new
            {
                ID = c.Id,
                Title = c.Title,
                Type = c.Type,
                Genres = string.Join(", ", c.Genres),
                Platform = c.Platform,
                Year = c.ReleaseYear,
                Rating = c.AverageRating,
                Duration = FormatDuration(c.Duration)
            }).ToList();

            grid.DataSource = displayData;

            if (grid.Columns.Contains("ID"))
            {
                grid.Columns["ID"].Visible = false;
            }
        }

        private string FormatDuration(int minutes)
        {
            int hours = minutes / 60;
            int mins = minutes % 60;

            if (hours > 0)
            {
                return $"{hours}h {mins}m";
            }
            else
            {
                return $"{mins}m";
            }
        }

        private void FilterContent()
        {
            string searchTerm = txtSearch.Text.Trim();

            // Get genre filter
            string genreFilter = null;
            if (cmbGenre.SelectedIndex > 0) // Not "All Genres"
            {
                genreFilter = cmbGenre.SelectedItem.ToString();
            }

            // Get platform filter
            string platformFilter = null;
            if (cmbPlatform.SelectedIndex > 0) // Not "All Platforms"
            {
                platformFilter = cmbPlatform.SelectedItem.ToString();
            }

            // Get type filter
            ContentType? typeFilter = null;
            if (rbMoviesOnly.Checked)
            {
                typeFilter = ContentType.Movie;
            }
            else if (rbSeriesOnly.Checked)
            {
                typeFilter = ContentType.Series;
            }

            // Apply filters
            List<string> genresList = genreFilter != null ? new List<string> { genreFilter } : null;
            _currentContentList = _contentController.SearchContent(searchTerm, genresList, platformFilter, typeFilter);
            DisplayContentInGrid(_currentContentList, dgvContent);
        }

        private void ViewContentDetails(DataGridView grid)
        {
            if (grid.SelectedRows.Count == 0) return;

            int contentId = (int)grid.SelectedRows[0].Cells["ID"].Value;
            Content selectedContent = _contentController.GetContentById(contentId);

            if (selectedContent != null)
            {
                ContentDetailsForm detailsForm = new ContentDetailsForm(selectedContent, _currentUser, _userController, _contentController);
                detailsForm.ShowDialog();

                // Refresh data after returning from details form
                if (tabControl.SelectedTab == tabWatchHistory)
                {
                    LoadWatchHistory();
                }
                else if (tabControl.SelectedTab == tabRecommendations)
                {
                    LoadRecommendations();
                }
                else
                {
                    // Keep current filters
                    FilterContent();
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            FilterContent();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                FilterContent();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void cmbGenre_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterContent();
        }

        private void cmbPlatform_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterContent();
        }

        private void rbTypeFilter_CheckedChanged(object sender, EventArgs e)
        {
            FilterContent();
        }

        private void btnViewDetails_Click(object sender, EventArgs e)
        {
            ViewContentDetails(dgvContent);
        }

        private void dgvContent_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                ViewContentDetails(dgvContent);
            }
        }

        private void dgvRecommendations_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                ViewContentDetails(dgvRecommendations);
            }
        }

        private void dgvWatchHistory_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                ViewContentDetails(dgvWatchHistory);
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == tabWatchHistory)
            {
                LoadWatchHistory();
            }
            else if (tabControl.SelectedTab == tabRecommendations)
            {
                LoadRecommendations();
            }
            else if (tabControl.SelectedTab == tabStats)
            {
                LoadUserStats();
            }
        }

        private void LoadUserStats()
        {
            // Clear existing controls
            foreach (Control ctrl in tabStats.Controls)
            {
                if (ctrl != lblStats)
                {
                    ctrl.Dispose();
                }
            }

            // Load watched content
            var watchedContent = new List<Content>();
            var allContent = _contentController.GetAllContent();

            foreach (var contentId in _currentUser.WatchedContentIds)
            {
                var content = allContent.FirstOrDefault(c => c.Id == contentId);
                if (content != null)
                {
                    watchedContent.Add(content);
                }
            }

            // Calculate total watch time
            int totalMinutes = watchedContent.Sum(c => c.Duration);
            int hours = totalMinutes / 60;
            int minutes = totalMinutes % 60;

            // Create stats labels
            Label lblWatchTime = new Label
            {
                Text = $"Total Watch Time: {hours} hours and {minutes} minutes",
                AutoSize = true,
                Location = new Point(20, 60),
                Font = new Font("Segoe UI", 12F)
            };

            Label lblWatchedCount = new Label
            {
                Text = $"Total Titles Watched: {watchedContent.Count}",
                AutoSize = true,
                Location = new Point(20, 90),
                Font = new Font("Segoe UI", 12F)
            };

            Label lblMoviesWatched = new Label
            {
                Text = $"Movies Watched: {watchedContent.Count(c => c.Type == ContentType.Movie)}",
                AutoSize = true,
                Location = new Point(20, 120),
                Font = new Font("Segoe UI", 12F)
            };

            Label lblSeriesWatched = new Label
            {
                Text = $"Series Watched: {watchedContent.Count(c => c.Type == ContentType.Series)}",
                AutoSize = true,
                Location = new Point(20, 150),
                Font = new Font("Segoe UI", 12F)
            };

            // Calculate favorite genres
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

            var favoriteGenres = genreCounts.OrderByDescending(kv => kv.Value).  = 1;
        }
            }
            
            var favoriteGenres = genreCounts.OrderByDescending(kv => kv.Value).Take(3).ToList();

        Label lblFavoriteGenres = new Label
        {
            Text = "Favorite Genres: " + (favoriteGenres.Count > 0
                ? string.Join(", ", favoriteGenres.Select(g => g.Key))
                : "None yet"),
            AutoSize = true,
            Location = new Point(20, 180),
            Font = new Font("Segoe UI", 12F)
        };

        // Calculate favorite platforms
        var platformCounts = new Dictionary<string, int>();
            foreach (var content in watchedContent)
            {
                if (platformCounts.ContainsKey(content.Platform))
                    platformCounts[content.Platform]++;
                else
                    platformCounts[content.Platform] = 1;
            }

var favoritePlatforms = platformCounts.OrderByDescending(kv => kv.Value).Take(3).ToList();

Label lblFavoritePlatforms = new Label
{
    Text = "Favorite Platforms: " + (favoritePlatforms.Count > 0
        ? string.Join(", ", favoritePlatforms.Select(p => p.Key))
        : "None yet"),
    AutoSize = true,
    Location = new Point(20, 210),
    Font = new Font("Segoe UI", 12F)
};

// Add controls to tab
tabStats.Controls.Add(lblWatchTime);
tabStats.Controls.Add(lblWatchedCount);
tabStats.Controls.Add(lblMoviesWatched);
tabStats.Controls.Add(lblSeriesWatched);
tabStats.Controls.Add(lblFavoriteGenres);
tabStats.Controls.Add(lblFavoritePlatforms);
        }
        
        private void UserDashboardForm_FormClosed(object sender, FormClosedEventArgs e)
{
    // Form is closing, no need to handle
}

private TabControl tabControl;
private TabPage tabAllContent;
private TabPage tabRecommendations;
private TabPage tabWatchHistory;
private TabPage tabStats;
private Label lblWelcome;
private Label lblSearch;
private TextBox txtSearch;
private Button btnSearch;
private Label lblGenreFilter;
private ComboBox cmbGenre;
private Label lblPlatformFilter;
private ComboBox cmbPlatform;
private RadioButton rbAllContent;
private RadioButton rbMoviesOnly;
private RadioButton rbSeriesOnly;
private DataGridView dgvContent;
private Button btnViewDetails;
private DataGridView dgvRecommendations;
private DataGridView dgvWatchHistory;
private Label lblStats;
private Panel panel1;
private Button btnLogout;
    }
}
