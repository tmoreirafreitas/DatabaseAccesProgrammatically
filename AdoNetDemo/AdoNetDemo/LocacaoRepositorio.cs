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
        //[id]
        //[idsocio]
        //[data_locacao]
        //[data_devolucao]
        //[status]

        private int _id;
        private int _idsocio;
        private int _data_locacao;
        private int _data_devolucao;
        private int _status;
        private int idSocio = -1;
        private SocioRepositorio socioRepositorio { get { return new SocioRepositorio(); } }
        private ItemLocacaoRepositorio itemLocacaoRepositorio { get { return new ItemLocacaoRepositorio(); } }

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
            try
            {
                itemLocacaoRepositorio.RemoveAllBy(item); // Remove todos os itens de locação.

                string sql = @"DELETE FROM [dbo].[Locacao] WHERE [id]=@id";
                var parametros = new Dictionary<string, object>();

                if (item.ID != 0)
                    parametros.Add("@id", item.ID);

                ExecuteCommand(sql, parametros);
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public void RemoveAllBy(Socio socio)
        {
            try
            {
                var locacoes = GetAllBy(socio);
                foreach (var locacao in locacoes)
                    itemLocacaoRepositorio.RemoveAllBy(locacao); // Remove todos os itens de locação.

                if (socio != null)
                {
                    if (!string.IsNullOrEmpty(socio.CPF))
                        idSocio = socioRepositorio.GetByCPF(socio.CPF).ID;
                    if (!string.IsNullOrEmpty(socio.RG))
                        idSocio = socioRepositorio.GetByRG(socio.RG).ID;
                    if (!string.IsNullOrEmpty(socio.Email))
                        idSocio = socioRepositorio.GetBy(socio.Email).ID;
                }

                string sql = @"DELETE FROM [dbo].[Locacao] WHERE [idsocio]=@idsocio";
                var parametros = new Dictionary<string, object>();

                if (socio.ID != 0)
                    parametros.Add("@idsocio", idSocio);

                ExecuteCommand(sql, parametros);
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

        public void Update(Locacao item)
        {
            try
            {
                string sql = @"UPDATE [dbo].[Locacao]
   SET [idsocio] = @idsocio
      ,[data_locacao] = @data_locacao
      ,[data_devolucao] = @data_devolucao
      ,[status] = @status
 WHERE [id] = @id";

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
                parametros.Add("@id", item.ID);

                ExecuteCommand(sql, parametros);
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

        public Locacao GetBy(int id)
        {
            try
            {
                string sql = @"SELECT [id]
      ,[idsocio]
      ,[data_locacao]
      ,[data_devolucao]
      ,[status]
  FROM [dbo].[Locacao] 
WHERE id = @id";
                var parametros = new Dictionary<string, object>();
                parametros.Add("@id", id);

                return Populate(ExecuteReader(sql, parametros));
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public List<Locacao> GetAllBy(Socio socio)
        {
            try
            {
                var locacoes = new List<Locacao>();
                string sql = @"SELECT [id]
      ,[idsocio]
      ,[data_locacao]
      ,[data_devolucao]
      ,[status]
  FROM [dbo].[Locacao] WHERE [idsocio] = @idsocio";

                if (socio != null)
                {
                    if (!string.IsNullOrEmpty(socio.CPF))
                        idSocio = socioRepositorio.GetByCPF(socio.CPF).ID;
                    if (!string.IsNullOrEmpty(socio.RG))
                        idSocio = socioRepositorio.GetByRG(socio.RG).ID;
                    if (!string.IsNullOrEmpty(socio.Email))
                        idSocio = socioRepositorio.GetBy(socio.Email).ID;
                }

                var parametros = new Dictionary<string, object>();
                parametros.Add("@idsocio", idSocio);
                var dataReader = ExecuteReader(sql, parametros);

                while (dataReader.Read())
                    locacoes.Add(Populate(dataReader));

                return locacoes;
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

        public List<Locacao> GetAll()
        {
            try
            {
                var locacoes = new List<Locacao>();
                string sql = @"SELECT [id]
      ,[idsocio]
      ,[data_locacao]
      ,[data_devolucao]
      ,[status]
  FROM [dbo].[Locacao]";

                var dataReader = ExecuteReader(sql);
                while (dataReader.Read())
                    locacoes.Add(Populate(dataReader));

                return locacoes;
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        protected override Locacao Populate(System.Data.IDataReader dataReader)
        {
            if (dataReader != null)
            {
                _id = dataReader.GetOrdinal("id");
                _idsocio = dataReader.GetOrdinal("idsocio");
                _data_locacao = dataReader.GetOrdinal("data_locacao");
                _data_devolucao = dataReader.GetOrdinal("data_devolucao");
                _status = dataReader.GetOrdinal("status");

                Locacao locacao = new Locacao();
                locacao.ID = dataReader.GetInt32(_id);
                locacao.Socio = socioRepositorio.GetBy(dataReader.GetInt32(_idsocio));
                locacao.DataLocacao = dataReader.GetDateTime(_data_locacao);
                locacao.DataDevolucao = dataReader.GetDateTime(_data_devolucao);
                locacao.Status = dataReader.GetBoolean(_status);

                return locacao;
            }

            throw new ArgumentNullException("Objeto DataReader não foi inicializado ou está fechado...");
        }
    }
}
