using PagedList;
using PhoneBook.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhoneBook.ViewModels.ContactVM
{
    public class ContactListVM
    {
        public string SortOrder { get; set; }
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        
        public string Search { get; set; }
        public IPagedList<Contact> Contacts{ get; set; }

        public int? Page { get; set; }
        public int PageSize { get; set; }
    }
}