﻿using System;
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
            string sql = @"INSERT INTO [dbo].[Categoria]([id],[descricao],[valor_locacao]) VALUES (@id, @descricao, @valor_locacao)";

            try
            {
                item.ID = GetNextId("Categoria");
                var parametros = new Dictionary<string, object>();
                parametros.Add("@id", item.ID);
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
                string sql = @"DELETE FROM [dbo].[Categoria] WHERE ID = @ID OR descricao = @descricao";
                var parametros = new Dictionary<string, object>();

                if (item.ID != null)
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
                string sql = @"UPDATE [dbo].[Categoria] SET [descricao] = @descricao, [valor_locacao] = @valor_locacao WHERE ID = @ID";
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
                string sql = @"SELECT [id],[descricao],[valor_locacao] FROM [dbo].[Categoria] WHERE ID = @ID";
                var parametros = new Dictionary<string, object>();
                parametros.Add("@ID", id);

                return Populate(ExecuteQuery(sql, parametros));
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
                string sql = @"SELECT [id],[descricao],[valor_locacao] FROM [dbo].[Categoria] WHERE descricao = @descricao";
                var parametros = new Dictionary<string, object>();
                parametros.Add("@descricao", descricao);

                return Populate(ExecuteQuery(sql, parametros));
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
                string sql = @"SELECT [id],[descricao],[valor_locacao] FROM [dbo].[Categoria]";
                var items = new List<Categoria>();
                var dataReader = ExecuteQuery(sql);

                while (dataReader.Read())
                    items.Add(Populate(dataReader));

                return items;
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        protected override Categoria Populate(System.Data.IDataReader dataReader)
        {
            Categoria item = new Categoria();
            if (dataReader.Read())
            {
                if (!dataReader.IsDBNull(0))
                    item.ID = dataReader.GetInt32(0);

                if (!dataReader.IsDBNull(1))
                    item.Descricao = dataReader.GetString(1);

                if (!dataReader.IsDBNull(2))
                    item.ValorLocacao = dataReader.GetDecimal(2);
            }
            return item;
        }
    }
}
