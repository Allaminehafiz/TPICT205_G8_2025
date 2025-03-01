using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class JuryAdjustment
    {
        public int Id { get; set; }

        [Required]
        public string Niveau { get; set; } = string.Empty; // Niveau concerné

        [Required]
        public int UnitId { get; set; }
        [ForeignKey("UnitId")]
        public Unit Unit { get; set; } = null!;

        [Required]
        public decimal PointsAdded { get; set; } // Points additionnels
    }
}
