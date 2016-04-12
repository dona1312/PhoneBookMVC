using PagedList;
using PhoneBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhoneBook.ViewModels.ContactVM
{
    public class ContactListVM
    {
        public List<Contact> Contacts { get; set; }
        public string SortOrder { get; set; }
        public string Search { get; set; }
        public IPagedList<Contact> ContactsPaged { get; set; }
    }
}