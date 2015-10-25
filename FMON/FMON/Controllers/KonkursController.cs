using Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMON.Controllers
{
    public class KonkursController : Controller
    {
        //
        // GET: /Konkurs/
        Context ctx = new Context();
        public ActionResult Index(int ? Id)
        {
            if (Id != null)
            {

                int id = Id ?? default(int);

                Konkursi k = ctx.Konkursis.Where(x => x.KonkursID == id).SingleOrDefault();

                if (k != null)
                {
                    return View(k);
                }              
            }
            return RedirectToAction("Index", "Index");
        }

    }
}
