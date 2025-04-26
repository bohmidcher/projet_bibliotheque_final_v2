using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace projet_bibliotheque.Controls
{
    public static class RoundRectangle
    {
        public static GraphicsPath Create(float x, float y, float width, float height, float radius)
        {
            GraphicsPath path = new GraphicsPath();
            
            // Coin supérieur gauche
            path.AddArc(x, y, radius * 2, radius * 2, 180, 90);
            
            // Ligne supérieure
            path.AddLine(x + radius, y, x + width - radius, y);
            
            // Coin supérieur droit
            path.AddArc(x + width - radius * 2, y, radius * 2, radius * 2, 270, 90);
            
            // Ligne droite
            path.AddLine(x + width, y + radius, x + width, y + height - radius);
            
            // Coin inférieur droit
            path.AddArc(x + width - radius * 2, y + height - radius * 2, radius * 2, radius * 2, 0, 90);
            
            // Ligne inférieure
            path.AddLine(x + width - radius, y + height, x + radius, y + height);
            
            // Coin inférieur gauche
            path.AddArc(x, y + height - radius * 2, radius * 2, radius * 2, 90, 90);
            
            // Ligne gauche
            path.AddLine(x, y + height - radius, x, y + radius);
            
            path.CloseFigure();
            
            return path;
        }
    }
}