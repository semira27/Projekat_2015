namespace Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PostaviPitanje")]
    public partial class PostaviPitanje
    {
        [Key]
        public int PitanjeID { get; set; }

        public string Pitanje { get; set; }

        public DateTime Datum { get; set; }

        public bool PitanjePregledano { get; set; }

        public bool IsActive { get; set; }
    }
}
