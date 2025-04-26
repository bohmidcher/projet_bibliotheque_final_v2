using System;
using System.Windows.Forms;
using projet_bibliotheque.Models;
using projet_bibliotheque.Data;

namespace projet_bibliotheque.Forms
{
    public partial class MemberForm : Form
    {
        private readonly LibraryContext _context;
        private readonly Member _member;

        public MemberForm(LibraryContext context, Member? member = null)
        {
            InitializeComponent();
            _context = context;
            _member = member ?? new Member();
            
            // Charger les données du membre si en mode édition
            if (member != null)
            {
                LoadMemberData();
            }
        }

        private void InitializeComponent()
        {
            // Couleurs du thème
            Color primaryColor = Color.FromArgb(31, 43, 71);
            Color secondaryColor = Color.FromArgb(241, 134, 48);
            Color lightColor = Color.FromArgb(250, 250, 250);
            
            this.SuspendLayout();
            
            // Configuration du formulaire
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Name = "MemberForm";
            this.Text = "Gestion des Membres";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = lightColor;
            
            // Ajouter les contrôles ici
            CreateControls();
            
            this.ResumeLayout(false);
        }
        
        private void CreateControls()
        {
            // Implémentation des contrôles pour le formulaire
        }
        
        private void LoadMemberData()
        {
            // Charger les données du membre dans les contrôles
        }
    }
}
