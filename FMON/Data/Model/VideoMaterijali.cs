namespace Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VideoMaterijali")]
    public partial class VideoMaterijali
    {
        public int VideoMaterijaliID { get; set; }

        public string Naziv { get; set; }

        public string Link { get; set; }

        public bool IsActive { get; set; }

        public int KorisnikID { get; set; }

        public virtual Korisnici Korisnici { get; set; }
    }
}
