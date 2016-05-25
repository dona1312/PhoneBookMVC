using PhoneBook.Models;
using PhoneBook.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
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
        public void SetSelectedGroups(Contact con, string[] groupIDs)
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
            return new CitiesRepository().GetAll(cities => cities.CountryID == countryID).Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.ID.ToString()
            }).OrderBy(city => city.Text);
        }
        public void vCardExport()
        {
            var builder = new StringBuilder();
           

            foreach (var contact in AuthenticationService.LoggedUser.Contacts)
            {
                builder.AppendLine("BEGIN:VCARD");
                builder.AppendLine("VERSION:2.1");
                builder.AppendLine("N:" + contact.LastName + ";" + contact.FirstName);

                builder.AppendLine("FN:" + contact.FirstName + " " + contact.LastName);

                builder.Append("ADR;HOME;PREF:;;");
                builder.Append(contact.Adress + ";");
                builder.Append(contact.City.Name + ";;");
                builder.Append("-" + ";");
                builder.AppendLine(contact.City.Country.Name);

                if (contact.Phones.Count == 0)
                {
                    builder.AppendLine("TEL;HOME;VOICE:" + "-");
                    builder.AppendLine("TEL;CELL;VOICE:" + "-");
                }
                else
                {
                    builder.AppendLine("TEL;HOME;VOICE:" + contact.Phones.Where(p => p.PhoneType == Enums.PhoneTypeEnum.Home));
                    builder.AppendLine("TEL;CELL;VOICE:" + contact.Phones.Where(p => p.PhoneType == Enums.PhoneTypeEnum.Mobile));
                }

                builder.AppendLine("END:VCARD");
            }

            string directory = HttpContext.Current.Server.MapPath("~/Cards/");
            string filename = AuthenticationService.LoggedUser.FirstName + "-" + AuthenticationService.LoggedUser.LastName + ".vcf";
            string targetPath = Path.Combine(directory, filename);

            File.Create(targetPath).Close();
            using (var writer = new StreamWriter(targetPath))
            {
                writer.Write(builder.ToString());
            }
            
        }
    }
}