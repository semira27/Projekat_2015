using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data.Model;
using Admin.Helper;
using System.Web.Security;

namespace Admin.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/
        Context ctx = new Context();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string username, string password)
        {
            Korisnici user = ctx.Korisnicis.Where(x => x.KorisnickoIme == username && x.IsActive == true).FirstOrDefault();
            if (user != null)
            {
                string lozinka = HashSaltGenerator.GenerateHash(password, user.LozinkaSalt);
                if (lozinka == user.LozinkaHash)
                {
                    //FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(user.KorisnikID.ToString(), false, 30);
                    //string encryptTicket = FormsAuthentication.Encrypt(ticket);
                    //HttpCookie loginCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptTicket);
                    //Response.Cookies.Add(loginCookie);
                    Session.Add("korisnik", user);
                    return RedirectToAction("Home", "Home");

                }
            }
            else
            {
                //očisti formu pošalji odg poruku
            }

            return View();
        }
        public ActionResult Logout()
        {
            if (HttpContext.Session["korisnik"] != null)
                HttpContext.Session.Remove("korisnik");

            return RedirectToAction("Index", "Login");

        }
    }
}
