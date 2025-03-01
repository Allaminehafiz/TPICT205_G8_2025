using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Backend.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Nom { get; set; } = string.Empty;

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = string.Empty; // "Etudiant", "Administrateur", "Jury"

        public string Matricule { get; set; } = string.Empty; // Pour les étudiants

        public string Niveau { get; set; } = string.Empty;    // Pour les étudiants et le jury

        public string Specialite { get; set; } = string.Empty; // Pour les étudiants de niveau 2 ou plus

        [Required]
        public string StatutDeliberation { get; set; } = "en cours";

        // Relation avec les notes
        public ICollection<StudentNote> StudentNotes { get; set; } = new List<StudentNote>();
    }
}