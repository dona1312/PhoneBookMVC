﻿using PhoneBook.Models;
using PhoneBook.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhoneBook.Services
{
    public static class AuthenticationService
    {
        public static User LoggedUser { get; set; }

        public static void Authenticate(string username, string password)
        {
            UsersRepository userRepo = new UsersRepository();
            LoggedUser = userRepo.GetAll().FirstOrDefault(u => u.Username == username && u.Password == password);
            
        }
        public static void AuthenticateByCookie(HttpCookie cookie)
        {
            if (cookie != null)
            {
                UsersRepository userRepo = new UsersRepository();
                AuthenticationService.LoggedUser = userRepo.GetAll().FirstOrDefault(u => u.RememberMeHash == cookie.Value);

            }
        }
        public static void Logout()
        {
            if (LoggedUser.RememberMeHash!=null)
            {
                CookieService.DeleteCookie();
            }
            AuthenticationService.LoggedUser = null;
        }
    }
}