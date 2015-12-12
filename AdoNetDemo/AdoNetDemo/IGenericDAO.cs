using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace AdoNetDemo
{
    interface IGenericDAO<T>
    {
        int Insert(T item);
        bool Remove(T item);
        bool Update(T item);
        T GetBy(int id);
        List<T> GetAll();
        T Populate(IDataReader dataReader);
    }
}
