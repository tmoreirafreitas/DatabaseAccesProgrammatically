using System;
using System.Collections.Generic;
using SVD.Model;

namespace AdoNetDemo
{
    public class CategoriaRepositorio : RepositorioBase<Categoria>, IRepositorio<Categoria>
    {
        public int Insert(Categoria item)
        {
            try
            {
                const string sql = @"INSERT INTO [dbo].[Categoria]([descricao],
        [valor_locacao]) VALUES (@descricao,
         @valor_locacao);SELECT CAST(SCOPE_IDENTITY() AS INT);";
                var parametros = new Dictionary<string, object>
                {
                    {"@descricao", item.Descricao},
                    {"@valor_locacao", item.ValorLocacao}
                };

                return ExecuteCommand(sql, parametros);
            }

            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public void Remove(Categoria item)
        {
            try
            {
                const string sql = @"DELETE FROM [dbo].[Categoria]
      WHERE Id = @Id OR descricao = @descricao";
                var parametros = new Dictionary<string, object>();

                if (item.ID != 0)
                    parametros.Add("@Id", item.ID);

                if (!string.IsNullOrEmpty(item.Descricao))
                    parametros.Add("@descricao", item.Descricao);

                ExecuteCommand(sql, parametros);
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public void Update(Categoria item)
        {
            try
            {
                const string sql = @"UPDATE [dbo].[Categoria]
   SET [descricao] = @descricao
      ,[valor_locacao] = @valor_locacao
 WHERE Id = @Id";
                var parametros = new Dictionary<string, object>
                {
                    {"@Id", item.ID},
                    {"@descricao", item.Descricao},
                    {"@valor_locacao", item.ValorLocacao}
                };
                ExecuteCommand(sql, parametros);
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }

        }

        public Categoria GetBy(int id)
        {
            try
            {
                const string sql = @"SELECT [id]
      ,[descricao]
      ,[valor_locacao]
  FROM [dbo].[Categoria] WHERE Id = @Id";
                var parametros = new Dictionary<string, object> {{"@Id", id}};
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

        public Categoria GetBy(string descricao)
        {
            try
            {
                const string sql = @"SELECT [id]
      ,[descricao]
      ,[valor_locacao]
  FROM [dbo].[Categoria] WHERE descricao = @descricao";
                var parametros = new Dictionary<string, object> {{"@descricao", descricao}};
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

        public List<Categoria> GetAll()
        {
            try
            {
                const string sql = @"SELECT [id]
      ,[descricao]
      ,[valor_locacao]
  FROM [dbo].[Categoria]";
                var items = new List<Categoria>();
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

        public List<Categoria> GetAll(string categoria)
        {
            try
            {
                var items = new List<Categoria>();
                const string sql = @"SELECT [id]
      ,[descricao]
      ,[valor_locacao]
  FROM [dbo].[Categoria] WHERE [descricao] LIKE @descricao + '%'";

                var parametro = new Dictionary<string, object> {{"@descricao", categoria}};
                var dataReader = ExecuteReader(sql, parametro);
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
        protected override Categoria Populate(System.Data.SqlClient.SqlDataReader dataReader)
        {
            const string msg = "Objeto DataReader não foi inicializado ou está fechado...";

            if (dataReader == null)
                throw new ArgumentNullException(msg);

            var item = new Categoria();
            if (!dataReader.IsDBNull(0))
                item.ID = dataReader.GetInt32(0);

            if (!dataReader.IsDBNull(1))
                item.Descricao = dataReader.GetString(1);

            if (!dataReader.IsDBNull(2))
                item.ValorLocacao = dataReader.GetDecimal(2);

            return item;
        }
    }
}
