using Microsoft.EntityFrameworkCore;
using projet_bibliotheque.Data;
using projet_bibliotheque.Models;
using System;
using System.Windows.Forms;

namespace projet_bibliotheque.Forms
{
    public partial class LoanForm : Form
    {
        private readonly LibraryContext _context;
        private readonly Loan _loan;
        private ComboBox cmbBooks;
        private ComboBox cmbMembers;
        private DateTimePicker dtpLoanDate;
        private DateTimePicker dtpReturnDate;
        private ElegantButton btnSave;
        private ElegantButton btnCancel;
        private readonly Color PrimaryColor = Color.FromArgb(31, 43, 71);
        private readonly Color SecondaryColor = Color.FromArgb(241, 134, 48);

        public LoanForm(LibraryContext context, Loan? loan = null)
        {
            InitializeComponent();
            _context = context;
            _loan = loan ?? new Loan();
            SetupForm();
            LoadData();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // LoanForm
            // 
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Name = "LoanForm";
            this.Text = "Loan Management";
            this.ResumeLayout(false);
        }

        private void SetupForm()
        {
            this.Text = "Nouvel emprunt";
            this.Size = new Size(400, 350);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Livre
            var lblBook = new Label
            {
                Text = "Livre :",
                Location = new Point(20, 20),
                AutoSize = true,
                Font = new Font("Poppins", 10)
            };

            cmbBooks = new ComboBox
            {
                Location = new Point(20, 45),
                Size = new Size(340, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Poppins", 10)
            };

            // Membre
            var lblMember = new Label
            {
                Text = "Membre :",
                Location = new Point(20, 80),
                AutoSize = true,
                Font = new Font("Poppins", 10)
            };

            cmbMembers = new ComboBox
            {
                Location = new Point(20, 105),
                Size = new Size(340, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Poppins", 10)
            };

            // Date d'emprunt
            var lblLoanDate = new Label
            {
                Text = "Date d'emprunt :",
                Location = new Point(20, 140),
                AutoSize = true,
                Font = new Font("Poppins", 10)
            };

            dtpLoanDate = new DateTimePicker
            {
                Location = new Point(20, 165),
                Size = new Size(340, 30),
                Format = DateTimePickerFormat.Short,
                Font = new Font("Poppins", 10),
                Value = DateTime.Today
            };

            // Date de retour
            var lblReturnDate = new Label
            {
                Text = "Date de retour :",
                Location = new Point(20, 200),
                AutoSize = true,
                Font = new Font("Poppins", 10)
            };

            dtpReturnDate = new DateTimePicker
            {
                Location = new Point(20, 225),
                Size = new Size(340, 30),
                Format = DateTimePickerFormat.Short,
                Font = new Font("Poppins", 10),
                Value = DateTime.Today.AddDays(14)
            };

            // Boutons
            btnSave = new ElegantButton
            {
                Text = "Enregistrer",
                Location = new Point(180, 270),
                Size = new Size(120, 35),
                BackColor = SecondaryColor,
                ForeColor = Color.White,
                Font = new Font("Poppins", 10),
                CornerRadius = 20
            };
            btnSave.Click += BtnSave_Click;

            btnCancel = new ElegantButton
            {
                Text = "Annuler",
                Location = new Point(310, 270),
                Size = new Size(120, 35),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                Font = new Font("Poppins", 10),
                CornerRadius = 20
            };
            btnCancel.Click += (s, e) => this.Close();

            this.Controls.AddRange(new Control[] {
                lblBook, cmbBooks,
                lblMember, cmbMembers,
                lblLoanDate, dtpLoanDate,
                lblReturnDate, dtpReturnDate,
                btnSave, btnCancel
            });
        }

        private async void LoadData()
        {
            try
            {
                // Charger les livres disponibles
                var books = await _context.Books
                    .Where(b => !_context.Loans.Any(l => l.BookId == b.Id))
                    .OrderBy(b => b.Title)
                    .ToListAsync();

                cmbBooks.DisplayMember = "Title";
                cmbBooks.ValueMember = "Id";
                cmbBooks.DataSource = books;

                // Charger les membres
                var members = await _context.Members
                    .OrderBy(m => m.Name)
                    .ToListAsync();

                cmbMembers.DisplayMember = "Name";
                cmbMembers.ValueMember = "Id";
                cmbMembers.DataSource = members;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des données : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private async void BtnSave_Click(object sender, EventArgs e)
        {
            if (cmbBooks.SelectedItem == null || cmbMembers.SelectedItem == null)
            {
                MessageBox.Show("Veuillez sélectionner un livre et un membre.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (dtpReturnDate.Value <= dtpLoanDate.Value)
            {
                MessageBox.Show("La date de retour doit être postérieure à la date d'emprunt.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                var loan = new Loan
                {
                    BookId = (int)cmbBooks.SelectedValue,
                    MemberId = (int)cmbMembers.SelectedValue,
                    LoanDate = dtpLoanDate.Value,
                    ReturnDate = dtpReturnDate.Value
                };

                _context.Loans.Add(loan);
                await _context.SaveChangesAsync();

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'enregistrement : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
