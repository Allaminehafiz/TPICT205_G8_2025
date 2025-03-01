using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
    public class LoginDto
    {
        [Required]
        public string Identifier { get; set; } = string.Empty; // Username ou Matricule ou Email
        
        [Required]
        public string Password { get; set; } = string.Empty;
        
        [Required]
        public string Role { get; set; } = string.Empty; // "Etudiant", "Administrateur", "Jury"
    }
}
