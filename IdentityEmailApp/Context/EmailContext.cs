using IdentityEmailApp.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

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
        public DbSet<TaskList> TaskLists { get; set; }
        public DbSet<UserTask> UserTasks { get; set; }
        public DbSet<SubTask> SubTasks { get; set; }
      
        public DbSet<TranslationHistory> TranslationHistories { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Notification>()
               .HasOne(x => x.AppUser)
               .WithMany(x => x.Notifications)
               .HasForeignKey(x => x.AppUserId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<SubTask>()
               .HasOne(x => x.UserTask)
               .WithMany(x => x.SubTasks)
               .HasForeignKey(x => x.UserTaskId)
               .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<UserTask>()
                .HasOne(x => x.TaskList)
                .WithMany(x => x.UserTasks)
                .HasForeignKey(x => x.TaskListId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserTask>()
                .HasOne(x => x.TaskList)
                .WithMany(x => x.UserTasks)
                .HasForeignKey(x => x.TaskListId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserTask>()
                .HasOne(x => x.AppUser)
                .WithMany()
                .HasForeignKey(x => x.AppUserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<TaskList>()
                .HasOne(x => x.AppUser)
                .WithMany()
                .HasForeignKey(x => x.AppUserId)
                .OnDelete(DeleteBehavior.NoAction);

          
        }
    }
}
