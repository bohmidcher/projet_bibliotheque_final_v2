using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using projet_bibliotheque.Controls;
using projet_bibliotheque.Models;
using System.Drawing.Drawing2D;

namespace projet_bibliotheque.Forms
{
    public partial class StudentForm : Form
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
        
        public StudentForm(Member user)
        {
            InitializeComponent();
            this.currentUser = user;
            SetupForm();
            CreateDashboard();
        }
        
        private void SetupForm()
        {
            this.Text = "BiblioHub - Espace Étudiant";
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
            
            // Créer le contenu de la page d'accueil
            LoadFormIntoContentPanel(new StudentHomeForm(currentUser));
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
                Text = currentUser != null ? currentUser.Name : "Étudiant",
                ForeColor = Color.White,
                Font = new Font("Poppins", 12, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleLeft,
                Location = new Point(60, 10),
                Size = new Size(140, 25)
            };
            
            Label lblUserEmail = new Label
            {
                Text = currentUser != null ? currentUser.Email : "etudiant@ihec.tn",
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
                "Accueil",
                "Ma Bibliothèque",
                "Emprunter un livre",
                "Notifications",
                "Historique",
                "Paramètres",
                "Profil",
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
                    BackColor = item == "Accueil" ? Color.FromArgb(40, 50, 90) : Color.Transparent,
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
                        case "Accueil": iconName = "home.svg"; break;
                        case "Ma Bibliothèque": iconName = "book.svg"; break;
                        case "Emprunter un livre": iconName = "borrow.svg"; break;
                        case "Notifications": iconName = "notifications.svg"; break;
                        case "Historique": iconName = "history.svg"; break;
                        case "Paramètres": iconName = "settings.svg"; break;
                        case "Profil": iconName = "profil.svg"; break;
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
                    case "Accueil":
                        LoadFormIntoContentPanel(new StudentHomeForm(currentUser));
                        break;
                    case "Ma Bibliothèque":
                        LoadFormIntoContentPanel(new StudentLibraryForm(currentUser));
                        break;
                    case "Emprunter un livre":
                        LoadFormIntoContentPanel(new StudentBorrowForm(currentUser));
                        break;
                    case "Notifications":
                        LoadFormIntoContentPanel(new StudentNotificationsForm(currentUser));
                        break;
                    case "Historique":
                        LoadFormIntoContentPanel(new StudentHistoryForm(currentUser));
                        break;
                    case "Paramètres":
                        LoadFormIntoContentPanel(new StudentSettingsForm(currentUser));
                        break;
                    case "Profil":
                        LoadFormIntoContentPanel(new StudentProfileForm(currentUser));
                        break;
                    case "Assistant Virtuel":
                        LoadFormIntoContentPanel(new ChatbotForm(currentUser));
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
    }
}
