using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MeetGreet;

public partial class MeetgreetContext : DbContext
{
    public MeetgreetContext()
    {
    }

    public MeetgreetContext(DbContextOptions<MeetgreetContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Efmigrationshistory> Efmigrationshistories { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Userclaim> Userclaims { get; set; }

    // TODO: CHANGE PASSWORD HERE TOO.
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySql("server=localhost;user=root;database=MEETGREET;password={CHANGE THIS PASSWORD}", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.31-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Efmigrationshistory>(entity =>
        {
            entity.HasKey(e => e.MigrationId).HasName("PRIMARY");

            entity.ToTable("__efmigrationshistory");

            entity.Property(e => e.MigrationId).HasMaxLength(150);
            entity.Property(e => e.ProductVersion).HasMaxLength(32);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("user");

            entity.Property(e => e.Id)
                .HasMaxLength(450)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.ConcurrencyStamp)
                .HasMaxLength(256)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Email)
                .HasMaxLength(256)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.EmailConfirmed).HasColumnType("bit(1)");
            entity.Property(e => e.LockoutEnabled).HasColumnType("bit(1)");
            entity.Property(e => e.LockoutEnd).HasColumnType("timestamp");
            entity.Property(e => e.NormalizedEmail)
                .HasMaxLength(256)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.NormalizedUserName)
                .HasMaxLength(256)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(256)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(256)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.PhoneNumberConfirmed).HasColumnType("bit(1)");
            entity.Property(e => e.SecurityStamp)
                .HasMaxLength(256)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.TwoFactorEnabled).HasColumnType("bit(1)");
            entity.Property(e => e.UserName)
                .HasMaxLength(256)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
        });

        modelBuilder.Entity<Userclaim>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("userclaims");

            entity.HasIndex(e => e.UserId, "FK_UserClaims_Users_UserId");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ClaimType)
                .HasMaxLength(450)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.ClaimValue)
                .HasMaxLength(450)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.UserId)
                .HasMaxLength(450)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");

            entity.HasOne(d => d.User).WithMany(p => p.Userclaims)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UserClaims_Users_UserId");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
