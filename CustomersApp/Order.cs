using System;
using System.Collections.Generic;
using System.Text;

namespace CustomersApp;


public class Order
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public decimal PriceTotal { get; set; }
    public DateTime Created { get; set; }

    // Navigation property (Many-To-One)
    public Customer Customer { get; set; } = null!;
}
