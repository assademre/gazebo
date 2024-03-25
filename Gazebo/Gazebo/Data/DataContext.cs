using EventOrganizationApp.Models;
using EventOrganizationApp.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace EventOrganizationApp.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<EventMember> EventMembers { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventsTask> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(c => c.UserId);

            modelBuilder.Entity<Event>()
                .HasKey(e => e.EventId)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);


            modelBuilder.Entity<Event>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(e => e.CreaterId);

            modelBuilder.Entity<EventsTask>()
                .HasKey(et => et.TaskId);

            modelBuilder.Entity<EventsTask>()
                .HasOne<Event>()
                .WithMany()
                .HasForeignKey(et => et.EventId);

            modelBuilder.Entity<EventsTask>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(et => et.OwnerId);

            modelBuilder.Entity<User>()
                .HasKey(u => u.UserId);
        }
    }
}
