using System;
using System.Windows.Forms;
using MovieSeriesManagement.Controllers;
using MovieSeriesManagement.Models;

namespace MovieSeriesManagement.Views
{
    public partial class ContentDetailsForm : Form
    {
        private readonly Content _content;
        private readonly User _currentUser;
        private readonly UserController _userController;
        private readonly ContentController _contentController;
        private double _userProgress;

        public ContentDetailsForm(Content content, User currentUser, UserController userController, ContentController contentController)
        {
            _content = content;
            _currentUser = currentUser;
            _userController = userController;
            _contentController = contentController;

            // Get user progress for this content
            _currentUser.WatchProgress.TryGetValue(_content.Id, out _userProgress);

            InitializeComponent();

            // Load content details
            LoadContentDetails();
        }

        private void InitializeComponent()
        {
            this.lblTitle = new Label();
            this.lblDescription = new Label();
            this.txtDescription = new TextBox();
            this.lblType = new Label();
            this.lblTypeValue = new Label();
            this.lblGenres = new Label();
            this.lblGenresValue = new Label();
            this.lblPlatform = new Label();
            this.lblPlatformValue = new Label();
            this.lblYear = new Label();
            this.lblYearValue = new Label();
            this.lblRating = new Label();
            this.lblRatingValue = new Label();
            this.lblDuration = new Label();
            this.lblDurationValue = new Label();
            this.gbRating = new GroupBox();
            this.rbRating1 = new RadioButton();
            this.rbRating2 = new RadioButton();
            this.rbRating3 = new RadioButton();
            this.rbRating4 = new RadioButton();
            this.rbRating5 = new RadioButton();
            this.rbRating6 = new RadioButton();
            this.rbRating7 = new RadioButton();
            this.rbRating8 = new RadioButton();
            this.rbRating9 = new RadioButton();
            this.rbRating10 = new RadioButton();
            this.btnSubmitRating = new Button();
            this.lblProgress = new Label();
            this.tbProgress = new TrackBar();
            this.lblProgressValue = new Label();
            this.btnMarkAsWatched = new Button();
            this.btnClose = new Button();
            this.pnlSeasons = new Panel();
            this.lblSeasons = new Label();

            // Set up form
            this.ClientSize = new System.Drawing.Size(750, 600);
            this.Name = "ContentDetailsForm";
            this.Text = "Content Details - Series & Movies Manager";
            this.StartPosition = FormStartPosition.CenterParent;

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.MaximumSize = new System.Drawing.Size(700, 0);

            // lblType
            this.lblType.AutoSize = true;
            this.lblType.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblType.Location = new System.Drawing.Point(25, 70);
            this.lblType.Text = "Type:";

            // lblTypeValue
            this.lblTypeValue.AutoSize = true;
            this.lblTypeValue.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblTypeValue.Location = new System.Drawing.Point(120, 70);

            // lblGenres
            this.lblGenres.AutoSize = true;
            this.lblGenres.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblGenres.Location = new System.Drawing.Point(25, 100);
            this.lblGenres.Text = "Genres:";

            // lblGenresValue
            this.lblGenresValue.AutoSize = true;
            this.lblGenresValue.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblGenresValue.Location = new System.Drawing.Point(120, 100);
            this.lblGenresValue.MaximumSize = new System.Drawing.Size(600, 0);

            // lblPlatform
            this.lblPlatform.AutoSize = true;
            this.lblPlatform.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblPlatform.Location = new System.Drawing.Point(25, 130);
            this.lblPlatform.Text = "Platform:";

            // lblPlatformValue
            this.lblPlatformValue.AutoSize = true;
            this.lblPlatformValue.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblPlatformValue.Location = new System.Drawing.Point(120, 130);

            // lblYear
            this.lblYear.AutoSize = true;
            this.lblYear.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblYear.Location = new System.Drawing.Point(25, 160);
            this.lblYear.Text = "Year:";

            // lblYearValue
            this.lblYearValue.AutoSize = true;
            this.lblYearValue.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblYearValue.Location = new System.Drawing.Point(120, 160);

            // lblRating
            this.lblRating.AutoSize = true;
            this.lblRating.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblRating.Location = new System.Drawing.Point(25, 190);
            this.lblRating.Text = "Rating:";

            // lblRatingValue
            this.lblRatingValue.AutoSize = true;
            this.lblRatingValue.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblRatingValue.Location = new System.Drawing.Point(120, 190);

            // lblDuration
            this.lblDuration.AutoSize = true;
            this.lblDuration.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblDuration.Location = new System.Drawing.Point(25, 220);
            this.lblDuration.Text = "Duration:";

            // lblDurationValue
            this.lblDurationValue.AutoSize = true;
            this.lblDurationValue.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblDurationValue.Location = new System.Drawing.Point(120, 220);

            // lblDescription
            this.lblDescription.AutoSize = true;
            this.lblDescription.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblDescription.Location = new System.Drawing.Point(25, 250);
            this.lblDescription.Text = "Description:";

            // txtDescription
            this.txtDescription.Location = new System.Drawing.Point(120, 250);
            this.txtDescription.Multiline = true;
            this.txtDescription.ReadOnly = true;
            this.txtDescription.Size = new System.Drawing.Size(600, 60);
            this.txtDescription.ScrollBars = ScrollBars.Vertical;

            // gbRating
            this.gbRating.Text = "Rate this content";
            this.gbRating.Location = new System.Drawing.Point(25, 330);
            this.gbRating.Size = new System.Drawing.Size(700, 60);

            // Rating radio buttons
            this.rbRating1.Text = "1";
            this.rbRating1.AutoSize = true;
            this.rbRating1.Location = new System.Drawing.Point(40, 25);
            this.rbRating1.Tag = 1;

            this.rbRating2.Text = "2";
            this.rbRating2.AutoSize = true;
            this.rbRating2.Location = new System.Drawing.Point(100, 25);
            this.rbRating2.Tag = 2;

            this.rbRating3.Text = "3";
            this.rbRating3.AutoSize = true;
            this.rbRating3.Location = new System.Drawing.Point(160, 25);
            this.rbRating3.Tag = 3;

            this.rbRating4.Text = "4";
            this.rbRating4.AutoSize = true;
            this.rbRating4.Location = new System.Drawing.Point(220, 25);
            this.rbRating4.Tag = 4;

            this.rbRating5.Text = "5";
            this.rbRating5.AutoSize = true;
            this.rbRating5.Location = new System.Drawing.Point(280, 25);
            this.rbRating5.Tag = 5;

            this.rbRating6.Text = "6";
            this.rbRating6.AutoSize = true;
            this.rbRating6.Location = new System.Drawing.Point(340, 25);
            this.rbRating6.Tag = 6;

            this.rbRating7.Text = "7";
            this.rbRating7.AutoSize = true;
            this.rbRating7.Location = new System.Drawing.Point(400, 25);
            this.rbRating7.Tag = 7;

            this.rbRating8.Text = "8";
            this.rbRating8.AutoSize = true;
            this.rbRating8.Location = new System.Drawing.Point(460, 25);
            this.rbRating8.Tag = 8;

            this.rbRating9.Text = "9";
            this.rbRating9.AutoSize = true;
            this.rbRating9.Location = new System.Drawing.Point(520, 25);
            this.rbRating9.Tag = 9;

            this.rbRating10.Text = "10";
            this.rbRating10.AutoSize = true;
            this.rbRating10.Location = new System.Drawing.Point(580, 25);
            this.rbRating10.Tag = 10;

            // Add rating radio buttons to groupbox
            this.gbRating.Controls.Add(this.rbRating1);
            this.gbRating.Controls.Add(this.rbRating2);
            this.gbRating.Controls.Add(this.rbRating3);
            this.gbRating.Controls.Add(this.rbRating4);
            this.gbRating.Controls.Add(this.rbRating5);
            this.gbRating.Controls.Add(this.rbRating6);
            this.gbRating.Controls.Add(this.rbRating7);
            this.gbRating.Controls.Add(this.rbRating8);
            this.gbRating.Controls.Add(this.rbRating9);
            this.gbRating.Controls.Add(this.rbRating10);

            // btnSubmitRating
            this.btnSubmitRating.Text = "Submit Rating";
            this.btnSubmitRating.Location = new System.Drawing.Point(580, 400);
            this.btnSubmitRating.Size = new System.Drawing.Size(140, 30);
            this.btnSubmitRating.Click += new EventHandler(this.btnSubmitRating_Click);

            // lblProgress
            this.lblProgress.AutoSize = true;
            this.lblProgress.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblProgress.Location = new System.Drawing.Point(25, 400);
            this.lblProgress.Text = "Your Progress:";

            // tbProgress
            this.tbProgress.Location = new System.Drawing.Point(150, 400);
            this.tbProgress.Size = new System.Drawing.Size(300, 45);
            this.tbProgress.Maximum = 100;
            this.tbProgress.TickFrequency = 10;
            this.tbProgress.Value = (int)(_userProgress * 100);
            this.tbProgress.ValueChanged += new EventHandler(this.tbProgress_ValueChanged);

            // lblProgressValue
            this.lblProgressValue.AutoSize = true;
            this.lblProgressValue.Location = new System.Drawing.Point(460, 405);
            this.lblProgressValue.Text = $"{(int)(_userProgress * 100)}%";

            // btnMarkAsWatched
            this.btnMarkAsWatched.Text = "Mark as Watched";
            this.btnMarkAsWatched.Location = new System.Drawing.Point(580, 450);
            this.btnMarkAsWatched.Size = new System.Drawing.Size(140, 30);
            this.btnMarkAsWatched.Click += new EventHandler(this.btnMarkAsWatched_Click);

            // lblSeasons (for Series only)
            this.lblSeasons.AutoSize = true;
            this.lblSeasons.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblSeasons.Location = new System.Drawing.Point(25, 450);
            this.lblSeasons.Text = "Seasons:";

            // pnlSeasons (for Series only)
            this.pnlSeasons.Location = new System.Drawing.Point(25, 480);
            this.pnlSeasons.Size = new System.Drawing.Size(540, 100);
            this.pnlSeasons.AutoScroll = true;

            // btnClose
            this.btnClose.Text = "Close";
            this.btnClose.Location = new System.Drawing.Point(580, 520);
            this.btnClose.Size = new System.Drawing.Size(140, 30);
            this.btnClose.Click += new EventHandler(this.btnClose_Click);

            // Add controls to form
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblType);
            this.Controls.Add(this.lblTypeValue);
            this.Controls.Add(this.lblGenres);
            this.Controls.Add(this.lblGenresValue);
            this.Controls.Add(this.lblPlatform);
            this.Controls.Add(this.lblPlatformValue);
            this.Controls.Add(this.lblYear);
            this.Controls.Add(this.lblYearValue);
            this.Controls.Add(this.lblRating);
            this.Controls.Add(this.lblRatingValue);
            this.Controls.Add(this.lblDuration);
            this.Controls.Add(this.lblDurationValue);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.gbRating);
            this.Controls.Add(this.btnSubmitRating);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.tbProgress);
            this.Controls.Add(this.lblProgressValue);
            this.Controls.Add(this.btnMarkAsWatched);
            this.Controls.Add(this.btnClose);
        }

        private void LoadContentDetails()
        {
            // Set content details
            this.lblTitle.Text = _content.Title;
            this.lblTypeValue.Text = _content.Type.ToString();
            this.lblGenresValue.Text = string.Join(", ", _content.Genres);
            this.lblPlatformValue.Text = _content.Platform;
            this.lblYearValue.Text = _content.ReleaseYear.ToString();
            this.lblRatingValue.Text = $"{_content.AverageRating}/10 ({_content.ViewCount} views)";
            this.lblDurationValue.Text = FormatDuration(_content.Duration);
            this.txtDescription.Text = _content.Description;

            // Set user's rating if they have rated this content
            if (_currentUser.ContentRatings.TryGetValue(_content.Id, out int userRating))
            {
                switch (userRating)
                {
                    case 1: rbRating1.Checked = true; break;
                    case 2: rbRating2.Checked = true; break;
                    case 3: rbRating3.Checked = true; break;
                    case 4: rbRating4.Checked = true; break;
                    case 5: rbRating5.Checked = true; break;
                    case 6: rbRating6.Checked = true; break;
                    case 7: rbRating7.Checked = true; break;
                    case 8: rbRating8.Checked = true; break;
                    case 9: rbRating9.Checked = true; break;
                    case 10: rbRating10.Checked = true; break;
                }
            }

            // For series, show seasons and episodes
            if (_content.Type == ContentType.Series && _content.Seasons.Count > 0)
            {
                this.Controls.Add(this.lblSeasons);
                this.Controls.Add(this.pnlSeasons);

                int yOffset = 0;

                foreach (var season in _content.Seasons)
                {
                    Label lblSeasonTitle = new Label
                    {
                        Text = season.Title,
                        Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold),
                        AutoSize = true,
                        Location = new System.Drawing.Point(0, yOffset)
                    };

                    pnlSeasons.Controls.Add(lblSeasonTitle);
                    yOffset += 25;

                    foreach (var episode in season.Episodes)
                    {
                        Label lblEpisode = new Label
                        {
                            Text = $"Episode {episode.Number}: {episode.Title} ({episode.Duration} mins)",
                            AutoSize = true,
                            Location = new System.Drawing.Point(20, yOffset)
                        };

                        pnlSeasons.Controls.Add(lblEpisode);
                        yOffset += 20;
                    }

                    yOffset += 10; // Add space between seasons
                }
            }
            else
            {
                // Hide season-related controls for movies
                this.lblSeasons.Visible = false;
                this.pnlSeasons.Visible = false;
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

        private void btnSubmitRating_Click(object sender, EventArgs e)
        {
            // Get selected rating
            int rating = 0;

            if (rbRating1.Checked) rating = 1;
            else if (rbRating2.Checked) rating = 2;
            else if (rbRating3.Checked) rating = 3;
            else if (rbRating4.Checked) rating = 4;
            else if (rbRating5.Checked) rating = 5;
            else if (rbRating6.Checked) rating = 6;
            else if (rbRating7.Checked) rating = 7;
            else if (rbRating8.Checked) rating = 8;
            else if (rbRating9.Checked) rating = 9;
            else if (rbRating10.Checked) rating = 10;

            if (rating > 0)
            {
                _userController.RateContent(_currentUser.Id, _content.Id, rating);

                // Refresh content to get updated average rating
                _content.AverageRating = _contentController.GetContentById(_content.Id).AverageRating;

                // Update rating display
                this.lblRatingValue.Text = $"{_content.AverageRating}/10 ({_content.ViewCount} views)";

                MessageBox.Show("Rating submitted successfully!", "Rating", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Please select a rating first.", "Rating", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void tbProgress_ValueChanged(object sender, EventArgs e)
        {
            // Update progress label
            this.lblProgressValue.Text = $"{tbProgress.Value}%";

            // Save progress to user
            double progress = tbProgress.Value / 100.0;
            _userProgress = progress;
            _userController.UpdateUserWatchHistory(_currentUser.Id, _content.Id, progress);
        }

        private void btnMarkAsWatched_Click(object sender, EventArgs e)
        {
            // Set progress to 100%
            tbProgress.Value = 100;
            _userProgress = 1.0;
            _userController.UpdateUserWatchHistory(_currentUser.Id, _content.Id, 1.0);

            MessageBox.Show("Marked as watched!", "Watch Status", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private Label lblTitle;
        private Label lblDescription;
        private TextBox txtDescription;
        private Label lblType;
        private Label lblTypeValue;
        private Label lblGenres;
        private Label lblGenresValue;
        private Label lblPlatform;
        private Label lblPlatformValue;
        private Label lblYear;
        private Label lblYearValue;
        private Label lblRating;
        private Label lblRatingValue;
        private Label lblDuration;
        private Label lblDurationValue;
        private GroupBox gbRating;
        private RadioButton rbRating1;
        private RadioButton rbRating2;
        private RadioButton rbRating3;
        private RadioButton rbRating4;
        private RadioButton rbRating5;
        private RadioButton rbRating6;
        private RadioButton rbRating7;
        private RadioButton rbRating8;
        private RadioButton rbRating9;
        private RadioButton rbRating10;
        private Button btnSubmitRating;
        private Label lblProgress;
        private TrackBar tbProgress;
        private Label lblProgressValue;
        private Button btnMarkAsWatched;
        private Button btnClose;
        private Panel pnlSeasons;
        private Label lblSeasons;
    }
}
