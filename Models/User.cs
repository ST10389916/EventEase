using System.ComponentModel.DataAnnotations;

namespace EventEase.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public string Role { get; set; } = "Admin";
    }
}
