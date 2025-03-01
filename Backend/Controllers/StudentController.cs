using System.Linq;
using System.Threading.Tasks;
using Backend.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/student")]
    public class StudentController : ControllerBase
    {
        private readonly AppDbContext _context;
        public StudentController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("profile/{userId}")]
        public async Task<IActionResult> GetProfile(int userId)
        {
            var user = await _context.Users
                .Include(u => u.StudentNotes)
                    .ThenInclude(sn => sn.Unit)
                .FirstOrDefaultAsync(u => u.Id == userId && u.Role == "Etudiant");

            if (user == null)
                return NotFound("Étudiant non trouvé.");

            var adjustments = await _context.JuryAdjustments
                .Where(j => j.Niveau == user.Niveau)
                .ToListAsync();

            var notesDto = user.StudentNotes.Select(sn =>
            {
                var adjustment = adjustments.FirstOrDefault(j => j.UnitId == sn.UnitId);
                decimal pointsAdded = adjustment != null ? adjustment.PointsAdded : 0;
                return new
                {
                    UnitCode = sn.Unit.UnitCode,
                    Semester = sn.Unit.Semester,
                    Type = sn.Unit.Type,
                    InitialNote = sn.Note,
                    PointsAdded = pointsAdded,
                    FinalNote = sn.Note + pointsAdded
                };
            }).ToList();

            decimal avg = notesDto.Count > 0 ? notesDto.Average(n => n.FinalNote) : 0;
            decimal mgp = avg / 25;
            string status = mgp >= 2 ? "admis" : "échoué";

            return Ok(new
            {
                user.Id,
                user.Nom,
                user.Username,
                user.Matricule,
                user.Niveau,
                user.Specialite,
                Notes = notesDto,
                MGP = mgp,
                StatutDeliberation = status
            });
        }
    }
}
