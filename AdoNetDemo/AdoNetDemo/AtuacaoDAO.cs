﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SVD.Model;

namespace AdoNetDemo
{
    public class AtuacaoDAO : RepositorioBase, IGenericDAO<Atuacao>
    {
        //[idator]      int
        //[idfilme]     int
        //[papel]       varchar

        private FilmeDAO filmeDAO;
        private AtorDAO atorDAO;
        private int _idAtor;
        private int _idFilme;
        private int _papel;

        public int Insert(Atuacao item)
        {
            try
            {
                item.ID = GetNextId("Atua");
                string sql = @"INSERT INTO [dbo].[Atua] ([idator] ,[idfilme] ,[papel]) VALUES (@idator ,@idfilme ,@papel)";
                var parametros = new Dictionary<string, object>();
                parametros.Add("@idator", item.Ator.ID);
                parametros.Add("@idfilme", item.Filme.ID);
                parametros.Add("@papel", item.Papel);

                return ExecuteCommand(sql, parametros);
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public bool Remove(Atuacao item)
        {
            try
            {
                string sql = @"DELETE FROM [dbo].[Atua] WHERE idator = @idator AND idfilme = @idfilme";
                var parametros = new Dictionary<string, object>();
                parametros.Add("@idator", item.Ator.ID);
                parametros.Add("@idfilme", item.Filme.ID);
                ExecuteCommand(sql, parametros);

                return true;
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public bool Update(Atuacao item)
        {
            try
            {
                string sql = @"UPDATE [dbo].[Atua] SET [papel] = @papel WHERE idator = @idator AND idfilme = @idfilme";
                var parametros = new Dictionary<string, object>();
                parametros.Add("@papel", item.Papel);
                parametros.Add("@idator", item.Ator.ID);
                parametros.Add("@idfilme", item.Filme.ID);
                ExecuteCommand(sql, parametros);

                return true;
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public Atuacao GetBy(int id)
        {
            throw new NotImplementedException("Funcionalidade não válida para esta classe, use o método: GetBy(int idator, int idfilme)");
        }

        public Atuacao GetBy(int idator, int idfilme)
        {
            try
            {
                string sql = @"SELECT [idator] ,[idfilme] ,[papel] FROM [dbo].[Atua] WHERE idator = @idator AND idfilme = @idfilme";
                var parametros = new Dictionary<string, object>();
                parametros.Add("@idator", idator);
                parametros.Add("@idfilme", idfilme);

                return Populate(ExecuteQuery(sql, parametros));
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public List<Atuacao> GetAll()
        {
            try
            {
                var atuacoes = new List<Atuacao>();
                string sql = @"SELECT [idator] ,[idfilme] ,[papel] FROM [dbo].[Atua]";
                var dataReader = ExecuteQuery(sql);

                while (dataReader.Read())
                    atuacoes.Add(Populate(dataReader));

                return atuacoes;
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public Atuacao Populate(System.Data.IDataReader dataReader)
        {
            if (dataReader != null)
            {
                _idAtor = dataReader.GetOrdinal("idator");
                _idFilme = dataReader.GetOrdinal("idfilme");
                _papel = dataReader.GetOrdinal("papel");

                filmeDAO = new FilmeDAO();
                atorDAO = new AtorDAO();

                Atuacao atuacao = new Atuacao
                {
                    Filme = filmeDAO.GetBy(dataReader.GetInt32(_idFilme)),
                    Ator = atorDAO.GetBy(dataReader.GetInt32(_idAtor)),
                    Papel = dataReader.GetString(_papel)
                };

                filmeDAO = null;
                atorDAO = null;

                return atuacao;
            }

            throw new ArgumentNullException("Objeto DataReader não instanciado...");
        }
    }
}
