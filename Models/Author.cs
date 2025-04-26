using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace projet_bibliotheque.Models
{
    public class Author
    {
        public Author()
        {
            Books = new HashSet<Book>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public DateTime? Birthdate { get; set; }

        [StringLength(50)]
        public string? Nationality { get; set; }

        // Navigation property
        public virtual ICollection<Book> Books { get; set; } = new HashSet<Book>();

        // Override ToString for proper display
        public override string ToString()
        {
            return $"{Name} ({Nationality ?? "Nationalité inconnue"})";
        }
    }
}