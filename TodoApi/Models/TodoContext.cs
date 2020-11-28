using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Models
{
    public class TodoContext : DbContext
    {
        // Add DBcontext to project
        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options)
        {
        }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tasks>()
                .HasKey(at => new { at.UserId, at.TodoId });
            modelBuilder.Entity<Tasks>()
                .HasOne(us => us.User)
                .WithMany(tk => tk.Tasks)
                .HasForeignKey(us => us.UserId);
            modelBuilder.Entity<Tasks>()
                .HasOne(td => td.Todo)
                .WithMany(tk => tk.Tasks)
                .HasForeignKey(td => td.TodoId);

        }

        public DbSet<User> User{ get; set; }
        public DbSet<Todo> Todo { get; set; }
        public DbSet<Tasks> Tasks { get; set; }
    }
}
