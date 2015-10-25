using Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMON.Controllers
{
    public class NajavaController : Controller
    {
        //
        // GET: /Najava/
        Context ctx = new Context();
        public ActionResult Vijest(int ? Id)
        {
            if (Id != null)
            {

                int id = Id ?? default(int);
                Najave n = ctx.Najaves.Where(x => x.IsActive == true && x.NajavaID == id).SingleOrDefault();
                if(n !=null)
                {
                    ViewData["najava"] = n;

                    return View();
                }                
            }
            return RedirectToAction("Index","Index");
           
        }

    }
}
