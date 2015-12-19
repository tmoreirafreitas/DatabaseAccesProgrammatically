using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SVD.Model;

namespace AdoNetDemo
{
    public class AtuacaoRepositorio : RepositorioBase<Atuacao>, IRepositorio<Atuacao>
    {
        //[idator]      int
        //[idfilme]     int
        //[papel]       varchar

        private FilmeRepositorio filmeRepositorio { get { return new FilmeRepositorio(); } }
        private AtorRepositorio atorRepositorio { get { return new AtorRepositorio(); } }
        private int _idAtor;
        private int _idFilme;
        private int _papel;

        public int Insert(Atuacao item)
        {
            try
            {
                string sql = @"INSERT INTO [dbo].[Atua] ([idator] ,[idfilme] ,[papel]) VALUES (@idator ,@idfilme ,@papel) SELECT SCOPE_IDENTITY()";
                var parametros = new Dictionary<string, object>();
                parametros.Add("@idator", item.Ator.ID);
                parametros.Add("@idfilme", item.Filme.ID);
                parametros.Add("@papel", item.Papel);

                return ExecuteCommand(sql, parametros);
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public void Remove(Atuacao item)
        {
            try
            {
                string sql = @"DELETE FROM [dbo].[Atua] WHERE idator = @idator AND idfilme = @idfilme";
                var parametros = new Dictionary<string, object>();
                parametros.Add("@idator", item.Ator.ID);
                parametros.Add("@idfilme", item.Filme.ID);
                ExecuteCommand(sql, parametros);
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public void Update(Atuacao item)
        {
            try
            {
                string sql = @"UPDATE [dbo].[Atua] SET [papel] = @papel WHERE idator = @idator AND idfilme = @idfilme";
                var parametros = new Dictionary<string, object>();
                parametros.Add("@papel", item.Papel);
                parametros.Add("@idator", item.Ator.ID);
                parametros.Add("@idfilme", item.Filme.ID);
                ExecuteCommand(sql, parametros);
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public Atuacao GetBy(int id)
        {
            throw new NotImplementedException("Funcionalidade não válida para esta classe, use o método: GetBy(int idator, int idfilme)");
        }

        public Atuacao GetBy(int idator, int idfilme)
        {
            try
            {
                string sql = @"SELECT [idator] ,[idfilme] ,[papel] FROM [dbo].[Atua] WHERE idator = @idator AND idfilme = @idfilme";
                var parametros = new Dictionary<string, object>();
                parametros.Add("@idator", idator);
                parametros.Add("@idfilme", idfilme);

                return Populate(ExecuteReader(sql, parametros));
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public List<Atuacao> GetAll()
        {
            try
            {
                var atuacoes = new List<Atuacao>();
                string sql = @"SELECT [idator] ,[idfilme] ,[papel] FROM [dbo].[Atua]";
                var dataReader = ExecuteReader(sql);

                while (dataReader.Read())
                    atuacoes.Add(Populate(dataReader));

                return atuacoes;
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        protected override Atuacao Populate(System.Data.IDataReader dataReader)
        {
            if (dataReader != null)
            {
                _idAtor = dataReader.GetOrdinal("idator");
                _idFilme = dataReader.GetOrdinal("idfilme");
                _papel = dataReader.GetOrdinal("papel");

                Atuacao atuacao = new Atuacao
                {
                    Filme = filmeRepositorio.GetBy(dataReader.GetInt32(_idFilme)),
                    Ator = atorRepositorio.GetBy(dataReader.GetInt32(_idAtor)),
                    Papel = dataReader.GetString(_papel)
                };

                return atuacao;
            }

            throw new ArgumentNullException("Objeto DataReader não instanciado...");
        }
    }
}
