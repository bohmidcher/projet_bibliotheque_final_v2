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
    public partial class StudentNotificationsForm : Form
    {
        private readonly Member currentUser;
        private readonly Color PrimaryColor = Color.FromArgb(8, 15, 40);  // Bleu foncé
        private readonly Color AccentColor = Color.FromArgb(45, 20, 80);  // Violet foncé
        
        public StudentNotificationsForm(Member user)
        {
            InitializeComponent();
            this.currentUser = user;
            this.BackColor = Color.WhiteSmoke;
            CreateNotificationsContent();
        }
        
        private void CreateNotificationsContent()
        {
            // Titre de la page
            Label lblTitle = new Label
            {
                Text = "Notifications",
                Font = new Font("Poppins", 24, FontStyle.Bold),
                ForeColor = PrimaryColor,
                Location = new Point(20, 20),
                AutoSize = true
            };
            
            // Sous-titre
            Label lblSubtitle = new Label
            {
                Text = "Vos notifications récentes",
                Font = new Font("Poppins", 12, FontStyle.Regular),
                ForeColor = Color.Gray,
                Location = new Point(20, lblTitle.Bottom + 5),
                AutoSize = true
            };
            
            // Ajouter les contrôles au formulaire
            this.Controls.Add(lblTitle);
            this.Controls.Add(lblSubtitle);
            
            // Créer la section des notifications
            CreateNotificationsSection(lblSubtitle.Bottom + 30);
        }
        
        private void CreateNotificationsSection(int startY)
        {
            // Créer un panel pour les notifications
            Panel notificationsPanel = new Panel
            {
                Location = new Point(20, startY),
                Size = new Size(this.Width - 40, this.Height - startY - 40),
                BackColor = Color.Transparent,
                AutoScroll = true
            };
            
            this.Controls.Add(notificationsPanel);
            
            // Ajouter des notifications (simulées)
            List<(string Title, string Message, string Date, bool IsRead, NotificationType Type)> notifications = new List<(string, string, string, bool, NotificationType)>
            {
                ("Retour de livre imminent", "Votre livre 'Marketing Digital' doit être retourné dans 3 jours.", "24/04/2025", false, NotificationType.Warning),
                ("Livre disponible", "Le livre 'Intelligence Artificielle' que vous avez réservé est maintenant disponible.", "22/04/2025", false, NotificationType.Info),
                ("Emprunt confirmé", "Vous avez emprunté 'L'Art de la Finance' avec succès.", "15/04/2025", true, NotificationType.Success),
                ("Retard de retour", "Votre livre 'Économie Moderne' est en retard de 2 jours. Veuillez le retourner dès que possible.", "10/04/2025", true, NotificationType.Error)
            };
            
            int notificationY = 0;
            int notificationHeight = 100;
            int notificationSpacing = 10;
            
            foreach (var notification in notifications)
            {
                Panel notificationCard = CreateNotificationCard(notification.Title, notification.Message, notification.Date, notification.IsRead, notification.Type, notificationsPanel.Width - 20);
                notificationCard.Location = new Point(0, notificationY);
                notificationsPanel.Controls.Add(notificationCard);
                
                notificationY += notificationHeight + notificationSpacing;
            }
            
            // Message si aucune notification
            if (notifications.Count == 0)
            {
                Label lblNoNotifications = new Label
                {
                    Text = "Vous n'avez pas de notifications.",
                    Font = new Font("Poppins", 14, FontStyle.Regular),
                    ForeColor = Color.Gray,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Location = new Point(0, 50),
                    Size = new Size(notificationsPanel.Width, 40)
                };
                notificationsPanel.Controls.Add(lblNoNotifications);
            }
        }
        
        private Panel CreateNotificationCard(string title, string message, string date, bool isRead, NotificationType type, int width)
        {
            Panel card = new Panel
            {
                Width = width,
                Height = 100,
                BackColor = isRead ? Color.White : Color.FromArgb(245, 245, 255)
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
            
            // Indicateur de type de notification
            Color indicatorColor = Color.Blue; // Par défaut
            switch (type)
            {
                case NotificationType.Success:
                    indicatorColor = Color.Green;
                    break;
                case NotificationType.Warning:
                    indicatorColor = Color.Orange;
                    break;
                case NotificationType.Error:
                    indicatorColor = Color.Red;
                    break;
                case NotificationType.Info:
                    indicatorColor = Color.Blue;
                    break;
            }
            
            Panel indicator = new Panel
            {
                Size = new Size(5, card.Height),
                Location = new Point(0, 0),
                BackColor = indicatorColor
            };
            
            // Titre de la notification
            Label lblTitle = new Label
            {
                Text = title,
                Font = new Font("Poppins", 12, FontStyle.Bold),
                ForeColor = Color.Black,
                Location = new Point(15, 10),
                Size = new Size(width - 120, 25),
                TextAlign = ContentAlignment.MiddleLeft
            };
            
            // Message de la notification
            Label lblMessage = new Label
            {
                Text = message,
                Font = new Font("Poppins", 10, FontStyle.Regular),
                ForeColor = Color.Gray,
                Location = new Point(15, lblTitle.Bottom),
                Size = new Size(width - 30, 40),
                TextAlign = ContentAlignment.TopLeft
            };
            
            // Date de la notification
            Label lblDate = new Label
            {
                Text = date,
                Font = new Font("Poppins", 9, FontStyle.Regular),
                ForeColor = Color.DarkGray,
                Location = new Point(width - 100, 10),
                Size = new Size(90, 20),
                TextAlign = ContentAlignment.TopRight
            };
            
            // Bouton de marquage comme lu/non lu
            Button btnMarkRead = new Button
            {
                Text = isRead ? "Marquer comme non lu" : "Marquer comme lu",
                Font = new Font("Poppins", 8, FontStyle.Regular),
                ForeColor = Color.Blue,
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(150, 25),
                Location = new Point(width - 165, lblMessage.Bottom),
                Cursor = Cursors.Hand
            };
            btnMarkRead.FlatAppearance.BorderSize = 0;
            btnMarkRead.Click += (s, e) => ToggleReadStatus(title, isRead);
            
            // Ajouter les contrôles à la carte
            card.Controls.Add(indicator);
            card.Controls.Add(lblTitle);
            card.Controls.Add(lblMessage);
            card.Controls.Add(lblDate);
            card.Controls.Add(btnMarkRead);
            
            return card;
        }
        
        private void ToggleReadStatus(string notificationTitle, bool currentStatus)
        {
            MessageBox.Show($"Notification '{notificationTitle}' marquée comme {(currentStatus ? "non lue" : "lue")}.", "Statut modifié", MessageBoxButtons.OK, MessageBoxIcon.Information);
            // Ici, vous mettriez à jour la base de données
            // Puis vous rechargeriez les notifications
            CreateNotificationsContent();
        }
        
        // Enum pour les types de notifications
        private enum NotificationType
        {
            Info,
            Success,
            Warning,
            Error
        }
    }
}
