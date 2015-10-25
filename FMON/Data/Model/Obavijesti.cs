namespace Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Obavijesti")]
    public partial class Obavijesti
    {
        public Obavijesti()
        {
            ObavijestiDokumentis = new HashSet<ObavijestiDokumenti>();
            ObavijestiSektoris = new HashSet<ObavijestiSektori>();
            ObavijestiSlikes = new HashSet<ObavijestiSlike>();
        }

        [Key]
        public int ObavijestID { get; set; }

        public string NaslovObavijesti { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime Datum { get; set; }

        public string Sazetak { get; set; }

        public string Obavijest { get; set; }

        public bool IsIstaknuta { get; set; }
        public bool Nova { get; set; }

        public bool IsActive { get; set; }

        public int KorisnikID { get; set; }

        public string SlikaObavijesti { get; set; }

        public virtual Korisnici Korisnici { get; set; }

        public virtual ICollection<ObavijestiDokumenti> ObavijestiDokumentis { get; set; }

        public virtual ICollection<ObavijestiSektori> ObavijestiSektoris { get; set; }

        public virtual ICollection<ObavijestiSlike> ObavijestiSlikes { get; set; }
    }
}
