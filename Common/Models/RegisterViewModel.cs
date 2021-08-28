using System.ComponentModel.DataAnnotations;

namespace Common.Models
{
    public class RegisterViewModel
    {
        [Required, MinLength(4), MaxLength(50)]
        public string Username { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required, MinLength(5), MaxLength(50)]
        public string Password { get; set; }
    }
}
