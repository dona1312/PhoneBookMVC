using PhoneBook.Models;
using PhoneBook.Services;
using PhoneBook.ViewModels.UserVM;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhoneBook.Controllers
{
    public class UsersController : Controller
    {

        // GET: Users
        public ActionResult List(int? page )
        {
            UsersService userService = new UsersService();
            UserListVM model = new UserListVM();
            TryUpdateModel(model);
            model.Users = userService.GetAll();

            if (model.Search != null)
            {
                model.Users = model.Users.Where(u => u.FirstName.Contains(model.Search) || u.LastName.Contains(model.Search) || u.Username.Contains(model.Search) || u.Email.Contains(model.Search)).ToList();
            }

            switch (model.SortOrder)
            {

                case "fname_desc": model.Users = model.Users.OrderByDescending(u => u.FirstName).ToList(); break;
                case "lname_asc": model.Users = model.Users.OrderBy(u => u.LastName).ToList(); break;
                case "lname_desc": model.Users = model.Users.OrderByDescending(u => u.LastName).ToList(); break;
                case "username_asc": model.Users = model.Users.OrderBy(u => u.Username).ToList(); break;
                case "username_desc": model.Users = model.Users.OrderByDescending(u => u.Username).ToList(); break;
                case "fname_asc":
                default:
                    model.Users = model.Users.OrderBy(u => u.FirstName).ToList();
                    break;
            }
            int pageSize =2;
            int pageNumber = (page ?? 1);
            model.UsersPaged = model.Users.ToPagedList(pageNumber, pageSize);

            return View(model);
        }
        public ActionResult Edit(int? id)
        {
            UsersService userService = new UsersService();
            UserEditVM model = new UserEditVM();
            User user;
            if (id.HasValue)
                user = userService.GetByID(id.Value);
            else
                user = new User();

            

            model.ID = user.ID;
            model.FirstName = user.FirstName;
            model.LastName = user.LastName;
            model.Username = user.Username;
            model.Password = user.Password;
            model.Contacts = user.Contacts;
            model.Email = user.Email;

            return View(model);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit()
        {
            UsersService userService = new UsersService();
            UserEditVM model = new UserEditVM();
            TryUpdateModel(model);
            if (!ModelState.IsValid)
                return View(model);

            User u;

            if (model.ID != 0)
                u = userService.GetByID(model.ID);
            else
                u = new User();

            if (u == null)
                return RedirectToAction("List");

            u.ID = model.ID;
            u.FirstName = model.FirstName;
            u.LastName = model.LastName;
            u.Username = model.Username;
            u.Password = model.Password;
            u.Contacts = model.Contacts;
            u.Email = model.Email;

            userService.Save(u);
            return RedirectToAction("List");

        }
        public ActionResult Delete(int? id)
        {
            UsersService userService = new UsersService();
            if (!id.HasValue)
                return RedirectToAction("List");
            else
                userService.Delete(id.Value);
            return RedirectToAction("List");
        }
    }
}