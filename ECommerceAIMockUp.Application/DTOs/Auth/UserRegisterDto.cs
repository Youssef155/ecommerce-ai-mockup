using System.ComponentModel.DataAnnotations;

namespace ECommerceAIMockUp.Application.DTOs.Auth
{
    public class UserRegisterDto
    {
        [Required]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "Minimum length of first name is {2} and maximum is {1}")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "Minimum length of last name is {2} and maximum is {1}")]
        public string LastName { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "Minimum length of first name is {2} and maximum is {1}")]
        public string Password { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }
    }
}
