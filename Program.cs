using System;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using projet_bibliotheque.Data;

namespace projet_bibliotheque
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            
            // Initialize the database before starting the application
            bool dbInitialized = DatabaseInitializer.InitializeDatabase();
            
            if (!dbInitialized)
            {
                MessageBox.Show("Impossible de se connecter à la base de données. L'application va se fermer.", 
                    "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            Application.Run(new HomeForm());
        }
    }
}