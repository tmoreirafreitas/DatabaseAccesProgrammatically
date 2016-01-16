using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SVD.Model;

namespace AdoNetDemo
{
    public class ItemLocacaoRepositorio : RepositorioBase<ItemLocacaoRepositorio>, IRepositorio<ItemLocacao>
    {
        //[idlocacao]
        //[idcopia]
        //[valor_locacao]

        private int _idlocacao;
        private int _idcopia;
        private int _valor_locacao;
        private LocacaoRepositorio locacaoRepositorio { get { return new LocacaoRepositorio(); } }
        private CopiaRepositorio copiaRepositorio { get { return new CopiaRepositorio(); } }

        public int Insert(ItemLocacao item)
        {
            try
            {
                string sql = @"INSERT INTO [dbo].[Item_Locacao]
           ([idlocacao]
           ,[idcopia]
           ,[valor_locacao])
     VALUES
           (@idlocacao
           ,@idcopia
           ,@valor_locacao);CAST(SCOPE_IDENTITY() AS INT);";

                var parametros = new Dictionary<string, object>();
                parametros.Add("@idlocacao", item.Locacao.ID);
                parametros.Add("@idcopia", item.Copia.ID);
                parametros.Add("@valor_locacao", item.ValorLocacao);

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
                string sql = @"DELETE FROM [dbo].[Item_Locacao] WHERE ID = @ID";
                var parametro = new Dictionary<string, object>();
                parametro.Add("@ID", item.ID);
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
                string sql = @"DELETE FROM [dbo].[Item_Locacao] WHERE idcopia = @idcopia";
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
                string sql = @"DELETE FROM [dbo].[Item_Locacao] WHERE idlocacao = @idlocacao";
                var parametro = new Dictionary<string, object>();
                parametro.Add("@idlocacao", locacao.ID);
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
                string sql = @"UPDATE [dbo].[Item_Locacao]
   SET [idlocacao] = @idlocacao
      ,[idcopia] = @idcopia
      ,[valor_locacao] = @valor_locacao
 WHERE ID = @ID";
                var parametro = new Dictionary<string, object>();
                parametro.Add("@ID", item.ID);
                parametro.Add("@idlocacao", item.Locacao.ID);
                parametro.Add("@idcopia", item.Copia.ID);
                parametro.Add("@valor_locacao", item.ValorLocacao);
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
                string sql = @"SELECT [idlocacao]
      ,[idcopia]
      ,[valor_locacao]
  FROM [dbo].[Item_Locacao] WHERE ID = @ID";
                var parametro = new Dictionary<string, object>();
                parametro.Add("@ID", id);
                return Populate(ExecuteReader(sql, parametro));
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
                string sql = @"SELECT [idlocacao]
      ,[idcopia]
      ,[valor_locacao]
  FROM [dbo].[Item_Locacao] WHERE [idcopia] = @idcopia AND [idlocacao] = @idlocacao";
                var parametro = new Dictionary<string, object>();
                parametro.Add("@idcopia", idCopia);
                parametro.Add("@idlocacao", idLocacao);
                return Populate(ExecuteReader(sql, parametro));
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
                string sql = @"SELECT [idlocacao]
      ,[idcopia]
      ,[valor_locacao]
  FROM [dbo].[Item_Locacao]";
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

        protected override ItemLocacao Populate(System.Data.IDataReader dataReader)
        {
            _idlocacao = dataReader.GetOrdinal("idlocacao");
            _idcopia = dataReader.GetOrdinal("idcopia");
            _valor_locacao = dataReader.GetOrdinal("valor_locacao");

            if (dataReader != null || !dataReader.IsClosed)
            {
                ItemLocacao item = new ItemLocacao();

                if (!dataReader.IsDBNull(_idcopia))
                    item.Copia = copiaRepositorio.GetBy(dataReader.GetInt32(_idcopia));

                if (!dataReader.IsDBNull(_idlocacao))
                    item.Locacao = locacaoRepositorio.GetBy(dataReader.GetInt32(_idlocacao));

                if (!dataReader.IsDBNull(_valor_locacao))
                    item.ValorLocacao = dataReader.GetDecimal(_valor_locacao);

                return item;
            }

            throw new ArgumentNullException("Objeto DataReader não foi inicializado ou está fechado...");
        }
    }
}
