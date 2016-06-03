namespace PowerBallNZ
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Entites : DbContext
    {
        public Entites()
            : base("name=Entities")
        {
        }

        public virtual DbSet<Draw> Draws { get; set; }
        public virtual DbSet<Combine> Combines { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Draw>()
                .Property(e => e.LottoStr)
                .IsUnicode(false);

            modelBuilder.Entity<Draw>()
                .Property(e => e.StrikeStr)
                .IsUnicode(false);

            modelBuilder.Entity<Combine>()
                .Property(e => e.CombineStr)
                .IsUnicode(false);
        }
    }
}
