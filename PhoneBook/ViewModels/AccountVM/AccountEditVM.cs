using PhoneBook.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PhoneBook.ViewModels.AccountVM
{
    public class AccountEditVM
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Required field")]
        [StringLength(20, ErrorMessage = "Max 20 symbols")]
        [Display(Name ="First name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Required field")]
        [StringLength(20, ErrorMessage = "Max 20 symbols")]
        [Display(Name = "Last name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Required field")]
        [StringLength(20,ErrorMessage ="Max 20 symbols")]
        public string Username { get; set; }
        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&'])[^ ]{8,}$",ErrorMessage = "Must contain at least one number, at least one uppercase letter,at least one lowercase letter.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Required field")]
        [StringLength(30, ErrorMessage = "Max 20 symbols")]
        [EmailAddress]
        [Display(Name = "E-mail")]
        public string Email { get; set; }


        public List<Contact> Contacts { get; set; }
    }
}