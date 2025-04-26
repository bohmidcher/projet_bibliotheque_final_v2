using System.Data;
using Microsoft.EntityFrameworkCore;
using projet_bibliotheque.Data;
using projet_bibliotheque.Models;
using projet_bibliotheque.Forms;

namespace projet_bibliotheque.Views
{
    public partial class AuthorsView : UserControl
    {
        private DataGridView dgvAuthors = null!;
        private TextBox txtSearch = null!;
        private ElegantButton btnAdd = null!;
        private ElegantButton btnEdit = null!;
        private ElegantButton btnDelete = null!;
        private Panel toolbarPanel = null!;
        private readonly Color PrimaryColor = Color.FromArgb(31, 43, 71);
        private readonly Color SecondaryColor = Color.FromArgb(241, 134, 48);
        private readonly Color LightColor = Color.FromArgb(250, 250, 250);
        private readonly LibraryContext _context;

        public AuthorsView()
        {
            var optionsBuilder = new DbContextOptionsBuilder<LibraryContext>();
            optionsBuilder.UseSqlServer(@"Server=localhost;Database=BibliothequeIHEC;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true",
                options => options.EnableRetryOnFailure())
                .UseLazyLoadingProxies();
            _context = new LibraryContext(optionsBuilder.Options);
            SetupView();
            LoadAuthors();
        }

        private void SetupView()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.White;
            this.Padding = new Padding(20);

            // Panel pour la barre d'outils
            toolbarPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.White
            };

            // Barre de recherche
            txtSearch = new TextBox
            {
                Location = new Point(0, 15),
                Size = new Size(300, 30),
                Font = new Font("Poppins", 12),
                PlaceholderText = "Rechercher un auteur..."
            };
            txtSearch.TextChanged += TxtSearch_TextChanged;

            // Boutons d'action
            btnAdd = new ElegantButton
            {
                Text = "Ajouter",
                Size = new Size(120, 40),
                Location = new Point(toolbarPanel.Width - 400, 10),
                BackColor = SecondaryColor,
                ForeColor = Color.White,
                Font = new Font("Poppins", 12),
                CornerRadius = 20,
                Anchor = AnchorStyles.Right
            };
            btnAdd.Click += BtnAdd_Click;

            btnEdit = new ElegantButton
            {
                Text = "Modifier",
                Size = new Size(120, 40),
                Location = new Point(toolbarPanel.Width - 270, 10),
                BackColor = PrimaryColor,
                ForeColor = Color.White,
                Font = new Font("Poppins", 12),
                CornerRadius = 20,
                Anchor = AnchorStyles.Right
            };
            btnEdit.Click += BtnEdit_Click;

            btnDelete = new ElegantButton
            {
                Text = "Supprimer",
                Size = new Size(120, 40),
                Location = new Point(toolbarPanel.Width - 140, 10),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                Font = new Font("Poppins", 12),
                CornerRadius = 20,
                Anchor = AnchorStyles.Right
            };
            btnDelete.Click += BtnDelete_Click;

            toolbarPanel.Controls.AddRange(new Control[] { txtSearch, btnAdd, btnEdit, btnDelete });

            // Grille des auteurs
            dgvAuthors = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Font = new Font("Poppins", 11)
            };

            dgvAuthors.DefaultCellStyle.SelectionBackColor = SecondaryColor;
            dgvAuthors.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvAuthors.ColumnHeadersDefaultCellStyle.Font = new Font("Poppins", 12, FontStyle.Bold);
            dgvAuthors.ColumnHeadersDefaultCellStyle.BackColor = PrimaryColor;
            dgvAuthors.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvAuthors.ColumnHeadersHeight = 40;
            dgvAuthors.RowTemplate.Height = 35;

            // Ajout des contrôles
            this.Controls.Add(dgvAuthors);
            this.Controls.Add(toolbarPanel);
        }

        private async void LoadAuthors(string search = "")
        {
            try
            {
                var authorsQuery = _context.Authors
                    .Include(a => a.Books)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    string lowerSearch = search.ToLower();
                    authorsQuery = authorsQuery.Where(a =>
                        (a.Name != null && a.Name.ToLower().Contains(lowerSearch)) ||
                        (a.Nationality != null && a.Nationality.ToLower().Contains(lowerSearch)));
                }

                var authors = await authorsQuery
                    .Select(a => new
                    {
                        ID = a.Id,
                        Nom = a.Name ?? "Inconnu",
                        DateNaissance = a.Birthdate.HasValue ? a.Birthdate.Value.ToShortDateString() : "Non spécifiée",
                        Nationalité = a.Nationality ?? "Non spécifiée",
                        NombreLivres = a.Books.Count
                    })
                    .ToListAsync();

                dgvAuthors.DataSource = authors;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des auteurs : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadAuthors(txtSearch.Text);
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            var form = new AuthorForm(_context);
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadAuthors();
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgvAuthors.CurrentRow == null) return;

            int authorId = (int)dgvAuthors.CurrentRow.Cells["ID"].Value;
            var author = _context.Authors.Find(authorId);
            var form = new AuthorForm(_context, author);
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadAuthors();
            }
        }

        private async void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvAuthors.CurrentRow == null) return;

            try
            {
                int authorId = (int)dgvAuthors.CurrentRow.Cells["ID"].Value;
                var authorName = dgvAuthors.CurrentRow.Cells["Nom"].Value?.ToString() ?? "Inconnu";
                var bookCount = (int)dgvAuthors.CurrentRow.Cells["NombreLivres"].Value;

                if (bookCount > 0)
                {
                    MessageBox.Show(
                        $"Impossible de supprimer l'auteur {authorName} car il a {bookCount} livre(s) associé(s).",
                        "Suppression impossible",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }

                var result = MessageBox.Show(
                    $"Êtes-vous sûr de vouloir supprimer l'auteur {authorName} ?",
                    "Confirmation de suppression",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    var author = await _context.Authors.FindAsync(authorId);
                    if (author != null)
                    {
                        _context.Authors.Remove(author);
                        await _context.SaveChangesAsync();
                        LoadAuthors();
                        MessageBox.Show(
                            "L'auteur a été supprimé avec succès.",
                            "Succès",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Erreur lors de la suppression de l'auteur : {ex.Message}",
                    "Erreur",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}
