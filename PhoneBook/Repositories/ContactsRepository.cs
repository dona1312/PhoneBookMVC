using PhoneBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhoneBook.Repositories
{
    public class ContactsRepository:BaseRepository<Contact>
    {
        public ContactsRepository():base()
        {

        }
        public ContactsRepository(UnitOfWork unit) : base(unit) { }
    }
}