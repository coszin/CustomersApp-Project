using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace CustomersApp;

internal class Program
{
    static void Main(string[] args)
    { 
    
    }

    /*--------------- 3 --------------------*/
    public class Customer
    {
        public int Id { get; set; }
        public string CompanyName { get; set; } = null!;
        public string City { get; set; } = null!;

        // Navigation property (One-To-Many)
        public List<Order> Orders { get; set; } = null!;
    }
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public decimal PriceTotal { get; set; }

        // Navigation property (Many-To-One)
        public Customer Customer { get; set; } = null!;
    }

    /*---------------------------------- 4 ------------------------------------------------*/

    public class ApplicationContext : DbContext
    {
        // Exponerar våra entiteter som DbSet
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
            .AddJsonFile("AppSettings.json")
            .Build();

            // Läs vår connection-string från konfigurations-filen
            var connStr = config.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connStr);
        }
    }

    /*---------------------------------------- 5 -------------------------------------------*/

        protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        // Specificerar vilken collation databasen ska använda
        modelBuilder.UseCollation("Finnish_Swedish_CI_AS");


        // Specificerar vilken datatyp databasen ska använda för en specifik kolumn
        modelBuilder.Entity<Order>()
        .Property(o => o.PriceTotal)
        .HasColumnType(SqlDbType.Money.ToString());


        // Specificerar data som en specifik tabell ska för-populeras med
        modelBuilder.Entity<Customer>().HasData(
        new Customer { Id = 1, CompanyName = "Company 1", City = "Stockholm" },
        new Customer { Id = 2, CompanyName = "Company 2", City = "Stockholm" },
        new Customer { Id = 3, CompanyName = "Company 3", City = "Göteborg" });
    }
}
