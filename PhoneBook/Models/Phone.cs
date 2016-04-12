using PhoneBook.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhoneBook.Models
{
    public class Phone:BaseModel
    {
        public int ContactID { get; set; }
        public PhoneTypeEnum PhoneType { get; set; }
        public string Number { get; set; }

        public virtual Contact Contact { get; set; }
    }
}