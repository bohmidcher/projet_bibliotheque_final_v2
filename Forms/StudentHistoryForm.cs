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
    public partial class StudentHistoryForm : Form
    {
        private readonly Member currentUser;
        private readonly Color PrimaryColor = Color.FromArgb(8, 15, 40);  // Bleu foncé
        private readonly Color AccentColor = Color.FromArgb(45, 20, 80);  // Violet foncé
        
        public StudentHistoryForm(Member user)
        {
            InitializeComponent();
            this.currentUser = user;
            this.BackColor = Color.WhiteSmoke;
            CreateHistoryContent();
        }
        
        private void CreateHistoryContent()
        {
            // Titre de la page
            Label lblTitle = new Label
            {
                Text = "Historique",
                Font = new Font("Poppins", 24, FontStyle.Bold),
                ForeColor = PrimaryColor,
                Location = new Point(20, 20),
                AutoSize = true
            };
            
            // Sous-titre
            Label lblSubtitle = new Label
            {
                Text = "Historique de vos emprunts",
                Font = new Font("Poppins", 12, FontStyle.Regular),
                ForeColor = Color.Gray,
                Location = new Point(20, lblTitle.Bottom + 5),
                AutoSize = true
            };
            
            // Ajouter les contrôles au formulaire
            this.Controls.Add(lblTitle);
            this.Controls.Add(lblSubtitle);
            
            // Créer la section de l'historique
            CreateHistorySection(lblSubtitle.Bottom + 30);
        }
        
        private void CreateHistorySection(int startY)
        {
            // Créer un panel pour l'historique
            Panel historyPanel = new Panel
            {
                Location = new Point(20, startY),
                Size = new Size(this.Width - 40, this.Height - startY - 40),
                BackColor = Color.Transparent,
                AutoScroll = true
            };
            
            this.Controls.Add(historyPanel);
            
            // Ajouter des emprunts historiques (simulés)
            List<(string Title, string Author, string BorrowDate, string ReturnDate, bool IsReturned, string ImagePath)> historyItems = new List<(string, string, string, string, bool, string)>
            {
                ("Marketing Digital", "Marie Laurent", "10/03/2025", "24/03/2025", true, "marketing_book.jpg"),
                ("Intelligence Artificielle", "Pierre Martin", "15/02/2025", "01/03/2025", true, "ai_book.jpg"),
                ("Économie Moderne", "Sophie Dubois", "05/01/2025", "19/01/2025", true, "economy_book.jpg"),
                ("Gestion de Projet", "Thomas Leroy", "10/12/2024", "24/12/2024", true, "project_book.jpg"),
                ("Principes de Comptabilité", "Robert Johnson", "15/11/2024", "29/11/2024", true, "accounting_book.jpg")
            };
            
            int itemY = 0;
            int itemHeight = 120;
            int itemSpacing = 10;
            
            foreach (var item in historyItems)
            {
                Panel historyCard = CreateHistoryCard(item.Title, item.Author, item.BorrowDate, item.ReturnDate, item.IsReturned, item.ImagePath, historyPanel.Width - 20);
                historyCard.Location = new Point(0, itemY);
                historyPanel.Controls.Add(historyCard);
                
                itemY += itemHeight + itemSpacing;
            }
            
            // Message si aucun historique
            if (historyItems.Count == 0)
            {
                Label lblNoHistory = new Label
                {
                    Text = "Vous n'avez pas d'historique d'emprunts.",
                    Font = new Font("Poppins", 14, FontStyle.Regular),
                    ForeColor = Color.Gray,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Location = new Point(0, 50),
                    Size = new Size(historyPanel.Width, 40)
                };
                historyPanel.Controls.Add(lblNoHistory);
            }
        }
        
        private Panel CreateHistoryCard(string title, string author, string borrowDate, string returnDate, bool isReturned, string imagePath, int width)
        {
            Panel card = new Panel
            {
                Width = width,
                Height = 120,
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
                Size = new Size(80, 100),
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
                Font = new Font("Poppins", 12, FontStyle.Bold),
                ForeColor = Color.Black,
                Location = new Point(100, 10),
                Size = new Size(width - 110, 25),
                TextAlign = ContentAlignment.MiddleLeft
            };
            
            // Auteur du livre
            Label lblAuthor = new Label
            {
                Text = "Par " + author,
                Font = new Font("Poppins", 10, FontStyle.Regular),
                ForeColor = Color.Gray,
                Location = new Point(100, lblTitle.Bottom),
                Size = new Size(width - 110, 20),
                TextAlign = ContentAlignment.MiddleLeft
            };
            
            // Date d'emprunt
            Label lblBorrowDate = new Label
            {
                Text = "Emprunté le: " + borrowDate,
                Font = new Font("Poppins", 9, FontStyle.Regular),
                ForeColor = Color.Black,
                Location = new Point(100, lblAuthor.Bottom + 5),
                Size = new Size(200, 20),
                TextAlign = ContentAlignment.MiddleLeft
            };
            
            // Date de retour
            Label lblReturnDate = new Label
            {
                Text = "Retourné le: " + returnDate,
                Font = new Font("Poppins", 9, FontStyle.Regular),
                ForeColor = Color.Black,
                Location = new Point(100, lblBorrowDate.Bottom),
                Size = new Size(200, 20),
                TextAlign = ContentAlignment.MiddleLeft
            };
            
            // Statut
            Label lblStatus = new Label
            {
                Text = isReturned ? "Retourné" : "Non retourné",
                Font = new Font("Poppins", 10, FontStyle.Bold),
                ForeColor = isReturned ? Color.Green : Color.Red,
                Location = new Point(width - 120, 50),
                Size = new Size(100, 20),
                TextAlign = ContentAlignment.MiddleRight
            };
            
            // Ajouter les contrôles à la carte
            card.Controls.Add(bookImage);
            card.Controls.Add(lblTitle);
            card.Controls.Add(lblAuthor);
            card.Controls.Add(lblBorrowDate);
            card.Controls.Add(lblReturnDate);
            card.Controls.Add(lblStatus);
            
            return card;
        }
    }
}
