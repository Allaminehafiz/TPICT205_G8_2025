using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string Nom { get; set; } = string.Empty;
        
        [Required]
        public string Username { get; set; } = string.Empty;
        
        [Required]
        public string Password { get; set; } = string.Empty;
        
        public string Email { get; set; } = string.Empty;
        
        [Required]
        public string Role { get; set; } = string.Empty; // "Etudiant", "Administrateur", "Jury"
        
        public string Matricule { get; set; } = string.Empty;
        
        public string Niveau { get; set; } = string.Empty;
        
        public string Specialite { get; set; } = string.Empty;
    }
}
