using Demo.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Demo.Api.Data
{
    public class DemoDbContext : DbContext
    {
        public DemoDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }

        public DbSet<TaskToDo> TaskToDos { get; set; }
        public DbSet<UserAccount> UserAccounts { get; set; }
    }
}
