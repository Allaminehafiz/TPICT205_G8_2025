using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
    public class JuryAdjustmentEntryDto
    {
        [Required]
        public int UnitId { get; set; }
        
        [Required]
        public decimal PointsToAdd { get; set; }
    }

    public class JuryAdjustmentDto
    {
        [Required]
        public string Niveau { get; set; } = string.Empty;
        
        [Required]
        public List<JuryAdjustmentEntryDto> Adjustments { get; set; } = new List<JuryAdjustmentEntryDto>();
    }
}
