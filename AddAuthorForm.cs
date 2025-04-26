using projet_bibliotheque.Models;
using projet_bibliotheque;
using projet_bibliotheque.Data;
using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using Microsoft.EntityFrameworkCore;
using projet_bibliotheque.Controls;

public class AddAuthorForm : Form
{
    private TextBox txtNom;
    private TextBox txtPrenom;
    private DateTimePicker dtpDateNaissance;
    private ComboBox cmbNiveauEducatif;
    private ComboBox cmbSpecialite;
    private TextBox txtEmail;
    private TextBox txtPassword;
    private TextBox txtConfirmPassword;
    private CheckBox chkAcceptRules;
    private Button btnInscription;
    private Panel mainPanel;
    private RadioButton rdoEtudiant;
    private RadioButton rdoBibliothecaire;
    private Button btnSuivant;
    private Panel rolePanel;
    private Panel inscriptionPanel;

    public Author? NewAuthor { get; private set; }

    public AddAuthorForm()
    {
        InitializeComponent();
        this.Paint += new PaintEventHandler(Form_Paint);
        this.Resize += new EventHandler(Form_Resize);
    }

    private void Form_Paint(object sender, PaintEventArgs e)
    {
        using (LinearGradientBrush brush = new LinearGradientBrush(
            this.ClientRectangle,
            Color.FromArgb(0, 0, 20),    // Bleu très foncé
            Color.FromArgb(0, 0, 70),    // Bleu marine
            45f))
        {
            e.Graphics.FillRectangle(brush, this.ClientRectangle);
        }

        using (SolidBrush overlayBrush = new SolidBrush(Color.FromArgb(100, 0, 0, 0)))
        {
            e.Graphics.FillRectangle(overlayBrush, this.ClientRectangle);
        }
    }

    private void Form_Resize(object sender, EventArgs e)
    {
        this.Invalidate();
        CenterPanels();
    }

    private void CenterPanels()
    {
        if (rolePanel != null)
        {
            rolePanel.Location = new Point(
                (this.ClientSize.Width - rolePanel.Width) / 2,
                (this.ClientSize.Height - rolePanel.Height) / 2
            );
        }

        if (inscriptionPanel != null)
        {
            inscriptionPanel.Location = new Point(
                (this.ClientSize.Width - inscriptionPanel.Width) / 2,
                (this.ClientSize.Height - inscriptionPanel.Height) / 2
            );
        }
    }

    private void InitializeComponent()
    {
        this.Text = "BiblioHub - Inscription";
        this.MinimumSize = new Size(1200, 800);
        this.Size = new Size(1200, 800);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.FormBorderStyle = FormBorderStyle.Sizable;
        this.DoubleBuffered = true;

        // Panel de sélection du rôle
        rolePanel = new Panel
        {
            Width = 400,
            Height = 300,
            BackColor = Color.White
        };

        rolePanel.Paint += (s, e) =>
        {
            using (GraphicsPath path = RoundRectangle.Create(0, 0, rolePanel.Width, rolePanel.Height, 20f))
            {
                rolePanel.Region = new Region(path);
            }
        };

        Label lblRole = new Label
        {
            Text = "Vous êtes:",
            Font = new Font("Poppins", 24, FontStyle.Bold),
            ForeColor = Color.FromArgb(0, 0, 20),
            TextAlign = ContentAlignment.MiddleCenter,
            Dock = DockStyle.Top,
            Height = 80
        };

        rdoEtudiant = new RadioButton
        {
            Text = "Étudiant ou professeur",
            Font = new Font("Poppins", 12),
            Location = new Point(50, 100),
            AutoSize = true
        };

        rdoBibliothecaire = new RadioButton
        {
            Text = "Bibliothécaire (Admin)",
            Font = new Font("Poppins", 12),
            Location = new Point(50, 140),
            AutoSize = true
        };

        btnSuivant = new Button
        {
            Text = "Suivant",
            Font = new Font("Poppins", 12),
            Size = new Size(150, 40),
            Location = new Point(125, 200),
            BackColor = Color.FromArgb(0, 0, 20),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        btnSuivant.FlatAppearance.BorderSize = 0;
        btnSuivant.Region = new Region(RoundRectangle.Create(0, 0, btnSuivant.Width, btnSuivant.Height, 20f));

        btnSuivant.Click += (s, e) =>
        {
            if (!rdoEtudiant.Checked && !rdoBibliothecaire.Checked)
            {
                MessageBox.Show("Veuillez sélectionner votre rôle.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            rolePanel.Visible = false;
            inscriptionPanel.Visible = true;
        };

        rolePanel.Controls.AddRange(new Control[] { lblRole, rdoEtudiant, rdoBibliothecaire, btnSuivant });

        // Panel d'inscription
        inscriptionPanel = new Panel
        {
            Width = 500,
            Height = 600,
            BackColor = Color.White,
            Visible = false
        };

        inscriptionPanel.Paint += (s, e) =>
        {
            using (GraphicsPath path = RoundRectangle.Create(0, 0, inscriptionPanel.Width, inscriptionPanel.Height, 20f))
            {
                inscriptionPanel.Region = new Region(path);
            }
        };

        Label lblTitle = new Label
        {
            Text = "S'inscrire",
            Font = new Font("Poppins", 24, FontStyle.Bold),
            ForeColor = Color.FromArgb(0, 0, 20),
            TextAlign = ContentAlignment.MiddleCenter,
            Dock = DockStyle.Top,
            Height = 60
        };

        TableLayoutPanel layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 8,
            Padding = new Padding(20),
            BackColor = Color.White
        };

        // Ajout des champs
        layout.Controls.Add(CreateField("Nom:", out txtNom, 0, 0));
        layout.Controls.Add(CreateField("Prénom:", out txtPrenom, 0, 1));
        layout.Controls.Add(CreateDateField("Date de naissance:", out dtpDateNaissance, 1, 0));
        layout.Controls.Add(CreateComboBoxField("Niveau éducatif:", out cmbNiveauEducatif, new string[] {
            "1ère année",
            "2ème année",
            "3ème année",
            "1ère année master",
            "2ème année master"
        }, 1, 1));
        layout.Controls.Add(CreateComboBoxField("Spécialité:", out cmbSpecialite, new string[] {
            "Finances",
            "Gestion",
            "Comptabilité",
            "Marketing",
            "Management",
            "Business Intelligence"
        }, 2, 0));
        layout.Controls.Add(CreateField("Adresse E-mail:", out txtEmail, 3, 0, 2));
        layout.Controls.Add(CreateField("Mot de passe:", out txtPassword, 4, 0));
        layout.Controls.Add(CreateField("Réecrire le mot de passe:", out txtConfirmPassword, 4, 1));

        chkAcceptRules = new CheckBox
        {
            Text = "J'accepte les règles de confidentialité",
            Font = new Font("Poppins", 10),
            AutoSize = true,
            Location = new Point(30, 500)
        };

        btnInscription = new Button
        {
            Text = "S'inscrire",
            Font = new Font("Poppins", 12),
            Size = new Size(150, 40),
            Location = new Point(320, 495),
            BackColor = Color.FromArgb(0, 0, 20),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        btnInscription.FlatAppearance.BorderSize = 0;
        btnInscription.Region = new Region(RoundRectangle.Create(0, 0, btnInscription.Width, btnInscription.Height, 20f));

        btnInscription.Click += BtnInscription_Click;

        inscriptionPanel.Controls.Add(btnInscription);
        inscriptionPanel.Controls.Add(chkAcceptRules);
        inscriptionPanel.Controls.Add(layout);
        inscriptionPanel.Controls.Add(lblTitle);

        // Ajout des panels au formulaire
        this.Controls.Add(rolePanel);
        this.Controls.Add(inscriptionPanel);
        CenterPanels();
    }

    private Panel CreateField(string label, out TextBox textBox, int row, int col, int columnSpan = 1)
    {
        Panel panel = new Panel { Margin = new Padding(10), Height = 70 };
        
        Label lbl = new Label
        {
            Text = label,
            Font = new Font("Poppins", 10),
            AutoSize = true
        };

        textBox = new TextBox
        {
            Font = new Font("Poppins", 12),
            Width = columnSpan == 2 ? 440 : 200,
            Height = 30,
            Location = new Point(0, 25)
        };

        panel.Controls.Add(lbl);
        panel.Controls.Add(textBox);

        return panel;
    }

    private Panel CreateDateField(string label, out DateTimePicker dtp, int row, int col)
    {
        Panel panel = new Panel { Margin = new Padding(10), Height = 70 };
        
        Label lbl = new Label
        {
            Text = label,
            Font = new Font("Poppins", 10),
            AutoSize = true
        };

        dtp = new DateTimePicker
        {
            Font = new Font("Poppins", 12),
            Width = 200,
            Format = DateTimePickerFormat.Short,
            Location = new Point(0, 25)
        };

        panel.Controls.Add(lbl);
        panel.Controls.Add(dtp);

        return panel;
    }

    private Panel CreateComboBoxField(string label, out ComboBox comboBox, string[] items, int row, int col)
    {
        Panel panel = new Panel { Margin = new Padding(10), Height = 70 };
        
        Label lbl = new Label
        {
            Text = label,
            Font = new Font("Poppins", 10),
            AutoSize = true
        };

        comboBox = new ComboBox
        {
            Font = new Font("Poppins", 12),
            Width = 200,
            Height = 30,
            Location = new Point(0, 25),
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        comboBox.Items.AddRange(items);
        if (comboBox.Items.Count > 0)
            comboBox.SelectedIndex = 0;

        panel.Controls.Add(lbl);
        panel.Controls.Add(comboBox);

        return panel;
    }

    private void BtnInscription_Click(object sender, EventArgs e)
    {
        if (!ValidateForm())
            return;

        try
        {
            var optionsBuilder = new DbContextOptionsBuilder<LibraryContext>();
            optionsBuilder.UseSqlServer(@"Server=localhost;Database=BibliothequeIHEC;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true")
                .UseLazyLoadingProxies();

            using (var context = new LibraryContext(optionsBuilder.Options))
            {
                var member = new Member
                {
                    Name = txtNom.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Password = txtPassword.Text,
                    DateInscription = DateTime.Now,
                    Birthdate = dtpDateNaissance.Value,
                    IsAdmin = rdoBibliothecaire.Checked,
                    NiveauEducatif = rdoEtudiant.Checked ? cmbNiveauEducatif.SelectedItem.ToString() : null,
                    Specialite = rdoEtudiant.Checked ? cmbSpecialite.SelectedItem.ToString() : null
                };

                context.Members.Add(member);
                context.SaveChanges();

                MessageBox.Show("Inscription réussie !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erreur lors de l'inscription : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private bool ValidateForm()
    {
        if (string.IsNullOrWhiteSpace(txtNom.Text) ||
            string.IsNullOrWhiteSpace(txtPrenom.Text) ||
            string.IsNullOrWhiteSpace(txtEmail.Text) ||
            string.IsNullOrWhiteSpace(txtPassword.Text) ||
            string.IsNullOrWhiteSpace(txtConfirmPassword.Text))
        {
            MessageBox.Show("Veuillez remplir tous les champs obligatoires.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        if (rdoEtudiant.Checked && (cmbNiveauEducatif.SelectedIndex == -1 || cmbSpecialite.SelectedIndex == -1))
        {
            MessageBox.Show("Veuillez sélectionner votre niveau éducatif et votre spécialité.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        if (txtPassword.Text != txtConfirmPassword.Text)
        {
            MessageBox.Show("Les mots de passe ne correspondent pas.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        if (!chkAcceptRules.Checked)
        {
            MessageBox.Show("Vous devez accepter les règles de confidentialité.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        return true;
    }
}


