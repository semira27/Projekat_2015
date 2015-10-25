using Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMON.Controllers
{
    public class DokumentController : Controller
    {
        //
        // GET: /Dokument/
        Context ctx = new Context();
        public ActionResult Index()
        {
           
            ViewData["dokumenti"] = ctx.Dokumentis.Where(x=>x.IsActive==true).ToList();
            ViewData["kategorije"] = ctx.DokumentiKategorijes.Where(x => x.IsActive == true).ToList();
            return View();
        }

    }
}
