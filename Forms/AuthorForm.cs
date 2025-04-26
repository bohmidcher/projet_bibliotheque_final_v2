using System;
using System.Windows.Forms;
using projet_bibliotheque.Models;
using projet_bibliotheque.Data;

namespace projet_bibliotheque.Forms
{
    public partial class AuthorForm : Form
    {
        private readonly LibraryContext _context;
        private readonly Author _author;

        public AuthorForm(LibraryContext context, Author? author = null)
        {
            InitializeComponent();
            _context = context;
            _author = author ?? new Author();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // AuthorForm
            // 
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Name = "AuthorForm";
            this.Text = "Author Management";
            this.ResumeLayout(false);
        }
    }
}
