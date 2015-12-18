using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SVD.Model;

namespace AdoNetDemo
{
    public class TelefoneRepositorio : RepositorioBase<Telefone>, IRepositorio<Telefone>
    {
        public int Insert(Telefone item)
        {
            throw new NotImplementedException();
        }

        public void Remove(Telefone item)
        {
            throw new NotImplementedException();
        }

        public void Update(Telefone item)
        {
            throw new NotImplementedException();
        }

        public Telefone GetBy(int id)
        {
            throw new NotImplementedException();
        }

        public List<Telefone> GetAll()
        {
            throw new NotImplementedException();
        }

        protected override Telefone Populate(System.Data.IDataReader dataReader)
        {
            throw new NotImplementedException();
        }
    }
}
