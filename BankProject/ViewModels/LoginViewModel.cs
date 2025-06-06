using System.ComponentModel.DataAnnotations;

namespace BankProject.ViewModels
{
    public class LoginViewModel
    {
        [Required, StringLength(12, MinimumLength = 12), Display(Name = "Номер картки")]
        [RegularExpression(@"^\d{12}$", ErrorMessage = "Номер картки має складатися з 12 цифр")]
        public string CardNumber { get; set; }

        [Required, StringLength(3, MinimumLength = 3), Display(Name = "CVV")]
        [RegularExpression(@"^\d{3}$", ErrorMessage = "CVV має складатися з 3 цифр")]
        public string CVV { get; set; }

        [Required, EmailAddress, Display(Name = "Електронна пошта")]
        public string Email { get; set; }

        [Required, StringLength(4, MinimumLength = 4), DataType(DataType.Password), Display(Name = "Пароль")]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "Пароль має складатися з 4 цифр")]
        public string Password { get; set; }
    }
}
