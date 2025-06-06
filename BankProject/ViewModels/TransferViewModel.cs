using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BankProject.ViewModels
{
    public class TransferViewModel
    {
        [Required(ErrorMessage = "Вкажіть суму переказу")]
        [Range(0.01, 1000000, ErrorMessage = "Сума має бути більшою за 0")]
        [Display(Name = "Сума")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Вкажіть номер картки отримувача")]
        [StringLength(12, MinimumLength = 12, ErrorMessage = "Номер картки має складатися з 12 цифр")]
        [RegularExpression(@"^\d{12}$", ErrorMessage = "Номер картки повинен містити лише цифри")]
        [Display(Name = "Номер картки отримувача")]
        public string DestinationCardNumber { get; set; }

        public List<SelectListItem> AvailableContacts { get; set; } = new();
    }
}
