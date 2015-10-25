namespace Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Gradovi")]
    public partial class Gradovi
    {
        [Key]
        public int GradID { get; set; }

        public string Naziv { get; set; }
        public bool IsGeneralno { get; set; }
   
    }
}
