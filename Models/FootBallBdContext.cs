using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebLabaICTry2.Models;

public partial class FootBallBdContext : DbContext
{
    public FootBallBdContext()
    {
    }

    public FootBallBdContext(DbContextOptions<FootBallBdContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Championship> Championships { get; set; }

    public virtual DbSet<Club> Clubs { get; set; }

    public virtual DbSet<Match> Matches { get; set; }

    public virtual DbSet<Player> Players { get; set; }

    public virtual DbSet<ScoredGoal> ScoredGoals { get; set; }

    public virtual DbSet<Stadium> Stadia { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-CFOLTDF\\SQLEXPRESS; Database=FootBallBD; Trusted_Connection=True; TrustServerCertificate = true ");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Championship>(entity =>
        {
            entity.ToTable("Championship");

            entity.Property(e => e.ChampionshipCountry).HasMaxLength(50);
            entity.Property(e => e.ChampionshipName).HasMaxLength(50);
        });

        modelBuilder.Entity<Club>(entity =>
        {
            entity.ToTable("Club");

            entity.Property(e => e.ClubCoachName).HasMaxLength(50);
            entity.Property(e => e.ClubEstablishmentDate).HasColumnType("date");
            entity.Property(e => e.ClubName).HasMaxLength(50);
            entity.Property(e => e.ClubOrigin).HasMaxLength(50);
        });

        modelBuilder.Entity<Match>(entity =>
        {
            entity.ToTable("Match");

            entity.Property(e => e.MatchDate).HasColumnType("date");

            entity.HasOne(d => d.GuestClub).WithMany(p => p.MatchGuestClubs)
                .HasForeignKey(d => d.GuestClubId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Match_GuestClub");

            entity.HasOne(d => d.HostClub).WithMany(p => p.MatchHostClubs)
                .HasForeignKey(d => d.HostClubId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Match_HostClub");

            entity.HasOne(d => d.Staidum).WithMany(p => p.Matches)
                .HasForeignKey(d => d.StaidumId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Match_Stadium");
        });

        modelBuilder.Entity<Player>(entity =>
        {
            entity.ToTable("Player");

            entity.Property(e => e.PlayerId).ValueGeneratedOnAdd();
            entity.Property(e => e.PlayerBirthDate).HasColumnType("date");
            entity.Property(e => e.PlayerName).HasMaxLength(40);
            entity.Property(e => e.PlayerPosition).HasMaxLength(20);
            entity.Property(e => e.PlayerSalary).HasColumnType("money");

            entity.HasOne(d => d.PlayerNavigation).WithOne(p => p.Player)
                .HasForeignKey<Player>(d => d.PlayerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Player_Club");
        });

        modelBuilder.Entity<ScoredGoal>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("ScoredGoal");

            entity.HasOne(d => d.Match).WithMany()
                .HasForeignKey(d => d.MatchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ScoredGoal_Match");

            entity.HasOne(d => d.Player).WithMany()
                .HasForeignKey(d => d.PlayerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ScoredGoal_Player");
        });

        modelBuilder.Entity<Stadium>(entity =>
        {
            entity.ToTable("Stadium");

            entity.Property(e => e.StadiumEstablismentDate).HasColumnType("date");
            entity.Property(e => e.StadiumLocation).HasMaxLength(50);

            entity.HasOne(d => d.Club).WithMany(p => p.Stadia)
                .HasForeignKey(d => d.ClubId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Stadium_Club");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
