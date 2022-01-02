using JenzHealth.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JenzHealth.Areas.Admin.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Admin/Error
        public ActionResult NotFound()
        {
            TempData["ErrorMessage"] = Global.ErrorMessage;
            return View();
        }

        public ActionResult Unauthorized()
        {
            TempData["ErrorMessage"] = Global.ErrorMessage;
            return View();
        }
        public ActionResult Error()
        {
            TempData["ErrorMessage"] = Global.ErrorMessage;
            return View();
        }
    }
}