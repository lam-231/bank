using System.ComponentModel.DataAnnotations;

namespace BankProject.ViewModels
{
    public class ATMWithdrawViewModel
    {
        [Required(ErrorMessage = "Вкажіть суму зняття")]
        [Range(0.01, 1000000, ErrorMessage = "Сума має бути більшою за 0")]
        [Display(Name = "Сума для зняття")]
        public decimal Amount { get; set; }
    }
}
