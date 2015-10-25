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
    public class KonkursiController : Controller
    {
        //
        // GET: /Konkursi/
        Context ctx = new Context();
        public ActionResult Snimi()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Snimi(string naziv, string obavijest, List<HttpPostedFileBase> dokumenti, int konkurs)
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {

                naziv = AntiXSS.GetSafeHtml(naziv);
                if (naziv != "")
                {
                    try
                    {
                        Konkursi kon = new Konkursi();
                        kon.Naziv = naziv;
                        kon.Obavijest = obavijest;

                        if (konkurs == 1)
                            kon.IsMedjunarodni = false;
                        else
                            kon.IsMedjunarodni = true;

                        kon.IsActive = true;
                        kon.KonkursID = k.KorisnikID;
                        kon.Datum = DateTime.Now;
                        kon.KorisnikID = k.KorisnikID;
                        ctx.Konkursis.Add(kon);
                        ctx.SaveChanges();

                        if (dokumenti[0] != null)
                        {
                            for (int i = 0; i < dokumenti.Count(); i++)
                            {
                                if (IsDokumentAllowed(dokumenti[i]))
                                {
                                    KonkursiDokumenti od = new KonkursiDokumenti();
                                    od.KonkursID = kon.KonkursID;
                                    od.Putanja = SnimiDokument(dokumenti[i]);
                                    od.Naziv = Path.GetFileNameWithoutExtension(dokumenti[i].FileName);
                                    od.TipDokumenta = Path.GetExtension(dokumenti[i].FileName);
                                    od.Velicina = dokumenti[i].ContentLength;
                                    od.IsActive = true;

                                    ctx.KonkursiDokumentis.Add(od);
                                    ctx.SaveChanges();
                                }
                            }
                        }
                        ViewBag.Success = "Uspješno ste snimili novi konkurs!";
                        return View();
                    }
                    catch (Exception)
                    {
                        ViewBag.Error = "Greška prilikom snimanja konkursa!";
                        return View();
                    }

                }
                else
                {
                    ViewBag.Error = "Niste unijeli sve informacije potrebne za upis!";
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
                ViewData["konkursi"] = ctx.Konkursis.Where(x => x.IsActive == true).OrderByDescending(x=>x.KonkursID).ToList() ;
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }
        public ActionResult IzmjenaKonkursa(int ? Id)
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {
                int id = Id ?? default(int);
                Konkursi konkurs = ctx.Konkursis.Where(x => x.KonkursID == id).FirstOrDefault();
                if (konkurs != null)
                {
                    ViewData["dokumenti"] = ctx.KonkursiDokumentis.Where(x => x.KonkursID == konkurs.KonkursID).ToList();
                    ViewData["konkurs"] = konkurs;
                    return View();
                }
                else
                    return RedirectToAction("Pregled", "Konkursi");
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult IzmjenaKonkursa(int konkursID, string naziv, string obavijest, int konkurs, List<HttpPostedFileBase> dokumenti)
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {
                Konkursi kon = ctx.Konkursis.Where(x => x.KonkursID == konkursID).FirstOrDefault();
                if(kon!=null)
                {
                    try
                    {
                        naziv = AntiXSS.GetSafeHtml(naziv);
                        if (naziv != "")
                            kon.Naziv = naziv;
                        kon.Obavijest = obavijest;

                        if (konkurs == 1)
                            kon.IsMedjunarodni = false;
                        else
                            kon.IsMedjunarodni = true;

                        kon.KorisnikID = k.KorisnikID;
                        kon.Datum = DateTime.Now;
                        ctx.SaveChanges();

                        if (dokumenti[0] != null)
                        {
                            List<KonkursiDokumenti> docs = ctx.KonkursiDokumentis.Where(x => x.KonkursID == kon.KonkursID).ToList();
                            if (docs.Count > 0)
                            {
                                foreach (KonkursiDokumenti item in docs)
                                {
                                    ctx.KonkursiDokumentis.Remove(item);
                                }
                            }


                            for (int i = 0; i < dokumenti.Count(); i++)
                            {
                                if (IsDokumentAllowed(dokumenti[i]))
                                {
                                    KonkursiDokumenti od = new KonkursiDokumenti();
                                    od.KonkursID = kon.KonkursID;
                                    od.Putanja = SnimiDokument(dokumenti[i]);
                                    od.Naziv = Path.GetFileNameWithoutExtension(dokumenti[i].FileName);
                                    od.TipDokumenta = Path.GetExtension(dokumenti[i].FileName);
                                    od.Velicina = dokumenti[i].ContentLength;
                                    od.IsActive = true;

                                    ctx.KonkursiDokumentis.Add(od);
                                    ctx.SaveChanges();
                                }
                            }
                        }
                        ViewData["dokumenti"] = ctx.KonkursiDokumentis.Where(x => x.KonkursID == kon.KonkursID).ToList();
                        ViewData["konkurs"] = kon;
                        ViewBag.Success = "Uspješno ste izmijenili konkurs!";
                        return View();
                    }
                    catch (Exception)
                    {
                         ViewData["dokumenti"] = ctx.KonkursiDokumentis.Where(x => x.KonkursID == kon.KonkursID).ToList();
                         ViewData["konkurs"] = kon;
                         ViewBag.Error = "Desila se greška pri izmjeni!";
                         return View();
                    }
                    }
                else
                {
                    return RedirectToAction("Pregled", "Konkursi");
                }
            }
            else
                return RedirectToAction("Index", "Login");
        }
        public ActionResult BrisanjeKonkursa(int ?Id)
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {
                int id = Id ?? default(int);
                Konkursi kon = ctx.Konkursis.Where(x => x.KonkursID == id).FirstOrDefault();
                if(kon!=null)
                {
                    try
                    {
                        kon.IsActive = false;
                        ctx.SaveChanges();
                    }
                    catch (Exception)
                    {
                        ViewBag.Error = "Konkurs nije obrisan!";                        
                        return RedirectToAction("Pregled", "Konkursi");
                    }
                }
                return RedirectToAction("Pregled", "Konkursi");
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
