using System;
using System.Collections.Generic;
using SVD.Model;

namespace AdoNetDemo
{
    public class CopiaRepositorio : RepositorioBase<Copia>, IRepositorio<Copia>
    {

        //id	            int
        //idfilme	        int
        //datacopia	        date
        //situacao_copia	bit

        private int _id;
        private int _idfilme;
        private int _datacopia;
        private int _situacaoCopia;
        private static FilmeRepositorio FilmeRepositorio { get { return new FilmeRepositorio(); } }
        private static ItemLocacaoRepositorio ItemLocacaoRepositorio { get { return new ItemLocacaoRepositorio(); } }
        private int _idFilme = -1;
        private Filme _filme;

        public int Insert(Copia item)
        {
            try
            {
                if (item.Filme != null)
                    if (item.Filme.ID == 0)
                        if (!string.IsNullOrEmpty(item.Filme.Titulo))
                            item.Filme = FilmeRepositorio.GetBy(item.Filme.Titulo);

                const string sql = @"INSERT INTO [dbo].[Copia] ([idfilme] ,[datacopia] ,[situacao_copia]) " +
                                   @"VALUES (@idfilme ,@datacopia ,@situacao_copia);SELECT CAST(SCOPE_IDENTITY() AS INT);";
                var parametros = new Dictionary<string, object>();
                if (item.Filme != null) parametros.Add("@idfilme", item.Filme.ID);
                parametros.Add("@datacopia", item.DataCopia);
                parametros.Add("@situacao_copia", item.SituacaoCopia);

                return ExecuteCommand(sql, parametros);
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public void Remove(Copia item)
        {
            try
            {
                ItemLocacaoRepositorio.RemoveAllBy(item);
                const string sql = @"DELETE FROM [dbo].[Copia] WHERE id = @id";
                var parametros = new Dictionary<string, object> {{"@id", item.ID}};

                ExecuteCommand(sql, parametros);
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public void RemoveAllBy(Filme filme)
        {
            try
            {
                var copias = GetAllBy(filme);

                foreach (var copia in copias)
                    ItemLocacaoRepositorio.RemoveAllBy(copia);

                const string sql = @"DELETE FROM [dbo].[Copia] WHERE idfilmes = @idfilmes";
                var parametros = new Dictionary<string, object> {{"@idfilmes", filme.ID}};

                ExecuteCommand(sql, parametros);
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public void Update(Copia item)
        {
            try
            {
                if (item.Filme != null)
                    if (!string.IsNullOrEmpty(item.Filme.Titulo))
                        item.Filme = FilmeRepositorio.GetBy(item.Filme.Titulo);

                const string sql = @"UPDATE [dbo].[Copia]
   SET [idfilme] = @idfilme
      ,[datacopia] = @datacopia
      ,[situacao_copia] = @situacao_copia
 WHERE id = @id";
                var parametros = new Dictionary<string, object>
                {
                    {"@id", item.ID},
                    {"@idfilme", item.Filme.ID},
                    {"@datacopia", item.DataCopia},
                    {"@situacao_copia", item.SituacaoCopia}
                };

                ExecuteCommand(sql, parametros);
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public Copia GetBy(int id)
        {
            try
            {
                const string sql = @"SELECT [id]
      ,[idfilme]
      ,[datacopia]
      ,[situacao_copia]
  FROM [dbo].[Copia] WHERE id = @id";
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
            finally
            {
                _idFilme = -1;
                if (_filme != null)
                    _filme = null;
            }
        }

        public List<Copia> GetAll()
        {
            try
            {
                var copias = new List<Copia>();
                const string sql = @"SELECT [id]
      ,[idfilme]
      ,[datacopia]
      ,[situacao_copia]
  FROM [dbo].[Copia]";
                var dataReader = ExecuteReader(sql);
                while (dataReader.Read())
                    copias.Add(Populate(dataReader));

                dataReader.Close();
                dataReader.Dispose();
                ConnectionFactory.Fechar();

                return copias;
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
            finally
            {
                _idFilme = -1;
                if (_filme != null)
                    _filme = null;
            }
        }

        public List<Copia> GetAllBy(Filme filme)
        {
            try
            {
                if (filme != null)
                    if (filme.ID == 0)
                        if (!string.IsNullOrEmpty(filme.Titulo))
                            filme = FilmeRepositorio.GetBy(filme.Titulo);

                var copias = new List<Copia>();
                const string sql = @"SELECT [id]
      ,[idfilme]
      ,[datacopia]
      ,[situacao_copia]
  FROM [dbo].[Copia] WHERE idfilmes = @idfilmes";
                _filme = filme;
                var parametros = new Dictionary<string, object> {{"@idfilmes", filme.ID}};
                var dataReader = ExecuteReader(sql, parametros);
                while (dataReader.Read())
                    copias.Add(Populate(dataReader));

                dataReader.Close();
                dataReader.Dispose();
                ConnectionFactory.Fechar();

                return copias;
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
            finally
            {
                _idFilme = -1;
                if (_filme != null)
                    _filme = null;
            }
        }

        protected override Copia Populate(System.Data.SqlClient.SqlDataReader dataReader)
        {
            const string msg = "Objeto DataReader não foi inicializado ou está fechado...";

            if (dataReader == null || dataReader.IsClosed)
                throw new ArgumentNullException(msg);

            _id = dataReader.GetOrdinal("id");
            _idfilme = dataReader.GetOrdinal("idfilme");
            _datacopia = dataReader.GetOrdinal("datacopia");
            _situacaoCopia = dataReader.GetOrdinal("situacao_copia");

            var copia = new Copia();

            if (!dataReader.IsDBNull(_id))
                copia.ID = dataReader.GetInt32(_id);

            if (!dataReader.IsDBNull(_datacopia))
                copia.DataCopia = dataReader.GetDateTime(_datacopia);

            if (!dataReader.IsDBNull(_situacaoCopia))
                copia.SituacaoCopia = dataReader.GetBoolean(_situacaoCopia);

            if (_idFilme == -1)
            {
                if (dataReader.IsDBNull(_idfilme)) return copia;
                _idFilme = dataReader.GetInt32(_idfilme);
                _filme = FilmeRepositorio.GetBy(_idFilme);
                copia.Filme = _filme;
            }
            else
            {
                if (dataReader.IsDBNull(_idfilme)) return copia;
                if (_idFilme != dataReader.GetInt32(_idfilme))
                {
                    _idFilme = dataReader.GetInt32(_idfilme);
                    _filme = FilmeRepositorio.GetBy(_idFilme);
                    copia.Filme = _filme;
                }
                else
                {
                    copia.Filme = _filme;
                }
            }

            return copia;
        }
    }
}
