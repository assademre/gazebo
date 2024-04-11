using EventOrganizationApp.Models;
using Gazebo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EventOrganizationApp.Data
{
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventsTask> Tasks { get; set; }
        public DbSet<UserAccess> UserAccess { get; set; }
        public DbSet<EventMember> EventMembers { get; set; }
        public DbSet<Notification>Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>()
                .HasKey(c => c.UserId);

            builder.Entity<UserAccess>()
                .HasKey(c => c.UserId);

            builder.Entity<Event>()
                .HasKey(e => e.EventId)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            builder.Entity<Event>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(e => e.CreaterId);

            builder.Entity<EventsTask>()
                .HasKey(et => et.TaskId);

            builder.Entity<EventsTask>()
                .HasOne<Event>()
                .WithMany()
                .HasForeignKey(et => et.EventId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<EventsTask>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(et => et.OwnerId);

            builder.Entity<EventMember>()
                .HasKey(c => c.UserId);

            builder.Entity<Notification>()
                .HasKey(c => c.NotificationId);

            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                },
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "USER",
                },
            };

            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
