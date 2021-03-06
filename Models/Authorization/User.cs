﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IS_Control.Models
{
    [Table ("Users")]
    public class User
    {
        [Dapper.Contrib.Extensions.Key]
        public int Id { get; set; }
        
        public int Iduser { get; set; }

        [Display(Name = "Пользователь")]
        [Required(ErrorMessage = "Должно быть не пустым")]
        public string username { get; set; }
        [Display(Name = "Полное имя пользователя")]
        public string UserFullname {get;set;}
        [Display(Name = "Пароль")]
        public string userpassword { get; set; }

        [Display(Name = "Код ПП")]
        public string UnitsId { get; set; }

        [Display(Name = "Роль")]
        public string Role { get; set; }

        //[Display(Name = "Отч. периуд")]
        //[Column(TypeName = "date")]
        //[DisplayFormat(DataFormatString = "{0:MMM yyyy}")]
        //public DateTime reportDt {get; set;}
    }
}