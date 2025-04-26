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
    public partial class StudentSettingsForm : Form
    {
        private readonly Member currentUser;
        private readonly Color PrimaryColor = Color.FromArgb(8, 15, 40);  // Bleu foncé
        private readonly Color AccentColor = Color.FromArgb(45, 20, 80);  // Violet foncé
        
        public StudentSettingsForm(Member user)
        {
            InitializeComponent();
            this.currentUser = user;
            this.BackColor = Color.WhiteSmoke;
            CreateSettingsContent();
        }
        
        private void CreateSettingsContent()
        {
            // Titre de la page
            Label lblTitle = new Label
            {
                Text = "Paramètres",
                Font = new Font("Poppins", 24, FontStyle.Bold),
                ForeColor = PrimaryColor,
                Location = new Point(20, 20),
                AutoSize = true
            };
            
            // Sous-titre
            Label lblSubtitle = new Label
            {
                Text = "Personnalisez votre expérience",
                Font = new Font("Poppins", 12, FontStyle.Regular),
                ForeColor = Color.Gray,
                Location = new Point(20, lblTitle.Bottom + 5),
                AutoSize = true
            };
            
            // Ajouter les contrôles au formulaire
            this.Controls.Add(lblTitle);
            this.Controls.Add(lblSubtitle);
            
            // Créer les sections de paramètres
            CreateAppearanceSection(lblSubtitle.Bottom + 30);
            CreateNotificationSection(lblSubtitle.Bottom + 200);
            CreateAccountSection(lblSubtitle.Bottom + 370);
        }
        
        private void CreateAppearanceSection(int startY)
        {
            // Titre de la section
            Label lblAppearance = new Label
            {
                Text = "Apparence",
                Font = new Font("Poppins", 16, FontStyle.Bold),
                ForeColor = PrimaryColor,
                Location = new Point(20, startY),
                AutoSize = true
            };
            
            this.Controls.Add(lblAppearance);
            
            // Panel pour les options d'apparence
            Panel appearancePanel = new Panel
            {
                Location = new Point(20, lblAppearance.Bottom + 10),
                Size = new Size(this.Width - 40, 120),
                BackColor = Color.White
            };
            
            this.Controls.Add(appearancePanel);
            
            // Thème
            Label lblTheme = new Label
            {
                Text = "Thème",
                Font = new Font("Poppins", 12, FontStyle.Regular),
                ForeColor = Color.Black,
                Location = new Point(20, 20),
                Size = new Size(100, 30)
            };
            
            RadioButton rdoLight = new RadioButton
            {
                Text = "Clair",
                Font = new Font("Poppins", 10),
                Location = new Point(150, 20),
                Size = new Size(100, 30),
                Checked = true
            };
            
            RadioButton rdoDark = new RadioButton
            {
                Text = "Sombre",
                Font = new Font("Poppins", 10),
                Location = new Point(250, 20),
                Size = new Size(100, 30)
            };
            
            // Langue
            Label lblLanguage = new Label
            {
                Text = "Langue",
                Font = new Font("Poppins", 12, FontStyle.Regular),
                ForeColor = Color.Black,
                Location = new Point(20, 60),
                Size = new Size(100, 30)
            };
            
            ComboBox cmbLanguage = new ComboBox
            {
                Font = new Font("Poppins", 10),
                Location = new Point(150, 60),
                Size = new Size(200, 30),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbLanguage.Items.AddRange(new string[] { "Français", "English" });
            cmbLanguage.SelectedIndex = 0;
            
            // Ajouter les contrôles au panel
            appearancePanel.Controls.Add(lblTheme);
            appearancePanel.Controls.Add(rdoLight);
            appearancePanel.Controls.Add(rdoDark);
            appearancePanel.Controls.Add(lblLanguage);
            appearancePanel.Controls.Add(cmbLanguage);
        }
        
        private void CreateNotificationSection(int startY)
        {
            // Titre de la section
            Label lblNotifications = new Label
            {
                Text = "Notifications",
                Font = new Font("Poppins", 16, FontStyle.Bold),
                ForeColor = PrimaryColor,
                Location = new Point(20, startY),
                AutoSize = true
            };
            
            this.Controls.Add(lblNotifications);
            
            // Panel pour les options de notifications
            Panel notificationsPanel = new Panel
            {
                Location = new Point(20, lblNotifications.Bottom + 10),
                Size = new Size(this.Width - 40, 120),
                BackColor = Color.White
            };
            
            this.Controls.Add(notificationsPanel);
            
            // Notifications par email
            CheckBox chkEmailNotifications = new CheckBox
            {
                Text = "Recevoir des notifications par email",
                Font = new Font("Poppins", 10),
                Location = new Point(20, 20),
                Size = new Size(300, 30),
                Checked = true
            };
            
            // Notifications de retour imminent
            CheckBox chkReturnNotifications = new CheckBox
            {
                Text = "Notifications de retour imminent (3 jours avant)",
                Font = new Font("Poppins", 10),
                Location = new Point(20, 50),
                Size = new Size(350, 30),
                Checked = true
            };
            
            // Notifications de nouveaux livres
            CheckBox chkNewBooksNotifications = new CheckBox
            {
                Text = "Notifications de nouveaux livres disponibles",
                Font = new Font("Poppins", 10),
                Location = new Point(20, 80),
                Size = new Size(350, 30),
                Checked = true
            };
            
            // Ajouter les contrôles au panel
            notificationsPanel.Controls.Add(chkEmailNotifications);
            notificationsPanel.Controls.Add(chkReturnNotifications);
            notificationsPanel.Controls.Add(chkNewBooksNotifications);
        }
        
        private void CreateAccountSection(int startY)
        {
            // Titre de la section
            Label lblAccount = new Label
            {
                Text = "Compte",
                Font = new Font("Poppins", 16, FontStyle.Bold),
                ForeColor = PrimaryColor,
                Location = new Point(20, startY),
                AutoSize = true
            };
            
            this.Controls.Add(lblAccount);
            
            // Panel pour les options de compte
            Panel accountPanel = new Panel
            {
                Location = new Point(20, lblAccount.Bottom + 10),
                Size = new Size(this.Width - 40, 150),
                BackColor = Color.White
            };
            
            this.Controls.Add(accountPanel);
            
            // Changer le mot de passe
            Button btnChangePassword = new Button
            {
                Text = "Changer le mot de passe",
                Font = new Font("Poppins", 10),
                Location = new Point(20, 20),
                Size = new Size(200, 35),
                FlatStyle = FlatStyle.Flat,
                BackColor = PrimaryColor,
                ForeColor = Color.White
            };
            btnChangePassword.FlatAppearance.BorderSize = 0;
            btnChangePassword.Click += (s, e) => ChangePassword();
            
            // Mettre à jour les informations du profil
            Button btnUpdateProfile = new Button
            {
                Text = "Mettre à jour le profil",
                Font = new Font("Poppins", 10),
                Location = new Point(20, 65),
                Size = new Size(200, 35),
                FlatStyle = FlatStyle.Flat,
                BackColor = PrimaryColor,
                ForeColor = Color.White
            };
            btnUpdateProfile.FlatAppearance.BorderSize = 0;
            btnUpdateProfile.Click += (s, e) => UpdateProfile();
            
            // Bouton de sauvegarde des paramètres
            Button btnSaveSettings = new Button
            {
                Text = "Enregistrer les paramètres",
                Font = new Font("Poppins", 12, FontStyle.Bold),
                Location = new Point(accountPanel.Width - 250, 100),
                Size = new Size(230, 40),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Green,
                ForeColor = Color.White
            };
            btnSaveSettings.FlatAppearance.BorderSize = 0;
            btnSaveSettings.Click += (s, e) => SaveSettings();
            
            // Ajouter les contrôles au panel
            accountPanel.Controls.Add(btnChangePassword);
            accountPanel.Controls.Add(btnUpdateProfile);
            accountPanel.Controls.Add(btnSaveSettings);
        }
        
        private void ChangePassword()
        {
            // Créer un formulaire de changement de mot de passe
            Form passwordForm = new Form
            {
                Text = "Changer le mot de passe",
                Size = new Size(400, 300),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };
            
            Label lblCurrentPassword = new Label
            {
                Text = "Mot de passe actuel:",
                Location = new Point(20, 20),
                Size = new Size(150, 20)
            };
            
            TextBox txtCurrentPassword = new TextBox
            {
                Location = new Point(20, 45),
                Size = new Size(340, 25),
                UseSystemPasswordChar = true
            };
            
            Label lblNewPassword = new Label
            {
                Text = "Nouveau mot de passe:",
                Location = new Point(20, 80),
                Size = new Size(150, 20)
            };
            
            TextBox txtNewPassword = new TextBox
            {
                Location = new Point(20, 105),
                Size = new Size(340, 25),
                UseSystemPasswordChar = true
            };
            
            Label lblConfirmPassword = new Label
            {
                Text = "Confirmer le nouveau mot de passe:",
                Location = new Point(20, 140),
                Size = new Size(250, 20)
            };
            
            TextBox txtConfirmPassword = new TextBox
            {
                Location = new Point(20, 165),
                Size = new Size(340, 25),
                UseSystemPasswordChar = true
            };
            
            Button btnSave = new Button
            {
                Text = "Enregistrer",
                Location = new Point(190, 210),
                Size = new Size(170, 35),
                BackColor = PrimaryColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSave.FlatAppearance.BorderSize = 0;
            
            Button btnCancel = new Button
            {
                Text = "Annuler",
                Location = new Point(20, 210),
                Size = new Size(150, 35),
                BackColor = Color.Gray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            
            btnSave.Click += (s, e) =>
            {
                if (string.IsNullOrEmpty(txtCurrentPassword.Text) || string.IsNullOrEmpty(txtNewPassword.Text) || string.IsNullOrEmpty(txtConfirmPassword.Text))
                {
                    MessageBox.Show("Veuillez remplir tous les champs.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                if (txtNewPassword.Text != txtConfirmPassword.Text)
                {
                    MessageBox.Show("Les nouveaux mots de passe ne correspondent pas.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                // Ici, vous vérifieriez le mot de passe actuel et mettriez à jour le nouveau mot de passe dans la base de données
                MessageBox.Show("Mot de passe modifié avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                passwordForm.Close();
            };
            
            btnCancel.Click += (s, e) => passwordForm.Close();
            
            passwordForm.Controls.Add(lblCurrentPassword);
            passwordForm.Controls.Add(txtCurrentPassword);
            passwordForm.Controls.Add(lblNewPassword);
            passwordForm.Controls.Add(txtNewPassword);
            passwordForm.Controls.Add(lblConfirmPassword);
            passwordForm.Controls.Add(txtConfirmPassword);
            passwordForm.Controls.Add(btnSave);
            passwordForm.Controls.Add(btnCancel);
            
            passwordForm.ShowDialog();
        }
        
        private void UpdateProfile()
        {
            MessageBox.Show("Cette fonctionnalité vous permet de mettre à jour vos informations de profil.", "Mise à jour du profil", MessageBoxButtons.OK, MessageBoxIcon.Information);
            // Ici, vous pourriez ouvrir un formulaire pour mettre à jour les informations du profil
        }
        
        private void SaveSettings()
        {
            MessageBox.Show("Paramètres enregistrés avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
            // Ici, vous sauvegarderiez les paramètres dans la base de données
        }
    }
}
