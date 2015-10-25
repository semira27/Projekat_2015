using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Data.Model;
namespace FMON.Controllers
{
    public class ZajmoviController : Controller
    {
        //
        // GET: /Zajmovi/

        Context ctx = new Context();
        public ActionResult Index(int? page)
        {
            int pageSize = 10;
            int pageNum = (page ?? 1);

            Sektori s = ctx.Sektoris.Where(x=>x.IsStudentskiZajam == true).SingleOrDefault();

            List<ObavijestiSektori> os = ctx.ObavijestiSektoris.Where(x => x.SektorID == s.SektorID).ToList();

            List<Obavijesti> o = new List<Obavijesti>();

            foreach (ObavijestiSektori item in os)
            {
                Obavijesti ob = ctx.Obavijestis.Where(x => x.ObavijestID == item.ObavijestID).SingleOrDefault();

                if (ob != null)
                {
                    o.Add(ob);
                }
            }
            return View(o.ToPagedList(pageNum, pageSize));
        }

    }
}
