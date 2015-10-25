namespace Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ObavijestiSektori")]
    public partial class ObavijestiSektori
    {
        [Key]
        public int ObavijestSektorID { get; set; }

        public int SektorID { get; set; }

        public int ObavijestID { get; set; }

        public bool IsActive { get; set; }

        public virtual Obavijesti Obavijesti { get; set; }

        public virtual Sektori Sektori { get; set; }
    }
}
