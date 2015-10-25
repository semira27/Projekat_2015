namespace Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KonkursiDokumenti")]
    public partial class KonkursiDokumenti
    {
        public int KonkursiDokumentiID { get; set; }

        public string Naziv { get; set; }

        public string TipDokumenta { get; set; }

        public string Putanja { get; set; }

        public long Velicina { get; set; }

        public bool IsActive { get; set; }

        public int KonkursID { get; set; }

        public virtual Konkursi Konkursi { get; set; }
    }
}
