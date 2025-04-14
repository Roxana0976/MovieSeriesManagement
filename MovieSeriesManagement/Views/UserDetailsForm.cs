using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MovieSeriesManagement.Controllers;
using MovieSeriesManagement.Models;

namespace MovieSeriesManagement.Views
{
    public partial class UserDetailsForm : Form
    {
        private readonly User _user;
        private readonly ContentController _contentController;

        public UserDetailsForm(User user, ContentController contentController)
        {
            _user = user;
            _contentController = contentController;

            InitializeComponent();

            // Load user details
            LoadUserDetails();
        }

        private void InitializeComponent()
        {
            this.lblUsername = new Label();
            this.lblUsernameValue = new Label();
            this.lblEmail = new Label();
            this.lblEmailValue = new Label();
            this.lblRole = new Label();
            this.lblRoleValue = new Label();
            this.lblRegistered = new Label();
            this.lblRegisteredValue = new Label();
            this.lblLastLogin = new Label();
            this.lblLastLoginValue = new Label();
            this.tabControl = new TabControl();
            this.tabWatchHistory = new TabPage();
            this.tabRatings = new TabPage();
            this.dgvWatchHistory = new DataGridView();
            this.dgvRatings = new DataGridView();
            this.btnClose = new Button();

            // Set up form
            this.Text = "User Details";
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // lblUsername
            this.lblUsername.AutoSize = true;
            this.lblUsername.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblUsername.Location = new Point(20, 20);
            this.lblUsername.Text = "Username:";

            // lblUsernameValue
            this.lblUsernameValue.AutoSize = true;
            this.lblUsernameValue.Font = new Font("Segoe UI", 10F);
            this.lblUsernameValue.Location = new Point(120, 20);

            // lblEmail
            this.lblEmail.AutoSize = true;
            this.lblEmail.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblEmail.Location = new Point(20, 50);
            this.lblEmail.Text = "Email:";

            // lblEmailValue
            this.lblEmailValue.AutoSize = true;
            this.lblEmailValue.Font = new Font("Segoe UI", 10F);
            this.lblEmailValue.Location = new Point(120, 50);

            // lblRole
            this.lblRole.AutoSize = true;
            this.lblRole.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblRole.Location = new Point(20, 80);
            this.lblRole.Text = "Role:";

            // lblRoleValue
            this.lblRoleValue.AutoSize = true;
            this.lblRoleValue.Font = new Font("Segoe UI", 10F);
            this.lblRoleValue.Location = new Point(120, 80);

            // lblRegistered
            this.lblRegistered.AutoSize = true;
            this.lblRegistered.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblRegistered.Location = new Point(350, 20);
            this.lblRegistered.Text = "Registered:";

            // lblRegisteredValue
            this.lblRegisteredValue.AutoSize = true;
            this.lblRegisteredValue.Font = new Font("Segoe UI", 10F);
            this.lblRegisteredValue.Location = new Point(450, 20);

            // lblLastLogin
            this.lblLastLogin.AutoSize = true;
            this.lblLastLogin.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblLastLogin.Location = new Point(350, 50);
            this.lblLastLogin.Text = "Last Login:";

            // lblLastLoginValue
            this.lblLastLoginValue.AutoSize = true;
            this.lblLastLoginValue.Font = new Font("Segoe UI", 10F);
            this.lblLastLoginValue.Location = new Point(450, 50);

            // tabControl
            this.tabControl.Location = new Point(20, 120);
            this.tabControl.Size = new Size(760, 420);

            // tabWatchHistory
            this.tabWatchHistory.Text = "Watch History";
            this.tabWatchHistory.Padding = new Padding(10);

            // dgvWatchHistory
            this.dgvWatchHistory.Dock = DockStyle.Fill;
            this.dgvWatchHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvWatchHistory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvWatchHistory.MultiSelect = false;
            this.dgvWatchHistory.ReadOnly = true;
            this.dgvWatchHistory.RowHeadersVisible = false;

            // tabRatings
            this.tabRatings.Text = "Ratings";
            this.tabRatings.Padding = new Padding(10);

            // dgvRatings
            this.dgvRatings.Dock = DockStyle.Fill;
            this.dgvRatings.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvRatings.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvRatings.MultiSelect = false;
            this.dgvRatings.ReadOnly = true;
            this.dgvRatings.RowHeadersVisible = false;

            // Add data grids to tabs
            this.tabWatchHistory.Controls.Add(this.dgvWatchHistory);
            this.tabRatings.Controls.Add(this.dgvRatings);

            // Add tabs to tabControl
            this.tabControl.TabPages.Add(this.tabWatchHistory);
            this.tabControl.TabPages.Add(this.tabRatings);

            // btnClose
            this.btnClose.Text = "Close";
            this.btnClose.Location = new Point(690, 550);
            this.btnClose.Size = new Size(90, 30);
            this.btnClose.Click += new EventHandler(this.btnClose_Click);

            // Add controls to form
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.lblUsernameValue);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.lblEmailValue);
            this.Controls.Add(this.lblRole);
            this.Controls.Add(this.lblRoleValue);
            this.Controls.Add(this.lblRegistered);
            this.Controls.Add(this.lblRegisteredValue);
            this.Controls.Add(this.lblLastLogin);
            this.Controls.Add(this.lblLastLoginValue);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.btnClose);
        }

        private void LoadUserDetails()
        {
            // Basic user info
            lblUsernameValue.Text = _user.Username;
            lblEmailValue.Text = _user.Email;
            lblRoleValue.Text = _user.Role.ToString();
            lblRegisteredValue.Text = _user.RegistrationDate.ToString("MMM dd, yyyy hh:mm tt");
            lblLastLoginValue.Text = _user.LastLoginDate.ToString("MMM dd, yyyy hh:mm tt");

            // Load watch history
            LoadWatchHistory();

            // Load ratings
            LoadRatings();
        }

        private void LoadWatchHistory()
        {
            var allContent = _contentController.GetAllContent();
            var watchedContent = new List<object>();

            foreach (var contentId in _user.WatchedContentIds)
            {
                var content = allContent.FirstOrDefault(c => c.Id == contentId);
                if (content != null)
                {
                    double progress = 0;
                    _user.WatchProgress.TryGetValue(contentId, out progress);

                    watchedContent.Add(new
                    {
                        ID = content.Id,
                        Title = content.Title,
                        Type = content.Type,
                        Platform = content.Platform,
                        Progress = $"{progress * 100:0}%",
                        Genres = string.Join(", ", content.Genres)
                    });
                }
            }

            dgvWatchHistory.DataSource = watchedContent;
        }

        private void LoadRatings()
        {
            var allContent = _contentController.GetAllContent();
            var ratedContent = new List<object>();

            foreach (var rating in _user.ContentRatings)
            {
                var content = allContent.FirstOrDefault(c => c.Id == rating.Key);
                if (content != null)
                {
                    ratedContent.Add(new
                    {
                        ID = content.Id,
                        Title = content.Title,
                        Type = content.Type,
                        Platform = content.Platform,
                        UserRating = rating.Value,
                        AverageRating = content.AverageRating
                    });
                }
            }

            dgvRatings.DataSource = ratedContent;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private Label lblUsername;
        private Label lblUsernameValue;
        private Label lblEmail;
        private Label lblEmailValue;
        private Label lblRole;
        private Label lblRoleValue;
        private Label lblRegistered;
        private Label lblRegisteredValue;
        private Label lblLastLogin;
        private Label lblLastLoginValue;
        private TabControl tabControl;
        private TabPage tabWatchHistory;
        private TabPage tabRatings;
        private DataGridView dgvWatchHistory;
        private DataGridView dgvRatings;
        private Button btnClose;
    }
}
