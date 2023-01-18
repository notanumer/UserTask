using System.ComponentModel.DataAnnotations;

namespace Core.Dto
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Введите ФИО")]
        [MaxLength(250, ErrorMessage = "Слишком длинная строка")]
        public string FIO { get; set; }

        [Required(ErrorMessage = "Введите номер телефона")]
        [MaxLength(11, ErrorMessage = "Номер телефона превышает допустимое число знаков")]
        [RegularExpression("^((7)+([0-9]){10})$", ErrorMessage = "Номер телефона должен начинаться с 7 и содеражать только цифры")]
        public string Phone { get; set; }

        [EmailAddress(ErrorMessage = "Введите корректную эл. почту")]
        public string Email { get; set; }

        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }
    }
}
