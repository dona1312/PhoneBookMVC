using AutoMapper;
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
using System.Web.Mvc.Expressions;

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

            //model.Groups = groupService.GetAll();
            model.Groups = new Dictionary<Group, IEnumerable<SelectListItem>>();
            foreach (var group in groupService.GetAll())
            {
                IEnumerable<SelectListItem> contacts = groupService.GetContactsByGroup(group);
                model.Groups.Add(group, contacts);
            }

            if (model.Search != null)
            {
                model.Groups = model.Groups.Where(g => g.Key.Name.Contains(model.Search)).ToDictionary(v => v.Key, v => v.Value);
            }

            switch (model.SortOrder)
            {
                case "name_desc": model.Groups = model.Groups.OrderByDescending(g => g.Key.Name).ToDictionary(v => v.Key, v => v.Value); break;
                case "name_asc":
                default:
                    model.Groups = model.Groups.OrderBy(g => g.Key.Name).ToDictionary(v => v.Key, v => v.Value);
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
            {
                group = groupService.GetByID(id.Value);
                if (group == null)
                {
                    return this.RedirectToAction(c => c.List());
                }
            }
            else
                group = new Group();

            Mapper.Map(group, model);


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
                return this.RedirectToAction(c => c.List());
            Mapper.Map(model, g);


            groupService.Save(g);
            return this.RedirectToAction(c => c.List());
        }
        public JsonResult RemoveFromGroup(int GroupID, int ContactID)
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
            UnitOfWork unitOfWork = new UnitOfWork();
            GroupsService groupService = new GroupsService(unitOfWork);
            if (!id.HasValue)
                return this.RedirectToAction(c => c.List());
            else
            {
                groupService.GetByID(id.Value).Contacts.Clear();
                groupService.Delete(id.Value);

            }
            return this.RedirectToAction(c => c.List());
        }

        public JsonResult Add(int[] contactsID, int groupID)
        {
            UnitOfWork unit = new UnitOfWork();
            GroupsService groupsService = new GroupsService(unit);

            Group group = groupsService.GetByID(groupID);

            group.Contacts.Clear();
            if (contactsID != null)
            {
                foreach (var item in contactsID)
                {
                    Contact contact = new ContactsService(unit).GetByID(item);

                    group.Contacts.Add(contact);
                }
            }
            else
            {
                contactsID = new int[0];
                groupsService.Save(group);
                return Json(new object[] { new object() }, JsonRequestBehavior.AllowGet);
            }


            groupsService.Save(group);

            var data = group.Contacts.Select(c => new
            {
                id = c.ID,
                firstName = c.FirstName,
                lastName = c.LastName
            });


            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Remove(int[] contactsID, int groupID)
        {
            UnitOfWork unit = new UnitOfWork();
            GroupsService groupsService = new GroupsService(unit);

            Group group = groupsService.GetByID(groupID);

            group.Contacts.Clear();
            foreach (var item in contactsID)
            {
                Contact contact = new ContactsService().GetByID(item);
                group.Contacts.Add(contact);
            }

            groupsService.Save(group);

            return Json(new object[] { new object() }, JsonRequestBehavior.AllowGet);
        }
    }
}