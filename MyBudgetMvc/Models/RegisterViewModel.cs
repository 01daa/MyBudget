using System.ComponentModel.DataAnnotations;

namespace MyBudgetMvc.Models
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(50)]
        [Display(Name = "Ім'я користувача")]
        public string UserName { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "ПІБ")]
        public string FullName { get; set; }

        [Required]
        [StringLength(16, MinimumLength = 8)]
        [RegularExpression(@"^(?=.*[0-9])(?=.*[A-Z])(?=.*\W).{8,16}$",
            ErrorMessage = "Пароль має містити 8-16 символів, щонайменше одну цифру, велику літеру та спеціальний знак.")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Паролі не збігаються.")]
        [Display(Name = "Підтвердження пароля")]
        public string ConfirmPassword { get; set; }

        [Required]
        [RegularExpression(@"^\+380\d{9}$", ErrorMessage = "Телефон має бути у форматі +380XXXXXXXXX.")]
        [Display(Name = "Телефон")]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
