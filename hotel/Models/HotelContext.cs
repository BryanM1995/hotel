using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace hotel.Models
{
    public partial class HotelContext : DbContext
    {
        public HotelContext()
        {
        }

        public HotelContext(DbContextOptions<HotelContext> options)
            : base(options)
        {
        }

        //public virtual DbSet<Habitacione> Habitaciones { get; set; } = null!;
        //public virtual DbSet<Huespede> Huespedes { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseSqlServer("server=LAP-SOP-02\\SQLEXPRESS;database=Hotel; User Id=sa; Password= James1804**;");
                
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Habitacione>(entity =>
            //{
            //    entity.ToTable("habitaciones");

            //    entity.Property(e => e.Id)
            //        .ValueGeneratedNever()
            //        .HasColumnName("id");

            //    entity.Property(e => e.Estado).HasColumnName("estado");

            //    entity.Property(e => e.Numero).HasColumnName("numero");
            //});

            //modelBuilder.Entity<Huespede>(entity =>
            //{
            //    entity.ToTable("huespedes");

            //    entity.Property(e => e.Id)
            //        .ValueGeneratedNever()
            //        .HasColumnName("id");

            //    entity.Property(e => e.HabitacionId).HasColumnName("habitacion_id");

            //    entity.Property(e => e.Identificacion)
            //        .HasMaxLength(50)
            //        .IsUnicode(false)
            //        .HasColumnName("identificacion");

            //    entity.Property(e => e.Ingreso)
            //        .HasColumnType("datetime")
            //        .HasColumnName("ingreso");

            //    entity.Property(e => e.Nombre)
            //        .HasMaxLength(100)
            //        .IsUnicode(false)
            //        .HasColumnName("nombre");

            //    entity.Property(e => e.Salida)
            //        .HasColumnType("datetime")
            //        .HasColumnName("salida");

            //    entity.HasOne(d => d.Habitacion)
            //        .WithMany(p => p.Huespedes)
            //        .HasForeignKey(d => d.HabitacionId)
            //        .HasConstraintName("FK__huespedes__habit__398D8EEE");
            //});

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
