using PhoneBook.Models;
using PhoneBook.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhoneBook.Services
{
    public static class CookieService
    {
        public static void CreateCookie()
        {
            UsersService service = new UsersService();
            User user = service.GetByID(AuthenticationService.LoggedUser.ID);

            if (user != null)
            {
                HttpCookie rememberMeCookie = new HttpCookie("rememberMe");

                rememberMeCookie.Name = "rememberMe";
                rememberMeCookie.Value = Guid.NewGuid().ToString();
                rememberMeCookie.Expires = DateTime.Now.AddMinutes(10);

                HttpContext.Current.Response.Cookies.Add(rememberMeCookie);

                user.RememberMeHash = rememberMeCookie.Value;
                user.DateExpire = DateTime.Now.AddMinutes(10);

                service.Save(user);

            }

        }
        public static void DeleteCookie()
        {
            HttpCookie cookie = new HttpCookie("rememberMe");
            cookie.Expires = DateTime.Now.AddMinutes(-10);
            HttpContext.Current.Request.Cookies.Set(cookie);

            UsersService service = new UsersService();
            User user = service.GetByID(AuthenticationService.LoggedUser.ID);
            user.RememberMeHash = null;
            user.DateExpire = null;

            service.Save(user);
        }
    }
}