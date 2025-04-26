using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.IO;
using projet_bibliotheque.Models;

namespace projet_bibliotheque.Forms
{
    public partial class ChatbotForm : Form
    {
        private Member currentUser;
        private Panel chatPanel;
        private TextBox messageBox;
        private Button sendButton;
        private FlowLayoutPanel messagesPanel;
        
        // Couleurs de l'application
        private readonly Color PrimaryColor = Color.FromArgb(8, 15, 40);  // Bleu foncé
        private readonly Color AccentColor = Color.FromArgb(45, 66, 132); // Bleu accent
        private readonly Color TextColor = Color.White;
        private readonly Color UserMessageColor = Color.FromArgb(45, 66, 132);
        private readonly Color BotMessageColor = Color.FromArgb(60, 60, 60);
        
        public ChatbotForm(Member user = null)
        {
            InitializeComponent();
            this.currentUser = user;
            SetupForm();
        }
        
        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Name = "ChatbotForm";
            this.Text = "BiblioHub - Assistant Virtuel";
            this.ResumeLayout(false);
        }
        
        private void SetupForm()
        {
            this.BackColor = Color.WhiteSmoke;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Dock = DockStyle.Fill;
            
            // Titre
            Label titleLabel = new Label
            {
                Text = "Assistant Virtuel BiblioHub",
                Font = new Font("Poppins", 18, FontStyle.Bold),
                ForeColor = PrimaryColor,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 60,
                Padding = new Padding(0, 10, 0, 0)
            };
            
            // Description
            Label descriptionLabel = new Label
            {
                Text = "Posez vos questions sur la bibliothèque, les livres, ou comment utiliser l'application.",
                Font = new Font("Poppins", 10, FontStyle.Regular),
                ForeColor = Color.DimGray,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 30
            };
            
            // Panel principal pour le chat
            chatPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20)
            };
            
            // Panel pour afficher les messages
            messagesPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                AutoScroll = true,
                WrapContents = false,
                Padding = new Padding(10)
            };
            
            // Panel pour la saisie de message
            Panel inputPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                Padding = new Padding(10)
            };
            
            // Champ de saisie
            messageBox = new TextBox
            {
                Font = new Font("Poppins", 11),
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(10, 5, 10, 5)
            };
            messageBox.KeyPress += (s, e) => {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    e.Handled = true;
                    SendMessage();
                }
            };
            
            // Bouton d'envoi
            sendButton = new Button
            {
                Text = "Envoyer",
                Font = new Font("Poppins", 10),
                ForeColor = Color.White,
                BackColor = AccentColor,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(100, 40),
                Dock = DockStyle.Right,
                Margin = new Padding(10, 0, 0, 0),
                Cursor = Cursors.Hand
            };
            sendButton.FlatAppearance.BorderSize = 0;
            sendButton.Click += (s, e) => SendMessage();
            
            // Ajouter les contrôles
            inputPanel.Controls.Add(messageBox);
            inputPanel.Controls.Add(sendButton);
            
            chatPanel.Controls.Add(messagesPanel);
            
            this.Controls.Add(chatPanel);
            this.Controls.Add(inputPanel);
            this.Controls.Add(descriptionLabel);
            this.Controls.Add(titleLabel);
            
            // Ajouter un message de bienvenue
            AddBotMessage("Bonjour ! Je suis l'assistant virtuel de BiblioHub. Comment puis-je vous aider aujourd'hui ?");
        }
        
        private void SendMessage()
        {
            string message = messageBox.Text.Trim();
            if (!string.IsNullOrEmpty(message))
            {
                AddUserMessage(message);
                messageBox.Clear();
                
                // Simuler une réponse du chatbot
                ProcessMessage(message);
            }
        }
        
        private void AddUserMessage(string message)
        {
            Panel messageContainer = CreateMessagePanel(message, true);
            messagesPanel.Controls.Add(messageContainer);
            ScrollToBottom();
        }
        
        private void AddBotMessage(string message)
        {
            Panel messageContainer = CreateMessagePanel(message, false);
            messagesPanel.Controls.Add(messageContainer);
            ScrollToBottom();
        }
        
        private Panel CreateMessagePanel(string message, bool isUser)
        {
            Panel container = new Panel
            {
                AutoSize = true,
                Width = messagesPanel.Width - 40,
                Margin = new Padding(5),
                Padding = new Padding(10)
            };
            
            Label messageLabel = new Label
            {
                Text = message,
                Font = new Font("Poppins", 10),
                ForeColor = isUser ? Color.White : Color.Black,
                BackColor = isUser ? UserMessageColor : BotMessageColor,
                AutoSize = true,
                MaximumSize = new Size(container.Width - 20, 0),
                Padding = new Padding(10),
                Margin = new Padding(isUser ? 50 : 0, 5, isUser ? 0 : 50, 5)
            };
            
            // Arrondir les coins du message
            messageLabel.Paint += (s, e) =>
            {
                using (GraphicsPath path = RoundRectangle.Create(0, 0, messageLabel.Width, messageLabel.Height, 10))
                {
                    messageLabel.Region = new Region(path);
                }
            };
            
            container.Controls.Add(messageLabel);
            
            // Aligner le message à droite si c'est un message utilisateur
            if (isUser)
            {
                messageLabel.Dock = DockStyle.Right;
            }
            else
            {
                messageLabel.Dock = DockStyle.Left;
            }
            
            return container;
        }
        
        private void ScrollToBottom()
        {
            messagesPanel.VerticalScroll.Value = messagesPanel.VerticalScroll.Maximum;
            messagesPanel.PerformLayout();
        }
        
        private void ProcessMessage(string message)
        {
            // Simuler un délai de réponse
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;
            timer.Tick += (s, e) =>
            {
                timer.Stop();
                
                // Réponses simples basées sur des mots-clés
                message = message.ToLower();
                
                if (message.Contains("bonjour") || message.Contains("salut") || message.Contains("hello"))
                {
                    AddBotMessage("Bonjour ! Comment puis-je vous aider aujourd'hui ?");
                }
                else if (message.Contains("livre") && (message.Contains("emprunter") || message.Contains("prêt") || message.Contains("emprunt")))
                {
                    AddBotMessage("Pour emprunter un livre, rendez-vous dans la section 'Emprunter un livre' depuis votre espace étudiant. Vous pouvez rechercher un livre par titre, auteur ou catégorie, puis cliquer sur 'Emprunter'.");
                }
                else if (message.Contains("retard") || message.Contains("retour") || (message.Contains("rendre") && message.Contains("livre")))
                {
                    AddBotMessage("Si vous êtes en retard pour rendre un livre, vous recevrez une notification. Veuillez rapporter le livre à la bibliothèque dès que possible pour éviter des pénalités supplémentaires.");
                }
                else if (message.Contains("compte") && (message.Contains("créer") || message.Contains("inscription")))
                {
                    AddBotMessage("Pour créer un compte, cliquez sur 'S'inscrire' sur la page d'accueil. Remplissez le formulaire avec vos informations personnelles et choisissez votre statut (étudiant ou bibliothécaire).");
                }
                else if (message.Contains("mot de passe") && (message.Contains("oublié") || message.Contains("perdu") || message.Contains("réinitialiser")))
                {
                    AddBotMessage("Si vous avez oublié votre mot de passe, contactez un administrateur de la bibliothèque pour le réinitialiser.");
                }
                else if (message.Contains("horaire") || message.Contains("heure") || message.Contains("ouverture"))
                {
                    AddBotMessage("La bibliothèque est ouverte du lundi au vendredi de 8h à 18h, et le samedi de 9h à 12h. Elle est fermée le dimanche et les jours fériés.");
                }
                else if (message.Contains("contact") || message.Contains("aide") || message.Contains("assistance"))
                {
                    AddBotMessage("Pour contacter l'équipe de la bibliothèque, vous pouvez envoyer un email à bibliotheque@ihec.tn ou appeler le +216 71 123 456 pendant les heures d'ouverture.");
                }
                else if (message.Contains("merci") || message.Contains("thanks"))
                {
                    AddBotMessage("Je vous en prie ! N'hésitez pas si vous avez d'autres questions.");
                }
                else if (message.Contains("au revoir") || message.Contains("bye"))
                {
                    AddBotMessage("Au revoir ! Bonne journée et à bientôt !");
                }
                else
                {
                    AddBotMessage("Je ne suis pas sûr de comprendre votre demande. Pourriez-vous reformuler ou poser une question plus précise sur la bibliothèque ou l'application ?");
                }
            };
            timer.Start();
        }
    }
    
    // Classe utilitaire pour créer des rectangles arrondis
    public static class RoundRectangle
    {
        public static GraphicsPath Create(int x, int y, int width, int height, int radius)
        {
            int diameter = radius * 2;
            Size size = new Size(diameter, diameter);
            Rectangle arc = new Rectangle(x, y, diameter, diameter);
            GraphicsPath path = new GraphicsPath();

            if (radius == 0)
            {
                path.AddRectangle(new Rectangle(x, y, width, height));
                return path;
            }

            // Top left arc  
            path.AddArc(arc, 180, 90);

            // Top right arc  
            arc.X = x + width - diameter;
            path.AddArc(arc, 270, 90);

            // Bottom right arc  
            arc.Y = y + height - diameter;
            path.AddArc(arc, 0, 90);

            // Bottom left arc 
            arc.X = x;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }
    }
}
