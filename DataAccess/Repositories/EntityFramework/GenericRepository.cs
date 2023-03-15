using DataAccess.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccess.Repositories.EntityFramework
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {

        protected readonly DbContext Context;
        public GenericRepository(DbContext context)
        {
            Context = context;
        }

        public void Insert(T t)
        {
            Context.Add(t);
            Context.SaveChanges();
        }

        public void Delete(T t)
        {
            Context.Remove(t);
            Context.SaveChanges();
        }

        public void Update(T t)
        {
            Context.Update(t);
            Context.SaveChanges();
        }

        public List<T> GetListAll()
        {
            return Context.Set<T>().ToList();
        }

        public T GetById(int id)
        {
            return Context.Set<T>().Find(id);
        }

        public List<T> GetListAllByFilter(Expression<Func<T, bool>> filter)
        {
            return Context.Set<T>().Where(filter).ToList();
        }
    }
}
