using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.API.Data;
using Project.API.Models; // Assuming EventDto is here
using Project.API.Entities; // Assuming Event entity is here
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Project.API.Controllers
{
    //[Authorize(Roles = "admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET api/events
        [HttpGet]
        public async Task<IActionResult> GetAllEvents()
        {
            var events = await _context.Events.ToListAsync();
            return Ok(events);
        }

        // POST api/events
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
                // other properties
            };
            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();
            return Ok(newEvent);
        }

        // PUT api/events/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(Guid id, EventDto eventDto)
        {
            var existingEvent = await _context.Events.FindAsync(id);
            if (existingEvent == null)
            {
                return NotFound();
            }

            existingEvent.Name = eventDto.Name;
            existingEvent.Location = eventDto.Location;
            existingEvent.StartTime = eventDto.StartTime;
            existingEvent.EndTime = eventDto.EndTime;
            // other properties update

            await _context.SaveChangesAsync();
            return Ok(existingEvent);
        }

        // DELETE api/events/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(Guid id)
        {
            var ev = await _context.Events.FindAsync(id);
            if (ev == null)
            {
                return NotFound();
            }

            _context.Events.Remove(ev);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
