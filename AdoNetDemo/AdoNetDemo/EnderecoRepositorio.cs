using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SVD.Model;

namespace AdoNetDemo
{
    public class EnderecoRepositorio : RepositorioBase<Endereco>, IRepositorio<Endereco>
    {
        public int Insert(Endereco item)
        {
            throw new NotImplementedException();
        }

        public void Remove(Endereco item)
        {
            throw new NotImplementedException();
        }

        public void Update(Endereco item)
        {
            throw new NotImplementedException();
        }

        public Endereco GetBy(int id)
        {
            throw new NotImplementedException();
        }

        public List<Endereco> GetAll()
        {
            throw new NotImplementedException();
        }

        protected override Endereco Populate(System.Data.IDataReader dataReader)
        {
            throw new NotImplementedException();
        }
    }
}
