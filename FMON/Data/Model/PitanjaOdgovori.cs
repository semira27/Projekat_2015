namespace Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PitanjaOdgovori")]
    public partial class PitanjaOdgovori
    {
        public int PitanjaOdgovoriID { get; set; }

        public string Pitanje { get; set; }

        public string Odgovor { get; set; }

        public bool IsActive { get; set; }
    }
}
