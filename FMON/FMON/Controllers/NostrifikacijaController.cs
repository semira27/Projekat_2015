using Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMON.Controllers
{
    public class NostrifikacijaController : Controller
    {
        //
        // GET: /Nostrifikacija/
        Context ctx = new Context();
        public ActionResult Index()
        {
            ViewData["nostrifikacije"] = ctx.RegistarNostrifikacijas.Where(x => x.IsActive == true && x.IsRegistar == false).ToList();
            return View();
        }

    }
}
