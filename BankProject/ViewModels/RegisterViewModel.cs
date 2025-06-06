using System.ComponentModel.DataAnnotations;

namespace BankProject.ViewModels
{
    public class RegisterViewModel
    {
        [Required, Display(Name = "Ім’я"), MaxLength(50)]
        public string Name { get; set; }

        [Required, Display(Name = "Прізвище"), MaxLength(50)]
        public string Surname { get; set; }

        [Required, Phone, Display(Name = "Телефон")]
        public string Phone { get; set; }

        [Required, EmailAddress, Display(Name = "Електронна пошта")]
        public string Email { get; set; }

        [Required, Display(Name = "Місце проживання"), MaxLength(100)]
        public string Address { get; set; }

        [Required, Range(18, 120), Display(Name = "Вік")]
        public int Age { get; set; }

        [Required, StringLength(4, MinimumLength = 4), DataType(DataType.Password), Display(Name = "Пароль (4 цифри)")]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "Пароль має складатися з 4 цифр")]
        public string Password { get; set; }

        [Required, DataType(DataType.Password), Display(Name = "Підтвердження паролю")]
        [Compare("Password", ErrorMessage = "Паролі не співпадають")]
        public string ConfirmPassword { get; set; }
    }
}
