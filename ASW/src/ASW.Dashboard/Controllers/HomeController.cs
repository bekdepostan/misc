using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASW.Framework.Simple;

namespace Scanner.Dashboard.Controllers
{
    public class HomeController : Controller
    {
        private ASWContext _db = new ASWContext();

        // GET: Home
        public ActionResult Index()
        {
            ViewBag.AppCases = _db.ApplicationCases.ToList();
            return View();
        }
    }
}