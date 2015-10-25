using Admin.Configuration;
using Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admin.Controllers
{
    public class KalendarController : Controller
    {
        //
        // GET: /Kalendar/
        Context ctx = new Context();
        public ActionResult Snimi()
        {
            return View();
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Snimi(string obavijest, DateTime ? datum, string naslov)
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {
                try
                {
                    naslov = AntiXSS.GetSafeHtml(naslov);
                    KalendarObavijesti ka = new KalendarObavijesti();
                    if (naslov != "" && datum != null && obavijest != "")
                    {
                        ka.Obavijest = obavijest;
                        ka.Naslov = naslov;
                        ka.Datum = datum ?? default(DateTime);
                        ka.IsActive = true;
                        ka.KorisnikID = k.KorisnikID;
                        ctx.KalendarObavijestis.Add(ka);
                        ctx.SaveChanges();

                        ViewBag.Success = "Uspješno ste snimili novu obavijest u kalendar.";
                        return View();
                    }
                    else
                    {
                        ViewBag.Error = "Niste unijeli sve informacije potrebne za upis!";
                        return View();
                    }
                }
                catch (Exception)
                {
                    ViewBag.Error = "Greška prilikom snimanja kalendara!";
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
                ViewData["kalendari"] = ctx.KalendarObavijestis.Where(x => x.IsActive == true).OrderByDescending(x=>x.KalendarID).ToList();
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }
        public ActionResult IzmjenaKalendara(int ?Id)
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {
                int id = Id ?? default(int);
                KalendarObavijesti ko = ctx.KalendarObavijestis.Where(x => x.KalendarID == id).FirstOrDefault();
                if (ko != null)
                {
                    ViewData["kalendar"] = ko;
                    return View();
                }
                else
                    return RedirectToAction("Pregled", "Kalendar");
            }
            else
                return RedirectToAction("Index", "Login");
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult IzmjenaKalendara(int kalendarID, string obavijest, DateTime datum, string naslov)
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {
                KalendarObavijesti ko = ctx.KalendarObavijestis.Where(x => x.KalendarID == kalendarID).FirstOrDefault();
                if (ko != null)
                {
                    try
                    {
                        naslov = AntiXSS.GetSafeHtml(naslov);
                        if (naslov != "")
                            ko.Naslov = naslov;
                        ko.KorisnikID = k.KorisnikID;
                        if (obavijest != "")
                        ko.Obavijest = obavijest;
                        if (datum != null)
                        ko.Datum = datum;
                        ctx.SaveChanges();
                        ViewData["kalendar"] = ko;
                        ViewBag.Success = "Uspješno ste izmijenili obavijest u kalendaru!";
                        return View();
                    }
                    catch (Exception)
                    {
                        ViewData["kalendar"] = ko;
                        ViewBag.Error = "Desila se greška pri izmjeni!";
                        return View();
                    }

                }
                else
                    return RedirectToAction("Pregled", "Kalendar");
            }
            else
                return RedirectToAction("Index", "Login");
        }
        public ActionResult BrisanjeKalendara(int ?Id)
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {
                int id = Id ?? default(int);
                KalendarObavijesti ko = ctx.KalendarObavijestis.Where(x => x.KalendarID == id).FirstOrDefault();
                if (ko != null)
                {
                    try
                    {
                        ko.IsActive = false;
                        ko.KorisnikID = k.KorisnikID;
                        ctx.SaveChanges();
                        return RedirectToAction("Pregled", "Kalendar");
                    }
                    catch (Exception)
                    {
                        ViewBag.Error = "Obavijest u kalendaru nije obrisana!";                        
                        return RedirectToAction("Pregled", "Kalendar");

                    }
                }
                else
                    return RedirectToAction("Pregled", "Kalendar");
            }
            else
                return RedirectToAction("Index", "Login");
        }
    }
}
