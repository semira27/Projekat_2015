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
    public class StudentskiZajmoviController : Controller
    {
        //
        // GET: /StudentskiZajmovi/
        Context ctx = new Context();
        public ActionResult Snimi()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Snimi(string naslov, string sazetak, string obavijest, List<HttpPostedFileBase> dokumenti)
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
             if (k != null)
             {
                 try
                 {
                     naslov = AntiXSS.GetSafeHtml(naslov);
                     if (naslov == "")
                     {
                         ViewBag.Error = "Niste unijeli naziv studentskog zajma!";
                         return View();
                     }

                     Obavijesti o = new Obavijesti();

                     o.KorisnikID = k.KorisnikID;
                     o.NaslovObavijesti = naslov;
                     o.IsIstaknuta = false;
                     o.IsActive = true;
                     o.Obavijest = obavijest;
                     o.Sazetak = sazetak;
                     o.SlikaObavijesti = "";
                     o.Datum = DateTime.Now;

                     ctx.Obavijestis.Add(o);
                     ctx.SaveChanges();


                     if (dokumenti[0] != null)
                     {
                         for (int i = 0; i < dokumenti.Count(); i++)
                         {
                             if (IsDokumentAllowed(dokumenti[i]))
                             {
                                 ObavijestiDokumenti od = new ObavijestiDokumenti();
                                 od.ObavjestID = o.ObavijestID;
                                 od.Lokacija = SnimiDokument(dokumenti[i]);
                                 od.NazivDokumenta = Path.GetFileNameWithoutExtension(dokumenti[i].FileName);
                                 od.TipDokumenta = Path.GetExtension(dokumenti[i].FileName);
                                 od.Velicina = dokumenti[i].ContentLength;
                                 od.IsActive = true;

                                 ctx.ObavijestiDokumentis.Add(od);
                                 ctx.SaveChanges();
                             }
                         }
                     }

                     Sektori s = ctx.Sektoris.Where(x => x.IsStudentskiZajam == true).SingleOrDefault();

                     ObavijestiSektori so = new ObavijestiSektori();
                     so.IsActive = true;
                     so.ObavijestID = o.ObavijestID;
                     so.SektorID = s.SektorID;
                     ctx.ObavijestiSektoris.Add(so);
                     ctx.SaveChanges();

                     ViewBag.Success = "Uspješno ste snimili novi studentski zajam.";
                     return View();
                 }
                 catch (Exception)
                 {
                      ViewBag.Error = "Greška prilikom snimanja studentskog zajma!";
                      return View();
                 }
             }
             return RedirectToAction("Index", "Login");
           
        }
        public ActionResult Pregled()
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {
                ViewData["sektori"] = (from o in ctx.Obavijestis
                                       join os in ctx.ObavijestiSektoris
                                           on o.ObavijestID equals os.ObavijestID
                                       join sek in ctx.Sektoris on os.SektorID equals sek.SektorID
                                       where sek.IsStudentskiZajam == true && o.IsActive == true
                                       select o).OrderByDescending(x=>x.ObavijestID).ToList();
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }
        public ActionResult IzmjenaZajmova(int ? Id)
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {
                int id = Id ?? default(int);
                Obavijesti o = ctx.Obavijestis.Where(x => x.ObavijestID == id).FirstOrDefault();
                if (o != null)
                {
                    ViewData["zajam"] = o;
                    ViewData["dokumenti"] = ctx.ObavijestiDokumentis.Where(x => x.ObavjestID == o.ObavijestID).ToList();
                    return View();
                }
                else
                    return RedirectToAction("Pregled", "StudentskiZajmovi");
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult IzmjenaZajmova(int zajamID, string naslov, string sazetak, string obavijest, List<HttpPostedFileBase> dokumenti)
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {
                Obavijesti o = ctx.Obavijestis.Where(x => x.ObavijestID == zajamID).FirstOrDefault();
                if(o!=null)
                {
                    try
                    {
                        naslov = AntiXSS.GetSafeHtml(naslov);
                        if (naslov != "")
                            o.NaslovObavijesti = naslov;
                        if (sazetak != "")
                            o.Sazetak = sazetak;
                        if (obavijest != "")
                            o.Obavijest = obavijest;
                        ctx.SaveChanges();
                        if (dokumenti[0] != null)
                        {
                            List<ObavijestiDokumenti> docs = ctx.ObavijestiDokumentis.Where(x => x.ObavjestID == o.ObavijestID).ToList();
                            if (docs.Count > 0)
                            {
                                foreach (ObavijestiDokumenti i in docs)
                                {
                                    i.IsActive = false;
                                    ctx.SaveChanges();
                                }
                            }

                            for (int i = 0; i < dokumenti.Count(); i++)
                            {
                                if (IsDokumentAllowed(dokumenti[i]))
                                {
                                    ObavijestiDokumenti od = new ObavijestiDokumenti();
                                    od.ObavjestID = o.ObavijestID;
                                    od.Lokacija = SnimiDokument(dokumenti[i]);
                                    od.NazivDokumenta = Path.GetFileNameWithoutExtension(dokumenti[i].FileName);
                                    od.TipDokumenta = Path.GetExtension(dokumenti[i].FileName);
                                    od.Velicina = dokumenti[i].ContentLength;
                                    od.IsActive = true;

                                    ctx.ObavijestiDokumentis.Add(od);
                                    ctx.SaveChanges();
                                }
                            }
                        }
                        ViewData["zajam"] = o;
                        ViewData["dokumenti"] = ctx.ObavijestiDokumentis.Where(x => x.ObavjestID == o.ObavijestID).ToList();
                        ViewBag.Success = "Uspješno ste izmijenili studentski zajam!";
                        return View();
                    }
                    catch (Exception)
                    {
                        ViewData["zajam"] = o;
                        ViewData["dokumenti"] = ctx.ObavijestiDokumentis.Where(x => x.ObavjestID == o.ObavijestID).ToList();
                        ViewBag.Error = "Desila se greška pri izmjeni!";
                        return View();
                    }
                }
                else
                    return RedirectToAction("Pregled", "StudentskiZajmovi");
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult BrisanjeZajmova(int? Id)
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {
                int id = Id ?? default(int);
                Obavijesti o = ctx.Obavijestis.Where(x => x.ObavijestID == id).FirstOrDefault();
                if (o != null)
                {
                    try
                    {
                        o.IsActive = false;
                        ctx.SaveChanges();
                        return RedirectToAction("Pregled", "StudentskiZajmovi");
                    }
                    catch (Exception)
                    {
                        ViewBag.Error = "Studentski zajam nije obrisan!";
                        return RedirectToAction("Pregled", "StudentskiZajmovi");
                    }
                }
                else
                    return RedirectToAction("Pregled", "StudentskiZajmovi");
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
