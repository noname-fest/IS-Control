using System.ComponentModel.DataAnnotations;

namespace IS_Control.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Не указан Имя пользователя")]
        public string username { get; set; }
         
        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        public string userpassword { get; set; }
    }
}