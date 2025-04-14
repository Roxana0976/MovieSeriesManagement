using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MovieSeriesManagement.Controllers;
using MovieSeriesManagement.Models;

namespace MovieSeriesManagement.Views
{
    public partial class ContentEditForm : Form
    {
        private readonly Content _content;
        private readonly ContentController _contentController;
        private bool _isNewContent;

        public ContentEditForm(Content content, ContentController contentController)
        {
            _contentController = contentController;
            _isNewContent = (content == null);

            if (_isNewContent)
            {
                _content = new Content();
            }
            else
            {
                _content = content;
            }

            InitializeComponent();

            // Load data
            LoadFormData();
        }

        private void InitializeComponent()
        {
            this.lblTitle = new Label();
            this.txtTitle = new TextBox();
            this.lblDescription = new Label();
            this.txtDescription = new TextBox();
            this.lblType = new Label();
            this.rbMovie = new RadioButton();
            this.rbSeries = new RadioButton();
            this.lblGenres = new Label();
            this.clbGenres = new CheckedListBox();
            this.lblPlatform = new Label();
            this.cmbPlatform = new ComboBox();
            this.lblYear = new Label();
            this.numYear = new NumericUpDown();
            this.lblDuration = new Label();
            this.numDuration = new NumericUpDown();
            this.lblSeries = new Label();
            this.pnlSeries = new Panel();
            this.btnAddSeason = new Button();
            this.btnSave = new Button();
            this.btnCancel = new Button();

            // Set up form
            this.Text = _isNewContent ? "Add Content" : "Edit Content";
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Text = "Title:";

            // txtTitle
            this.txtTitle.Location = new System.Drawing.Point(120, 17);
            this.txtTitle.Size = new System.Drawing.Size(650, 23);

            // lblDescription
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(20, 55);
            this.lblDescription.Text = "Description:";

            // txtDescription
            this.txtDescription.Location = new System.Drawing.Point(120, 52);
            this.txtDescription.Size = new System.Drawing.Size(650, 80);
            this.txtDescription.Multiline = true;
            this.txtDescription.ScrollBars = ScrollBars.Vertical;

            // lblType
            this.lblType.AutoSize = true;
            this.lblType.Location = new System.Drawing.Point(20, 150);
            this.lblType.Text = "Type:";

            // rbMovie
            this.rbMovie.AutoSize = true;
            this.rbMovie.Location = new System.Drawing.Point(120, 150);
            this.rbMovie.Text = "Movie";
            this.rbMovie.Checked = true;
            this.rbMovie.CheckedChanged += new EventHandler(this.rbType_CheckedChanged);

            // rbSeries
            this.rbSeries.AutoSize = true;
            this.rbSeries.Location = new System.Drawing.Point(200, 150);
            this.rbSeries.Text = "Series";
            this.rbSeries.CheckedChanged += new EventHandler(this.rbType_CheckedChanged);

            // lblGenres
            this.lblGenres.AutoSize = true;
            this.lblGenres.Location = new System.Drawing.Point(20, 190);
            this.lblGenres.Text = "Genres:";

            // clbGenres
            this.clbGenres.Location = new System.Drawing.Point(120, 187);
            this.clbGenres.Size = new System.Drawing.Size(200, 120);
            this.clbGenres.CheckOnClick = true;

            // Add standard genres
            string[] genres = { "Action", "Adventure", "Animation", "Comedy", "Crime", "Documentary",
                               "Drama", "Fantasy", "Horror", "Mystery", "Romance", "Sci-Fi", "Thriller", "Western" };
            foreach (var genre in genres)
            {
                clbGenres.Items.Add(genre);
            }

            // lblPlatform
            this.lblPlatform.AutoSize = true;
            this.lblPlatform.Location = new System.Drawing.Point(350, 190);
            this.lblPlatform.Text = "Platform:";

            // cmbPlatform
            this.cmbPlatform.Location = new System.Drawing.Point(450, 187);
            this.cmbPlatform.Size = new System.Drawing.Size(200, 23);
            this.cmbPlatform.DropDownStyle = ComboBoxStyle.DropDownList;

            // Add standard platforms
            string[] platforms = { "Netflix", "Amazon Prime", "Disney+", "HBO Max", "Hulu", "Apple TV+", "Paramount+" };
            foreach (var platform in platforms)
            {
                cmbPlatform.Items.Add(platform);
            }

            // lblYear
            this.lblYear.AutoSize = true;
            this.lblYear.Location = new System.Drawing.Point(350, 230);
            this.lblYear.Text = "Release Year:";

            // numYear
            this.numYear.Location = new System.Drawing.Point(450, 227);
            this.numYear.Size = new System.Drawing.Size(120, 23);
            this.numYear.Maximum = DateTime.Now.Year;
            this.numYear.Minimum = 1900;
            this.numYear.Value = DateTime.Now.Year;

            // lblDuration
            this.lblDuration.AutoSize = true;
            this.lblDuration.Location = new System.Drawing.Point(350, 270);
            this.lblDuration.Text = "Duration (mins):";

            // numDuration
            this.numDuration.Location = new System.Drawing.Point(450, 267);
            this.numDuration.Size = new System.Drawing.Size(120, 23);
            this.numDuration.Maximum = 1000;
            this.numDuration.Minimum = 1;
            this.numDuration.Value = 90;

            // lblSeries
            this.lblSeries.AutoSize = true;
            this.lblSeries.Location = new System.Drawing.Point(20, 320);
            this.lblSeries.Text = "Seasons & Episodes:";

            // pnlSeries
            this.pnlSeries.Location = new System.Drawing.Point(20, 350);
            this.pnlSeries.Size = new System.Drawing.Size(750, 170);
            this.pnlSeries.BorderStyle = BorderStyle.FixedSingle;
            this.pnlSeries.AutoScroll = true;

            // btnAddSeason
            this.btnAddSeason.Text = "Add Season";
            this.btnAddSeason.Location = new System.Drawing.Point(670, 320);
            this.btnAddSeason.Size = new System.Drawing.Size(100, 25);
            this.btnAddSeason.Click += new EventHandler(this.btnAddSeason_Click);

            // btnSave
            this.btnSave.Text = "Save";
            this.btnSave.Location = new System.Drawing.Point(590, 540);
            this.btnSave.Size = new System.Drawing.Size(90, 30);
            this.btnSave.Click += new EventHandler(this.btnSave_Click);

            // btnCancel
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Location = new System.Drawing.Point(690, 540);
            this.btnCancel.Size = new System.Drawing.Size(90, 30);
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);

            // Add controls to form
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.txtTitle);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.lblType);
            this.Controls.Add(this.rbMovie);
            this.Controls.Add(this.rbSeries);
            this.Controls.Add(this.lblGenres);
            this.Controls.Add(this.clbGenres);
            this.Controls.Add(this.lblPlatform);
            this.Controls.Add(this.cmbPlatform);
            this.Controls.Add(this.lblYear);
            this.Controls.Add(this.numYear);
            this.Controls.Add(this.lblDuration);
            this.Controls.Add(this.numDuration);
            this.Controls.Add(this.lblSeries);
            this.Controls.Add(this.pnlSeries);
            this.Controls.Add(this.btnAddSeason);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);

            // Initial setup
            UpdateSeriesControls();
        }

        private void LoadFormData()
        {
            if (_isNewContent) return;

            txtTitle.Text = _content.Title;
            txtDescription.Text = _content.Description;
            rbMovie.Checked = (_content.Type == ContentType.Movie);
            rbSeries.Checked = (_content.Type == ContentType.Series);

            // Set genres
            for (int i = 0; i < clbGenres.Items.Count; i++)
            {
                if (_content.Genres.Contains(clbGenres.Items[i].ToString()))
                {
                    clbGenres.SetItemChecked(i, true);
                }
            }

            // Set platform
            if (cmbPlatform.Items.Contains(_content.Platform))
            {
                cmbPlatform.SelectedItem = _content.Platform;
            }

            numYear.Value = _content.ReleaseYear;
            numDuration.Value = Math.Min(_content.Duration, numDuration.Maximum);

            // Load seasons and episodes
            if (_content.Type == ContentType.Series)
            {
                LoadSeasons();
            }

            UpdateSeriesControls();
        }

        private void UpdateSeriesControls()
        {
            bool isSeries = rbSeries.Checked;

            lblSeries.Visible = isSeries;
            pnlSeries.Visible = isSeries;
            btnAddSeason.Visible = isSeries;

            if (isSeries && pnlSeries.Controls.Count == 0 && (_isNewContent || _content.Seasons.Count == 0))
            {
                // Add initial season for new series
                AddNewSeason();
            }
        }

        private void LoadSeasons()
        {
            pnlSeries.Controls.Clear();

            int yOffset = 10;

            for (int i = 0; i < _content.Seasons.Count; i++)
            {
                Season season = _content.Seasons[i];

                Label lblSeasonHeader = new Label
                {
                    Text = $"Season {season.Number}: {season.Title}",
                    AutoSize = true,
                    Location = new System.Drawing.Point(10, yOffset),
                    Tag = i // Store season index
                };

                Button btnAddEpisode = new Button
                {
                    Text = "Add Episode",
                    Size = new System.Drawing.Size(100, 23),
                    Location = new System.Drawing.Point(520, yOffset - 5),
                    Tag = i // Store season index
                };
                btnAddEpisode.Click += new EventHandler(this.btnAddEpisode_Click);

                Button btnDeleteSeason = new Button
                {
                    Text = "Delete Season",
                    Size = new System.Drawing.Size(100, 23),
                    Location = new System.Drawing.Point(630, yOffset - 5),
                    Tag = i // Store season index
                };
                btnDeleteSeason.Click += new EventHandler(this.btnDeleteSeason_Click);

                pnlSeries.Controls.Add(lblSeasonHeader);
                pnlSeries.Controls.Add(btnAddEpisode);
                pnlSeries.Controls.Add(btnDeleteSeason);

                yOffset += 30;

                // Add episodes
                for (int j = 0; j < season.Episodes.Count; j++)
                {
                    Episode episode = season.Episodes[j];

                    Panel pnlEpisode = new Panel
                    {
                        Size = new System.Drawing.Size(700, 30),
                        Location = new System.Drawing.Point(20, yOffset),
                        Tag = new int[] { i, j } // Store season and episode indices
                    };

                    Label lblEpNumber = new Label
                    {
                        Text = $"Ep {episode.Number}:",
                        AutoSize = true,
                        Location = new System.Drawing.Point(0, 5)
                    };

                    TextBox txtEpTitle = new TextBox
                    {
                        Text = episode.Title,
                        Size = new System.Drawing.Size(300, 23),
                        Location = new System.Drawing.Point(70, 2),
                        Tag = new int[] { i, j } // Store season and episode indices
                    };
                    txtEpTitle.TextChanged += new EventHandler(this.txtEpisodeTitle_TextChanged);

                    Label lblDuration = new Label
                    {
                        Text = "Duration:",
                        AutoSize = true,
                        Location = new System.Drawing.Point(380, 5)
                    };

                    NumericUpDown numEpDuration = new NumericUpDown
                    {
                        Value = episode.Duration,
                        Size = new System.Drawing.Size(60, 23),
                        Location = new System.Drawing.Point(440, 2),
                        Minimum = 1,
                        Maximum = 300,
                        Tag = new int[] { i, j } // Store season and episode indices
                    };
                    numEpDuration.ValueChanged += new EventHandler(this.numEpisodeDuration_ValueChanged);

                    Button btnDeleteEpisode = new Button
                    {
                        Text = "Delete",
                        Size = new System.Drawing.Size(70, 23),
                        Location = new System.Drawing.Point(510, 2),
                        Tag = new int[] { i, j } // Store season and episode indices
                    };
                    btnDeleteEpisode.Click += new EventHandler(this.btnDeleteEpisode_Click);

                    pnlEpisode.Controls.Add(lblEpNumber);
                    pnlEpisode.Controls.Add(txtEpTitle);
                    pnlEpisode.Controls.Add(lblDuration);
                    pnlEpisode.Controls.Add(numEpDuration);
                    pnlEpisode.Controls.Add(btnDeleteEpisode);

                    pnlSeries.Controls.Add(pnlEpisode);

                    yOffset += 35;
                }

                yOffset += 10; // Space between seasons
            }
        }

        private void rbType_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSeriesControls();
        }

        private void btnAddSeason_Click(object sender, EventArgs e)
        {
            AddNewSeason();
        }

        private void AddNewSeason()
        {
            // Initialize Seasons list if needed
            if (_content.Seasons == null)
            {
                _content.Seasons = new List<Season>();
            }

            // Add new season
            int seasonNumber = _content.Seasons.Count + 1;
            Season newSeason = new Season
            {
                Number = seasonNumber,
                Title = $"Season {seasonNumber}",
                Episodes = new List<Episode>()
            };

            // Add default episode
            newSeason.Episodes.Add(new Episode
            {
                Number = 1,
                Title = "Episode 1",
                Duration = 30
            });

            _content.Seasons.Add(newSeason);

            // Refresh seasons display
            LoadSeasons();
        }

        private void btnAddEpisode_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int seasonIndex = (int)btn.Tag;

            if (seasonIndex >= 0 && seasonIndex < _content.Seasons.Count)
            {
                Season season = _content.Seasons[seasonIndex];

                // Add new episode
                int episodeNumber = season.Episodes.Count + 1;
                season.Episodes.Add(new Episode
                {
                    Number = episodeNumber,
                    Title = $"Episode {episodeNumber}",
                    Duration = 30
                });

                // Refresh seasons display
                LoadSeasons();
            }
        }

        private void btnDeleteSeason_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int seasonIndex = (int)btn.Tag;

            if (seasonIndex >= 0 && seasonIndex < _content.Seasons.Count)
            {
                if (_content.Seasons.Count <= 1)
                {
                    MessageBox.Show("Series must have at least one season.", "Cannot Delete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Delete season
                _content.Seasons.RemoveAt(seasonIndex);

                // Renumber remaining seasons
                for (int i = 0; i < _content.Seasons.Count; i++)
                {
                    _content.Seasons[i].Number = i + 1;
                    _content.Seasons[i].Title = $"Season {i + 1}";
                }

                // Refresh seasons display
                LoadSeasons();
            }
        }

        private void btnDeleteEpisode_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int[] indices = (int[])btn.Tag;
            int seasonIndex = indices[0];
            int episodeIndex = indices[1];

            if (seasonIndex >= 0 && seasonIndex < _content.Seasons.Count &&
                episodeIndex >= 0 && episodeIndex < _content.Seasons[seasonIndex].Episodes.Count)
            {
                Season season = _content.Seasons[seasonIndex];

                if (season.Episodes.Count <= 1)
                {
                    MessageBox.Show("Season must have at least one episode.", "Cannot Delete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Delete episode
                season.Episodes.RemoveAt(episodeIndex);

                // Renumber remaining episodes
                for (int i = 0; i < season.Episodes.Count; i++)
                {
                    season.Episodes[i].Number = i + 1;
                }

                // Refresh seasons display
                LoadSeasons();
            }
        }

        private void txtEpisodeTitle_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            int[] indices = (int[])txt.Tag;
            int seasonIndex = indices[0];
            int episodeIndex = indices[1];

            if (seasonIndex >= 0 && seasonIndex < _content.Seasons.Count &&
                episodeIndex >= 0 && episodeIndex < _content.Seasons[seasonIndex].Episodes.Count)
            {
                _content.Seasons[seasonIndex].Episodes[episodeIndex].Title = txt.Text;
            }
        }

        private void numEpisodeDuration_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown num = (NumericUpDown)sender;
            int[] indices = (int[])num.Tag;
            int seasonIndex = indices[0];
            int episodeIndex = indices[1];

            if (seasonIndex >= 0 && seasonIndex < _content.Seasons.Count &&
                episodeIndex >= 0 && episodeIndex < _content.Seasons[seasonIndex].Episodes.Count)
            {
                _content.Seasons[seasonIndex].Episodes[episodeIndex].Duration = (int)num.Value;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Title is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtTitle.Focus();
                return;
            }

            if (clbGenres.CheckedItems.Count == 0)
            {
                MessageBox.Show("Please select at least one genre.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                clbGenres.Focus();
                return;
            }

            if (cmbPlatform.SelectedIndex < 0)
            {
                MessageBox.Show("Please select a platform.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cmbPlatform.Focus();
                return;
            }

            // Update content object
            _content.Title = txtTitle.Text;
            _content.Description = txtDescription.Text;
            _content.Type = rbMovie.Checked ? ContentType.Movie : ContentType.Series;
            _content.ReleaseYear = (int)numYear.Value;

            // Get selected genres
            _content.Genres.Clear();
            foreach (var item in clbGenres.CheckedItems)
            {
                _content.Genres.Add(item.ToString());
            }

            _content.Platform = cmbPlatform.SelectedItem.ToString();

            // Calculate total duration for series
            if (_content.Type == ContentType.Series)
            {
                int totalDuration = 0;
                foreach (var season in _content.Seasons)
                {
                    foreach (var episode in season.Episodes)
                    {
                        totalDuration += episode.Duration;
                    }
                }
                _content.Duration = totalDuration;
            }
            else
            {
                _content.Duration = (int)numDuration.Value;
                _content.Seasons.Clear(); // Clear seasons for movie type
            }

            // Save content
            if (_isNewContent)
            {
                _contentController.AddContent(_content);
            }
            else
            {
                _contentController.UpdateContent(_content);
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private Label lblTitle;
        private TextBox txtTitle;
        private Label lblDescription;
        private TextBox txtDescription;
        private Label lblType;
        private RadioButton rbMovie;
        private RadioButton rbSeries;
        private Label lblGenres;
        private CheckedListBox clbGenres;
        private Label lblPlatform;
        private ComboBox cmbPlatform;
        private Label lblYear;
        private NumericUpDown numYear;
        private Label lblDuration;
        private NumericUpDown numDuration;
        private Label lblSeries;
        private Panel pnlSeries;
        private Button btnAddSeason;
        private Button btnSave;
        private Button btnCancel;
    }
}
