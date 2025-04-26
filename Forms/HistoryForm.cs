using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using projet_bibliotheque.Data;
using projet_bibliotheque.Models;

namespace projet_bibliotheque.Forms
{
    public class HistoryForm : Form
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

        // Composants du formulaire
        private Panel sidebarPanel;
        private Panel contentPanel;
        private Panel headerPanel;
        private Label lblTitle;
        private ComboBox cmbPeriod;
        private DataGridView dgvHistory;
        private Button btnExport;

        // Contexte de la base de données
        private readonly LibraryContext _context;

        public HistoryForm(LibraryContext context = null)
        {
            _context = context ?? new LibraryContext(new DbContextOptionsBuilder<LibraryContext>().Options);
            InitializeComponent();
            LoadHistory();
        }

        private void InitializeComponent()
        {
            // Configuration du formulaire
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.Text = "Historique des Activités";
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
                Text = "Historique des Activités",
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
            string[] menuItems = { "Tableau de bord", "Gérer la bibliothèque", "Gérer les étudiants", "Notifications", "Historique", "Paramètres" };
            int menuY = 150;

            foreach (string item in menuItems)
            {
                Button btnMenuItem = new Button
                {
                    Text = "  " + item,
                    TextAlign = ContentAlignment.MiddleLeft,
                    FlatStyle = FlatStyle.Flat,
                    BackColor = item == "Historique" ? Color.FromArgb(40, 50, 90) : Color.Transparent,
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
                    case "Paramètres":
                        this.Close();
                        if (_context != null)
                        {
                            var settingsForm = new SettingsForm(_context);
                            settingsForm.ShowDialog();
                        }
                        break;
                    // Historique est déjà ouvert, donc pas besoin de faire quoi que ce soit
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

            // Créer le filtre de période
            Panel filterPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = Color.Transparent
            };

            Label lblPeriod = new Label
            {
                Text = "Période :",
                Font = new Font("Segoe UI", 10),
                ForeColor = TextColor,
                AutoSize = true,
                Location = new Point(0, 15)
            };

            cmbPeriod = new ComboBox
            {
                Size = new Size(200, 30),
                Location = new Point(lblPeriod.Right + 10, 10),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };

            cmbPeriod.Items.AddRange(new object[] { "Aujourd'hui", "Cette semaine", "Ce mois", "Cette année", "Tout" });
            cmbPeriod.SelectedIndex = 0;
            cmbPeriod.SelectedIndexChanged += CmbPeriod_SelectedIndexChanged;

            btnExport = new Button
            {
                Text = "Exporter",
                Size = new Size(100, 30),
                Location = new Point(contentPanel.Width - 120, 10),
                FlatStyle = FlatStyle.Flat,
                BackColor = PrimaryColor,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10)
            };
            btnExport.Click += BtnExport_Click;

            filterPanel.Controls.Add(lblPeriod);
            filterPanel.Controls.Add(cmbPeriod);
            filterPanel.Controls.Add(btnExport);

            // Créer la grille de données
            dgvHistory = new DataGridView
            {
                Dock = DockStyle.Fill,
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
            dgvHistory.Columns.Add("ID", "ID");
            dgvHistory.Columns.Add("Date", "Date");
            dgvHistory.Columns.Add("Type", "Type");
            dgvHistory.Columns.Add("Description", "Description");
            dgvHistory.Columns.Add("Utilisateur", "Utilisateur");

            // Configurer l'apparence de la grille
            dgvHistory.EnableHeadersVisualStyles = false;
            dgvHistory.ColumnHeadersDefaultCellStyle.BackColor = PrimaryColor;
            dgvHistory.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvHistory.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvHistory.ColumnHeadersHeight = 40;

            // Ajouter les contrôles au panneau de contenu
            contentPanel.Controls.Add(dgvHistory);
            contentPanel.Controls.Add(filterPanel);

            // Ajouter le panneau de contenu au layout principal
            mainLayout.Controls.Add(contentPanel, 0, 1);
        }

        private void LoadHistory()
        {
            try
            {
                // Simuler des données d'historique
                // Dans une application réelle, ces données viendraient de la base de données
                dgvHistory.Rows.Clear();
                
                // Exemple de données
                dgvHistory.Rows.Add("1", DateTime.Now.ToString("dd/MM/yyyy HH:mm"), "Emprunt", "Livre 'Le Petit Prince' emprunté", "Ahmed Chermiti");
                dgvHistory.Rows.Add("2", DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy HH:mm"), "Retour", "Livre 'Harry Potter' retourné", "Sami Ben Ali");
                dgvHistory.Rows.Add("3", DateTime.Now.AddDays(-2).ToString("dd/MM/yyyy HH:mm"), "Inscription", "Nouvel étudiant inscrit", "Admin");
                dgvHistory.Rows.Add("4", DateTime.Now.AddDays(-3).ToString("dd/MM/yyyy HH:mm"), "Ajout", "Nouveau livre ajouté: 'Les Misérables'", "Admin");
                dgvHistory.Rows.Add("5", DateTime.Now.AddDays(-4).ToString("dd/MM/yyyy HH:mm"), "Emprunt", "Livre 'L'Étranger' emprunté", "Leila Karoui");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement de l'historique : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CmbPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Filtrer les données en fonction de la période sélectionnée
            LoadHistory();
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                // Implémenter la fonctionnalité d'exportation
                MessageBox.Show("Fonctionnalité d'exportation à implémenter", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'exportation : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
