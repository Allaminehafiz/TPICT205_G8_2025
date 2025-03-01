using BCrypt.Net;
using System.Threading.Tasks;
using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        
        public AuthController(AppDbContext context)
        {
            _context = context;
        }
        
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
            {
                return BadRequest("Le nom d'utilisateur existe déjà.");
            }

            var user = new User
            {
                Nom = dto.Nom,
                Username = dto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = dto.Role,
                Email = dto.Email,
                Matricule = dto.Matricule,
                Niveau = dto.Niveau,
                Specialite = dto.Specialite,
                StatutDeliberation = "en cours"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("Inscription réussie. Veuillez vous connecter.");
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            User user = null;
            if(dto.Role == "Etudiant")
            {
                user = await _context.Users.FirstOrDefaultAsync(u => 
                    (u.Username == dto.Identifier || u.Matricule == dto.Identifier) 
                    && u.Role == "Etudiant");
            }
            else
            {
                user = await _context.Users.FirstOrDefaultAsync(u => 
                    (u.Username == dto.Identifier || u.Email == dto.Identifier) 
                    && u.Role == dto.Role);
            }
            
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            {
                return Unauthorized("Identifiants incorrects.");
            }
            
            return Ok(new 
            {
                user.Id,
                user.Nom,
                user.Username,
                user.Role,
                user.Niveau,
                user.Specialite,
                user.Matricule,
                user.StatutDeliberation
            });
        }
    }
}
