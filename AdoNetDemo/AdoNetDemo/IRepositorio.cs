using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace AdoNetDemo
{
    interface IRepositorio<T> where T : class
    {
        int Insert(T item);
        void Remove(T item);
        void Update(T item);
        T GetBy(int id);
        List<T> GetAll();
    }
}
