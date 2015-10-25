using Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
namespace FMON.Controllers
{
    public class SektorController : Controller
    {
        //
        // GET: /Sektori/
        Context ctx = new Context(); 
        public ActionResult Index(int ? Id,int ? page)
        {
            int pageSize = 5;
            int pageNum = (page ?? 1);

            ViewBag.Sektori = ctx.Sektoris.Where(x => x.IsActive == true).ToList();

            if(Id!=null)
            {
                int id = Id ?? default(int);
                Sektori s = ctx.Sektoris.Where(x => x.SektorID == id).SingleOrDefault();

                if (s != null)
                {
                    ViewData["Sektor"] = s;

                    List<ObavijestiSektori> sek = ctx.ObavijestiSektoris.Where(x => x.SektorID == s.SektorID && x.IsActive == true).OrderByDescending(x=>x.ObavijestSektorID).ToList();

                    List<Obavijesti> obavijesti = new List<Obavijesti>();

                    foreach (ObavijestiSektori item in sek)
                    {
                        Obavijesti o = ctx.Obavijestis.Where(x => x.ObavijestID == item.ObavijestID && x.IsActive == true).SingleOrDefault();

                        if (o != null)
                            obavijesti.Add(o);
                    }
                    return View(obavijesti.ToPagedList(pageNum, pageSize));
                }                                    
            } 
            return RedirectToAction("Index", "Index");
     
        }

    }
}
