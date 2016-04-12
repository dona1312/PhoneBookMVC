using PhoneBook.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PhoneBook.ViewModels.PhoneVM
{
    public class PhoneEditVM
    {
        public int ID { get; set; }
        public int ContactID { get; set; }
        public string Type { get; set; }
        [Display(Name = "Phone Type")]
        public PhoneTypeEnum PhoneType { get; set; }
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Numbers only")]
        [Required(ErrorMessage = "Required field")]
        public string Number { get; set; }
    }
}