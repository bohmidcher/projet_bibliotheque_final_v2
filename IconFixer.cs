using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace projet_bibliotheque.Utils
{
    public static class IconFixer
    {
        /// <summary>
        /// Crée des icônes PNG par défaut pour remplacer les SVG qui posent problème
        /// </summary>
        public static void CreateDefaultIcons()
        {
            Console.WriteLine("Création des icônes PNG à partir des SVG...");
            
            // Chemin du dossier des icônes SVG
            string svgDir = Path.Combine(Application.StartupPath, "Assets", "icons");
            
            // Chemin du dossier des icônes PNG
            string pngDir = Path.Combine(Application.StartupPath, "Assets", "icons_png");
            
            // Vérifier si les dossiers existent
            if (!Directory.Exists(svgDir))
            {
                Console.WriteLine($"Le dossier {svgDir} n'existe pas.");
                return;
            }
            
            if (!Directory.Exists(pngDir))
            {
                Directory.CreateDirectory(pngDir);
                Console.WriteLine($"Dossier {pngDir} créé.");
            }
            
            // Créer des icônes PNG pour chaque icône nécessaire
            CreateDefaultIcon(pngDir, "dashboard.png");
            CreateDefaultIcon(pngDir, "book.png");
            CreateDefaultIcon(pngDir, "profil.png");
            CreateDefaultIcon(pngDir, "notifications.png");
            CreateDefaultIcon(pngDir, "history.png");
            CreateDefaultIcon(pngDir, "settings.png");
            CreateDefaultIcon(pngDir, "logo.png", 100, 100);
            
            Console.WriteLine("Icônes PNG créées avec succès !");
        }
        
        /// <summary>
        /// Crée une icône PNG par défaut
        /// </summary>
        /// <param name="directory">Répertoire de destination</param>
        /// <param name="filename">Nom du fichier</param>
        /// <param name="width">Largeur de l'icône</param>
        /// <param name="height">Hauteur de l'icône</param>
        private static void CreateDefaultIcon(string directory, string filename, int width = 24, int height = 24)
        {
            string fullPath = Path.Combine(directory, filename);
            
            // Créer une image bitmap simple
            using (Bitmap bmp = new Bitmap(width, height))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    // Fond transparent
                    g.Clear(Color.Transparent);
                    
                    // Dessiner un cercle coloré
                    using (SolidBrush brush = new SolidBrush(GetColorForIcon(filename)))
                    {
                        g.FillEllipse(brush, 2, 2, width - 4, height - 4);
                    }
                    
                    // Ajouter une bordure
                    using (Pen pen = new Pen(Color.White, 1))
                    {
                        g.DrawEllipse(pen, 2, 2, width - 4, height - 4);
                    }
                }
                
                // Sauvegarder l'image
                bmp.Save(fullPath, ImageFormat.Png);
                Console.WriteLine($"Icône créée : {fullPath}");
            }
        }
        
        /// <summary>
        /// Retourne une couleur différente selon le nom de l'icône
        /// </summary>
        /// <param name="filename">Nom du fichier icône</param>
        /// <returns>Couleur appropriée pour l'icône</returns>
        private static Color GetColorForIcon(string filename)
        {
            if (filename.Contains("dashboard")) return Color.FromArgb(52, 152, 219); // Bleu
            if (filename.Contains("book")) return Color.FromArgb(231, 76, 60);       // Rouge
            if (filename.Contains("profil")) return Color.FromArgb(46, 204, 113);    // Vert
            if (filename.Contains("notifications")) return Color.FromArgb(241, 196, 15); // Jaune
            if (filename.Contains("history")) return Color.FromArgb(155, 89, 182);   // Violet
            if (filename.Contains("settings")) return Color.FromArgb(52, 73, 94);    // Gris foncé
            if (filename.Contains("logo")) return Color.FromArgb(25, 55, 109);       // Bleu royal
            
            return Color.FromArgb(149, 165, 166); // Gris par défaut
        }
        
        /// <summary>
        /// Charge une icône à partir de plusieurs chemins possibles et avec différentes extensions
        /// </summary>
        /// <param name="iconName">Nom de base de l'icône (sans extension)</param>
        /// <returns>L'image chargée ou null si non trouvée</returns>
        public static Image LoadIcon(string iconName)
        {
            // Extensions à essayer
            string[] extensions = { ".png", ".svg", ".jpg", ".jpeg", ".gif" };
            
            // Dossiers à vérifier
            string[] folders = { "icons_png", "icons", "img" };
            
            // Chemins de base à vérifier
            string[] basePaths = {
                Application.StartupPath,
                Directory.GetCurrentDirectory(),
                Path.Combine(Directory.GetCurrentDirectory(), "..")
            };
            
            // Essayer toutes les combinaisons possibles
            foreach (string basePath in basePaths)
            {
                foreach (string folder in folders)
                {
                    foreach (string ext in extensions)
                    {
                        string fullPath = Path.Combine(basePath, "Assets", folder, iconName + ext);
                        
                        if (File.Exists(fullPath))
                        {
                            try
                            {
                                // Méthode 1: Charger via stream pour éviter les problèmes de verrouillage
                                using (var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
                                {
                                    return Image.FromStream(stream);
                                }
                            }
                            catch
                            {
                                try
                                {
                                    // Méthode 2: Charger directement
                                    return Image.FromFile(fullPath);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Erreur lors du chargement de l'icône {fullPath}: {ex.Message}");
                                }
                            }
                        }
                    }
                }
            }
            
            // Si aucune icône n'a été trouvée, créer une icône par défaut
            return CreateDefaultIconImage(iconName);
        }
        
        /// <summary>
        /// Crée une icône par défaut en mémoire
        /// </summary>
        /// <param name="iconName">Nom de l'icône</param>
        /// <param name="width">Largeur</param>
        /// <param name="height">Hauteur</param>
        /// <returns>Image de l'icône</returns>
        private static Image CreateDefaultIconImage(string iconName, int width = 24, int height = 24)
        {
            // Créer une image bitmap simple
            Bitmap bmp = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                // Fond transparent
                g.Clear(Color.Transparent);
                
                // Dessiner un cercle coloré
                using (SolidBrush brush = new SolidBrush(GetColorForIcon(iconName)))
                {
                    g.FillEllipse(brush, 2, 2, width - 4, height - 4);
                }
                
                // Ajouter une bordure
                using (Pen pen = new Pen(Color.White, 1))
                {
                    g.DrawEllipse(pen, 2, 2, width - 4, height - 4);
                }
            }
            
            return bmp;
        }
    }
}
