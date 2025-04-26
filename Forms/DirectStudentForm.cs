using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using projet_bibliotheque.Data;
using projet_bibliotheque.Models;
using projet_bibliotheque.Utils;

namespace projet_bibliotheque.Forms
{
    public class DirectStudentForm : Form
    {
        // Couleurs du thème
        private readonly Color PrimaryColor = Color.FromArgb(31, 43, 71);
        private readonly Color SecondaryColor = Color.FromArgb(241, 134, 48);
        private readonly Color LightColor = Color.FromArgb(250, 250, 250);
        private readonly Color BackgroundColor = Color.FromArgb(248, 249, 250);
        private readonly Color TextColor = Color.FromArgb(33, 37, 41);

        // Composants du formulaire
        private Panel contentPanel;
        private Panel headerPanel;
        private Label lblTitle;
        private DataGridView dgvStudents;
        private TextBox txtSearch;
        private Button btnSearch;
        private Button btnFilter;
        private Button btnNew;

        // Contexte de la base de données
        private readonly LibraryContext _context;

        // Liste des étudiants
        private List<Member> students = new List<Member>();

        public DirectStudentForm(LibraryContext context)
        {
            _context = context;
            InitializeComponent();
            LoadStudents();
            CreateSidebar();
        }

        private void InitializeComponent()
        {
            // Configuration du formulaire
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.Text = "Gestion des Étudiants";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = BackgroundColor;
            
            // Créer le layout principal
            TableLayoutPanel mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                Padding = new Padding(20)
            };
            
            // Configurer les styles de ligne
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            
            // Créer le panel d'en-tête
            headerPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent
            };
            
            // Créer le titre
            lblTitle = new Label
            {
                Text = "Gestion des Étudiants",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = PrimaryColor,
                AutoSize = true,
                Location = new Point(0, 10)
            };
            
            // Créer le panel de contenu
            contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent
            };
            
            // Créer la barre de recherche
            txtSearch = new TextBox
            {
                Size = new Size(250, 30),
                Location = new Point(0, 10),
                Font = new Font("Segoe UI", 10),
                PlaceholderText = "Rechercher un étudiant..."
            };
            txtSearch.TextChanged += SearchBox_TextChanged;
            
            // Créer le bouton de recherche
            btnSearch = new Button
            {
                Text = "Rechercher",
                Size = new Size(100, 30),
                Location = new Point(txtSearch.Right + 10, 10),
                BackColor = PrimaryColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            
            // Créer le bouton de filtre
            btnFilter = new Button
            {
                Text = "Filtrer",
                Size = new Size(80, 30),
                Location = new Point(btnSearch.Right + 10, 10),
                BackColor = Color.FromArgb(52, 58, 64),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnFilter.Click += BtnFilter_Click;
            
            // Créer le bouton d'ajout
            btnNew = new Button
            {
                Text = "Ajouter",
                Size = new Size(80, 30),
                Location = new Point(btnFilter.Right + 10, 10),
                BackColor = PrimaryColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnNew.Click += BtnNew_Click;
            
            // Créer la grille de données
            dgvStudents = new DataGridView
            {
                Dock = DockStyle.Bottom,
                Height = contentPanel.Height - 50,
                BackgroundColor = LightColor,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Font = new Font("Segoe UI", 9)
            };
            dgvStudents.CellDoubleClick += DgvStudents_CellDoubleClick;
            
            // Configurer les colonnes de la grille
            dgvStudents.Columns.Add("ID", "ID");
            dgvStudents.Columns.Add("Name", "Nom");
            dgvStudents.Columns.Add("Email", "Email");
            dgvStudents.Columns.Add("Phone", "Téléphone");
            dgvStudents.Columns.Add("Address", "Adresse");
            dgvStudents.Columns.Add("Status", "Statut");
            
            // Configurer l'apparence de la grille
            dgvStudents.EnableHeadersVisualStyles = false;
            dgvStudents.ColumnHeadersDefaultCellStyle.BackColor = PrimaryColor;
            dgvStudents.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvStudents.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvStudents.ColumnHeadersHeight = 40;
            
            // Ajouter les contrôles au formulaire
            headerPanel.Controls.Add(lblTitle);
            contentPanel.Controls.Add(txtSearch);
            contentPanel.Controls.Add(btnSearch);
            contentPanel.Controls.Add(btnFilter);
            contentPanel.Controls.Add(btnNew);
            contentPanel.Controls.Add(dgvStudents);
            
            mainLayout.Controls.Add(headerPanel, 0, 0);
            mainLayout.Controls.Add(contentPanel, 0, 1);
            
            this.Controls.Add(mainLayout);
        }
        
        private void LoadStudents()
        {
            try
            {
                // Simuler des données d'étudiants
                // Dans une application réelle, ces données viendraient de la base de données
                students = new List<Member>
                {
                    new Member { Id = 1, Name = "Ahmed Chermiti", Email = "ahmed.chermiti@example.com", Phone = "12345678", Address = "Tunis", Status = "Actif" },
                    new Member { Id = 2, Name = "Sami Ben Ali", Email = "sami.benali@example.com", Phone = "87654321", Address = "Sousse", Status = "Actif" },
                    new Member { Id = 3, Name = "Leila Karoui", Email = "leila.karoui@example.com", Phone = "23456789", Address = "Sfax", Status = "Inactif" },
                    new Member { Id = 4, Name = "Mohamed Trabelsi", Email = "mohamed.trabelsi@example.com", Phone = "98765432", Address = "Monastir", Status = "Actif" },
                    new Member { Id = 5, Name = "Fatma Mansour", Email = "fatma.mansour@example.com", Phone = "34567890", Address = "Bizerte", Status = "Actif" }
                };
                
                // Afficher les étudiants dans la grille
                dgvStudents.Rows.Clear();
                foreach (var student in students)
                {
                    dgvStudents.Rows.Add(student.Id, student.Name, student.Email, student.Phone, student.Address, student.Status);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des étudiants : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void SearchBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string searchText = txtSearch.Text.ToLower();
                
                // Filtrer les étudiants en fonction du texte de recherche
                var filteredStudents = students.Where(s => 
                    s.Name.ToLower().Contains(searchText) || 
                    s.Email.ToLower().Contains(searchText) || 
                    s.Phone.ToLower().Contains(searchText) || 
                    s.Address.ToLower().Contains(searchText) || 
                    s.Status.ToLower().Contains(searchText)
                ).ToList();
                
                // Afficher les étudiants filtrés dans la grille
                dgvStudents.Rows.Clear();
                foreach (var student in filteredStudents)
                {
                    dgvStudents.Rows.Add(student.Id, student.Name, student.Email, student.Phone, student.Address, student.Status);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la recherche : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void BtnFilter_Click(object sender, EventArgs e)
        {
            try
            {
                // Implémenter la fonctionnalité de filtre
                // Par exemple, afficher une boîte de dialogue avec des options de filtre
                MessageBox.Show("Fonctionnalité de filtre à implémenter", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du filtrage : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void BtnNew_Click(object sender, EventArgs e)
        {
            try
            {
                // Implémenter la fonctionnalité d'ajout d'étudiant
                // Par exemple, ouvrir un formulaire pour ajouter un nouvel étudiant
                MessageBox.Show("Fonctionnalité d'ajout d'étudiant à implémenter", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'ajout d'un étudiant : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void DgvStudents_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // Vérifier si une ligne valide est sélectionnée
                if (e.RowIndex >= 0)
                {
                    int studentId = Convert.ToInt32(dgvStudents.Rows[e.RowIndex].Cells["ID"].Value);
                    var student = students.FirstOrDefault(s => s.Id == studentId);
                    
                    if (student != null)
                    {
                        // Ici, vous pouvez implémenter la logique pour afficher les détails de l'étudiant
                        // Par exemple, ouvrir un formulaire avec les détails de l'étudiant
                        MessageBox.Show($"Édition de l'étudiant : {student.Name}", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'édition d'un étudiant : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void CreateSidebar()
        {
            // Créer le panneau de la sidebar
            Panel sidebarPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 250,
                BackColor = PrimaryColor,
                Padding = new Padding(10)
            };
            
            // Créer les éléments du menu
            string[] menuItems = new string[]
            {
                "Retour",
                "Ajouter un étudiant",
                "Modifier un étudiant",
                "Supprimer un étudiant",
                "Exporter la liste"
            };
            
            int buttonY = 20;
            foreach (string item in menuItems)
            {
                Button btnMenuItem = new Button
                {
                    Text = item,
                    BackColor = PrimaryColor,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Dock = DockStyle.Top,
                    Height = 50,
                    Font = new Font("Segoe UI", 10, FontStyle.Regular),
                    TextAlign = ContentAlignment.MiddleLeft,
                    Margin = new Padding(0, 5, 0, 5)
                };
                
                btnMenuItem.FlatAppearance.BorderSize = 0;
                
                if (item == "Retour")
                {
                    btnMenuItem.Click += (s, e) => ReturnToDashboard();
                }
                else if (item == "Ajouter un étudiant")
                {
                    btnMenuItem.Click += (s, e) => AddNewStudent();
                }
                else if (item == "Modifier un étudiant")
                {
                    btnMenuItem.Click += (s, e) => EditSelectedStudent();
                }
                else if (item == "Supprimer un étudiant")
                {
                    btnMenuItem.Click += (s, e) => DeleteSelectedStudent();
                }
                else if (item == "Exporter la liste")
                {
                    btnMenuItem.Click += (s, e) => ExportStudentList();
                }
                
                sidebarPanel.Controls.Add(btnMenuItem);
                buttonY += 60;
            }
            
            this.Controls.Add(sidebarPanel);
        }

        // Méthode pour retourner au tableau de bord
        private void ReturnToDashboard()
        {
            this.Close();
        }

        // Méthode pour ajouter un nouvel étudiant
        private void AddNewStudent()
        {
            try
            {
                // Créer un formulaire pour ajouter un nouvel étudiant
                // Ici, vous pouvez implémenter un formulaire spécifique pour l'ajout d'étudiant
                MessageBox.Show("Fonctionnalité d'ajout d'étudiant à implémenter", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'ajout d'un étudiant : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Méthode pour modifier un étudiant sélectionné
        private void EditSelectedStudent()
        {
            try
            {
                // Vérifier si un étudiant est sélectionné
                if (dgvStudents.SelectedRows.Count > 0)
                {
                    int studentId = Convert.ToInt32(dgvStudents.SelectedRows[0].Cells["ID"].Value);
                    var student = students.FirstOrDefault(s => s.Id == studentId);
                    
                    if (student != null)
                    {
                        // Ici, vous pouvez implémenter la logique pour modifier un étudiant sélectionné
                        MessageBox.Show($"Modification de l'étudiant : {student.Name}", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Veuillez sélectionner un étudiant à modifier", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la modification d'un étudiant : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Méthode pour supprimer un étudiant sélectionné
        private void DeleteSelectedStudent()
        {
            try
            {
                // Vérifier si un étudiant est sélectionné
                if (dgvStudents.SelectedRows.Count > 0)
                {
                    int studentId = Convert.ToInt32(dgvStudents.SelectedRows[0].Cells["ID"].Value);
                    var student = students.FirstOrDefault(s => s.Id == studentId);
                    
                    if (student != null)
                    {
                        // Demander confirmation avant de supprimer
                        DialogResult result = MessageBox.Show($"Voulez-vous vraiment supprimer l'étudiant {student.Name} ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        
                        if (result == DialogResult.Yes)
                        {
                            // Ici, vous pouvez implémenter la logique pour supprimer un étudiant sélectionné
                            MessageBox.Show($"Suppression de l'étudiant : {student.Name}", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Veuillez sélectionner un étudiant à supprimer", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la suppression d'un étudiant : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Méthode pour exporter la liste des étudiants
        private void ExportStudentList()
        {
            try
            {
                // Implémenter la fonctionnalité d'exportation de la liste des étudiants
                // Par exemple, exporter vers Excel, CSV, PDF, etc.
                MessageBox.Show("Fonctionnalité d'exportation de la liste des étudiants à implémenter", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'exportation de la liste des étudiants : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
