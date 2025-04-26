using System;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace projet_bibliotheque.Data
{
    public static class DatabaseInitializer
    {
        public static bool InitializeDatabase()
        {
            try
            {
                var optionsBuilder = new DbContextOptionsBuilder<LibraryContext>();
                optionsBuilder.UseSqlServer(@"Server=localhost;Database=BibliothequeIHEC;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true",
                    options => options.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorNumbersToAdd: null))
                    .UseLazyLoadingProxies();

                using (var context = new LibraryContext(optionsBuilder.Options))
                {
                    // Vérifie si la base de données existe et peut être atteinte
                    if (!context.Database.CanConnect())
                    {
                        MessageBox.Show("Impossible de se connecter à la base de données. Vérifiez que SQL Server est démarré.",
                            "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    // Vérifie s'il y a des migrations en attente
                    if (context.Database.GetPendingMigrations().Any())
                    {
                        var result = MessageBox.Show(
                            "Des mises à jour de la base de données sont disponibles. Voulez-vous les appliquer maintenant ?",
                            "Mise à jour de la base de données",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        if (result == DialogResult.Yes)
                        {
                            context.Database.Migrate();
                        }
                        else
                        {
                            return false;
                        }
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'initialisation de la base de données : {ex.Message}", 
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
