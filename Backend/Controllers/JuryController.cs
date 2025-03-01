using System.Linq;
using System.Threading.Tasks;
using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/jury")]
    public class JuryController : ControllerBase
    {
        private readonly AppDbContext _context;
        public JuryController(AppDbContext context)
        {
            _context = context;
        }

        // ➕ Ajouter des ajustements
        [HttpPost("add-adjustments")]
        public async Task<IActionResult> AddAdjustments(JuryAdjustmentDto dto)
        {
            foreach (var adjustmentEntry in dto.Adjustments)
            {
                var existingAdjustment = await _context.JuryAdjustments
                    .FirstOrDefaultAsync(j => j.Niveau == dto.Niveau && j.UnitId == adjustmentEntry.UnitId);
                if (existingAdjustment != null)
                {
                    existingAdjustment.PointsAdded += adjustmentEntry.PointsToAdd;
                }
                else
                {
                    var newAdjustment = new JuryAdjustment
                    {
                        Niveau = dto.Niveau,
                        UnitId = adjustmentEntry.UnitId,
                        PointsAdded = adjustmentEntry.PointsToAdd
                    };
                    _context.JuryAdjustments.Add(newAdjustment);
                }
            }
            await _context.SaveChangesAsync();
            return Ok("Ajustements appliqués avec succès.");
        }

        // 📋 Voir la liste des étudiants d’un niveau
        [HttpGet("students/{niveau}")]
        public async Task<IActionResult> GetStudents(string niveau)
        {
            var students = await _context.Users
                .Where(u => u.Niveau == niveau && u.Role == "Etudiant")
                .Include(u => u.StudentNotes)
                    .ThenInclude(sn => sn.Unit)
                .ToListAsync();
            return Ok(students);
        }
    }
}
