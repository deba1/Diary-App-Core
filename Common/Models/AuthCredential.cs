using System.ComponentModel.DataAnnotations;

namespace Common.Models
{
    public class AuthCredential
    {
        [Required]
        public string Username { get; set; }
        [Required, MinLength(5)]
        public string Password { get; set; }
    }
}
