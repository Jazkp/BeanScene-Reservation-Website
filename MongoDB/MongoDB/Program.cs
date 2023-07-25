using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB;

public class Program
{
    private static void Main(string[] args)
    {
        MongoClient DbClient = new MongoClient("mongodb://localhost:27017");

        // Creating database or getting it
        var database = DbClient.GetDatabase("bsdb");
        var collection = database.GetCollection<Product>("Products");

        // Starting Database from scratch
        //database.DropCollection("Products");
        var waffle = new Product
        {
            Name = "Waffle",
            Sittings = new string[] { "Breakfast" },
            Sizes = new string[] { "Regular", "Small" },
            Prices = new decimal[] { 12.00m, 8.00m },
            Vegan = false

        };

        var products = new List<Product>
        {
            new Product
            {
                Name = "Bacon & Eggs",
                Sittings = new string[]{"Breakfast"},
                Sizes = new string[] {"Regular"},
                Prices = new decimal[] {10.00m},
                Vegan = false
            },

            new Product
            {
                Name = "Avocado on Toast",
                Sittings = new string[]{"Breakfast"},
                Sizes = new string[] {"Regular"},
                Prices = new decimal[] {12.00m},
                Vegan = true
            },
            new Product
            {
                Name = "Pancakes",
                Sittings = new string[]{"Breakfast"},
                Sizes = new string[] {"Regular"},
                Prices = new decimal[] {9.00m},
                Vegan = false
            },
            new Product
            {
                Name = "Granola Bowl",
                Sittings = new string[]{"Breakfast"},
                Sizes = new string[] {"Regular"},
                Prices = new decimal[] {7.00m},
                Vegan = false
            },
            new Product
            {
                Name = "Latte",
                Sittings = new string[]{"Breakfast"},
                Sizes = new string[] {"Large", "Regular", "Small"},
                Prices = new decimal[]{5.00m, 4.50m, 4.00m},
                Vegan = false
            },
            new Product
            {
                Name = "Flat White",
                Sittings = new string[]{"Breakfast"},
                Sizes = new string[] {"Large", "Regular", "Small"},
                Prices = new decimal[]{5.00m, 4.50m, 4.00m},
                Vegan = false
            },
            new Product
            {
                Name = "Long Black",
                Sittings = new string[]{"Breakfast"},
                Sizes = new string[] {"Large", "Regular", "Small"},
                Prices = new decimal[]{4.50m, 4.00m, 3.50m},
                Vegan = true
            },
            new Product
            {
                Name = "Espresso",
                Sittings = new string[]{"Breakfast"},
                Sizes = new string[] {"Small"},
                Prices = new decimal[]{3.00m},
                Vegan = false
            },
            new Product
            {
                Name = "Caesar Salad",
                Sittings = new string[]{"Lunch"},
                Sizes = new string[] {"Regular"},
                Prices = new decimal[]{27.50m},
                Vegan = false
            },
            new Product
            {
                Name = "Wagyu Brisket",
                Sittings = new string[]{"Lunch"},
                Sizes = new string[] {"Regular"},
                Prices = new decimal[]{22.50m},
                Vegan = false
            },
            new Product
            {
                Name = "Pasta Carbonara",
                Sittings = new string[]{"Lunch"},
                Sizes = new string[] {"Regular"},
                Prices = new decimal[]{25.90m},
                Vegan = false
            },
            new Product
            {
                Name = "Chicken Schnitzel",
                Sittings = new string[]{"Lunch"},
                Sizes = new string[] {"Regular"},
                Prices = new decimal[]{31.90m},
                Vegan = false
            },
            new Product
            {
                Name = "Beef Nachos",
                Sittings = new string[]{"Lunch"},
                Sizes = new string[] {"Regular"},
                Prices = new decimal[]{28.50m},
                Vegan = false
            },
            new Product
            {
                Name = "Chips",
                Sittings = new string[]{"Lunch"},
                Sizes = new string[] {"Regular"},
                Prices = new decimal[]{6.00m},
                Vegan = false
            },
            new Product
            {
                Name = "Rice",
                Sittings = new string[]{"Lunch"},
                Sizes = new string[] {"Regular"},
                Prices = new decimal[]{4.00m},
                Vegan = true
            },
            new Product
            {
                Name = "Double BBQ Burger",
                Sittings = new string[]{"Lunch"},
                Sizes = new string[] {"Regular", "Small"},
                Prices = new decimal[]{18.30m, 9.00m},
                Vegan = false
            },
            new Product
            {
                Name = "Mushroom Risotto",
                Sittings = new string[] {"Dinner"},
                Sizes= new string[] {"Regular"},
                Prices = new decimal[]{19.00m},
                Vegan = true
            },
            new Product
            {
                Name = "Pad Thai",
                Sittings = new string[] {"Dinner"},
                Sizes= new string[] {"Regular", "Small"},
                Prices = new decimal[]{18.00m, 7.00m},
                Vegan = false
            },
            new Product
            {
                Name = "Beef Noodles",
                Sittings = new string[] {"Dinner"},
                Sizes= new string[] {"Regular", "Small"},
                Prices = new decimal[]{16.00m, 8.00m},
                Vegan = false
            },
            new Product
            {
                Name = "Butter Chickem",
                Sittings = new string[] {"Dinner"},
                Sizes= new string[] {"Regular"},
                Prices = new decimal[]{19.00m},
                Vegan = false
            },
            new Product
            {
                Name = "Laksa",
                Sittings = new string[] {"Dinner"},
                Sizes= new string[] {"Regular"},
                Prices = new decimal[]{20.00m},
                Vegan = false
            },
            new Product
            {
                Name = "Paella",
                Sittings = new string[] {"Dinner"},
                Sizes= new string[] {"Regular"},
                Prices = new decimal[]{32.89m},
                Vegan = false
            },
            new Product
            {
                Name = "California Sushi Roll",
                Sittings = new string[] {"Dinner"},
                Sizes= new string[] {"Regular"},
                Prices = new decimal[]{12.50m},
                Vegan = false
            },
            new Product
            {
                Name = "Birria Tacos x3",
                Sittings = new string[] {"Dinner"},
                Sizes= new string[] {"Regular"},
                Prices = new decimal[]{18.30m},
                Vegan = false
            },
            new Product
            {
                Name = "Fillter Mignon",
                Sittings = new string[] {"Dinner"},
                Sizes= new string[] {"Regular"},
                Prices = new decimal[]{31.20m},
                Vegan = false
            },
            new Product
            {
                Name = "Soft Drink",
                Sittings = new string[] {"Breakfast", "Lunch", "Dinner"},
                Sizes= new string[] {"Regular", "Small"},
                Prices = new decimal[]{5.00m, 3.50m},
                Vegan = false
            },
            new Product
            {
                Name = "Tap Beer",
                Sittings = new string[] {"Lunch", "Dinner"},
                Sizes= new string[] {"Regular", "Small"},
                Prices = new decimal[]{11.00m, 9.00m},
                Vegan = false
            },
            new Product
            {
                Name = "Bottle of Water",
                Sittings = new string[] { "Breakfast", "Lunch", "Dinner" },
                Sizes= new string[] {"Regular"},
                Prices = new decimal[]{4.00m},
                Vegan = false
            },
            new Product
            {
                Name = "Selected Fruit Juice",
                Sittings = new string[] { "Breakfast", "Lunch", "Dinner"},
                Sizes= new string[] {"Regular", "Small"},
                Prices = new decimal[]{8.00m, 6.00m},
                Vegan = true
            }

        };

        //collection.InsertOne(waffle);
        collection.InsertMany(products);
    }
}