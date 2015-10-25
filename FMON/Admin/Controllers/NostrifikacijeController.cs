using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data.Model;
using System.IO;
using Admin.Configuration;

namespace Admin.Controllers
{
    public class NostrifikacijeController : Controller
    {
        //
        // GET: /Nostrifikacije/
        Context ctx = new Context();

        public ActionResult DodajNostrifikaciju()
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult DodajNostrifikaciju(string naziv, HttpPostedFileBase dokument)
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {
                try
                {
                    RegistarNostrifikacija n = new RegistarNostrifikacija();
                    n.IsActive = true;
                    n.IsRegistar = false;
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
                    ViewBag.Success = "Uspješno ste snimili novu nostrifikaciju.";
                    return View();
                }
                catch (Exception)
                {
                    ViewBag.Error = "Greška prilikom snimanja nostrifikacije!";
                    return View();
                }
            }

            else
                return RedirectToAction("Index", "Login");

        }

        public ActionResult SveNostrifikacije()
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {
                ViewData["nostrifikacije"] = ctx.RegistarNostrifikacijas.Where(x => x.IsRegistar == false && x.IsActive == true).OrderByDescending(x=>x.RegistarNostrifikacijaID).ToList();
                return View();
            }
            else
                return RedirectToAction("Index", "Login");

        }

        public ActionResult IzmjenaNostrifikacije(int? Id)
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {
                int id = Id ?? default(int);
                RegistarNostrifikacija r = ctx.RegistarNostrifikacijas.Where(x => x.RegistarNostrifikacijaID == id).FirstOrDefault();
                if (r != null)
                {
                    ViewData["nostrifikacija"] = r;
                    return View();
                }
                return RedirectToAction("SveNostrifikacije", "Nostrifikacije");
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult IzmjenaNostrifikacije(string naziv, HttpPostedFileBase dokument, int nostrifikacijaID)
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {
                if (nostrifikacijaID != null)
                {
                    RegistarNostrifikacija r = ctx.RegistarNostrifikacijas.Where(x => x.RegistarNostrifikacijaID == nostrifikacijaID).FirstOrDefault();
                    if (r != null)
                    {
                        try
                        {
                            r.KorisnikID = k.KorisnikID;
                            naziv = AntiXSS.GetSafeHtml(naziv);
                            if (naziv != "")
                                r.Naziv = naziv;
                            if (dokument != null)
                            {
                                r.Tip = Path.GetExtension(dokument.FileName);
                                r.Velicina = dokument.ContentLength;
                                r.Lokacija = SnimiDokument(dokument);
                            }
                            ctx.SaveChanges();
                            ViewBag.Success = "Uspješno ste izmijenili nostrifikaciju!";
                            ViewData["nostrifikacija"] = r;
                            return View();
                        }
                        catch (Exception)
                        {
                             ViewBag.Error = "Desila se greška pri izmjeni!";
                             ViewData["nostrifikacija"] = r;
                             return View();
                        }
                    }
                    return RedirectToAction("SveNostrifikacije", "Nostrifikacije");
                }
                else
                    return RedirectToAction("SveNostrifikacije", "Nostrifikacije");
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult ObrisiNostrifikaciju(int ?Id)
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            RegistarNostrifikacija rn = ctx.RegistarNostrifikacijas.Where(x => x.RegistarNostrifikacijaID == Id).FirstOrDefault();
            if (k != null && Id != null)
            {
                try
                {
                    rn.IsActive = false;
                    ctx.SaveChanges();
                    return RedirectToAction("SveNostrifikacije", "Nostrifikacije");
                }
                catch (Exception)
                {
                        ViewBag.Error = "Nostrifikacija nije obrisana!";
                        return RedirectToAction("SveNostrifikacije", "Nostrifikacije");
                }
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
