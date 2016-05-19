using AutoMapper;
using PhoneBook.Filters;
using PhoneBook.Models;
using PhoneBook.Repositories;
using PhoneBook.Services;
using PhoneBook.ViewModels.PhoneVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Expressions;

namespace PhoneBook.Controllers
{
    [AuthenticationFilter]
    public class PhonesController : Controller
    {
        // GET: Phones
        public ActionResult List()
        {
            PhonesService phoneService = new PhonesService();
            PhoneListVM model = new PhoneListVM();
            TryUpdateModel(model);

            if (!model.ContactID.HasValue)
            {
                return ControllerExtensions.RedirectToAction<ContactsController>(this, c => c.List(1));
            }
            
            model.Contact = phoneService.GetContact(model.ContactID.Value);
            model.Phones = phoneService.GetAll().Where(p => p.ContactID == model.ContactID.Value).ToList();
           

            return View(model);
        }
        public ActionResult Edit(int? id)
        {
            PhonesService phonesService = new PhonesService();
            PhoneEditVM model = new PhoneEditVM();
            TryUpdateModel(model);

            Phone phone;
            if (!id.HasValue)
            {
                phone = new Phone();
            }
            else
            {
                phone = phonesService.GetByID(id.Value);
                if (phone == null)
                {
                    if (phonesService.GetContact(model.ContactID) == null)
                    {
                        return this.RedirectToAction<ContactsController>(c => c.List(1));
                    }

                    return this.RedirectToAction(c => c.List(), new { ContactID = model.ContactID });
                }
                model.ContactID = phone.ContactID;
            }

            Mapper.Map(model, phone);

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit()
        {
            UnitOfWork unit = new UnitOfWork();
            PhonesService phoneService = new PhonesService(unit);
            PhoneEditVM model = new PhoneEditVM();
            TryUpdateModel(model);
            Phone p;
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (model.ID != 0)
            {
                p = phoneService.GetByID(model.ID);
            }
            else
                p = new Phone();


            Mapper.Map(model,p);

            phoneService.Save(p);
            return this.RedirectToAction( c => c.List(), new { contactID = model.ContactID });
        }
        public ActionResult Delete(int? id)
        {
            PhonesService phoneService = new PhonesService();
            if (!id.HasValue)
                return this.RedirectToAction(c => c.List());
            else
                phoneService.Delete(id.Value);
            return this.RedirectToAction(c => c.List());
        }
    }
}