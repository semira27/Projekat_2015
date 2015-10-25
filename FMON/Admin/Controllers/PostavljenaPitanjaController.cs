using Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admin.Controllers
{
    public class PostavljenaPitanjaController : Controller
    {
        //
        // GET: /PostavljenaPitanja/
        Context ctx = new Context();
        public ActionResult Index()
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {
                ViewData["pitanja"] = ctx.PostaviPitanjes.Where(x => x.IsActive == true).OrderByDescending(x=>x.Datum).ToList(); ;

                return View();
            }
            return RedirectToAction("Index", "Login");
        }

        public ActionResult Pregledaj(int ? Id)
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {
                int id = Id ?? default(int);
                PostaviPitanje p  = ctx.PostaviPitanjes.Where(x => x.IsActive == true && x.PitanjeID == id).OrderByDescending(x=>x.PitanjeID).SingleOrDefault();
                if(p != null)
                {
                    p.PitanjePregledano = true;
                    ctx.SaveChanges();
                    ViewData["pitanje"] = p;
                    return View();
                }               
            }

            return RedirectToAction("Index", "Login");
        }

        public ActionResult Obrisi(int? Id)
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {
                int id = Id ?? default(int);
                PostaviPitanje p = ctx.PostaviPitanjes.Where(x => x.IsActive == true && x.PitanjeID == id).SingleOrDefault();
                if (p != null)
                {
                    try
                    {
                        p.IsActive = false;
                        ctx.SaveChanges();

                        return RedirectToAction("Index", "PostavljenaPitanja");
                    }
                    catch (Exception)
                    {
                        ViewBag.Error = "Postavljeno pitanje nije obrisano!";
                        return RedirectToAction("Index", "PostavljenaPitanja");
                    }
                }
            }
            return RedirectToAction("Index", "Login");
        }


    }
}
