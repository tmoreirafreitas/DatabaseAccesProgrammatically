using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SVD.Model;

namespace AdoNetDemo
{
    public class SocioRepositorio : RepositorioBase<Socio>, IRepositorio<Socio>
    {
        public int Insert(Socio item)
        {
            throw new NotImplementedException();
        }

        public void Remove(Socio item)
        {
            throw new NotImplementedException();
        }

        public void Update(Socio item)
        {
            throw new NotImplementedException();
        }

        public Socio GetBy(int id)
        {
            throw new NotImplementedException();
        }

        public List<Socio> GetAll()
        {
            throw new NotImplementedException();
        }

        protected override Socio Populate(System.Data.IDataReader dataReader)
        {
            throw new NotImplementedException();
        }
    }
}
