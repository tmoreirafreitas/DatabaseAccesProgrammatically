using System;
using System.Collections.Generic;
using SVD.Model;

namespace AdoNetDemo
{
    public class ItemLocacaoRepositorio : RepositorioBase<ItemLocacao>, IRepositorio<ItemLocacao>
    {
        //[idlocacao]
        //[idcopia]
        //[valor_locacao]

        private int _idlocacao;
        private int _idcopia;
        private int _valorLocacao;
        private LocacaoRepositorio locacaoRepositorio { get { return new LocacaoRepositorio(); } }
        private CopiaRepositorio copiaRepositorio { get { return new CopiaRepositorio(); } }

        public int Insert(ItemLocacao item)
        {
            try
            {
                const string sql = @"INSERT INTO [dbo].[Item_Locacao]
           ([idlocacao]
           ,[idcopia]
           ,[valor_locacao])
     VALUES
           (@idlocacao
           ,@idcopia
           ,@valor_locacao);CAST(SCOPE_IDENTITY() AS INT);";

                var parametros = new Dictionary<string, object>
                {
                    {"@idlocacao", item.Locacao.ID},
                    {"@idcopia", item.Copia.ID},
                    {"@valor_locacao", item.ValorLocacao}
                };

                return ExecuteCommand(sql, parametros);
            }

            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public void Remove(ItemLocacao item)
        {
            try
            {
                const string sql = @"DELETE FROM [dbo].[Item_Locacao] WHERE Id = @Id";
                var parametro = new Dictionary<string, object> {{"@Id", item.ID}};
                ExecuteCommand(sql, parametro);
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public void RemoveAllBy(Copia copia)
        {
            try
            {
                const string sql = @"DELETE FROM [dbo].[Item_Locacao] WHERE idcopia = @idcopia";
                var parametro = new Dictionary<string, object>();
                parametro.Add("@idcopia", copia.ID);
                ExecuteCommand(sql, parametro);
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public void RemoveAllBy(Locacao locacao)
        {
            try
            {
                const string sql = @"DELETE FROM [dbo].[Item_Locacao] WHERE idlocacao = @idlocacao";
                var parametro = new Dictionary<string, object> {{"@idlocacao", locacao.ID}};
                ExecuteCommand(sql, parametro);
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public void Update(ItemLocacao item)
        {
            try
            {
                const string sql = @"UPDATE [dbo].[Item_Locacao]
   SET [idlocacao] = @idlocacao
      ,[idcopia] = @idcopia
      ,[valor_locacao] = @valor_locacao
 WHERE Id = @Id";
                var parametro = new Dictionary<string, object>
                {
                    {"@Id", item.ID},
                    {"@idlocacao", item.Locacao.ID},
                    {"@idcopia", item.Copia.ID},
                    {"@valor_locacao", item.ValorLocacao}
                };
                ExecuteCommand(sql, parametro);
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public ItemLocacao GetBy(int id)
        {
            try
            {
                const string sql = @"SELECT [idlocacao]
      ,[idcopia]
      ,[valor_locacao]
  FROM [dbo].[Item_Locacao] WHERE Id = @Id";
                var parametro = new Dictionary<string, object> {{"@Id", id}};
                var dataReader = ExecuteReader(sql, parametro);
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

        public ItemLocacao GetBy(int idLocacao, int idCopia)
        {
            try
            {
                const string sql = @"SELECT [idlocacao]
      ,[idcopia]
      ,[valor_locacao]
  FROM [dbo].[Item_Locacao] WHERE [idcopia] = @idcopia AND [idlocacao] = @idlocacao";
                var parametro = new Dictionary<string, object> {{"@idcopia", idCopia}, {"@idlocacao", idLocacao}};
                var dataReader = ExecuteReader(sql, parametro);
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

        public List<ItemLocacao> GetAll()
        {
            try
            {
                var items = new List<ItemLocacao>();
                const string sql = @"SELECT [idlocacao]
      ,[idcopia]
      ,[valor_locacao]
  FROM [dbo].[Item_Locacao]";
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

        protected override ItemLocacao Populate(System.Data.SqlClient.SqlDataReader dataReader)
        {
            _idlocacao = dataReader.GetOrdinal("idlocacao");
            _idcopia = dataReader.GetOrdinal("idcopia");
            _valorLocacao = dataReader.GetOrdinal("valor_locacao");

            const string msg = "Objeto DataReader não foi inicializado ou está fechado...";

            if (dataReader == null || dataReader.IsClosed)
                throw new ArgumentNullException(msg);

            var item = new ItemLocacao();

            if (!dataReader.IsDBNull(_idcopia))
                item.Copia = copiaRepositorio.GetBy(dataReader.GetInt32(_idcopia));

            if (!dataReader.IsDBNull(_idlocacao))
                item.Locacao = locacaoRepositorio.GetBy(dataReader.GetInt32(_idlocacao));

            if (!dataReader.IsDBNull(_valorLocacao))
                item.ValorLocacao = dataReader.GetDecimal(_valorLocacao);

            return item;
        }
    }
}
