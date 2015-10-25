using Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
namespace FMON.Controllers
{
    public class RegistarPController : Controller
    {
        //
        // GET: /RegistarP/
        Context ctx = new Context();
        public ActionResult Index(int? page)
        {
            int pageSize = 20;
            int pageNum = (page ?? 1);

            List<RegistarNostrifikacija> lista  = ctx.RegistarNostrifikacijas.Where(x => x.IsActive == true && x.IsRegistar == true).ToList();
            return View(lista.ToPagedList(pageNum, pageSize));
        }

    }
}
