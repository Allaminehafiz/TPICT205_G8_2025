using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
    public class NoteEntryDto
    {
        [Required]
        public int UnitId { get; set; }
        
        [Required]
        public decimal Note { get; set; }
    }

    public class UpdateStudentNotesDto
    {
        [Required]
        public string Matricule { get; set; } = string.Empty;
        
        [Required]
        public List<NoteEntryDto> Notes { get; set; } = new List<NoteEntryDto>();
    }
}
