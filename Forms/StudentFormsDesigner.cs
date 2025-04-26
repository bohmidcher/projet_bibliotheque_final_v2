using System;
using System.Windows.Forms;
using projet_bibliotheque.Models;
using projet_bibliotheque.Data;
using Microsoft.EntityFrameworkCore;

namespace projet_bibliotheque.Forms
{
    public partial class StudentForm
    {
        // Méthode pour initialiser les composants (requise par le designer)
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // StudentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 800);
            this.Name = "StudentForm";
            this.Text = "BiblioHub - Espace Étudiant";
            this.ResumeLayout(false);
        }
    }
    
    public partial class StudentHomeForm
    {
        // Méthode pour initialiser les composants (requise par le designer)
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // StudentHomeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Name = "StudentHomeForm";
            this.Text = "BiblioHub - Accueil";
            this.ResumeLayout(false);
        }
    }
    
    public partial class StudentLibraryForm
    {
        // Méthode pour initialiser les composants (requise par le designer)
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // StudentLibraryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Name = "StudentLibraryForm";
            this.Text = "BiblioHub - Ma Bibliothèque";
            this.ResumeLayout(false);
        }
    }
    
    public partial class StudentBorrowForm
    {
        // Méthode pour initialiser les composants (requise par le designer)
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // StudentBorrowForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Name = "StudentBorrowForm";
            this.Text = "BiblioHub - Emprunter un livre";
            this.ResumeLayout(false);
        }
    }
    
    public partial class StudentNotificationsForm
    {
        // Méthode pour initialiser les composants (requise par le designer)
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // StudentNotificationsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Name = "StudentNotificationsForm";
            this.Text = "BiblioHub - Notifications";
            this.ResumeLayout(false);
        }
    }
    
    public partial class StudentHistoryForm
    {
        // Méthode pour initialiser les composants (requise par le designer)
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // StudentHistoryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Name = "StudentHistoryForm";
            this.Text = "BiblioHub - Historique";
            this.ResumeLayout(false);
        }
    }
    
    public partial class StudentSettingsForm
    {
        // Méthode pour initialiser les composants (requise par le designer)
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // StudentSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Name = "StudentSettingsForm";
            this.Text = "BiblioHub - Paramètres";
            this.ResumeLayout(false);
        }
    }
    
    public partial class StudentProfileForm
    {
        // Méthode pour initialiser les composants (requise par le designer)
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // StudentProfileForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Name = "StudentProfileForm";
            this.Text = "BiblioHub - Profil";
            this.ResumeLayout(false);
        }
    }
}
