﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhoneBook.Models
{
    public class Group:BaseModel
    {
        public string Name { get; set; }
        
        public virtual List<Contact> Contacts { get; set; }

    }
}