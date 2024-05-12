using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.DTOs.IdentityDtos
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string DisplayName { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[\W_]).+$", 
            ErrorMessage ="Password must contain 1 Uppercase , 1 Lowercase , 1 digit , 1 Special character")]
        public string Password { get; set; }
    }
}
