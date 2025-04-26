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
    public partial class StudentLibraryForm : Form
    {
        private readonly Member currentUser;
        private readonly Color PrimaryColor = Color.FromArgb(8, 15, 40);  // Bleu foncé
        private readonly Color AccentColor = Color.FromArgb(45, 20, 80);  // Violet foncé
        
        public StudentLibraryForm(Member user)
        {
            InitializeComponent();
            this.currentUser = user;
            this.BackColor = Color.WhiteSmoke;
            CreateLibraryContent();
        }
        
        private void CreateLibraryContent()
        {
            // Titre de la page
            Label lblTitle = new Label
            {
                Text = "Ma Bibliothèque",
                Font = new Font("Poppins", 24, FontStyle.Bold),
                ForeColor = PrimaryColor,
                Location = new Point(20, 20),
                AutoSize = true
            };
            
            // Sous-titre
            Label lblSubtitle = new Label
            {
                Text = "Vos livres empruntés actuellement",
                Font = new Font("Poppins", 12, FontStyle.Regular),
                ForeColor = Color.Gray,
                Location = new Point(20, lblTitle.Bottom + 5),
                AutoSize = true
            };
            
            // Ajouter les contrôles au formulaire
            this.Controls.Add(lblTitle);
            this.Controls.Add(lblSubtitle);
            
            // Créer la section des livres empruntés
            CreateBorrowedBooksSection(lblSubtitle.Bottom + 30);
        }
        
        private void CreateBorrowedBooksSection(int startY)
        {
            // Créer un panel pour les livres empruntés
            Panel booksPanel = new Panel
            {
                Location = new Point(20, startY),
                Size = new Size(this.Width - 40, this.Height - startY - 40),
                BackColor = Color.Transparent,
                AutoScroll = true
            };
            
            this.Controls.Add(booksPanel);
            
            // Ajouter des livres empruntés (simulés)
            List<(string Title, string Author, string BorrowDate, string ReturnDate, string ImagePath)> borrowedBooks = new List<(string, string, string, string, string)>
            {
                ("L'Art de la Finance", "Jean Dupont", "15/04/2025", "29/04/2025", "finance_book.jpg"),
                ("Marketing Digital", "Marie Laurent", "10/04/2025", "24/04/2025", "marketing_book.jpg"),
                ("Intelligence Artificielle", "Pierre Martin", "20/04/2025", "04/05/2025", "ai_book.jpg")
            };
            
            int bookY = 0;
            int bookHeight = 150;
            int bookSpacing = 20;
            
            foreach (var book in borrowedBooks)
            {
                Panel bookCard = CreateBorrowedBookCard(book.Title, book.Author, book.BorrowDate, book.ReturnDate, book.ImagePath, booksPanel.Width - 40);
                bookCard.Location = new Point(0, bookY);
                booksPanel.Controls.Add(bookCard);
                
                bookY += bookHeight + bookSpacing;
            }
            
            // Message si aucun livre emprunté
            if (borrowedBooks.Count == 0)
            {
                Label lblNoBooks = new Label
                {
                    Text = "Vous n'avez pas de livres empruntés actuellement.",
                    Font = new Font("Poppins", 14, FontStyle.Regular),
                    ForeColor = Color.Gray,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Location = new Point(0, 50),
                    Size = new Size(booksPanel.Width, 40)
                };
                booksPanel.Controls.Add(lblNoBooks);
            }
        }
        
        private Panel CreateBorrowedBookCard(string title, string author, string borrowDate, string returnDate, string imagePath, int width)
        {
            Panel card = new Panel
            {
                Width = width,
                Height = 150,
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
                Size = new Size(100, 130),
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
                Location = new Point(120, 10),
                Size = new Size(width - 130, 30),
                TextAlign = ContentAlignment.MiddleLeft
            };
            
            // Auteur du livre
            Label lblAuthor = new Label
            {
                Text = "Par " + author,
                Font = new Font("Poppins", 10, FontStyle.Regular),
                ForeColor = Color.Gray,
                Location = new Point(120, lblTitle.Bottom),
                Size = new Size(width - 130, 20),
                TextAlign = ContentAlignment.MiddleLeft
            };
            
            // Date d'emprunt
            Label lblBorrowDate = new Label
            {
                Text = "Emprunté le: " + borrowDate,
                Font = new Font("Poppins", 10, FontStyle.Regular),
                ForeColor = Color.Black,
                Location = new Point(120, lblAuthor.Bottom + 10),
                Size = new Size(width - 130, 20),
                TextAlign = ContentAlignment.MiddleLeft
            };
            
            // Date de retour
            Label lblReturnDate = new Label
            {
                Text = "À retourner avant le: " + returnDate,
                Font = new Font("Poppins", 10, FontStyle.Regular),
                ForeColor = Color.Red,
                Location = new Point(120, lblBorrowDate.Bottom),
                Size = new Size(width - 130, 20),
                TextAlign = ContentAlignment.MiddleLeft
            };
            
            // Bouton de prolongation
            Button btnExtend = new Button
            {
                Text = "Prolonger",
                Font = new Font("Poppins", 10, FontStyle.Regular),
                ForeColor = Color.White,
                BackColor = PrimaryColor,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(100, 30),
                Location = new Point(120, lblReturnDate.Bottom + 5),
                Cursor = Cursors.Hand
            };
            btnExtend.FlatAppearance.BorderSize = 0;
            btnExtend.Click += (s, e) => ExtendBorrowPeriod(title);
            
            // Bouton de retour
            Button btnReturn = new Button
            {
                Text = "Retourner",
                Font = new Font("Poppins", 10, FontStyle.Regular),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(200, 0, 0),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(100, 30),
                Location = new Point(230, lblReturnDate.Bottom + 5),
                Cursor = Cursors.Hand
            };
            btnReturn.FlatAppearance.BorderSize = 0;
            btnReturn.Click += (s, e) => ReturnBook(title);
            
            // Ajouter les contrôles à la carte
            card.Controls.Add(bookImage);
            card.Controls.Add(lblTitle);
            card.Controls.Add(lblAuthor);
            card.Controls.Add(lblBorrowDate);
            card.Controls.Add(lblReturnDate);
            card.Controls.Add(btnExtend);
            card.Controls.Add(btnReturn);
            
            return card;
        }
        
        private void ExtendBorrowPeriod(string bookTitle)
        {
            MessageBox.Show($"La période d'emprunt pour '{bookTitle}' a été prolongée de 14 jours.", "Prolongation", MessageBoxButtons.OK, MessageBoxIcon.Information);
            // Ici, vous mettriez à jour la base de données
        }
        
        private void ReturnBook(string bookTitle)
        {
            DialogResult result = MessageBox.Show($"Êtes-vous sûr de vouloir retourner '{bookTitle}' ?", "Retourner le livre", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                MessageBox.Show($"Le livre '{bookTitle}' a été retourné avec succès.", "Retour", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Ici, vous mettriez à jour la base de données
                // Puis vous rechargeriez la liste des livres empruntés
                CreateLibraryContent();
            }
        }
    }
}
