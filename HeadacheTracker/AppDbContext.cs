using HeadacheTracker.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HeadacheTracker {
    public class AppDbContext : IdentityDbContext<AppUser> {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Trigger> Triggers { get; set; }
        public DbSet<Treatment> Treatments { get; set; }
        public DbSet<Medication> Medications { get; set; }
        public DbSet<HeadacheRecord> HeadacheRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);

            builder.Entity<Medication>()
                .HasOne(m => m.User)
                .WithMany(u => u.Medications)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Treatment>()
                .HasOne(t => t.User)
                .WithMany(u => u.Treatments)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Trigger>()
                .HasOne(t => t.User)
                .WithMany(u => u.Triggers)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<HeadacheRecord>()
             .HasOne(hr => hr.Medication)
             .WithMany()
             .HasForeignKey(hr => hr.MedicationId)
             .OnDelete(DeleteBehavior.SetNull);  // Jen tady SetNull

            builder.Entity<HeadacheRecord>()
                .HasOne(hr => hr.Treatment)
                .WithMany()
                .HasForeignKey(hr => hr.TreatmentId)
                .OnDelete(DeleteBehavior.NoAction);  // Tady NoAction

            builder.Entity<HeadacheRecord>()
                .HasOne(hr => hr.Trigger)
                .WithMany()
                .HasForeignKey(hr => hr.TriggerId)
                .OnDelete(DeleteBehavior.NoAction);  // Tady NoAction
        }


    }
}
