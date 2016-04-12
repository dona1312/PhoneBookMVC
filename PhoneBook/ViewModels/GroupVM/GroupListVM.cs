using PhoneBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhoneBook.ViewModels.GroupVM
{
    public class GroupListVM
    {
        public List<Group> Groups { get; set; }
        public string SortOrder { get; set; }
        public string Search { get; set; }
        public List<Contact> Contacts { get; set; }
    }
}