using PhoneBook.Models;
using PhoneBook.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhoneBook.Services
{
    public class CitiesService:BaseService<City>
    {
        public CitiesService()
        {

        }
        public CitiesService(UnitOfWork unit):base(unit)
        {

        }
    }
}