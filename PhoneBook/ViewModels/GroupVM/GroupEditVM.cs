using PhoneBook.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhoneBook.ViewModels.GroupVM
{
    public class GroupEditVM
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Required field")]
        [StringLength(20)]
       
        public string Name { get; set; }
        public List<Contact> Contacts { get; set; }
        public int? ContactID { get; set; }
        public int? GroupID { get; set; }
    }
}