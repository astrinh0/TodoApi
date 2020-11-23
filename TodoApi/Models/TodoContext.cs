using Microsoft.EntityFrameworkCore;

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
            modelBuilder.Entity<Task>()
                .HasKey(at => new { at.UserId, at.TodoId });
            modelBuilder.Entity<Task>()
                .HasOne(us => us.User)
                .WithMany(tk => tk.Tasks)
                .HasForeignKey(us => us.UserId);
            modelBuilder.Entity<Task>()
                .HasOne(td => td.Todo)
                .WithMany(tk => tk.Tasks)
                .HasForeignKey(td => td.TodoId);

        }

        public DbSet<User> User{ get; set; }
        public DbSet<Todo> Todo { get; set; }
        public DbSet<Task> Task { get; set; }
    }
}
