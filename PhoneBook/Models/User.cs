﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhoneBook.Models
{
    public class User:BaseModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string RememberMeHash { get; set; }
        public DateTime DateExpire { get; set; }

        public virtual List<Contact> Contacts { get; set; }
        

    }
}