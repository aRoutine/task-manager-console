using Microsoft.EntityFrameworkCore;
using TaskManager.Models;

namespace TaskManager.Data;

public class TaskManagerDbContext : DbContext
{
    public TaskManagerDbContext(DbContextOptions<TaskManagerDbContext> options)
        : base(options)
    {
    }

    public DbSet<TaskItem> Tasks { get; set; }
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
        .HasIndex(user => user.Email)
        .IsUnique();

        modelBuilder.Entity<TaskItem>()
        .HasOne(task => task.User)
        .WithMany(user => user.Tasks)
        .HasForeignKey(task => task.UserId)
        .OnDelete(DeleteBehavior.Cascade);
    }
}