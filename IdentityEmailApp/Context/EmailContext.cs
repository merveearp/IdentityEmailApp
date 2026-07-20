using IdentityEmailApp.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityEmailApp.Context
{
    public class EmailContext :IdentityDbContext<AppUser>
    {
        public EmailContext(DbContextOptions options):base(options)
        {
            
        }
      

        public DbSet<Category> Categories { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Notification>()
               .HasOne(x => x.AppUser)
               .WithMany(x => x.Notifications)
               .HasForeignKey(x => x.AppUserId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
