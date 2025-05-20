using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.API.Data;
using Project.API.Models;
using Project.API.Entities;

namespace Project.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegistrationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RegistrationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST api/registrations
        [HttpPost]
        public async Task<IActionResult> CreateRegistration(RegistrationDto registrationDto)
        {
            try
            {
                // Validate input
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Vérifier si l'événement existe
                var eventEntity = await _context.Events.FindAsync(registrationDto.EventId);
                if (eventEntity == null)
                {
                    return BadRequest("Event not found");
                }

                // Vérifier si l'utilisateur existe (optional but recommended)
                var userExists = await _context.Users.AnyAsync(u => u.Id == registrationDto.UserId);
                if (!userExists)
                {
                    return BadRequest("User not found");
                }

                // Vérifier si l'utilisateur n'est pas déjà inscrit à cet événement
                var existingRegistration = await _context.Registrations
                    .AnyAsync(r => r.UserId == registrationDto.UserId && r.EventId == registrationDto.EventId);

                if (existingRegistration)
                {
                    return BadRequest("You are already registered for this event");
                }

                // Créer une nouvelle inscription
                var registration = new Registration
                {
                    Id = Guid.NewGuid(),
                    FirstName = registrationDto.FirstName,
                    LastName = registrationDto.LastName,
                    Email = registrationDto.Email,
                    Phone = registrationDto.Phone,
                    EventId = registrationDto.EventId,
                    UserId = registrationDto.UserId, // Add this line
                    RegistrationDate = DateTime.UtcNow
                };

                _context.Registrations.Add(registration);
                await _context.SaveChangesAsync();

                // Récupérer la registration avec l'événement et l'utilisateur pour la réponse
                var registrationWithDetails = await _context.Registrations
                    .Include(r => r.Event)
                    .Include(r => r.User) // Include user details
                    .FirstOrDefaultAsync(r => r.Id == registration.Id);

                var response = new RegistrationResponseDto
                {
                    Id = registrationWithDetails.Id,
                    FirstName = registrationWithDetails.FirstName,
                    LastName = registrationWithDetails.LastName,
                    Email = registrationWithDetails.Email,
                    Phone = registrationWithDetails.Phone,
                    EventId = registrationWithDetails.EventId,
                    EventName = registrationWithDetails.Event.Name,
                    UserId = registrationWithDetails.UserId,
                    UserName = registrationWithDetails.User != null ?
                        $"{registrationWithDetails.User.FirstName} {registrationWithDetails.User.LastName}" :
                        $"{registrationWithDetails.FirstName} {registrationWithDetails.LastName}",
                    RegistrationDate = registrationWithDetails.RegistrationDate
                };

                return Ok(response);
            }
            catch (DbUpdateException dbEx)
            {
                // Log the specific database error
                // _logger.LogError(dbEx, "Database error during registration creation");
                return StatusCode(500, "Database error occurred while processing your registration");
            }
            catch (Exception ex)
            {
                // Log the general error
                // _logger.LogError(ex, "Error during registration creation");
                return StatusCode(500, $"An error occurred while processing your registration: {ex.Message}");
            }
        }

        // GET api/registrations
        [HttpGet]
        public async Task<IActionResult> GetAllRegistrations()
        {
            try
            {
                var registrations = await _context.Registrations
                    .Include(r => r.Event)
                    .Include(r => r.User)
                    .Select(r => new RegistrationResponseDto
                    {
                        Id = r.Id,
                        FirstName = r.FirstName,
                        LastName = r.LastName,
                        Email = r.Email,
                        Phone = r.Phone,
                        EventId = r.EventId,
                        EventName = r.Event.Name,
                        UserId = r.UserId,
                        UserName = r.User != null ?
                            $"{r.User.FirstName} {r.User.LastName}" :
                            $"{r.FirstName} {r.LastName}",
                        RegistrationDate = r.RegistrationDate
                    })
                    .ToListAsync();

                return Ok(registrations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving registrations");
            }
        }

        // GET api/registrations/event/{eventId}
        [HttpGet("event/{eventId}")]
        public async Task<IActionResult> GetRegistrationsByEvent(Guid eventId)
        {
            try
            {
                var registrations = await _context.Registrations
                    .Include(r => r.Event)
                    .Include(r => r.User)
                    .Where(r => r.EventId == eventId)
                    .Select(r => new RegistrationResponseDto
                    {
                        Id = r.Id,
                        FirstName = r.FirstName,
                        LastName = r.LastName,
                        Email = r.Email,
                        Phone = r.Phone,
                        EventId = r.EventId,
                        EventName = r.Event.Name,
                        UserId = r.UserId,
                        UserName = r.User != null ?
                            $"{r.User.FirstName} {r.User.LastName}" :
                            $"{r.FirstName} {r.LastName}",
                        RegistrationDate = r.RegistrationDate
                    })
                    .ToListAsync();

                return Ok(registrations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving registrations");
            }
        }

        // PUT api/registrations/{id}/confirm
        [HttpPut("{id}/confirm")]
        public async Task<IActionResult> ConfirmRegistration(Guid id)
        {
            try
            {
                var registration = await _context.Registrations.FindAsync(id);
                if (registration == null)
                {
                    return NotFound("Registration not found");
                }

                // Add confirmation logic here if needed
                // For example: registration.IsConfirmed = true;

                await _context.SaveChangesAsync();

                return Ok("Registration confirmed successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while confirming registration");
            }
        }
    }
}