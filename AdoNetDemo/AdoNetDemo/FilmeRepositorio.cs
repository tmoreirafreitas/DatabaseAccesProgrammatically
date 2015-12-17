using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SVD.Model;

namespace AdoNetDemo
{
    public class FilmeRepositorio : RepositorioBase, IRepositorio<Filme>
    {
        //id	        int
        //idgenero	    int
        //idcategoria	int
        //titulo	    varchar
        //duracao	    varchar

        private CategoriaRepositorio categoriaRepositorio;
        private GeneroRepositorio generoRepositorio;
        private int _id;
        private int _idgenero;
        private int _idcategoria;
        private int _titulo;
        private int _duracao;

        public int Insert(Filme item)
        {
            try
            {
                categoriaRepositorio = new CategoriaRepositorio();
                generoRepositorio = new GeneroRepositorio();

                item.ID = GetNextId("Filme");
                var generos = generoRepositorio.GetAll();
                var categorias = categoriaRepositorio.GetAll();

                item.Genero = (from genero in generos
                               where genero.Descricao.ToLowerInvariant() == item.Genero.Descricao.ToLowerInvariant()
                               select (new Genero
                               {
                                   ID = genero.ID,
                                   Descricao = genero.Descricao
                               })).FirstOrDefault();

                item.Categoria = (from categoria in categorias
                                  where categoria.Descricao.ToLowerInvariant() == item.Categoria.Descricao.ToLowerInvariant()
                                  select (new Categoria
                                  {
                                      ID = categoria.ID,
                                      Descricao = categoria.Descricao,
                                      ValorLocacao = categoria.ValorLocacao
                                  })).FirstOrDefault();

                categoriaRepositorio = null;
                generoRepositorio = null;

                string sql = @"INSERT INTO [dbo].[Filme] ([id] ,[idgenero] ,[idcategoria] ,[titulo] ,[duracao]) VALUES (@id ,@idgenero ,@idcategoria ,@titulo ,@duracao)";
                var parametros = new Dictionary<string, object>();
                parametros.Add("@id", item.ID);
                parametros.Add("@idgenero", item.Genero.ID);
                parametros.Add("@idcategoria", item.Categoria.ID);
                parametros.Add("@titulo", item.Titulo);
                parametros.Add("@duracao", item.Duracao);
                return ExecuteCommand(sql, parametros);
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public void Remove(Filme item)
        {
            try
            {
                string sql = @"DELETE FROM [dbo].[Filme] WHERE titulo = @titulo";
                var parametros = new Dictionary<string, object>();
                parametros.Add("@titulo", item.Titulo);
                ExecuteCommand(sql, parametros);
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public void Update(Filme item)
        {
            try
            {
                string sql = @"UPDATE [dbo].[Filme] SET [idgenero] = @idgenero ,[idcategoria] = @idcategoria ,[titulo] = @titulo ,[duracao] = @duracao WHERE id = @id";
                var parametros = new Dictionary<string, object>();
                parametros.Add("@id", item.ID);
                parametros.Add("@idgenero", item.Genero.ID);
                parametros.Add("@idcategoria", item.Categoria.ID);
                parametros.Add("@titulo", item.Titulo);
                parametros.Add("@duracao", item.Duracao);
                ExecuteCommand(sql, parametros);
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public Filme GetBy(int id)
        {
            try
            {
                string sql = @"SELECT [id] ,[idgenero] ,[idcategoria] ,[titulo] ,[duracao] FROM [SVDB].[dbo].[Filme] WHERE id = @id";
                var parametros = new Dictionary<string, object>();
                parametros.Add("@id", id);

                return Populate(ExecuteQuery(sql, parametros));
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public Filme GetBy(string titulo)
        {
            try
            {
                string sql = @"SELECT [id] ,[idgenero] ,[idcategoria] ,[titulo] ,[duracao] FROM [SVDB].[dbo].[Filme] WHERE titulo = @titulo";
                var parametros = new Dictionary<string, object>();
                parametros.Add("@titulo", titulo);

                return Populate(ExecuteQuery(sql, parametros));
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public List<Filme> GetFilmes(string titulo)
        {
            var filmes = new List<Filme>();
            try
            {
                string sql = @"SELECT [id] ,[idgenero] ,[idcategoria] ,[titulo] ,[duracao] FROM [SVDB].[dbo].[Filme] WHERE titulo LIKE '%' + @titulo + '%'";
                var parametros = new Dictionary<string, object>();
                parametros.Add("@titulo", titulo);
                var dataReader = ExecuteQuery(sql, parametros);

                while (dataReader.Read())
                    filmes.Add(Populate(dataReader));

                return filmes;
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public List<Filme> GetByCategoria(string descricao)
        {
            categoriaRepositorio = new CategoriaRepositorio();
            var categoria = categoriaRepositorio.GetBy(descricao);
            var filmes = new List<Filme>();
            categoriaRepositorio = null;

            try
            {
                string sql = @"SELECT [id] ,[idgenero] ,[idcategoria] ,[titulo] ,[duracao] FROM [SVDB].[dbo].[Filme] WHERE idcategoria = @idcategoria";
                var parametros = new Dictionary<string, object>();
                parametros.Add("@idcategoria", categoria.ID);
                var dataReader = ExecuteQuery(sql, parametros);

                while (dataReader.Read())
                    filmes.Add(Populate(dataReader));

                return filmes;
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public List<Filme> GetByGenero(string descricao)
        {
            generoRepositorio = new GeneroRepositorio();
            var genero = generoRepositorio.GetBy(descricao);
            var filmes = new List<Filme>();
            generoRepositorio = null;

            try
            {
                string sql = @"SELECT [id] ,[idgenero] ,[idcategoria] ,[titulo] ,[duracao] FROM [SVDB].[dbo].[Filme] WHERE idgenero = @idgenero";
                var parametros = new Dictionary<string, object>();
                parametros.Add("@idgenero", genero.ID);
                var dataReader = ExecuteQuery(sql, parametros);

                while (dataReader.Read())
                    filmes.Add(Populate(dataReader));

                return filmes;
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public List<Filme> GetByGeneroAndCategoria(Genero genero, Categoria categoria)
        {
            generoRepositorio = new GeneroRepositorio();
            categoriaRepositorio = new CategoriaRepositorio();
            var filmes = new List<Filme>();

            try
            {
                if (genero != null)
                    if (!string.IsNullOrEmpty(genero.Descricao))
                        genero = generoRepositorio.GetBy(genero.Descricao);

                if (categoria != null)
                    if (!string.IsNullOrEmpty(categoria.Descricao))
                        categoria = categoriaRepositorio.GetBy(categoria.Descricao);

                categoriaRepositorio = null;
                generoRepositorio = null;

                string sql = @"SELECT [id] ,[idgenero] ,[idcategoria] ,[titulo] ,[duracao] FROM [SVDB].[dbo].[Filme] WHERE idgenero = @idgenero AND idcategoria = @idcategoria";
                var parametros = new Dictionary<string, object>();
                parametros.Add("@idgenero", genero.ID);
                parametros.Add("@idcategoria", categoria.ID);
                var dataReader = ExecuteQuery(sql, parametros);

                while (dataReader.Read())
                    filmes.Add(Populate(dataReader));

                return filmes;
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public List<Filme> GetAll()
        {
            var filmes = new List<Filme>();
            try
            {
                string sql = @"SELECT [id] ,[idgenero] ,[idcategoria] ,[titulo] ,[duracao] FROM [SVDB].[dbo].[Filme]";
                var dataReader = ExecuteQuery(sql);

                while (dataReader.Read())
                    filmes.Add(Populate(dataReader));

                return filmes;
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public Filme Populate(System.Data.IDataReader dataReader)
        {
            if (dataReader != null)
            {
                _id = dataReader.GetOrdinal("id");
                _idgenero = dataReader.GetOrdinal("idgenero");
                _idcategoria = dataReader.GetOrdinal("idcategoria");
                _titulo = dataReader.GetOrdinal("titulo");
                _duracao = dataReader.GetOrdinal("duracao");

                categoriaRepositorio = new CategoriaRepositorio();
                generoRepositorio = new GeneroRepositorio();

                var filme = new Filme();

                if (!dataReader.IsDBNull(_id))
                    filme.ID = dataReader.GetInt32(_id);

                if (!dataReader.IsDBNull(_idgenero))
                    filme.Genero = generoRepositorio.GetBy(dataReader.GetInt32(_idgenero));

                if (!dataReader.IsDBNull(_idcategoria))
                    filme.Categoria = categoriaRepositorio.GetBy(dataReader.GetInt32(_idcategoria));

                if (!dataReader.IsDBNull(_titulo))
                    filme.Titulo = dataReader.GetString(_titulo);

                if (!dataReader.IsDBNull(_duracao))
                    filme.Duracao = dataReader.GetString(_duracao);

                categoriaRepositorio = null;
                generoRepositorio = null;

                return filme;
            }

            return null;
        }
    }
}
