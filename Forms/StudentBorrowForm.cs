using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using projet_bibliotheque.Controls;
using projet_bibliotheque.Models;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Linq;
using projet_bibliotheque.Data;
using Microsoft.EntityFrameworkCore;

namespace projet_bibliotheque.Forms
{
    public partial class StudentBorrowForm : Form
    {
        private readonly Member currentUser;
        private readonly Color PrimaryColor = Color.FromArgb(8, 15, 40);  // Bleu foncé
        private readonly Color AccentColor = Color.FromArgb(45, 20, 80);  // Violet foncé
        
        public StudentBorrowForm(Member user)
        {
            InitializeComponent();
            this.currentUser = user;
            this.BackColor = Color.WhiteSmoke;
            CreateBorrowContent();
        }
        
        private void CreateBorrowContent()
        {
            // Titre de la page
            Label lblTitle = new Label
            {
                Text = "Emprunter un livre",
                Font = new Font("Poppins", 24, FontStyle.Bold),
                ForeColor = PrimaryColor,
                Location = new Point(20, 20),
                AutoSize = true
            };
            
            // Sous-titre
            Label lblSubtitle = new Label
            {
                Text = "Recherchez et empruntez des livres de la bibliothèque",
                Font = new Font("Poppins", 12, FontStyle.Regular),
                ForeColor = Color.Gray,
                Location = new Point(20, lblTitle.Bottom + 5),
                AutoSize = true
            };
            
            // Ajouter les contrôles au formulaire
            this.Controls.Add(lblTitle);
            this.Controls.Add(lblSubtitle);
            
            // Créer la barre de recherche
            CreateSearchBar(lblSubtitle.Bottom + 30);
            
            // Créer les filtres
            CreateFilters(lblSubtitle.Bottom + 80);
            
            // Créer la section des livres disponibles
            CreateAvailableBooksSection(lblSubtitle.Bottom + 130);
        }
        
        private void CreateSearchBar(int startY)
        {
            // Panel pour la barre de recherche
            Panel searchPanel = new Panel
            {
                Location = new Point(20, startY),
                Size = new Size(this.Width - 40, 40),
                BackColor = Color.White
            };
            
            // Champ de recherche
            TextBox txtSearch = new TextBox
            {
                Location = new Point(10, 8),
                Size = new Size(searchPanel.Width - 120, 24),
                Font = new Font("Poppins", 12),
                BorderStyle = BorderStyle.None,
                PlaceholderText = "Rechercher un livre par titre, auteur ou mot-clé..."
            };
            
            // Bouton de recherche
            Button btnSearch = new Button
            {
                Text = "Rechercher",
                Location = new Point(searchPanel.Width - 110, 5),
                Size = new Size(100, 30),
                FlatStyle = FlatStyle.Flat,
                BackColor = PrimaryColor,
                ForeColor = Color.White,
                Font = new Font("Poppins", 10)
            };
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.Click += (s, e) => SearchBooks(txtSearch.Text);
            
            // Ajouter les contrôles au panel
            searchPanel.Controls.Add(txtSearch);
            searchPanel.Controls.Add(btnSearch);
            
            // Ajouter le panel au formulaire
            this.Controls.Add(searchPanel);
        }
        
        private void CreateFilters(int startY)
        {
            // Panel pour les filtres
            Panel filterPanel = new Panel
            {
                Location = new Point(20, startY),
                Size = new Size(this.Width - 40, 40),
                BackColor = Color.Transparent
            };
            
            // Étiquette pour les filtres
            Label lblFilter = new Label
            {
                Text = "Filtrer par:",
                Location = new Point(0, 10),
                Size = new Size(80, 20),
                Font = new Font("Poppins", 10),
                ForeColor = Color.Black
            };
            
            // Boutons de filtre
            string[] filters = { "Tous", "Économie", "Marketing", "Finance", "Informatique", "Management" };
            int buttonX = 80;
            
            foreach (string filter in filters)
            {
                Button btnFilter = new Button
                {
                    Text = filter,
                    Location = new Point(buttonX, 5),
                    Size = new Size(100, 30),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = filter == "Tous" ? PrimaryColor : Color.White,
                    ForeColor = filter == "Tous" ? Color.White : Color.Black,
                    Font = new Font("Poppins", 9),
                    Tag = filter
                };
                btnFilter.FlatAppearance.BorderColor = Color.LightGray;
                btnFilter.FlatAppearance.BorderSize = 1;
                btnFilter.Click += (s, e) => FilterBooks(filter);
                
                filterPanel.Controls.Add(btnFilter);
                buttonX += 110;
            }
            
            // Ajouter le panel au formulaire
            this.Controls.Add(filterPanel);
        }
        
        private void CreateAvailableBooksSection(int startY)
        {
            // Créer un panel pour les livres disponibles
            Panel booksPanel = new Panel
            {
                Location = new Point(20, startY),
                Size = new Size(this.Width - 40, this.Height - startY - 40),
                BackColor = Color.Transparent,
                AutoScroll = true
            };
            
            this.Controls.Add(booksPanel);
            
            // Ajouter des livres disponibles (simulés)
            List<(string Title, string Author, string Category, string Description, string ImagePath)> availableBooks = new List<(string, string, string, string, string)>
            {
                ("Principes de Comptabilité", "Robert Johnson", "Finance", "Un guide complet sur les principes fondamentaux de la comptabilité.", "accounting_book.jpg"),
                ("Big Data Analytics", "David Smith", "Informatique", "Exploration des techniques d'analyse de données massives.", "data_book.jpg"),
                ("Leadership en Entreprise", "Emma Wilson", "Management", "Stratégies de leadership pour les managers modernes.", "leadership_book.jpg"),
                ("Stratégies de Négociation", "Michael Brown", "Management", "Techniques avancées pour des négociations réussies.", "negotiation_book.jpg"),
                ("Droit des Affaires", "Laura Davis", "Économie", "Introduction au cadre juridique des entreprises.", "law_book.jpg"),
                ("Marketing Digital", "Marie Laurent", "Marketing", "Les dernières tendances en marketing numérique.", "marketing_book.jpg"),
                ("Intelligence Artificielle", "Pierre Martin", "Informatique", "Applications de l'IA dans le monde des affaires.", "ai_book.jpg")
            };
            
            int bookY = 0;
            int bookHeight = 180;
            int bookSpacing = 20;
            
            foreach (var book in availableBooks)
            {
                Panel bookCard = CreateAvailableBookCard(book.Title, book.Author, book.Category, book.Description, book.ImagePath, booksPanel.Width - 40);
                bookCard.Location = new Point(0, bookY);
                booksPanel.Controls.Add(bookCard);
                
                bookY += bookHeight + bookSpacing;
            }
        }
        
        private Panel CreateAvailableBookCard(string title, string author, string category, string description, string imagePath, int width)
        {
            Panel card = new Panel
            {
                Width = width,
                Height = 180,
                BackColor = Color.White
            };
            
            // Ajouter une ombre
            card.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var path = new GraphicsPath())
                {
                    path.AddRectangle(new Rectangle(0, 0, card.Width, card.Height));
                    using (var pen = new Pen(Color.FromArgb(20, 0, 0, 0), 1))
                    {
                        e.Graphics.DrawPath(pen, path);
                    }
                }
            };
            
            // Image du livre
            PictureBox bookImage = new PictureBox
            {
                Size = new Size(120, 160),
                Location = new Point(10, 10),
                BackColor = Color.LightGray,
                SizeMode = PictureBoxSizeMode.Zoom
            };
            
            // Essayer de charger l'image
            try
            {
                string imagePath2 = Path.Combine(Application.StartupPath, "Assets", "books", imagePath);
                if (File.Exists(imagePath2))
                {
                    bookImage.Image = Image.FromFile(imagePath2);
                }
            }
            catch { /* Ignorer les erreurs d'image */ }
            
            // Titre du livre
            Label lblTitle = new Label
            {
                Text = title,
                Font = new Font("Poppins", 14, FontStyle.Bold),
                ForeColor = Color.Black,
                Location = new Point(140, 10),
                Size = new Size(width - 350, 30),
                TextAlign = ContentAlignment.MiddleLeft
            };
            
            // Auteur du livre
            Label lblAuthor = new Label
            {
                Text = "Par " + author,
                Font = new Font("Poppins", 10, FontStyle.Regular),
                ForeColor = Color.Gray,
                Location = new Point(140, lblTitle.Bottom),
                Size = new Size(width - 350, 20),
                TextAlign = ContentAlignment.MiddleLeft
            };
            
            // Catégorie
            Label lblCategory = new Label
            {
                Text = category,
                Font = new Font("Poppins", 9, FontStyle.Regular),
                ForeColor = Color.White,
                BackColor = PrimaryColor,
                Location = new Point(140, lblAuthor.Bottom + 5),
                Size = new Size(100, 20),
                TextAlign = ContentAlignment.MiddleCenter
            };
            
            // Description
            Label lblDescription = new Label
            {
                Text = description,
                Font = new Font("Poppins", 9, FontStyle.Regular),
                ForeColor = Color.Black,
                Location = new Point(140, lblCategory.Bottom + 5),
                Size = new Size(width - 350, 60),
                TextAlign = ContentAlignment.TopLeft
            };
            
            // Bouton d'emprunt
            Button btnBorrow = new Button
            {
                Text = "Emprunter",
                Font = new Font("Poppins", 12, FontStyle.Regular),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(0, 120, 0),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(150, 40),
                Location = new Point(width - 170, 70),
                Cursor = Cursors.Hand
            };
            btnBorrow.FlatAppearance.BorderSize = 0;
            btnBorrow.Click += (s, e) => BorrowBook(title);
            
            // Bouton de détails
            Button btnDetails = new Button
            {
                Text = "Détails",
                Font = new Font("Poppins", 10, FontStyle.Regular),
                ForeColor = Color.Black,
                BackColor = Color.LightGray,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(150, 30),
                Location = new Point(width - 170, 120),
                Cursor = Cursors.Hand
            };
            btnDetails.FlatAppearance.BorderSize = 0;
            btnDetails.Click += (s, e) => ShowBookDetails(title, author, category, description);
            
            // Ajouter les contrôles à la carte
            card.Controls.Add(bookImage);
            card.Controls.Add(lblTitle);
            card.Controls.Add(lblAuthor);
            card.Controls.Add(lblCategory);
            card.Controls.Add(lblDescription);
            card.Controls.Add(btnBorrow);
            card.Controls.Add(btnDetails);
            
            return card;
        }
        
        private void SearchBooks(string searchText)
        {
            MessageBox.Show($"Recherche de livres contenant: {searchText}", "Recherche", MessageBoxButtons.OK, MessageBoxIcon.Information);
            // Ici, vous filtreriez les livres en fonction du texte de recherche
        }
        
        private void FilterBooks(string category)
        {
            MessageBox.Show($"Filtrage des livres par catégorie: {category}", "Filtre", MessageBoxButtons.OK, MessageBoxIcon.Information);
            // Ici, vous filtreriez les livres par catégorie
        }
        
        private void BorrowBook(string bookTitle)
        {
            DialogResult result = MessageBox.Show($"Voulez-vous emprunter '{bookTitle}' pour 14 jours ?", "Emprunter", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                MessageBox.Show($"Vous avez emprunté '{bookTitle}' avec succès. Veuillez le retourner avant 14 jours.", "Emprunt réussi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Ici, vous mettriez à jour la base de données
            }
        }
        
        private void ShowBookDetails(string title, string author, string category, string description)
        {
            MessageBox.Show($"Titre: {title}\nAuteur: {author}\nCatégorie: {category}\nDescription: {description}", "Détails du livre", MessageBoxButtons.OK, MessageBoxIcon.Information);
            // Ici, vous pourriez ouvrir un formulaire de détails du livre
        }
    }
}
