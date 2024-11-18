using DataAccessLib.Interfaces;
using DataAccessLib.Models;
using DataAccessLib.Repositories;

namespace DataAccessLib;

public class UnitOfWork : IUnitOfWork
{
   private DatabaseContext DbContext { get; }
   public IRepository<Category> Categories { get; }
   public IRepository<Product> Products { get; }
   public UnitOfWork(DatabaseContext context)
   {
      DbContext = context;
      Categories = new GenericRepository<Category>(context);
      Products = new GenericRepository<Product>(context);
   }

   public void Dispose()
   {
      DbContext.Dispose();
   }

   public int Save()
   {
      return DbContext.SaveChanges();
   }
}
