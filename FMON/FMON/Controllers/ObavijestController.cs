using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data.Model;
namespace FMON.Controllers
{
    public class ObavijestController : Controller
    {
        //
        // GET: /Obavijest/
        Context ctx = new Context();
        public ActionResult Pregled(int ? Id)
        {
            if (Id != null)
            {
                int id = Id ?? default(int);

                Obavijesti o = ctx.Obavijestis.Where(x => x.ObavijestID == id).SingleOrDefault();

                if (o != null)
                {
                    ViewData["obavijest"] = o;
                    return View();
                }              
            }
            return RedirectToAction("Index","Index");
        }

    }
}
