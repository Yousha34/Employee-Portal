using System.ComponentModel.DataAnnotations;

namespace Employee_Portal.Models
{
    public class Admin
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? FullName { get; set; }
    }
}