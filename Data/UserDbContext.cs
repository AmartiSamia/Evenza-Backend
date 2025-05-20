using Microsoft.EntityFrameworkCore;
using Project.API.Entities;
namespace Project.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Registration> Registrations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuration pour User
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Configuration pour Event
            modelBuilder.Entity<Event>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Location).HasMaxLength(200);
                entity.Property(e => e.StartTime).IsRequired();
                entity.Property(e => e.EndTime).IsRequired();
            });

            // Configuration pour Registration
            modelBuilder.Entity<Registration>(entity =>
            {
                entity.HasKey(r => r.Id);

                // Property configurations
                entity.Property(r => r.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(r => r.LastName).IsRequired().HasMaxLength(50);
                entity.Property(r => r.Email).IsRequired().HasMaxLength(100);
                entity.Property(r => r.Phone).HasMaxLength(20);
                entity.Property(r => r.RegistrationDate).IsRequired().HasDefaultValueSql("GETUTCDATE()");


                // Relationship with Event
                entity.HasOne(r => r.Event)
                    .WithMany() // Consider adding a navigation property in Event if needed: .WithMany(e => e.Registrations)
                    .HasForeignKey(r => r.EventId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Registration_Event");

                // Relationship with User
                entity.HasOne(r => r.User)
                    .WithMany() // Consider adding a navigation property in User if needed: .WithMany(u => u.Registrations)
                    .HasForeignKey(r => r.UserId)
                    .OnDelete(DeleteBehavior.Restrict) // Prevent accidental deletion of registrations when a user is deleted
                    .HasConstraintName("FK_Registration_User");

                // Unique composite index to prevent multiple registrations for the same user/event
                entity.HasIndex(r => new { r.UserId, r.EventId })
                    .IsUnique()
                    .HasDatabaseName("IX_Registration_UserId_EventId");

                // Additional index on Email + EventId (keep if you need email-based uniqueness)
                entity.HasIndex(r => new { r.Email, r.EventId })
                    .IsUnique()
                    .HasDatabaseName("IX_Registration_Email_EventId");

                // Performance indexes
                entity.HasIndex(r => r.EventId)
                    .HasDatabaseName("IX_Registration_EventId");

                entity.HasIndex(r => r.UserId)
                    .HasDatabaseName("IX_Registration_UserId");
            });
        }
    }
}