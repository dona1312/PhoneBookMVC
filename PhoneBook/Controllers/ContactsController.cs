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

namespace PhoneBook.Controllers
{
    [AuthenticationFilter]
    public class ContactsController : Controller
    {
        // GET: Contacts
        public ActionResult List(int? page)
        {
            ContactsService contactService = new ContactsService();
            ContactListVM model = new ContactListVM();
            TryUpdateModel(model);

            model.Contacts = contactService.GetAll().Where(c => c.UserID == AuthenticationService.LoggedUser.ID).ToList();
            if (model.Search != null)
            {
                model.Contacts = model.Contacts.Where(c => c.FirstName.Contains(model.Search) || c.LastName.Contains(model.Search) || c.Adress.Contains(model.Search)).ToList();
            }

            switch (model.SortOrder)
            {

                case "fname_desc": model.Contacts = model.Contacts.OrderByDescending(c => c.FirstName).ToList(); break;
                case "lname_asc": model.Contacts = model.Contacts.OrderBy(c => c.LastName).ToList(); break;
                case "lname_desc": model.Contacts = model.Contacts.OrderByDescending(c => c.LastName).ToList(); break;
                case "adress_asc": model.Contacts = model.Contacts.OrderBy(c => c.Adress).ToList(); break;
                case "adress_des": model.Contacts = model.Contacts.OrderByDescending(c => c.Adress).ToList(); break;
                case "fname_asc":
                default:
                    model.Contacts = model.Contacts.OrderBy(u => u.FirstName).ToList();
                    break;
            }
            int pageSize = 2;
            int pageNumber = (page ?? 1);
            model.ContactsPaged = model.Contacts.ToPagedList(pageNumber, pageSize);

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
                    return this.RedirectToAction(c => c.List(1));
                }
            }
            else
                contact = new Contact();

            model.ID = contact.ID;
            model.UserID = contact.UserID;
            model.FirstName = contact.FirstName;
            model.LastName = contact.LastName;
            model.Adress = contact.Adress;
            model.Phones = contact.Phones;
            model.ImageUrl = contact.ImagePath;
            if (model.ImageUrl == null)
            {
                model.ImageUrl = "default.png";
            }
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
                c = contactService.GetByID(model.ID);
            else
                c = new Contact();

            if (c == null)
                return this.RedirectToAction(co => co.List(1));


            if (model.ImageUpload != null && model.ImageUpload.ContentLength > 0)
            {
                if (!model.ImageUpload.FileName.Contains(".jpg") || model.ImageUpload.FileName.Contains(".png"))
                {
                    ModelState.AddModelError("", "Image format not accepted!");
                }
                var uploadDir = "~/Uploads/";
                var imagePath = Path.Combine(Server.MapPath(uploadDir), model.ImageUpload.FileName);
                var imageUrl = Path.Combine(uploadDir, model.ImageUpload.FileName);
                model.ImageUpload.SaveAs(imagePath);
            }
            if (!ModelState.IsValid)
            {
                model.Groups = contactService.GetSelectedGroups(c.Groups);
                return View(model);
            }

        

            c.ID = model.ID;
            c.UserID = AuthenticationService.LoggedUser.ID;
            c.FirstName = model.FirstName;
            c.LastName = model.LastName;
            c.Adress = model.Adress;
            c.Phones = model.Phones;
            c.ImagePath = model.ImageUpload.FileName;
            contactService.SetSelectedGroups(c, model.SelectedGroups);

            contactService.Save(c);
            return this.RedirectToAction(co => co.List(1));
        }
        public JsonResult DeleteImage(int contactID)
        {
            //get contact
            //remove  his image
            //set default image
            //return default image
            ContactsService cs = new ContactsService();
            Contact c = cs.GetByID(contactID);
            c.ImagePath = "default.png";
            cs.Save(c);

            return Json(new object[] { new object() }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Delete(int? id)
        {
            ContactsService contactService = new ContactsService();
            if (!id.HasValue)
                return this.RedirectToAction(c => c.List(1));
            else
                contactService.Delete(id.Value);
            return this.RedirectToAction(c => c.List(1));
        }
    }
}