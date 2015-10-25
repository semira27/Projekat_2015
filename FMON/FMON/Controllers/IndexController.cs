using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data.Model;
using System.Net;
using Newtonsoft.Json;
using Admin.Configuration;
namespace FMON.Controllers
{
    public class IndexController : Controller
    {
        //
        // GET: /Index/

        class ReCaptchaClass
        {
            [JsonProperty("success")]
            public bool Success { get; set; }
        }
        Context ctx = new Context();
        public ActionResult Index()
        {

            ViewData["istaknuto"] = ctx.Obavijestis.Where(x => x.IsIstaknuta == true && x.IsActive == true && x.Nova==true).OrderByDescending(x => x.Datum).Take(3).ToList();
            ViewData["obavijesti"] = ctx.Obavijestis.Where(x => x.IsActive == true && x.IsIstaknuta == false && x.Nova == true).OrderByDescending(x => x.Datum).Take(5).ToList();
            ViewData["Kontakt"] = ctx.Postavkes.First();

            ViewData["Konkursi"] = ctx.Konkursis.Where(x => x.IsActive == true && x.IsMedjunarodni == false).OrderByDescending(x => x.Datum).Take(10).ToList();
            ViewData["Medjunarodni"] = ctx.Konkursis.Where(x => x.IsActive == true && x.IsMedjunarodni == true).OrderByDescending(x => x.Datum).Take(3).ToList();
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(string pitanje)
        {
            string g_resp = Request["g-recaptcha-response"];

            string url = string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", "6LehfgkTAAAAAJ3C8Rs5puUYFU8ln8om40ov-T7_", g_resp);

            WebClient wc = new WebClient();
            string str = wc.DownloadString(new Uri(url));

            var str2 = JsonConvert.DeserializeObject<ReCaptchaClass>(str);
            string Pitanje = AntiXSS.GetSafeHtml(pitanje);

            if (str2.Success && Pitanje != "")
            {
                try
                { 
                
                PostaviPitanje p = new PostaviPitanje();
                p.Datum = DateTime.Now;
                p.IsActive = true;
                p.Pitanje = pitanje;
                p.PitanjePregledano = false;

                ctx.PostaviPitanjes.Add(p);
                ctx.SaveChanges();
                ViewBag.Success = "Uspiješno ste postavili pitanje";
                }
                catch (Exception)
                {
                    ViewBag.Error ="Pitanje nije snimljeno!";
                    return View();
                }
               
            }
            else
            {
                ViewBag.Error = "Pitanje nije snimljeno!";
            }

            ViewData["istaknuto"] = ctx.Obavijestis.Where(x => x.IsIstaknuta == true && x.IsActive == true && x.Nova == true).OrderByDescending(x => x.Datum).Take(3).ToList();
            ViewData["obavijesti"] = ctx.Obavijestis.Where(x => x.IsActive == true && x.IsIstaknuta == false && x.Nova == true).OrderByDescending(x => x.Datum).Take(5).ToList();
            ViewData["Kontakt"] = ctx.Postavkes.First();

            ViewData["Konkursi"] = ctx.Konkursis.Where(x => x.IsActive == true && x.IsMedjunarodni == false).OrderByDescending(x => x.Datum).Take(10).ToList();
            ViewData["Medjunarodni"] = ctx.Konkursis.Where(x => x.IsActive == true && x.IsMedjunarodni == true).OrderByDescending(x => x.Datum).Take(3).ToList();
            return  View();
        }
    }
}
