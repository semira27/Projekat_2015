namespace Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ObavijestiSlike")]
    public partial class ObavijestiSlike
    {
        [Key]
        public int SlikaID { get; set; }
        public string Putanja { get; set; }
        public int ObavijestID { get; set; }

        public bool IsActive { get; set; }

        public virtual Obavijesti Obavijesti { get; set; }
    }
}
