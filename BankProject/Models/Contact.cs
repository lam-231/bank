using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BankProject.Models
{
    public class Contact
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        [Required]
        [MaxLength(50)]
        public string ContactName { get; set; }

        [Required]
        [StringLength(12, MinimumLength = 12)]
        [RegularExpression(@"^\d{12}$", ErrorMessage = "Номер картки має складатися з 12 цифр")]
        public string ContactCardNumber { get; set; }
    }
}
