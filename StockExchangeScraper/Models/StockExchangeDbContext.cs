using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace StockExchangeScraper.Models
{
    public partial class StockExchangeDbContext : DbContext
    {
        public StockExchangeDbContext()
        {
        }

        public StockExchangeDbContext(DbContextOptions<StockExchangeDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<EquityType> EquityTypes { get; set; }
        public virtual DbSet<ExchangeHoliday> ExchangeHolidays { get; set; }
        public virtual DbSet<Exchange> Exchanges { get; set; }
        public virtual DbSet<Industry> Industries { get; set; }
        public virtual DbSet<Sector> Sectors { get; set; }
        public virtual DbSet<StockQuote> StockQuotes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>(entity =>
            {
                entity.HasKey(e => e.Isin)
                    .HasName("PK__Companie__447D6FC4DD15E799");

                entity.Property(e => e.Isin)
                    .HasColumnName("ISIN")
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.EquityTypeId).HasColumnName("EquityTypeID");

                entity.Property(e => e.ExchangeId).HasColumnName("ExchangeID");

                entity.Property(e => e.IndustryId).HasColumnName("IndustryID");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.SectorId).HasColumnName("SectorID");

                entity.Property(e => e.Url)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.EquityType)
                    .WithMany(p => p.Companies)
                    .HasForeignKey(d => d.EquityTypeId)
                    .HasConstraintName("C3");

                entity.HasOne(d => d.Exchange)
                    .WithMany(p => p.Companies)
                    .HasForeignKey(d => d.ExchangeId)
                    .HasConstraintName("C4");

                entity.HasOne(d => d.Industry)
                    .WithMany(p => p.Companies)
                    .HasForeignKey(d => d.IndustryId)
                    .HasConstraintName("C2");

                entity.HasOne(d => d.Sector)
                    .WithMany(p => p.Companies)
                    .HasForeignKey(d => d.SectorId)
                    .HasConstraintName("C1");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.Property(e => e.CountryId).HasColumnName("CountryID");

                entity.Property(e => e.Name)
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EquityType>(entity =>
            {
                entity.Property(e => e.EquityTypeId).HasColumnName("EquityTypeID");

                entity.Property(e => e.Name)
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ExchangeHoliday>(entity =>
            {
                entity.Property(e => e.ExchangeHolidayId).HasColumnName("ExchangeHolidayID");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.ExchangeId).HasColumnName("ExchangeID");

                entity.HasOne(d => d.Exchange)
                    .WithMany(p => p.ExchangeHolidays)
                    .HasForeignKey(d => d.ExchangeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("C5");
            });

            modelBuilder.Entity<Exchange>(entity =>
            {
                entity.HasKey(e => e.ExchangeId)
                    .HasName("PK__Exchange__72E600AB3A7577FD");

                entity.Property(e => e.ExchangeId).HasColumnName("ExchangeID");

                entity.Property(e => e.CountryId).HasColumnName("CountryID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Exchanges)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("C6");
            });

            modelBuilder.Entity<Industry>(entity =>
            {
                entity.Property(e => e.IndustryId).HasColumnName("IndustryID");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Sector>(entity =>
            {
                entity.Property(e => e.SectorId).HasColumnName("SectorID");

                entity.Property(e => e.Name)
                    .HasMaxLength(25)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<StockQuote>(entity =>
            {
                entity.Property(e => e.StockQuoteId).HasColumnName("StockQuoteID");

                entity.Property(e => e.CompanyIsin)
                    .IsRequired()
                    .HasColumnName("CompanyISIN")
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Date).HasColumnType("date");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.StockQuotes)
                    .HasForeignKey(d => d.CompanyIsin)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("C7");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
