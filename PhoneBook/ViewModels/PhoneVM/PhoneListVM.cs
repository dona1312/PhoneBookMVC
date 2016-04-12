using PhoneBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhoneBook.ViewModels.PhoneVM
{
    public class PhoneListVM
    {
        public List<Phone> Phones { get; set; }
        public Contact Contact{ get; set; }
        public int? ContactID { get; set; }
    }
}