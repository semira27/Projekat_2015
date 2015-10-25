namespace Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KalendarObavijesti")]
    public partial class KalendarObavijesti
    {
        [Key]
        public int KalendarID { get; set; }

        [Column(TypeName = "date")]
        public DateTime Datum { get; set; }

        public string Obavijest { get; set; }

        public bool IsActive { get; set; }
        public string Naslov { get; set; }

        public int KorisnikID { get; set; }

        public virtual Korisnici Korisnici { get; set; }

    }
}
