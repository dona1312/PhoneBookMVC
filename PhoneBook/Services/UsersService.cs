using PhoneBook.Models;
using PhoneBook.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhoneBook.Services
{
    public class UsersService:BaseService<User>
    {
        public UsersService() : base() { }
        public UsersService(UnitOfWork unit) : base(unit) { }
        
        public User CheckUsernameOrMail(User user)
        {
                UsersService userService = new UsersService();
            User userMail = userService.GetAll(u => u.Email == user.Email || u.Username == user.Username.ToLower()).FirstOrDefault();
            if (userMail != null)
            {
                return user;
            }
            else
                return null;
        }
    }
}