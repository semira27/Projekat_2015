using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Admin.Configuration;
using Data.Model;
using PagedList;
using System.Web.Helpers;
namespace Admin.Controllers
{

    public class SektoriController : Controller
    {
        //
        // GET: /Sektori/
        Context ctx = new Context();

        [HttpGet]
        public ActionResult InsertSektor()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult InsertSektor(string a, HttpPostedFileBase ikonica,string redosljed, HttpPostedFileBase slika, string naziv, string opis)
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {
                try
                {
                    int err = 0;
                    Sektori s = new Sektori();
                    if (ikonica != null && IsFilesAllowed(ikonica))
                        s.IkonaSektora = SnimiIkonu(ikonica);
                    else
                        err++;
                    if (slika != null && IsFilesAllowed(slika))
                        s.Slika = SnimiIkonicu(slika);
                    else
                        s.Slika = "";
                    try
                    {
                        int red = Convert.ToInt32(redosljed);
                        s.Redoslijed = red;
                    }
                    catch (Exception)
                    {

                        err++;
                    }
                    s.IsStudentskiZajam = false;
                    s.NazivSektora = AntiXSS.GetSafeHtml(naziv);
                    s.OSektoru = opis;
                    s.IsActive = true;

                    if (err > 0)
                    {
                        ViewBag.Error = "Niste unijeli sve informacije potrebne za upis!";
                        return View();
                    }
                    else
                    {
                        ctx.Sektoris.Add(s);
                        ctx.SaveChanges(); 
                        ViewBag.Success = "Uspješno ste snimili novi sektor!";
                        return View();
                    }
                    
                }
                catch (Exception)
                {
                    ViewBag.Error = "Greška prilikom snimanja sektora!";
                    return View();
                }
            }
            else
            return RedirectToAction("Index", "Login");

        }

        private bool IsFilesAllowed(HttpPostedFileBase files)
        {

            HttpPostedFileBase f = files;

            string extension = Path.GetExtension(f.FileName);

            if (!Config.ALLOWED_EXTENSIONS.Any(x => x == extension))
                return false;

            return true;
        }


        private string SnimiIkonicu(HttpPostedFileBase file)
        {
            string name = Path.GetFileNameWithoutExtension(file.FileName);
            string extension = Path.GetExtension(file.FileName);
            name = string.Format("{0}-{1}{2}", name, Guid.NewGuid().ToString("n").Substring(0, 7), extension);

            string savePath = string.Format("{0}{1}", HttpContext.Server.MapPath("~") + "/" + Config.FULL_PATH_IKONICE, name);

            file.SaveAs(savePath);

            return "/Upload/Ikonice/" + name;
        }

        public ActionResult SviSektori()
        {
            ViewData["SviSektori"] = ctx.Sektoris.Where(x => x.IsActive == true && x.IsStudentskiZajam == false).OrderByDescending(x => x.Redoslijed).ToList();

            return View();
        }
        [HttpGet]
        public ActionResult IzmjenaSektora(int? Id)
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {
                if (Id != null)
                {
                    int id = Id ?? default(int);
                    Sektori s = ctx.Sektoris.Where(x => x.SektorID == id).SingleOrDefault();

                    if (s != null)
                    {
                        ViewData["Sektor"] = s;
                        return View();
                    }
                }
                return RedirectToAction("SviSektori", "Sektori");
            }

            return RedirectToAction("Index", "Login");
            
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult IzmjenaSektora(int sektorId,string redosljed, string naziv, string opis, HttpPostedFileBase ikonica, HttpPostedFileBase slika)
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {
                try
                {
                    Sektori s = ctx.Sektoris.Where(x => x.SektorID == sektorId).SingleOrDefault();
                    if (s != null)
                    {
                        if (ikonica != null && IsFilesAllowed(ikonica))
                            s.IkonaSektora = SnimiIkonu(ikonica);

                        if (slika != null && IsFilesAllowed(slika))
                            s.Slika = SnimiIkonicu(slika);

                        naziv = AntiXSS.GetSafeHtml(naziv);
                        if (naziv != "")
                            s.NazivSektora = naziv;
                        s.OSektoru = opis;

                        if(redosljed!="")
                        {
                            s.Redoslijed = Convert.ToInt32(redosljed);
                        }

                        ctx.SaveChanges();


                        ViewBag.Success = "Uspješno ste izmijenili sektor!";
                        ViewData["Sektor"] = s;
                        return View();
                    }
                }
                catch (Exception)
                {
                    Sektori s = ctx.Sektoris.Where(x => x.SektorID == sektorId).SingleOrDefault();
                    ViewData["Sektor"] = s;
                    ViewBag.Error = "Desila se greška pri izmjeni!";
                    return View();
                }
            }
            return RedirectToAction("Index", "Login");

        }

        public ActionResult BrisanjeSektora(int? Id)
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null && Id != null)
            {
                int id = Id ?? default(int);
                Sektori s = ctx.Sektoris.Where(X => X.SektorID == id).FirstOrDefault();

                if (id != null) 
                {
                    try
                    {
                        s.IsActive = false;
                        ctx.SaveChanges();
                        return RedirectToAction("SviSektori", "Sektori");
                    }
                    catch (Exception)
                    {
                        ViewBag.Error = "Sektor nije obrisan!";                        
                        return RedirectToAction("SviSektori", "Sektori");
                    }
                }
            }
            return RedirectToAction("Index", "Login");
        }

        private string SnimiIkonu(HttpPostedFileBase file)
        {
            string name = Path.GetFileNameWithoutExtension(file.FileName);
            string extension = Path.GetExtension(file.FileName);
            name = string.Format("{0}-{1}{2}", name, Guid.NewGuid().ToString("n").Substring(0, 7), extension);

            string savePath = string.Format("{0}{1}", HttpContext.Server.MapPath("~") + "/" + Config.FULL_PATH_IKONICE, name);

            WebImage img = new WebImage(file.InputStream);
            img.FileName = name;
            img.Resize(300, 70, false, false);

            img.Save(savePath);

            return "/Upload/Ikonice/" + name;
        }
    }
}
