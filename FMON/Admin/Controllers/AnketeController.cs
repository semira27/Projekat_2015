using Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Admin.Configuration;
namespace Admin.Controllers
{
    
    public class AnketeController : Controller
    {
        //
        // GET: /Ankete/
        Context ctx = new Context();
        public ActionResult Snimi()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Snimi(string pitanje)
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {
                try
                {
                    pitanje = AntiXSS.GetSafeHtml(pitanje);
                    Anketa anketa = new Anketa();
                    if (pitanje != "")
                    {
                        List<Anketa> listaAnketa = ctx.Anketas.Where(x => x.IsActive == true).ToList();

                        foreach (Anketa item in listaAnketa)
                        {
                            item.IsActive = false;
                        }

                        anketa.Pitanje = pitanje;
                        anketa.DatumObjave = DateTime.Now; ;
                        anketa.IsActive = true;
                        anketa.KorisnikID = k.KorisnikID;

                        ctx.Anketas.Add(anketa);
                        ctx.SaveChanges();
                        ViewBag.Success = "Uspješno ste snimili novu anketu.";
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
                    ViewBag.Error = "Greška prilikom snimanja ankete!";
                    return View();
                }
            }
            else
                return RedirectToAction("Index", "Login");
        }
        [HttpGet]
        public ActionResult Izmjeni(int ? Id)
        {
             Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
             if (k != null)
             {
                 int id = Id ?? default (int);
                 Anketa a = ctx.Anketas.Where(x => x.AnketaID == id).SingleOrDefault();
                 if (a != null) 
                 { 
                 ViewData["anketa"] = a;

                 ViewData["odgovori"] = ctx.AnketaOdgovors.Where(x => x.AnketaID == a.AnketaID && x.IsActive== true).ToList();

                 return View();
                 }
             }

             return RedirectToAction("Index", "Login");
        }
        [HttpPost]
        public ActionResult Izmjeni(string pitanje, string isActive, string odgovor, int anketaId)
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {
                Anketa a = ctx.Anketas.Where(x => x.AnketaID == anketaId).SingleOrDefault();

                if(a != null)
                {
                    try
                    {
                        pitanje = AntiXSS.GetSafeHtml(pitanje);
                        odgovor = AntiXSS.GetSafeHtml(odgovor);

                        if (pitanje != a.Pitanje && pitanje != "")
                            a.Pitanje = pitanje;
                        if (isActive == "on")
                            a.IsActive = true;
                        else
                            a.IsActive = false;

                        if (odgovor != "")
                        {
                            AnketaOdgovor o = new AnketaOdgovor();
                            o.IsActive = true;
                            o.AnketaID = a.AnketaID;
                            o.Naziv = odgovor;
                            o.BrojGlasova = 0;

                            ctx.AnketaOdgovors.Add(o);
                        }
                        ctx.SaveChanges();
                        ViewData["anketa"] = a;
                        ViewData["odgovori"] = ctx.AnketaOdgovors.Where(x => x.AnketaID == a.AnketaID && x.IsActive == true).ToList();
                        ViewBag.Success = "Uspješno ste izmijenili anketu!";
                        return View();
                    }
                    catch (Exception)
                    {
                        ViewBag.Error = "Desila se greška pri izmjeni!";
                        ViewData["anketa"] = a;
                        ViewData["odgovori"] = ctx.AnketaOdgovors.Where(x => x.AnketaID == a.AnketaID && x.IsActive == true).ToList();
                        return View();
                    }
                }      
                else
                    return RedirectToAction("Pregled", "Ankete");
            }
            return RedirectToAction("Index", "Login");
        }



        public ActionResult ObrisiOdgovor(int? Id)
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {
                int id = Id ?? default (int);

                AnketaOdgovor o = ctx.AnketaOdgovors.Where(x => x.OdgovorID == id).SingleOrDefault();

                if (o != null)
                {
                    try
                    {
                        o.IsActive = false;
                        ctx.SaveChanges();
                        ViewBag.Success = "Uspješno ste obrisali odgovor!";
                        return RedirectToAction("Izmjeni", "Ankete", new { Id = o.AnketaID });
                    }
                    catch (Exception)
                    {
                        ViewBag.Error = "Desila se greška pri brisanju odgovora ankete!";
                        return RedirectToAction("Izmjeni", "Ankete", new { Id = o.AnketaID });
                    }
                }                       
            }
            return RedirectToAction("Index", "Login");
        }



        public ActionResult Pregled()
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {
                ViewData["Ankete"] = ctx.Anketas.OrderByDescending(x=>x.AnketaID).ToList();
                return View();
            }
            return RedirectToAction("Index", "Login");      
        }

        public ActionResult Brisi(int ? Id)
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {

                try
                {
                    int id = Id ?? default(int);

                    Anketa a = ctx.Anketas.Where(x => x.AnketaID == id).SingleOrDefault();

                    if (a != null)
                    {
                        if (a.IsActive)
                            a.IsActive = false;
                        else
                            a.IsActive = true;
                        ctx.SaveChanges();
                        return RedirectToAction("Pregled", "Ankete");
                    }
                }
                catch (Exception)
                {
                        ViewBag.Error = "Anketa nije obrisana!";
                        return RedirectToAction("Pregled", "Ankete");
                }
            }
            return RedirectToAction("Index", "Login");   
        }
    }
}
