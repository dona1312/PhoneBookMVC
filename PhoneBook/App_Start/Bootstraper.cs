using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PhoneBook.Mappings;

namespace PhoneBook.App_Start
{
    public class Bootstraper
    {
        public static void Run()
        {
            AutoMapperConfiguration.Configure();
            //LocationConfig.ParseData();
        }
    }
}