namespace Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Najave")]
    public partial class Najave
    {
        [Key]
        public int NajavaID { get; set; }

        [Column(TypeName = "date")]
        public DateTime Datum { get; set; }

        public string Sazetak { get; set; }

        public string Vijest { get; set; }
        public bool IsActive { get; set; }

        public int KorisnikID { get; set; }

        public virtual Korisnici Korisnici { get; set; }
    }
}
