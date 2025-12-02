using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CustomersApp;

internal class Program
{
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


    // Sample data for quick testing
    static readonly Customer[] customers = 
    {
        new Customer { Id = 1, CompanyName = "SAAB", City = "Köping" },
        new Customer { Id = 2, CompanyName = "Nordic Solutions", City = "Stockholm" },
        new Customer { Id = 3, CompanyName = "Göta Trading", City = "Göteborg" },
        new Customer { Id = 4, CompanyName = "Lapland Logistics", City = "Luleå" },
        new Customer { Id = 5, CompanyName = "SkandiFoods", City = "Malmö" }
    };

    static readonly Order[] orders =
    {
        new Order { Id = 1, CustomerId = 1, PriceTotal = 1250.50m },
        new Order { Id = 2, CustomerId = 1, PriceTotal = 299.99m },
        new Order { Id = 3, CustomerId = 2, PriceTotal = 4500.00m },
        new Order { Id = 4, CustomerId = 3, PriceTotal = 75.20m },
        new Order { Id = 5, CustomerId = 4, PriceTotal = 980.00m },
        new Order { Id = 6, CustomerId = 5, PriceTotal = 199.95m },
        new Order { Id = 7, CustomerId = 2, PriceTotal = 1300.00m }
    };


    static void Main(string[] args)
    { 

        //_3A();
        _3B(Input());
       
        
    }

    private static string Input()
    {
        //var input = Console.ReadKey(true);
        ConsoleKeyInfo Output = Console.ReadKey(true);
        return Output.KeyChar.ToString();
    }

    private static void _3B(string Input)
    {

        var TorF = int.TryParse(Input, out int Number);

        if (TorF)
        {
            var c = customers
                .Where(c => c.Id == Number)
                .Select(c => c.CompanyName);


            foreach (var item in c)
            {
                Console.WriteLine(item);
            }
        }
        else
        {
            Console.WriteLine("Enter a valid ID");
            
            Console.WriteLine($"You entered: {Input}");
        }
    }

    private static void _3A()
    {
        var q = customers
           .Select(c => new { c.Id, c.CompanyName });

        foreach (var item in q)
        {
            Console.WriteLine(item);
        }
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
}
