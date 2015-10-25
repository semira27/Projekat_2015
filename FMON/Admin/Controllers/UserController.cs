using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data.Model;
using Admin.Helper;
using Admin.Configuration;

namespace Admin.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/
        Context ctx = new Context();

        public ActionResult AddUser()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddUser(string ime, string prezime, string email,
            string korisnickoIme, string lozinka, string potvrda)
        {
            Korisnici ko = HttpContext.Session["korisnik"] as Korisnici;
            if (ko != null)
            {
                try
                {
                    Korisnici k = new Korisnici();
                    int err = 0;
                    ime = AntiXSS.GetSafeHtml(ime);
                    if (ime != "")
                        k.Ime = ime;
                    else
                        err++;
                    prezime = AntiXSS.GetSafeHtml(prezime);
                    if (prezime != "")
                        k.Prezime = prezime;
                    else
                        err++;
                    email = AntiXSS.GetSafeHtml(email);
                    if (email != "")
                        k.Email = email;
                    else
                        err++;
                    korisnickoIme = AntiXSS.GetSafeHtml(korisnickoIme);
                    if (korisnickoIme != "")
                        k.KorisnickoIme = korisnickoIme;
                    else
                        err++;
                    lozinka = AntiXSS.GetSafeHtml(lozinka);
                    if (lozinka != "")
                    {
                        k.LozinkaSalt = HashSaltGenerator.GenerateSalt();
                        k.LozinkaHash = HashSaltGenerator.GenerateHash(lozinka, k.LozinkaSalt);
                    }
                    else
                        err++;
                    
                    k.IsActive = true;

                    potvrda = AntiXSS.GetSafeHtml(potvrda);
                    if (lozinka != potvrda)
                    {
                        ViewBag.Error = "Lozinka i potvrda lozinke se ne podudaraju!";
                        return View();
                    }
                    else 
                    {
                        if (err > 0)
                        {
                            ViewBag.Error = "Niste unijeli sve informacije potrebne za upis!";
                            return View();
                        }
                        else
                        {
                            ctx.Korisnicis.Add(k);
                            ctx.SaveChanges(); 
                            ViewBag.Success = "Uspješno ste snimili novog korisnika!";
                            return View();
                        }
                    }

                }
                catch (Exception)
                {
                    ViewBag.Error = "Greška prilikom snimanja korisnika!";
                    return View();
                }

            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult SviKorisnici()
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {
                ViewData["korisnici"] = ctx.Korisnicis.Where(x => x.IsActive == true).OrderByDescending(x=>x.Prezime).ToList();
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult BrisanjeKorisnika(int? Id)
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null && Id != null)
            {
                try
                {
                    Korisnici ko = ctx.Korisnicis.Where(x => x.KorisnikID == Id).FirstOrDefault();
                    ko.IsActive = false;
                    ctx.SaveChanges();
                    return RedirectToAction("SviKorisnici", "User");
                }
                catch (Exception)
                {
                       ViewBag.Error = "Korisnik nije obrisan!";                        
                       return RedirectToAction("SviKorisnici", "User");
                }
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult IzmjenaKorisnika(int? Id)
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null && Id != null)
            {
                ViewData["korisnik"] = ctx.Korisnicis.Where(x => x.KorisnikID == Id).FirstOrDefault();
                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult IzmjenaKorisnika(string ime, string prezime, string email,
            string korisnickoIme, string lozinka, string potvrda, int korisnikID)
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {
                Korisnici ko = ctx.Korisnicis.Where(x => x.KorisnikID == korisnikID).FirstOrDefault();
                if(ko != null)
                { 
                    try
                    {
                        ime = AntiXSS.GetSafeHtml(ime);
                        if (ime != "")
                            ko.Ime = ime;
                        prezime = AntiXSS.GetSafeHtml(prezime);
                        if (prezime != "")
                            ko.Prezime = prezime;
                        email = AntiXSS.GetSafeHtml(email);
                        if (email != "")
                            ko.Email = email;
                        korisnickoIme = AntiXSS.GetSafeHtml(korisnickoIme);
                        if (korisnickoIme != "")
                            ko.KorisnickoIme = korisnickoIme;
                        lozinka = AntiXSS.GetSafeHtml(lozinka);
                        potvrda = AntiXSS.GetSafeHtml(potvrda);
                        if (lozinka != potvrda)
                        {
                            ViewBag.Error = "Lozinka i potvrda lozinke se ne podudaraju!";
                            ViewData["korisnik"] = ko;
                            return View();
                        }

                        if (lozinka != "")
                        {
                            ko.LozinkaSalt = HashSaltGenerator.GenerateSalt();
                            ko.LozinkaHash = HashSaltGenerator.GenerateHash(lozinka, ko.LozinkaSalt);
                        }
                        ctx.SaveChanges();
                        ViewBag.Success = "Uspješno ste izmijenili korisnika!";
                        ViewData["korisnik"] = ko;
                        return View();
                    }
                    catch (Exception)
                    {
                        ViewData["korisnik"] = ko;
                        ViewBag.Error = "Desila se greška pri izmjeni!";
                        return View();
                    }
              }
                else
                    return RedirectToAction("Pregled", "User");
            }
            else
                return RedirectToAction("Index", "Login");
        }
    }
}
