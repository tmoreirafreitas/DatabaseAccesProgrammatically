using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SVD.Model;

namespace AdoNetDemo
{
    public class FilmeDAO : GenericDAO, IGenericDAO<Filme>
    {
        private CategoriaDAO categoriaDAO;
        private GeneroDAO generoDAO;

        public FilmeDAO()
        {
            categoriaDAO = new CategoriaDAO();
            generoDAO = new GeneroDAO();
        }

        public int Insert(Filme item)
        {
            try
            {
                item.ID = GetNextId("Filme");
                var generos = generoDAO.GetAll();
                var categorias = categoriaDAO.GetAll();

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

                string sql = @"INSERT INTO [dbo].[Filme] ([id] ,[idgenero] ,[idcategoria] ,[titulo] ,[duracao]) VALUES (@id ,@idgenero ,@idcategoria ,@titulo ,@duracao)";
                var parametros = new Dictionary<string, object>();
                parametros.Add("@id", item.ID);
                parametros.Add("@idgenero", item.Genero.ID);
                parametros.Add("@idcategoria", item.Categoria.ID);
                parametros.Add("@titulo", item.Titulo);
                parametros.Add("@duracao", item.Duracao);
                return ExecuteCommand(sql, parametros);
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public bool Remove(Filme item)
        {
            throw new NotImplementedException();
        }

        public bool Update(Filme item)
        {
            throw new NotImplementedException();
        }

        public Filme GetBy(int id)
        {
            throw new NotImplementedException();
        }

        public Filme GetBy(string titulo)
        {
            throw new NotImplementedException();
        }

        public List<Filme> GetAll()
        {
            throw new NotImplementedException();
        }

        public Filme Populate(System.Data.IDataReader dataReader)
        {
            throw new NotImplementedException();
        }
    }
}
