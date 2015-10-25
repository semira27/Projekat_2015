namespace Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("InstitucijaLinkovi")]
    public partial class InstitucijaLinkovi
    {
        [Key]
        public int LinkID { get; set; }

        public int GradID { get; set; }
        public string NazivLinka { get; set; }

        public string Link { get; set; }

        public bool IsActive { get; set; }

        public int KorisnikID { get; set; }

        public virtual Gradovi Grad { get; set; }

        public virtual Korisnici Korisnici { get; set; }
    }
}
