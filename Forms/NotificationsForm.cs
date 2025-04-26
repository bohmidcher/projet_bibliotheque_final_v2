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
    public class NotificationsForm : Form
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
        private Label lblNewNotifications;
        private Label lblOlderNotifications;
        private FlowLayoutPanel newNotificationsPanel;
        private FlowLayoutPanel olderNotificationsPanel;

        // Contexte de la base de données
        private readonly LibraryContext _context;

        public NotificationsForm(LibraryContext context = null)
        {
            _context = context ?? new LibraryContext(new DbContextOptionsBuilder<LibraryContext>().Options);
            InitializeComponent();
            LoadNotifications();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Configuration du formulaire
            this.ClientSize = new Size(1200, 800);
            this.Text = "BiblioHub - Notifications";
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

            // Avatar et nom d'utilisateur
            Panel userPanel = new Panel
            {
                Size = new Size(220, 80),
                Location = new Point(0, 120),
                BackColor = Color.Transparent
            };

            PictureBox avatarBox = new PictureBox
            {
                Size = new Size(50, 50),
                Location = new Point(20, 15),
                BackColor = Color.Gray,
                SizeMode = PictureBoxSizeMode.StretchImage
            };

            // Créer un cercle pour l'avatar
            avatarBox.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using (SolidBrush brush = new SolidBrush(Color.Gray))
                {
                    e.Graphics.FillEllipse(brush, 0, 0, avatarBox.Width, avatarBox.Height);
                }
            };

            Label lblUserName = new Label
            {
                Text = "Ahmed Chermiti",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(80, 15),
                AutoSize = true
            };

            Label lblUserEmail = new Label
            {
                Text = "ahmed.chermiti.2024@ihec.ucar.tn",
                ForeColor = Color.Silver,
                Font = new Font("Segoe UI", 8),
                Location = new Point(80, 35),
                AutoSize = true
            };

            userPanel.Controls.Add(avatarBox);
            userPanel.Controls.Add(lblUserName);
            userPanel.Controls.Add(lblUserEmail);

            // Menu items
            string[] menuItems = { "Tableau de bord", "Gérer la bibliothèque", "Gérer les étudiants", "Notifications", "Historique", "Paramètres" };
            int menuY = 210;

            foreach (string item in menuItems)
            {
                Button btnMenuItem = new Button
                {
                    Text = "  " + item,
                    TextAlign = ContentAlignment.MiddleLeft,
                    FlatStyle = FlatStyle.Flat,
                    BackColor = item == "Notifications" ? Color.FromArgb(40, 50, 90) : Color.Transparent,
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
            sidebarPanel.Controls.Add(userPanel);

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
                    // Notifications est déjà ouvert, donc pas besoin de faire quoi que ce soit
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la navigation : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                Text = "Notifications",
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
            // Section des nouvelles notifications
            lblNewNotifications = new Label
            {
                Text = "Nouveau",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = TextColor,
                AutoSize = true,
                Location = new Point(0, headerPanel.Bottom + 20)
            };

            newNotificationsPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                AutoScroll = true,
                WrapContents = false,
                Size = new Size(contentPanel.Width - 60, 200),
                Location = new Point(0, lblNewNotifications.Bottom + 20),
                BackColor = BackgroundColor
            };

            // Section des anciennes notifications
            lblOlderNotifications = new Label
            {
                Text = "Il y a un jour",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = TextColor,
                AutoSize = true,
                Location = new Point(0, newNotificationsPanel.Bottom + 30)
            };

            olderNotificationsPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                AutoScroll = true,
                WrapContents = false,
                Size = new Size(contentPanel.Width - 60, 200),
                Location = new Point(0, lblOlderNotifications.Bottom + 20),
                BackColor = BackgroundColor
            };

            // Ajouter les contrôles au contenu
            contentPanel.Controls.Add(lblNewNotifications);
            contentPanel.Controls.Add(newNotificationsPanel);
            contentPanel.Controls.Add(lblOlderNotifications);
            contentPanel.Controls.Add(olderNotificationsPanel);
        }

        private void LoadNotifications()
        {
            // Exemple de notification de succès
            CreateNotification(
                newNotificationsPanel,
                "Le livre \"Économie Internationale\" a été emprunté avec succès à l'étudiant \"Ahmed Chermiti\"",
                NotificationType.Success,
                "Marquer comme lu"
            );

            // Exemple de notification d'avertissement
            CreateNotification(
                newNotificationsPanel,
                "Il reste 4 jours pour que l'étudiant \"Aziz Mahfoudhi\" rend le livre \"Accountancy\".",
                NotificationType.Warning
            );

            // Exemples de notifications plus anciennes
            CreateNotification(
                olderNotificationsPanel,
                "Le livre \"Probability and Statistics\" a été retourné par l'étudiant \"Ahmed Zribi\".",
                NotificationType.Info
            );

            CreateNotification(
                olderNotificationsPanel,
                "L'étudiant \"Aziz Mahfoudhi\" a emprunté le livre \"Real English Authenticity\".",
                NotificationType.Info
            );
        }

        private enum NotificationType
        {
            Success,
            Warning,
            Danger,
            Info
        }

        private void CreateNotification(FlowLayoutPanel panel, string message, NotificationType type, string actionText = null)
        {
            Panel notificationPanel = new Panel
            {
                Size = new Size(panel.Width - 20, 80),
                BackColor = Color.White,
                Margin = new Padding(0, 0, 0, 10),
                Padding = new Padding(15),
                BorderStyle = BorderStyle.None
            };

            // Icône de notification
            PictureBox iconBox = new PictureBox
            {
                Size = new Size(24, 24),
                Location = new Point(15, 15),
                SizeMode = PictureBoxSizeMode.CenterImage
            };

            // Couleur et icône selon le type de notification
            Color textColor;
            switch (type)
            {
                case NotificationType.Success:
                    textColor = SuccessColor;
                    // Ici, vous pouvez ajouter une icône de succès
                    break;
                case NotificationType.Warning:
                    textColor = WarningColor;
                    // Ici, vous pouvez ajouter une icône d'avertissement
                    break;
                case NotificationType.Danger:
                    textColor = DangerColor;
                    // Ici, vous pouvez ajouter une icône de danger
                    break;
                default:
                    textColor = TextColor;
                    // Ici, vous pouvez ajouter une icône d'information
                    break;
            }

            // Message de notification
            Label lblMessage = new Label
            {
                Text = message,
                Font = new Font("Segoe UI", 10),
                ForeColor = TextColor,
                AutoSize = false,
                Size = new Size(notificationPanel.Width - 80, 50),
                Location = new Point(50, 15)
            };

            // Bouton d'action (si spécifié)
            if (!string.IsNullOrEmpty(actionText))
            {
                Button btnAction = new Button
                {
                    Text = actionText,
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.Transparent,
                    ForeColor = textColor,
                    Font = new Font("Segoe UI", 9),
                    Size = new Size(120, 30),
                    Location = new Point(notificationPanel.Width - 140, notificationPanel.Height - 40),
                    Cursor = Cursors.Hand
                };
                btnAction.FlatAppearance.BorderSize = 0;

                notificationPanel.Controls.Add(btnAction);
            }

            // Ajouter les contrôles au panneau de notification
            notificationPanel.Controls.Add(iconBox);
            notificationPanel.Controls.Add(lblMessage);

            // Ajouter le panneau de notification au panneau parent
            panel.Controls.Add(notificationPanel);
        }
    }
}
