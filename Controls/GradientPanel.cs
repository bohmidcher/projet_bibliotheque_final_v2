using System.Drawing.Drawing2D;

namespace projet_bibliotheque
{
    public class GradientPanel : Panel
    {
        public Color StartColor { get; set; }
        public Color EndColor { get; set; }
        public LinearGradientMode GradientMode { get; set; }

        protected override void OnPaint(PaintEventArgs e)
        {
            LinearGradientBrush lgb = new LinearGradientBrush(
                this.ClientRectangle,
                this.StartColor,
                this.EndColor,
                this.GradientMode);
            Graphics g = e.Graphics;
            g.FillRectangle(lgb, this.ClientRectangle);
            base.OnPaint(e);
        }
    }
}
