using Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMON.Controllers
{
    public class LinkController : Controller
    {
        //
        // GET: /Link/
        Context ctx = new Context();
        public ActionResult Index()
        {
            ViewData["gradovi"] = ctx.Gradovis.Where(x => x.IsGeneralno).ToList();
            return View();
        }

        public ActionResult Institucije(int? Id)
        {
            if (Id != null)
            {
                int id = Id ?? default(int);

                List<InstitucijaLinkovi> linkovi = ctx.InstitucijaLinkovis.Where(x => x.GradID == id).ToList();
                if (linkovi != null)
                {
                    Gradovi g = ctx.Gradovis.Where(x => x.GradID == id).SingleOrDefault();

                    ViewData["grad"] = g;
                    ViewData["linkovi"] = linkovi;
                    return View();
                }
            }        
                return RedirectToAction("Index", "Link");
        }

    }
}
