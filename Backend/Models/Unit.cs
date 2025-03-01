using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Unit
    {
        public int Id { get; set; }

        [Required]
        public string UnitCode { get; set; } = string.Empty; // Ex: "ICT101"

        [Required]
        public int Semester { get; set; } // 1 pour premier semestre, 2 pour deuxième

        [Required]
        public string Type { get; set; } = string.Empty; // "commun" ou "specialite"

        // Relations
        public ICollection<StudentNote> StudentNotes { get; set; } = new List<StudentNote>();
        public ICollection<JuryAdjustment> JuryAdjustments { get; set; } = new List<JuryAdjustment>();
    }
}
