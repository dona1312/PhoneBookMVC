using PhoneBook.Models;
using PhoneBook.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhoneBook.Services
{
    public class GroupsService : BaseService<Group>
    {
        public GroupsService() : base()
        {
        }
        public GroupsService(UnitOfWork unit) : base(unit) { }
        public List<Contact> GetContacts(Group gr)
        {
            ContactsService cs = new ContactsService();
            return cs.GetAll(c => c.Groups.Contains(gr));
                
        }
    }
}