using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.API.Entities
{
    public class Registration
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Phone]
        [StringLength(20)]
        public string? Phone { get; set; }

        [Required]
        public Guid EventId { get; set; }

        [ForeignKey("EventId")]
        public virtual Event Event { get; set; } = null!;

        // Corrected User relationship
        [Required]
        public Guid UserId { get; set; }

        [ForeignKey("UserId")]  // Changed from [ForeignKey("Id")] to [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        [Required]
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

    }
}