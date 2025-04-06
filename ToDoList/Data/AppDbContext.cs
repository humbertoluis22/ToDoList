using Microsoft.EntityFrameworkCore;
using ToDoList.Model;

namespace ToDoList.Data
{
    public class AppDbContext:DbContext
    {
        public DbSet<TarefasModel> Tarefas { get; set; }
        public DbSet<UserModel> Usuarios { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options) 
        {
        }

    }
}
