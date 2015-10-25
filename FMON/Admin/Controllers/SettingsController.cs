using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data.Model;
using System.Web.UI.WebControls;
using Admin.Configuration;

namespace Admin.Controllers
{
    public class SettingsController : Controller
    {
        //
        // GET: /Settings/
        Context ctx = new Context();
        public ActionResult Index()
        {
            ViewData["settings"] = ctx.Postavkes.FirstOrDefault();
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(string isActive, string referentniMail1, string referentniMail2, string referentniMail3, string mostarUlica,
            string mostarTel1, string mostarTel2, string mostarfax1, string mostarfax2, string mostarinfo1, string mostarinfo2,
            string saulica, string satel1, string satel2, string safax1, string safax2, string sainfo1, string sainfo2,
            string youtube, string twitter, string facebook,

            string naslovna, string dokumenti, string nostrifikacija, string pitanjaOdgovori, string linkovi, string zajmovi, string registar,

            string NaslovnaisActive, string dokumentiisActive, string nostrifikacijaisActive, string pitanjaOdgovoriisActive, string linkoviisActive, string zajmoviisActive, string registarisActive
            
            )
        {
            try
            {
                Postavke settings = ctx.Postavkes.First();


                naslovna = AntiXSS.GetSafeHtml(naslovna);
                dokumenti = AntiXSS.GetSafeHtml(dokumenti);
                nostrifikacija = AntiXSS.GetSafeHtml(nostrifikacija);
                pitanjaOdgovori = AntiXSS.GetSafeHtml(pitanjaOdgovori);
                linkovi = AntiXSS.GetSafeHtml(linkovi);
                zajmovi = AntiXSS.GetSafeHtml(zajmovi);
                registar = AntiXSS.GetSafeHtml(registar);

                if (naslovna != "")
                    settings.NaslovnaTekst = naslovna;

                if (dokumenti != "")
                    settings.DokumentiTekst = dokumenti;

                if (nostrifikacija != "")
                    settings.NostrifikacijaTekst = nostrifikacija;

                if (pitanjaOdgovori != "")
                    settings.PitanjaOdgovoriTekst  = pitanjaOdgovori;

                if (linkovi != "")
                    settings.LinkoviTekst = linkovi;

                if (zajmovi != "")
                    settings.ZajmoviTekst = zajmovi;

                if (registar != "")
                    settings.RegistarTekst = registar;

                if (NaslovnaisActive == "on")
                    settings.NaslovnaIsActive = true;
                else
                    settings.NaslovnaIsActive = false;


                if (dokumentiisActive == "on")
                    settings.DokumentiIsActive = true;
                else
                    settings.DokumentiIsActive = false;

                if (nostrifikacijaisActive == "on")
                    settings.NostrifikacijaIsActive = true;
                else
                    settings.NostrifikacijaIsActive = false;

                if (pitanjaOdgovoriisActive == "on")
                    settings.PitanjaOdgovoriIsActive = true;
                else
                    settings.PitanjaOdgovoriIsActive = false;

                if (linkoviisActive == "on")
                    settings.LinkoviIsActive = true;
                else
                    settings.LinkoviIsActive = false;

                if (zajmoviisActive == "on")
                    settings.ZajmoviIsActive = true;
                else
                    settings.ZajmoviIsActive = false;

                if (registarisActive == "on")
                    settings.RegistarIsActive = true;
                else
                    settings.RegistarIsActive = false;

                /*------------------------------------------------------------*/

                if (isActive == "on")
                    settings.IsActivePostaviPitanje = true;
                else
                    settings.IsActivePostaviPitanje = false;

                referentniMail1 = AntiXSS.GetSafeHtml(referentniMail1);
                if (referentniMail1 != "")
                    settings.ReferentniMail1 = referentniMail1;

                referentniMail2 = AntiXSS.GetSafeHtml(referentniMail2);
                if (referentniMail2 != "")
                    settings.ReferentniMail2 = referentniMail2;

                referentniMail3 = AntiXSS.GetSafeHtml(referentniMail3);
                if (referentniMail3 != "")
                    settings.ReferentniMail3 = referentniMail3;

                mostarUlica = AntiXSS.GetSafeHtml(mostarUlica);
                if (mostarUlica != "")
                    settings.UredMostarUlica = mostarUlica;

                mostarTel1 = AntiXSS.GetSafeHtml(mostarTel1);
                if (mostarTel1 != "")
                    settings.UredMostarTelefon1 = mostarTel1;

                mostarTel2 = AntiXSS.GetSafeHtml(mostarTel2);
                if (mostarTel2 != "")
                    settings.UredMostarTelefon2 = mostarTel2;

                mostarfax1 = AntiXSS.GetSafeHtml(mostarfax1);
                if (mostarfax1 != "")
                    settings.UredMostarFax1 = mostarfax1;

                mostarfax2 = AntiXSS.GetSafeHtml(mostarfax2);
                if (mostarfax2 != "")
                    settings.UredMostarFax2 = mostarfax2;

                mostarinfo1 = AntiXSS.GetSafeHtml(mostarinfo1);
                if (mostarinfo1 != "")
                    settings.UredMostarInfo1 = mostarinfo1;

                mostarinfo2 = AntiXSS.GetSafeHtml(mostarinfo2);
                if (mostarinfo2 != "")
                    settings.UredMostarInfo2 = mostarinfo2;

                saulica = AntiXSS.GetSafeHtml(saulica);
                if (saulica != "")
                    settings.UredSarajevoUlica = saulica;

                satel1 = AntiXSS.GetSafeHtml(satel1);
                if (satel1 != "")
                    settings.UredSarajevoTelefon1 = satel1;

                satel2 = AntiXSS.GetSafeHtml(satel2);
                if (satel2 != "")
                    settings.UredSarajevoTelefon2 = satel2;

                safax1 = AntiXSS.GetSafeHtml(safax1);
                if (safax1 != "")
                    settings.UredSarajevoFax1 = safax1;

                safax2 = AntiXSS.GetSafeHtml(safax2);
                if (safax2 != "")
                    settings.UredSarajevoFax2 = safax2;

                sainfo1 = AntiXSS.GetSafeHtml(sainfo1);
                if (sainfo1 != "")
                    settings.UredSarajevoInfo1 = sainfo1;

                sainfo2 = AntiXSS.GetSafeHtml(sainfo2);
                if (sainfo2 != "")
                    settings.UredSarajevoInfo2 = sainfo2;

                youtube = AntiXSS.GetSafeHtml(youtube);
                if (youtube != "")
                    settings.LinkYoutube = youtube;

                twitter = AntiXSS.GetSafeHtml(twitter);
                if (twitter != "")
                    settings.LinkTwitter = twitter;

                facebook = AntiXSS.GetSafeHtml(facebook);
                if (facebook != "")
                    settings.LinkFacebook = facebook;

                ctx.SaveChanges();

                ViewData["settings"] = ctx.Postavkes.FirstOrDefault();
                ViewBag.Success = "Uspješno ste izmijenili postavke!";
                return View();
            }
            catch (Exception)
            {
                ViewBag.Success = "Desila se greška pri izmjeni!";
                return View();
            }
        }
    }
}
