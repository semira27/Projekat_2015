using FMON.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data.Model;
using PagedList;
namespace FMON.Controllers
{
    public class PretragaController : Controller
    {
        //
        // GET: /Pretraga/
        Context ctx = new Context();
        [ValidateInput(false)]
        public ActionResult Index(string pitanje, int? page)
        {
            int pageSize = 5;
            int pageNum = (page ?? 1);

            string Pitanje = AntiXSS.GetSafeHtml(pitanje);

            List<Obavijesti> o = ctx.Obavijestis.Where(x => x.NaslovObavijesti.Contains(Pitanje)).OrderByDescending(x=>x.Datum).ToList();
            return View(o.ToPagedList(pageNum, pageSize));
            
        }

    }
}
