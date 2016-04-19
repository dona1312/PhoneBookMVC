using PhoneBook.Models;
using PhoneBook.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhoneBook.Services
{
    public class CountriesService:BaseService<Country>
    {
        public CountriesService()
        {

        }
        public CountriesService(UnitOfWork unit):base(unit)
        {

        }
    }
}