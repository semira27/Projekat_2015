using Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace FMON.Controllers
{
    public class Donut
    {
        public string value { get; set; }
        public string label { get; set; }
    }
    public class AnketaController : Controller
    {
        //
        // GET: /Anketa/
        Context ctx = new Context();
        public ActionResult SnimiOdgovor(int ? odgovor)
        {
            int id = odgovor ?? default(int);
            string pcName = "";
            
            AnketaOdgovor o = ctx.AnketaOdgovors.Where(x => x.OdgovorID == id).SingleOrDefault();

            if (this.ControllerContext.HttpContext.Request.Cookies.AllKeys.Contains("Cookie"))
            {
                pcName = this.ControllerContext.HttpContext.Request.Cookies["Cookie"].Value;
            }

            if (o != null && (pcName != System.Environment.MachineName))
            {               
                o.BrojGlasova++;
                ctx.SaveChanges();

                HttpCookie cookie = new HttpCookie("Cookie");
                cookie.Value = System.Environment.MachineName;
                this.ControllerContext.HttpContext.Response.Cookies.Add(cookie);
                return RedirectToAction("Chart", "Anketa", new { Id = o.AnketaID });// temp lokejšn
            }
            return RedirectToAction("Index", "Index");            
        }
        [HttpGet]
        public ActionResult Chart(int? Id)
        {
            int id = Id ?? default(int);
            Anketa a = ctx.Anketas.Where(x => x.AnketaID == id && x.IsActive == true).SingleOrDefault();
            if (a != null)
            {
                int sum = 0;
                List<AnketaOdgovor> odgovori = ctx.AnketaOdgovors.Where(x => x.AnketaID == a.AnketaID && x.IsActive == true).ToList();
                foreach (AnketaOdgovor i in odgovori)
                {
                    sum += i.BrojGlasova;
                }
                ViewData["Anketa"] = a;
                ViewData["BrojGlasova"] = sum.ToString(); ;
                return View();
            }
            else
                return RedirectToAction("Index", "Index");
        }

        public JsonResult GetData(int? Id)
        {
            List<Donut> graphDataList = new List<Donut>();
            int id = Id ?? default(int);
            List<AnketaOdgovor> odgovori = ctx.AnketaOdgovors.Where(x => x.AnketaID == id && x.IsActive == true).ToList();
            if (odgovori != null)
            {
                foreach (AnketaOdgovor i in odgovori)
                {
                    Donut graphData = new Donut();
                    graphData.label = i.Naziv;
                    graphData.value = i.BrojGlasova.ToString();

                    graphDataList.Add(graphData);
                }
            }
            return Json(graphDataList, JsonRequestBehavior.AllowGet);
        }

    }
}
