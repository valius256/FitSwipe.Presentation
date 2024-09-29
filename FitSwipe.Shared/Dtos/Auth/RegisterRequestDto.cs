
using FitSwipe.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace FitSwipe.Shared.Dtos.Auth
{
    public class RegisterRequestDto
    {
        [Required]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        // [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email format.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string? Password { get; set; }

        // [Required(ErrorMessage = "Phone number is required.")]
        // [RegularExpression(@"(84|0[3|5|7|8|9])[0-9]{8}", ErrorMessage = "Invalid phone number format.")]
        [Required]
        public string? Phone { get; set; }

        // [Required(ErrorMessage = "Date of birth is required.")]
        // [CustomValidation(typeof(DateOfBirthValidator), nameof(DateOfBirthValidator.IsValidDateOfBirth))]
        // public DateTime DateOfBirth { get; set; }

        [Required]
        public Role Role { get; set; }
    }
}
