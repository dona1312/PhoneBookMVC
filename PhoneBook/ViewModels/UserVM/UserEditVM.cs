using PhoneBook.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PhoneBook.ViewModels.UserVM
{
    public class UserEditVM
    {
        public int ID { get; set; }
        [Required(ErrorMessage ="Field required")]
        [StringLength(20)]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Field required")]
        [StringLength(20)]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Field required")]
        [StringLength(20)]
        public string Username { get; set; }
        [Required(ErrorMessage = "Field required")]
        [StringLength(20)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Field required")]
        [EmailAddress]
        public string Email { get; set; }
       
        public List<Contact> Contacts { get; set; }
    }
}