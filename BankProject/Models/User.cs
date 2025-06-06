using System.ComponentModel.DataAnnotations;

namespace BankProject.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        [Required, MaxLength(50)]
        public string Surname { get; set; }

        [Required, Phone]
        public string Phone { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MaxLength(100)]
        public string Address { get; set; }

        [Required, Range(18, 120)]
        public int Age { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public Card Card { get; set; }
    }
}
