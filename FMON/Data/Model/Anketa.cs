namespace Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Anketa")]
    public partial class Anketa
    {
        public Anketa()
        {
            AnketaOdgovors = new HashSet<AnketaOdgovor>();
        }

        public int AnketaID { get; set; }

        public string Pitanje { get; set; }

        [Column(TypeName = "date")]
        public DateTime DatumObjave { get; set; }

        public bool IsActive { get; set; }

        public int KorisnikID { get; set; }

        public virtual Korisnici Korisnici { get; set; }

        public virtual ICollection<AnketaOdgovor> AnketaOdgovors { get; set; }
    }
}
