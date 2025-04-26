using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.IO;
using projet_bibliotheque.Controls;


namespace projet_bibliotheque
{
    public partial class HomeForm : Form
    {
        private readonly Color TextColor = Color.White;
        private readonly Color ButtonColor = Color.White;
        private readonly Color ButtonHoverColor = Color.FromArgb(240, 240, 240);
        
        private readonly string backgroundImagePath = "Assets\\img\\IHEC.png";
        private readonly string logoPath = "Assets\\img\\logo.svg";

        private Panel mainPanel;
        private PictureBox logoBox;
        private Label title1, title2, title3, description;
        private Button btnStart, btnQuit;

        public HomeForm()
        {
            InitializeComponent();
            SetupForm();
            CreateUI();
            this.Paint += new PaintEventHandler(HomeForm_Paint);
            this.Resize += new EventHandler(HomeForm_Resize);
        }

        private void SetupForm()
        {
            this.Text = "BiblioHub";
            this.MinimumSize = new Size(1200, 800);
            this.Size = new Size(1200, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.DoubleBuffered = true;
        }

        private void HomeForm_Paint(object sender, PaintEventArgs e)
        {
            using (LinearGradientBrush brush = new LinearGradientBrush(
                this.ClientRectangle,
                Color.FromArgb(255, 0, 0, 20),    // Bleu très foncé
                Color.FromArgb(255, 0, 0, 70),    // Bleu marine
                45f))  // Angle du dégradé
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }

            // Ajout d'une légère transparence noire par-dessus pour l'effet de profondeur
            using (SolidBrush overlayBrush = new SolidBrush(Color.FromArgb(100, 0, 0, 0)))
            {
                e.Graphics.FillRectangle(overlayBrush, this.ClientRectangle);
            }
        }

        private void HomeForm_Resize(object sender, EventArgs e)
        {
            this.Invalidate(); // Force le redessin du formulaire
            UpdateControlsLayout();
        }

        private void UpdateControlsLayout()
        {
            if (mainPanel == null) return;

            // Calcul des marges responsives
            int leftMargin = (int)(this.ClientSize.Width * 0.1); // 10% de la largeur
            int topMargin = (int)(this.ClientSize.Height * 0.05); // 5% de la hauteur

            // Mise à jour de la position du logo
            if (logoBox != null)
            {
                logoBox.Location = new Point(leftMargin, topMargin);
                logoBox.Size = new Size((int)(this.ClientSize.Width * 0.08), (int)(this.ClientSize.Width * 0.08));
            }

            // Calcul de la taille de police responsive
            float titleFontSize = Math.Min(64, this.ClientSize.Width / 20);
            float bigTitleFontSize = Math.Min(80, this.ClientSize.Width / 16);
            float descriptionFontSize = Math.Min(16, this.ClientSize.Width / 75);

            // Mise à jour des titres
            if (title1 != null)
            {
                title1.Font = new Font("Poppins", titleFontSize, FontStyle.Regular);
                title1.Location = new Point(leftMargin, (int)(this.ClientSize.Height * 0.25));
            }

            if (title2 != null)
            {
                title2.Font = new Font("Poppins", titleFontSize, FontStyle.Regular);
                title2.Location = new Point(leftMargin, title1.Bottom - (int)(titleFontSize * 0.3));
            }

            if (title3 != null)
            {
                title3.Font = new Font("Poppins", bigTitleFontSize, FontStyle.Bold);
                title3.Location = new Point(leftMargin, title2.Bottom - (int)(titleFontSize * 0.3));
            }

            if (description != null)
            {
                description.Font = new Font("Poppins", descriptionFontSize, FontStyle.Regular);
                description.MaximumSize = new Size((int)(this.ClientSize.Width * 0.8), 0);
                description.Location = new Point(leftMargin, title3.Bottom + (int)(this.ClientSize.Height * 0.02));
            }

            // Mise à jour des boutons
            int buttonWidth = (int)(this.ClientSize.Width * 0.12);
            int buttonHeight = (int)(this.ClientSize.Height * 0.06);
            float buttonFontSize = Math.Min(14, this.ClientSize.Width / 85);

            if (btnStart != null)
            {
                btnStart.Size = new Size(buttonWidth, buttonHeight);
                btnStart.Location = new Point(leftMargin, description.Bottom + (int)(this.ClientSize.Height * 0.04));
                btnStart.Font = new Font("Poppins", buttonFontSize, FontStyle.Regular);
                btnStart.Region = new Region(RoundRectangle.Create(0, 0, buttonWidth, buttonHeight, 22f));
            }

            if (btnQuit != null)
            {
                btnQuit.Size = new Size(buttonWidth, buttonHeight);
                btnQuit.Location = new Point(btnStart.Right + (int)(this.ClientSize.Width * 0.02), btnStart.Top);
                btnQuit.Font = new Font("Poppins", buttonFontSize, FontStyle.Regular);
                btnQuit.Region = new Region(RoundRectangle.Create(0, 0, buttonWidth, buttonHeight, 22f));
            }
        }

        private void CreateUI()
        {
            mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent
            };
            this.Controls.Add(mainPanel);
            
            // Logo
            try
            {
                string fullLogoPath = Path.Combine(Application.StartupPath, logoPath);
                if (File.Exists(fullLogoPath))
                {
                    logoBox = new PictureBox
                    {
                        SizeMode = PictureBoxSizeMode.Zoom,
                        BackColor = Color.Transparent,
                        Image = Image.FromFile(fullLogoPath)
                    };
                    mainPanel.Controls.Add(logoBox);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading logo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Titles
            title1 = new Label
            {
                Text = "L'excellence",
                AutoSize = true,
                BackColor = Color.Transparent,
                ForeColor = TextColor
            };
            mainPanel.Controls.Add(title1);

            title2 = new Label
            {
                Text = "n'est pas",
                AutoSize = true,
                BackColor = Color.Transparent,
                ForeColor = TextColor
            };
            mainPanel.Controls.Add(title2);

            title3 = new Label
            {
                Text = "un choix",
                AutoSize = true,
                BackColor = Color.Transparent,
                ForeColor = TextColor
            };
            mainPanel.Controls.Add(title3);

            description = new Label
            {
                Text = "Connectez vous pour avoir accès à des milliers des livres de la bibliothèque de l'IHEC Carthage!",
                AutoSize = true,
                BackColor = Color.Transparent,
                ForeColor = TextColor
            };
            mainPanel.Controls.Add(description);

            // Buttons
            btnStart = CreateRoundedButton("Démarrer", Point.Empty);
            btnStart.Click += (s, e) =>
            {
                LoginForm loginForm = new LoginForm();
                this.Hide();  // Cache la fenêtre actuelle
                loginForm.FormClosed += (s, args) => this.Close();  // Ferme HomeForm quand LoginForm est fermé
                loginForm.Show();
            };
            mainPanel.Controls.Add(btnStart);

            btnQuit = CreateRoundedButton("Quitter", Point.Empty);
            mainPanel.Controls.Add(btnQuit);

            btnQuit.Click += (s, e) => Application.Exit();

            // Initial layout update
            UpdateControlsLayout();
        }

        private Button CreateRoundedButton(string text, Point location)
        {
            Button btn = new Button
            {
                Text = text,
                ForeColor = Color.FromArgb(10, 20, 50),
                BackColor = ButtonColor,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            
            btn.FlatAppearance.BorderSize = 0;
            btn.MouseEnter += (s, e) => btn.BackColor = ButtonHoverColor;
            btn.MouseLeave += (s, e) => btn.BackColor = ButtonColor;
            
            return btn;
        }
    }
}