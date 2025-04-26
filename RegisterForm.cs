using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using projet_bibliotheque.Data;
using projet_bibliotheque.Models;
using System.Drawing.Drawing2D;
using projet_bibliotheque.Controls;
using System.Text.RegularExpressions;
using System.IO;

namespace projet_bibliotheque
{
    public partial class RegisterForm : Form
    {
        private readonly Color PrimaryColor = Color.FromArgb(8, 15, 40);    // Bleu très foncé
        private readonly Color SecondaryColor = Color.FromArgb(0, 0, 70);   // Bleu marine
        private readonly Color TextColor = Color.White;
        private readonly Color ButtonColor = Color.FromArgb(45, 66, 132);   // Bleu moyen
        private readonly Color ButtonHoverColor = Color.FromArgb(60, 85, 150); // Bleu clair

        private TextBox txtName = null!;
        private TextBox txtEmail = null!;
        private TextBox txtPassword = null!;
        private TextBox txtConfirmPassword = null!;
        private ComboBox cmbNiveauEducatif = null!;
        private ComboBox cmbSpecialite = null!;
        private DateTimePicker dtpBirthdate = null!;
        private RadioButton rdoEtudiant = null!;
        private RadioButton rdoBibliothecaire = null!;
        private Button btnRegister = null!;
        private Label lblTitle = null!;
        private LinkLabel lnkLogin = null!;
        private Panel registerPanel = null!;
        private PictureBox logoBox = null!;
        private Panel etudiantPanel = null!;
        private Panel birthdatePanel = null!;

        public RegisterForm()
        {
            InitializeComponent();
            
            // Initialiser les champs à null pour éviter les erreurs
            txtName = null!;
            txtEmail = null!;
            txtPassword = null!;
            txtConfirmPassword = null!;
            cmbNiveauEducatif = null!;
            cmbSpecialite = null!;
            dtpBirthdate = null!;
            rdoEtudiant = null!;
            rdoBibliothecaire = null!;
            btnRegister = null!;
            lblTitle = null!;
            lnkLogin = null!;
            registerPanel = null!;
            logoBox = null!;
            etudiantPanel = null!;
            birthdatePanel = null!;
            
            SetupForm();
            this.Paint += new PaintEventHandler(RegisterForm_Paint);
            this.Resize += new EventHandler(RegisterForm_Resize);
            this.FormClosing += new FormClosingEventHandler(RegisterForm_FormClosing);
        }

        private void RegisterForm_Paint(object sender, PaintEventArgs e)
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

        private void RegisterForm_Resize(object sender, EventArgs e)
        {
            this.Invalidate();
            CenterRegisterPanel();
        }

        private void CenterRegisterPanel()
        {
            if (registerPanel != null)
            {
                registerPanel.Location = new Point(
                    (this.ClientSize.Width - registerPanel.Width) / 2,
                    (this.ClientSize.Height - registerPanel.Height) / 2
                );
            }
        }

        private void SetupForm()
        {
            this.Text = "BiblioHub - Inscription";
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
            registerPanel = new Panel
            {
                Width = 500,
                Height = 700,
                BackColor = Color.FromArgb(240, 255, 255, 255)
            };

            // Arrondir les coins du panel
            registerPanel.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (GraphicsPath path = RoundRectangle.Create(0, 0, registerPanel.Width, registerPanel.Height, 20))
                {
                    using (SolidBrush brush = new SolidBrush(registerPanel.BackColor))
                    {
                        e.Graphics.FillPath(brush, path);
                    }
                    registerPanel.Region = new Region(path);
                }
            };
            registerPanel.Invalidate(); // Force le redessin du panel

            // Titre
            lblTitle = new Label
            {
                Text = "Inscription",
                Font = new Font("Poppins", 28, FontStyle.Bold),
                ForeColor = TextColor,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 80
            };

            // Panel pour le choix du statut
            Panel statusPanel = new Panel
            {
                Height = 200,
                Dock = DockStyle.Top,
                Padding = new Padding(30, 20, 30, 20)
            };

            // Bouton Étudiant
            Button btnEtudiant = new Button
            {
                Text = "Étudiant",
                Font = new Font("Poppins", 14, FontStyle.Regular),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(0, 0, 20),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(200, 45),
                Cursor = Cursors.Hand,
                Dock = DockStyle.None
            };
            btnEtudiant.FlatAppearance.BorderSize = 0;
            btnEtudiant.Region = new Region(RoundRectangle.Create(0, 0, btnEtudiant.Width, btnEtudiant.Height, 22));
            btnEtudiant.MouseEnter += (s, e) => btnEtudiant.BackColor = Color.FromArgb(0, 0, 40);
            btnEtudiant.MouseLeave += (s, e) => btnEtudiant.BackColor = Color.FromArgb(0, 0, 20);
            btnEtudiant.Click += (s, e) => ShowRegistrationForm(false);

            // Bouton Bibliothécaire
            Button btnBibliothecaire = new Button
            {
                Text = "Bibliothécaire (Admin)",
                Font = new Font("Poppins", 14, FontStyle.Regular),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(0, 0, 20),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(200, 45),
                Cursor = Cursors.Hand,
                Dock = DockStyle.None
            };
            btnBibliothecaire.FlatAppearance.BorderSize = 0;
            btnBibliothecaire.Region = new Region(RoundRectangle.Create(0, 0, btnBibliothecaire.Width, btnBibliothecaire.Height, 22));
            btnBibliothecaire.MouseEnter += (s, e) => btnBibliothecaire.BackColor = Color.FromArgb(0, 0, 40);
            btnBibliothecaire.MouseLeave += (s, e) => btnBibliothecaire.BackColor = Color.FromArgb(0, 0, 20);
            btnBibliothecaire.Click += (s, e) => ShowRegistrationForm(true);

            // Centrer les boutons verticalement l'un au-dessus de l'autre et au milieu du panel
            int centerX = (registerPanel.Width - btnEtudiant.Width) / 2;
            btnEtudiant.Location = new Point(centerX, 20);
            btnBibliothecaire.Location = new Point(centerX, 85); // 20 + 45 + 20 (marge)

            statusPanel.Controls.Add(btnBibliothecaire);
            statusPanel.Controls.Add(btnEtudiant);

            // Lien de connexion
            lnkLogin = new LinkLabel
            {
                Text = "Déjà un compte ? Connectez-vous",
                Font = new Font("Poppins", 11),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 40,
                LinkColor = Color.FromArgb(0, 0, 20)
            };
            lnkLogin.Click += LnkLogin_Click;

            // Ajout des contrôles au panel principal
            registerPanel.Controls.Add(lnkLogin);
            registerPanel.Controls.Add(statusPanel);
            registerPanel.Controls.Add(lblTitle);

            // Ajout du panel au formulaire
            this.Controls.Add(registerPanel);
            CenterRegisterPanel();

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

        private void ShowRegistrationForm(bool isAdmin)
        {
            // Vider le panel principal
            registerPanel.Controls.Clear();

            // Titre
            lblTitle = new Label
            {
                Text = "Inscription",
                Font = new Font("Poppins", 28, FontStyle.Bold),
                ForeColor = Color.FromArgb(8, 15, 40),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 80
            };

            // Nom
            Panel namePanel = CreateInputPanel("Nom", out txtName);
            txtName.PlaceholderText = "Entrez votre nom";

            // Email
            Panel emailPanel = CreateInputPanel("Email", out txtEmail);
            txtEmail.PlaceholderText = "Entrez votre email";

            // Mot de passe
            Panel passwordPanel = CreateInputPanel("Mot de passe", out txtPassword);
            txtPassword.PlaceholderText = "Entrez votre mot de passe";
            txtPassword.UseSystemPasswordChar = true;

            // Confirmer mot de passe
            Panel confirmPasswordPanel = CreateInputPanel("Confirmer le mot de passe", out txtConfirmPassword);
            txtConfirmPassword.PlaceholderText = "Confirmez votre mot de passe";
            txtConfirmPassword.UseSystemPasswordChar = true;

            // Date de naissance
            birthdatePanel = new Panel
            {
                Height = 90,
                Dock = DockStyle.Top,
                Padding = new Padding(30, 5, 30, 5)
            };

            Label lblBirthdate = new Label
            {
                Text = "Date de naissance",
                Font = new Font("Poppins", 12),
                ForeColor = Color.Black,
                Dock = DockStyle.Top,
                Height = 25
            };

            dtpBirthdate = new DateTimePicker
            {
                Font = new Font("Poppins", 12),
                Format = DateTimePickerFormat.Short,
                CustomFormat = "dd/MM/yyyy",
                ShowUpDown = false,
                Dock = DockStyle.Top,
                Height = 35,
                MaxDate = DateTime.Now.AddYears(-16),
                MinDate = DateTime.Now.AddYears(-100),
                Value = DateTime.Now.AddYears(-18)
            };

            birthdatePanel.Controls.Add(dtpBirthdate);
            birthdatePanel.Controls.Add(lblBirthdate);

            // Initialiser les boutons radio pour le type d'utilisateur
            rdoEtudiant = new RadioButton
            {
                Text = "Étudiant",
                Font = new Font("Poppins", 12),
                Checked = !isAdmin,
                Visible = false // Caché car déjà choisi
            };

            rdoBibliothecaire = new RadioButton
            {
                Text = "Bibliothécaire",
                Font = new Font("Poppins", 12),
                Checked = isAdmin,
                Visible = false // Caché car déjà choisi
            };

            // Panel pour les informations étudiant (si non admin)
            if (!isAdmin)
            {
                etudiantPanel = new Panel
                {
                    Height = 120, // Réduction de la hauteur car on supprime un champ
                    Dock = DockStyle.Top,
                    Padding = new Padding(30, 5, 30, 5),
                    Visible = true,
                    BackColor = Color.White
                };

                // Niveau éducatif
                Label lblNiveau = new Label
                {
                    Text = "Niveau éducatif",
                    Font = new Font("Poppins", 12),
                    ForeColor = Color.Black,
                    Dock = DockStyle.Top,
                    Height = 25,
                    Margin = new Padding(0, 5, 0, 0)
                };

                cmbNiveauEducatif = new ComboBox
                {
                    Font = new Font("Poppins", 12),
                    Dock = DockStyle.Top,
                    Height = 35,
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    BackColor = Color.White
                };
                cmbNiveauEducatif.Items.AddRange(new string[] {
                    "1ère année Sciences de gestion",
                    "1ère année Informatiques de gestion",
                    "2ème année Sciences de gestion",
                    "2ème année Comptabilité",
                    "1ère année Business Intelligence",
                    "3ème année Management",
                    "3ème année Marketing",
                    "3ème année Finances",
                    "3ème année Comptabilité",
                    "3ème année Buisiness Intelligence",
                    "1ère année Master Recherche en Management Stratégie et Conseil",
                    "1ère année Master Recherche en Marketing",
                    "1ère année Master Recherche en Ingénierie Economique et Financière",
                    "1ère année Master Recherche en Finances",
                    "1ère année Master Professionnel en Analyste Financier",
                    "1ère année Master Master Professionnel en Marketing Moderne et Veille Stratégique",
                    "1ère année Master Master Professionnel en Comptabilité",
                    "1ère année Master Master Professionnel en Big Data Analytics et e-commerce",
                    "1ère année Master Master Professionnel en Entreprenariat",
                    "1ère année Master Master Professionnel en Hospitality Tourism and Product",
                    "2ème année Master Recherche en Management Stratégie et Conseil",
                    "2ème année Master Recherche en Marketing",
                    "2ème année Master Recherche en Ingénierie Economique et Financière",
                    "2ème année Master Recherche en Finances",
                    "2ème année Master Professionnel en Analyste Financier",
                    "2ème année Master Master Professionnel en Marketing Moderne et Veille Stratégique",
                    "2ème année Master Master Professionnel en Comptabilité",
                    "2ème année Master Master Professionnel en Big Data Analytics et e-commerce",
                    "2ème année Master Master Professionnel en Entreprenariat",
                    "2ème année Master Master Professionnel en Hospitality Tourism and Product",
                });
                cmbNiveauEducatif.SelectedIndex = 0;

                // Suppression du champ Spécialité et de son label

                etudiantPanel.Controls.Add(lblNiveau);
                etudiantPanel.Controls.Add(cmbNiveauEducatif);
            }

            // Bouton d'inscription
            btnRegister = new Button
            {
                Text = "S'inscrire",
                Font = new Font("Poppins", 14, FontStyle.Regular),
                ForeColor = Color.White,
                BackColor = ButtonColor,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(340, 45),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.None
            };
            btnRegister.FlatAppearance.BorderSize = 0;
            btnRegister.Region = new Region(RoundRectangle.Create(0, 0, btnRegister.Width, btnRegister.Height, 22));
            btnRegister.MouseEnter += (s, e) => btnRegister.BackColor = ButtonHoverColor;
            btnRegister.MouseLeave += (s, e) => btnRegister.BackColor = ButtonColor;
            btnRegister.Click += BtnRegister_Click;

            Panel buttonPanel = new Panel
            {
                Height = 80,
                Dock = DockStyle.Top,
                Padding = new Padding(0)
            };
            
            // Centrer correctement le bouton d'inscription
            btnRegister.Location = new Point((buttonPanel.ClientSize.Width - btnRegister.Width) / 2, 20);
            buttonPanel.Controls.Add(btnRegister);

            // Lien de connexion
            lnkLogin = new LinkLabel
            {
                Text = "Déjà un compte ? Connectez-vous",
                Font = new Font("Poppins", 11),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 40,
                LinkColor = Color.FromArgb(0, 0, 20)
            };
            lnkLogin.Click += LnkLogin_Click;

            // Ajout des contrôles au panel principal dans l'ordre inversé
            // Commencer par le lien de connexion qui sera tout en bas
            registerPanel.Controls.Add(lnkLogin);
            
            if (isAdmin)
            {
                // Pour le bibliothécaire, ordre inversé
                registerPanel.Controls.Add(buttonPanel); // Bouton d'inscription
                registerPanel.Controls.Add(birthdatePanel); // Date de naissance
                registerPanel.Controls.Add(confirmPasswordPanel); // Confirmation mot de passe
                registerPanel.Controls.Add(passwordPanel); // Mot de passe
                registerPanel.Controls.Add(emailPanel); // Email
                registerPanel.Controls.Add(namePanel); // Nom
                registerPanel.Controls.Add(lblTitle); // Titre
            }
            else
            {
                // Pour l'étudiant, ordre inversé avec les comboboxes avant le bouton
                registerPanel.Controls.Add(buttonPanel); // Bouton d'inscription
                registerPanel.Controls.Add(etudiantPanel); // Comboboxes (niveau éducatif et spécialité)
                registerPanel.Controls.Add(birthdatePanel); // Date de naissance
                registerPanel.Controls.Add(confirmPasswordPanel); // Confirmation mot de passe
                registerPanel.Controls.Add(passwordPanel); // Mot de passe
                registerPanel.Controls.Add(emailPanel); // Email
                registerPanel.Controls.Add(namePanel); // Nom
                registerPanel.Controls.Add(lblTitle); // Titre
            }
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

        private async void BtnRegister_Click(object sender, EventArgs e)
        {
            // Validation des champs
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text) ||
                string.IsNullOrWhiteSpace(txtConfirmPassword.Text))
            {
                MessageBox.Show("Veuillez remplir tous les champs.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validation du mot de passe
            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("Les mots de passe ne correspondent pas.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validation de l'email
            if (!IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show("Veuillez entrer une adresse email valide.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Création de l'utilisateur
            try
            {
                var optionsBuilder = new DbContextOptionsBuilder<LibraryContext>();
                optionsBuilder.UseSqlServer(@"Server=localhost;Database=BibliothequeIHEC;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true")
                    .UseLazyLoadingProxies();

                using (var context = new LibraryContext(optionsBuilder.Options))
                {
                    // Vérifier si l'email existe déjà
                    if (await context.Members.AnyAsync(u => u.Email == txtEmail.Text))
                    {
                        MessageBox.Show("Cette adresse email est déjà utilisée.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Vérifier que les boutons radio sont correctement initialisés
                    if (rdoBibliothecaire == null)
                    {
                        MessageBox.Show("Erreur d'initialisation du formulaire. Veuillez réessayer.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Vérifier que le niveau éducatif est sélectionné pour les étudiants
                    if (!rdoBibliothecaire.Checked && (cmbNiveauEducatif == null || string.IsNullOrEmpty(cmbNiveauEducatif.Text)))
                    {
                        MessageBox.Show("Veuillez sélectionner votre niveau éducatif.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Créer l'utilisateur
                    Member newUser = new Member
                    {
                        Name = txtName.Text,
                        Email = txtEmail.Text,
                        Password = txtPassword.Text, // Dans un vrai projet, il faudrait hasher le mot de passe
                        Birthdate = dtpBirthdate.Value,
                        IsAdmin = rdoBibliothecaire != null && rdoBibliothecaire.Checked,
                        NiveauEducatif = (!rdoBibliothecaire.Checked && cmbNiveauEducatif != null) ? cmbNiveauEducatif.Text : null,
                        DateInscription = DateTime.Now
                    };

                    context.Members.Add(newUser);
                    await context.SaveChangesAsync();

                    MessageBox.Show("Inscription réussie ! Vous pouvez maintenant vous connecter.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    // Rediriger vers la page de connexion
                    this.Hide();
                    new LoginForm().Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'inscription : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Méthode pour valider l'email
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void LnkLogin_Click(object sender, EventArgs e)
        {
            // Rediriger vers la page de connexion
            this.Hide();
            new LoginForm().Show();
        }

        // Cette méthode est déjà définie dans le fichier Designer

        // Ajout d'une méthode pour fermer proprement l'application
        private void CleanupAndExit()
        {
            // Libérer les ressources de la base de données
            try
            {
                var optionsBuilder = new DbContextOptionsBuilder<LibraryContext>();
                optionsBuilder.UseSqlServer(@"Server=localhost;Database=BibliothequeIHEC;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true");
                
                using (var context = new LibraryContext(optionsBuilder.Options))
                {
                    context.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la fermeture de la connexion à la base de données : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Libérer les ressources du formulaire
            this.Dispose();
            
            // Fermer l'application
            Application.Exit();
        }

        // Modifier la méthode FormClosing pour appeler CleanupAndExit
        private void RegisterForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Si l'application est en train de se fermer, libérer les ressources
            if (e.CloseReason == CloseReason.UserClosing || e.CloseReason == CloseReason.ApplicationExitCall)
            {
                // Libérer les ressources si nécessaire
                if (logoBox != null && logoBox.Image != null)
                {
                    logoBox.Image.Dispose();
                    logoBox.Image = null;
                }
            }
        }
    }
}
