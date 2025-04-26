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
    public partial class StudentProfileForm : Form
    {
        private readonly Member currentUser;
        private readonly Color PrimaryColor = Color.FromArgb(8, 15, 40);  // Bleu foncé
        private readonly Color AccentColor = Color.FromArgb(45, 20, 80);  // Violet foncé
        
        public StudentProfileForm(Member user)
        {
            InitializeComponent();
            this.currentUser = user;
            this.BackColor = Color.WhiteSmoke;
            CreateProfileContent();
        }
        
        private void CreateProfileContent()
        {
            // Titre de la page
            Label lblTitle = new Label
            {
                Text = "Profil",
                Font = new Font("Poppins", 24, FontStyle.Bold),
                ForeColor = PrimaryColor,
                Location = new Point(20, 20),
                AutoSize = true
            };
            
            // Sous-titre
            Label lblSubtitle = new Label
            {
                Text = "Vos informations personnelles",
                Font = new Font("Poppins", 12, FontStyle.Regular),
                ForeColor = Color.Gray,
                Location = new Point(20, lblTitle.Bottom + 5),
                AutoSize = true
            };
            
            // Ajouter les contrôles au formulaire
            this.Controls.Add(lblTitle);
            this.Controls.Add(lblSubtitle);
            
            // Créer la section du profil
            CreateProfileSection(lblSubtitle.Bottom + 30);
            
            // Créer la section des statistiques
            CreateStatsSection(lblSubtitle.Bottom + 350);
        }
        
        private void CreateProfileSection(int startY)
        {
            // Panel pour les informations de profil
            Panel profilePanel = new Panel
            {
                Location = new Point(20, startY),
                Size = new Size(this.Width - 40, 300),
                BackColor = Color.White
            };
            
            this.Controls.Add(profilePanel);
            
            // Avatar de l'utilisateur
            PictureBox avatarBox = new PictureBox
            {
                Size = new Size(150, 150),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.LightGray,
                Location = new Point(30, 30),
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
            
            // Bouton pour changer l'avatar
            Button btnChangeAvatar = new Button
            {
                Text = "Changer l'avatar",
                Font = new Font("Poppins", 9, FontStyle.Regular),
                ForeColor = Color.White,
                BackColor = PrimaryColor,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(150, 30),
                Location = new Point(30, avatarBox.Bottom + 10),
                Cursor = Cursors.Hand
            };
            btnChangeAvatar.FlatAppearance.BorderSize = 0;
            btnChangeAvatar.Click += (s, e) => ChangeAvatar();
            
            // Informations de l'utilisateur
            int infoX = avatarBox.Right + 50;
            int infoWidth = profilePanel.Width - infoX - 30;
            
            // Nom
            Label lblName = new Label
            {
                Text = "Nom:",
                Font = new Font("Poppins", 12, FontStyle.Bold),
                ForeColor = Color.Black,
                Location = new Point(infoX, 30),
                Size = new Size(100, 25)
            };
            
            Label lblNameValue = new Label
            {
                Text = currentUser != null ? currentUser.Name : "Nom de l'étudiant",
                Font = new Font("Poppins", 12, FontStyle.Regular),
                ForeColor = Color.Black,
                Location = new Point(infoX + 100, 30),
                Size = new Size(infoWidth - 100, 25)
            };
            
            // Email
            Label lblEmail = new Label
            {
                Text = "Email:",
                Font = new Font("Poppins", 12, FontStyle.Bold),
                ForeColor = Color.Black,
                Location = new Point(infoX, 65),
                Size = new Size(100, 25)
            };
            
            Label lblEmailValue = new Label
            {
                Text = currentUser != null ? currentUser.Email : "email@ihec.tn",
                Font = new Font("Poppins", 12, FontStyle.Regular),
                ForeColor = Color.Black,
                Location = new Point(infoX + 100, 65),
                Size = new Size(infoWidth - 100, 25)
            };
            
            // Date de naissance
            Label lblBirthdate = new Label
            {
                Text = "Né(e) le:",
                Font = new Font("Poppins", 12, FontStyle.Bold),
                ForeColor = Color.Black,
                Location = new Point(infoX, 100),
                Size = new Size(100, 25)
            };
            
            Label lblBirthdateValue = new Label
            {
                Text = currentUser != null && currentUser.Birthdate.HasValue ? currentUser.Birthdate.Value.ToString("dd/MM/yyyy") : "01/01/2000",
                Font = new Font("Poppins", 12, FontStyle.Regular),
                ForeColor = Color.Black,
                Location = new Point(infoX + 100, 100),
                Size = new Size(infoWidth - 100, 25)
            };
            
            // Niveau éducatif
            Label lblEducation = new Label
            {
                Text = "Niveau:",
                Font = new Font("Poppins", 12, FontStyle.Bold),
                ForeColor = Color.Black,
                Location = new Point(infoX, 135),
                Size = new Size(100, 25)
            };
            
            Label lblEducationValue = new Label
            {
                Text = currentUser != null ? currentUser.NiveauEducatif : "3ème année Licence en Informatique",
                Font = new Font("Poppins", 12, FontStyle.Regular),
                ForeColor = Color.Black,
                Location = new Point(infoX + 100, 135),
                Size = new Size(infoWidth - 100, 25)
            };
            
            // Date d'inscription
            Label lblRegistration = new Label
            {
                Text = "Inscrit le:",
                Font = new Font("Poppins", 12, FontStyle.Bold),
                ForeColor = Color.Black,
                Location = new Point(infoX, 170),
                Size = new Size(100, 25)
            };
            
            Label lblRegistrationValue = new Label
            {
                Text = currentUser != null ? currentUser.DateInscription.ToString("dd/MM/yyyy") : "01/09/2024",
                Font = new Font("Poppins", 12, FontStyle.Regular),
                ForeColor = Color.Black,
                Location = new Point(infoX + 100, 170),
                Size = new Size(infoWidth - 100, 25)
            };
            
            // Bouton pour modifier le profil
            Button btnEditProfile = new Button
            {
                Text = "Modifier le profil",
                Font = new Font("Poppins", 12, FontStyle.Regular),
                ForeColor = Color.White,
                BackColor = PrimaryColor,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(200, 40),
                Location = new Point(infoX, 220),
                Cursor = Cursors.Hand
            };
            btnEditProfile.FlatAppearance.BorderSize = 0;
            btnEditProfile.Click += (s, e) => EditProfile();
            
            // Ajouter les contrôles au panel
            profilePanel.Controls.Add(avatarBox);
            profilePanel.Controls.Add(btnChangeAvatar);
            profilePanel.Controls.Add(lblName);
            profilePanel.Controls.Add(lblNameValue);
            profilePanel.Controls.Add(lblEmail);
            profilePanel.Controls.Add(lblEmailValue);
            profilePanel.Controls.Add(lblBirthdate);
            profilePanel.Controls.Add(lblBirthdateValue);
            profilePanel.Controls.Add(lblEducation);
            profilePanel.Controls.Add(lblEducationValue);
            profilePanel.Controls.Add(lblRegistration);
            profilePanel.Controls.Add(lblRegistrationValue);
            profilePanel.Controls.Add(btnEditProfile);
        }
        
        private void CreateStatsSection(int startY)
        {
            // Titre de la section
            Label lblStats = new Label
            {
                Text = "Statistiques",
                Font = new Font("Poppins", 16, FontStyle.Bold),
                ForeColor = PrimaryColor,
                Location = new Point(20, startY),
                AutoSize = true
            };
            
            this.Controls.Add(lblStats);
            
            // Panel pour les statistiques
            Panel statsPanel = new Panel
            {
                Location = new Point(20, lblStats.Bottom + 10),
                Size = new Size(this.Width - 40, 150),
                BackColor = Color.White
            };
            
            this.Controls.Add(statsPanel);
            
            // Statistiques
            CreateStatItem(statsPanel, "Livres empruntés", "15", 30, 30);
            CreateStatItem(statsPanel, "Livres actuellement empruntés", "3", 30, 80);
            CreateStatItem(statsPanel, "Retards", "0", statsPanel.Width / 2 + 30, 30);
            CreateStatItem(statsPanel, "Catégorie préférée", "Informatique", statsPanel.Width / 2 + 30, 80);
        }
        
        private void CreateStatItem(Panel parent, string label, string value, int x, int y)
        {
            Label lblStatLabel = new Label
            {
                Text = label,
                Font = new Font("Poppins", 12, FontStyle.Regular),
                ForeColor = Color.Gray,
                Location = new Point(x, y),
                Size = new Size(250, 25)
            };
            
            Label lblStatValue = new Label
            {
                Text = value,
                Font = new Font("Poppins", 14, FontStyle.Bold),
                ForeColor = PrimaryColor,
                Location = new Point(x, y + 25),
                Size = new Size(250, 25)
            };
            
            parent.Controls.Add(lblStatLabel);
            parent.Controls.Add(lblStatValue);
        }
        
        private void ChangeAvatar()
        {
            MessageBox.Show("Cette fonctionnalité vous permet de changer votre avatar.", "Changer l'avatar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            // Ici, vous pourriez ouvrir un dialogue pour sélectionner une image
        }
        
        private void EditProfile()
        {
            MessageBox.Show("Cette fonctionnalité vous permet de modifier vos informations de profil.", "Modifier le profil", MessageBoxButtons.OK, MessageBoxIcon.Information);
            // Ici, vous pourriez ouvrir un formulaire pour modifier les informations du profil
        }
    }
}
