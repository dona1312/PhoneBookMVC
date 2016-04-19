using PhoneBook.Models;
using PhoneBook.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhoneBook.Services
{
    public class ContactsService : BaseService<Contact>
    {
        public ContactsService() : base()
        {

        }
        public ContactsService(UnitOfWork unit) : base(unit) { }

        public IEnumerable<SelectListItem> GetSelectedGroups(List<Group> goups)
        {
            if (goups == null)
                goups = new List<Group>();

            var selectedIds = goups.Select(g => g.ID);

            return new GroupsRepository().GetAll().Select(g => new SelectListItem
            {
                Text = g.Name,
                Value = g.ID.ToString(),
                Selected = selectedIds.Contains(g.ID)
            });
        }
        public void SetSelectedGroups(Contact con,string[] groupIDs)
        {
            if (groupIDs == null)
                groupIDs = new string[0];

            if (con.ID != 0)
                con.Groups.Clear();
            else
                con.Groups = new List<Group>();

            foreach (var item in new GroupsService(base.UnitOfWork).GetAll())
            {
                if (groupIDs.Contains(item.ID.ToString()))
                {
                    con.Groups.Add(item);
                }
            }
        }
        public IEnumerable<SelectListItem> GetAllCountries()
        {

            return new CountriesRepository().GetAll().Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.ID.ToString()
            });
        }
        public IEnumerable<SelectListItem> GetAllCities()
        {
            return new CitiesRepository().GetAll().Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.ID.ToString()
            });
        }
        public IEnumerable<SelectListItem> GetCitiesByCountry(int countryID)
        {
            return new CitiesRepository().GetAll(cities=>cities.CountryID==countryID).Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.ID.ToString()
            });
        }

    }

}