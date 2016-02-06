using System;
using System.Collections.Generic;
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
        private int _dataLocacao;
        private int _dataDevolucao;
        private int _status;
        private int _idSocio = -1;
        private SocioRepositorio socioRepositorio { get { return new SocioRepositorio(); } }
        private ItemLocacaoRepositorio itemLocacaoRepositorio { get { return new ItemLocacaoRepositorio(); } }

        public int Insert(Locacao item)
        {
            try
            {
                const string sql = @"INSERT INTO [dbo].[Locacao]
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
                    if (!string.IsNullOrEmpty(item.Socio.Cpf))
                        _idSocio = socioRepositorio.GetByCpf(item.Socio.Cpf).Id;
                    if (!string.IsNullOrEmpty(item.Socio.Rg))
                        _idSocio = socioRepositorio.GetByRg(item.Socio.Rg).Id;
                    if (!string.IsNullOrEmpty(item.Socio.Email))
                        _idSocio = socioRepositorio.GetBy(item.Socio.Email).Id;
                }

                var parametros = new Dictionary<string, object>
                {
                    {"@idsocio", _idSocio},
                    {"@data_locacao", item.DataLocacao},
                    {"@data_devolucao", item.DataDevolucao},
                    {"@status", item.Status}
                };

                return ExecuteCommand(sql, parametros);
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
            finally
            {
                _idSocio = -1;
            }
        }

        public void Remove(Locacao item)
        {
            try
            {
                itemLocacaoRepositorio.RemoveAllBy(item); // Remove todos os itens de locação.

                const string sql = @"DELETE FROM [dbo].[Locacao] WHERE [id]=@id";
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
                    if (!string.IsNullOrEmpty(socio.Cpf))
                        _idSocio = socioRepositorio.GetByCpf(socio.Cpf).Id;
                    if (!string.IsNullOrEmpty(socio.Rg))
                        _idSocio = socioRepositorio.GetByRg(socio.Rg).Id;
                    if (!string.IsNullOrEmpty(socio.Email))
                        _idSocio = socioRepositorio.GetBy(socio.Email).Id;
                }

                const string sql = @"DELETE FROM [dbo].[Locacao] WHERE [idsocio]=@idsocio";
                var parametros = new Dictionary<string, object>();

                if (socio != null && socio.Id != 0)
                    parametros.Add("@idsocio", _idSocio);

                ExecuteCommand(sql, parametros);
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
            finally
            {
                _idSocio = -1;
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
                    if (!string.IsNullOrEmpty(item.Socio.Cpf))
                        _idSocio = socioRepositorio.GetByCpf(item.Socio.Cpf).Id;
                    if (!string.IsNullOrEmpty(item.Socio.Rg))
                        _idSocio = socioRepositorio.GetByRg(item.Socio.Rg).Id;
                    if (!string.IsNullOrEmpty(item.Socio.Email))
                        _idSocio = socioRepositorio.GetBy(item.Socio.Email).Id;
                }

                var parametros = new Dictionary<string, object>
                {
                    {"@idsocio", _idSocio},
                    {"@data_locacao", item.DataLocacao},
                    {"@data_devolucao", item.DataDevolucao},
                    {"@status", item.Status},
                    {"@id", item.ID}
                };

                ExecuteCommand(sql, parametros);
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
            finally
            {
                _idSocio = -1;
            }
        }

        public Locacao GetBy(int id)
        {
            try
            {
                const string sql = @"SELECT [id]
      ,[idsocio]
      ,[data_locacao]
      ,[data_devolucao]
      ,[status]
  FROM [dbo].[Locacao] 
WHERE id = @id";
                var parametros = new Dictionary<string, object> {{"@id", id}};
                var dataReader = ExecuteReader(sql, parametros);
                var item = Populate(dataReader);

                dataReader.Close();
                dataReader.Dispose();
                ConnectionFactory.Fechar();

                return item;
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public List<Locacao> GetAllBy(Socio socio, bool status)
        {
            try
            {
                var locacoes = new List<Locacao>();
                const string sql = @"SELECT [id]
      ,[idsocio]
      ,[data_locacao]
      ,[data_devolucao]
      ,[status]
  FROM [dbo].[Locacao] WHERE [idsocio] = @idsocio AND [status] = @status";

                if (socio != null)
                {
                    if (!string.IsNullOrEmpty(socio.Cpf))
                        _idSocio = socioRepositorio.GetByCpf(socio.Cpf).Id;
                    if (!string.IsNullOrEmpty(socio.Rg))
                        _idSocio = socioRepositorio.GetByRg(socio.Rg).Id;
                    if (!string.IsNullOrEmpty(socio.Email))
                        _idSocio = socioRepositorio.GetBy(socio.Email).Id;
                }

                var parametros = new Dictionary<string, object> {{"@idsocio", _idSocio}, {"@status", status}};
                var dataReader = ExecuteReader(sql, parametros);

                while (dataReader.Read())
                    locacoes.Add(Populate(dataReader));

                dataReader.Close();
                dataReader.Dispose();
                ConnectionFactory.Fechar();

                return locacoes;
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
            finally
            {
                _idSocio = -1;
            }
        }

        public List<Locacao> GetAllBy(bool status)
        {
            try
            {
                var locacoes = new List<Locacao>();
                const string sql = @"SELECT [id]
      ,[idsocio]
      ,[data_locacao]
      ,[data_devolucao]
      ,[status]
  FROM [dbo].[Locacao] WHERE [status] = @status";

                var parametros = new Dictionary<string, object> {{"@status", status}};
                var dataReader = ExecuteReader(sql, parametros);

                while (dataReader.Read())
                    locacoes.Add(Populate(dataReader));

                dataReader.Close();
                dataReader.Dispose();
                ConnectionFactory.Fechar();

                return locacoes;
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public List<Locacao> GetAllBy(DateTime dataLocacao)
        {
            try
            {
                var locacoes = new List<Locacao>();
                const string sql = @"SELECT [id]
      ,[idsocio]
      ,[data_locacao]
      ,[data_devolucao]
      ,[status]
  FROM [dbo].[Locacao] WHERE [data_locacao] BETWEEN @data_locacao AND @dataHoje";

                var parametros = new Dictionary<string, object>
                {
                    {"@data_locacao", dataLocacao},
                    {"@dataHoje", DateTime.Now}
                };
                var dataReader = ExecuteReader(sql, parametros);

                while (dataReader.Read())
                    locacoes.Add(Populate(dataReader));

                dataReader.Close();
                dataReader.Dispose();
                ConnectionFactory.Fechar();

                return locacoes;
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public List<Locacao> GetAllBy(DateTime dataLocacao, bool status)
        {
            try
            {
                var locacoes = new List<Locacao>();
                const string sql = @"SELECT [id]
      ,[idsocio]
      ,[data_locacao]
      ,[data_devolucao]
      ,[status]
  FROM [dbo].[Locacao] WHERE [status] = @status AND [data_locacao] BETWEEN @data_locacao AND @dataHoje";

                var parametros = new Dictionary<string, object>
                {
                    {"@data_locacao", dataLocacao},
                    {"@dataHoje", DateTime.Now},
                    {"@status", status}
                };
                var dataReader = ExecuteReader(sql, parametros);

                while (dataReader.Read())
                    locacoes.Add(Populate(dataReader));

                dataReader.Close();
                dataReader.Dispose();
                ConnectionFactory.Fechar();

                return locacoes;
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public List<Locacao> GetAllBy(Socio socio, DateTime dataLocacao)
        {
            try
            {
                var locacoes = new List<Locacao>();
                const string sql = @"SELECT [id]
      ,[idsocio]
      ,[data_locacao]
      ,[data_devolucao]
      ,[status]
  FROM [dbo].[Locacao] WHERE [idsocio] = @idsocio AND [data_locacao] BETWEEN @data_locacao AND @dataHoje";

                if (socio != null)
                {
                    if (!string.IsNullOrEmpty(socio.Cpf))
                        _idSocio = socioRepositorio.GetByCpf(socio.Cpf).Id;
                    if (!string.IsNullOrEmpty(socio.Rg))
                        _idSocio = socioRepositorio.GetByRg(socio.Rg).Id;
                    if (!string.IsNullOrEmpty(socio.Email))
                        _idSocio = socioRepositorio.GetBy(socio.Email).Id;
                }

                var parametros = new Dictionary<string, object>
                {
                    {"@idsocio", _idSocio},
                    {"@data_locacao", dataLocacao},
                    {"@dataHoje", DateTime.Now}
                };
                var dataReader = ExecuteReader(sql, parametros);

                while (dataReader.Read())
                    locacoes.Add(Populate(dataReader));

                dataReader.Close();
                dataReader.Dispose();
                ConnectionFactory.Fechar();

                return locacoes;
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
            finally
            {
                _idSocio = -1;
            }
        }

        public List<Locacao> GetAllBy(Socio socio)
        {
            try
            {
                var locacoes = new List<Locacao>();
                const string sql = @"SELECT [id]
      ,[idsocio]
      ,[data_locacao]
      ,[data_devolucao]
      ,[status]
  FROM [dbo].[Locacao] WHERE [idsocio] = @idsocio";

                if (socio != null)
                {
                    if (!string.IsNullOrEmpty(socio.Cpf))
                        _idSocio = socioRepositorio.GetByCpf(socio.Cpf).Id;
                    if (!string.IsNullOrEmpty(socio.Rg))
                        _idSocio = socioRepositorio.GetByRg(socio.Rg).Id;
                    if (!string.IsNullOrEmpty(socio.Email))
                        _idSocio = socioRepositorio.GetBy(socio.Email).Id;
                }

                var parametros = new Dictionary<string, object> {{"@idsocio", _idSocio}};
                var dataReader = ExecuteReader(sql, parametros);

                while (dataReader.Read())
                    locacoes.Add(Populate(dataReader));

                dataReader.Close();
                dataReader.Dispose();
                ConnectionFactory.Fechar();

                return locacoes;
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
            finally
            {
                _idSocio = -1;
            }
        }

        public List<Locacao> GetAll()
        {
            try
            {
                var locacoes = new List<Locacao>();
                const string sql = @"SELECT [id]
      ,[idsocio]
      ,[data_locacao]
      ,[data_devolucao]
      ,[status]
  FROM [dbo].[Locacao]";

                var dataReader = ExecuteReader(sql);
                while (dataReader.Read())
                    locacoes.Add(Populate(dataReader));

                dataReader.Close();
                dataReader.Dispose();
                ConnectionFactory.Fechar();

                return locacoes;
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        protected override Locacao Populate(System.Data.SqlClient.SqlDataReader dataReader)
        {
            const string msg = "Objeto DataReader não foi inicializado ou está fechado...";

            if (dataReader == null || dataReader.IsClosed)
                throw new ArgumentNullException(msg);

            _id = dataReader.GetOrdinal("id");
            _idsocio = dataReader.GetOrdinal("idsocio");
            _dataLocacao = dataReader.GetOrdinal("data_locacao");
            _dataDevolucao = dataReader.GetOrdinal("data_devolucao");
            _status = dataReader.GetOrdinal("status");

            var locacao = new Locacao();
            if (!dataReader.IsDBNull(_id))
                locacao.ID = dataReader.GetInt32(_id);

            if (!dataReader.IsDBNull(_idsocio))
                locacao.Socio = socioRepositorio.GetBy(dataReader.GetInt32(_idsocio));

            if (!dataReader.IsDBNull(_dataLocacao))
                locacao.DataLocacao = dataReader.GetDateTime(_dataLocacao);

            if (!dataReader.IsDBNull(_dataDevolucao))
                locacao.DataDevolucao = dataReader.GetDateTime(_dataDevolucao);

            if (!dataReader.IsDBNull(_status))
                locacao.Status = dataReader.GetBoolean(_status);

            return locacao;
        }
    }
}
