namespace Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RegistarNostrifikacija")]
    public partial class RegistarNostrifikacija
    {
        public int RegistarNostrifikacijaID { get; set; }

        public string Naziv { get; set; }

        public string Lokacija { get; set; }

        public long Velicina { get; set; }

        [StringLength(50)]
        public string Tip { get; set; }

        public bool IsRegistar { get; set; }

        public bool IsActive { get; set; }

        public int KorisnikID { get; set; }

        public virtual Korisnici Korisnici { get; set; }
    }
}
