using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BankProject.Models
{
    public class Card
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, StringLength(12)]
        public string CardNumber { get; set; }

        [Required, StringLength(3)]
        public string CVV { get; set; }

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; } = 0m;

        [Required]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
    }
}
