using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SVD.Model;

namespace AdoNetDemo
{
    public class ItemLocacaoRepositorio : RepositorioBase<ItemLocacaoRepositorio>, IRepositorio<ItemLocacao>
    {
        public int Insert(ItemLocacao item)
        {
            throw new NotImplementedException();
        }

        public void Remove(ItemLocacao item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAllBy(Copia copia)
        {
            throw new NotImplementedException();
        }

        public void RemoveAllBy(Locacao locacao)
        {
            throw new NotImplementedException();
        }

        public void Update(ItemLocacao item)
        {
            throw new NotImplementedException();
        }

        public ItemLocacao GetBy(int id)
        {
            throw new NotImplementedException();
        }

        public List<ItemLocacao> GetAll()
        {
            throw new NotImplementedException();
        }

        protected override ItemLocacaoRepositorio Populate(System.Data.IDataReader dataReader)
        {
            throw new NotImplementedException();
        }
    }
}
