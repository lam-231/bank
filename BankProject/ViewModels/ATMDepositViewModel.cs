using System.ComponentModel.DataAnnotations;

namespace BankProject.ViewModels
{
    public class ATMDepositViewModel
    {
        [Required(ErrorMessage = "Вкажіть суму депозиту")]
        [Range(0.01, 5000, ErrorMessage = "Сума депозиту має бути між 0.01 та 5000")]
        [Display(Name = "Сума депозиту")]
        public decimal Amount { get; set; }
    }
}
