using System.Linq.Expressions;

namespace efcoreApi.Services
{
    public interface IRepositoryBase<T> where T : class
    {
        T GetById(int id);
        IEnumerable<T> FindAll();
        IEnumerable<T> Find(Expression<Func<T, bool>> expression);
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}
