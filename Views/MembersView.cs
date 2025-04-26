using System.Data;
using Microsoft.EntityFrameworkCore;
using projet_bibliotheque.Data;
using projet_bibliotheque.Models;
using projet_bibliotheque.Forms;

namespace projet_bibliotheque.Views
{
    public partial class MembersView : UserControl, IDisposable
    {
        private DataGridView dgvMembers;
        private TextBox txtSearch;
        private ElegantButton btnAdd;
        private ElegantButton btnEdit;
        private ElegantButton btnDelete;
        private Panel toolbarPanel;
        private readonly Color PrimaryColor = Color.FromArgb(31, 43, 71);
        private readonly Color SecondaryColor = Color.FromArgb(241, 134, 48);
        private readonly Color LightColor = Color.FromArgb(250, 250, 250);
        private readonly LibraryContext _context;
        private bool disposedValue;

        public MembersView()
        {
            _context = new LibraryContext(new DbContextOptionsBuilder<LibraryContext>().Options);
            SetupView();
            LoadMembers();
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
                PlaceholderText = "Rechercher un membre..."
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

            // Grille des membres
            dgvMembers = new DataGridView
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

            dgvMembers.DefaultCellStyle.SelectionBackColor = SecondaryColor;
            dgvMembers.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvMembers.ColumnHeadersDefaultCellStyle.Font = new Font("Poppins", 12, FontStyle.Bold);
            dgvMembers.ColumnHeadersDefaultCellStyle.BackColor = PrimaryColor;
            dgvMembers.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvMembers.ColumnHeadersHeight = 40;
            dgvMembers.RowTemplate.Height = 35;

            // Ajout des contrôles
            this.Controls.Add(dgvMembers);
            this.Controls.Add(toolbarPanel);
        }

        private async void LoadMembers(string search = "")
        {
            try
            {
                var membersQuery = _context.Members
                    .Include(m => m.Loans)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    string lowerSearch = search.ToLower();
                    membersQuery = membersQuery.Where(m =>
                        (m.Name != null && m.Name.ToLower().Contains(lowerSearch)) ||
                        (m.Email != null && m.Email.ToLower().Contains(lowerSearch)));
                }

                var members = await membersQuery
                    .Select(m => new
                    {
                        ID = m.Id,
                        Nom = m.Name ?? "",
                        Email = m.Email ?? "",
                        DateInscription = m.DateInscription.ToShortDateString(),
                        NombreEmprunts = m.Loans != null ? m.Loans.Count : 0
                    })
                    .ToListAsync();

                dgvMembers.DataSource = members;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des membres : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadMembers(txtSearch.Text);
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            var form = new MemberForm(_context);
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadMembers();
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgvMembers.CurrentRow == null) return;

            int memberId = (int)dgvMembers.CurrentRow.Cells["ID"].Value;
            var member = _context.Members.Find(memberId);
            var form = new MemberForm(_context, member);
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadMembers();
            }
        }

        private async void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvMembers.CurrentRow == null) return;

            int memberId = (int)dgvMembers.CurrentRow.Cells["ID"].Value;
            string memberName = $"{dgvMembers.CurrentRow.Cells["Nom"].Value}";
            int loanCount = (int)dgvMembers.CurrentRow.Cells["NombreEmprunts"].Value;

            if (loanCount > 0)
            {
                MessageBox.Show(
                    $"Impossible de supprimer le membre {memberName} car il a {loanCount} emprunt(s) en cours.",
                    "Suppression impossible",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            if (MessageBox.Show(
                $"Êtes-vous sûr de vouloir supprimer le membre {memberName} ?",
                "Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    var member = await _context.Members.FindAsync(memberId);
                    if (member != null)
                    {
                        _context.Members.Remove(member);
                        await _context.SaveChangesAsync();
                        LoadMembers();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Erreur lors de la suppression : {ex.Message}",
                        "Erreur",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                disposedValue = true;
            }
            base.Dispose(disposing);
        }
    }
}
