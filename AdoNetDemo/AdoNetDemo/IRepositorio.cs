using System.Collections.Generic;

namespace AdoNetDemo
{
    public interface IRepositorio<T> where T : class
    {
        int Insert(T item);
        void Remove(T item);
        void Update(T item);
        T GetBy(int id);
        List<T> GetAll();
    }
}
