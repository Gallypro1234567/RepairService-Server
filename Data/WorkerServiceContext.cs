using Microsoft.EntityFrameworkCore;
using WorkAppReactAPI.Models;

namespace WorkAppReactAPI.Data
{
    public class WorkerServiceContext : DbContext
    {
        public WorkerServiceContext(DbContextOptions<WorkerServiceContext> opt) : base(opt)
        {

        }
        public DbSet<User> Users { set; get; }

        public DbSet<Customer> Customers { set; get; }
    }
}