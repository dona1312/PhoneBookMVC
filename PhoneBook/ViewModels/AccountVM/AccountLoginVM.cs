﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PhoneBook.ViewModels.AccountVM
{
    public class AccountLoginVM
    {
        [Required(ErrorMessage = "Required field")]
        [StringLength(20, ErrorMessage = "Max 20 symbols")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Required field")]
        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&'])[^ ]{8,}$",ErrorMessage = "Must contain at least one number, at least one uppercase letter,at least one lowercase letter.")]
        public string Password { get; set; }
    }
}