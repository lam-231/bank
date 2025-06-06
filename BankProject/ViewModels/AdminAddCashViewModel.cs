using System.ComponentModel.DataAnnotations;

namespace BankProject.ViewModels
{
    public class AdminAddCashViewModel
    {
        [Required(ErrorMessage = "Вкажіть суму")]
        [Range(0.01, 100000000, ErrorMessage = "Сума має бути більшою за 0")]
        [Display(Name = "Сума для додавання")]
        public decimal Amount { get; set; }
    }
}
