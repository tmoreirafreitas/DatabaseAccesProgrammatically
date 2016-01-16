using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private int _situacao_copia;
        private FilmeRepositorio filmeRepositorio { get { return new FilmeRepositorio(); } }
        private ItemLocacaoRepositorio itemLocacaoRepositorio { get { return new ItemLocacaoRepositorio(); } }
        private int idFilme = -1;
        private Filme filme;

        public int Insert(Copia item)
        {
            try
            {
                if (item.Filme != null)
                    if (item.Filme.ID == null)
                        if (!string.IsNullOrEmpty(item.Filme.Titulo))
                            item.Filme = filmeRepositorio.GetBy(item.Filme.Titulo);

                var sql = @"INSERT INTO [dbo].[Copia] ([idfilme] ,[datacopia] ,[situacao_copia]) " +
@"VALUES (@idfilme ,@datacopia ,@situacao_copia);SELECT CAST(SCOPE_IDENTITY() AS INT);";
                var parametros = new Dictionary<string, object>();
                parametros.Add("@idfilme", item.Filme.ID);
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
                itemLocacaoRepositorio.RemoveAllBy(item);
                var sql = @"DELETE FROM [dbo].[Copia] WHERE id = @id";
                var parametros = new Dictionary<string, object>();
                parametros.Add("@id", item.ID);

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
                    itemLocacaoRepositorio.RemoveAllBy(copia);

                var sql = @"DELETE FROM [dbo].[Copia] WHERE idfilmes = @idfilmes";
                var parametros = new Dictionary<string, object>();
                parametros.Add("@idfilmes", filme.ID);

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
                        item.Filme = filmeRepositorio.GetBy(item.Filme.Titulo);

                var sql = @"UPDATE [dbo].[Copia]
   SET [idfilme] = @idfilme
      ,[datacopia] = @datacopia
      ,[situacao_copia] = @situacao_copia
 WHERE id = @id";
                var parametros = new Dictionary<string, object>();
                parametros.Add("@id", item.ID);
                parametros.Add("@idfilme", item.Filme.ID);
                parametros.Add("@datacopia", item.DataCopia);
                parametros.Add("@situacao_copia", item.SituacaoCopia);

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
                var sql = @"SELECT [id]
      ,[idfilme]
      ,[datacopia]
      ,[situacao_copia]
  FROM [dbo].[Copia] WHERE id = @id";
                var parametros = new Dictionary<string, object>();
                parametros.Add("@id", id);

                return Populate(ExecuteReader(sql, parametros));
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
            finally
            {
                idFilme = -1;
                if (filme != null)
                    filme = null;
            }
        }

        public List<Copia> GetAll()
        {
            try
            {
                var copias = new List<Copia>();
                var sql = @"SELECT [id]
      ,[idfilme]
      ,[datacopia]
      ,[situacao_copia]
  FROM [dbo].[Copia]";
                var dataReader = ExecuteReader(sql);
                while (dataReader.Read())
                    copias.Add(Populate(dataReader));

                return copias;
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
            finally
            {
                idFilme = -1;
                if (filme != null)
                    filme = null;
            }
        }

        public List<Copia> GetAllBy(Filme filme)
        {
            try
            {
                if (filme != null)
                    if (filme.ID == 0)
                        if (!string.IsNullOrEmpty(filme.Titulo))
                            filme = filmeRepositorio.GetBy(filme.Titulo);

                var copias = new List<Copia>();
                var sql = @"SELECT [id]
      ,[idfilme]
      ,[datacopia]
      ,[situacao_copia]
  FROM [dbo].[Copia] WHERE idfilmes = @idfilmes";
                this.filme = filme;
                var parametros = new Dictionary<string, object>();
                parametros.Add("@idfilmes", filme.ID);
                var dataReader = ExecuteReader(sql, parametros);
                while (dataReader.Read())
                    copias.Add(Populate(dataReader));

                return copias;
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
            finally
            {
                idFilme = -1;
                if (this.filme != null)
                    this.filme = null;
            }
        }

        protected override Copia Populate(System.Data.IDataReader dataReader)
        {
            if (dataReader != null || !dataReader.IsClosed)
            {
                _id = dataReader.GetOrdinal("id");
                _idfilme = dataReader.GetOrdinal("idfilme");
                _datacopia = dataReader.GetOrdinal("datacopia");
                _situacao_copia = dataReader.GetOrdinal("situacao_copia");

                Copia copia = new Copia();

                if (!dataReader.IsDBNull(_id))
                    copia.ID = dataReader.GetInt32(_id);

                if (!dataReader.IsDBNull(_datacopia))
                    copia.DataCopia = dataReader.GetDateTime(_datacopia);

                if (!dataReader.IsDBNull(_situacao_copia))
                    copia.SituacaoCopia = dataReader.GetBoolean(_situacao_copia);

                if (idFilme == -1)
                {
                    if (!dataReader.IsDBNull(_idfilme))
                    {
                        idFilme = dataReader.GetInt32(_idfilme);
                        filme = filmeRepositorio.GetBy(idFilme);
                        copia.Filme = filme;
                    }
                }
                else
                {
                    if (!dataReader.IsDBNull(_idfilme))
                    {
                        if (idFilme != dataReader.GetInt32(_idfilme))
                        {
                            idFilme = dataReader.GetInt32(_idfilme);
                            filme = filmeRepositorio.GetBy(idFilme);
                            copia.Filme = filme;
                        }
                        else
                        {
                            copia.Filme = filme;
                        }
                    }
                }

                return copia;
            }

            throw new ArgumentNullException("Objeto DataReader não foi inicializado ou está fechado...");
        }
    }
}
