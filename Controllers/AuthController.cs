using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Project.API.Data;
using Project.API.Models;
using Project.API.Services;
using Project.API.Entities;
using Microsoft.EntityFrameworkCore;


namespace Project.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        //[Authorize(Roles = "admin")]
        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterAdmin(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (registerDto.Role.ToLower() != "admin")
            {
                return BadRequest("Only admin registration allowed on this endpoint.");
            }

            var result = await _authService.RegisterAsync(registerDto);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Reject any attempt to register as admin via this endpoint
      
  

            var result = await _authService.RegisterAsync(registerDto);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.LoginAsync(loginDto);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);
        }
        //[Authorize(Roles = "admin")]
        [ApiController]
        [Route("api/[controller]")]
        public class EventsController : ControllerBase
        {
            private readonly ApplicationDbContext _context;
            public EventsController(ApplicationDbContext context) => _context = context;

            [HttpGet]
            public async Task<IActionResult> GetAllEvents() => Ok(await _context.Events.ToListAsync());

            [HttpPost]
            public async Task<IActionResult> CreateEvent(EventDto eventDto)
            {
                var newEvent = new Event
                {
                    Id = Guid.NewGuid(),
                    Name = eventDto.Name,
                    Location = eventDto.Location,
                    StartTime = eventDto.StartTime,
                    EndTime = eventDto.EndTime,
                    // other props
                };
                _context.Events.Add(newEvent);
                await _context.SaveChangesAsync();
                return Ok(newEvent);
            }

            [HttpPut("{id}")]
            public async Task<IActionResult> UpdateEvent(Guid id, EventDto eventDto)
            {
                var existingEvent = await _context.Events.FindAsync(id);
                if (existingEvent == null) return NotFound();

                existingEvent.Name = eventDto.Name;
                existingEvent.Location = eventDto.Location;
                existingEvent.StartTime = eventDto.StartTime;
                existingEvent.EndTime = eventDto.EndTime;
                // other props

                await _context.SaveChangesAsync();
                return Ok(existingEvent);
            }

            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteEvent(Guid id)
            {
                var ev = await _context.Events.FindAsync(id);
                if (ev == null) return NotFound();

                _context.Events.Remove(ev);
                await _context.SaveChangesAsync();
                return NoContent();
            }
        }

    }
}