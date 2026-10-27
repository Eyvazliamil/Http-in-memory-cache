using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SharedParts.Configurations;
using SharedParts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedParts.Contexts
{
    public class Context : DbContext
    {
        private static readonly IConfiguration Config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Config.GetConnectionString("Default"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration<Product>(new ProductConfiguration());

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Product> Products { get; set; }
    }
}
