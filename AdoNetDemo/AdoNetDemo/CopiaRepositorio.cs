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

        public int Insert(Copia item)
        {
            try
            {
                if (item.Filme != null)
                    if (item.Filme.ID == null)
                        if (!string.IsNullOrEmpty(item.Filme.Titulo))
                            item.Filme = filmeRepositorio.GetBy(item.Filme.Titulo);

                item.ID = GetNextId("Copia");
                var sql = @"INSERT INTO [dbo].[Copia] ([id] ,[idfilme] ,[datacopia] ,[situacao_copia]) VALUES (@id ,@idfilme ,@datacopia ,@situacao_copia)";
                var parametros = new Dictionary<string, object>();
                parametros.Add("@id", item.ID);
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

        public void Update(Copia item)
        {
            try
            {
                if (item.Filme != null)
                    if (item.Filme.ID == null)
                        if (!string.IsNullOrEmpty(item.Filme.Titulo))
                            item.Filme = filmeRepositorio.GetBy(item.Filme.Titulo);

                var sql = @"UPDATE [dbo].[Copia] SET [idfilme] = @idfilme ,[datacopia] = @datacopia ,[situacao_copia] = @situacao_copia WHERE id = @id";
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
                var sql = @"SELECT [id] ,[idfilme] ,[datacopia] ,[situacao_copia] FROM [SVDB].[dbo].[Copia] WHERE id = @id";
                var parametros = new Dictionary<string, object>();
                parametros.Add("@id", id);

                return Populate(ExecuteQuery(sql, parametros));
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public List<Copia> GetAll()
        {
            try
            {
                var copias = new List<Copia>();
                var sql = @"SELECT [id] ,[idfilme] ,[datacopia] ,[situacao_copia] FROM [SVDB].[dbo].[Copia]";
                var dataReader = ExecuteQuery(sql);
                while (dataReader.Read())
                    copias.Add(Populate(dataReader));

                return copias;
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        protected override Copia Populate(System.Data.IDataReader dataReader)
        {
            if (dataReader != null)
            {
                _id = dataReader.GetOrdinal("id");
                _idfilme = dataReader.GetOrdinal("idfilme");
                _datacopia = dataReader.GetOrdinal("datacopia");
                _situacao_copia = dataReader.GetOrdinal("situacao_copia");

                Copia copia = new Copia();
                copia.ID = dataReader.GetInt32(_id);
                copia.Filme = filmeRepositorio.GetBy(dataReader.GetInt32(_idfilme));
                copia.DataCopia = dataReader.GetDateTime(_datacopia);
                copia.SituacaoCopia = dataReader.GetBoolean(_situacao_copia);

                return copia;
            }

            return null;
        }
    }
}
