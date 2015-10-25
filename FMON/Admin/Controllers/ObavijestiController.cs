using Admin.Configuration;
using Data.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Admin.Controllers
{
    public class ObavijestiController : Controller
    {
        //
        // GET: /Obavijesti/
        Context ctx = new Context();
        [HttpGet]
        public ActionResult DodajObavijest()
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {
                ViewData["sektori"] = ctx.Sektoris.Where(x => x.IsActive == true).ToList();

                return View();
            }
            return RedirectToAction("Index", "Login");

        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult DodajObavijest(int[] sektori, string naslov, string sazetak, string obavijest, string chkIsIstaknuta,string IsStara, HttpPostedFileBase slikaObavijesti, List<HttpPostedFileBase> slike, List<HttpPostedFileBase> dokumenti)
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {
                try
                {
                    int err = 0;
                    Obavijesti o = new Obavijesti();
                    o.Datum = DateTime.Now;
                    o.IsActive = true;
                    o.KorisnikID = k.KorisnikID;
                    if (IsStara == "on")
                        o.Nova = false;
                    else
                        o.Nova = true;

                    if (chkIsIstaknuta == "on")
                    {
                        o.IsIstaknuta = true;
                        if (IsSlikaAllowed(slikaObavijesti))
                            o.SlikaObavijesti = SnimiSlikuObavijesti(slikaObavijesti); /////// nova funkcija
                        else
                            err++;

                    }
                    else
                    {
                        o.IsIstaknuta = false;
                        o.SlikaObavijesti = "";
                    }
                    if (sazetak != "")
                        o.Sazetak = sazetak;
                    else
                        err++;

                    if (obavijest != "")
                        o.Obavijest = obavijest;
                    else
                        err++;

                    if (naslov != "")
                        o.NaslovObavijesti = naslov;
                    else
                        err++;

                    if (err > 0)
                    {
                        ViewBag.Error = "Niste unijeli sve informacije potrebne za upis!";
                        return View();
                    }
                    else
                    {

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
                        if (slike[0] != null)
                        {
                            for (int i = 0; i < slike.Count(); i++)
                            {
                                if (IsSlikaAllowed(slike[i]))
                                {
                                    ObavijestiSlike s = new ObavijestiSlike();
                                    s.IsActive = true;
                                    s.ObavijestID = o.ObavijestID;
                                    s.Putanja = SnimiSliku(slike[i]);

                                    ctx.ObavijestiSlikes.Add(s);
                                    ctx.SaveChanges();
                                }
                            }
                        }
                        if (sektori != null)
                        {
                            foreach (int item in sektori)
                            {
                                ObavijestiSektori so = new ObavijestiSektori();
                                so.IsActive = true;
                                so.ObavijestID = o.ObavijestID;
                                so.SektorID = item;

                                ctx.ObavijestiSektoris.Add(so);
                                ctx.SaveChanges();
                            }
                        }
                        else
                        {
                            Sektori s = ctx.Sektoris.Where(x => x.IsActive == true && x.IsStudentskiZajam == false).First();

                            ObavijestiSektori so = new ObavijestiSektori();
                            so.IsActive = true;
                            so.ObavijestID = o.ObavijestID;
                            so.SektorID = s.SektorID;
                            ctx.ObavijestiSektoris.Add(so);
                            ctx.SaveChanges();
                        }
                    }

                    ViewBag.Success = "Uspješno ste snimili novu obavijest!";
                    return View();
                }
                catch (Exception)
                {
                      ViewBag.Error = "Greška prilikom snimanja obavijesti!";
                      return View();
                }
            }
            return RedirectToAction("Index", "Login");
        }
        public ActionResult SveObavijesti()
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {
                ViewData["obavijesti"] = ctx.Obavijestis.Where(x => x.IsActive == true).OrderBy(x=>x.ObavijestID).ToList();
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }
        public ActionResult IzmjenaObavijesti(int? Id)
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {
                if (Id != null)
                {
                    int id = Id ?? default(int);
                    Obavijesti o = ctx.Obavijestis.Where(x => x.ObavijestID == Id).FirstOrDefault();
                    if (o != null)
                    {
                        ViewData["obavijest"] = o;
                        ViewData["dokumenti"] = ctx.ObavijestiDokumentis.Where(x => x.ObavjestID == o.ObavijestID).ToList();
                        ViewData["slike"] = ctx.ObavijestiSlikes.Where(x => x.ObavijestID == o.ObavijestID).ToList();
                        ViewData["sektori"] = (from obavijestiSektori in ctx.ObavijestiSektoris
                                               join sektori in ctx.Sektoris
                                               on obavijestiSektori.SektorID equals sektori.SektorID
                                               where (obavijestiSektori.ObavijestID == o.ObavijestID)
                                               select sektori).ToList();
                        ViewData["sviSektori"] = ctx.Sektoris.Where(x => x.IsActive == true && x.NazivSektora != "Studentski zajam").ToList();
                        return View();
                    }
                    else
                        return RedirectToAction("SveObavijesti", "Obavijesti");
                }
                else
                    return RedirectToAction("SveObavijesti", "Obavijesti");

            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult IzmjenaObavijesti(int[] sektori,string IsStara, string naslov, string sazetak, string obavijest, string chkIsIstaknuta, HttpPostedFileBase slikaObavijesti, List<HttpPostedFileBase> slike, List<HttpPostedFileBase> dokumenti, int obavijestID)
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {
                Obavijesti o = ctx.Obavijestis.Where(x => x.ObavijestID == obavijestID).FirstOrDefault();
                try
                {
                    if (o != null)
                    {
                        int err = 0;
                        o.Datum = DateTime.Now;
                        o.IsActive = true;
                        o.KorisnikID = k.KorisnikID;
                        if (IsStara == "on")
                            o.Nova = false;
                        else
                            o.Nova = true;

                        if (chkIsIstaknuta == "on")
                        {
                            o.IsIstaknuta = true;
                            if (slikaObavijesti != null)
                            {
                                if (IsSlikaAllowed(slikaObavijesti))
                                    o.SlikaObavijesti = SnimiSlikuObavijesti(slikaObavijesti); /////// nova funkcija
                                else
                                    err++;
                            }
                        }
                        else
                        {
                            o.IsIstaknuta = false;
                            o.SlikaObavijesti = "";
                        }
                        if (sazetak != "")
                            o.Sazetak = sazetak;
                        else
                            err++;
                        if (obavijest != "")
                            o.Obavijest = obavijest;
                        else
                            err++;
                        if (naslov != "")
                            o.NaslovObavijesti = naslov;

                        if (err == 0)
                        {
                            ctx.SaveChanges();
                        }
                        else
                        {
                            ViewBag.Error = "Niste unijeli sve informacije potrebne za upis!";
                            ViewData["obavijest"] = o;
                            ViewData["dokumenti"] = ctx.ObavijestiDokumentis.Where(x => x.ObavjestID == o.ObavijestID).ToList();
                            ViewData["slike"] = ctx.ObavijestiSlikes.Where(x => x.ObavijestID == o.ObavijestID).ToList();
                            ViewData["sektori"] = (from obavijestiSektori in ctx.ObavijestiSektoris
                                                   join sektor in ctx.Sektoris
                                                   on obavijestiSektori.SektorID equals sektor.SektorID
                                                   where (obavijestiSektori.ObavijestID == o.ObavijestID)
                                                   select sektor).ToList();
                            ViewData["sviSektori"] = ctx.Sektoris.Where(x => x.IsActive == true && x.NazivSektora != "Studentski zajam").ToList();
                            return View();
                        }

                        if (dokumenti[0]!=null)
                        {
                            List<ObavijestiDokumenti> doc = ctx.ObavijestiDokumentis.Where(x => x.ObavjestID == o.ObavijestID).ToList();
                            foreach (ObavijestiDokumenti i in doc)
                            {
                                ctx.ObavijestiDokumentis.Remove(i);
                                ctx.SaveChanges();
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
                        if (slike[0]!=null)
                        {
                            List<ObavijestiSlike> pic = ctx.ObavijestiSlikes.Where(x => x.ObavijestID == o.ObavijestID).ToList();
                            foreach (ObavijestiSlike i in pic)
                            {
                                ctx.ObavijestiSlikes.Remove(i);
                                ctx.SaveChanges();
                            }

                            for (int i = 0; i < slike.Count(); i++)
                            {
                                if (IsSlikaAllowed(slike[i]))
                                {
                                    ObavijestiSlike s = new ObavijestiSlike();
                                    s.IsActive = true;
                                    s.ObavijestID = o.ObavijestID;
                                    s.Putanja = SnimiSliku(slike[i]);

                                    ctx.ObavijestiSlikes.Add(s);
                                    ctx.SaveChanges();
                                }
                            }
                        }
                        if (sektori != null)
                        {
                            List<ObavijestiSektori> sec = ctx.ObavijestiSektoris.Where(x => x.ObavijestID == o.ObavijestID).ToList();

                            foreach (ObavijestiSektori i in sec)
                            {
                                ctx.ObavijestiSektoris.Remove(i);
                                ctx.SaveChanges();
                            }
                            foreach (int item in sektori)
                            {
                                ObavijestiSektori so = new ObavijestiSektori();
                                so.IsActive = true;
                                so.ObavijestID = o.ObavijestID;
                                so.SektorID = item;

                                ctx.ObavijestiSektoris.Add(so);
                                ctx.SaveChanges();
                            }
                        }
                        ViewData["obavijest"] = o;
                        ViewData["dokumenti"] = ctx.ObavijestiDokumentis.Where(x => x.ObavjestID == o.ObavijestID).ToList();
                        ViewData["slike"] = ctx.ObavijestiSlikes.Where(x => x.ObavijestID == o.ObavijestID).ToList();
                        ViewData["sektori"] = (from obavijestiSektori in ctx.ObavijestiSektoris
                                               join sektor in ctx.Sektoris
                                               on obavijestiSektori.SektorID equals sektor.SektorID
                                               where (obavijestiSektori.ObavijestID == o.ObavijestID)
                                               select sektor).ToList();
                        ViewData["sviSektori"] = ctx.Sektoris.Where(x => x.IsActive == true && x.NazivSektora != "Studentski zajam").ToList();

                        ViewBag.Success = "Uspješno ste izmijenili obavijest!";
                        return View();
                    }
                }
                catch (Exception)
                {
                    ViewBag.Error = "Desila se greška pri izmjeni!";
                    ViewData["obavijest"] = o;
                    ViewData["dokumenti"] = ctx.ObavijestiDokumentis.Where(x => x.ObavjestID == o.ObavijestID).ToList();
                    ViewData["slike"] = ctx.ObavijestiSlikes.Where(x => x.ObavijestID == o.ObavijestID).ToList();
                    ViewData["sektori"] = (from obavijestiSektori in ctx.ObavijestiSektoris
                                           join sektor in ctx.Sektoris
                                           on obavijestiSektori.SektorID equals sektor.SektorID
                                           where (obavijestiSektori.ObavijestID == o.ObavijestID)
                                           select sektor).ToList();
                    ViewData["sviSektori"] = ctx.Sektoris.Where(x => x.IsActive == true && x.NazivSektora != "Studentski zajam").ToList();
                    return View();
                }
            }
            return RedirectToAction("Index", "Login");
        }

        public ActionResult ObrisiObavijest(int? Id)
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {
                if (Id != null)
                {
                    int id = Id ?? default(int);
                    Obavijesti o = ctx.Obavijestis.Where(x => x.ObavijestID == id).FirstOrDefault();
                    if (o != null)
                    {
                        try
                        {
                            o.IsActive = false;
                            ctx.SaveChanges();
                        }
                        catch (Exception)
                        {
                             ViewBag.Error = "Obavijest nije obrisana!";
                             return RedirectToAction("SveObavijesti", "Obavijesti");
                        }
                    }
                }
                return RedirectToAction("SveObavijesti", "Obavijesti");
            }
            else
                return RedirectToAction("Index", "Login");
        }
        #region helper
        private bool IsSlikaAllowed(HttpPostedFileBase files)
        {

            HttpPostedFileBase f = files;

            string extension = Path.GetExtension(f.FileName);

            if (!Config.ALLOWED_EXTENSIONS.Any(x => x == extension))
                return false;

            return true;
        }
        private string SnimiSliku(HttpPostedFileBase file)
        {
            string name = Path.GetFileNameWithoutExtension(file.FileName);
            string extension = Path.GetExtension(file.FileName);
            name = string.Format("{0}-{1}{2}", name, Guid.NewGuid().ToString("n").Substring(0, 7), extension);

            string savePath = string.Format("{0}{1}", HttpContext.Server.MapPath("~") + "/" + Config.FULL_PATH_SLIKE, name);

            file.SaveAs(savePath);

            return "/Upload/Slike/" + name;
        }
        private string SnimiSlikuObavijesti(HttpPostedFileBase file)
        {
            string name = Path.GetFileNameWithoutExtension(file.FileName);
            string extension = Path.GetExtension(file.FileName);
            name = string.Format("{0}-{1}{2}", name, Guid.NewGuid().ToString("n").Substring(0, 7), extension);

            string savePath = string.Format("{0}{1}", HttpContext.Server.MapPath("~") + "/" + Config.FULL_PATH_SLIKE, name);

            WebImage img = new WebImage(file.InputStream);
            img.FileName = name;
            img.Resize(800, 350, false, false);

            img.Save(savePath);

            return "/Upload/Slike/" + name;
        }
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
