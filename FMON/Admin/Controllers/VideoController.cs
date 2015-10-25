using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data.Model;
namespace Admin.Controllers
{
    public class VideoController : Controller
    {
        //
        // GET: /Video/
        Context ctx = new Context();
        [HttpGet]
        public ActionResult SnimiVideo()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SnimiVideo(string naziv, string link)
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {
                VideoMaterijali video = new VideoMaterijali();
                if (naziv != "" || link != "")
                {
                    video.Naziv = naziv;
                    video.Link = link;
                    video.IsActive = true;
                    video.KorisnikID = k.KorisnikID;
                    ctx.VideoMaterijalis.Add(video);
                    ctx.SaveChanges();

                    ViewBag.Success = "Uspiješno ste snimili video materijal";
                    return View();
                    
                }
                else
                {
                    ViewBag.Error = "Niste unijeli sve informacije potrebne za upis";
                    return View();
                }
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult IzmijeniVideo()
        {
             Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
             if (k != null)
             {
                 ViewData["videoZapisi"] = ctx.VideoMaterijalis.Where(x => x.IsActive == true).OrderByDescending(x=>x.VideoMaterijaliID).ToList();
                 return View();
             }
             else
                 return RedirectToAction("Index", "Login");
        }

        public ActionResult BrisanjeVideo(int? Id)
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {
                if (Id != null)
                {
                    int id = Id ?? default(int);
                    VideoMaterijali v = ctx.VideoMaterijalis.Where(x => x.VideoMaterijaliID == id).SingleOrDefault();

                    v.IsActive = false;
                    ctx.SaveChanges();
                    return RedirectToAction("IzmijeniVideo", "Video");
                }               
            }
            return RedirectToAction("Index", "Login");
        
        }

        public ActionResult Izmjena(int? Id)
        {
             Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
             if (k != null)
             {
                 if (Id != null)
                 {

                     int id = Id ?? default(int);

                     VideoMaterijali video = ctx.VideoMaterijalis.Where(x => x.VideoMaterijaliID == id).SingleOrDefault();

                     if (video != null)
                     {
                         ViewData["Video"] = video;
                         return View();
                     }
                     else
                     {
                         return RedirectToAction("IzmijeniVideo", "Video");
                     }
                 }
                 return RedirectToAction("IzmijeniVideo", "Video");
             }
                 return RedirectToAction("Index", "Login");         
        }
        [HttpPost]
        public ActionResult Izmjena(string naziv, string link, int VideoID)
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            VideoMaterijali video = ctx.VideoMaterijalis.Where(x => x.VideoMaterijaliID == VideoID).SingleOrDefault();
            if (k != null && video != null)
            {
                try
                {
                    if(naziv!="")
                    video.Naziv = naziv;
                    if(link!="")
                    video.Link = link;
                    ctx.SaveChanges();
                    ViewData["Video"] = video;
                    ViewBag.Success = "Uspješno ste izmijenili video materijal";
                    return View();
                }
                catch (Exception)
                {
                    ViewBag.Error = "Desila se greška pri izmjeni!";
                    return RedirectToAction("Izmjena", "Video", new { Id = video.VideoMaterijaliID });
                }
               
            }
            return RedirectToAction("Index", "Login"); 
        }

        [HttpPost]
        public ActionResult Uklanjanje(int ? videoID)
        {
            Korisnici k = HttpContext.Session["korisnik"] as Korisnici;
            if (k != null)
            {
                if (videoID != null)
                {
                    int id = videoID ?? default(int);
                    VideoMaterijali vm = ctx.VideoMaterijalis.Where(x => x.VideoMaterijaliID == id).FirstOrDefault();
                    if (vm != null)
                    {
                        try
                        {
                            vm.IsActive = false;
                            ctx.SaveChanges();
                            ViewData["videoZapisi"] = ctx.VideoMaterijalis.Where(x => x.IsActive == true).ToList();
                            return RedirectToAction("IzmijeniVideo", "Video");
                        }
                        catch (Exception)
                        {
                            ViewBag.Error = "Video nije obrisan!";
                            return RedirectToAction("IzmijeniVideo", "Video");
                        }
                    }
                }
            }
            return RedirectToAction("Index", "Login");
        }
    }
}
