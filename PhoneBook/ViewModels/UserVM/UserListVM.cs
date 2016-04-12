using PagedList;
using PhoneBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhoneBook.ViewModels.UserVM
{
    public class UserListVM
    {
        public List<User> Users { get; set; }
        public string SortOrder { get; set; }
        public string Search { get; set; }
        public int PageCount { get; set; }
        public int PageNumber { get; set; }
        public IPagedList<User> UsersPaged { get; set; }
    }
}