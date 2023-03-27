using System;
using System.Collections.Generic;
using MeetGreet.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MeetGreet.Data;

public partial class MeetgreetContext : IdentityUserContext<User, string, UserClaim, UserLogin, UserToken>
{
    public MeetgreetContext()
    {
    }

    public MeetgreetContext(DbContextOptions<MeetgreetContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserClaim> UserClaims { get; set; }

    public virtual DbSet<UserLogin> UserLogins { get; set; }

    public virtual DbSet<UserToken> UserTokens { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=meetgreet-dev.ccg48bpsuvv6.us-east-2.rds.amazonaws.com,3306;user=admin;database=MEETGREET;password=MeetGreetTGB!", ServerVersion.Parse("8.0.28-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("User");

            entity.Property(e => e.Id).HasMaxLength(450);
            entity.Property(e => e.ConcurrencyStamp).HasMaxLength(800);
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.EmailConfirmed).HasColumnType("bit(1)");
            entity.Property(e => e.LockoutEnabled).HasColumnType("bit(1)");
            entity.Property(e => e.LockoutEnd).HasColumnType("timestamp");
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.PasswordHash).HasMaxLength(800);
            entity.Property(e => e.PhoneNumber).HasMaxLength(800);
            entity.Property(e => e.PhoneNumberConfirmed).HasColumnType("bit(1)");
            entity.Property(e => e.RememberMe)
                .HasDefaultValueSql("b'0'")
                .HasColumnType("bit(1)");
            entity.Property(e => e.SecurityStamp).HasMaxLength(800);
            entity.Property(e => e.TwoFactorEnabled).HasColumnType("bit(1)");
            entity.Property(e => e.UserName).HasMaxLength(256);
        });

        modelBuilder.Entity<UserClaim>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("UserClaims");

            entity.HasIndex(e => e.UserId, "UserClaims_Users_UserId");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ClaimType).HasMaxLength(450);
            entity.Property(e => e.ClaimValue).HasMaxLength(450);
            entity.Property(e => e.UserId).HasMaxLength(450);

            entity.HasOne(d => d.User).WithMany(p => p.UserClaims)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("UserClaims_Users_UserId");
        });

        modelBuilder.Entity<UserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.HasIndex(e => e.UserId, "AspNetUserLogins_AspNetUsers_UserId");

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.ProviderKey).HasMaxLength(128);
            entity.Property(e => e.ProviderDisplayName).HasMaxLength(800);
            entity.Property(e => e.UserId).HasMaxLength(450);

            entity.HasOne(d => d.User).WithMany(p => p.UserLogins)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("AspNetUserLogins_AspNetUsers_UserId");
        });

        modelBuilder.Entity<UserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

            entity.Property(e => e.UserId).HasMaxLength(450);
            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.Name).HasMaxLength(128);
            entity.Property(e => e.Value).HasMaxLength(255);

            entity.HasOne(d => d.User).WithMany(p => p.UserTokens)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("UserTokens_User_UserId");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
