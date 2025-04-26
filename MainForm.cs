using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.IO;
using projet_bibliotheque.Controls;
using projet_bibliotheque.Models;
using projet_bibliotheque.Views;
using System.Linq;
using System.Collections.Generic;
using projet_bibliotheque.Data;
using Microsoft.EntityFrameworkCore;
using projet_bibliotheque.Forms;

namespace projet_bibliotheque
{
    public partial class MainForm : Form
    {
        private Member currentUser;
        private Panel sidebarPanel;
        private Panel contentPanel;
        private Button currentActiveButton;
        
        // Couleurs de l'application
        private readonly Color PrimaryColor = Color.FromArgb(8, 15, 40);  // Bleu foncé
        private readonly Color AccentColor = Color.FromArgb(45, 20, 80);  // Violet foncé
        private readonly Color TextColor = Color.White;
        private readonly Color SidebarColor = Color.FromArgb(20, 30, 70);
        
        public MainForm(Member user = null)
        {
            InitializeComponent();
            this.currentUser = user;
            SetupForm();
            CreateDashboard();
        }
        
        private void SetupForm()
        {
            this.Text = "BiblioHub - Tableau de bord";
            this.MinimumSize = new Size(1200, 800);
            this.Size = new Size(1200, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.DoubleBuffered = true;
        }
        
        private void CreateDashboard()
        {
            // Créer le layout principal
            TableLayoutPanel mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Color.White
            };
            
            // Configurer les colonnes (sidebar et contenu principal)
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200F));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            
            // Créer la sidebar
            sidebarPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = SidebarColor
            };
            
            // Créer le contenu principal
            contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.WhiteSmoke,
                Padding = new Padding(20)
            };
            
            // Ajouter les panels au layout
            mainLayout.Controls.Add(sidebarPanel, 0, 0);
            mainLayout.Controls.Add(contentPanel, 1, 0);
            
            // Ajouter le layout au formulaire
            this.Controls.Add(mainLayout);
            
            // Créer les éléments de la sidebar
            CreateSidebar();
            
            // Créer le contenu du tableau de bord
            CreateDashboardContent();
        }
        
        private void CreateSidebar()
        {
            // Logo en haut de la sidebar
            PictureBox logoBox = new PictureBox
            {
                Size = new Size(120, 120),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent,
                Location = new Point((sidebarPanel.Width - 120) / 2, 20)
            };
            
            try
            {
                string logoPath = Path.Combine(Application.StartupPath, "Assets", "img", "logo.svg");
                if (File.Exists(logoPath))
                {
                    logoBox.Image = Image.FromFile(logoPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement du logo : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            // Informations de l'utilisateur
            Panel userInfoPanel = new Panel
            {
                Width = sidebarPanel.Width,
                Height = 60,
                BackColor = Color.FromArgb(30, 40, 80),
                Location = new Point(0, 150)
            };
            
            Label lblUserName = new Label
            {
                Text = currentUser != null ? currentUser.Name : "Ahmed Chermiti",
                ForeColor = Color.White,
                Font = new Font("Poppins", 12, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleLeft,
                Location = new Point(60, 10),
                Size = new Size(140, 25)
            };
            
            Label lblUserEmail = new Label
            {
                Text = currentUser != null ? currentUser.Email : "ahmed.chermiti@ihec.tn",
                ForeColor = Color.Silver,
                Font = new Font("Poppins", 8, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleLeft,
                Location = new Point(60, 35),
                Size = new Size(140, 15)
            };
            
            // Avatar de l'utilisateur
            PictureBox avatarBox = new PictureBox
            {
                Size = new Size(40, 40),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent,
                Location = new Point(10, 10),
                BorderStyle = BorderStyle.None
            };
            
            // Créer un cercle pour l'avatar
            avatarBox.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (SolidBrush brush = new SolidBrush(Color.Gray))
                {
                    e.Graphics.FillEllipse(brush, 0, 0, avatarBox.Width, avatarBox.Height);
                }
            };
            
            // Menu items
            string[] menuItems = new string[]
            {
                "Tableau de bord",
                "Gérer les étudiants",
                "Gérer la bibliothèque",
                "Emprunts",
                "Historique",
                "Notifications",
                "Paramètres",
                "Assistant Virtuel"
            };
            
            int menuY = 230;
            
            foreach (string item in menuItems)
            {
                Button btnMenuItem = new Button
                {
                    Text = "  " + item,
                    TextAlign = ContentAlignment.MiddleLeft,
                    FlatStyle = FlatStyle.Flat,
                    BackColor = item == "Tableau de bord" ? Color.FromArgb(40, 50, 90) : Color.Transparent,
                    ForeColor = Color.White,
                    Font = new Font("Poppins", 10, FontStyle.Regular),
                    Size = new Size(200, 40),
                    Location = new Point(0, menuY),
                    Cursor = Cursors.Hand,
                    Tag = item // Stocker le nom du menu dans la propriété Tag
                };
                
                btnMenuItem.FlatAppearance.BorderSize = 0;
                btnMenuItem.FlatAppearance.MouseOverBackColor = Color.FromArgb(40, 50, 90);
                
                // Ajouter un gestionnaire d'événements pour le clic sur le bouton
                btnMenuItem.Click += MenuButton_Click;
                
                // Ajouter un icône selon le menu item
                try 
                {
                    string iconName = "";
                    switch (item)
                    {
                        case "Tableau de bord": iconName = "dashboard.svg"; break;
                        case "Gérer la bibliothèque": iconName = "book.svg"; break;
                        case "Gérer les étudiants": iconName = "profil.svg"; break;
                        case "Notifications": iconName = "notifications.svg"; break;
                        case "Historique": iconName = "history.svg"; break;
                        case "Paramètres": iconName = "settings.svg"; break;
                        case "Assistant Virtuel": iconName = "chatbot.svg"; break;
                    }
                    
                    string iconPath = Path.Combine(Application.StartupPath, "Assets", "icons", iconName);
                    if (File.Exists(iconPath))
                    {
                        PictureBox iconBox = new PictureBox
                        {
                            Size = new Size(20, 20),
                            SizeMode = PictureBoxSizeMode.Zoom,
                            BackColor = Color.Transparent,
                            Location = new Point(10, 10),
                            Image = Image.FromFile(iconPath)
                        };
                        btnMenuItem.Controls.Add(iconBox);
                    }
                }
                catch (Exception) { /* Ignorer les erreurs d'icônes */ }
                
                sidebarPanel.Controls.Add(btnMenuItem);
                menuY += 40;
            }
            
            // Section des mots-clés favoris
            Label lblFavorites = new Label
            {
                Text = "Mots-clés favoris",
                ForeColor = Color.Silver,
                Font = new Font("Poppins", 10, FontStyle.Regular),
                Location = new Point(10, menuY + 20),
                Size = new Size(180, 20)
            };
            
            // Ajouter quelques mots-clés favoris
            string[] favorites = { "Économie", "Finances", "Informatiques" };
            int favY = menuY + 50;
            Color[] dotColors = { Color.Blue, Color.Red, Color.Green };
            
            for (int i = 0; i < favorites.Length; i++)
            {
                Panel dotPanel = new Panel
                {
                    Size = new Size(8, 8),
                    Location = new Point(15, favY + 6),
                    BackColor = dotColors[i]
                };
                
                dotPanel.Paint += (s, e) =>
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    using (SolidBrush brush = new SolidBrush(((Panel)s).BackColor))
                    {
                        e.Graphics.FillEllipse(brush, 0, 0, 8, 8);
                    }
                };
                
                Label lblFavorite = new Label
                {
                    Text = favorites[i],
                    ForeColor = Color.White,
                    Font = new Font("Poppins", 9, FontStyle.Regular),
                    Location = new Point(30, favY),
                    Size = new Size(150, 20)
                };
                
                sidebarPanel.Controls.Add(dotPanel);
                sidebarPanel.Controls.Add(lblFavorite);
                favY += 25;
            }
            
            // Ajouter les contrôles à la sidebar
            userInfoPanel.Controls.Add(lblUserName);
            userInfoPanel.Controls.Add(lblUserEmail);
            userInfoPanel.Controls.Add(avatarBox);
            
            sidebarPanel.Controls.Add(logoBox);
            sidebarPanel.Controls.Add(userInfoPanel);
            sidebarPanel.Controls.Add(lblFavorites);
        }
        
        private void CreateDashboardContent()
        {
            // Charger le formulaire de tableau de bord
            LoadFormIntoContentPanel(new Forms.DashboardForm());
        }
        
        private void MenuButton_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton != null)
            {
                // Mettre à jour l'apparence du bouton actif
                if (currentActiveButton != null)
                {
                    currentActiveButton.BackColor = Color.Transparent;
                }
                
                clickedButton.BackColor = Color.FromArgb(40, 50, 90);
                currentActiveButton = clickedButton;
                
                // Charger le formulaire correspondant selon le bouton cliqué
                string menuItem = clickedButton.Tag.ToString();
                
                switch (menuItem)
                {
                    case "Tableau de bord":
                        LoadFormIntoContentPanel(new Forms.DashboardForm());
                        break;
                    case "Gérer les étudiants":
                        {
                            var optionsBuilder = new DbContextOptionsBuilder<LibraryContext>();
                            optionsBuilder.UseSqlServer(@"Server=localhost;Database=BibliothequeIHEC;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true");
                            LoadFormIntoContentPanel(new Forms.StudentManagementForm(new LibraryContext(optionsBuilder.Options)));
                        }
                        break;
                    case "Gérer la bibliothèque":
                        {
                            var optionsBuilder = new DbContextOptionsBuilder<LibraryContext>();
                            optionsBuilder.UseSqlServer(@"Server=localhost;Database=BibliothequeIHEC;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true");
                            LoadFormIntoContentPanel(new Forms.LibraryManagementForm(new LibraryContext(optionsBuilder.Options)));
                        }
                        break;
                    case "Emprunts":
                        LoadFormIntoContentPanel(new Forms.LoanManagementForm());
                        break;
                    case "Historique":
                        LoadFormIntoContentPanel(new Forms.HistoryForm());
                        break;
                    case "Notifications":
                        LoadFormIntoContentPanel(new Forms.NotificationsForm());
                        break;
                    case "Paramètres":
                        LoadFormIntoContentPanel(new Forms.SettingsForm());
                        break;
                    case "Assistant Virtuel":
                        LoadFormIntoContentPanel(new Forms.ChatbotForm(currentUser));
                        break;
                    default:
                        break;
                }
            }
        }
        
        private void LoadFormIntoContentPanel(Form formToLoad)
        {
            // Vider le panel de contenu
            contentPanel.Controls.Clear();
            
            // Configurer le formulaire à charger
            formToLoad.TopLevel = false;
            formToLoad.FormBorderStyle = FormBorderStyle.None;
            formToLoad.Dock = DockStyle.Fill;
            
            // Ajouter le formulaire au panel de contenu
            contentPanel.Controls.Add(formToLoad);
            formToLoad.Show();
        }
        
        private void CreateStatCards()
        {
            // Statistiques à afficher
            string[] statTitles = { "Livres disponibles", "Emprunts actifs", "Étudiants inscrits", "Livres ajoutés ce mois" };
            string[] statValues = { "1,245", "87", "356", "32" };
            Color[] statColors = { 
                Color.FromArgb(52, 152, 219), // Bleu
                Color.FromArgb(231, 76, 60),  // Rouge
                Color.FromArgb(46, 204, 113), // Vert
                Color.FromArgb(155, 89, 182)  // Violet
            };
            
            int cardWidth = 220;
            int cardHeight = 100;
            int cardSpacing = 20;
            int startY = 100;
            
            for (int i = 0; i < statTitles.Length; i++)
            {
                Panel cardPanel = new Panel
                {
                    Size = new Size(cardWidth, cardHeight),
                    Location = new Point(20 + (cardWidth + cardSpacing) * i, startY),
                    BackColor = Color.White
                };
                
                // Ajouter une ombre
                cardPanel.Paint += (s, e) =>
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    using (var path = new GraphicsPath())
                    {
                        path.AddRectangle(new Rectangle(0, 0, cardPanel.Width, cardPanel.Height));
                        using (var pen = new Pen(Color.FromArgb(20, 0, 0, 0), 1))
                        {
                            e.Graphics.DrawPath(pen, path);
                        }
                    }
                };
                
                // Indicateur de couleur
                Panel colorIndicator = new Panel
                {
                    Size = new Size(5, cardHeight),
                    Location = new Point(0, 0),
                    BackColor = statColors[i % statColors.Length]
                };
                
                // Titre de la statistique
                Label lblTitle = new Label
                {
                    Text = statTitles[i],
                    Font = new Font("Poppins", 12, FontStyle.Regular),
                    ForeColor = Color.Gray,
                    Location = new Point(15, 15),
                    AutoSize = true
                };
                
                // Valeur de la statistique
                Label lblValue = new Label
                {
                    Text = statValues[i],
                    Font = new Font("Poppins", 24, FontStyle.Bold),
                    ForeColor = PrimaryColor,
                    Location = new Point(15, 45),
                    AutoSize = true
                };
                
                cardPanel.Controls.Add(colorIndicator);
                cardPanel.Controls.Add(lblTitle);
                cardPanel.Controls.Add(lblValue);
                
                contentPanel.Controls.Add(cardPanel);
            }
        }
        
        private void CreateRecentBooksSection()
        {
            // Titre de la section
            Label lblRecentBooks = new Label
            {
                Text = "Livres récemment ajoutés",
                Font = new Font("Poppins", 18, FontStyle.Bold),
                ForeColor = PrimaryColor,
                Location = new Point(20, 220),
                AutoSize = true
            };
            
            contentPanel.Controls.Add(lblRecentBooks);
            
            // Créer un panel pour la liste des livres
            Panel booksPanel = new Panel
            {
                Location = new Point(20, lblRecentBooks.Bottom + 10),
                Size = new Size(contentPanel.Width - 40, 300),
                BackColor = Color.White,
                AutoScroll = true
            };
            
            contentPanel.Controls.Add(booksPanel);
            
            // Exemple de livres récents
            string[] bookTitles = {
                "L'Art de la Guerre", 
                "Principes d'Économie", 
                "Introduction au Marketing Digital",
                "Finance d'Entreprise",
                "Gestion des Ressources Humaines"
            };
            
            string[] bookAuthors = {
                "Sun Tzu", 
                "Gregory Mankiw", 
                "Philip Kotler",
                "Pierre Vernimmen",
                "Jean-Marie Peretti"
            };
            
            string[] bookDates = {
                "12/05/2023", 
                "28/04/2023", 
                "15/04/2023",
                "02/04/2023",
                "25/03/2023"
            };
            
            int bookItemHeight = 60;
            
            for (int i = 0; i < bookTitles.Length; i++)
            {
                Panel bookItem = new Panel
                {
                    Size = new Size(booksPanel.Width - 20, bookItemHeight),
                    Location = new Point(0, i * bookItemHeight),
                    BackColor = i % 2 == 0 ? Color.White : Color.FromArgb(248, 249, 250)
                };
                
                Label lblBookTitle = new Label
                {
                    Text = bookTitles[i],
                    Font = new Font("Poppins", 12, FontStyle.Bold),
                    ForeColor = PrimaryColor,
                    Location = new Point(15, 10),
                    AutoSize = true
                };
                
                Label lblBookAuthor = new Label
                {
                    Text = "Par " + bookAuthors[i],
                    Font = new Font("Poppins", 10, FontStyle.Regular),
                    ForeColor = Color.Gray,
                    Location = new Point(15, 35),
                    AutoSize = true
                };
                
                Label lblBookDate = new Label
                {
                    Text = "Ajouté le " + bookDates[i],
                    Font = new Font("Poppins", 9, FontStyle.Regular),
                    ForeColor = Color.DarkGray,
                    Location = new Point(bookItem.Width - 150, 20),
                    AutoSize = true
                };
                
                bookItem.Controls.Add(lblBookTitle);
                bookItem.Controls.Add(lblBookAuthor);
                bookItem.Controls.Add(lblBookDate);
                
                booksPanel.Controls.Add(bookItem);
            }
        }
    }
}
