using SqlQueryProviderLib;
using DataLib;

namespace SqlQueryProviderConsoleApp;

public class Application
{
   private readonly IQueryable<Product> _products;
   private readonly IQueryable<Order> _orders;

   public Application(IQueryable<Product> products, IQueryable<Order> orders)
   {
      _products = products;
      _orders = orders;
   }

   public void Run()
   {
      //var result = _products.Where(p => p.Id == 1).ToList();

      //foreach (var product in result)
      //{
      //   Console.WriteLine(product.Id);
      //   Console.WriteLine(product.Description);
      //   Console.WriteLine(product.Weight);
      //   Console.WriteLine(product.Height);
      //   Console.WriteLine(product.Width);
      //   Console.WriteLine(product.Length);
      //}

      //var query = from p in _products
      //            where p.Id == 1
      //            select p;

      //foreach (var product in query)
      //{
      //   Console.WriteLine(product.Id);
      //   Console.WriteLine(product.Description);
      //   Console.WriteLine(product.Weight);
      //   Console.WriteLine(product.Height);
      //   Console.WriteLine(product.Width);
      //   Console.WriteLine(product.Length);
      //}

      //var result2 = _products.Where(p => p.Id > 1).ToList();

      //foreach (var product in result2)
      //{
      //   Console.WriteLine(product.Id);
      //   Console.WriteLine(product.Description);
      //   Console.WriteLine(product.Weight);
      //   Console.WriteLine(product.Height);
      //   Console.WriteLine(product.Width);
      //   Console.WriteLine(product.Length);
      //}

      //var result3 = _products.Where(p => p.Id < 10).ToList();

      //foreach (var product in result3)
      //{
      //   Console.WriteLine(product.Id);
      //   Console.WriteLine(product.Description);
      //   Console.WriteLine(product.Weight);
      //   Console.WriteLine(product.Height);
      //   Console.WriteLine(product.Width);
      //   Console.WriteLine(product.Length);
      //}

      //var result4 = _products.Where(p => p.Description == "Test prod 1" && p.Id == 1).ToList();

      //foreach (var product in result4)
      //{
      //   Console.WriteLine(product.Id);
      //   Console.WriteLine(product.Description);
      //   Console.WriteLine(product.Weight);
      //   Console.WriteLine(product.Height);
      //   Console.WriteLine(product.Width);
      //   Console.WriteLine(product.Length);
      //}

      //var query2 = from p in _products
      //             where p.Id == 1 &&
      //             p.Description == "Test prod 1"
      //             select p;

      //foreach (var product in query2)
      //{
      //   Console.WriteLine(product.Id);
      //   Console.WriteLine(product.Description);
      //   Console.WriteLine(product.Weight);
      //   Console.WriteLine(product.Height);
      //   Console.WriteLine(product.Width);
      //   Console.WriteLine(product.Length);
      //}

      //var result = _orders.Where(p => p.Id == 1).ToList();

      //foreach (var order in result)
      //{
      //   Console.WriteLine(order.Id);
      //   Console.WriteLine(order.CreateDate);
      //   Console.WriteLine(order.UpdateDate);
      //   Console.WriteLine(order.ProductId);
      //   Console.WriteLine(order.Status);
      //}
   }
}