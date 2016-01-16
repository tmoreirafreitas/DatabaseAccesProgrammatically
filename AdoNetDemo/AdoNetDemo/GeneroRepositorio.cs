using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SVD.Model;

namespace AdoNetDemo
{
    public class GeneroRepositorio : RepositorioBase<Genero>, IRepositorio<Genero>
    {
        public int Insert(Genero item)
        {
            try
            {
                string sql = @"INSERT INTO [dbo].[Genero]([descricao]) VALUES (@descricao);SELECT CAST(SCOPE_IDENTITY() AS INT);";
                var parametro = new Dictionary<string, object>();
                parametro.Add("@descricao", item.Descricao);

                return ExecuteCommand(sql, parametro);
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public void Remove(Genero item)
        {
            try
            {
                string sql = @"DELETE FROM [dbo].[Genero] WHERE ID=@ID";
                var parametro = new Dictionary<string, object>();
                parametro.Add("@ID", item.ID);
                ExecuteCommand(sql, parametro);
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public void Update(Genero item)
        {
            try
            {
                string sql = @"UPDATE [dbo].[Genero] SET [descricao] = @descricao, WHERE ID = @ID";
                var parametro = new Dictionary<string, object>();
                parametro.Add("@descricao", item.Descricao);
                parametro.Add("@ID", item.ID);
                ExecuteCommand(sql, parametro);
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public Genero GetBy(int id)
        {
            try
            {
                string sql = @"SELECT [id],[descricao] FROM [dbo].[Genero] WHERE ID = @ID";
                var parametro = new Dictionary<string, object>();
                parametro.Add("@ID", id);
                return Populate(ExecuteReader(sql, parametro));
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public Genero GetBy(string descricao)
        {
            try
            {
                string sql = @"SELECT [id],[descricao] FROM [dbo].[Genero] WHERE descricao = @descricao";
                var parametro = new Dictionary<string, object>();
                parametro.Add("@descricao", descricao);
                return Populate(ExecuteReader(sql, parametro));
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public List<Genero> GetAll()
        {
            try
            {
                var items = new List<Genero>();
                string sql = @"SELECT [id],[descricao] FROM [dbo].[Genero]";
                var dataReader = ExecuteReader(sql);
                while (dataReader.Read())
                    items.Add(Populate(dataReader));

                return items;
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        protected override Genero Populate(System.Data.IDataReader dataReader)
        {
            if (dataReader != null || !dataReader.IsClosed)
            {
                var item = new Genero();

                if (dataReader.Read())
                {
                    if (dataReader.IsDBNull(0))
                        item.ID = dataReader.GetInt32(0);

                    if (dataReader.IsDBNull(1))
                        item.Descricao = dataReader.GetString(1);
                }

                return item;
            }

            throw new ArgumentNullException("Objeto DataReader não foi inicializado ou está fechado...");
        }
    }
}
