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
        private SocioRepositorio socioRepositorio { get { return new SocioRepositorio(); } }
        private int idSocio = -1;

        public int Insert(Locacao item)
        {
            try
            {
                var sql = @"INSERT INTO [dbo].[Locacao]
           ([idsocio]
           ,[data_locacao]
           ,[data_devolucao]
           ,[status])
     VALUES
           (@idsocio
           ,@data_locacao
           ,@data_devolucao
           ,@status);SELECT CAST(SCOPE_IDENTITY() AS INT);";

                if (item.Socio != null)
                {
                    if (!string.IsNullOrEmpty(item.Socio.CPF))
                        idSocio = socioRepositorio.GetByCPF(item.Socio.CPF).ID;
                    if (!string.IsNullOrEmpty(item.Socio.RG))
                        idSocio = socioRepositorio.GetByRG(item.Socio.RG).ID;
                    if (!string.IsNullOrEmpty(item.Socio.Email))
                        idSocio = socioRepositorio.GetBy(item.Socio.Email).ID;
                }

                var parametros = new Dictionary<string, object>();
                parametros.Add("@idsocio", idSocio);
                parametros.Add("@data_locacao", item.DataLocacao);
                parametros.Add("@data_devolucao", item.DataDevolucao);
                parametros.Add("@status", item.Status);

                return ExecuteCommand(sql, parametros);
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
            finally
            {
                idSocio = -1;
            }
        }

        public void Remove(Locacao item)
        {
            
            throw new NotImplementedException();
        }

        public void RemoveAllBy(Socio socio)
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

        public List<Locacao> GetAllBy(Socio socio)
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
