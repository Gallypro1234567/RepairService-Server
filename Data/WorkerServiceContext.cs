using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WorkAppReactAPI.Models;

namespace WorkAppReactAPI.Data
{
    public class WorkerServiceContext : IdentityDbContext
    {
        public WorkerServiceContext(DbContextOptions<WorkerServiceContext> opt) : base(opt)
        {

        }
        public DbSet<User> Partner { set; get; }
        public DbSet<Customer> Customers { set; get; }
        public DbSet<Worker> Workers { set; get; }
        public DbSet<Booking> Bookings { set; get; }
        public DbSet<Feelback> Feelbacks { set; get; }
        public DbSet<HistoryAdress> HistoryAdress { set; get; }
        public DbSet<Preferential> Preferentials { set; get; }
        public DbSet<PreferentialOfService> PreferentialOfServices { set; get; }
        public DbSet<Service> Services { set; get; }
         public DbSet<WorkerOfService> WorkerOfServices { set; get; }
        

    }
}