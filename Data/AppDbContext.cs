using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TempHumidityBackend.Models;

namespace TempHumidityBackend.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TempHumidity> TempHumidities { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Name=ConnectionStrings:ApplicationConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TempHumidity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("temp_humidity_pkey");

            entity.ToTable("temp_humidity");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.RelHumidity).HasColumnName("rel_humidity");
            entity.Property(e => e.TempC).HasColumnName("temp_c");
            entity.Property(e => e.ReadAt).HasColumnName("read_at");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
