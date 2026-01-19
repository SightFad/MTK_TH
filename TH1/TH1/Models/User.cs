using System.ComponentModel.DataAnnotations;

namespace TH1.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        [StringLength(50)]
        public string Username { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        [Required]
        [StringLength(100)]
        public string Email { get; set; }
        public string Role { get; set; } // "USER" or "ADMIN"
    }
}
