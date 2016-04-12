using PhoneBook.Filters;
using PhoneBook.Models;
using PhoneBook.Repositories;
using PhoneBook.Services;
using PhoneBook.ViewModels.GroupVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhoneBook.Controllers
{
    [AuthenticationFilter]
    public class GroupsController : Controller
    {
        // GET: Groups
        public ActionResult List()
        {
            GroupsService groupService = new GroupsService();
            GroupListVM model = new GroupListVM();
            TryUpdateModel(model);

            model.Groups = groupService.GetAll();
            if (model.Search != null)
            {
                model.Groups = model.Groups.Where(g => g.Name.Contains(model.Search)).ToList();
            }

            switch (model.SortOrder)
            {
                case "name_desc": model.Groups = model.Groups.OrderByDescending(g => g.Name).ToList(); break;
                case "name_asc":
                default:
                    model.Groups = model.Groups.OrderBy(g => g.Name).ToList();
                    break;
            }

            return View(model);
        }
        public ActionResult Edit(int? id)
        {
            GroupsService groupService = new GroupsService();
            GroupEditVM model = new GroupEditVM();

            Group group;
            if (id.HasValue)
                group = groupService.GetByID(id.Value);
            else
                group = new Group();

            if (group.ID == 0)
                return RedirectToAction("List");


            model.ID = group.ID;
            model.Name = group.Name;
            model.Contacts = group.Contacts;

         

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit()
        {
            GroupsService groupService = new GroupsService();
            GroupEditVM model = new GroupEditVM();
            TryUpdateModel(model);
            if (!ModelState.IsValid)
                return View(model);

            Group g;

            if (model.ID != 0)
                g = groupService.GetByID(model.ID);
            else
                g = new Group();

            if (g == null)
                return RedirectToAction("List");

            g.ID = model.ID;
            g.Name = model.Name;
            g.Contacts = model.Contacts;

           
            groupService.Save(g);
            return RedirectToAction("List");
        }
        //public ActionResult RemoveFromGroup()
        //{
        //    UnitOfWork unit = new UnitOfWork();
        //    GroupsService groupService = new GroupsService(unit);
        //    GroupEditVM model = new GroupEditVM();
        //    TryUpdateModel(model);

        //    if (model.GroupID.HasValue)
        //    {
        //        Group grou = groupService.GetByID(model.GroupID.Value);
        //        model.Contacts = grou.Contacts;
        //        Contact co = model.Contacts.Find(c=>c.ID==model.ContactID);
        //        grou.Contacts.Remove(co);
        //        groupService.Save(grou);
        //    }

        //    return RedirectToAction("List");
        //}
        public JsonResult RemoveFromGroup(int GroupID,int ContactID)
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            GroupsService groupsService = new GroupsService(unitOfWork);
            Group group = groupsService.GetByID(GroupID);

            if (group == null)
            {
                return Json(new object[] { new object() }, JsonRequestBehavior.AllowGet);
            }

            group.Contacts = group.Contacts.Where(c => c.ID != ContactID).ToList();
            groupsService.Save(group);
            
            return Json(new object[] { new object() }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Delete(int? id)
        {
            GroupsService groupService = new GroupsService();
            if (!id.HasValue)
                return RedirectToAction("List");
            else
                groupService.Delete(id.Value);
            return RedirectToAction("List");
        }
    }
}