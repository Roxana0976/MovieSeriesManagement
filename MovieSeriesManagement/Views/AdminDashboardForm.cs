using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MovieSeriesManagement.Controllers;
using MovieSeriesManagement.Models;

namespace MovieSeriesManagement.Views
{
    public partial class AdminDashboardForm : Form
    {
        private readonly User _adminUser;
        private readonly UserController _userController;
        private readonly ContentController _contentController;
        private List<Content> _currentContentList;
        private List<User> _currentUserList;

        public AdminDashboardForm(User adminUser)
        {
            InitializeComponent();
            _adminUser = adminUser;
            _userController = new UserController();
            _contentController = new ContentController();

            // Set welcome message
            lblWelcome.Text = $"Welcome, Administrator {_adminUser.Username}!";

            // Load initial data
            LoadAllContent();
            LoadAllUsers();
            LoadContentStats();
        }

        private void InitializeComponent()
        {
            this.tabControl = new TabControl();
            this.tabContent = new TabPage();
            this.tabUsers = new TabPage();
            this.tabStats = new TabPage();
            this.lblWelcome = new Label();
            this.panel1 = new Panel();
            this.btnLogout = new Button();
            this.dgvContent = new DataGridView();
            this.btnAddContent = new Button();
            this.btnEditContent = new Button();
            this.btnDeleteContent = new Button();
            this.lblContentSearch = new Label();
            this.txtContentSearch = new TextBox();
            this.btnContentSearch = new Button();
            this.dgvUsers = new DataGridView();
            this.lblUserSearch = new Label();
            this.txtUserSearch = new TextBox();
            this.btnUserSearch = new Button();
            this.btnViewUserDetails = new Button();
            this.lblStats = new Label();
            this.pnlStats = new Panel();

            // Set up form
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Name = "AdminDashboardForm";
            this.Text = "Admin Dashboard - Series & Movies Manager";
            this.StartPosition = FormStartPosition.CenterScreen;

            // panel1 - Header
            this.panel1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panel1.Dock = DockStyle.Top;
            this.panel1.Height = 60;
            this.panel1.Padding = new Padding(10);

            // lblWelcome
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.lblWelcome.Location = new Point(20, 15);
            this.lblWelcome.Text = "Welcome, Administrator!";

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

            // tabContent
            this.tabContent.Text = "Content Management";
            this.tabContent.Padding = new Padding(10);

            // Content Search Controls
            this.lblContentSearch.Text = "Search:";
            this.lblContentSearch.AutoSize = true;
            this.lblContentSearch.Location = new Point(15, 15);

            this.txtContentSearch.Location = new Point(80, 12);
            this.txtContentSearch.Size = new Size(200, 23);
            this.txtContentSearch.KeyDown += new KeyEventHandler(this.txtContentSearch_KeyDown);

            this.btnContentSearch.Text = "Search";
            this.btnContentSearch.Location = new Point(290, 11);
            this.btnContentSearch.Size = new Size(75, 25);
            this.btnContentSearch.Click += new EventHandler(this.btnContentSearch_Click);

            // Content DataGridView
            this.dgvContent.Location = new Point(15, 50);
            this.dgvContent.Size = new Size(970, 550);
            this.dgvContent.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvContent.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvContent.MultiSelect = false;
            this.dgvContent.ReadOnly = true;
            this.dgvContent.RowHeadersVisible = false;
            this.dgvContent.CellDoubleClick += new DataGridViewCellEventHandler(this.dgvContent_CellDoubleClick);

            // Content Management Buttons
            this.btnAddContent.Text = "Add Content";
            this.btnAddContent.Location = new Point(670, 610);
            this.btnAddContent.Size = new Size(100, 30);
            this.btnAddContent.Click += new EventHandler(this.btnAddContent_Click);

            this.btnEditContent.Text = "Edit Content";
            this.btnEditContent.Location = new Point(780, 610);
            this.btnEditContent.Size = new Size(100, 30);
            this.btnEditContent.Click += new EventHandler(this.btnEditContent_Click);

            this.btnDeleteContent.Text = "Delete Content";
            this.btnDeleteContent.Location = new Point(890, 610);
            this.btnDeleteContent.Size = new Size(100, 30);
            this.btnDeleteContent.Click += new EventHandler(this.btnDeleteContent_Click);

            // Add controls to tabContent
            this.tabContent.Controls.Add(this.lblContentSearch);
            this.tabContent.Controls.Add(this.txtContentSearch);
            this.tabContent.Controls.Add(this.btnContentSearch);
            this.tabContent.Controls.Add(this.dgvContent);
            this.tabContent.Controls.Add(this.btnAddContent);
            this.tabContent.Controls.Add(this.btnEditContent);
            this.tabContent.Controls.Add(this.btnDeleteContent);

            // tabUsers
            this.tabUsers.Text = "User Management";
            this.tabUsers.Padding = new Padding(10);

            // User Search Controls
            this.lblUserSearch.Text = "Search:";
            this.lblUserSearch.AutoSize = true;
            this.lblUserSearch.Location = new Point(15, 15);

            this.txtUserSearch.Location = new Point(80, 12);
            this.txtUserSearch.Size = new Size(200, 23);
            this.txtUserSearch.KeyDown += new KeyEventHandler(this.txtUserSearch_KeyDown);

            this.btnUserSearch.Text = "Search";
            this.btnUserSearch.Location = new Point(290, 11);
            this.btnUserSearch.Size = new Size(75, 25);
            this.btnUserSearch.Click += new EventHandler(this.btnUserSearch_Click);

            // Users DataGridView
            this.dgvUsers.Location = new Point(15, 50);
            this.dgvUsers.Size = new Size(970, 550);
            this.dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvUsers.MultiSelect = false;
            this.dgvUsers.ReadOnly = true;
            this.dgvUsers.RowHeadersVisible = false;
            this.dgvUsers.CellDoubleClick += new DataGridViewCellEventHandler(this.dgvUsers_CellDoubleClick);

            // User Management Buttons
            this.btnViewUserDetails.Text = "View User Details";
            this.btnViewUserDetails.Location = new Point(845, 610);
            this.btnViewUserDetails.Size = new Size(140, 30);
            this.btnViewUserDetails.Click += new EventHandler(this.btnViewUserDetails_Click);

            // Add controls to tabUsers
            this.tabUsers.Controls.Add(this.lblUserSearch);
            this.tabUsers.Controls.Add(this.txtUserSearch);
            this.tabUsers.Controls.Add(this.btnUserSearch);
            this.tabUsers.Controls.Add(this.dgvUsers);
            this.tabUsers.Controls.Add(this.btnViewUserDetails);

            // tabStats
            this.tabStats.Text = "Statistics";
            this.tabStats.Padding = new Padding(10);

            // Stats label
            this.lblStats.Text = "System Statistics";
            this.lblStats.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.lblStats.AutoSize = true;
            this.lblStats.Location = new Point(15, 15);

            // Stats panel
            this.pnlStats.Location = new Point(15, 50);
            this.pnlStats.Size = new Size(970, 600);
            this.pnlStats.AutoScroll = true;

            // Add controls to tabStats
            this.tabStats.Controls.Add(this.lblStats);
            this.tabStats.Controls.Add(this.pnlStats);

            // Add tabs to tabControl
            this.tabControl.TabPages.Add(this.tabContent);
            this.tabControl.TabPages.Add(this.tabUsers);
            this.tabControl.TabPages.Add(this.tabStats);
            this.tabControl.SelectedIndexChanged += new EventHandler(this.tabControl_SelectedIndexChanged);

            // Add controls to form
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tabControl);

            this.ResumeLayout(false);
        }

        private void LoadAllContent()
        {
            _currentContentList = _contentController.GetAllContent();
            DisplayContentInGrid(_currentContentList);
        }

        private void LoadAllUsers()
        {
            _currentUserList = _userController.GetAllUsers();
            DisplayUsersInGrid(_currentUserList);
        }

        private void DisplayContentInGrid(List<Content> contentList)
        {
            dgvContent.DataSource = null;

            if (contentList.Count == 0)
            {
                dgvContent.DataSource = null;
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
                Views = c.ViewCount,
                Duration = FormatDuration(c.Duration)
            }).ToList();

            dgvContent.DataSource = displayData;
        }

        private void DisplayUsersInGrid(List<User> userList)
        {
            dgvUsers.DataSource = null;

            if (userList.Count == 0)
            {
                dgvUsers.DataSource = null;
                return;
            }

            var displayData = userList.Select(u => new
            {
                ID = u.Id,
                Username = u.Username,
                Email = u.Email,
                Role = u.Role,
                ContentWatched = u.WatchedContentIds.Count,
                ContentRated = u.ContentRatings.Count,
                Registered = u.RegistrationDate.ToShortDateString(),
                LastLogin = u.LastLoginDate.ToShortDateString()
            }).ToList();

            dgvUsers.DataSource = displayData;
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

        private void SearchContent()
        {
            string searchTerm = txtContentSearch.Text.Trim();

            if (string.IsNullOrEmpty(searchTerm))
            {
                LoadAllContent();
                return;
            }

            _currentContentList = _contentController.SearchContent(searchTerm);
            DisplayContentInGrid(_currentContentList);
        }

        private void SearchUsers()
        {
            string searchTerm = txtUserSearch.Text.Trim();

            if (string.IsNullOrEmpty(searchTerm))
            {
                LoadAllUsers();
                return;
            }

            // Filter users by username or email
            _currentUserList = _userController.GetAllUsers()
                .Where(u => u.Username.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0 ||
                            u.Email.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();

            DisplayUsersInGrid(_currentUserList);
        }

        private void LoadContentStats()
        {
            // Clear existing controls
            pnlStats.Controls.Clear();

            // Content count statistics
            var allMovies = _contentController.GetAllMovies();
            var allSeries = _contentController.GetAllSeries();

            Label lblContentCount = new Label
            {
                Text = $"Total Content: {allMovies.Count + allSeries.Count} (Movies: {allMovies.Count}, Series: {allSeries.Count})",
                AutoSize = true,
                Location = new Point(10, 10),
                Font = new Font("Segoe UI", 12F)
            };

            // User count statistics
            var allUsers = _userController.GetAllUsers();
            var regularUsers = allUsers.Count(u => u.Role == UserRole.Regular);
            var adminUsers = allUsers.Count(u => u.Role == UserRole.Administrator);

            Label lblUserCount = new Label
            {
                Text = $"Total Users: {allUsers.Count} (Regular: {regularUsers}, Admins: {adminUsers})",
                AutoSize = true,
                Location = new Point(10, 40),
                Font = new Font("Segoe UI", 12F)
            };

            // Activity statistics
            int totalWatched = allUsers.Sum(u => u.WatchedContentIds.Count);
            int totalRatings = allUsers.Sum(u => u.ContentRatings.Count);

            Label lblActivityStats = new Label
            {
                Text = $"Total Watch Count: {totalWatched}, Total Ratings: {totalRatings}",
                AutoSize = true,
                Location = new Point(10, 70),
                Font = new Font("Segoe UI", 12F)
            };

            // Genre statistics
            var genreStats = _contentController.GetGenreStatistics();

            Label lblGenreStats = new Label
            {
                Text = "Content by Genre:",
                AutoSize = true,
                Location = new Point(10, 110),
                Font = new Font("Segoe UI", 12F, FontStyle.Bold)
            };

            int yPos = 140;
            foreach (var genre in genreStats.OrderByDescending(g => g.Value))
            {
                Label lblGenre = new Label
                {
                    Text = $"{genre.Key}: {genre.Value} titles",
                    AutoSize = true,
                    Location = new Point(30, yPos),
                    Font = new Font("Segoe UI", 10F)
                };

                pnlStats.Controls.Add(lblGenre);
                yPos += 25;
            }

            // Platform statistics
            var platformStats = _contentController.GetPlatformStatistics();

            Label lblPlatformStats = new Label
            {
                Text = "Content by Platform:",
                AutoSize = true,
                Location = new Point(300, 110),
                Font = new Font("Segoe UI", 12F, FontStyle.Bold)
            };

            yPos = 140;
            foreach (var platform in platformStats.OrderByDescending(p => p.Value))
            {
                Label lblPlatform = new Label
                {
                    Text = $"{platform.Key}: {platform.Value} titles",
                    AutoSize = true,
                    Location = new Point(320, yPos),
                    Font = new Font("Segoe UI", 10F)
                };

                pnlStats.Controls.Add(lblPlatform);
                yPos += 25;
            }

            // Recent registrations
            Label lblRecentRegistrations = new Label
            {
                Text = "Recent User Registrations:",
                AutoSize = true,
                Location = new Point(600, 110),
                Font = new Font("Segoe UI", 12F, FontStyle.Bold)
            };

            var recentUsers = allUsers
                .Where(u => u.Role == UserRole.Regular)
                .OrderByDescending(u => u.RegistrationDate)
                .Take(10)
                .ToList();

            yPos = 140;
            foreach (var user in recentUsers)
            {
                Label lblUser = new Label
                {
                    Text = $"{user.Username} - {user.RegistrationDate.ToShortDateString()}",
                    AutoSize = true,
                    Location = new Point(620, yPos),
                    Font = new Font("Segoe UI", 10F)
                };

                pnlStats.Controls.Add(lblUser);
                yPos += 25;
            }

            // Add labels to panel
            pnlStats.Controls.Add(lblContentCount);
            pnlStats.Controls.Add(lblUserCount);
            pnlStats.Controls.Add(lblActivityStats);
            pnlStats.Controls.Add(lblGenreStats);
            pnlStats.Controls.Add(lblPlatformStats);
            pnlStats.Controls.Add(lblRecentRegistrations);
        }

        private void btnContentSearch_Click(object sender, EventArgs e)
        {
            SearchContent();
        }

        private void txtContentSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchContent();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void btnUserSearch_Click(object sender, EventArgs e)
        {
            SearchUsers();
        }

        private void txtUserSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchUsers();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void btnAddContent_Click(object sender, EventArgs e)
        {
            // Open content add form
            ContentEditForm addForm = new ContentEditForm(null, _contentController);
            if (addForm.ShowDialog() == DialogResult.OK)
            {
                // Refresh content list
                LoadAllContent();
            }
        }

        private void btnEditContent_Click(object sender, EventArgs e)
        {
            if (dgvContent.SelectedRows.Count == 0) return;

            int contentId = (int)dgvContent.SelectedRows[0].Cells["ID"].Value;
            Content selectedContent = _contentController.GetContentById(contentId);

            if (selectedContent != null)
            {
                ContentEditForm editForm = new ContentEditForm(selectedContent, _contentController);
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    // Refresh content list
                    LoadAllContent();
                }
            }
        }

        private void btnDeleteContent_Click(object sender, EventArgs e)
        {
            if (dgvContent.SelectedRows.Count == 0) return;

            int contentId = (int)dgvContent.SelectedRows[0].Cells["ID"].Value;
            string contentTitle = dgvContent.SelectedRows[0].Cells["Title"].Value.ToString();

            // Confirm deletion
            if (MessageBox.Show($"Are you sure you want to delete '{contentTitle}'?",
                               "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                _contentController.DeleteContent(contentId);

                // Refresh content list
                LoadAllContent();
            }
        }

        private void btnViewUserDetails_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count == 0) return;

            int userId = (int)dgvUsers.SelectedRows[0].Cells["ID"].Value;
            User selectedUser = _userController.GetAllUsers().FirstOrDefault(u => u.Id == userId);

            if (selectedUser != null)
            {
                UserDetailsForm detailsForm = new UserDetailsForm(selectedUser, _contentController);
                detailsForm.ShowDialog();
            }
        }

        private void dgvContent_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                btnEditContent_Click(sender, e);
            }
        }

        private void dgvUsers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                btnViewUserDetails_Click(sender, e);
            }
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == tabContent)
            {
                LoadAllContent();
            }
            else if (tabControl.SelectedTab == tabUsers)
            {
                LoadAllUsers();
            }
            else if (tabControl.SelectedTab == tabStats)
            {
                LoadContentStats();
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private TabControl tabControl;
        private TabPage tabContent;
        private TabPage tabUsers;
        private TabPage tabStats;
        private Label lblWelcome;
        private Panel panel1;
        private Button btnLogout;
        private DataGridView dgvContent;
        private Button btnAddContent;
        private Button btnEditContent;
        private Button btnDeleteContent;
        private Label lblContentSearch;
        private TextBox txtContentSearch;
        private Button btnContentSearch;
        private DataGridView dgvUsers;
        private Label lblUserSearch;
        private TextBox txtUserSearch;
        private Button btnUserSearch;
        private Button btnViewUserDetails;
        private Label lblStats;
        private Panel pnlStats;
    }
}
