﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhoneBook.Models
{
    public class Contact:BaseModel
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Adress { get; set; }
        public string ImagePath { get; set; }
        public int CityID { get; set; }

        public virtual City City { get; set; }
        public virtual List<Phone> Phones { get; set; }
        public virtual User User { get; set; }
        public virtual List<Group> Groups { get; set; }
    }
}