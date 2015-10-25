namespace Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ObavijestiDokumenti")]
    public partial class ObavijestiDokumenti
    {
        [Key]
        public int ObavjestiDokumentID { get; set; }

        public int ? ObavjestID { get; set; }

        public string NazivDokumenta { get; set; }

        public string TipDokumenta { get; set; }

        public long Velicina { get; set; }

        public string Lokacija { get; set; }

        public bool IsActive { get; set; }

        public virtual Obavijesti Obavijesti { get; set; }
    }
}
