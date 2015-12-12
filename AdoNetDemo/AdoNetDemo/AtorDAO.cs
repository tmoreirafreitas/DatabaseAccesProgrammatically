﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SVD.Model;

namespace AdoNetDemo
{
    public class AtorDAO : GenericDAO, IGenericDAO<Ator>
    {
        public int Insert(Ator item)
        {
            string sql = @"INSERT INTO [dbo].[Ator]([Id],[nome]) VALUES (@id, @nome)";

            try
            {
                item.ID = GetNextId("Ator");
                var parametros = new Dictionary<string, object>();
                parametros.Add("@id", item.ID);
                parametros.Add("@nome", item.Nome);

                return ExecuteCommand(sql, parametros);
            }

            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public bool Remove(Ator item)
        {
            string sql = @"DELETE FROM [dbo].[Ator] WHERE id = @id";

            try
            {
                var parametros = new Dictionary<string, object>();
                parametros.Add("@id", item.ID);
                ExecuteCommand(sql, parametros);
                return true;
            }

            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public bool Update(Ator item)
        {
            string sql = @"UPDATE [dbo].[Ator] SET [nome] = @nome WHERE id = @id";

            try
            {
                var parametros = new Dictionary<string, object>();
                parametros.Add("@id", item.ID);
                parametros.Add("@nome", item.Nome);
                ExecuteCommand(sql, parametros);
                return true;
            }

            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public Ator GetBy(int id)
        {
            string sql = @"SELECT [id],[nome] FROM [dbo].[Ator] WHERE id = @id";

            try
            {
                var parametros = new Dictionary<string, object>();
                parametros.Add("@id", id);
                return Populate(ExecuteQuery(sql, parametros));
            }

            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public Ator GetBy(string nome)
        {
            string sql = @"SELECT [id],[nome] FROM [dbo].[Ator] WHERE nome = @nome";

            try
            {
                var parametros = new Dictionary<string, object>();
                parametros.Add("@nome", nome);
                return Populate(ExecuteQuery(sql, parametros));
            }

            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public List<Ator> GetAll()
        {
            var items = new List<Ator>();
            string sql = @"SELECT [id],[nome] FROM [dbo].[Ator]";

            try
            {
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

        public Ator Populate(System.Data.IDataReader dataReader)
        {
            var item = new Ator();

            if (dataReader.Read())
            {
                if (!dataReader.IsDBNull(0))
                    item.ID = dataReader.GetInt32(0);

                if (!dataReader.IsDBNull(1))
                    item.Nome = dataReader.GetString(1);
            }

            return item;
        }
    }
}