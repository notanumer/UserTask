using System.ComponentModel.DataAnnotations;

namespace Core.Dto
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Введите номер телефона")]
        [MaxLength(11, ErrorMessage = "Номер телефона превышает допустимое число знаков")]
        [RegularExpression("^((7)+([0-9]){10})$", ErrorMessage = "Номер телефона должен начинаться с 7 и содеражать только цифры")]
        public string Phone { get; set; }

        [MaxLength(20)]
        public string Password { get; set; }
    }
}
