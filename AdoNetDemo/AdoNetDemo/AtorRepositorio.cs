using System;
using System.Collections.Generic;
using SVD.Model;

namespace AdoNetDemo
{
    public class AtorRepositorio : RepositorioBase<Ator>, IRepositorio<Ator>
    {
        public int Insert(Ator item)
        {
            try
            {
                const string sql = @"INSERT INTO [dbo].[Ator]([nome]) VALUES (@nome);CAST(SCOPE_IDENTITY() AS INT);";
                var parametros = new Dictionary<string, object> {{"@nome", item.Nome}};
                return ExecuteCommand(sql, parametros);
            }

            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public void Remove(Ator item)
        {
            try
            {
                const string sql = @"DELETE FROM [dbo].[Ator] WHERE id = @id";
                var parametros = new Dictionary<string, object> {{"@id", item.ID}};
                ExecuteCommand(sql, parametros);
            }

            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public void Update(Ator item)
        {
            try
            {
                const string sql = @"UPDATE [dbo].[Ator] SET [nome] = @nome WHERE id = @id";
                var parametros = new Dictionary<string, object> {{"@id", item.ID}, {"@nome", item.Nome}};
                ExecuteCommand(sql, parametros);
            }

            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public Ator GetBy(int id)
        {
            try
            {
                const string sql = @"SELECT [id],[nome] FROM [dbo].[Ator] WHERE id = @id";
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

        public Ator GetBy(string nome)
        {
            try
            {
                const string sql = @"SELECT [id],[nome] FROM [dbo].[Ator] WHERE nome = @nome";
                var parametros = new Dictionary<string, object> {{"@nome", nome}};
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

        public List<Ator> GetAll()
        {
            try
            {
                var items = new List<Ator>();
                const string sql = @"SELECT [id],[nome] FROM [dbo].[Ator]";
                var dataReader = ExecuteReader(sql);
                while (dataReader.Read())
                    items.Add(Populate(dataReader));

                dataReader.Close();
                dataReader.Dispose();
                ConnectionFactory.Fechar();

                return items;
            }

            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        protected override Ator Populate(System.Data.SqlClient.SqlDataReader dataReader)
        {
            const string msg = "Objeto DataReader não foi inicializado ou está fechado...";

            if (dataReader == null || dataReader.IsClosed)
                throw new ArgumentNullException(msg);

            var item = new Ator();

            if (!dataReader.IsDBNull(0))
                item.ID = dataReader.GetInt32(0);

            if (!dataReader.IsDBNull(1))
                item.Nome = dataReader.GetString(1);

            return item;
        }
    }
}
