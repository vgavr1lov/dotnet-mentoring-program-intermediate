namespace DataAccessLib.Interfaces;

public interface IRepository<T> where T : class
{
   void Create(T entity);
   void Delete(int id);
   T? Read(int id);
   void Update(T entity);
   List<T> Read();
}
