using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data.Model;

namespace Admin.Content
{
    public class HomeController : Controller
    {
        //
        // GET: /Index/
        Context ctx = new Context();
        public ActionResult Home()
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k!=null)
            {
                ViewData["LastFiveP"] =  ctx.Obavijestis.Where(x => x.IsActive == true).OrderByDescending(x => x.ObavijestID).Take(5).ToList();      
                ViewData["PostavljenaPitanja"] = ctx.PostaviPitanjes.Where(x => x.PitanjePregledano == false && x.IsActive == true).OrderByDescending(x => x.PitanjeID).Take(5).ToList();
                ViewData["Konkursi"] =           ctx.Konkursis.Where(x => x.IsActive == true).OrderByDescending(x => x.KonkursID).ToList();

                return View();

            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

    }
}
