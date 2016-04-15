using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhoneBook.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base
        public ActionResult List()
        {
            return View();
        }
    }
}