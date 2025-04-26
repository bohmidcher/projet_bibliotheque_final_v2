using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace projet_bibliotheque.Utils
{
    public static class ImageLoader
    {
        /// <summary>
        /// Charge une image à partir de plusieurs chemins possibles
        /// </summary>
        /// <param name="fileName">Nom du fichier image</param>
        /// <param name="subfolder">Sous-dossier dans Assets (par défaut: icons)</param>
        /// <returns>L'image chargée ou null si non trouvée</returns>
        public static Image LoadImage(string fileName, string subfolder = "icons")
        {
            // Liste des chemins possibles pour trouver l'image
            string[] possiblePaths = {
                Path.Combine(Application.StartupPath, "Assets", subfolder, fileName),
                Path.Combine(Directory.GetCurrentDirectory(), "Assets", subfolder, fileName),
                Path.Combine(Directory.GetCurrentDirectory(), "..", "Assets", subfolder, fileName),
                Path.Combine(Directory.GetCurrentDirectory(), @"..\Assets\" + subfolder, fileName),
                Path.Combine(Application.StartupPath, "..", "Assets", subfolder, fileName)
            };
            
            // Essayer chaque chemin jusqu'à trouver l'image
            foreach (string path in possiblePaths)
            {
                if (File.Exists(path))
                {
                    try
                    {
                        // Méthode 1: Charger via stream pour éviter les problèmes de verrouillage de fichier
                        using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                        {
                            return Image.FromStream(stream);
                        }
                    }
                    catch
                    {
                        try
                        {
                            // Méthode 2: Charger directement
                            return Image.FromFile(path);
                        }
                        catch
                        {
                            // Continuer avec le chemin suivant si ça échoue
                            Console.WriteLine($"Échec du chargement de l'image: {path}");
                        }
                    }
                }
            }
            
            // Si aucune image n'a été trouvée, afficher un message dans la console
            Console.WriteLine($"Image non trouvée: {fileName} dans {subfolder}");
            
            // Retourner null si aucune image n'a pu être chargée
            return null;
        }
        
        /// <summary>
        /// Tente de charger une image avec différentes extensions
        /// </summary>
        /// <param name="baseFileName">Nom de base du fichier sans extension</param>
        /// <param name="subfolder">Sous-dossier dans Assets</param>
        /// <returns>L'image chargée ou null si non trouvée</returns>
        public static Image LoadImageWithMultipleExtensions(string baseFileName, string subfolder = "icons")
        {
            // Essayer d'abord avec l'extension SVG
            Image img = LoadImage(baseFileName + ".svg", subfolder);
            if (img != null) return img;
            
            // Essayer avec d'autres extensions courantes
            string[] extensions = { ".png", ".jpg", ".jpeg", ".gif", ".bmp" };
            foreach (string ext in extensions)
            {
                img = LoadImage(baseFileName + ext, subfolder);
                if (img != null) return img;
            }
            
            return null;
        }
    }
}
