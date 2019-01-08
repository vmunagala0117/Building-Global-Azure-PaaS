using System;
using AZ_Paas_Demo.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AZ_Paas_Demo.Data
{
    public partial class azpaasdemodbContext : DbContext
    {
        public azpaasdemodbContext()
        {
        }

        public azpaasdemodbContext(DbContextOptions<azpaasdemodbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Juices> Juices { get; set; }
        public virtual DbSet<OrderLines> OrderLines { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<Stores> Stores { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=tcp:azpaasdemosqlserversouthindia.database.windows.net,1433;Initial Catalog=azpaasdemo-db-india;Persist Security Info=False;User ID=vamsi.munagala;Password=Mun@g@l@1984;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Juices>(entity =>
            {
                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<OrderLines>(entity =>
            {
                entity.HasOne(d => d.Juice)
                    .WithMany(p => p.OrderLines)
                    .HasForeignKey(d => d.JuiceId)
                    .HasConstraintName("JuiceForeignKey");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderLines)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("OrdersForeignKey");
            });

            modelBuilder.Entity<Orders>(entity =>
            {
                entity.Property(e => e.Status).HasMaxLength(50);

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.StoreId)
                    .HasConstraintName("StoreForeignKey");
            });

            modelBuilder.Entity<Stores>(entity =>
            {
                entity.Property(e => e.Country)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
        }
    }
}
