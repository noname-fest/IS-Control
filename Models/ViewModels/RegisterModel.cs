using System;
using System.ComponentModel.DataAnnotations;

namespace IS_Control.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage ="Не указана полное Имя пользователя")]
        public string UserFullname { get; set; }

        [Required(ErrorMessage ="Не указан Имя пользователя(логин)")]
        public string username { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        public string userpassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("userpassword", ErrorMessage = "Пароль введен неверно")]
        public string ConfirmPassword { get; set; }

        public string UnitsId { get; set; }
        public string Role { get; set; }

        //[Required(ErrorMessage = "Не указан дата отчетности")]
        //[Column(TypeName = "date")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:MMM yyyy}")]
        //public DateTime reportDt {get;set;} = new DateTime(DateTime.Today.Year,DateTime.Today.Month-1,1) ;
        //public int repDtYear {get;set;}
        //public int repDtMont {get; set;}
    }
}