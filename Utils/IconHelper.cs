using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace projet_bibliotheque.Utils
{
    public static class IconHelper
    {
        /// <summary>
        /// Charge une icône à partir de différents chemins possibles et avec différentes extensions
        /// </summary>
        /// <param name="iconName">Nom de base de l'icône (sans extension)</param>
        /// <returns>L'image chargée ou null si non trouvée</returns>
        public static Image LoadIcon(string iconName)
        {
            // Extensions à essayer
            string[] extensions = { ".svg", ".png", ".jpg", ".jpeg", ".gif" };
            
            // Dossiers à vérifier
            string[] folders = { "icons", "icons_png", "img" };
            
            // Chemins de base à vérifier
            string[] basePaths = {
                Application.StartupPath,
                Directory.GetCurrentDirectory(),
                Path.Combine(Directory.GetCurrentDirectory(), ".."),
                Path.Combine(Directory.GetCurrentDirectory(), @"..\.."),
                Path.GetDirectoryName(Application.ExecutablePath)
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
            
            // Si aucune icône n'a été trouvée, créer une icône par défaut avec la couleur appropriée
            return CreateDefaultIcon(iconName);
        }
        
        /// <summary>
        /// Crée une icône par défaut adaptée au thème de l'application
        /// </summary>
        /// <param name="iconName">Nom de l'icône pour déterminer sa couleur</param>
        /// <returns>Une icône par défaut</returns>
        private static Image CreateDefaultIcon(string iconName = "default")
        {
            // Créer une image bitmap avec fond transparent
            Bitmap bmp = new Bitmap(24, 24);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                // Activer l'anti-aliasing pour un rendu plus lisse
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                
                // Fond transparent
                g.Clear(Color.Transparent);
                
                // Déterminer la couleur en fonction du nom de l'icône
                Color iconColor = GetColorForIcon(iconName);
                
                // Dessiner un cercle coloré
                using (SolidBrush brush = new SolidBrush(iconColor))
                {
                    g.FillEllipse(brush, 2, 2, 20, 20);
                }
                
                // Ajouter une bordure
                using (Pen pen = new Pen(Color.White, 1))
                {
                    g.DrawEllipse(pen, 2, 2, 20, 20);
                }
            }
            return bmp;
        }
        
        /// <summary>
        /// Retourne une couleur différente selon le nom de l'icône
        /// </summary>
        /// <param name="iconName">Nom de l'icône</param>
        /// <returns>Couleur appropriée pour l'icône</returns>
        private static Color GetColorForIcon(string iconName)
        {
            if (iconName.Contains("dashboard")) return Color.FromArgb(52, 152, 219); // Bleu
            if (iconName.Contains("book")) return Color.FromArgb(231, 76, 60);       // Rouge
            if (iconName.Contains("profil") || iconName.Contains("student")) return Color.FromArgb(46, 204, 113);    // Vert
            if (iconName.Contains("notification")) return Color.FromArgb(241, 196, 15); // Jaune
            if (iconName.Contains("history")) return Color.FromArgb(155, 89, 182);   // Violet
            if (iconName.Contains("setting")) return Color.FromArgb(52, 73, 94);    // Gris foncé
            if (iconName.Contains("logo")) return Color.FromArgb(25, 55, 109);       // Bleu royal
            
            return Color.FromArgb(149, 165, 166); // Gris par défaut
        }
    }
}
