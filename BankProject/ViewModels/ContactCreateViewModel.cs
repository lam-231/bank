using System.ComponentModel.DataAnnotations;

namespace BankProject.ViewModels
{
    public class ContactCreateViewModel
    {
        [Required]
        [MaxLength(50)]
        [Display(Name = "Ім’я контакту")]
        public string ContactName { get; set; }

        [Required]
        [StringLength(12, MinimumLength = 12)]
        [RegularExpression(@"^\d{12}$", ErrorMessage = "Номер картки має складатися з 12 цифр")]
        [Display(Name = "Номер картки контакту (12 цифр)")]
        public string ContactCardNumber { get; set; }
    }
}
