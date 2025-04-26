using System;
using System.Collections.Generic;

namespace projet_bibliotheque.Models
{
    public class Member
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime DateInscription { get; set; } = DateTime.Now;
        public bool IsAdmin { get; set; }
        
        // Propriétés qui causaient l'erreur
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Status { get; set; }
        
        public DateTime? Birthdate { get; set; }
        public string? NiveauEducatif { get; set; }
        public string? Specialite { get; set; }
        
        public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();
    }
}
