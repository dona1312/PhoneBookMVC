using PhoneBook.Models;
using PhoneBook.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhoneBook.Services
{
    public class PhonesService:BaseService<Phone>
    {
        public PhonesService():base()
        {

        }
        public PhonesService(UnitOfWork unit) : base(unit) { }
        public Contact GetContact(int id)
        {
            ContactsService cs = new ContactsService();
            return cs.GetByID(id);
        }
    }
}