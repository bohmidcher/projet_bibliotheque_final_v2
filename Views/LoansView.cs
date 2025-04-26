using System.Data;
using Microsoft.EntityFrameworkCore;
using projet_bibliotheque.Data;
using projet_bibliotheque.Models;
using projet_bibliotheque.Forms;

namespace projet_bibliotheque.Views
{
    public partial class LoansView : UserControl
    {
        private DataGridView dgvLoans;
        private TextBox txtSearch;
        private ElegantButton btnAdd;
        private ElegantButton btnReturn;
        private Panel toolbarPanel;
        private readonly Color PrimaryColor = Color.FromArgb(31, 43, 71);
        private readonly Color SecondaryColor = Color.FromArgb(241, 134, 48);
        private readonly Color LightColor = Color.FromArgb(250, 250, 250);
        private readonly LibraryContext _context;

        public LoansView(LibraryContext context)
        {
            _context = context;
            SetupView();
            LoadLoans();
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
                PlaceholderText = "Rechercher un emprunt..."
            };
            txtSearch.TextChanged += TxtSearch_TextChanged;

            // Boutons d'action
            btnAdd = new ElegantButton
            {
                Text = "Nouvel emprunt",
                Size = new Size(150, 40),
                Location = new Point(toolbarPanel.Width - 330, 10),
                BackColor = SecondaryColor,
                ForeColor = Color.White,
                Font = new Font("Poppins", 12),
                CornerRadius = 20,
                Anchor = AnchorStyles.Right
            };
            btnAdd.Click += BtnAdd_Click;

            btnReturn = new ElegantButton
            {
                Text = "Retourner",
                Size = new Size(150, 40),
                Location = new Point(toolbarPanel.Width - 160, 10),
                BackColor = PrimaryColor,
                ForeColor = Color.White,
                Font = new Font("Poppins", 12),
                CornerRadius = 20,
                Anchor = AnchorStyles.Right
            };
            btnReturn.Click += BtnReturn_Click;

            toolbarPanel.Controls.AddRange(new Control[] { txtSearch, btnAdd, btnReturn });

            // Grille des emprunts
            dgvLoans = new DataGridView
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

            dgvLoans.DefaultCellStyle.SelectionBackColor = SecondaryColor;
            dgvLoans.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvLoans.ColumnHeadersDefaultCellStyle.Font = new Font("Poppins", 12, FontStyle.Bold);
            dgvLoans.ColumnHeadersDefaultCellStyle.BackColor = PrimaryColor;
            dgvLoans.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvLoans.ColumnHeadersHeight = 40;
            dgvLoans.RowTemplate.Height = 35;

            // Ajout des contrôles
            this.Controls.Add(dgvLoans);
            this.Controls.Add(toolbarPanel);
        }

        private async void LoadLoans(string search = "")
        {
            try
            {
                var loansQuery = _context.Loans
                    .Include(l => l.Book)
                    .Include(l => l.Member)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    string lowerSearch = search.ToLower();
                    loansQuery = loansQuery.Where(l =>
                        (l.Book.Title != null && l.Book.Title.ToLower().Contains(lowerSearch)) ||
                        (l.Member.Name != null && l.Member.Name.ToLower().Contains(lowerSearch)));
                }

                var loans = await loansQuery
                    .Select(l => new
                    {
                        ID = l.Id,
                        Livre = l.Book.Title,
                        Membre = l.Member.Name,
                        DateEmprunt = l.LoanDate.ToShortDateString(),
                        DateRetour = l.ReturnDate.HasValue ? l.ReturnDate.Value.ToShortDateString() : "Non retourné",
                        JoursRestants = l.ReturnDate.HasValue ? (l.ReturnDate.Value - DateTime.Now).Days : 0,
                        Statut = l.ReturnDate.HasValue 
                            ? (DateTime.Now > l.ReturnDate.Value 
                                ? $"En retard ({(DateTime.Now - l.ReturnDate.Value).Days} jours)" 
                                : $"En cours ({(l.ReturnDate.Value - DateTime.Now).Days} jours restants)")
                            : "En cours"
                    })
                    .ToListAsync();

                dgvLoans.DataSource = loans;

                // Colorer les lignes en fonction du statut
                foreach (DataGridViewRow row in dgvLoans.Rows)
                {
                    int joursRestants = Convert.ToInt32(row.Cells["JoursRestants"].Value);
                    if (joursRestants < 0) // En retard
                    {
                        row.DefaultCellStyle.ForeColor = Color.White;
                        row.DefaultCellStyle.BackColor = Color.FromArgb(220, 53, 69); // Rouge
                        row.DefaultCellStyle.SelectionBackColor = Color.FromArgb(200, 35, 51);
                    }
                    else if (joursRestants <= 3) // Bientôt en retard
                    {
                        row.DefaultCellStyle.ForeColor = Color.Black;
                        row.DefaultCellStyle.BackColor = Color.FromArgb(255, 193, 7); // Jaune
                        row.DefaultCellStyle.SelectionBackColor = Color.FromArgb(235, 173, 0);
                    }
                }

                // Vérifier s'il y a des emprunts en retard et afficher une notification
                var empruntsEnRetard = loans.Count(l => Convert.ToInt32(l.JoursRestants) < 0);
                var empruntsBientotEnRetard = loans.Count(l => {
                    var jours = Convert.ToInt32(l.JoursRestants);
                    return jours >= 0 && jours <= 3;
                });

                if (empruntsEnRetard > 0 || empruntsBientotEnRetard > 0)
                {
                    string message = "";
                    if (empruntsEnRetard > 0)
                    {
                        message += $"Il y a {empruntsEnRetard} emprunt(s) en retard.\n";
                    }
                    if (empruntsBientotEnRetard > 0)
                    {
                        message += $"Il y a {empruntsBientotEnRetard} emprunt(s) qui seront bientôt en retard.";
                    }

                    MessageBox.Show(
                        message,
                        "Attention aux retards",
                        MessageBoxButtons.OK,
                        empruntsEnRetard > 0 ? MessageBoxIcon.Error : MessageBoxIcon.Warning
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des emprunts : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadLoans(txtSearch.Text);
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            var form = new LoanForm(_context);
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadLoans();
            }
        }

        private async void BtnReturn_Click(object sender, EventArgs e)
        {
            if (dgvLoans.CurrentRow == null) return;

            try
            {
                int loanId = (int)dgvLoans.CurrentRow.Cells["ID"].Value;
                string bookTitle = dgvLoans.CurrentRow.Cells["Livre"].Value.ToString();
                string memberName = dgvLoans.CurrentRow.Cells["Membre"].Value.ToString();
                int joursRetard = Convert.ToInt32(dgvLoans.CurrentRow.Cells["JoursRestants"].Value);

                string message = $"Confirmer le retour du livre '{bookTitle}' emprunté par {memberName} ?";
                decimal penalite = 0;

                if (joursRetard < 0)
                {
                    penalite = Math.Abs(joursRetard) * 1.0m; // 1 dinar par jour de retard
                    message += $"\n\nRetard de {Math.Abs(joursRetard)} jours.";
                    message += $"\nPénalité à payer : {penalite} dinars";
                }

                var result = MessageBox.Show(
                    message,
                    "Confirmation de retour",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    var loan = await _context.Loans
                        .Include(l => l.Book)
                        .Include(l => l.Member)
                        .FirstOrDefaultAsync(l => l.Id == loanId);

                    if (loan != null)
                    {
                        // Supprimer l'emprunt
                        _context.Loans.Remove(loan);
                        await _context.SaveChangesAsync();
                        LoadLoans();

                        string successMessage = "Le livre a été retourné avec succès.";
                        if (penalite > 0)
                        {
                            successMessage += $"\n\nPénalité calculée : {penalite} dinars";
                            successMessage += $"\nVeuillez collecter le paiement.";

                            MessageBox.Show(
                                $"ATTENTION : Le membre {memberName} doit payer une pénalité de {penalite} dinars pour le retard.",
                                "Pénalité à payer",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning
                            );
                        }

                        MessageBox.Show(
                            successMessage,
                            "Succès",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du retour du livre : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
