using DataAccessLib.Models;
using DataAccessLib.Repositories;

namespace DataAccessLib.Interfaces;

public interface IUnitOfWork
{
   IRepository<Category> Categories { get; }
   IRepository<Product> Products { get; }
   void Dispose();
   int Save();
}
