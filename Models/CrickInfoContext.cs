//using crickinfo_mvc_ef_core.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Text.RegularExpressions;

namespace crickinfo_mvc_ef_core.Models
{
    public class CrickInfoContext : DbContext
    {
        public CrickInfoContext(DbContextOptions<CrickInfoContext> options)
            : base(options)
        {
        }

        // DbSet properties for your models
        public DbSet<User> Users { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<Matches> Matches { get; set; }
        public DbSet<PointsTable> PointsTables { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamTournament> TeamTournaments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TeamTournament>().HasKey(pt => new { pt.TeamId, pt.TournamentId });
            modelBuilder.Entity<TeamTournament>().
                         HasOne(pt => pt.Team).WithMany(pt => pt.TeamTournaments).HasForeignKey(pt => pt.TeamId);
            modelBuilder.Entity<TeamTournament>().
            HasOne(pt => pt.Tournament).WithMany(pt => pt.TeamTournaments).HasForeignKey(pt => pt.TournamentId);

            modelBuilder.Entity<Matches>()
                .HasOne(m => m.TeamA) // assuming you have navigation properties set
                .WithMany() // no inverse navigation
                .HasForeignKey(m => m.TeamAId)
                .OnDelete(DeleteBehavior.Restrict); // change to Restrict or NoAction

            modelBuilder.Entity<Matches>()
                .HasOne(m => m.TeamB)
                .WithMany()
                .HasForeignKey(m => m.TeamBId)
                .OnDelete(DeleteBehavior.Restrict); // change to Restrict or NoAction

            modelBuilder.Entity<Matches>()
                .HasOne(m => m.Tournament)
                .WithMany()
                .HasForeignKey(m => m.TournamentId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
