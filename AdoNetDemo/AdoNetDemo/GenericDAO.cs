using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetDemo
{
    /// <summary>
    /// Classe responsável pelos comandos genéricos de querys a uma base de dados
    /// </summary>
    public abstract class GenericDAO
    {
        /// <summary>
        /// Este método executa querys do tipo SELECT no banco de dados.
        /// Tem um parâmetro obrigatório que é uma "query" de consulta ao banco de dados e a mesma é do tipo "String". 
        /// Tem como parâmetros opcionais o "parameterColun" onde o desenvolvedor passa o array de string com os nomes das colunas
        /// da tabela como parâmetro.
        /// e o parâmetro "values" é um array com os valores a serem consultados na tabela.
        /// </summary>
        /// <param name="query">query</param>
        /// <param name="ColumnsAndValues">Dictionary<string, object> ColumnsAndValues = null</param>
        /// <returns>dataReader</returns>
        protected IDataReader ExecuteQuery(string query, Dictionary<string, object> ColumnsAndValues = null)
        {
            IDataReader dataReader = null;

            try
            {
                using (SqlCommand command = new SqlCommand())
                {
                    using (command.Connection = ConnectionFactory.CreateConnection())
                    {
                        command.CommandText = query;
                        command.Connection.Open();
                        if (ColumnsAndValues != null)
                            foreach (var item in ColumnsAndValues)
                                command.Parameters.AddWithValue(item.Key, item.Value);

                        dataReader = command.ExecuteReader();
                    }
                }

                return dataReader;
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        /// <summary>
        /// Método que executa para execução dos comandos Insert, Update e Delete
        /// O primeiro argumento é uma quary sql. Exemplo: "INSERT INTO Category(Id, CategoryName) VALUES (@ID, @CategoryName)"
        /// O Segundo argumento opcional é um dicionário, temos uma par de string e um object, onde o primeiro é o parâmetro
        /// da coluna e o segundo o seu respectivo valor. Exemplo: ColumnsAndValues.Add("@ID", element.ID)
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ColumnsAndValues">Dictionary<string, object> ColumnsAndValues = null</param>
        /// <returns></returns>
        protected int ExecuteCommand(string query, Dictionary<string, object> ColumnsAndValues = null)
        {
            int result = 0;
            try
            {                
                using (SqlCommand command = new SqlCommand())
                {
                    using (command.Connection = ConnectionFactory.CreateConnection())
                    {
                        command.CommandText = query;
                        command.Connection.Open();
                        if (ColumnsAndValues != null)
                            foreach (var item in ColumnsAndValues)
                                command.Parameters.AddWithValue(item.Key, item.Value);

                        result = command.ExecuteNonQuery();
                    }
                }
                return result;
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        /// <summary>
        /// Método que pega o próximo ID de uma entidade que será inserida na tabela.
        /// </summary>
        /// <param name="tableName">tableName</param>
        /// <returns>ID</returns>
        protected int GetNextId(string tableName)
        {
            int result = 0;
            int _id;

            try
            {
                using (IDataReader dataReader = ExecuteQuery("SELECT MAX(ID) FROM [dbo]." + "[" + tableName + "]"))
                {
                    _id = dataReader.GetOrdinal("id");

                    if (dataReader.Read())
                    {
                        if (dataReader.IsDBNull(_id))
                            result = 0;
                        else
                            result = dataReader.GetInt32(_id);
                    }                    
                }

                if (result == 0)
                    return 1;
                else
                    return (result + 1);
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }
    }
}
