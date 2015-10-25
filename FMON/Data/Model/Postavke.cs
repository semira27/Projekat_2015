namespace Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Postavke")]
    public partial class Postavke
    {
        public int PostavkeID { get; set; }

        public bool IsActivePostaviPitanje { get; set; }

        public string ReferentniMail1 { get; set; }

        public string ReferentniMail2 { get; set; }

        public string ReferentniMail3 { get; set; }

        public string UredMostarUlica { get; set; }
        public string UredMostarTelefon1{ get; set; }
        public string UredMostarTelefon2 { get; set; }
        public string UredMostarFax1 { get; set; }
        public string UredMostarFax2 { get; set; }
        public string UredMostarInfo1 { get; set; }
        public string UredMostarInfo2 { get; set; }

        public string UredSarajevoUlica { get; set; }
        public string UredSarajevoTelefon1 { get; set; }
        public string UredSarajevoTelefon2 { get; set; }
        public string UredSarajevoFax1 { get; set; }
        public string UredSarajevoFax2 { get; set; }
        public string UredSarajevoInfo1 { get; set; }
        public string UredSarajevoInfo2 { get; set; }


        public string LinkYoutube { get; set; }
        public string LinkTwitter { get; set; }
        public string LinkFacebook { get; set; }

        public string NaslovnaTekst { get; set; }
        public string DokumentiTekst { get; set; }
        public string NostrifikacijaTekst { get; set; }
        public string PitanjaOdgovoriTekst { get; set; }
        public string LinkoviTekst { get; set; }
        public string ZajmoviTekst { get; set; }
        public string RegistarTekst { get; set; }

        public bool NaslovnaIsActive { get; set; }
        public bool DokumentiIsActive { get; set; }
        public bool NostrifikacijaIsActive { get; set; }
        public bool PitanjaOdgovoriIsActive { get; set; }
        public bool LinkoviIsActive { get; set; }
        public bool ZajmoviIsActive { get; set; }
        public bool RegistarIsActive { get; set; }

    }
}
