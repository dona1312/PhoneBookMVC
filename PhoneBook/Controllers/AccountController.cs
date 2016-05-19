using AutoMapper;
using PhoneBook.Filters;
using PhoneBook.Models;
using PhoneBook.Services;
using PhoneBook.ViewModels.AccountVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Expressions;

namespace PhoneBook.Controllers
{
    public class AccountController : Controller
    {
        [AuthorizeAccessFilter]
        // GET: Account
        public ActionResult Login(string RedirectUrl)
        {
            AccountLoginVM model = new AccountLoginVM();
            return View(model);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeAccessFilter]
        public ActionResult Login()
        {
            AccountLoginVM model = new AccountLoginVM();
            TryUpdateModel(model);

            AuthenticationService.Authenticate(model.Username, model.Password);

            if (AuthenticationService.LoggedUser != null)
            {
                if (model.IsRememebered)
                {
                    CookieService.CreateCookie();
                }
                return this.RedirectToAction<ContactsController>(c => c.List());

            }
            else
            {
                ModelState.AddModelError("", "Invalid username or password");
                return View(model);
            }
        }
        [AuthorizeAccessFilter]
        public ActionResult Register(string str)
        {
            AccountEditVM model = new AccountEditVM();
            return View(model);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeAccessFilter]
        public ActionResult Register()
        {
            AccountEditVM model = new AccountEditVM();
            TryUpdateModel(model);

            PhoneBook.Models.User user = new Models.User();
            UsersService userService = new UsersService();

            if (userService.CheckUsernameOrMail(model) != null)
            {
                ModelState.AddModelError("", "Username or email already exists!");
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Mapper.Map(model, user);

            user.Password = Guid.NewGuid().ToString();
            userService.Save(user);
            PhoneBook.Services.EmailService.SendEmail(user, ControllerContext);

            return this.RedirectToAction(c => c.Login());

        }
        [AuthorizeAccessFilter]
        public ActionResult Verify(int userID)
        {
            AccountVerifyVM model = new AccountVerifyVM();
            if (userID<int.MinValue || userID>int.MaxValue)
            {
                ModelState.AddModelError("", "There is no such user!");
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeAccessFilter]
        public ActionResult Verify(int userID, string key)
        {
            AccountVerifyVM model = new AccountVerifyVM();
            TryUpdateModel(model);
            UsersService userService = new UsersService();

            User user = userService.GetByID(userID);
            if (user == null)
            {
                ModelState.AddModelError("", "There is no such user!");
            }
            else
            {
                Guid guidValue = Guid.NewGuid();
                if (!Guid.TryParse(key, out guidValue))
                {
                    ModelState.AddModelError("", "Inavlid key! Please check your e-mail for correct activation link!");
                }
                if (user.Password == key)
                {
                    user.Password = model.Password;
                    userService.Save(user);
                }
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return this.RedirectToAction(c => c.Login());
        }
        public ActionResult Logout()
        {
            AuthenticationService.Logout();
            return this.RedirectToAction(c => c.Login());
        }
    }
}