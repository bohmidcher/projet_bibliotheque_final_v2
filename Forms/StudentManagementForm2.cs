using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using projet_bibliotheque.Data;
using projet_bibliotheque.Models;
using projet_bibliotheque.Utils;
using System.IO;

namespace projet_bibliotheque.Forms
{
    public partial class StudentManagementForm2 : Form
    {
        // Couleurs du thème
        private readonly Color PrimaryColor = Color.FromArgb(31, 43, 71);
        private readonly Color SecondaryColor = Color.FromArgb(241, 134, 48);
        private readonly Color LightColor = Color.FromArgb(250, 250, 250);
        private readonly Color BackgroundColor = Color.FromArgb(248, 249, 250);
        private readonly Color TextColor = Color.FromArgb(33, 37, 41);
        private readonly Color SuccessColor = Color.FromArgb(40, 167, 69);
        private readonly Color WarningColor = Color.FromArgb(255, 193, 7);
        private readonly Color DangerColor = Color.FromArgb(220, 53, 69);
        private readonly Color SidebarColor = Color.FromArgb(25, 55, 109);

        // Composants du formulaire
        private Panel contentPanel;
        private Panel headerPanel;
        private TextBox searchBox;
        private Button btnFilter;
        private Button btnNew;
        private Label lblTitle;
        private Label lblStudents;
        private Label lblStudentCount;
        private FlowLayoutPanel studentsPanel;
        private DataGridView dgvStudents;

        // Contexte de la base de données
        private readonly LibraryContext _context;
        private List<Member> students;

        public StudentManagementForm2(LibraryContext context)
        {
            _context = context;
            InitializeComponent();
            LoadStudents();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Configuration du formulaire
            this.ClientSize = new Size(1200, 800);
            this.Text = "BiblioHub - Gérer les étudiants";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = BackgroundColor;
            
            // Création du layout principal
            CreateLayout();
            
            // Création de l'en-tête
            CreateHeader();
            
            // Création du contenu principal
            CreateContent();
            
            this.ResumeLayout(false);
        }

        private void CreateLayout()
        {
            // Panneau de contenu principal
            contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = BackgroundColor,
                Padding = new Padding(30)
            };

            this.Controls.Add(contentPanel);
        }

        private void CreateHeader()
        {
            // Panneau d'en-tête
            headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 120,
                BackColor = BackgroundColor,
                Padding = new Padding(0, 0, 0, 20)
            };

            // Titre
            lblTitle = new Label
            {
                Text = "Gérer les étudiants",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = TextColor,
                AutoSize = true,
                Location = new Point(0, 0)
            };

            // Barre de recherche
            searchBox = new TextBox
            {
                Size = new Size(300, 40),
                Location = new Point(0, 60),
                Font = new Font("Segoe UI", 12),
                PlaceholderText = "Rechercher des étudiants...",
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(10, 5, 10, 5)
            };

            // Bouton de filtre
            btnFilter = new Button
            {
                Text = "Filtrer",
                Size = new Size(100, 40),
                Location = new Point(searchBox.Right + 20, 60),
                FlatStyle = FlatStyle.Flat,
                BackColor = LightColor,
                ForeColor = TextColor,
                Font = new Font("Segoe UI", 10),
                TextAlign = ContentAlignment.MiddleCenter,
                ImageAlign = ContentAlignment.MiddleLeft
            };

            // Ajouter une icône de filtre
            try
            {
                Image filterIcon = IconHelper.LoadIcon("filter");
                if (filterIcon != null)
                {
                    filterIcon = new Bitmap(filterIcon, new Size(16, 16));
                    btnFilter.Image = filterIcon;
                    btnFilter.ImageAlign = ContentAlignment.MiddleLeft;
                    btnFilter.TextAlign = ContentAlignment.MiddleRight;
                    btnFilter.TextImageRelation = TextImageRelation.ImageBeforeText;
                    btnFilter.Padding = new Padding(10, 0, 10, 0);
                }
            }
            catch { /* Ignorer les erreurs d'icône */ }

            // Bouton Nouveau
            btnNew = new Button
            {
                Text = "Nouveau",
                Size = new Size(120, 40),
                Location = new Point(contentPanel.Width - 120, 60),
                FlatStyle = FlatStyle.Flat,
                BackColor = PrimaryColor,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Ajouter une icône plus
            try
            {
                Image plusIcon = IconHelper.LoadIcon("plus");
                if (plusIcon != null)
                {
                    plusIcon = new Bitmap(plusIcon, new Size(16, 16));
                    btnNew.Image = plusIcon;
                    btnNew.ImageAlign = ContentAlignment.MiddleLeft;
                    btnNew.TextAlign = ContentAlignment.MiddleRight;
                    btnNew.TextImageRelation = TextImageRelation.ImageBeforeText;
                    btnNew.Padding = new Padding(10, 0, 10, 0);
                }
            }
            catch { /* Ignorer les erreurs d'icône */ }

            // Ajouter les contrôles à l'en-tête
            headerPanel.Controls.Add(lblTitle);
            headerPanel.Controls.Add(searchBox);
            headerPanel.Controls.Add(btnFilter);
            headerPanel.Controls.Add(btnNew);

            // Ajouter l'en-tête au contenu
            contentPanel.Controls.Add(headerPanel);

            // Événements
            searchBox.TextChanged += SearchBox_TextChanged;
            btnFilter.Click += BtnFilter_Click;
            btnNew.Click += BtnNew_Click;
        }

        private void CreateContent()
        {
            // Titre de la section Étudiants
            lblStudents = new Label
            {
                Text = "Étudiants",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = TextColor,
                AutoSize = true,
                Location = new Point(0, headerPanel.Bottom + 10)
            };

            // Nombre d'étudiants
            lblStudentCount = new Label
            {
                Text = "0 Étudiants",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(lblStudents.Right + 10, headerPanel.Bottom + 20)
            };

            // Grille de données pour afficher les étudiants
            dgvStudents = new DataGridView
            {
                Location = new Point(0, lblStudents.Bottom + 20),
                Size = new Size(contentPanel.Width - 60, contentPanel.Height - lblStudents.Bottom - 40),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                GridColor = Color.FromArgb(230, 230, 230),
                RowTemplate = { Height = 40 }
            };

            // Configurer les colonnes de la grille
            dgvStudents.Columns.Add("Id", "ID");
            dgvStudents.Columns.Add("Name", "Nom");
            dgvStudents.Columns.Add("Email", "Email");
            dgvStudents.Columns.Add("DateInscription", "Date d'inscription");
            dgvStudents.Columns.Add("NiveauEducatif", "Niveau éducatif");
            dgvStudents.Columns.Add("Specialite", "Spécialité");
            dgvStudents.Columns.Add("IsAdmin", "Admin");

            // Configurer l'apparence des colonnes
            dgvStudents.Columns["Id"].Width = 50;
            dgvStudents.Columns["Name"].Width = 150;
            dgvStudents.Columns["Email"].Width = 200;
            dgvStudents.Columns["DateInscription"].Width = 150;
            dgvStudents.Columns["NiveauEducatif"].Width = 150;
            dgvStudents.Columns["Specialite"].Width = 150;
            dgvStudents.Columns["IsAdmin"].Width = 80;

            // Configurer l'apparence de la grille
            dgvStudents.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgvStudents.DefaultCellStyle.ForeColor = TextColor;
            dgvStudents.DefaultCellStyle.BackColor = Color.White;
            dgvStudents.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 240, 250);
            dgvStudents.DefaultCellStyle.SelectionForeColor = TextColor;
            dgvStudents.DefaultCellStyle.Padding = new Padding(5);

            dgvStudents.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvStudents.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            dgvStudents.ColumnHeadersDefaultCellStyle.ForeColor = TextColor;
            dgvStudents.ColumnHeadersDefaultCellStyle.Padding = new Padding(5);
            dgvStudents.ColumnHeadersHeight = 50;
            dgvStudents.EnableHeadersVisualStyles = false;

            // Ajouter les contrôles au contenu
            contentPanel.Controls.Add(lblStudents);
            contentPanel.Controls.Add(lblStudentCount);
            contentPanel.Controls.Add(dgvStudents);

            // Ajouter un gestionnaire d'événements pour le double-clic sur une ligne
            dgvStudents.CellDoubleClick += DgvStudents_CellDoubleClick;
        }

        private void LoadStudents()
        {
            try
            {
                // Charger les étudiants depuis la base de données
                students = _context.Members
                    .OrderBy(m => m.Name)
                    .ToList();

                // Mettre à jour le compteur d'étudiants
                lblStudentCount.Text = $"{students.Count} Étudiants";

                // Effacer les lignes existantes
                dgvStudents.Rows.Clear();

                // Ajouter chaque étudiant à la grille
                foreach (var student in students)
                {
                    dgvStudents.Rows.Add(
                        student.Id,
                        student.Name,
                        student.Email,
                        student.DateInscription.ToString("dd/MM/yyyy"),
                        student.NiveauEducatif ?? "Non spécifié",
                        student.Specialite ?? "Non spécifié",
                        student.IsAdmin ? "Oui" : "Non"
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des étudiants : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SearchBox_TextChanged(object sender, EventArgs e)
        {
            // Filtrer les étudiants en fonction du texte de recherche
            string searchText = searchBox.Text.ToLower();
            
            dgvStudents.Rows.Clear();
            
            foreach (var student in students)
            {
                if (string.IsNullOrEmpty(searchText) || 
                    student.Name.ToLower().Contains(searchText) || 
                    student.Email.ToLower().Contains(searchText) ||
                    (student.NiveauEducatif != null && student.NiveauEducatif.ToLower().Contains(searchText)) ||
                    (student.Specialite != null && student.Specialite.ToLower().Contains(searchText)))
                {
                    dgvStudents.Rows.Add(
                        student.Id,
                        student.Name,
                        student.Email,
                        student.DateInscription.ToString("dd/MM/yyyy"),
                        student.NiveauEducatif ?? "Non spécifié",
                        student.Specialite ?? "Non spécifié",
                        student.IsAdmin ? "Oui" : "Non"
                    );
                }
            }
        }

        private void BtnFilter_Click(object sender, EventArgs e)
        {
            // Afficher un menu de filtre (à implémenter)
            MessageBox.Show("Fonctionnalité de filtre à implémenter", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            // Ouvrir le formulaire d'ajout d'étudiant (à implémenter)
            MessageBox.Show("Fonctionnalité d'ajout d'étudiant à implémenter", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void DgvStudents_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Récupérer l'ID de l'étudiant sélectionné
                int studentId = Convert.ToInt32(dgvStudents.Rows[e.RowIndex].Cells["Id"].Value);
                
                // Trouver l'étudiant correspondant
                Member student = students.FirstOrDefault(s => s.Id == studentId);
                
                if (student != null)
                {
                    // Ouvrir le formulaire d'édition d'étudiant (à implémenter)
                    MessageBox.Show($"Édition de l'étudiant : {student.Name}", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}
