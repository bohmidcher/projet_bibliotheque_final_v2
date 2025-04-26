using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace projet_bibliotheque.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        public DateTime PublicationDate { get; set; }

        [Required]
        [StringLength(50)]
        public string Genre { get; set; } = string.Empty;
        
        [StringLength(50)]
        public string Category { get; set; } = string.Empty;
        
        [StringLength(20)]
        public string Status { get; set; } = "Disponible";

        public int AuthorId { get; set; }

        [Required]
        [StringLength(13)]
        public string ISBN { get; set; } = string.Empty;

        // Navigation property
        public virtual Author Author { get; set; } = null!;
        
        // Propriété pour faciliter l'affichage du nom de l'auteur
        public string AuthorName { get; set; } = string.Empty;

        // Collection navigation property
        public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();
    }
} 