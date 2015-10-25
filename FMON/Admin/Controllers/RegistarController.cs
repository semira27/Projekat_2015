using Admin.Configuration;
using Data.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admin.Controllers
{
    public class RegistarController : Controller
    {
        //
        // GET: /Registar/
        Context ctx = new Context();
        [HttpGet]
        public ActionResult Snimi()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Snimi(string naziv, HttpPostedFileBase dokument)
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {
                try
                {
                    RegistarNostrifikacija n = new RegistarNostrifikacija();
                    n.IsActive = true;
                    n.IsRegistar = true;
                    n.KorisnikID = k.KorisnikID;
                    naziv = AntiXSS.GetSafeHtml(naziv);
                    if (naziv != "")
                        n.Naziv = naziv;
                    else
                        n.Naziv = Path.GetFileNameWithoutExtension(dokument.FileName);

                    n.Tip = Path.GetExtension(dokument.FileName);
                    n.Velicina = dokument.ContentLength;
                    n.Lokacija = SnimiDokument(dokument);
                    ctx.RegistarNostrifikacijas.Add(n);
                    ctx.SaveChanges();
                    ViewBag.Success = "Uspješno ste snimili novi dokument u registar!";
                    return View();
                }
                catch (Exception)
                {
                    ViewBag.Error = "Greška prilikom snimanja dokumenta u registar!";
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
                ViewData["registri"] = ctx.RegistarNostrifikacijas.Where(x => x.IsRegistar == true && x.IsActive == true).OrderByDescending(x=>x.RegistarNostrifikacijaID).ToList();
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }
        public ActionResult IzmjenaRegistra(int? Id)
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {
                int id = Id ?? default(int);
                RegistarNostrifikacija r = ctx.RegistarNostrifikacijas.Where(x => x.RegistarNostrifikacijaID == id).FirstOrDefault();
                if (r != null)
                {
                    ViewData["registar"] = r;
                    return View();
                }
                return RedirectToAction("Pregled", "Registar");
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult IzmjenaRegistra(string naziv, HttpPostedFileBase dokument, int registarID)
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {
                if (registarID != null)
                {
                    RegistarNostrifikacija r = ctx.RegistarNostrifikacijas.Where(x => x.RegistarNostrifikacijaID == registarID).FirstOrDefault();
                    if (r != null)
                    {
                        try
                        {
                            r.KorisnikID = k.KorisnikID;
                            if (naziv != "")
                                r.Naziv = naziv;
                            if (dokument != null)
                            {
                                r.Tip = Path.GetExtension(dokument.FileName);
                                r.Velicina = dokument.ContentLength;
                                r.Lokacija = SnimiDokument(dokument);
                            }
                            ctx.SaveChanges();
                            ViewData["registar"] = r;
                            ViewBag.Success = "Uspješno ste izmijenili dokument u registru!";
                            return View();
                        }
                        catch (Exception)
                        {
                             ViewData["registar"] = r;
                             ViewBag.Error = "Desila se greška pri izmjeni!";
                             return View();
                        }
                    }
                    return RedirectToAction("Pregled", "Registar");
                }
                else
                    return RedirectToAction("Pregled", "Registar");
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult Brisanjeregistra(int ?Id)
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {
                int id = Id ?? default(int);
                RegistarNostrifikacija r = ctx.RegistarNostrifikacijas.Where(x => x.RegistarNostrifikacijaID == id).FirstOrDefault();
                if(r!=null)
                {
                    try
                    {
                        r.IsActive = false;
                        ctx.SaveChanges();
                    }
                    catch (Exception)
                    {
                        ViewBag.Error = "Dokument iz registra nije obrisan!";                        
                        return RedirectToAction("Pregled", "Registar");
                    }
                }
                return RedirectToAction("Pregled", "Registar");
            }
            else
                return RedirectToAction("Index", "Login");
        }

        #region helper
        private bool IsDokumentAllowed(HttpPostedFileBase files)
        {

            HttpPostedFileBase f = files;

            string extension = Path.GetExtension(f.FileName);

            if (!Config.ALLOWED_EXTENSIONS_DOKUMENT.Any(x => x == extension))
                return false;

            return true;
        }
        private string SnimiDokument(HttpPostedFileBase file)
        {
            string name = Path.GetFileNameWithoutExtension(file.FileName);
            string extension = Path.GetExtension(file.FileName);
            name = string.Format("{0}-{1}{2}", name, Guid.NewGuid().ToString("n").Substring(0, 7), extension);

            string savePath = string.Format("{0}{1}", HttpContext.Server.MapPath("~") + "/" + Config.FULL_PATH_DOKUMENTI, name);

            file.SaveAs(savePath);

            return "/Upload/Dokumenti/" + name;
        }
        #endregion
    }
}
