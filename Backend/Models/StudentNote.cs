using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class StudentNote
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        [Required]
        public int UnitId { get; set; }
        [ForeignKey("UnitId")]
        public Unit Unit { get; set; } = null!;

        [Required]
        public decimal Note { get; set; } // Note sur 100
    }
}
