namespace Data.Model
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Context : DbContext
    {
        public Context()
            : base("name=Context")
        {
        }

        public virtual DbSet<Anketa> Anketas { get; set; }
        public virtual DbSet<AnketaOdgovor> AnketaOdgovors { get; set; }
        public virtual DbSet<Dokumenti> Dokumentis { get; set; }
        public virtual DbSet<DokumentiKategorije> DokumentiKategorijes { get; set; }
        public virtual DbSet<Gradovi> Gradovis { get; set; }
        public virtual DbSet<InstitucijaLinkovi> InstitucijaLinkovis { get; set; }
        public virtual DbSet<KalendarObavijesti> KalendarObavijestis { get; set; }
        public virtual DbSet<Konkursi> Konkursis { get; set; }
        public virtual DbSet<KonkursiDokumenti> KonkursiDokumentis { get; set; }
        public virtual DbSet<Korisnici> Korisnicis { get; set; }
        public virtual DbSet<Najave> Najaves { get; set; }
        public virtual DbSet<Obavijesti> Obavijestis { get; set; }
        public virtual DbSet<ObavijestiDokumenti> ObavijestiDokumentis { get; set; }
        public virtual DbSet<ObavijestiSektori> ObavijestiSektoris { get; set; }
        public virtual DbSet<ObavijestiSlike> ObavijestiSlikes { get; set; }
        public virtual DbSet<PitanjaOdgovori> PitanjaOdgovoris { get; set; }
        public virtual DbSet<PostaviPitanje> PostaviPitanjes { get; set; }
        public virtual DbSet<Postavke> Postavkes { get; set; }
        public virtual DbSet<RegistarNostrifikacija> RegistarNostrifikacijas { get; set; }
        public virtual DbSet<Sektori> Sektoris { get; set; }
        public virtual DbSet<VideoMaterijali> VideoMaterijalis { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DokumentiKategorije>()
                .HasMany(e => e.Dokumentis)
                .WithOptional(e => e.DokumentiKategorije)
                .HasForeignKey(e => e.DokumentKategorijaID);

            modelBuilder.Entity<Obavijesti>()
                .HasMany(e => e.ObavijestiDokumentis)
                .WithOptional(e => e.Obavijesti)
                .HasForeignKey(e => e.ObavjestID);

            modelBuilder.Entity<InstitucijaLinkovi>()
             .HasRequired(x=>x.Grad)
             .WithMany()
             .HasForeignKey(e => e.GradID);
        }
    }
}
