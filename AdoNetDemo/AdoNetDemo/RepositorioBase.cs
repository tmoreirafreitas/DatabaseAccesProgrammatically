using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace AdoNetDemo
{
    /// <summary>
    /// Classe responsável pelos comandos genéricos de querys a uma base de dados
    /// </summary>
    public abstract class RepositorioBase<T> where T : class
    {
        /// <summary>
        /// Este método executa querys do tipo SELECT no banco de dados.
        /// Tem um parâmetro obrigatório que é uma "query" de consulta ao banco de dados e a mesma é do tipo "String". 
        /// Tem como parâmetros opcionais o "parameterColun" onde o desenvolvedor passa o array de string com os nomes das colunas
        /// da tabela como parâmetro.
        /// e o parâmetro "values" é um array com os valores a serem consultados na tabela.
        /// </summary>
        /// <param name="query">query</param>
        /// <param name="columnsAndValues">columnsAndValues = null</param>
        /// <returns>dataReader</returns>
        protected SqlDataReader ExecuteReader(string query, Dictionary<string, object> columnsAndValues = null)
        {
            try
            {
                var command = new SqlCommand(query, ConnectionFactory.CreateConnection());
                if (columnsAndValues != null)
                    foreach (var item in columnsAndValues)
                        command.Parameters.AddWithValue(item.Key, item.Value);

                var dataReader = command.ExecuteReader();

                return dataReader;
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        /// <summary>
        /// Método que executa para execução dos comandos Insert, Update e Delete
        /// O primeiro argumento é uma quary sql. Exemplo: "INSERT INTO Category(Id, CategoryName) VALUES (@Id, @CategoryName)"
        /// O Segundo argumento opcional é um dicionário, temos uma par de string e um object, onde o primeiro é o parâmetro
        /// da coluna e o segundo o seu respectivo valor. Exemplo: columnsAndValues.Add("@Id", element.Id)
        /// </summary>
        /// <param name="query">query</param>
        /// <param name="columnsAndValues">columnsAndValues</param>
        /// <returns>result</returns>
        protected int ExecuteCommand(string query, Dictionary<string, object> columnsAndValues = null)
        {
            try
            {
                int result;
                using (var command = new SqlCommand(query, ConnectionFactory.CreateConnection()))
                {
                    if (columnsAndValues != null)
                        foreach (var item in columnsAndValues)
                            command.Parameters.AddWithValue(item.Key, item.Value);

                    result = Convert.ToInt32(command.ExecuteScalar());
                }
                return result;
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
            finally
            {
                ConnectionFactory.Fechar();
            }
        }

        /// <summary>
        /// Método que pega o próximo Id de uma entidade que será inserida na tabela.
        /// </summary>
        /// <param name="tableName">tableName</param>
        /// <returns>Id</returns>
        protected int GetNextId(string tableName)
        {
            var result = 0;

            try
            {
                using (SqlDataReader dataReader = ExecuteReader("SELECT MAX(Id) FROM [dbo]." + "[" + tableName + "]"))
                {
                    if (dataReader.Read())
                    {
                        result = dataReader.IsDBNull(0) ? 0 : dataReader.GetInt32(0);
                    }

                    if (result == 0)
                        return 1;
                    return (result + 1);
                }
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        protected abstract T Populate(SqlDataReader dataReader);
    }
}
