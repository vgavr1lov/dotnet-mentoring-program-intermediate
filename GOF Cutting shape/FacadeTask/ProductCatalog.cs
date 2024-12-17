namespace FacadeTask;

public class ProductCatalog : IProductCatalog
{
    private List<Product> Products { get; set; } = new List<Product>
    {
        new Product { ProductId = "1", ProductName = "Chai", UnitPrice = 18.00m },
        new Product { ProductId = "2", ProductName = "Chang", UnitPrice = 19.00m },
        new Product { ProductId = "3", ProductName = "Aniseed Syrup", UnitPrice = 10.00m },
        new Product { ProductId = "4", ProductName = "Chef Anton's Cajun Seasoning", UnitPrice = 22.00m },
        new Product { ProductId = "5", ProductName = "Uncle Bob's Organic Dried Pears", UnitPrice = 30.00m },
        new Product { ProductId = "6", ProductName = "Northwoods Cranberry Sauce", UnitPrice = 40.00m },
        new Product { ProductId = "7", ProductName = "Mishi Kobe Niku", UnitPrice = 97.00m },
        new Product { ProductId = "8", ProductName = "Ikura", UnitPrice = 31.00m },
        new Product { ProductId = "9", ProductName = "Queso Cabrales", UnitPrice = 21.00m },
        new Product { ProductId = "10", ProductName = "Queso Manchego La Pastora", UnitPrice = 38.00m }
    };
    public Product GetProductDetails(string productId)
    {
        return Products.First(p => p.ProductId == productId);
    }
}
