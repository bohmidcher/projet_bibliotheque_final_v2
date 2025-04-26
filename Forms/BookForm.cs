using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using projet_bibliotheque.Data;
using projet_bibliotheque.Models;
using Microsoft.Data.SqlClient;

namespace projet_bibliotheque.Forms
{
    public partial class BookForm : Form
    {
        private readonly Color PrimaryColor = Color.FromArgb(31, 43, 71);
        private readonly Color SecondaryColor = Color.FromArgb(241, 134, 48);
        private readonly Color LightColor = Color.FromArgb(250, 250, 250);
        private readonly Color AccentColor = Color.FromArgb(255, 87, 34);

        private TextBox txtTitle;
        private TextBox txtISBN;
        private ComboBox cmbGenre;
        private ComboBox cmbAuthors;
        private Label lblSelectedAuthor;
        private DateTimePicker dtpPublicationDate;
        private ElegantButton btnSave;
        private ElegantButton btnCancel;
        private ElegantButton btnAddAuthor;

        private readonly Book? BookToEdit;
        private Author? selectedAuthor;
        private readonly LibraryContext _context;

        public BookForm(LibraryContext context, Book? book = null)
        {
            _context = context;
            BookToEdit = book;
            InitializeComponent();
            
            // Load authors immediately without showing any error
            LoadAuthorsSync();
            
            cmbGenre.SelectedIndex = 0;
            dtpPublicationDate.Value = DateTime.Today;

            if (book != null)
            {
                LoadBookData();
            }
        }

        private void LoadAuthorsSync()
        {
            try
            {
                using var connection = new Microsoft.Data.SqlClient.SqlConnection(_context.Database.GetConnectionString());
                connection.Open();

                using var command = new Microsoft.Data.SqlClient.SqlCommand(
                    "SELECT Id, Name, Nationality, Birthdate FROM dbo.Authors", connection);

                using var reader = command.ExecuteReader();
                
                cmbAuthors.Items.Clear();
                while (reader.Read())
                {
                    var author = new Author
                    {
                        Id = reader.GetInt32(0),
                        Name = !reader.IsDBNull(1) ? reader.GetString(1) : "",
                        Nationality = !reader.IsDBNull(2) ? reader.GetString(2) : "",
                        Birthdate = !reader.IsDBNull(3) ? reader.GetDateTime(3) : DateTime.Today
                    };
                    cmbAuthors.Items.Add(author);
                }

                if (BookToEdit?.AuthorId > 0)
                {
                    for (int i = 0; i < cmbAuthors.Items.Count; i++)
                    {
                        var author = (Author)cmbAuthors.Items[i];
                        if (author.Id == BookToEdit.AuthorId)
                        {
                            cmbAuthors.SelectedIndex = i;
                            selectedAuthor = author;
                            UpdateSelectedAuthorLabel();
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading authors: {ex.Message}");
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // BookForm
            // 
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Name = "BookForm";
            this.Text = "Book Management";
            this.ResumeLayout(false);
        }

        private Panel CreateLabeledField(string labelText, out TextBox textBox)
        {
            Panel panel = new Panel { Height = 70, Dock = DockStyle.Top }; 

            Label label = new Label
            {
                Text = labelText,
                Font = new Font("Poppins", 11, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 25,
                ForeColor = PrimaryColor
            };

            textBox = new TextBox
            {
                Font = new Font("Poppins", 11),
                Dock = DockStyle.Top,
                Height = 35,
                BorderStyle = BorderStyle.FixedSingle
            };

            panel.Controls.Add(textBox);
            panel.Controls.Add(label);

            return panel;
        }

        private Panel CreateLabeledCombo(string labelText, out ComboBox comboBox, string[] items)
        {
            Panel panel = new Panel { Height = 70, Dock = DockStyle.Top }; 

            Label label = new Label
            {
                Text = labelText,
                Font = new Font("Poppins", 11, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 25,
                ForeColor = PrimaryColor
            };

            comboBox = new ComboBox
            {
                Font = new Font("Poppins", 11),
                Dock = DockStyle.Top,
                Height = 35,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            comboBox.Items.AddRange(items);

            panel.Controls.Add(comboBox);
            panel.Controls.Add(label);

            return panel;
        }

        private Panel CreateLabeledDate(string labelText, out DateTimePicker dateTimePicker)
        {
            Panel panel = new Panel { Height = 70, Dock = DockStyle.Top }; 

            Label label = new Label
            {
                Text = labelText,
                Font = new Font("Poppins", 11, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 25,
                ForeColor = PrimaryColor
            };

            dateTimePicker = new DateTimePicker
            {
                Font = new Font("Poppins", 11),
                Dock = DockStyle.Top,
                Height = 35,
                Format = DateTimePickerFormat.Short
            };

            panel.Controls.Add(dateTimePicker);
            panel.Controls.Add(label);

            return panel;
        }

        private Panel CreateAuthorSelectionPanel()
        {
            Panel panel = new Panel { Height = 200, Dock = DockStyle.Top }; 

            Label lblAuthor = new Label
            {
                Text = "Sélectionner un auteur",
                Font = new Font("Poppins", 11, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 25,
                ForeColor = PrimaryColor
            };

            lblSelectedAuthor = new Label
            {
                Text = "Aucun auteur sélectionné",
                Font = new Font("Poppins", 10),
                ForeColor = Color.Gray,
                Dock = DockStyle.Top,
                Height = 25,
                Padding = new Padding(0, 5, 0, 5)
            };

            cmbAuthors = new ComboBox
            {
                Font = new Font("Poppins", 11),
                Dock = DockStyle.Top,
                Height = 35,
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat
            };
            cmbAuthors.SelectedIndexChanged += CmbAuthors_SelectedIndexChanged;

            btnAddAuthor = new ElegantButton
            {
                Text = "+ Ajouter un nouvel auteur",
                BackColor = SecondaryColor,
                ForeColor = LightColor,
                Dock = DockStyle.Top,
                Height = 35,
                Margin = new Padding(0, 10, 0, 0),
                Font = new Font("Poppins", 10, FontStyle.Bold)
            };
            btnAddAuthor.Click += BtnAddAuthor_Click;

            panel.Controls.Add(btnAddAuthor);
            panel.Controls.Add(cmbAuthors);
            panel.Controls.Add(lblSelectedAuthor);
            panel.Controls.Add(lblAuthor);

            return panel;
        }

        private void CmbAuthors_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbAuthors.SelectedItem != null)
            {
                selectedAuthor = (Author)cmbAuthors.SelectedItem;
                UpdateSelectedAuthorLabel();
            }
        }

        private void UpdateSelectedAuthorLabel()
        {
            if (selectedAuthor != null)
            {
                lblSelectedAuthor.Text = $"Auteur sélectionné: {selectedAuthor.Name} ({selectedAuthor.Nationality})";
                lblSelectedAuthor.ForeColor = PrimaryColor;
            }
            else
            {
                lblSelectedAuthor.Text = "Aucun auteur sélectionné";
                lblSelectedAuthor.ForeColor = Color.Gray;
            }
        }

        private void LoadBookData()
        {
            if (BookToEdit != null)
            {
                txtTitle.Text = BookToEdit.Title;
                txtISBN.Text = BookToEdit.ISBN;
                cmbGenre.SelectedItem = BookToEdit.Genre;
                dtpPublicationDate.Value = BookToEdit.PublicationDate;
            }
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Le titre est obligatoire", "Erreur de validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtISBN.Text))
            {
                MessageBox.Show("L'ISBN est obligatoire", "Erreur de validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cmbAuthors.SelectedItem == null)
            {
                MessageBox.Show("Veuillez sélectionner un auteur", "Erreur de validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cmbGenre.SelectedItem == null)
            {
                MessageBox.Show("Le genre est obligatoire", "Erreur de validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private async void BtnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateForm()) return;

            try
            {
                var book = BookToEdit ?? new Book();
                book.Title = txtTitle.Text.Trim();
                book.ISBN = txtISBN.Text.Trim();
                book.Genre = cmbGenre.SelectedItem.ToString();
                book.PublicationDate = dtpPublicationDate.Value;
                book.AuthorId = ((Author)cmbAuthors.SelectedItem).Id;

                if (BookToEdit == null)
                {
                    _context.Books.Add(book);
                }
                else
                {
                    _context.Books.Update(book);
                }

                await _context.SaveChangesAsync();
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'enregistrement: {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private async void BtnAddAuthor_Click(object sender, EventArgs e)
        {
            using (var addAuthorForm = new AddAuthorForm())
            {
                if (addAuthorForm.ShowDialog() == DialogResult.OK)
                {
                    LoadAuthorsSync();
                    
                    if (addAuthorForm.NewAuthor != null)
                    {
                        for (int i = 0; i < cmbAuthors.Items.Count; i++)
                        {
                            var author = (Author)cmbAuthors.Items[i];
                            if (author.Id == addAuthorForm.NewAuthor.Id)
                            {
                                cmbAuthors.SelectedIndex = i;
                                selectedAuthor = author;
                                UpdateSelectedAuthorLabel();
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}
