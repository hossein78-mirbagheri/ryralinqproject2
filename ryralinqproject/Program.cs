using System;
using System.Collections.Generic;
using System.Linq;
namespace ryralinqproject
{
    

    // Enum برای دسته بندی محصولات
    public enum Category
    {
        Electronics,
        Clothing,
        Books,
        Food,
        Furniture
    }

    // کلاس محصول
    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public Category Category { get; set; }

        public Product(string name, decimal price, Category category)
        {
            Name = name;
            Price = price;
            Category = category;
        }
    }

    // سرویس انتزاعی برای مدیریت محصولات
    public abstract class ProductService
    {
        public abstract IEnumerable<Product> GetAllProducts();
    }

    // پیاده‌سازی سرویس نمونه برای بازیابی محصولات
    public class SampleProductService : ProductService
    {
        public override IEnumerable<Product> GetAllProducts()
        {
            return new List<Product>
        {
            new Product("Laptop", 1200, Category.Electronics),
            new Product("Smartphone", 800, Category.Electronics),
            new Product("T-shirt", 25, Category.Clothing),
            new Product("Book: C# Programming", 45, Category.Books),
            new Product("Table", 150, Category.Furniture),
            new Product("Apple", 2, Category.Food)
        };
        }
    }

    // کلاس اصلی که در آن کائری های LINQ را ایجاد خواهیم کرد
    public class ProductQueries
    {
        private readonly ProductService _productService;

        public ProductQueries(ProductService productService)
        {
            _productService = productService;
        }

        // 1. بازیابی تمامی محصولات از یک دسته‌بندی خاص
        public IEnumerable<Product> GetProductsByCategory(Category category)
        {
            var products = _productService.GetAllProducts();

            var result = from product in products
                         where product.Category == category
                         select product;

            return result;
        }

        // 2. پیدا کردن محصول با بالاترین قیمت
        public Product GetMostExpensiveProduct()
        {
            var products = _productService.GetAllProducts();

            var mostExpensiveProduct = (from product in products
                                        orderby product.Price descending
                                        select product).FirstOrDefault();

            return mostExpensiveProduct;
        }

        // 3. محاسبه مجموع قیمت تمامی محصولات
        public decimal GetTotalPriceOfAllProducts()
        {
            var products = _productService.GetAllProducts();

            var totalPrice = products.Sum(product => product.Price);

            return totalPrice;
        }

        // 4. گروه‌بندی محصولات بر اساس دسته‌بندی
        public IEnumerable<IGrouping<Category, Product>> GetProductsGroupedByCategory()
        {
            var products = _productService.GetAllProducts();

            var groupedProducts = from product in products
                                  group product by product.Category into productGroup
                                  select productGroup;

            return groupedProducts;
        }

        // 5. پیدا کردن میانگین قیمت محصولات
        public decimal GetAveragePriceOfAllProducts()
        {
            var products = _productService.GetAllProducts();

            var averagePrice = products.Average(product => product.Price);

            return averagePrice;
        }
    }

    // برنامه اصلی برای اجرای کائری ها
    public class Program
    {
        public static void Main()
        {
            // استفاده از سرویس نمونه برای بازیابی محصولات
            var productService = new SampleProductService();
            var productQueries = new ProductQueries(productService);

            // 1. نمایش تمامی محصولات از دسته‌بندی Electronics
            var electronics = productQueries.GetProductsByCategory(Category.Electronics);
            Console.WriteLine("Products in Electronics category:");
            foreach (var product in electronics)
            {
                Console.WriteLine($"- {product.Name}: ${product.Price}");
            }

            // 2. نمایش محصول با بالاترین قیمت
            var mostExpensiveProduct = productQueries.GetMostExpensiveProduct();
            Console.WriteLine($"\nMost expensive product: {mostExpensiveProduct.Name} - ${mostExpensiveProduct.Price}");

            // 3. نمایش مجموع قیمت تمامی محصولات
            var totalPrice = productQueries.GetTotalPriceOfAllProducts();
            Console.WriteLine($"\nTotal price of all products: ${totalPrice}");

            // 4. نمایش محصولات گروه‌بندی شده بر اساس دسته‌بندی
            var groupedProducts = productQueries.GetProductsGroupedByCategory();
            Console.WriteLine("\nProducts grouped by category:");
            foreach (var group in groupedProducts)
            {
                Console.WriteLine($"{group.Key}:");
                foreach (var product in group)
                {
                    Console.WriteLine($"  - {product.Name} - ${product.Price}");
                }
            }

            // 5. نمایش میانگین قیمت محصولات
            var averagePrice = productQueries.GetAveragePriceOfAllProducts();
            Console.WriteLine($"\nAverage price of all products: ${averagePrice}");
        }
    }

}
