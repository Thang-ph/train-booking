using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PRN211_Project_Group_4.Models
{
    public partial class PRN211Context : DbContext
    {
        public PRN211Context()
        {
        }

        public PRN211Context(DbContextOptions<PRN211Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<Booking> Bookings { get; set; } = null!;
        public virtual DbSet<Location> Locations { get; set; } = null!;
        public virtual DbSet<RouteTrain> RouteTrains { get; set; } = null!;
        public virtual DbSet<TicketClass> TicketClasses { get; set; } = null!;
        public virtual DbSet<Train> Trains { get; set; } = null!;
        public virtual DbSet<Trip> Trips { get; set; } = null!;
        public virtual DbSet<Type> Types { get; set; } = null!;
        public virtual DbSet<Wagon> Wagons { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=localhost;database=PRN211;Trusted_Connection=SSPI;Encrypt=false;TrustServerCertificate=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");

                entity.Property(e => e.Password).HasMaxLength(100);

                entity.Property(e => e.Username).HasMaxLength(100);
            });

            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasKey(e => e.BookId);

                entity.Property(e => e.SeatStatus).HasMaxLength(250);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("FK_Bookings_Account");

                entity.HasOne(d => d.Trip)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.TripId)
                    .HasConstraintName("FK_Bookings_Trip1");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.ToTable("Location");

                entity.Property(e => e.Location1)
                    .HasMaxLength(100)
                    .HasColumnName("Location");
            });

            modelBuilder.Entity<RouteTrain>(entity =>
            {
                entity.HasKey(e => e.RouteId)
                    .HasName("PK_Route");

                entity.ToTable("RouteTrain");

                entity.HasOne(d => d.End)
                    .WithMany(p => p.RouteTrainEnds)
                    .HasForeignKey(d => d.EndId)
                    .HasConstraintName("FK_Route_Location1");

                entity.HasOne(d => d.Start)
                    .WithMany(p => p.RouteTrainStarts)
                    .HasForeignKey(d => d.StartId)
                    .HasConstraintName("FK_Route_Location");
            });

            modelBuilder.Entity<TicketClass>(entity =>
            {
                entity.HasKey(e => e.ClassId)
                    .HasName("PK_Class");

                entity.ToTable("TicketClass");

                entity.Property(e => e.Class).HasMaxLength(100);
            });

            modelBuilder.Entity<Train>(entity =>
            {
                entity.ToTable("Train");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Trains)
                    .HasForeignKey(d => d.TypeId)
                    .HasConstraintName("FK_Train_Type");
            });

            modelBuilder.Entity<Trip>(entity =>
            {
                entity.ToTable("Trip");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Price).HasColumnType("money");

                entity.HasOne(d => d.Route)
                    .WithMany(p => p.Trips)
                    .HasForeignKey(d => d.RouteId)
                    .HasConstraintName("FK_Trip_RouteTrain");

                entity.HasOne(d => d.Train)
                    .WithMany(p => p.Trips)
                    .HasForeignKey(d => d.TrainId)
                    .HasConstraintName("FK_Trip_Train");
            });

            modelBuilder.Entity<Type>(entity =>
            {
                entity.ToTable("Type");

                entity.Property(e => e.Type1)
                    .HasMaxLength(100)
                    .HasColumnName("Type");
            });

            modelBuilder.Entity<Wagon>(entity =>
            {
                entity.ToTable("Wagon");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.Wagons)
                    .HasForeignKey(d => d.ClassId)
                    .HasConstraintName("FK_Wagon_Class");

                entity.HasOne(d => d.Train)
                    .WithMany(p => p.Wagons)
                    .HasForeignKey(d => d.TrainId)
                    .HasConstraintName("FK_Wagon_Train");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
