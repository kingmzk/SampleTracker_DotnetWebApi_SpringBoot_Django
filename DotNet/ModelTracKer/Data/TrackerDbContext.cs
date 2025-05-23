using Microsoft.EntityFrameworkCore;
using ModelTracKer.Models;

namespace ModelTracKer.Data
{
    public class TrackerDbContext : DbContext
    {
        public TrackerDbContext(DbContextOptions<TrackerDbContext> options) : base(options) { }

        public DbSet<Accelerator> Accelerators { get; set; }

        public DbSet<Tracker> Trackers { get; set; }

        public DbSet<opp_Accelerator> OppAccelerators { get; set; }

        public DbSet<MicroService> MicroServices { get; set; }

        public DbSet<opp_microservice> OppMicroservices { get; set; }

        public DbSet<Competition> Competitions { get; set; }

        public DbSet<opp_competition> OppCompetitions { get; set; }

        public DbSet<GenAiTool> GenAiTools { get; set; }

        public DbSet<ReasonForNoGenAiAdoptation> ReasonForNoGenAiAdoptations { get; set; }

    }
}









/*
 
public class Tracker
{
    public int Id { get; set; }

    public ICollection<Accelerator> Accelerators { get; set; }
}

public class Accelerator
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public ICollection<Tracker> Trackers { get; set; }
}


No need to define the join table. EF Core will create it automatically:

public DbSet<Accelerator> Accelerators { get; set; }
public DbSet<Tracker> Trackers { get; set; }

EF Core will auto-create a join table named something like AcceleratorTracker with two foreign keys:
TrackerId
AcceleratorId


*/



/*  (FLUIENT API)
 
 using Microsoft.EntityFrameworkCore;

namespace ModelTracKer.Models
{
    public class TrackerDbContext : DbContext
    {
        public DbSet<Accelerator> Accelerators { get; set; }
        public DbSet<Tracker> Trackers { get; set; }
        public DbSet<opp_Accelerator> OppAccelerators { get; set; }

        public TrackerDbContext(DbContextOptions<TrackerDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<opp_Accelerator>()
                .HasKey(oa => oa.Id); // or composite key if you want

            modelBuilder.Entity<opp_Accelerator>()
                .HasOne(oa => oa.Trackers)
                .WithMany(t => t.opp_Accelerator)
                .HasForeignKey(oa => oa.TrackerId)
                .OnDelete(DeleteBehavior.Restrict); // or your choice: Cascade, SetNull, etc.

            modelBuilder.Entity<opp_Accelerator>()
                .HasOne(oa => oa.Accelerators)
                .WithMany(a => a.opp_Accelerator)
                .HasForeignKey(oa => oa.AcceleratorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

 */