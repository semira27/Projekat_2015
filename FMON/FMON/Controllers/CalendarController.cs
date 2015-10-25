using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data.Model;
using System.Globalization;
namespace FMON.Controllers
{
    public class Kalendar
    {
        public string date { get; set; }
        public string title { get; set; }
        public bool badge { get; set; }
    }

    public class Kalendar2
    {
        public string date { get; set; }
        public bool badge { get; set; }
        public string title { get; set; }     
        public string body { get; set; }
        public string footer { get; set; }
        public string classname { get; set; } 
    }

    public class CalendarController : Controller
    {
        //
        // GET: /Calendar/
        Context ctx = new Context();
        public JsonResult GetData()
        {
            List<Kalendar> graphDataList = new List<Kalendar>();

            List<KalendarObavijesti> kal = ctx.KalendarObavijestis.Where(x => x.IsActive == true).ToList();
            if (kal != null)
            {
                foreach (KalendarObavijesti i in kal)
                {
                    Kalendar graphData = new Kalendar();
                    graphData.title = i.Obavijest;
                    graphData.badge = false;
                    graphData.date = i.Datum.ToString("yyyy-MM-dd");
                    graphDataList.Add(graphData);
                }
            }
            return Json(graphDataList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetData2()
        {
            List<Kalendar2> graphDataList = new List<Kalendar2>();

            List<KalendarObavijesti> kal = ctx.KalendarObavijestis.Where(x => x.IsActive == true).ToList();
            if (kal != null)
            {
                foreach (KalendarObavijesti i in kal)
                {
                    Kalendar2 graphData = new Kalendar2();
                    graphData.title = i.Naslov;
                    graphData.badge = true;
                    graphData.date = i.Datum.ToString("yyyy-MM-dd");
                    graphData.body = i.Obavijest;
                    graphData.classname = "purple-event";
                    graphData.footer = "";
                    graphDataList.Add(graphData);
                }
            }
            return Json(graphDataList, JsonRequestBehavior.AllowGet);
        }

    }
}
