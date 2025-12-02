using System;
using System.Collections.Generic;
using System.Text;
using static CustomersApp.Program;

namespace CustomersApp;


    public class Customer
    {
        public int Id { get; set; }
        public string CompanyName { get; set; } = null!;
        public string City { get; set; } = null!;

        // Navigation property (One-To-Many)
        public List<Order> Orders { get; set; } = null!;
    }

