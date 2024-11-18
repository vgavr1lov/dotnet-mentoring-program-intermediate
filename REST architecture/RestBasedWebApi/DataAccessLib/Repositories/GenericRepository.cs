using DataAccessLib.Interfaces;
using DataAccessLib.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLib.Repositories;

public class GenericRepository<T> : IRepository<T> where T : class
{
   protected DatabaseContext DbContext { get; set; }
   protected DbSet<T> DbSet { get; set; }

   public GenericRepository(DatabaseContext context)
   {
      DbContext = context;
      DbSet = DbContext.Set<T>();
   }
   public void Create(T entity)
   {
      DbContext.Add(entity);
   }

   public void Delete(int id)
   {
      var entity = DbSet.Find(id);
      if (entity != null)
         DbContext.Remove(entity);
   }

   public virtual T? Read(int id)
   {
      return DbSet.Find(id);
   }

   public virtual List<T> Read()
   {
      return DbSet.ToList();
   }

   public void Update(T entity)
   {
      DbSet.Attach(entity);
      DbContext.Entry(entity).State = EntityState.Modified;
   }
}