using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Followups.Models.DB
{
    public partial class FollowUpDbContext : DbContext
    {
        public FollowUpDbContext()
        {
        }

        public FollowUpDbContext(DbContextOptions<FollowUpDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Countries> Countries { get; set; }
        public virtual DbSet<CustomerFollowUp> CustomerFollowUp { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<FollowupView> FollowupView { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.\\sqlexpress;Database=FollowUpDb;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Countries>(entity =>
            {
                entity.HasIndex(e => e.Iso)
                    .HasName("uc_Countries_Iso")
                    .IsUnique();

                entity.Property(e => e.Iso)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Iso3)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CustomerFollowUp>(entity =>
            {
                entity.HasKey(e => e.FollowId);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.CustomerInterest)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.DateOfContact).HasColumnType("datetime");

                entity.Property(e => e.Idstatus)
                    .HasColumnName("IDStatus")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Phone).HasMaxLength(15);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.Property(e => e.Department)
                    .HasColumnName("department")
                    .HasMaxLength(50);

                entity.Property(e => e.Designation)
                    .HasColumnName("designation")
                    .HasMaxLength(50);

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(50);

                entity.Property(e => e.Phone)
                    .HasColumnName("phone")
                    .HasMaxLength(10)
                    .IsFixedLength();
            });

            modelBuilder.Entity<FollowupView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("followupView");

                entity.Property(e => e.Country)
                    .HasColumnName("country")
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerInterest)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.DateOfContact).HasColumnType("datetime");

                entity.Property(e => e.Employee)
                    .HasColumnName("employee")
                    .HasMaxLength(50);

                entity.Property(e => e.Idstatus)
                    .HasColumnName("IDStatus")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Phone).HasMaxLength(15);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("modified_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Password)
                    .HasColumnName("password")
                    .HasMaxLength(50);

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Username)
                    .HasColumnName("username")
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
