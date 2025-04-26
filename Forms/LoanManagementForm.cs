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
    public class LoanManagementForm : Form
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
        private Panel sidebarPanel;
        private Panel contentPanel;
        private Panel headerPanel;
        private TextBox searchBox;
        private Button btnFilter;
        private Button btnNew;
        private Label lblTitle;
        private Label lblLoans;
        private Label lblLoanCount;
        private DataGridView dgvLoans;

        // Contexte de la base de données
        private readonly LibraryContext _context;
        private List<Loan> loans;

        public LoanManagementForm(LibraryContext context = null)
        {
            _context = context ?? new LibraryContext(new DbContextOptionsBuilder<LibraryContext>().Options);
            InitializeComponent();
            LoadLoans();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Configuration du formulaire
            this.ClientSize = new Size(1200, 800);
            this.Text = "BiblioHub - Gérer les emprunts";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = BackgroundColor;
            
            // Création du layout principal
            TableLayoutPanel mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                Padding = new Padding(20)
            };
            
            // Configurer les styles de ligne
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F));
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
                Text = "Gérer les emprunts",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = PrimaryColor,
                AutoSize = true,
                Location = new Point(0, 10)
            };
            
            // Créer la sidebar
            CreateSidebar();
            
            // Ajouter les contrôles à l'en-tête
            headerPanel.Controls.Add(lblTitle);
            mainLayout.Controls.Add(headerPanel, 0, 0);
            
            // Créer le contenu principal
            CreateContent(mainLayout);
            
            this.Controls.Add(mainLayout);
            
            this.ResumeLayout(false);
        }
        
        private void CreateSidebar()
        {
            // Panneau de la sidebar
            sidebarPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 220,
                BackColor = PrimaryColor
            };

            // Logo en haut de la sidebar
            PictureBox logoBox = new PictureBox
            {
                Size = new Size(80, 80),
                Location = new Point(70, 30),
                BackColor = Color.Transparent,
                SizeMode = PictureBoxSizeMode.Zoom
            };

            try
            {
                logoBox.Image = Image.FromFile("Resources/logo.png");
            }
            catch
            {
                // Image par défaut si le logo n'est pas trouvé
            }

            // Menu items
            string[] menuItems = { "Tableau de bord", "Gérer la bibliothèque", "Gérer les étudiants", "Gérer les emprunts", "Notifications", "Historique", "Paramètres" };
            int menuY = 150;

            foreach (string item in menuItems)
            {
                Button btnMenuItem = new Button
                {
                    Text = "  " + item,
                    TextAlign = ContentAlignment.MiddleLeft,
                    FlatStyle = FlatStyle.Flat,
                    BackColor = item == "Gérer les emprunts" ? Color.FromArgb(40, 50, 90) : Color.Transparent,
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 10, FontStyle.Regular),
                    Size = new Size(220, 40),
                    Location = new Point(0, menuY),
                    Cursor = Cursors.Hand,
                    Tag = item
                };

                btnMenuItem.FlatAppearance.BorderSize = 0;
                btnMenuItem.FlatAppearance.MouseOverBackColor = Color.FromArgb(40, 50, 90);
                btnMenuItem.Click += BtnMenuItem_Click;

                sidebarPanel.Controls.Add(btnMenuItem);
                menuY += 40;
            }

            // Ajouter les contrôles à la sidebar
            sidebarPanel.Controls.Add(logoBox);

            // Ajouter la sidebar au formulaire
            this.Controls.Add(sidebarPanel);
        }
        
        private void BtnMenuItem_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            string menuItem = clickedButton?.Tag?.ToString();
            
            try
            {
                switch (menuItem)
                {
                    case "Tableau de bord":
                        this.Close();
                        break;
                    case "Gérer la bibliothèque":
                        this.Close();
                        if (_context != null)
                        {
                            var libraryForm = new LibraryManagementForm(_context);
                            libraryForm.ShowDialog();
                        }
                        break;
                    case "Gérer les étudiants":
                        this.Close();
                        if (_context != null)
                        {
                            var studentForm = new StudentManagementForm(_context);
                            studentForm.ShowDialog();
                        }
                        break;
                    case "Notifications":
                        this.Close();
                        if (_context != null)
                        {
                            var notificationsForm = new NotificationsForm(_context);
                            notificationsForm.ShowDialog();
                        }
                        break;
                    case "Historique":
                        this.Close();
                        if (_context != null)
                        {
                            var historyForm = new HistoryForm(_context);
                            historyForm.ShowDialog();
                        }
                        break;
                    case "Paramètres":
                        this.Close();
                        if (_context != null)
                        {
                            var settingsForm = new SettingsForm(_context);
                            settingsForm.ShowDialog();
                        }
                        break;
                    // Gérer les emprunts est déjà ouvert, donc pas besoin de faire quoi que ce soit
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la navigation : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CreateContent(TableLayoutPanel mainLayout)
        {
            // Panneau de contenu principal
            contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = BackgroundColor,
                Padding = new Padding(20)
            };

            // Barre de recherche
            searchBox = new TextBox
            {
                Size = new Size(300, 40),
                Location = new Point(0, 10),
                Font = new Font("Segoe UI", 12),
                PlaceholderText = "Rechercher des emprunts...",
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(10, 5, 10, 5)
            };

            // Bouton de filtre
            btnFilter = new Button
            {
                Text = "Filtrer",
                Size = new Size(100, 40),
                Location = new Point(searchBox.Right + 20, 10),
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
                Text = "Nouvel emprunt",
                Size = new Size(150, 40),
                Location = new Point(contentPanel.Width - 150, 10),
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

            // Titre de la section Emprunts
            lblLoans = new Label
            {
                Text = "Emprunts",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = TextColor,
                AutoSize = true,
                Location = new Point(0, searchBox.Bottom + 20)
            };

            // Nombre d'emprunts
            lblLoanCount = new Label
            {
                Text = "0 emprunts",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(lblLoans.Right + 20, lblLoans.Top + 5)
            };

            // Grille des emprunts
            dgvLoans = new DataGridView
            {
                Location = new Point(0, lblLoans.Bottom + 20),
                Size = new Size(contentPanel.Width - 40, contentPanel.Height - lblLoans.Bottom - 60),
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

            // Configurer les colonnes de la grille
            dgvLoans.Columns.Add("ID", "ID");
            dgvLoans.Columns.Add("Book", "Livre");
            dgvLoans.Columns.Add("Student", "Étudiant");
            dgvLoans.Columns.Add("BorrowDate", "Date d'emprunt");
            dgvLoans.Columns.Add("ReturnDate", "Date de retour prévue");
            dgvLoans.Columns.Add("Status", "Statut");
            dgvLoans.Columns.Add("Actions", "Actions");

            // Configurer l'apparence de la grille
            dgvLoans.EnableHeadersVisualStyles = false;
            dgvLoans.ColumnHeadersDefaultCellStyle.BackColor = PrimaryColor;
            dgvLoans.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvLoans.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvLoans.ColumnHeadersHeight = 40;

            // Ajouter les contrôles au panneau de contenu
            contentPanel.Controls.Add(searchBox);
            contentPanel.Controls.Add(btnFilter);
            contentPanel.Controls.Add(btnNew);
            contentPanel.Controls.Add(lblLoans);
            contentPanel.Controls.Add(lblLoanCount);
            contentPanel.Controls.Add(dgvLoans);

            // Ajouter le panneau de contenu au layout principal
            mainLayout.Controls.Add(contentPanel, 0, 1);

            // Événements
            searchBox.TextChanged += SearchBox_TextChanged;
            btnFilter.Click += BtnFilter_Click;
            btnNew.Click += BtnNew_Click;
            dgvLoans.CellClick += DgvLoans_CellClick;
        }

        private void LoadLoans()
        {
            try
            {
                // Simuler des données d'emprunts
                // Dans une application réelle, ces données viendraient de la base de données
                loans = new List<Loan>();
                dgvLoans.Rows.Clear();
                
                // Exemple de données
                dgvLoans.Rows.Add("1", "Le Petit Prince", "Ahmed Chermiti", "01/04/2025", "15/04/2025", "En cours", "Retourner");
                dgvLoans.Rows.Add("2", "Harry Potter", "Sami Ben Ali", "25/03/2025", "08/04/2025", "En retard", "Retourner");
                dgvLoans.Rows.Add("3", "Les Misérables", "Leila Karoui", "15/03/2025", "29/03/2025", "Retourné", "");
                dgvLoans.Rows.Add("4", "L'Étranger", "Mohamed Trabelsi", "10/04/2025", "24/04/2025", "En cours", "Retourner");
                dgvLoans.Rows.Add("5", "Le Rouge et le Noir", "Fatma Mansour", "05/04/2025", "19/04/2025", "En cours", "Retourner");
                
                // Mettre à jour le compteur d'emprunts
                lblLoanCount.Text = $"{dgvLoans.Rows.Count} emprunts";
                
                // Colorer les cellules de statut
                foreach (DataGridViewRow row in dgvLoans.Rows)
                {
                    string status = row.Cells["Status"].Value.ToString();
                    if (status == "En cours")
                    {
                        row.Cells["Status"].Style.ForeColor = SuccessColor;
                    }
                    else if (status == "En retard")
                    {
                        row.Cells["Status"].Style.ForeColor = DangerColor;
                    }
                    else if (status == "Retourné")
                    {
                        row.Cells["Status"].Style.ForeColor = Color.Gray;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des emprunts : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SearchBox_TextChanged(object sender, EventArgs e)
        {
            // Filtrer les données en fonction du texte de recherche
            LoadLoans();
        }

        private void BtnFilter_Click(object sender, EventArgs e)
        {
            try
            {
                // Implémenter la fonctionnalité de filtre
                MessageBox.Show("Fonctionnalité de filtre à implémenter", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du filtrage des emprunts : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            try
            {
                // Implémenter la fonctionnalité d'ajout d'un nouvel emprunt
                MessageBox.Show("Fonctionnalité d'ajout d'un nouvel emprunt à implémenter", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'ajout d'un nouvel emprunt : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvLoans_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // Vérifier si l'utilisateur a cliqué sur la colonne Actions
                if (e.ColumnIndex == dgvLoans.Columns["Actions"].Index && e.RowIndex >= 0)
                {
                    string status = dgvLoans.Rows[e.RowIndex].Cells["Status"].Value.ToString();
                    if (status != "Retourné")
                    {
                        // Demander confirmation avant de retourner le livre
                        string bookTitle = dgvLoans.Rows[e.RowIndex].Cells["Book"].Value.ToString();
                        DialogResult result = MessageBox.Show($"Voulez-vous vraiment marquer le livre '{bookTitle}' comme retourné ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        
                        if (result == DialogResult.Yes)
                        {
                            // Marquer le livre comme retourné
                            dgvLoans.Rows[e.RowIndex].Cells["Status"].Value = "Retourné";
                            dgvLoans.Rows[e.RowIndex].Cells["Status"].Style.ForeColor = Color.Gray;
                            dgvLoans.Rows[e.RowIndex].Cells["Actions"].Value = "";
                            MessageBox.Show($"Le livre '{bookTitle}' a été marqué comme retourné.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du traitement de l'action : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
