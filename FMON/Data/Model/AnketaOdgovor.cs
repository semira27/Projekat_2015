namespace Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AnketaOdgovor")]
    public partial class AnketaOdgovor
    {
        [Key]
        public int OdgovorID { get; set; }

        public int AnketaID { get; set; }

        [StringLength(250)]
        public string Naziv { get; set; }

        public int BrojGlasova { get; set; }

        public bool IsActive { get; set; }

        public virtual Anketa Anketa { get; set; }
    }
}
