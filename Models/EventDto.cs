using System;
using System.ComponentModel.DataAnnotations;
using Project.API.Entities;
namespace Project.API.Models
{
    public class EventDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Location cannot be longer than 200 characters")]
        public string? Location { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        [DateTimeValidation]
        public DateTime EndTime { get; set; }
    }

    // Custom validation attribute to ensure EndTime is after StartTime
    public class DateTimeValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var eventDto = (EventDto)validationContext.ObjectInstance;

            if (eventDto.EndTime <= eventDto.StartTime)
            {
                return new ValidationResult("End time must be after start time.");
            }

            return ValidationResult.Success;
        }
    }
}
