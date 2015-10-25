using Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Admin.Configuration;
namespace Admin.Controllers
{
    public class PitanjaOdgovoriController : Controller
    {
        Context ctx = new Context();
        //
        // GET: /PitanjaOdgovori/
        public ActionResult Snimi()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Snimi(string pitanje, string odgovor)
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {
                try
                {
                    pitanje = AntiXSS.GetSafeHtml(pitanje);
                    if (pitanje != "")
                    {
                        PitanjaOdgovori p = new PitanjaOdgovori();
                        p.Pitanje = pitanje;
                        p.Odgovor = odgovor;
                        p.IsActive = true;
                        ctx.PitanjaOdgovoris.Add(p);
                        ctx.SaveChanges();
                        ViewBag.Success = "Uspješno ste snimili novo često postavljeno pitanje.";
                        return View();
                    }
                    else
                    {
                        ViewBag.Error = "Niste unijeli sve informacije potrebne za upis.";
                        return View();
                    }
                }
                catch (Exception)
                {
                    ViewBag.Error = "Niste unijeli sve informacije potrebne za upis.";
                    return View();
                }
            }
            else
                return RedirectToAction("Index", "Login");
        }
        public ActionResult Pregled()
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {
                ViewData["pitanja"] = ctx.PitanjaOdgovoris.Where(x => x.IsActive == true).OrderByDescending(x=>x.PitanjaOdgovoriID).ToList();
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }
        public ActionResult IzmjenaPitanja(int? Id)
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {
                int id = Id ?? default(int);
                PitanjaOdgovori p = ctx.PitanjaOdgovoris.Where(x => x.PitanjaOdgovoriID == id).FirstOrDefault();
                if (p != null)
                {
                    ViewData["pitanja"] = p;
                    return View();
                }
                else
                    return RedirectToAction("Pregled", "PitanjaOdgovori");
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult IzmjenaPitanja(string pitanje, string odgovor, int pitanjeID)
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {
                PitanjaOdgovori po = ctx.PitanjaOdgovoris.Where(x => x.PitanjaOdgovoriID == pitanjeID).FirstOrDefault();
                if (po != null)
                {
                    try
                    {
                        pitanje = AntiXSS.GetSafeHtml(pitanje);
                        if (pitanje != "")
                            po.Pitanje = pitanje;
                        if (odgovor != "")
                            po.Odgovor = odgovor;
                        ctx.SaveChanges();
                        ViewData["pitanja"] = po;
                        ViewBag.Success = "Uspješno ste izmijenili pitanje i odgovor!";
                        return View();
                    }
                    catch (Exception)
                    {
                        ViewBag.Error = "Desila se greška pri izmjeni!";
                        ViewData["pitanja"] = po;
                        return View();
                    }
                }
                else
                    return RedirectToAction("Pregled", "PitanjaOdgovori");
            }
            else
                return RedirectToAction("Index", "Login");
        }
        public ActionResult BrisanjePitanja(int ?Id)
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {
                int id = Id ?? default(int);
                PitanjaOdgovori p = ctx.PitanjaOdgovoris.Where(x => x.PitanjaOdgovoriID == id).FirstOrDefault();
                if(p!=null)
                {
                    try
                    {
                        p.IsActive = false;
                        ctx.SaveChanges();
                        return RedirectToAction("Pregled", "PitanjaOdgovori");
                    }
                    catch (Exception)
                    {
                        ViewBag.Error = "Pitanje i odgovor nisu obrisani!";
                        return RedirectToAction("Pregled", "PitanjaOdgovori");
                    }
                }
                else
                {
                    return RedirectToAction("Pregled", "PitanjaOdgovori");
                }
            }
            else
                return RedirectToAction("Index", "Login");
        }
    }
}
