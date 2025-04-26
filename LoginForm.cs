using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using projet_bibliotheque.Data;
using projet_bibliotheque.Models;
using System.Drawing.Drawing2D;
using System.IO;
using projet_bibliotheque.Controls;

namespace projet_bibliotheque
{
    public partial class LoginForm : Form
    {
        private readonly Color PrimaryColor = Color.FromArgb(8, 15, 40);    // Bleu très foncé
        private readonly Color SecondaryColor = Color.FromArgb(0, 0, 70);   // Bleu marine
        private readonly Color TextColor = Color.White;
        private readonly Color ButtonColor = Color.FromArgb(45, 66, 132);   // Bleu moyen
        private readonly Color ButtonHoverColor = Color.FromArgb(60, 85, 150); // Bleu clair

        private TextBox txtEmail = null!;
        private TextBox txtPassword = null!;
        private Button btnLogin = null!;
        private Label lblTitle = null!;
        private LinkLabel lnkRegister = null!;
        private Panel loginPanel = null!;
        private PictureBox logoBox = null!;

        public LoginForm()
        {
            InitializeComponent();
            
            // Initialiser les champs à null pour éviter les erreurs
            txtEmail = null!;
            txtPassword = null!;
            btnLogin = null!;
            lblTitle = null!;
            lnkRegister = null!;
            loginPanel = null!;
            logoBox = null!;
            
            SetupForm();
            this.Paint += new PaintEventHandler(LoginForm_Paint);
            this.Resize += new EventHandler(LoginForm_Resize);
        }

        private void LoginForm_Paint(object sender, PaintEventArgs e)
        {
            using (LinearGradientBrush brush = new LinearGradientBrush(
                this.ClientRectangle,
                PrimaryColor,
                SecondaryColor,
                45f))
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }

            using (SolidBrush overlayBrush = new SolidBrush(Color.FromArgb(100, 0, 0, 0)))
            {
                e.Graphics.FillRectangle(overlayBrush, this.ClientRectangle);
            }
        }

        private void LoginForm_Resize(object sender, EventArgs e)
        {
            this.Invalidate();
            CenterLoginPanel();
        }

        private void CenterLoginPanel()
        {
            if (loginPanel != null)
            {
                loginPanel.Location = new Point(
                    (this.ClientSize.Width - loginPanel.Width) / 2,
                    (this.ClientSize.Height - loginPanel.Height) / 2
                );
            }
        }

        private void SetupForm()
        {
            this.Text = "BiblioHub - Connexion";
            this.MinimumSize = new Size(1200, 800);
            this.Size = new Size(1200, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.DoubleBuffered = true;

            // Logo
            try
            {
                string logoPath = Path.Combine(Application.StartupPath, "Assets", "img", "logo.svg");
                if (File.Exists(logoPath))
                {
                    logoBox = new PictureBox
                    {
                        Size = new Size(80, 80),
                        Location = new Point(30, 30),
                        SizeMode = PictureBoxSizeMode.Zoom,
                        BackColor = Color.Transparent,
                        Image = Image.FromFile(logoPath)
                    };
                    this.Controls.Add(logoBox);
                    logoBox.BringToFront();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement du logo : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Panel principal centré avec fond semi-transparent
            loginPanel = new Panel
            {
                Width = 500,
                Height = 400,
                BackColor = Color.FromArgb(240, 255, 255, 255)
            };

            // Arrondir les coins du panel
            loginPanel.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (GraphicsPath path = RoundRectangle.Create(0, 0, loginPanel.Width, loginPanel.Height, 20))
                {
                    using (SolidBrush brush = new SolidBrush(loginPanel.BackColor))
                    {
                        e.Graphics.FillPath(brush, path);
                    }
                    loginPanel.Region = new Region(path);
                }
            };
            loginPanel.Invalidate(); // Force le redessin du panel

            // Titre
            lblTitle = new Label
            {
                Text = "Se connecter",
                Font = new Font("Poppins", 28, FontStyle.Bold),
                ForeColor = Color.FromArgb(8, 15, 40),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 80
            };

            // Email
            Panel emailPanel = CreateInputPanel("Adresse E-mail :", out txtEmail);
            txtEmail.PlaceholderText = "Entrez votre email";

            // Mot de passe
            Panel passwordPanel = CreateInputPanel("Mot de passe :", out txtPassword);
            txtPassword.PlaceholderText = "Entrez votre mot de passe";
            txtPassword.UseSystemPasswordChar = true;

            // Bouton de connexion
            btnLogin = new Button
            {
                Text = "Connecter",
                Font = new Font("Poppins", 14, FontStyle.Regular),
                ForeColor = Color.White,
                BackColor = ButtonColor,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(200, 45),
                Cursor = Cursors.Hand
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Region = new Region(RoundRectangle.Create(0, 0, btnLogin.Width, btnLogin.Height, 22));
            btnLogin.MouseEnter += (s, e) => btnLogin.BackColor = ButtonHoverColor;
            btnLogin.MouseLeave += (s, e) => btnLogin.BackColor = ButtonColor;
            btnLogin.Click += BtnLogin_Click;

            // Panel pour le bouton
            Panel buttonPanel = new Panel
            {
                Height = 80,
                Dock = DockStyle.Top,
                Padding = new Padding(30, 20, 30, 20)
            };
            
            // Centrer correctement le bouton de connexion
            buttonPanel.Controls.Add(btnLogin);
            btnLogin.Location = new Point((buttonPanel.ClientSize.Width - btnLogin.Width) / 2, 20);

            // Lien d'inscription
            lnkRegister = new LinkLabel
            {
                Text = "Pas encore de compte ? Inscrivez-vous",
                Font = new Font("Poppins", 11),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 40,
                LinkColor = Color.FromArgb(45, 66, 132)
            };
            lnkRegister.Click += LnkRegister_Click;

            // Ajout des contrôles au panel principal dans l'ordre inversé
            // Commencer par le lien d'inscription qui sera tout en bas
            loginPanel.Controls.Add(lnkRegister);
            loginPanel.Controls.Add(buttonPanel); // Bouton de connexion
            loginPanel.Controls.Add(passwordPanel); // Mot de passe
            loginPanel.Controls.Add(emailPanel); // Email
            loginPanel.Controls.Add(lblTitle); // Titre

            // Ajout du panel au formulaire
            this.Controls.Add(loginPanel);
            CenterLoginPanel();

            // Bouton retour
            Button btnBack = new Button
            {
                Text = "←",
                Font = new Font("Poppins", 24, FontStyle.Bold),
                Size = new Size(60, 60),
                Location = new Point(120, 30),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                BackColor = Color.FromArgb(255, 255, 255, 20),
                ForeColor = Color.White
            };
            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.Region = new Region(RoundRectangle.Create(0, 0, btnBack.Width, btnBack.Height, 30));
            btnBack.Click += (s, e) => { this.Hide(); new HomeForm().Show(); };
            btnBack.MouseEnter += (s, e) => btnBack.BackColor = Color.FromArgb(255, 255, 255, 40);
            btnBack.MouseLeave += (s, e) => btnBack.BackColor = Color.FromArgb(255, 255, 255, 20);
            
            this.Controls.Add(btnBack);
            btnBack.BringToFront();
        }

        private Panel CreateInputPanel(string labelText, out TextBox textBox)
        {
            Panel panel = new Panel
            {
                Height = 90,
                Dock = DockStyle.Top,
                Padding = new Padding(30, 5, 30, 5)
            };

            Label label = new Label
            {
                Text = labelText,
                Font = new Font("Poppins", 12, FontStyle.Regular),
                ForeColor = Color.Black,
                Dock = DockStyle.Top,
                Height = 25
            };

            textBox = new TextBox
            {
                Font = new Font("Poppins", 12),
                Dock = DockStyle.Top,
                Height = 35,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White,
                ForeColor = Color.Black
            };

            panel.Controls.Add(textBox);
            panel.Controls.Add(label);

            return panel;
        }

        private async void BtnLogin_Click(object sender, EventArgs e)
        {
            // Validation des champs
            if (string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Veuillez remplir tous les champs.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Connexion à la base de données
            try
            {
                var optionsBuilder = new DbContextOptionsBuilder<LibraryContext>();
                optionsBuilder.UseSqlServer(@"Server=localhost;Database=BibliothequeIHEC;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true")
                    .UseLazyLoadingProxies();

                using (var context = new LibraryContext(optionsBuilder.Options))
                {
                    // Vérifier les identifiants
                    var user = await context.Members
                        .FirstOrDefaultAsync(u => u.Email == txtEmail.Text && u.Password == txtPassword.Text);

                    if (user == null)
                    {
                        MessageBox.Show("Email ou mot de passe incorrect.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Connexion réussie
                    MessageBox.Show($"Bienvenue, {user.Name} !", "Connexion réussie", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    // Rediriger vers le tableau de bord si c'est un bibliothécaire, sinon vers la page principale
                    this.Hide();
                    if (user.IsAdmin)
                    {
                        // Ouvrir le tableau de bord pour les bibliothécaires
                        new MainForm(user).Show();
                    }
                    else
                    {
                        // Ouvrir la page principale pour les étudiants
                        new Forms.StudentForm(user).Show();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la connexion : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LnkRegister_Click(object sender, EventArgs e)
        {
            // Rediriger vers la page d'inscription
            this.Hide();
            new RegisterForm().Show();
        }
    }
}