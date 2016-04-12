using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PhoneBook.ViewModels.AccountVM
{
    public class AccountVerifyVM
    {
        [Required(ErrorMessage = "Required field")]
        [StringLength(20, ErrorMessage = "Max 20 symbols")]
        public string Password { get; set; }
    }
}