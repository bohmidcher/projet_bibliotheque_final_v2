using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using projet_bibliotheque.Data;
using projet_bibliotheque.Models;

namespace projet_bibliotheque.Forms
{
    public class LibraryManagementForm : Form
    {
        // Couleurs du thème
        private readonly Color PrimaryColor = Color.FromArgb(31, 43, 71);
        private readonly Color SecondaryColor = Color.FromArgb(241, 134, 48);
        private readonly Color LightColor = Color.FromArgb(250, 250, 250);
        private readonly Color BackgroundColor = Color.FromArgb(248, 249, 250);
        private readonly Color TextColor = Color.FromArgb(33, 37, 41);
        private readonly Color SuccessColor = Color.FromArgb(40, 167, 69);
        private readonly Color WarningColor = Color.FromArgb(255, 193, 7);
        private readonly Color DangerColor = Color.FromArgb(220, 53, 69);

        // Composants du formulaire
        private Panel contentPanel;
        private Panel headerPanel;
        private Label lblTitle;
        private DataGridView dgvBooks;
        private TextBox txtSearch;
        private Button btnSearch;
        private Button btnFilter;
        private Button btnAdd;

        // Contexte de la base de données
        private readonly LibraryContext _context;

        // Liste des livres
        private List<Book> books = new List<Book>();

        public LibraryManagementForm(LibraryContext context)
        {
            _context = context;
            InitializeComponent();
            LoadBooks();
            CreateSidebar();
        }

        private void InitializeComponent()
        {
            // Configuration du formulaire
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.Text = "Gestion de la Bibliothèque";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = BackgroundColor;
            
            // Créer le layout principal
            TableLayoutPanel mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                Padding = new Padding(20)
            };
            
            // Configurer les styles de ligne
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            
            // Créer le panel d'en-tête
            headerPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent
            };
            
            // Créer le titre
            lblTitle = new Label
            {
                Text = "Gestion de la Bibliothèque",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = PrimaryColor,
                AutoSize = true,
                Location = new Point(0, 10)
            };
            
            // Créer le panel de contenu
            contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent
            };
            
            // Créer la barre de recherche
            txtSearch = new TextBox
            {
                Size = new Size(250, 30),
                Location = new Point(0, 10),
                Font = new Font("Segoe UI", 10),
                PlaceholderText = "Rechercher un livre..."
            };
            
            // Créer le bouton de recherche
            btnSearch = new Button
            {
                Text = "Rechercher",
                Size = new Size(100, 30),
                Location = new Point(txtSearch.Right + 10, 10),
                BackColor = PrimaryColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            
            // Créer le bouton de filtre
            btnFilter = new Button
            {
                Text = "Filtrer",
                Size = new Size(80, 30),
                Location = new Point(btnSearch.Right + 10, 10),
                BackColor = Color.FromArgb(52, 58, 64),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            
            // Créer le bouton d'ajout
            btnAdd = new Button
            {
                Text = "Ajouter",
                Size = new Size(80, 30),
                Location = new Point(btnFilter.Right + 10, 10),
                BackColor = SuccessColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            
            // Créer la grille de données
            dgvBooks = new DataGridView
            {
                Dock = DockStyle.Bottom,
                Height = contentPanel.Height - 50,
                BackgroundColor = LightColor,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Font = new Font("Segoe UI", 9)
            };
            
            // Configurer les colonnes de la grille
            dgvBooks.Columns.Add("ID", "ID");
            dgvBooks.Columns.Add("Title", "Titre");
            dgvBooks.Columns.Add("Author", "Auteur");
            dgvBooks.Columns.Add("Category", "Catégorie");
            dgvBooks.Columns.Add("ISBN", "ISBN");
            dgvBooks.Columns.Add("Status", "Statut");
            
            // Configurer l'apparence de la grille
            dgvBooks.EnableHeadersVisualStyles = false;
            dgvBooks.ColumnHeadersDefaultCellStyle.BackColor = PrimaryColor;
            dgvBooks.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvBooks.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvBooks.ColumnHeadersHeight = 40;
            
            // Ajouter les contrôles au formulaire
            headerPanel.Controls.Add(lblTitle);
            contentPanel.Controls.Add(txtSearch);
            contentPanel.Controls.Add(btnSearch);
            contentPanel.Controls.Add(btnFilter);
            contentPanel.Controls.Add(btnAdd);
            contentPanel.Controls.Add(dgvBooks);
            
            mainLayout.Controls.Add(headerPanel, 0, 0);
            mainLayout.Controls.Add(contentPanel, 0, 1);
            
            this.Controls.Add(mainLayout);
        }
        
        private void LoadBooks()
        {
            try
            {
                // Simuler des données de livres
                // Dans une application réelle, ces données viendraient de la base de données
                books = new List<Book>
                {
                    new Book { Id = 1, Title = "Le Petit Prince", AuthorName = "Antoine de Saint-Exupéry", Category = "Fiction", ISBN = "978-2-07-040850-4", Status = "Disponible" },
                    new Book { Id = 2, Title = "Harry Potter à l'école des sorciers", AuthorName = "J.K. Rowling", Category = "Fantasy", ISBN = "978-2-07-054302-1", Status = "Emprunté" },
                    new Book { Id = 3, Title = "Les Misérables", AuthorName = "Victor Hugo", Category = "Classique", ISBN = "978-2-253-09634-8", Status = "Disponible" },
                    new Book { Id = 4, Title = "L'Étranger", AuthorName = "Albert Camus", Category = "Philosophie", ISBN = "978-2-07-036002-4", Status = "Emprunté" },
                    new Book { Id = 5, Title = "Le Seigneur des Anneaux", AuthorName = "J.R.R. Tolkien", Category = "Fantasy", ISBN = "978-2-267-01901-7", Status = "Disponible" }
                };
                
                // Afficher les livres dans la grille
                dgvBooks.Rows.Clear();
                foreach (var book in books)
                {
                    dgvBooks.Rows.Add(book.Id, book.Title, book.AuthorName, book.Category, book.ISBN, book.Status);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des livres : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CreateSidebar()
        {
            // Créer les éléments du menu
            string[] menuItems = new string[]
            {
                "Retour",
                "Ajouter un livre",
                "Gérer les auteurs",
                "Gérer les catégories",
                "Exporter la liste"
            };
            
            foreach (string item in menuItems)
            {
                // Créer le bouton
                Button btnMenuItem = new Button
                {
                    Text = item,
                    Size = new Size(80, 30),
                    Location = new Point(310, 60),
                    BackColor = PrimaryColor,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };
                
                if (item == "Retour")
                {
                    btnMenuItem.Click += (s, e) => ReturnToDashboard();
                }
                else if (item == "Ajouter un livre")
                {
                    btnMenuItem.Click += (s, e) => AddNewBook();
                }
                else if (item == "Gérer les auteurs")
                {
                    btnMenuItem.Click += (s, e) => ManageAuthors();
                }
                else if (item == "Gérer les catégories")
                {
                    btnMenuItem.Click += (s, e) => ManageCategories();
                }
                else if (item == "Exporter la liste")
                {
                    btnMenuItem.Click += (s, e) => ExportBookList();
                }
                
                // Ajouter le bouton au menu
                headerPanel.Controls.Add(btnMenuItem);
            }
        }

        // Méthode pour retourner au tableau de bord
        private void ReturnToDashboard()
        {
            this.Close();
        }

        // Méthode pour ajouter un nouveau livre
        private void AddNewBook()
        {
            try
            {
                // Créer un formulaire pour ajouter un nouveau livre
                // Ici, vous pouvez implémenter un formulaire spécifique pour l'ajout de livre
                MessageBox.Show("Fonctionnalité d'ajout de livre à implémenter", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'ajout d'un livre : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Méthode pour gérer les auteurs
        private void ManageAuthors()
        {
            try
            {
                // Implémenter la gestion des auteurs
                MessageBox.Show("Fonctionnalité de gestion des auteurs à implémenter", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la gestion des auteurs : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Méthode pour gérer les catégories
        private void ManageCategories()
        {
            try
            {
                // Implémenter la gestion des catégories
                MessageBox.Show("Fonctionnalité de gestion des catégories à implémenter", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la gestion des catégories : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Méthode pour exporter la liste des livres
        private void ExportBookList()
        {
            try
            {
                // Implémenter la fonctionnalité d'exportation de la liste des livres
                // Par exemple, exporter vers Excel, CSV, PDF, etc.
                MessageBox.Show("Fonctionnalité d'exportation de la liste des livres à implémenter", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'exportation de la liste des livres : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
