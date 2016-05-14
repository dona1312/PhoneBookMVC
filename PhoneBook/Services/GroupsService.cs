using PhoneBook.Models;
using PhoneBook.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        public IEnumerable<SelectListItem> GetContactsByGroup(Group group)
        {
            List<string> selectedIds = group.Contacts.Select(c => c.ID.ToString()).ToList();

            return new ContactsRepository().GetAll().Select(c => new SelectListItem
            {
                Text = c.FirstName+" "+c.LastName,
                Value = c.ID.ToString(),
                Selected = selectedIds.Contains(c.ID.ToString())
            });
        }

    }
}