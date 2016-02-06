using System;
using System.Collections.Generic;
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
                const string sql = @"INSERT INTO [dbo].[Atua] ([idator] ,[idfilme] ,[papel]) VALUES (@idator ,@idfilme ,@papel);SELECT CAST(SCOPE_IDENTITY() AS INT);";
                var parametros = new Dictionary<string, object>
                {
                    {"@idator", item.Ator.ID},
                    {"@idfilme", item.Filme.ID},
                    {"@papel", item.Papel}
                };

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
                const string sql = @"DELETE FROM [dbo].[Atua] WHERE idator = @idator AND idfilme = @idfilme";
                var parametros = new Dictionary<string, object> {{"@idator", item.Ator.ID}, {"@idfilme", item.Filme.ID}};
                ExecuteCommand(sql, parametros);
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public void RemoveAllBy(Ator ator)
        {
            try
            {
                const string sql = @"DELETE FROM [dbo].[Atua] WHERE idator = @idator";
                var parametros = new Dictionary<string, object> {{"@idator", ator.ID}};
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
                const string sql = @"DELETE FROM [dbo].[Atua] WHERE idfilme = @idfilme";
                var parametros = new Dictionary<string, object> {{"@idfilme", filme.ID}};
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
                const string sql = @"UPDATE [dbo].[Atua] SET [papel] = @papel WHERE idator = @idator AND idfilme = @idfilme";
                var parametros = new Dictionary<string, object>
                {
                    {"@papel", item.Papel},
                    {"@idator", item.Ator.ID},
                    {"@idfilme", item.Filme.ID}
                };
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
                const string sql = @"SELECT [idator] ,[idfilme] ,[papel] FROM [dbo].[Atua] WHERE idator = @idator AND idfilme = @idfilme";
                var parametros = new Dictionary<string, object> {{"@idator", idator}, {"@idfilme", idfilme}};
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

        public List<Atuacao> GetAll()
        {
            try
            {
                var atuacoes = new List<Atuacao>();
                const string sql = @"SELECT [idator] ,[idfilme] ,[papel] FROM [dbo].[Atua]";
                var dataReader = ExecuteReader(sql);

                while (dataReader.Read())
                    atuacoes.Add(Populate(dataReader));

                dataReader.Close();
                dataReader.Dispose();
                ConnectionFactory.Fechar();

                return atuacoes;
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        protected override Atuacao Populate(System.Data.SqlClient.SqlDataReader dataReader)
        {
            const string msg = "Objeto DataReader não foi inicializado ou está fechado...";

            if (dataReader == null || dataReader.IsClosed)
                throw new ArgumentNullException(msg);

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
    }
}
