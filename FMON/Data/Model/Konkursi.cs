namespace Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Konkursi")]
    public partial class Konkursi
    {
        public Konkursi()
        {
            KonkursiDokumentis = new HashSet<KonkursiDokumenti>();
        }

        [Key]
        public int KonkursID { get; set; }

        public string Naziv { get; set; }

        public string Obavijest { get; set; }

        [Column(TypeName = "date")]
        public DateTime Datum { get; set; }

        public bool IsMedjunarodni { get; set; }

        public bool IsActive { get; set; }

        public int KorisnikID { get; set; }

        public virtual Korisnici Korisnici { get; set; }

        public virtual ICollection<KonkursiDokumenti> KonkursiDokumentis { get; set; }
    }
}
