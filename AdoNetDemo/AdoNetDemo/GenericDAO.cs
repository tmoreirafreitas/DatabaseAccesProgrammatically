using System;
using System.Collections.Generic;
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
        /// <param name="query">string query</param>
        /// <param name="namesOfColuns">string[] namesOfColuns = null</param>
        /// <param name="valuesOfColuns">object[] valuesOfColuns = null</param>
        /// <returns></returns>
        public SqlDataReader ExecuteQuery(string query, string[] namesOfColuns = null, object[] valuesOfColuns = null)
        {
            SqlDataReader sdr = null;

            try
            {
                using (SqlCommand command = new SqlCommand())
                {
                    using (command.Connection = ConnectionFactory.CreateConnection())
                    {
                        command.CommandText = query;
                        command.Connection.Open();

                        if (namesOfColuns != null && valuesOfColuns != null)
                            for (int i = 0; i < namesOfColuns.Length; i++)
                                command.Parameters.AddWithValue(namesOfColuns[i], valuesOfColuns[i]);

                        sdr = command.ExecuteReader();
                    }
                }

                return sdr;
            }
            catch (SystemException myException)
            {
                throw new SystemException(myException.Message);
            }
        }

        /// <summary>
        /// Método que executa para execução dos comandos Insert, Update e Delete
        /// </summary>
        /// <param name="query">string query</param>
        /// <param name="namesOfColuns">string[] namesOfColuns = null</param>
        /// <param name="valuesOfColuns">object[] valuesOfColuns = null</param>
        /// <returns></returns>
        public int ExecuteCommand(string query, string[] namesOfColuns = null, object[] valuesOfColuns = null)
        {
            int result = 0;
            try
            {
                using (SqlCommand command = new SqlCommand())
                {
                    using (command.Connection = ConnectionFactory.CreateConnection())
                    {
                        command.CommandText = query;

                        if (namesOfColuns != null && valuesOfColuns != null)
                            for (int i = 0; i < namesOfColuns.Length; i++)
                                command.Parameters.AddWithValue(namesOfColuns[i], valuesOfColuns[i]);

                        command.Connection.Open();
                        result = command.ExecuteNonQuery();
                        command.Connection.Close();
                    }
                }
                return result;
            }

            catch (SystemException myException)
            {
                throw new SystemException(myException.Message);
            }
        }

        /// <summary>
        /// Método que pega o próximo ID de uma entidade que será inserida na tabela.
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns>ID</returns>
        public int GetNextId(string tableName)
        {
            int result = 0;

            try
            {
                SqlDataReader sdr = ExecuteQuery("SELECT MAX(Id) FROM [dbo]." + "[" + tableName + "]");

                while (sdr.Read())
                {
                    if (sdr.IsDBNull(0))
                        result = 0;
                    else
                        result = sdr.GetInt32(0);
                }

                sdr.Close();

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
