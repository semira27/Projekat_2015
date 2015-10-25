using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data.Model;
using Admin.Configuration;
namespace Admin.Controllers
{
    public class KantoniController : Controller
    {
        //
        // GET: /Kantoni/
        Context ctx = new Context();
        public ActionResult Pregled()
        {
            ViewData["kantoni"] = ctx.Gradovis.Where(x => x.IsGeneralno == false).ToList();
            return View();
        }

        public ActionResult Izmjena(int? Id)
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {
                int id = Id ?? default(int);
                Gradovi d = ctx.Gradovis.Where(x => x.GradID == id).FirstOrDefault();
                if (d != null)
                {
                    ViewData["kategorija"] = d;

                    return View();
                }
                else
                {

                    return View();
                }
            }
            else
                return RedirectToAction("Index", "Login");
        }
        [HttpPost]
        public ActionResult Izmjena(int? Id, string naziv)
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {
                naziv = AntiXSS.GetSafeHtml(naziv);
                int id = Id ?? default(int);
                Gradovi d = ctx.Gradovis.Where(x => x.GradID == id).FirstOrDefault();
                if (d != null)
                {
                    if (naziv != "")
                        d.Naziv = naziv;
                    ctx.SaveChanges();

                    ViewData["kategorija"] = d;
                    ViewBag.Success = "Uspješno ste izmijenili naziv kantona!";
                    return View();
                }
                {
                    ViewBag.Error = "Desila se greška pri izmjeni!";
                    return View();
                }

            }
            else
                return RedirectToAction("Index", "Login");
        }

    }
}
