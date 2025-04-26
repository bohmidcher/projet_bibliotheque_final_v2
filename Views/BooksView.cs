using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using projet_bibliotheque.Data;
using projet_bibliotheque.Models;
using projet_bibliotheque.Forms;

namespace projet_bibliotheque.Views
{
    public class BooksView : UserControl, IDisposable
    {
        private readonly Color PrimaryColor = Color.FromArgb(31, 43, 71);
        private readonly Color SecondaryColor = Color.FromArgb(241, 134, 48);
        private readonly Color LightColor = Color.FromArgb(250, 250, 250);
        private readonly Color AccentColor = Color.FromArgb(255, 87, 34);
        private readonly LibraryContext _context;

        private DataGridView gridBooks;
        private TextBox txtSearch;
        private ComboBox cmbGenre;
        private ElegantButton btnAdd;
        private ElegantButton btnEdit;
        private ElegantButton btnDelete;
        private Panel topPanel;
        private Panel gridPanel;
        private bool disposedValue;

        public BooksView()
        {
            _context = new LibraryContext(new DbContextOptionsBuilder<LibraryContext>().Options);
            InitializeComponent();
            LoadBooks();
        }

        private void InitializeComponent()
        {
            topPanel = new Panel();
            txtSearch = new TextBox();
            cmbGenre = new ComboBox();
            btnAdd = new ElegantButton();
            btnEdit = new ElegantButton();
            btnDelete = new ElegantButton();
            gridBooks = new DataGridView();
            gridPanel = new Panel();
            topPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridBooks).BeginInit();
            gridPanel.SuspendLayout();
            SuspendLayout();

            // topPanel
            topPanel.Controls.Add(txtSearch);
            topPanel.Controls.Add(cmbGenre);
            topPanel.Controls.Add(btnAdd);
            topPanel.Controls.Add(btnEdit);
            topPanel.Controls.Add(btnDelete);
            topPanel.Location = new Point(20, 20);
            topPanel.Name = "topPanel";
            topPanel.Size = new Size(950, 50);
            topPanel.TabIndex = 0;

            // txtSearch
            txtSearch.Location = new Point(10, 10);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "Rechercher...";
            txtSearch.Size = new Size(200, 27);
            txtSearch.TabIndex = 0;
            txtSearch.TextChanged += TxtSearch_TextChanged;

            // cmbGenre
            cmbGenre.Items.AddRange(new object[] { "Tous les genres", "Roman", "Science", "Histoire", "Économie", "Management", "Théâtre" });
            cmbGenre.Location = new Point(220, 10);
            cmbGenre.Name = "cmbGenre";
            cmbGenre.Size = new Size(150, 28);
            txtSearch.TabIndex = 1;
            cmbGenre.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbGenre.SelectedIndex = 0;
            cmbGenre.SelectedIndexChanged += CmbGenre_SelectedIndexChanged;

            // btnAdd
            btnAdd.Text = "Ajouter";
            btnAdd.Location = new Point(400, 10);
            btnAdd.Size = new Size(100, 30);
            btnAdd.Click += BtnAdd_Click;

            // btnEdit
            btnEdit.Text = "Modifier";
            btnEdit.Location = new Point(510, 10);
            btnEdit.Size = new Size(100, 30);
            btnEdit.Click += BtnEdit_Click;

            // btnDelete
            btnDelete.Text = "Supprimer";
            btnDelete.Location = new Point(620, 10);
            btnDelete.Size = new Size(100, 30);
            btnDelete.Click += BtnDelete_Click;

            // gridBooks
            gridBooks.ColumnHeadersHeight = 29;
            gridBooks.Location = new Point(0, 0);
            gridBooks.Name = "gridBooks";
            gridBooks.RowHeadersWidth = 51;
            gridBooks.Size = new Size(950, 400);
            gridBooks.ReadOnly = true;
            gridBooks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridBooks.MultiSelect = false;
            gridBooks.CellDoubleClick += GridBooks_CellDoubleClick;

            // gridPanel
            gridPanel.Controls.Add(gridBooks);
            gridPanel.Location = new Point(20, 80);
            gridPanel.Name = "gridPanel";
            gridPanel.Size = new Size(950, 420);
            gridPanel.TabIndex = 1;

            // BooksView
            BackColor = Color.White;
            Controls.Add(topPanel);
            Controls.Add(gridPanel);
            Name = "BooksView";
            Padding = new Padding(20);
            Size = new Size(993, 531);
            topPanel.ResumeLayout(false);
            topPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)gridBooks).EndInit();
            gridPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        private async void LoadBooks(string search = "", string genre = "Tous les genres")
        {
            try
            {
                var booksQuery = _context.Books.Include(b => b.Author).AsQueryable();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    string lowerSearch = search.ToLower();
                    booksQuery = booksQuery.Where(b =>
                        b.Title.ToLower().Contains(lowerSearch) ||
                        b.Author.Name.ToLower().Contains(lowerSearch) ||
                        b.Genre.ToLower().Contains(lowerSearch));
                }

                if (genre != "Tous les genres")
                {
                    booksQuery = booksQuery.Where(b => b.Genre == genre);
                }

                var books = await booksQuery
                    .Select(b => new
                    {
                        ID = b.Id,
                        Titre = b.Title,
                        Auteur = b.Author.Name,
                        Genre = b.Genre,
                        ISBN = b.ISBN,
                        DatePublication = b.PublicationDate.ToShortDateString()
                    })
                    .ToListAsync();

                gridBooks.DataSource = books;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des livres: {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadBooks(txtSearch.Text, cmbGenre.SelectedItem?.ToString());
        }

        private void CmbGenre_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadBooks(txtSearch.Text, cmbGenre.SelectedItem?.ToString());
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            using (var form = new BookForm(_context))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadBooks(txtSearch.Text, cmbGenre.SelectedItem?.ToString());
                }
            }
        }

        private async void BtnEdit_Click(object sender, EventArgs e)
        {
            if (gridBooks.SelectedRows.Count > 0)
            {
                int bookId = (int)gridBooks.SelectedRows[0].Cells["ID"].Value;
                var book = await _context.Books.Include(b => b.Author).FirstOrDefaultAsync(b => b.Id == bookId);
                if (book != null)
                {
                    using (var form = new BookForm(_context, book))
                    {
                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            LoadBooks(txtSearch.Text, cmbGenre.SelectedItem?.ToString());
                        }
                    }
                }
            }
        }

        private async void BtnDelete_Click(object sender, EventArgs e)
        {
            if (gridBooks.SelectedRows.Count > 0)
            {
                int bookId = (int)gridBooks.SelectedRows[0].Cells["ID"].Value;
                if (MessageBox.Show("Êtes-vous sûr de vouloir supprimer ce livre ?", "Confirmation",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    var book = await _context.Books.FindAsync(bookId);
                    if (book != null)
                    {
                        _context.Books.Remove(book);
                        await _context.SaveChangesAsync();
                        LoadBooks(txtSearch.Text, cmbGenre.SelectedItem?.ToString());
                    }
                }
            }
        }

        private void GridBooks_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int bookId = (int)gridBooks.Rows[e.RowIndex].Cells["ID"].Value;
                // TODO: Implémenter un formulaire de détails si souhaité
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                disposedValue = true;
            }
            base.Dispose(disposing);
        }
    }
}
