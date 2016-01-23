using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SVD.Model;

namespace AdoNetDemo
{
    public class CategoriaRepositorio : RepositorioBase<Categoria>, IRepositorio<Categoria>
    {
        public int Insert(Categoria item)
        {
            try
            {
                string sql = @"INSERT INTO [dbo].[Categoria]([descricao],
        [valor_locacao]) VALUES (@descricao,
         @valor_locacao);SELECT CAST(SCOPE_IDENTITY() AS INT);";
                var parametros = new Dictionary<string, object>();
                parametros.Add("@descricao", item.Descricao);
                parametros.Add("@valor_locacao", item.ValorLocacao);

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
                string sql = @"DELETE FROM [dbo].[Categoria]
      WHERE ID = @ID OR descricao = @descricao";
                var parametros = new Dictionary<string, object>();

                if (item.ID != 0)
                    parametros.Add("@ID", item.ID);

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
                string sql = @"UPDATE [dbo].[Categoria]
   SET [descricao] = @descricao
      ,[valor_locacao] = @valor_locacao
 WHERE ID = @ID";
                var parametros = new Dictionary<string, object>();
                parametros.Add("@ID", item.ID);
                parametros.Add("@descricao", item.Descricao);
                parametros.Add("@valor_locacao", item.ValorLocacao);
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
                string sql = @"SELECT [id]
      ,[descricao]
      ,[valor_locacao]
  FROM [dbo].[Categoria] WHERE ID = @ID";
                var parametros = new Dictionary<string, object>();
                parametros.Add("@ID", id);

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
                string sql = @"SELECT [id]
      ,[descricao]
      ,[valor_locacao]
  FROM [dbo].[Categoria] WHERE descricao = @descricao";
                var parametros = new Dictionary<string, object>();
                parametros.Add("@descricao", descricao);

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
                string sql = @"SELECT [id]
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
                string sql = @"SELECT [id]
      ,[descricao]
      ,[valor_locacao]
  FROM [dbo].[Categoria] [descricao] LIKE @descricao + '%'";
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
        protected override Categoria Populate(System.Data.SqlClient.SqlDataReader dataReader)
        {
            if (dataReader != null || !dataReader.IsClosed)
            {
                Categoria item = new Categoria();
                if (!dataReader.IsDBNull(0))
                    item.ID = dataReader.GetInt32(0);

                if (!dataReader.IsDBNull(1))
                    item.Descricao = dataReader.GetString(1);

                if (!dataReader.IsDBNull(2))
                    item.ValorLocacao = dataReader.GetDecimal(2);

                return item;
            }

            throw new ArgumentNullException("Objeto DataReader não foi inicializado ou está fechado...");
        }
    }
}
