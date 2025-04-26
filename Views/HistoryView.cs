using System;
using System.Windows.Forms;
using System.Linq;
using projet_bibliotheque.Data;
using projet_bibliotheque.Models;
using System.Drawing;
using Microsoft.EntityFrameworkCore;

namespace projet_bibliotheque.Views
{
    public partial class HistoryView : UserControl
    {
        private DataGridView historyGrid = new DataGridView();

        public HistoryView()
        {
            SetupView();
            LoadHistory();
        }

        private void SetupView()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.White;
            this.Padding = new Padding(20);

            // Title
            var lblTitle = new Label
            {
                Text = "Historique de la Bibliothèque",
                Font = new Font("Poppins", 20, FontStyle.Bold),
                Location = new Point(20, 20),
                AutoSize = true
            };

            // History Grid
            historyGrid = new DataGridView
            {
                Dock = DockStyle.Fill, // Use all available space
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Font = new Font("Poppins", 12),
                RowTemplate = { Height = 40 }
            };

            historyGrid.Columns.Add("Date", "Date");
            historyGrid.Columns.Add("Action", "Action");
            historyGrid.Columns.Add("Détails", "Détails");
            historyGrid.Columns.Add("Membre", "Membre");

            historyGrid.Columns["Date"].Width = 200;
            historyGrid.Columns["Action"].Width = 150;
            historyGrid.Columns["Détails"].Width = 400;
            historyGrid.Columns["Membre"].Width = 250;

            // Style des en-têtes
            historyGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Poppins", 12, FontStyle.Bold);
            historyGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(31, 43, 71);
            historyGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            historyGrid.ColumnHeadersHeight = 50;

            // Style des cellules
            historyGrid.DefaultCellStyle.Padding = new Padding(5);
            historyGrid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(241, 134, 48);
            historyGrid.DefaultCellStyle.SelectionForeColor = Color.White;

            var mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20, 60, 20, 20) // Further adjusted top padding
            };

            mainPanel.Controls.Add(lblTitle);
            mainPanel.Controls.Add(historyGrid);

            this.Controls.Add(mainPanel);
        }

        private void LoadHistory()
        {
            try
            {
                historyGrid.Rows.Clear();

                using (var context = new LibraryContext(new DbContextOptionsBuilder<LibraryContext>().Options))
                {
                    // Get recent loans
                    var recentLoans = context.Loans
                        .Include(l => l.Book)
                        .Include(l => l.Book.Author)
                        .Include(l => l.Member)
                        .OrderByDescending(l => l.LoanDate)
                        .Take(50)
                        .ToList();

                    foreach (var loan in recentLoans)
                    {
                        var action = loan.ReturnDate.HasValue ? "Retourné" : "Emprunté";
                        var details = $"{loan.Book.Title} par {loan.Book.Author?.Name}";
                        historyGrid.Rows.Add(
                            loan.LoanDate.ToString("yyyy-MM-dd HH:mm"),
                            action,
                            details,
                            loan.Member.Name
                        );

                        // If the book was returned, add the return entry
                        if (loan.ReturnDate.HasValue)
                        {
                            historyGrid.Rows.Add(
                                loan.ReturnDate.Value.ToString("yyyy-MM-dd HH:mm"),
                                "Retourné",
                                details,
                                loan.Member.Name
                            );
                        }
                    }

                    // Get recent book additions
                    var recentBooks = context.Books
                        .Include(b => b.Author)
                        .OrderByDescending(b => b.Id)
                        .Take(20)
                        .ToList();

                    foreach (var book in recentBooks)
                    {
                        historyGrid.Rows.Add(
                            DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                            "Ajouté",
                            $"{book.Title} par {book.Author?.Name}",
                            "Système"
                        );
                    }

                    // Get recent member registrations
                    var recentMembers = context.Members
                        .OrderByDescending(m => m.Id)
                        .Take(20)
                        .ToList();

                    foreach (var member in recentMembers)
                    {
                        historyGrid.Rows.Add(
                            member.DateInscription.ToString("yyyy-MM-dd HH:mm"),
                            "Inscrit",
                            member.Name,
                            "Système"
                        );
                    }
                }

                // Sort by date
                historyGrid.Sort(historyGrid.Columns["Date"], System.ComponentModel.ListSortDirection.Descending);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement de l'historique : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
} 