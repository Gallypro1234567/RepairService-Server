 
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
        public DbSet<Worker> Workers { set; get; }
        public DbSet<Feedback> Feedbacks { set; get; }
        public DbSet<HistoryAdress> HistoryAdress { set; get; }
        public DbSet<Preferential> Preferentials { set; get; }
        public DbSet<Service> Services { set; get; }
        public DbSet<Post> Posts { set; get; }
        public DbSet<ApplyToPost> ApplyToPosts { set; get; }
        public DbSet<WorkerOfService> WorkerOfServices { set; get; }
        public DbSet<PreferentialOfService> PreferentialOfServices { set; get; }


    }
}