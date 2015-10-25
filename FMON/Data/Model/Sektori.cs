namespace Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Sektori")]
    public partial class Sektori
    {
        public Sektori()
        {
            ObavijestiSektoris = new HashSet<ObavijestiSektori>();
        }

        [Key]
        public int SektorID { get; set; }

        public string Slika { get; set; }

        public string IkonaSektora { get; set; }

        public string OSektoru { get; set; }

        public int Redoslijed { get; set; }
        public string NazivSektora { get; set; }

        public bool IsActive { get; set; }

        public bool IsStudentskiZajam { get; set; }

        public virtual ICollection<ObavijestiSektori> ObavijestiSektoris { get; set; }
    }
}
