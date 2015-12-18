using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SVD.Model;

namespace AdoNetDemo
{
    public class LocacaoRepositorio : RepositorioBase<Locacao>, IRepositorio<Locacao>
    {
        public int Insert(Locacao item)
        {
            throw new NotImplementedException();
        }

        public void Remove(Locacao item)
        {
            throw new NotImplementedException();
        }

        public void Update(Locacao item)
        {
            throw new NotImplementedException();
        }

        public Locacao GetBy(int id)
        {
            throw new NotImplementedException();
        }

        public List<Locacao> GetAll()
        {
            throw new NotImplementedException();
        }

        protected override Locacao Populate(System.Data.IDataReader dataReader)
        {
            throw new NotImplementedException();
        }
    }
}
