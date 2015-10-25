using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data.Model;
using Admin.Configuration;

namespace Admin.Controllers
{
    public class NajaveController : Controller
    {
        //
        // GET: /Najave/
        Context ctx = new Context();
        public ActionResult InsertNajave()
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {
                return View();

            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult InsertNajave(string sazetak, string sadrzaj, DateTime ? datum)
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;

            if (k != null)
            {
                try
                {
                    sazetak = AntiXSS.GetSafeHtml(sazetak);
                    if (datum != null && sazetak != "" && sadrzaj != "")
                    {
                        Najave n = new Najave();
                        n.Datum = datum ?? default(DateTime);
                        n.Sazetak = sazetak;
                        n.IsActive = true;
                        n.Vijest = sadrzaj;
                        n.KorisnikID = k.KorisnikID;
                        ctx.Najaves.Add(n);
                        ctx.SaveChanges(); 
                        ViewBag.Success = "Uspješno ste snimili novu najavu za medije.";
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
                    ViewBag.Error = "Greška prilikom snimanja najave za medije!";
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }

        }
        public ActionResult SveNajave()
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {
                ViewData["najave"] = ctx.Najaves.Where(x => x.IsActive == true).OrderByDescending(x=>x.NajavaID).ToList();
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        public ActionResult IzmjenaNajave(int? Id)
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {
                if (Id != null)
                {
                    int id = Id ?? default(int);
                    Najave n = ctx.Najaves.Where(x => x.NajavaID == id).SingleOrDefault();

                    if (n != null)
                    {
                        ViewData["Najava"] = n;
                        return View();
                    }
                }
                return RedirectToAction("SveNajave", "Najave");
            }

            return RedirectToAction("Index", "Login");
            
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult IzmjenaNajave(string sazetak, string sadrzaj, DateTime datum, int najavaID)
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;

            if (k != null)
            {
                Najave n = ctx.Najaves.Where(x => x.NajavaID == najavaID).FirstOrDefault();
                if(n!=null)
                {
                    try
                    {
                        if(datum != null)
                        n.Datum = datum;
                        sazetak = AntiXSS.GetSafeHtml(sazetak);
                        if (sazetak != "")
                            n.Sazetak = sazetak;
                        if (sadrzaj != "")
                            n.Vijest = sadrzaj;
                        ctx.SaveChanges();
                        ViewData["Najava"] = n;
                        ViewBag.Success = "Uspješno ste izmijenili najavu!";
                        return View();
                    }
                    catch (Exception)
                    {
                        ViewBag.Error = "Desila se greška pri izmjeni!";
                        ViewData["Najava"] = n;
                        return View();
                    }
                }
                else
                {
                    return RedirectToAction("SveNajave", "Najave");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public ActionResult BrisanjeNajave(int? Id)
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if(k!=null && Id!=null)
            {
                int id = Id ?? default(int);
                Najave n = ctx.Najaves.Where(X => X.NajavaID == id).FirstOrDefault();
                if (n != null)
                {
                    try
                    {
                        n.IsActive = false;
                        ctx.SaveChanges();
                        ViewData["najave"] = n;
                        return RedirectToAction("SveNajave", "Najave");
                    }
                    catch (Exception)
                    {
                        ViewBag.Error = "Najava nije obrisana!";
                        return RedirectToAction("SveNajave", "Najave");
                    }
                }
            }
            return RedirectToAction("Index", "Login");
        }
    }
}
