using Microsoft.EntityFrameworkCore;

namespace todo_list_backend.Data
{
    public class TodoListDbContext : DbContext
    {
        public TodoListDbContext (DbContextOptions<TodoListDbContext> options)
            : base(options)
        {
        }

        public DbSet<todo_list_backend.Models.Todo> Todo { get; set; }
    }
}
