using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using projet_bibliotheque.Controls;
using projet_bibliotheque.Models;
using projet_bibliotheque.Data;
using Microsoft.EntityFrameworkCore;
using projet_bibliotheque.Utils;

namespace projet_bibliotheque.Forms
{
    public class DashboardForm : Form
    {
        private Member currentUser;
        private Panel mainPanel;
        private Panel sidebarPanel;
        private Panel contentPanel;
        
        // Couleurs de l'application
        private readonly Color PrimaryColor = Color.FromArgb(8, 15, 40);  // Bleu foncé
        private readonly Color AccentColor = Color.FromArgb(45, 20, 80);  // Violet foncé
        private readonly Color TextColor = Color.White;
        private readonly Color SidebarColor = Color.FromArgb(20, 30, 70);
        private readonly Color BackgroundColor = Color.WhiteSmoke;       
        private readonly LibraryContext _context;
        
        // Statistiques
        private int totalUsers = 7265;
        private int totalBooks = 3671;
        private int newUsers = 156;
        private int activeStudents = 2318;
        
        // Variations (en pourcentage)
        private float usersVariation = 11.01f;
        private float booksVariation = -0.13f;
        private float newUsersVariation = 15.03f;
        private float activeStudentsVariation = 6.08f;
        
        public DashboardForm(Member user = null, LibraryContext context = null)
        {
            this.currentUser = user;
            this._context = context ?? new LibraryContext(new DbContextOptionsBuilder<LibraryContext>().Options);
            InitializeComponent();
            SetupForm();
            CreateDashboard();
            
            // Ajouter un bouton direct pour gérer les étudiants
            AddDirectStudentButton();
        }
        
        // Nous n'avons plus besoin de cette méthode car nous utilisons le menu de la sidebar
        private void AddDirectStudentButton()
        {
            // Méthode vide pour ne pas affecter le code existant
        }
        
        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1200, 800);
            this.Name = "DashboardForm";
            this.Text = "BiblioHub - Tableau de bord";
            this.ResumeLayout(false);
        }
        
        private void SetupForm()
        {
            this.Text = "BiblioHub - Tableau de bord";
            this.MinimumSize = new Size(1200, 800);
            this.Size = new Size(1200, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.DoubleBuffered = true;
        }
        
        private void CreateDashboard()
        {
            // Créer le layout principal
            TableLayoutPanel mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Color.White
            };
            
            // Configurer les colonnes (sidebar et contenu principal)
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220F));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            
            // Créer la sidebar
            sidebarPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 220,
                BackColor = SidebarColor
            };
            
            // Créer le contenu principal
            contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = BackgroundColor,
                Padding = new Padding(20)
            };
            
            // Ajouter les panels au layout
            mainLayout.Controls.Add(sidebarPanel, 0, 0);
            mainLayout.Controls.Add(contentPanel, 1, 0);
            
            // Ajouter le layout au formulaire
            this.Controls.Add(mainLayout);
            
            // Créer les éléments de la sidebar
            CreateSidebar();
            
            // Créer le contenu du tableau de bord
            CreateDashboardContent();
        }
        
        private void CreateDashboardContent()
        {
            // Titre du tableau de bord avec le nom de l'utilisateur
            Label lblWelcome = new Label
            {
                Text = "Bienvenue, " + (currentUser != null ? currentUser.Name : "Ahmed") + " !",
                Font = new Font("Poppins", 18, FontStyle.Regular),
                ForeColor = PrimaryColor,
                Location = new Point(30, 30),
                AutoSize = true
            };
            
            // Sous-titre
            Label lblDashboardTitle = new Label
            {
                Text = "Tableau de bord :",
                Font = new Font("Poppins", 24, FontStyle.Bold),
                ForeColor = PrimaryColor,
                Location = new Point(30, lblWelcome.Bottom),
                AutoSize = true
            };
            
            // Ajouter les contrôles au panel de contenu
            contentPanel.Controls.Add(lblWelcome);
            contentPanel.Controls.Add(lblDashboardTitle);
            
            // Créer les cartes de statistiques
            CreateStatCards();
            
            // Créer les sections pour les graphiques
            CreateCharts();
        }
        
        private void CreateStatCards()
        {
            // Définir les dimensions des cartes
            int cardWidth = 180;
            int cardHeight = 120;
            int cardSpacing = 20;
            int startY = 100;
            int startX = 30;
            
            // Créer les cartes de statistiques
            CreateStatCard("Utilisateurs", totalUsers.ToString("N0"), usersVariation, startX, startY, cardWidth, cardHeight, Color.FromArgb(52, 152, 219));
            CreateStatCard("Livres empruntés", totalBooks.ToString("N0"), booksVariation, startX + cardWidth + cardSpacing, startY, cardWidth, cardHeight, Color.FromArgb(231, 76, 60));
            CreateStatCard("Nouveaux Utilisateurs", newUsers.ToString("N0"), newUsersVariation, startX + (cardWidth + cardSpacing) * 2, startY, cardWidth, cardHeight, Color.FromArgb(46, 204, 113));
            CreateStatCard("Étudiants Actifs", activeStudents.ToString("N0"), activeStudentsVariation, startX + (cardWidth + cardSpacing) * 3, startY, cardWidth, cardHeight, Color.FromArgb(155, 89, 182));
        }
        
        private void CreateStatCard(string title, string value, float variation, int x, int y, int width, int height, Color color)
        {
            // Créer le panel principal de la carte
            Panel cardPanel = new Panel
            {
                Size = new Size(width, height),
                Location = new Point(x, y),
                BackColor = Color.White,
                BorderStyle = BorderStyle.None
            };
            
            // Ajouter une ombre et des coins arrondis
            cardPanel.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var path = RoundedRectangle(new Rectangle(0, 0, cardPanel.Width, cardPanel.Height), 10))
                {
                    using (var brush = new SolidBrush(Color.White))
                    {
                        e.Graphics.FillPath(brush, path);
                    }
                }
            };
            
            // Titre de la statistique
            Label lblTitle = new Label
            {
                Text = title,
                Font = new Font("Poppins", 12, FontStyle.Regular),
                ForeColor = Color.Gray,
                Location = new Point(15, 15),
                AutoSize = true
            };
            
            // Valeur de la statistique
            Label lblValue = new Label
            {
                Text = value,
                Font = new Font("Poppins", 24, FontStyle.Bold),
                ForeColor = PrimaryColor,
                Location = new Point(15, 45),
                AutoSize = true
            };
            
            // Variation
            string variationText = variation >= 0 ? "+" + variation.ToString("0.00") + "%" : variation.ToString("0.00") + "%";
            Label lblVariation = new Label
            {
                Text = variationText,
                Font = new Font("Poppins", 10, FontStyle.Regular),
                ForeColor = variation >= 0 ? Color.Green : Color.Red,
                Location = new Point(15, 80),
                AutoSize = true
            };
            
            // Icône de tendance (flèche vers le haut ou vers le bas)
            PictureBox iconTrend = new PictureBox
            {
                Size = new Size(15, 15),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent,
                Location = new Point(lblVariation.Right + 5, lblVariation.Top + 2)
            };
            
            // Ajouter une ligne de couleur en haut de la carte
            Panel colorBar = new Panel
            {
                Size = new Size(width, 5),
                Location = new Point(0, 0),
                BackColor = color
            };
            
            // Ajouter les contrôles à la carte
            cardPanel.Controls.Add(lblTitle);
            cardPanel.Controls.Add(lblValue);
            cardPanel.Controls.Add(lblVariation);
            cardPanel.Controls.Add(iconTrend);
            cardPanel.Controls.Add(colorBar);
            
            // Ajouter la carte au panel de contenu
            contentPanel.Controls.Add(cardPanel);
        }
        
        private void CreateCharts()
        {
            // Section des étudiants actifs et statut des emprunts
            Panel chartSection1 = new Panel
            {
                Size = new Size(contentPanel.Width - 60, 250),
                Location = new Point(30, 240),
                BackColor = Color.White,
                BorderStyle = BorderStyle.None
            };
            
            // Ajouter une ombre et des coins arrondis
            chartSection1.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var path = RoundedRectangle(new Rectangle(0, 0, chartSection1.Width, chartSection1.Height), 10))
                {
                    using (var brush = new SolidBrush(Color.White))
                    {
                        e.Graphics.FillPath(brush, path);
                    }
                }
            };
            
            // Titre de la section
            Label lblChartTitle1 = new Label
            {
                Text = "Étudiants actifs",
                Font = new Font("Poppins", 14, FontStyle.Bold),
                ForeColor = PrimaryColor,
                Location = new Point(20, 15),
                AutoSize = true
            };
            
            // Sous-titre
            Label lblChartSubtitle1 = new Label
            {
                Text = "Statut des emprunts",
                Font = new Font("Poppins", 10, FontStyle.Regular),
                ForeColor = Color.Gray,
                Location = new Point(20, 40),
                AutoSize = true
            };
            
            // Légende
            Panel legendPanel = new Panel
            {
                Size = new Size(200, 30),
                Location = new Point(chartSection1.Width - 220, 15),
                BackColor = Color.Transparent
            };
            
            // Légende pour l'année en cours
            Panel currentYearDot = new Panel
            {
                Size = new Size(8, 8),
                Location = new Point(0, 10),
                BackColor = Color.Black
            };
            currentYearDot.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (SolidBrush brush = new SolidBrush(Color.Black))
                {
                    e.Graphics.FillEllipse(brush, 0, 0, 8, 8);
                }
            };
            
            Label lblCurrentYear = new Label
            {
                Text = "Cette année",
                Font = new Font("Poppins", 9, FontStyle.Regular),
                ForeColor = Color.Black,
                Location = new Point(15, 5),
                AutoSize = true
            };
            
            // Légende pour l'année précédente
            Panel previousYearDot = new Panel
            {
                Size = new Size(8, 8),
                Location = new Point(100, 10),
                BackColor = Color.LightGray
            };
            previousYearDot.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (SolidBrush brush = new SolidBrush(Color.LightGray))
                {
                    e.Graphics.FillEllipse(brush, 0, 0, 8, 8);
                }
            };
            
            Label lblPreviousYear = new Label
            {
                Text = "L'année précédente",
                Font = new Font("Poppins", 9, FontStyle.Regular),
                ForeColor = Color.Gray,
                Location = new Point(115, 5),
                AutoSize = true
            };
            
            // Placeholder pour le graphique (à remplacer par un vrai graphique)
            PictureBox chartPlaceholder = new PictureBox
            {
                Size = new Size(chartSection1.Width - 40, 150),
                Location = new Point(20, 70),
                BackColor = Color.Transparent,
                BorderStyle = BorderStyle.None
            };
            
            chartPlaceholder.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                
                // Dessiner les lignes horizontales (grille)
                using (Pen gridPen = new Pen(Color.FromArgb(230, 230, 230)))
                {
                    for (int y = 0; y < 4; y++)
                    {
                        e.Graphics.DrawLine(gridPen, 0, y * 50, chartPlaceholder.Width, y * 50);
                    }
                }
                
                // Dessiner les étiquettes des mois
                string[] months = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul" };
                using (Font monthFont = new Font("Poppins", 8))
                using (SolidBrush textBrush = new SolidBrush(Color.Gray))
                {
                    for (int i = 0; i < months.Length; i++)
                    {
                        float x = i * (chartPlaceholder.Width / 6);
                        e.Graphics.DrawString(months[i], monthFont, textBrush, x, chartPlaceholder.Height - 20);
                    }
                }
                
                // Dessiner une courbe pour cette année
                Point[] currentYearPoints = {
                    new Point(0, 100),
                    new Point(chartPlaceholder.Width / 6, 80),
                    new Point(chartPlaceholder.Width / 3, 60),
                    new Point(chartPlaceholder.Width / 2, 30),
                    new Point(2 * chartPlaceholder.Width / 3, 70),
                    new Point(5 * chartPlaceholder.Width / 6, 50),
                    new Point(chartPlaceholder.Width, 40)
                };
                
                using (Pen curvePen = new Pen(Color.Black, 2))
                {
                    e.Graphics.DrawCurve(curvePen, currentYearPoints, 0.5f);
                }
                
                // Dessiner une courbe pour l'année précédente
                Point[] previousYearPoints = {
                    new Point(0, 120),
                    new Point(chartPlaceholder.Width / 6, 100),
                    new Point(chartPlaceholder.Width / 3, 90),
                    new Point(chartPlaceholder.Width / 2, 70),
                    new Point(2 * chartPlaceholder.Width / 3, 110),
                    new Point(5 * chartPlaceholder.Width / 6, 80),
                    new Point(chartPlaceholder.Width, 90)
                };
                
                using (Pen curvePen = new Pen(Color.LightGray, 2))
                {
                    e.Graphics.DrawCurve(curvePen, previousYearPoints, 0.5f);
                }
            };
            
            // Ajouter les contrôles à la légende
            legendPanel.Controls.Add(currentYearDot);
            legendPanel.Controls.Add(lblCurrentYear);
            legendPanel.Controls.Add(previousYearDot);
            legendPanel.Controls.Add(lblPreviousYear);
            
            // Ajouter les contrôles à la section
            chartSection1.Controls.Add(lblChartTitle1);
            chartSection1.Controls.Add(lblChartSubtitle1);
            chartSection1.Controls.Add(legendPanel);
            chartSection1.Controls.Add(chartPlaceholder);
            
            // Ajouter la section au panel de contenu
            contentPanel.Controls.Add(chartSection1);
            
            // Section des graphiques de répartition
            CreateDistributionCharts();
        }
        
        private void CreateDistributionCharts()
        {
            int startY = 510;
            int chartWidth = (contentPanel.Width - 70) / 2;
            int chartHeight = 250;
            
            // Section des étudiants par niveau
            Panel levelChartPanel = new Panel
            {
                Size = new Size(chartWidth, chartHeight),
                Location = new Point(30, startY),
                BackColor = Color.White,
                BorderStyle = BorderStyle.None
            };
            
            // Ajouter une ombre et des coins arrondis
            levelChartPanel.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var path = RoundedRectangle(new Rectangle(0, 0, levelChartPanel.Width, levelChartPanel.Height), 10))
                {
                    using (var brush = new SolidBrush(Color.White))
                    {
                        e.Graphics.FillPath(brush, path);
                    }
                }
            };
            
            // Titre de la section
            Label lblLevelChartTitle = new Label
            {
                Text = "Étudiants inscrits par niveau",
                Font = new Font("Poppins", 12, FontStyle.Bold),
                ForeColor = PrimaryColor,
                Location = new Point(15, 15),
                AutoSize = true
            };
            
            // Placeholder pour le graphique en barres
            PictureBox levelChartPlaceholder = new PictureBox
            {
                Size = new Size(chartWidth - 30, 200),
                Location = new Point(15, 45),
                BackColor = Color.Transparent,
                BorderStyle = BorderStyle.None
            };
            
            levelChartPlaceholder.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                
                // Dessiner les barres
                int barWidth = 40;
                int spacing = 30;
                int maxHeight = 180;
                
                // Valeurs pour chaque niveau
                int[] values = { 800, 1300, 1000, 1500, 600 };
                string[] labels = { "1ère", "2ème", "3ème", "M1", "M2" };
                Color[] barColors = {
                    Color.FromArgb(65, 105, 225),  // Bleu royal
                    Color.FromArgb(100, 149, 237), // Bleu cornflower
                    Color.FromArgb(65, 105, 225),  // Bleu royal
                    Color.FromArgb(100, 149, 237), // Bleu cornflower
                    Color.FromArgb(65, 105, 225)   // Bleu royal
                };
                
                // Trouver la valeur maximale pour l'échelle
                int maxValue = values.Max();
                
                // Dessiner les barres
                for (int i = 0; i < values.Length; i++)
                {
                    int barHeight = (int)((float)values[i] / maxValue * maxHeight);
                    int x = i * (barWidth + spacing) + 20;
                    int y = maxHeight - barHeight + 10;
                    
                    // Dessiner la barre avec des coins arrondis en haut
                    using (var path = new GraphicsPath())
                    {
                        int radius = 5;
                        path.AddArc(x, y, radius * 2, radius * 2, 180, 90);
                        path.AddArc(x + barWidth - radius * 2, y, radius * 2, radius * 2, 270, 90);
                        path.AddLine(x + barWidth, y + radius, x + barWidth, y + barHeight);
                        path.AddLine(x + barWidth, y + barHeight, x, y + barHeight);
                        path.AddLine(x, y + barHeight, x, y + radius);
                        
                        using (var brush = new SolidBrush(barColors[i]))
                        {
                            e.Graphics.FillPath(brush, path);
                        }
                    }
                    
                    // Dessiner l'étiquette
                    using (Font labelFont = new Font("Poppins", 8))
                    using (SolidBrush textBrush = new SolidBrush(Color.Gray))
                    {
                        SizeF textSize = e.Graphics.MeasureString(labels[i], labelFont);
                        e.Graphics.DrawString(labels[i], labelFont, textBrush, x + (barWidth - textSize.Width) / 2, maxHeight + 15);
                    }
                }
            };
            
            // Ajouter les contrôles à la section
            levelChartPanel.Controls.Add(lblLevelChartTitle);
            levelChartPanel.Controls.Add(levelChartPlaceholder);
            
            // Section des étudiants par spécialité
            Panel specialtyChartPanel = new Panel
            {
                Size = new Size(chartWidth, chartHeight),
                Location = new Point(30 + chartWidth + 10, startY),
                BackColor = Color.White,
                BorderStyle = BorderStyle.None
            };
            
            // Ajouter une ombre et des coins arrondis
            specialtyChartPanel.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var path = RoundedRectangle(new Rectangle(0, 0, specialtyChartPanel.Width, specialtyChartPanel.Height), 10))
                {
                    using (var brush = new SolidBrush(Color.White))
                    {
                        e.Graphics.FillPath(brush, path);
                    }
                }
            };
            
            // Titre de la section
            Label lblSpecialtyChartTitle = new Label
            {
                Text = "Étudiants inscrits par spécialités",
                Font = new Font("Poppins", 12, FontStyle.Bold),
                ForeColor = PrimaryColor,
                Location = new Point(15, 15),
                AutoSize = true
            };
            
            // Placeholder pour le graphique en camembert
            PictureBox pieChartPlaceholder = new PictureBox
            {
                Size = new Size(180, 180),
                Location = new Point(15, 45),
                BackColor = Color.Transparent,
                BorderStyle = BorderStyle.None
            };
            
            pieChartPlaceholder.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                
                // Données pour le camembert
                float[] percentages = { 52.1f, 22.8f, 13.9f, 11.2f };
                Color[] sliceColors = {
                    Color.FromArgb(65, 105, 225),   // Bleu royal
                    Color.FromArgb(255, 165, 0),   // Orange
                    Color.FromArgb(46, 139, 87),    // Vert mer
                    Color.FromArgb(220, 220, 220)   // Gris clair
                };
                
                // Dessiner le camembert
                Rectangle pieRect = new Rectangle(10, 10, 160, 160);
                float startAngle = 0;
                
                for (int i = 0; i < percentages.Length; i++)
                {
                    float sweepAngle = percentages[i] * 360 / 100;
                    
                    using (SolidBrush brush = new SolidBrush(sliceColors[i]))
                    {
                        e.Graphics.FillPie(brush, pieRect, startAngle, sweepAngle);
                    }
                    
                    startAngle += sweepAngle;
                }
                
                // Dessiner un cercle blanc au centre pour créer un effet de donut
                Rectangle innerCircle = new Rectangle(50, 50, 80, 80);
                using (SolidBrush brush = new SolidBrush(Color.White))
                {
                    e.Graphics.FillEllipse(brush, innerCircle);
                }
            };
            
            // Légende du camembert
            Panel pieChartLegend = new Panel
            {
                Size = new Size(chartWidth - 200, 180),
                Location = new Point(200, 45),
                BackColor = Color.Transparent
            };
            
            // Données pour la légende
            string[] specialties = { "Comptabilité", "Finances", "Management", "Autres" };
            string[] specialtyPercentages = { "52.1%", "22.8%", "13.9%", "11.2%" };
            Color[] legendColors = {
                Color.FromArgb(65, 105, 225),   // Bleu royal
                Color.FromArgb(255, 165, 0),   // Orange
                Color.FromArgb(46, 139, 87),    // Vert mer
                Color.FromArgb(220, 220, 220)   // Gris clair
            };
            
            for (int i = 0; i < specialties.Length; i++)
            {
                // Indicateur de couleur
                Panel colorIndicator = new Panel
                {
                    Size = new Size(10, 10),
                    Location = new Point(0, i * 30 + 5),
                    BackColor = legendColors[i]
                };
                
                // Nom de la spécialité
                Label lblSpecialty = new Label
                {
                    Text = specialties[i],
                    Font = new Font("Poppins", 10, FontStyle.Regular),
                    ForeColor = Color.Black,
                    Location = new Point(20, i * 30),
                    AutoSize = true
                };
                
                // Pourcentage
                Label lblPercentage = new Label
                {
                    Text = specialtyPercentages[i],
                    Font = new Font("Poppins", 10, FontStyle.Bold),
                    ForeColor = Color.Black,
                    Location = new Point(120, i * 30),
                    AutoSize = true
                };
                
                pieChartLegend.Controls.Add(colorIndicator);
                pieChartLegend.Controls.Add(lblSpecialty);
                pieChartLegend.Controls.Add(lblPercentage);
            }
            
            // Ajouter les contrôles à la section
            specialtyChartPanel.Controls.Add(lblSpecialtyChartTitle);
            specialtyChartPanel.Controls.Add(pieChartPlaceholder);
            specialtyChartPanel.Controls.Add(pieChartLegend);
            
            // Ajouter les sections au panel de contenu
            contentPanel.Controls.Add(levelChartPanel);
            contentPanel.Controls.Add(specialtyChartPanel);
        }
        
        // Méthode utilitaire pour créer un rectangle arrondi
        private GraphicsPath RoundedRectangle(Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            Size size = new Size(diameter, diameter);
            Rectangle arc = new Rectangle(bounds.Location, size);
            GraphicsPath path = new GraphicsPath();
            
            // Top left arc
            path.AddArc(arc, 180, 90);
            
            // Top right arc
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);
            
            // Bottom right arc
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);
            
            // Bottom left arc
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);
            
            path.CloseFigure();
            return path;
        }
        
        private void CreateSidebar()
        {
            // Panneau de la sidebar
            sidebarPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 220,
                BackColor = SidebarColor
            };
            
            // Logo en haut de la sidebar
            PictureBox logoBox = new PictureBox
            {
                Size = new Size(100, 100),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent,
                Location = new Point((sidebarPanel.Width - 100) / 2, 20)
            };
            
            try
            {
                string logoPath = Path.Combine(Application.StartupPath, "Assets", "img", "logo.svg");
                if (File.Exists(logoPath))
                {
                    logoBox.Image = Image.FromFile(logoPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement du logo : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            // Informations de l'utilisateur
            Panel userInfoPanel = new Panel
            {
                Width = sidebarPanel.Width,
                Height = 60,
                BackColor = Color.FromArgb(30, 40, 80),
                Location = new Point(0, 130)
            };
            
            Label lblUserName = new Label
            {
                Text = currentUser != null ? currentUser.Name : "Ahmed Chermiti",
                ForeColor = Color.White,
                Font = new Font("Poppins", 12, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleLeft,
                Location = new Point(60, 10),
                Size = new Size(160, 25)
            };
            
            Label lblUserEmail = new Label
            {
                Text = currentUser != null ? currentUser.Email : "ahmed.chermiti@ihec.tn",
                ForeColor = Color.Silver,
                Font = new Font("Poppins", 8, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleLeft,
                Location = new Point(60, 35),
                Size = new Size(160, 15)
            };
            
            // Avatar de l'utilisateur
            PictureBox avatarBox = new PictureBox
            {
                Size = new Size(40, 40),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent,
                Location = new Point(10, 10),
                BorderStyle = BorderStyle.None
            };
            
            // Créer un cercle pour l'avatar
            avatarBox.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (SolidBrush brush = new SolidBrush(Color.Gray))
                {
                    e.Graphics.FillEllipse(brush, 0, 0, avatarBox.Width, avatarBox.Height);
                }
            };
            
            // Menu items
            string[] menuItems = { "Tableau de bord", "Gérer la bibliothèque", "Gérer les étudiants", "Gérer les emprunts", "Notifications", "Historique", "Paramètres" };
            int menuY = 210;
            
            foreach (string item in menuItems)
            {
                Button btnMenuItem = new Button
                {
                    Text = "  " + item,
                    TextAlign = ContentAlignment.MiddleLeft,
                    FlatStyle = FlatStyle.Flat,
                    BackColor = item == "Tableau de bord" ? Color.FromArgb(40, 50, 90) : Color.Transparent,
                    ForeColor = Color.White,
                    Font = new Font("Poppins", 10, FontStyle.Regular),
                    Size = new Size(220, 40),
                    Location = new Point(0, menuY),
                    Cursor = Cursors.Hand,
                    Tag = item  // Stocker le nom de l'item dans la propriété Tag
                };
                
                btnMenuItem.FlatAppearance.BorderSize = 0;
                btnMenuItem.FlatAppearance.MouseOverBackColor = Color.FromArgb(40, 50, 90);
                
                // Ajouter un gestionnaire d'événements spécifique pour chaque bouton
                if (item == "Gérer les étudiants")
                {
                    btnMenuItem.Click += (s, e) => OpenStudentManagement();
                }
                else if (item == "Gérer la bibliothèque")
                {
                    btnMenuItem.Click += (s, e) => OpenLibraryManagement();
                }
                else if (item == "Notifications")
                {
                    btnMenuItem.Click += (s, e) => OpenNotifications();
                }
                else if (item == "Historique")
                {
                    btnMenuItem.Click += (s, e) => OpenHistory();
                }
                else if (item == "Paramètres")
                {
                    btnMenuItem.Click += (s, e) => OpenSettings();
                }
                else if (item == "Gérer les emprunts")
                {
                    btnMenuItem.Click += (s, e) => OpenLoanManagement();
                }
                
                // Ajouter un icône selon le menu item
                try 
                {
                    string iconName = "";
                    switch (item)
                    {
                        case "Tableau de bord": iconName = "dashboard"; break;
                        case "Gérer la bibliothèque": iconName = "book"; break;
                        case "Gérer les étudiants": iconName = "profil"; break;
                        case "Notifications": iconName = "notifications"; break;
                        case "Historique": iconName = "history"; break;
                        case "Paramètres": iconName = "settings"; break;
                    }
                    
                    // Utiliser IconHelper pour charger l'icône
                    Image iconImage = IconHelper.LoadIcon(iconName);
                    if (iconImage != null)
                    {
                        PictureBox iconBox = new PictureBox
                        {
                            Size = new Size(20, 20),
                            SizeMode = PictureBoxSizeMode.Zoom,
                            BackColor = Color.Transparent,
                            Location = new Point(10, 10),
                            Image = iconImage
                        };
                        btnMenuItem.Controls.Add(iconBox);
                    }
                }
                catch (Exception) { /* Ignorer les erreurs d'icônes */ }
                
                sidebarPanel.Controls.Add(btnMenuItem);
                menuY += 40;
            }
            
            // Section des mots-clés favoris
            Label lblFavoris = new Label
            {
                Text = "Mots-clés favoris",
                ForeColor = Color.Silver,
                Font = new Font("Poppins", 10, FontStyle.Regular),
                Location = new Point(10, menuY + 20),
                Size = new Size(180, 20)
            };
            
            // Ajouter quelques mots-clés favoris
            string[] favorites = { "Économie", "Finances", "Informatiques" };
            int favY = menuY + 50;
            Color[] dotColors = { Color.Blue, Color.Red, Color.Green };
            
            for (int i = 0; i < favorites.Length; i++)
            {
                Panel dotPanel = new Panel
                {
                    Size = new Size(8, 8),
                    Location = new Point(15, favY + 6),
                    BackColor = dotColors[i]
                };
                
                dotPanel.Paint += (s, e) =>
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    using (SolidBrush brush = new SolidBrush(((Panel)s).BackColor))
                    {
                        e.Graphics.FillEllipse(brush, 0, 0, 8, 8);
                    }
                };
                
                Label lblFavorite = new Label
                {
                    Text = favorites[i],
                    ForeColor = Color.White,
                    Font = new Font("Poppins", 9, FontStyle.Regular),
                    Location = new Point(30, favY),
                    Size = new Size(150, 20)
                };
                
                sidebarPanel.Controls.Add(dotPanel);
                sidebarPanel.Controls.Add(lblFavorite);
                favY += 25;
            }
            
            // Ajouter les contrôles à la sidebar
            userInfoPanel.Controls.Add(lblUserName);
            userInfoPanel.Controls.Add(lblUserEmail);
            userInfoPanel.Controls.Add(avatarBox);
            
            sidebarPanel.Controls.Add(logoBox);
            sidebarPanel.Controls.Add(userInfoPanel);
            sidebarPanel.Controls.Add(lblFavoris);
        }
        
        /// <summary>
        /// Gestionnaire d'événement commun pour tous les boutons du menu
        /// </summary>
        private void BtnMenuItem_Click(object sender, EventArgs e)
        {
            // Récupérer le bouton qui a été cliqué
            Button clickedButton = sender as Button;
            
            if (clickedButton != null && clickedButton.Tag != null)
            {
                // Récupérer le nom de l'item de menu à partir de la propriété Tag
                string menuItem = clickedButton.Tag.ToString();
                
                try
                {
                    // Traiter le clic en fonction de l'item de menu
                    switch (menuItem)
                    {
                        case "Tableau de bord":
                            // Déjà sur le tableau de bord, ne rien faire
                            break;
                            
                        case "Gérer la bibliothèque":
                            // Ouvrir le formulaire de gestion de la bibliothèque
                            this.Hide();
                            var libraryForm = new LibraryManagementForm(_context);
                            libraryForm.Show();
                            break;
                            
                        case "Gérer les étudiants":
                            // Ouvrir le formulaire de gestion des étudiants
                            this.Hide();
                            var studentForm = new StudentManagementForm2(_context);
                            studentForm.Show();
                            break;
                            
                        case "Notifications":
                            // Ouvrir le formulaire de notifications
                            this.Hide();
                            var notificationsForm = new NotificationsForm(_context);
                            notificationsForm.Show();
                            break;
                            
                        case "Historique":
                            // Ouvrir le formulaire d'historique
                            this.Hide();
                            var historyForm = new HistoryForm(_context);
                            historyForm.Show();
                            break;
                            
                        case "Paramètres":
                            // Ouvrir le formulaire de paramètres
                            this.Hide();
                            var settingsForm = new SettingsForm(_context);
                            settingsForm.Show();
                            break;
                            
                        default:
                            MessageBox.Show($"Fonctionnalité '{menuItem}' non implémentée", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors de l'ouverture de '{menuItem}' : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        
        /// <summary>
        /// Gestionnaire d'événement pour le bouton "Gérer les étudiants"
        /// </summary>
        private void BtnManageStudents_Click(object sender, EventArgs e)
        {
            try
            {
                // Ouvrir le formulaire de gestion des étudiants
                using (var studentForm = new StudentManagementForm2(_context))
                {
                    studentForm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'ouverture du formulaire de gestion des étudiants : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        /// <summary>
        /// Gestionnaire d'événement pour le bouton "Gérer la bibliothèque"
        /// </summary>
        private void BtnManageLibrary_Click(object sender, EventArgs e)
        {
            try
            {
                // Ouvrir le formulaire de gestion de la bibliothèque
                using (var libraryForm = new LibraryManagementForm(_context))
                {
                    libraryForm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'ouverture du formulaire de gestion de la bibliothèque : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        /// <summary>
        /// Ouvre le formulaire de gestion des étudiants
        /// </summary>
        private void OpenStudentManagement()
        {
            try
            {
                // Créer et afficher le formulaire de gestion des étudiants
                var studentForm = new StudentManagementForm(_context);
                studentForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'ouverture du formulaire de gestion des étudiants : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Ouvre le formulaire de gestion de la bibliothèque
        /// </summary>
        private void OpenLibraryManagement()
        {
            try
            {
                // Créer et afficher le formulaire de gestion de la bibliothèque
                var libraryForm = new LibraryManagementForm(_context);
                libraryForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'ouverture du formulaire de gestion de la bibliothèque : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Ouvre le formulaire des notifications
        /// </summary>
        private void OpenNotifications()
        {
            try
            {
                // Créer et afficher le formulaire des notifications
                var notificationsForm = new NotificationsForm(_context);
                notificationsForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'ouverture du formulaire des notifications : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Ouvre le formulaire de l'historique
        /// </summary>
        private void OpenHistory()
        {
            try
            {
                // Créer et afficher le formulaire de l'historique
                var historyForm = new HistoryForm(_context);
                historyForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'ouverture du formulaire de l'historique : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Ouvre le formulaire des paramètres
        /// </summary>
        private void OpenSettings()
        {
            try
            {
                // Créer et afficher le formulaire des paramètres
                var settingsForm = new SettingsForm(_context);
                settingsForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'ouverture du formulaire des paramètres : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        /// <summary>
        /// Ouvre le formulaire de gestion des emprunts
        /// </summary>
        private void OpenLoanManagement()
        {
            try
            {
                // Créer et afficher le formulaire de gestion des emprunts
                var loanForm = new LoanManagementForm(_context);
                loanForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'ouverture du formulaire de gestion des emprunts : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
