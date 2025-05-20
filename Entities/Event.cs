using System;
using System.ComponentModel.DataAnnotations;

namespace Project.API.Entities
{
    public class Event
    {
        [Key]
        public Guid Id { get; set; }           // ✅ Parfait pour l'identifiant unique
        [Required]
        [StringLength(100)]
        public string Name { get; set; }       // ✅ Correspond au nom de l'événement
        [StringLength(200)]
        public string? Location { get; set; }  // ✅ Correspond à la localisation
        [Required]
        public DateTime StartTime { get; set; } // ✅ Pour la date/heure de début
        [Required]
        public DateTime EndTime { get; set; }   // ✅ Pour la date/heure de fin
    }
}
