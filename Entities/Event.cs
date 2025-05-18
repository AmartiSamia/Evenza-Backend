using System;
using System.ComponentModel.DataAnnotations;

namespace Project.API.Entities
{
    public class Event
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Location { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        // Tu peux ajouter d'autres propriétés spécifiques à ton besoin, par exemple :
        // public string Description { get; set; }
        // public string Organizer { get; set; }
    }
}
