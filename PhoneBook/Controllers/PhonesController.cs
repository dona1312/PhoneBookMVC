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

            model.Contact = phoneService.GetContact(model.ContactID.Value);
            model.Phones = phoneService.GetAll().Where(p => p.ContactID == model.ContactID.Value).ToList();
           

            return View(model);
        }
        public ActionResult Edit(int? id)
        {
            PhonesService phoneService = new PhonesService();
            PhoneEditVM model = new PhoneEditVM();
            TryUpdateModel(model);

            Phone phone;
            if (id.HasValue)
            {
                phone = phoneService.GetByID(id.Value);
                if (phone==null)
                {
                    return RedirectToAction("List");
                }
            }
            else
                phone = new Phone();

            model.ID = phone.ID;
            model.ContactID = phone.ContactID;
            model.Number = phone.Number;
            model.PhoneType = phone.PhoneType;

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


            p.ID = model.ID;
            p.ContactID = model.ContactID;
            p.Number = model.Number;
            p.PhoneType = model.PhoneType;

            phoneService.Save(p);
            return RedirectToAction("List", new { contactID = model.ContactID });
        }
        public ActionResult Delete(int? id)
        {
            PhonesService phoneService = new PhonesService();
            if (!id.HasValue)
                return RedirectToAction("List");
            else
                phoneService.Delete(id.Value);
            return RedirectToAction("List");
        }
    }
}