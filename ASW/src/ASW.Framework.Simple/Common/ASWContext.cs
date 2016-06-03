namespace ASW.Framework.Simple
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using ASW.Framework.Core;

    public partial class ASWContext : DbContext
    {
        public ASWContext()
            : base("name=ASWConnection")
        {
        }

        public virtual DbSet<ApplicationCase> ApplicationCases { get; set; }
        public virtual DbSet<Card> Cards { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationCase>()
                .Property(e => e.Username)
                .IsUnicode(false);

            modelBuilder.Entity<ApplicationCase>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<ApplicationCase>()
                .Property(e => e.AppId)
                .IsUnicode(false);

            modelBuilder.Entity<Card>()
                .Property(e => e.Id)
                .IsUnicode(false);
            modelBuilder.Entity<Card>()
                .Property(e => e.CardType)
                .IsUnicode(false);
            modelBuilder.Entity<Card>()
                .Property(e => e.Holder)
                .IsUnicode(false);
            modelBuilder.Entity<Card>()
                .Property(e => e.CardNumber)
                .IsUnicode(false);
            modelBuilder.Entity<Card>()
                .Property(e => e.ExpiryMonth)
                .IsUnicode(false);
            modelBuilder.Entity<Card>()
                .Property(e => e.ExpiryYear)
                .IsUnicode(false);
            modelBuilder.Entity<Card>()
                .Property(e => e.SecureCode)
                .IsUnicode(false);
        }
    }
}
