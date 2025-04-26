using System;
using System.Drawing;
using System.Windows.Forms;
using projet_bibliotheque.Models;
using projet_bibliotheque;

namespace projet_bibliotheque.Forms
{
    public class AddAuthorForm : Form
    {
        private readonly Color PrimaryColor = Color.FromArgb(31, 43, 71);
        private readonly Color SecondaryColor = Color.FromArgb(241, 134, 48);
        private readonly Color LightColor = Color.FromArgb(250, 250, 250);
        private readonly Color AccentColor = Color.FromArgb(255, 87, 34);

        private TextBox txtName = null!;
        private TextBox txtNationality = null!;
        private DateTimePicker dtpBirthdate = null!;
        private ElegantButton btnSave = null!;
        private ElegantButton btnCancel = null!;

        public Author? NewAuthor { get; private set; }

        public AddAuthorForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(500, 400);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.White;

            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 5,
                Padding = new Padding(30),
                BackColor = Color.White
            };

            Label lblTitle = new Label
            {
                Text = "Ajouter un auteur",
                Font = new Font("Poppins", 22, FontStyle.Bold),
                ForeColor = PrimaryColor,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 60
            };

            var namePanel = CreateLabeledField("Nom de l'auteur", out txtName);
            var nationalityPanel = CreateLabeledField("Nationalité", out txtNationality);
            var birthdatePanel = CreateLabeledDate("Date de naissance", out dtpBirthdate);

            Panel buttonPanel = new Panel
            {
                Height = 60,
                Dock = DockStyle.Top
            };

            btnSave = new ElegantButton
            {
                Text = "ENREGISTRER",
                BackColor = AccentColor,
                ForeColor = LightColor,
                Size = new Size(160, 45),
                Font = new Font("Poppins", 10, FontStyle.Bold),
                Location = new Point(60, 10)
            };
            btnSave.Click += BtnSave_Click;

            btnCancel = new ElegantButton
            {
                Text = "ANNULER",
                BackColor = Color.Gray,
                ForeColor = LightColor,
                Size = new Size(160, 45),
                Font = new Font("Poppins", 10, FontStyle.Bold),
                Location = new Point(240, 10)
            };
            btnCancel.Click += BtnCancel_Click;

            buttonPanel.Controls.Add(btnSave);
            buttonPanel.Controls.Add(btnCancel);

            layout.Controls.Add(lblTitle);
            layout.Controls.Add(namePanel);
            layout.Controls.Add(nationalityPanel);
            layout.Controls.Add(birthdatePanel);
            layout.Controls.Add(buttonPanel);

            this.Controls.Add(layout);
        }

        private Panel CreateLabeledField(string label, out TextBox textbox)
        {
            Panel panel = new Panel { Height = 80, Dock = DockStyle.Top };
            Label lbl = new Label
            {
                Text = label,
                Font = new Font("Poppins", 11),
                Dock = DockStyle.Top,
                Height = 25
            };
            textbox = new TextBox
            {
                Font = new Font("Poppins", 12),
                Dock = DockStyle.Top,
                Height = 35
            };
            panel.Controls.Add(textbox);
            panel.Controls.Add(lbl);
            return panel;
        }

        private Panel CreateLabeledDate(string label, out DateTimePicker datePicker)
        {
            Panel panel = new Panel { Height = 80, Dock = DockStyle.Top };
            Label lbl = new Label
            {
                Text = label,
                Font = new Font("Poppins", 11),
                Dock = DockStyle.Top,
                Height = 25
            };
            datePicker = new DateTimePicker
            {
                Font = new Font("Poppins", 12),
                Dock = DockStyle.Top,
                Format = DateTimePickerFormat.Short,
                Height = 35
            };
            panel.Controls.Add(datePicker);
            panel.Controls.Add(lbl);
            return panel;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateForm()) return;

            NewAuthor = new Author
            {
                Name = txtName.Text.Trim(),
                Nationality = txtNationality.Text.Trim(),
                Birthdate = dtpBirthdate.Value
            };

            DialogResult = DialogResult.OK;
            Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Le nom de l'auteur est obligatoire.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtNationality.Text))
            {
                MessageBox.Show("La nationalité est obligatoire.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (dtpBirthdate.Value > DateTime.Today)
            {
                MessageBox.Show("La date de naissance ne peut pas être dans le futur.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }
    }
}
