namespace Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Dokumenti")]
    public partial class Dokumenti
    {
        [Key]
        public int DokumentID { get; set; }

        public int ? DokumentKategorijaID { get; set; }

        [Column(TypeName = "date")]
        public DateTime DatumObjave { get; set; }

        public string Lokacija { get; set; }

        public string TipDokumenta { get; set; }

        public string Naziv { get; set; }

        public bool IsActive { get; set; }

        public int KorisnikID { get; set; }

        public long Velicina { get; set; }

        public virtual DokumentiKategorije DokumentiKategorije { get; set; }

        public virtual Korisnici Korisnici { get; set; }
    }
}
