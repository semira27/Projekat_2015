using Admin.Configuration;
using Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admin.Controllers
{
    public class LinkoviController : Controller
    {
        //
        // GET: /Linkovi/
        Context ctx = new Context();
        public ActionResult DodajLink()
        {
            ViewData["Gradovi"] = ctx.Gradovis.ToList();

            return View();
        }
        [HttpPost]
        public ActionResult DodajLink(string naziv, string link, int grad)
        {
            ViewData["Gradovi"] = ctx.Gradovis.ToList();
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {
                try
                {
                    InstitucijaLinkovi l = new InstitucijaLinkovi();
                    naziv = AntiXSS.GetSafeHtml(naziv);
                    link = AntiXSS.GetSafeHtml(link);
                    if (naziv != "" && link != "")
                    {
                        l.NazivLinka = naziv;
                        l.Link = link;
                        l.IsActive = true;
                        l.KorisnikID = k.KorisnikID;
                        l.GradID = grad;

                        ctx.InstitucijaLinkovis.Add(l);
                        ctx.SaveChanges();

                        ViewBag.Success = "Uspješno ste snimili novi link.";
                        ViewData["Gradovi"] = ctx.Gradovis.ToList();
                        return View();
                    }
                    else
                    {
                        ViewBag.Error = "Niste unijeli sve informacije potrebne za upis.";
                        ViewData["Gradovi"] = ctx.Gradovis.ToList();
                        return View();
                    }
                }
                catch (Exception)
                {
                    ViewBag.Error = "Greška prilikom snimanja linka!";
                    ViewData["Gradovi"] = ctx.Gradovis.ToList();
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
                ViewData["linkovi"] = ctx.InstitucijaLinkovis.Where(x => x.IsActive == true).OrderByDescending(x=>x.LinkID).ToList();
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }
        public ActionResult IzmjenaLinka(int ?Id)
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {
                int id = Id ?? default(int);
                InstitucijaLinkovi i = ctx.InstitucijaLinkovis.Where(x => x.LinkID == id).FirstOrDefault();
                if (i != null)
                {
                    ViewData["kantoni"] = ctx.Gradovis.ToList();
                    ViewData["link"] = i;
                    return View();
                }
                else
                    return RedirectToAction("Pregled", "Linkovi");
            }
            else
                return RedirectToAction("Index", "Login");
        }
        [HttpPost]
        public ActionResult IzmjenaLinka(int linkID, string naziv, string link, int grad)
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {
                InstitucijaLinkovi i = ctx.InstitucijaLinkovis.Where(x => x.LinkID == linkID).FirstOrDefault();
                if (i != null)
                {
                    try
                    {
                        naziv = AntiXSS.GetSafeHtml(naziv);
                        if (naziv != "")
                            i.NazivLinka = naziv;
                        link = AntiXSS.GetSafeHtml(link);
                        if (link != "")
                            i.Link = link;
                        i.GradID = grad;
                        ctx.SaveChanges();
                        ViewData["link"] = i;
                        ViewData["kantoni"] = ctx.Gradovis.ToList();
                        ViewBag.Success = "Uspješno ste izmijenili link!";
                        return View();
                    }
                    catch (Exception)
                    {
                        ViewData["link"] = i;
                        ViewData["kantoni"] = ctx.Gradovis.ToList();
                        ViewBag.Error = "Desila se greška pri izmjeni!";
                        return View();
                    }
                }
                else
                    return RedirectToAction("Pregled", "Linkovi");
            }
            else
                return RedirectToAction("Index", "Login");
        }
        public ActionResult BrisanjeLinka(int ?Id)
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {
                int id = Id ?? default(int);
                InstitucijaLinkovi i = ctx.InstitucijaLinkovis.Where(x => x.LinkID == id).FirstOrDefault();
                if (i != null)
                {
                    i.IsActive = false;
                    ctx.SaveChanges();
                    return RedirectToAction("Pregled", "Linkovi");
                }
                else
                    return RedirectToAction("Pregled", "Linkovi");
            }
            else
                return RedirectToAction("Index", "Login");
        }
    }
}
