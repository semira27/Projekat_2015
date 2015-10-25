using Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMON.Controllers
{
    public class PitanjaController : Controller
    {
        //
        // GET: /Pitanja/
        Context ctx = new Context();
        public ActionResult Index()
        {
            ViewData["pitanja"] = ctx.PitanjaOdgovoris.Where(x => x.IsActive == true).ToList();
            return View();
        }

    }
}
