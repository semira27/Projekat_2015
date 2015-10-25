namespace Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DokumentiKategorije")]
    public partial class DokumentiKategorije
    {
        public DokumentiKategorije()
        {
            Dokumentis = new HashSet<Dokumenti>();
        }

        [Key]
       // [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int KategorijaDokumentaID { get; set; }

        public string Naziv { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<Dokumenti> Dokumentis { get; set; }
    }
}
