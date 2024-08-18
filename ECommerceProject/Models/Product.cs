using System.Collections.Generic;

namespace ECommerceProject.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
    }
}