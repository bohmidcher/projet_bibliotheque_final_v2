using System;
using System.Drawing;
using System.Windows.Forms;

namespace projet_bibliotheque.Forms
{
    public class TestForm : Form
    {
        private Button btnOpenStudents;
        private Button btnOpenLibrary;

        public TestForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.ClientSize = new Size(400, 300);
            this.Text = "Test Form";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            // Créer les boutons de test
            btnOpenStudents = new Button
            {
                Text = "Ouvrir Gestion Étudiants",
                Size = new Size(200, 40),
                Location = new Point(100, 80),
                BackColor = Color.FromArgb(0, 123, 255),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnOpenStudents.FlatAppearance.BorderSize = 0;
            btnOpenStudents.Click += BtnOpenStudents_Click;

            btnOpenLibrary = new Button
            {
                Text = "Ouvrir Gestion Bibliothèque",
                Size = new Size(200, 40),
                Location = new Point(100, 150),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnOpenLibrary.FlatAppearance.BorderSize = 0;
            btnOpenLibrary.Click += BtnOpenLibrary_Click;

            // Ajouter les contrôles au formulaire
            this.Controls.Add(btnOpenStudents);
            this.Controls.Add(btnOpenLibrary);
        }

        private void BtnOpenStudents_Click(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show("Tentative d'ouverture du formulaire de gestion des étudiants...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // Créer une instance du formulaire de gestion des étudiants
                var studentForm = new StudentManagementForm2(null);
                
                // Afficher le formulaire
                studentForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'ouverture du formulaire de gestion des étudiants : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnOpenLibrary_Click(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show("Tentative d'ouverture du formulaire de gestion de la bibliothèque...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // Créer une instance du formulaire de gestion de la bibliothèque
                var libraryForm = new LibraryManagementForm(null);
                
                // Afficher le formulaire
                libraryForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'ouverture du formulaire de gestion de la bibliothèque : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
