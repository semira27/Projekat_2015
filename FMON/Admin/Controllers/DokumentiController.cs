using Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Admin.Configuration;
using System.IO;

namespace Admin.Controllers
{
    public class DokumentiController : Controller
    {
        //
        // GET: /Dokumenti/
        Context ctx = new Context();
        [HttpGet]
        public ActionResult Index()
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {
                ViewData["Kategorije"] = ctx.DokumentiKategorijes.Where(x => x.IsActive == true).ToList();
                return View();
            }
            return RedirectToAction("Index", "Login");
        }
        [HttpPost]
        public ActionResult Index(int kategorijaId, HttpPostedFileBase dokument, string naziv)
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {
                try
                {
                    ViewData["Kategorije"] = ctx.DokumentiKategorijes.Where(x => x.IsActive == true).ToList();
                    if (dokument == null)
                    {
                        ViewBag.Error = "Potrebno je odabrati bar jedan dokument!";
                        return View();
                    }
                    naziv = AntiXSS.GetSafeHtml(naziv);
                    Dokumenti d = new Dokumenti();
                    d.DatumObjave = DateTime.Now;
                    d.DokumentKategorijaID = kategorijaId;
                    if (naziv != "")
                        d.Naziv = naziv;
                    else
                        d.Naziv = Path.GetFileNameWithoutExtension(dokument.FileName);
                    d.Lokacija = SnimiDokument(dokument);
                    d.TipDokumenta = Path.GetExtension(dokument.FileName);
                    d.Velicina = dokument.ContentLength;
                    d.KorisnikID = k.KorisnikID; ///////////////////////////////////////// testddd
                    d.IsActive = true;
                    ctx.Dokumentis.Add(d);
                    ctx.SaveChanges();

                    ViewBag.Success = "Uspješno ste snimili novi dokument!";
                    return View();
                }
                catch (Exception)
                {
                    ViewBag.Error = "Greška prilikom snimanja dokumenta!";
                    return View();
                }

            }
            return RedirectToAction("Index", "Login");
        }

        public ActionResult SviDokumenti()
        {
            ViewData["dokumenti"] = ctx.Dokumentis.Where(x => x.IsActive == true).OrderByDescending(x => x.DokumentID).ToList();
            return View();
        }

        public ActionResult IzmjenaDokumenta(int? Id)
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {
                int id = Id ?? default(int);
                Dokumenti d = ctx.Dokumentis.Where(x => x.DokumentID == id).FirstOrDefault();
                if (d != null)
                {
                    ViewData["dokument"] = d;
                    ViewData["kategorije"] = ctx.DokumentiKategorijes.Where(x => x.IsActive == true).ToList();
                    return View();
                }
                return RedirectToAction("SviDokumenti", "Dokumenti");
            }
            else
                return RedirectToAction("Index", "Login");
        }
        public ActionResult IzmjenaKategorija(int? Id)
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {
                int id = Id ?? default(int);
                DokumentiKategorije d = ctx.DokumentiKategorijes.Where(x => x.KategorijaDokumentaID == id).FirstOrDefault();
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
        public ActionResult IzmjenaKategorija(int? Id, string naziv)
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {
                naziv = AntiXSS.GetSafeHtml(naziv);
                int id = Id ?? default(int);
                DokumentiKategorije d = ctx.DokumentiKategorijes.Where(x => x.KategorijaDokumentaID == id).FirstOrDefault();
                if (d != null)
                {
                    if (naziv != "")
                        d.Naziv = naziv;
                    ctx.SaveChanges();

                    ViewData["kategorija"] = d;
                    ViewBag.Success = "Uspješno ste izmijenili kategoriju!";
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

        [HttpPost]
        public ActionResult IzmjenaDokumenta(int dokumentID, int kategorijaID, HttpPostedFileBase dokument, string naziv)
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {

                Dokumenti d = ctx.Dokumentis.Where(x => x.DokumentID == dokumentID).FirstOrDefault();
                try
                {
                    if (d != null)
                    {
                        d.DatumObjave = DateTime.Now;
                        d.DokumentKategorijaID = kategorijaID;
                        if (naziv != "")
                            d.Naziv = naziv;
                        if (dokument != null)
                        {
                            d.Lokacija = SnimiDokument(dokument);
                            d.TipDokumenta = Path.GetExtension(dokument.FileName);
                            d.Velicina = dokument.ContentLength;
                            d.KorisnikID = k.KorisnikID;
                        }
                        ctx.SaveChanges();
                    }
                    ViewData["dokument"] = d;
                    ViewData["kategorije"] = ctx.DokumentiKategorijes.Where(x => x.IsActive == true).ToList();
                    ViewBag.Success = "Uspješno ste izmijenili dokument!";
                    return View();
                }
                catch (Exception)
                {
                    ViewBag.Error = "Desila se greška pri izmjeni!";
                    return RedirectToAction("IzmjenaDokumenta", "Dokumenti", new { Id = d.DokumentID });
                }

            }
            else
                return RedirectToAction("Index", "Login");
        }
        [HttpPost]
        public ActionResult SnimiKategoriju(string nazivKategorije)
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {
                nazivKategorije = AntiXSS.GetSafeHtml(nazivKategorije);

                if (nazivKategorije != "")
                {
                    try
                    {
                        DokumentiKategorije doc = new DokumentiKategorije();
                        doc.Naziv = nazivKategorije;
                        doc.IsActive = true;

                        ctx.DokumentiKategorijes.Add(doc);
                        ctx.SaveChanges();
                        ViewBag.Success = "Uspješno ste snimili kategoriju!";
                        return RedirectToAction("Index", "Dokumenti");
                    }
                    catch (Exception)
                    {
                        ViewBag.Error = "Kategorija nije snimljena!";
                        return RedirectToAction("Index", "Dokumenti");
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Dokumenti");
                }
            }
            else
                return RedirectToAction("Index", "Login");
        }
        public ActionResult Kategorije()
        {
            ViewData["kategorije"] = ctx.DokumentiKategorijes.Where(x => x.IsActive == true).ToList();
            return View();
        }


        public ActionResult ObrisiDokument(int? Id)
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {
                int id = Id ?? default(int);
                Dokumenti d = ctx.Dokumentis.Where(x => x.DokumentID == id).FirstOrDefault();
                if (d != null)
                {
                    try
                    {
                        d.IsActive = false;
                        ctx.SaveChanges();
                        return RedirectToAction("SviDokumenti", "Dokumenti");
                    }
                    catch (Exception)
                    {
                        ViewBag.Error = "Dokument nije obrisan!";
                        return RedirectToAction("SviDokumenti", "Dokumenti");

                    }
                }
                else
                {
                    return RedirectToAction("SviDokumenti", "Dokumenti");
                }
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult ObrisiKategoriju(int? Id)
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {
                int id = Id ?? default(int);
                DokumentiKategorije d = ctx.DokumentiKategorijes.Where(x => x.KategorijaDokumentaID == id).FirstOrDefault();
                if (d != null)
                {
                    try
                    {
                        d.IsActive = false;
                        ctx.SaveChanges();
                        return RedirectToAction("Kategorije", "Dokumenti");
                    }
                    catch (Exception)
                    {
                        ViewBag.Error = "Dokument nije obrisan!";
                        return RedirectToAction("Kategorije", "Dokumenti");

                    }
                }
                else
                {
                    return RedirectToAction("Kategorije", "Dokumenti");
                }
            }
            else
                return RedirectToAction("Index", "Login");
        }

        private bool IsFilesAllowed(HttpPostedFileBase files)
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
            string APP_PATH = System.Web.HttpContext.Current.Request.ApplicationPath.ToLower();
            name = string.Format("{0}-{1}{2}", name, Guid.NewGuid().ToString("n").Substring(0, 7), extension);

            string savePath = string.Format("{0}{1}", HttpContext.Server.MapPath("~") + "/" + Config.FULL_PATH_DOKUMENTI, name);

            file.SaveAs(savePath);

            return "/Upload/Dokumenti/" + name;
        }


    }
}
