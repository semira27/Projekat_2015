namespace Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Korisnici")]
    public partial class Korisnici
    {
        public Korisnici()
        {
            Anketas = new HashSet<Anketa>();
            Dokumentis = new HashSet<Dokumenti>();
            InstitucijaLinkovis = new HashSet<InstitucijaLinkovi>();
            KalendarObavijestis = new HashSet<KalendarObavijesti>();
            Konkursis = new HashSet<Konkursi>();
            Najaves = new HashSet<Najave>();
            Obavijestis = new HashSet<Obavijesti>();
            RegistarNostrifikacijas = new HashSet<RegistarNostrifikacija>();
            VideoMaterijalis = new HashSet<VideoMaterijali>();
        }

        [Key]
        public int KorisnikID { get; set; }

        public string Ime { get; set; }

        public string Prezime { get; set; }

        public string KorisnickoIme { get; set; }

        public string LozinkaHash { get; set; }

        public string LozinkaSalt { get; set; }

        public string Email { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<Anketa> Anketas { get; set; }

        public virtual ICollection<Dokumenti> Dokumentis { get; set; }

        public virtual ICollection<InstitucijaLinkovi> InstitucijaLinkovis { get; set; }

        public virtual ICollection<KalendarObavijesti> KalendarObavijestis { get; set; }

        public virtual ICollection<Konkursi> Konkursis { get; set; }

        public virtual ICollection<Najave> Najaves { get; set; }

        public virtual ICollection<Obavijesti> Obavijestis { get; set; }

        public virtual ICollection<RegistarNostrifikacija> RegistarNostrifikacijas { get; set; }

        public virtual ICollection<VideoMaterijali> VideoMaterijalis { get; set; }
    }
}
