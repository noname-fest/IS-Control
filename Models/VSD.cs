using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Dapper.Contrib.Extensions;

namespace IS_Control.Models
{
    [Table ("VSD")]
    public class VSD
    {
        [Key]
        public Guid id {get;set;}
        //принадлежность записи
        public string userId {get;set;}

        [Display(Name = "Форма ВСД")]
        public string VSDFrom   {get;set;}

        [Display(Name = "№ документа")]        
        public string DocNumber    {get;set;}

        [Display(Name = "Дата выдачи")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime dtIssue {get;set;}
        
        [Display(Name = "Организация/Субъект")]
        public string Subject   {get;set;}
        
        [Display(Name = "Представитель")]
        public string Representative {get;set;}

        [Display(Name = "Грузоотправитель")]
        public string Shipper {get;set;}

        [Display(Name = "Грузополучатель")]
        public string Consignee {get;set;}

        [Display(Name = "Вид транспорта")]
        public string TransportID {get;set;} //link

        [Display(Name = "№ транспорта")]
        public string TransportNumber {get;set;}

        [Display(Name = "Товаротранспортные документы")]
        public string ShippingDoc     {get;set;}

        [Display(Name = "Наименование продукции")]
        public string ProductName {get;set;}

        [Display(Name = "ТН ВЭД")]
        public Int32 TnVED {get;set;}

        [Display(Name = "Наименование для печати")]
        public string NameForPrint {get;set;}

        [Display(Name = "Кол-во продукции")]
        [Required(ErrorMessage = "только численное значение")]
        public Int32 AmountOfProduct {get;set;}

        [Display(Name = "Ед.изм")]
        public string EdizmOfProduct {get;set;} //link

        [Display(Name = "Кол-во упаковок")]
        [Required(ErrorMessage = "только численное значение")]
        public Int32 AmountOfPackages {get;set;}

        [Display(Name = "Ед.изм")]
        public string EdizmOfPackages {get;set;} //link

        [Display(Name = "Маркировка")]
        public string Marking {get;set;}

        [Display(Name = "Заключение")] //(реализация без ограничений, с огранич)
        public string Conclusion {get;set;} // link
       
        [Display(Name = "Дополнительная информация")]
        public string Additional {get;set;}

        public static implicit operator VSD(List<VSD> v)
        {
            throw new NotImplementedException();
        }
    }
}