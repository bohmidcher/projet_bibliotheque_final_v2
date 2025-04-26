using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using projet_bibliotheque.Data;
using projet_bibliotheque.Models;
using projet_bibliotheque.Utils;
using projet_bibliotheque.Views;

namespace projet_bibliotheque.Forms
{
    public partial class StudentManagementForm : Form
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
        private Label lblTitle;
        private MembersView membersView;

        // Contexte de la base de données
        private readonly LibraryContext _context;

        public StudentManagementForm(LibraryContext context)
        {
            _context = context;
            InitializeComponent();
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
                Font = new Font("Poppins", 24, FontStyle.Bold),
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
            
            // Créer la vue des membres
            membersView = new MembersView();
            membersView.Dock = DockStyle.Fill;
            
            // Ajouter les contrôles au formulaire
            headerPanel.Controls.Add(lblTitle);
            contentPanel.Controls.Add(membersView);
            
            mainLayout.Controls.Add(headerPanel, 0, 0);
            mainLayout.Controls.Add(contentPanel, 0, 1);
            
            this.Controls.Add(mainLayout);
        }

        private void CreateSidebar()
        {
            // Créer le panneau de la sidebar s'il n'existe pas déjà
            Panel sidebarPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 250,
                BackColor = SidebarColor,
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
                    BackColor = SidebarColor,
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
                // Ici, vous pouvez implémenter la logique pour modifier un étudiant sélectionné
                MessageBox.Show("Fonctionnalité de modification d'étudiant à implémenter", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                // Ici, vous pouvez implémenter la logique pour supprimer un étudiant sélectionné
                MessageBox.Show("Fonctionnalité de suppression d'étudiant à implémenter", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
