using PagedList;
using PhoneBook.Filters;
using PhoneBook.Models;
using PhoneBook.Repositories;
using PhoneBook.Services;
using PhoneBook.ViewModels.ContactVM;
using System.Web.Mvc.Expressions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;

namespace PhoneBook.Controllers
{
    [AuthenticationFilter]
    public class ContactsController : Controller
    {
    
        // GET: Contacts
        public ActionResult List()
        {
            ContactsService contactService = new ContactsService();
            ContactListVM model = new ContactListVM();
            TryUpdateModel(model);

            List<Contact> contacts=contactService.GetAll().Where(c => c.UserID == AuthenticationService.LoggedUser.ID).ToList();
            
            if (model.Search != null)
            {
                contacts = contacts.Where(c => (c.FirstName.Trim() + c.LastName.Trim()).Trim().ToLower().Contains(model.Search.Replace(" ", string.Empty).ToLower()) || c.Adress.Contains(model.Search)).ToList();
            }

            switch (model.SortOrder)
            {

                case "fname_desc": contacts = contacts.OrderByDescending(c => c.FirstName).ToList(); break;
                case "lname_asc": contacts = contacts.OrderBy(c => c.LastName).ToList(); break;
                case "lname_desc": contacts = contacts.OrderByDescending(c => c.LastName).ToList(); break;
                case "adress_asc": contacts = contacts.OrderBy(c => c.Adress).ToList(); break;
                case "adress_des": contacts = contacts.OrderByDescending(c => c.Adress).ToList(); break;
                case "fname_asc":
                default:
                    contacts = contacts.OrderBy(u => u.FirstName).ToList();
                    break;
            }
            int pageSize = 3;
            if (model.PageSize != 0)
            {
                pageSize = model.PageSize;
            }

            int pageNumber = model.Page ?? 1;

            model.Contacts = contacts.ToPagedList(pageNumber, pageSize);

            return View(model);
        }
        public ActionResult Edit(int? id)
        {
            ContactsService contactService = new ContactsService();
            ContactEditVM model = new ContactEditVM();
            Contact contact;
            if (id.HasValue)
            {
                contact = contactService.GetByID(id.Value);
                if (contact == null)
                {
                    return this.RedirectToAction(c => c.List());
                }

                model.CountryID = contact.City.CountryID;
            }
            else
            {
                contact = new Contact();
                contact.ImagePath = "default.png";

                model.CountryID = int.Parse(contactService.GetAllCountries().FirstOrDefault().Value);

            }

            Mapper.Map(contact, model);

            if (model.ImagePath == null)
            {
                model.ImagePath = "default.png";
            }
            model.Countries = contactService.GetAllCountries().OrderBy(c=>c.Text);
            model.Cities = contactService.GetCitiesByCountry(model.CountryID);

            model.Groups = contactService.GetSelectedGroups(contact.Groups);

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit()
        {
            UnitOfWork uf = new UnitOfWork();
            ContactsService contactService = new ContactsService(uf);
            ContactEditVM model = new ContactEditVM();
            TryUpdateModel(model);

            Contact c;

            if (model.ID != 0)
            {
                c = contactService.GetByID(model.ID);
                if (c == null)
                {
                    return this.RedirectToAction(co => co.List());
                }
            }
            else
                c = new Contact();

            if (model.ImageUpload != null && model.ImageUpload.ContentLength > 0)
            {
                var ext = Path.GetExtension(model.ImageUpload.FileName);
                if (String.IsNullOrEmpty(ext) || !ext.Equals(".jpg", StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError("", "Image format not accepted!");
                }
                else
                {
                    var uploadDir = "/Uploads/";
                    var imagePath = Path.Combine(Server.MapPath(uploadDir), model.ImageUpload.FileName);
                    model.ImagePath = model.ImageUpload.FileName;
                    model.ImageUpload.SaveAs(imagePath);
                }


            }
            if (!ModelState.IsValid)
            {
                model.Groups = contactService.GetSelectedGroups(c.Groups);
                model.Countries = contactService.GetAllCountries().OrderBy(country=> country.Text);
                model.Cities = contactService.GetCitiesByCountry(model.CountryID);

                return View(model);
            }

            Mapper.Map(model, c);
            c.UserID = AuthenticationService.LoggedUser.ID;

            contactService.SetSelectedGroups(c, model.SelectedGroups);

            contactService.Save(c);
            return this.RedirectToAction(co => co.List());
        }
        public JsonResult GetCities(int countryID)
        {
            ContactsService contactService = new ContactsService();
            ContactEditVM model = new ContactEditVM();
            model.Cities = contactService.GetCitiesByCountry(countryID);

            return Json(model.Cities.ToArray(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteImage(int contactID)
        {
            ContactsService cs = new ContactsService();
            Contact contact = cs.GetByID(contactID);
            ContactEditVM model = new ContactEditVM();

            contact.ImagePath = "default.png";

            cs.Save(contact);

            return Json(new object[] { new object() }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Delete(int? id)
        {
            ContactsService contactService = new ContactsService();
            if (!id.HasValue)
                return this.RedirectToAction(c => c.List());
            else
                contactService.Delete(id.Value);
            return this.RedirectToAction(c => c.List());
        }
        public ActionResult Export()
        {
            ContactsService cs = new ContactsService();
            cs.vCardExport();
            return this.RedirectToAction(c => c.List());
        }
    }
}