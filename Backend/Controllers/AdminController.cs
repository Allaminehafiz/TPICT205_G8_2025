using System.Linq;
using System.Threading.Tasks;
using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/admin")]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _context;
        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        // 📝 Mettre à jour les notes d'un étudiant
        [HttpPost("update-notes")]
        public async Task<IActionResult> UpdateStudentNotes(UpdateStudentNotesDto dto)
        {
            var student = await _context.Users.FirstOrDefaultAsync(u => u.Matricule == dto.Matricule && u.Role == "Etudiant");
            if (student == null)
                return NotFound("Étudiant non trouvé.");

            foreach (var noteEntry in dto.Notes)
            {
                var studentNote = await _context.StudentNotes
                    .FirstOrDefaultAsync(sn => sn.UserId == student.Id && sn.UnitId == noteEntry.UnitId);
                if (studentNote != null)
                {
                    studentNote.Note = noteEntry.Note;
                }
                else
                {
                    studentNote = new StudentNote
                    {
                        UserId = student.Id,
                        UnitId = noteEntry.UnitId,
                        Note = noteEntry.Note
                    };
                    _context.StudentNotes.Add(studentNote);
                }
            }

            await _context.SaveChangesAsync();
            return Ok("Notes mises à jour avec succès.");
        }

        // 📄 Télécharger le PV (Procès-Verbal)
        [HttpGet("download-pv/{niveau}")]
        public async Task<IActionResult> DownloadPV(string niveau)
        {
            var students = await _context.Users
                .Where(u => u.Niveau == niveau && u.Role == "Etudiant")
                .Include(u => u.StudentNotes)
                    .ThenInclude(sn => sn.Unit)
                .ToListAsync();

            if (students.Count == 0)
                return NotFound("Aucun étudiant trouvé pour ce niveau.");

            var pendingStudents = students.Where(s => s.StatutDeliberation == "en cours").ToList();
            if (pendingStudents.Any())
                return BadRequest("Tous les statuts doivent être finalisés avant le téléchargement du PV.");

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Procès-Verbal - Niveau {niveau}");
            sb.AppendLine("Nom, Matricule, MGP, Statut");

            foreach (var student in students)
            {
                decimal avg = student.StudentNotes.Count > 0 ? student.StudentNotes.Average(n => n.Note) : 0;
                decimal mgp = avg / 25;
                string status = mgp >= 2 ? "admis" : "échoué";

                sb.AppendLine($"{student.Nom}, {student.Matricule}, {mgp:F2}, {status}");
            }

            var fileName = $"PV_Niveau_{niveau}.csv";
            var fileContent = Encoding.UTF8.GetBytes(sb.ToString());
            return File(fileContent, "text/csv", fileName);
        }
    }
}
