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
    public class SettingsForm : Form
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
        private TabControl tabSettings;
        private TabPage tabGeneral;
        private TabPage tabAccount;
        private TabPage tabNotifications;
        private TabPage tabDatabase;
        private Button btnSave;

        // Contexte de la base de données
        private readonly LibraryContext _context;

        public SettingsForm(LibraryContext context = null)
        {
            _context = context ?? new LibraryContext(new DbContextOptionsBuilder<LibraryContext>().Options);
            InitializeComponent();
            LoadSettings();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Configuration du formulaire
            this.ClientSize = new System.Drawing.Size(900, 600);
            this.Text = "BiblioHub - Paramètres";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = BackgroundColor;
            
            // Création du layout principal
            CreateLayout();
            
            // Création de la sidebar
            CreateSidebar();
            
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
                    BackColor = item == "Paramètres" ? Color.FromArgb(40, 50, 90) : Color.Transparent,
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

        private void CreateHeader()
        {
            // Panneau d'en-tête
            headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 100,
                BackColor = BackgroundColor,
                Padding = new Padding(0, 0, 0, 20)
            };

            // Titre
            lblTitle = new Label
            {
                Text = "Paramètres",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = TextColor,
                AutoSize = true,
                Location = new Point(0, 30)
            };

            // Ajouter les contrôles à l'en-tête
            headerPanel.Controls.Add(lblTitle);

            // Ajouter l'en-tête au contenu
            contentPanel.Controls.Add(headerPanel);
        }

        private void CreateContent()
        {
            // Onglets de paramètres
            tabSettings = new TabControl
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10),
                Padding = new Point(20, 8)
            };

            // Onglet Général
            tabGeneral = new TabPage("Général");
            CreateGeneralTab();

            // Onglet Compte
            tabAccount = new TabPage("Compte");
            CreateAccountTab();

            // Onglet Notifications
            tabNotifications = new TabPage("Notifications");
            CreateNotificationsTab();

            // Onglet Base de données
            tabDatabase = new TabPage("Base de données");
            CreateDatabaseTab();

            // Ajouter les onglets au contrôle
            tabSettings.Controls.Add(tabGeneral);
            tabSettings.Controls.Add(tabAccount);
            tabSettings.Controls.Add(tabNotifications);
            tabSettings.Controls.Add(tabDatabase);

            // Bouton Enregistrer
            btnSave = new Button
            {
                Text = "Enregistrer",
                Size = new Size(120, 40),
                Location = new Point(contentPanel.Width - 150, contentPanel.Height - 70),
                BackColor = SuccessColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnSave.Click += BtnSave_Click;

            // Ajouter les contrôles au contenu
            contentPanel.Controls.Add(tabSettings);
            contentPanel.Controls.Add(btnSave);
        }

        private void CreateGeneralTab()
        {
            // Contenu de l'onglet Général
            Panel generalPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20)
            };

            // Thème
            Label lblTheme = new Label
            {
                Text = "Thème",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 30)
            };

            ComboBox cmbTheme = new ComboBox
            {
                Size = new Size(200, 30),
                Location = new Point(200, 30),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbTheme.Items.AddRange(new object[] { "Clair", "Sombre", "Système" });
            cmbTheme.SelectedIndex = 0;

            // Langue
            Label lblLanguage = new Label
            {
                Text = "Langue",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 80)
            };

            ComboBox cmbLanguage = new ComboBox
            {
                Size = new Size(200, 30),
                Location = new Point(200, 80),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbLanguage.Items.AddRange(new object[] { "Français", "Anglais", "Arabe" });
            cmbLanguage.SelectedIndex = 0;

            // Format de date
            Label lblDateFormat = new Label
            {
                Text = "Format de date",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 130)
            };

            ComboBox cmbDateFormat = new ComboBox
            {
                Size = new Size(200, 30),
                Location = new Point(200, 130),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbDateFormat.Items.AddRange(new object[] { "JJ/MM/AAAA", "MM/JJ/AAAA", "AAAA-MM-JJ" });
            cmbDateFormat.SelectedIndex = 0;

            // Ajouter les contrôles au panel
            generalPanel.Controls.Add(lblTheme);
            generalPanel.Controls.Add(cmbTheme);
            generalPanel.Controls.Add(lblLanguage);
            generalPanel.Controls.Add(cmbLanguage);
            generalPanel.Controls.Add(lblDateFormat);
            generalPanel.Controls.Add(cmbDateFormat);

            // Ajouter le panel à l'onglet
            tabGeneral.Controls.Add(generalPanel);
        }

        private void CreateAccountTab()
        {
            // Contenu de l'onglet Compte
            Panel accountPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20)
            };

            // Nom d'utilisateur
            Label lblUsername = new Label
            {
                Text = "Nom d'utilisateur",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 30)
            };

            TextBox txtUsername = new TextBox
            {
                Size = new Size(250, 30),
                Location = new Point(200, 30),
                Text = "Ahmed Chermiti"
            };

            // Email
            Label lblEmail = new Label
            {
                Text = "Email",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 80)
            };

            TextBox txtEmail = new TextBox
            {
                Size = new Size(250, 30),
                Location = new Point(200, 80),
                Text = "ahmed.chermiti.2024@ihec.ucar.tn"
            };

            // Mot de passe actuel
            Label lblCurrentPassword = new Label
            {
                Text = "Mot de passe actuel",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 130)
            };

            TextBox txtCurrentPassword = new TextBox
            {
                Size = new Size(250, 30),
                Location = new Point(200, 130),
                PasswordChar = '•'
            };

            // Nouveau mot de passe
            Label lblNewPassword = new Label
            {
                Text = "Nouveau mot de passe",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 180)
            };

            TextBox txtNewPassword = new TextBox
            {
                Size = new Size(250, 30),
                Location = new Point(200, 180),
                PasswordChar = '•'
            };

            // Confirmer le mot de passe
            Label lblConfirmPassword = new Label
            {
                Text = "Confirmer le mot de passe",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 230)
            };

            TextBox txtConfirmPassword = new TextBox
            {
                Size = new Size(250, 30),
                Location = new Point(200, 230),
                PasswordChar = '•'
            };

            // Ajouter les contrôles au panel
            accountPanel.Controls.Add(lblUsername);
            accountPanel.Controls.Add(txtUsername);
            accountPanel.Controls.Add(lblEmail);
            accountPanel.Controls.Add(txtEmail);
            accountPanel.Controls.Add(lblCurrentPassword);
            accountPanel.Controls.Add(txtCurrentPassword);
            accountPanel.Controls.Add(lblNewPassword);
            accountPanel.Controls.Add(txtNewPassword);
            accountPanel.Controls.Add(lblConfirmPassword);
            accountPanel.Controls.Add(txtConfirmPassword);

            // Ajouter le panel à l'onglet
            tabAccount.Controls.Add(accountPanel);
        }

        private void CreateNotificationsTab()
        {
            // Contenu de l'onglet Notifications
            Panel notificationsPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20)
            };

            // Activer les notifications
            CheckBox chkEnableNotifications = new CheckBox
            {
                Text = "Activer les notifications",
                Font = new Font("Segoe UI", 10),
                AutoSize = true,
                Location = new Point(20, 30),
                Checked = true
            };

            // Notifications par email
            CheckBox chkEmailNotifications = new CheckBox
            {
                Text = "Recevoir les notifications par email",
                Font = new Font("Segoe UI", 10),
                AutoSize = true,
                Location = new Point(20, 70),
                Checked = true
            };

            // Notifications de retard
            CheckBox chkLateNotifications = new CheckBox
            {
                Text = "Notifications de retard",
                Font = new Font("Segoe UI", 10),
                AutoSize = true,
                Location = new Point(20, 110),
                Checked = true
            };

            // Notifications de nouveaux livres
            CheckBox chkNewBooksNotifications = new CheckBox
            {
                Text = "Notifications de nouveaux livres",
                Font = new Font("Segoe UI", 10),
                AutoSize = true,
                Location = new Point(20, 150),
                Checked = true
            };

            // Notifications de disponibilité
            CheckBox chkAvailabilityNotifications = new CheckBox
            {
                Text = "Notifications de disponibilité",
                Font = new Font("Segoe UI", 10),
                AutoSize = true,
                Location = new Point(20, 190),
                Checked = true
            };

            // Fréquence des notifications
            Label lblNotificationFrequency = new Label
            {
                Text = "Fréquence des notifications",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 240)
            };

            ComboBox cmbNotificationFrequency = new ComboBox
            {
                Size = new Size(200, 30),
                Location = new Point(250, 240),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbNotificationFrequency.Items.AddRange(new object[] { "Immédiatement", "Quotidiennement", "Hebdomadairement" });
            cmbNotificationFrequency.SelectedIndex = 0;

            // Ajouter les contrôles au panel
            notificationsPanel.Controls.Add(chkEnableNotifications);
            notificationsPanel.Controls.Add(chkEmailNotifications);
            notificationsPanel.Controls.Add(chkLateNotifications);
            notificationsPanel.Controls.Add(chkNewBooksNotifications);
            notificationsPanel.Controls.Add(chkAvailabilityNotifications);
            notificationsPanel.Controls.Add(lblNotificationFrequency);
            notificationsPanel.Controls.Add(cmbNotificationFrequency);

            // Ajouter le panel à l'onglet
            tabNotifications.Controls.Add(notificationsPanel);
        }

        private void CreateDatabaseTab()
        {
            // Contenu de l'onglet Base de données
            Panel databasePanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20)
            };

            // Chaîne de connexion
            Label lblConnectionString = new Label
            {
                Text = "Chaîne de connexion",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 30)
            };

            TextBox txtConnectionString = new TextBox
            {
                Size = new Size(400, 30),
                Location = new Point(20, 60),
                Text = "Data Source=localhost;Initial Catalog=BiblioHub;Integrated Security=True",
                Multiline = true,
                Height = 60
            };

            // Bouton de test de connexion
            Button btnTestConnection = new Button
            {
                Text = "Tester la connexion",
                Size = new Size(150, 30),
                Location = new Point(20, 130),
                BackColor = PrimaryColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            // Sauvegarde de la base de données
            Label lblBackup = new Label
            {
                Text = "Sauvegarde de la base de données",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 180)
            };

            Button btnBackup = new Button
            {
                Text = "Sauvegarder maintenant",
                Size = new Size(180, 30),
                Location = new Point(20, 210),
                BackColor = PrimaryColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            // Restauration de la base de données
            Label lblRestore = new Label
            {
                Text = "Restauration de la base de données",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 260)
            };

            Button btnRestore = new Button
            {
                Text = "Restaurer",
                Size = new Size(180, 30),
                Location = new Point(20, 290),
                BackColor = PrimaryColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            // Ajouter les contrôles au panel
            databasePanel.Controls.Add(lblConnectionString);
            databasePanel.Controls.Add(txtConnectionString);
            databasePanel.Controls.Add(btnTestConnection);
            databasePanel.Controls.Add(lblBackup);
            databasePanel.Controls.Add(btnBackup);
            databasePanel.Controls.Add(lblRestore);
            databasePanel.Controls.Add(btnRestore);

            // Ajouter le panel à l'onglet
            tabDatabase.Controls.Add(databasePanel);
        }

        private void LoadSettings()
        {
            try
            {
                // Charger les paramètres depuis la base de données ou un fichier de configuration
                // Dans cet exemple, nous utilisons des valeurs par défaut
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des paramètres : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Enregistrer les paramètres
                MessageBox.Show("Paramètres enregistrés avec succès !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'enregistrement des paramètres : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                    case "Gérer les emprunts":
                        this.Close();
                        if (_context != null)
                        {
                            var loanForm = new LoanManagementForm(_context);
                            loanForm.ShowDialog();
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
                    // Paramètres est déjà ouvert, donc pas besoin de faire quoi que ce soit
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la navigation : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
