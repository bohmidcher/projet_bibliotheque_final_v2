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
    public partial class StudentHomeForm : Form
    {
        private readonly Member currentUser;
        private readonly Color PrimaryColor = Color.FromArgb(8, 15, 40);  // Bleu foncé
        private readonly Color AccentColor = Color.FromArgb(45, 20, 80);  // Violet foncé
        
        public StudentHomeForm(Member user)
        {
            InitializeComponent();
            this.currentUser = user;
            this.BackColor = Color.WhiteSmoke;
            CreateHomeContent();
        }
        
        private void CreateHomeContent()
        {
            // Titre de la page d'accueil
            Label lblWelcome = new Label
            {
                Text = $"Bienvenue, {currentUser.Name}",
                Font = new Font("Poppins", 24, FontStyle.Bold),
                ForeColor = PrimaryColor,
                Location = new Point(20, 20),
                AutoSize = true
            };
            
            // Date actuelle
            Label lblDate = new Label
            {
                Text = DateTime.Now.ToString("dddd, dd MMMM yyyy"),
                Font = new Font("Poppins", 12, FontStyle.Regular),
                ForeColor = Color.Gray,
                Location = new Point(20, lblWelcome.Bottom + 5),
                AutoSize = true
            };
            
            // Ajouter les contrôles au formulaire
            this.Controls.Add(lblWelcome);
            this.Controls.Add(lblDate);
            
            // Créer la section des livres recommandés
            CreateRecommendedBooksSection(lblDate.Bottom + 30);
            
            // Créer la section des livres populaires
            CreatePopularBooksSection(lblDate.Bottom + 350);
        }
        
        private void CreateRecommendedBooksSection(int startY)
        {
            // Titre de la section
            Label lblRecommended = new Label
            {
                Text = "Livres recommandés pour vous",
                Font = new Font("Poppins", 18, FontStyle.Bold),
                ForeColor = PrimaryColor,
                Location = new Point(20, startY),
                AutoSize = true
            };
            
            this.Controls.Add(lblRecommended);
            
            // Créer un panel horizontal pour les livres recommandés
            Panel booksPanel = new Panel
            {
                Location = new Point(20, lblRecommended.Bottom + 10),
                Size = new Size(this.Width - 40, 250),
                BackColor = Color.Transparent,
                AutoScroll = true
            };
            
            this.Controls.Add(booksPanel);
            
            // Ajouter des livres recommandés (simulés)
            List<(string Title, string Author, string ImagePath)> recommendedBooks = new List<(string, string, string)>
            {
                ("L'Art de la Finance", "Jean Dupont", "finance_book.jpg"),
                ("Marketing Digital", "Marie Laurent", "marketing_book.jpg"),
                ("Intelligence Artificielle", "Pierre Martin", "ai_book.jpg"),
                ("Économie Moderne", "Sophie Dubois", "economy_book.jpg"),
                ("Gestion de Projet", "Thomas Leroy", "project_book.jpg")
            };
            
            int bookX = 0;
            int bookWidth = 150;
            int bookSpacing = 20;
            
            foreach (var book in recommendedBooks)
            {
                Panel bookCard = CreateBookCard(book.Title, book.Author, book.ImagePath, bookWidth);
                bookCard.Location = new Point(bookX, 0);
                booksPanel.Controls.Add(bookCard);
                
                bookX += bookWidth + bookSpacing;
            }
        }
        
        private void CreatePopularBooksSection(int startY)
        {
            // Titre de la section
            Label lblPopular = new Label
            {
                Text = "Livres populaires",
                Font = new Font("Poppins", 18, FontStyle.Bold),
                ForeColor = PrimaryColor,
                Location = new Point(20, startY),
                AutoSize = true
            };
            
            this.Controls.Add(lblPopular);
            
            // Créer un panel horizontal pour les livres populaires
            Panel booksPanel = new Panel
            {
                Location = new Point(20, lblPopular.Bottom + 10),
                Size = new Size(this.Width - 40, 250),
                BackColor = Color.Transparent,
                AutoScroll = true
            };
            
            this.Controls.Add(booksPanel);
            
            // Ajouter des livres populaires (simulés)
            List<(string Title, string Author, string ImagePath)> popularBooks = new List<(string, string, string)>
            {
                ("Principes de Comptabilité", "Robert Johnson", "accounting_book.jpg"),
                ("Big Data Analytics", "David Smith", "data_book.jpg"),
                ("Leadership en Entreprise", "Emma Wilson", "leadership_book.jpg"),
                ("Stratégies de Négociation", "Michael Brown", "negotiation_book.jpg"),
                ("Droit des Affaires", "Laura Davis", "law_book.jpg")
            };
            
            int bookX = 0;
            int bookWidth = 150;
            int bookSpacing = 20;
            
            foreach (var book in popularBooks)
            {
                Panel bookCard = CreateBookCard(book.Title, book.Author, book.ImagePath, bookWidth);
                bookCard.Location = new Point(bookX, 0);
                booksPanel.Controls.Add(bookCard);
                
                bookX += bookWidth + bookSpacing;
            }
        }
        
        private Panel CreateBookCard(string title, string author, string imagePath, int width)
        {
            Panel card = new Panel
            {
                Width = width,
                Height = 220,
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
                Size = new Size(width - 20, 120),
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
                Font = new Font("Poppins", 10, FontStyle.Bold),
                ForeColor = Color.Black,
                Location = new Point(10, bookImage.Bottom + 5),
                Size = new Size(width - 20, 40),
                TextAlign = ContentAlignment.TopLeft
            };
            
            // Auteur du livre
            Label lblAuthor = new Label
            {
                Text = author,
                Font = new Font("Poppins", 8, FontStyle.Regular),
                ForeColor = Color.Gray,
                Location = new Point(10, lblTitle.Bottom),
                Size = new Size(width - 20, 20),
                TextAlign = ContentAlignment.TopLeft
            };
            
            // Ajouter les contrôles à la carte
            card.Controls.Add(bookImage);
            card.Controls.Add(lblTitle);
            card.Controls.Add(lblAuthor);
            
            // Ajouter un événement de clic pour voir les détails du livre
            card.Click += (s, e) => ShowBookDetails(title, author);
            card.Cursor = Cursors.Hand;
            
            return card;
        }
        
        private void ShowBookDetails(string title, string author)
        {
            MessageBox.Show($"Détails du livre: {title} par {author}", "Détails du livre", MessageBoxButtons.OK, MessageBoxIcon.Information);
            // Ici, vous pourriez ouvrir un formulaire de détails du livre
        }
    }
}
