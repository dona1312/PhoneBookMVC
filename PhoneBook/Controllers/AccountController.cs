using PhoneBook.Filters;
using PhoneBook.Models;
using PhoneBook.Services;
using PhoneBook.ViewModels.AccountVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhoneBook.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Login(string RedirectUrl)
        {
            AccountLoginVM model = new AccountLoginVM();
            return View(model);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login()
        {
            AccountLoginVM model = new AccountLoginVM();
            TryUpdateModel(model);

            AuthenticationService.Authenticate(model.Username, model.Password);
            if(AuthenticationService.LoggedUser!=null)
            {
                return RedirectToAction("List","Contacts");
            }
            else
            {
                ModelState.AddModelError("","Invalid username or password");
                return View(model);
            }
        }
        public ActionResult Register(string str)
        {
            AccountEditVM model = new AccountEditVM();
            return View(model);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register()
        {
            AccountEditVM model = new AccountEditVM();
            TryUpdateModel(model);

            PhoneBook.Models.User user = new Models.User();
            UsersService userService = new UsersService();
          
            if (userService.CheckUsernameOrMail(user) != null)
            {
                ModelState.AddModelError("", "Username or email already exists!");
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            user.ID = model.ID;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Username = model.Username;
            user.Password = Guid.NewGuid().ToString();
            user.Email = model.Email;
            user.Contacts = model.Contacts;
            userService.Save(user);
            PhoneBook.Services.EmailService.SendEmail(user);
            return RedirectToAction("Login");
        }
        public ActionResult Verify()
        {
            AccountVerifyVM model = new AccountVerifyVM();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Verify(int userID, string key)
        {
            AccountVerifyVM model = new AccountVerifyVM();
            TryUpdateModel(model);
            UsersService userService = new UsersService();

            User user = userService.GetByID(userID);
            user.Password = model.Password;
            userService.Save(user);

            return RedirectToAction("Login");
        }
        public ActionResult Logout()
        {
            AuthenticationService.LoggedUser = null;
            return RedirectToAction("Login");
        }
    }
}