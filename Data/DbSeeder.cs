using System.Linq;
using store_api.Models;

namespace store_api.Data
{
    public static class DbSeeder
    {
        public static void SeedData(ApplicationDbItem context)
        {
            if (!context.Items.Any())
            {
                context.Items.AddRange(
                    new Item { Name = "Winter Jacket", Description = "Warm and waterproof jacket for cold weather", Price = 200, Quantity = 20 },
                    new Item { Name = "Family Tent", Description = "Spacious tent suitable for 4-6 people", Price = 1024, Quantity = 10 },
                    new Item { Name = "Hiking Boots", Description = "Durable boots for rugged terrain", Price = 120, Quantity = 40 },
                    new Item { Name = "Travel Backpack", Description = "Multi-compartment travel backpack", Price = 80, Quantity = 60 }, 
                    new Item { Name = "Thermal Socks", Description = "Insulated socks for winter hikes", Price = 15, Quantity = 150 }
                );
                context.SaveChanges();
            }
        }
    }
}