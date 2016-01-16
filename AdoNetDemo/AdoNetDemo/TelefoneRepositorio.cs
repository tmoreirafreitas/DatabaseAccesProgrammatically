using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SVD.Model;

namespace AdoNetDemo
{
    public class TelefoneRepositorio : RepositorioBase<Telefone>, IRepositorio<Telefone>
    {
        //id	    bigint
        //idsocio	bigint
        //ddd	    char
        //numero	char

        private int _id;
        private int _idsocio;
        private int _ddd;
        private int _numero;
        private SocioRepositorio socioRepositorio { get { return new SocioRepositorio(); } }

        public int Insert(Telefone item)
        {
            try
            {
                if (item.Socio != null)
                {
                    if (item.Socio.ID == null)
                    {
                        if (!string.IsNullOrEmpty(item.Socio.CPF.ToString()))
                            item.Socio = socioRepositorio.GetByCPF(item.Socio.CPF);
                        else if (!string.IsNullOrEmpty(item.Socio.RG.ToString()))
                            item.Socio = socioRepositorio.GetByRG(item.Socio.RG);
                        else if (!string.IsNullOrEmpty(item.Socio.Email))
                            item.Socio = socioRepositorio.GetBy(item.Socio.Email);
                    }
                }

                string sql = @"INSERT INTO [dbo].[Telefone]
           ([idsocio]
           ,[ddd]
           ,[numero])
     VALUES
           (@idsocio
           ,@ddd
           ,@numero);SELECT CAST(SCOPE_IDENTITY() AS INT);";

                var parametros = new Dictionary<string, object>();
                parametros.Add("@idsocio", item.Socio.ID);
                parametros.Add("@ddd", item.DDD);
                parametros.Add("@numero", item.Numero);

                return ExecuteCommand(sql, parametros);
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public void Remove(Telefone item)
        {
            try
            {
                string sql = @"DELETE FROM [dbo].[Telefone] WHERE ddd = @ddd AND numero = @numero";
                var parametros = new Dictionary<string, object>();
                parametros.Add("@ddd", item.DDD);
                parametros.Add("@numero", item.Numero);
                ExecuteCommand(sql, parametros);
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public void RemoveAllBy(Socio socio)
        {
            try
            {
                string sql = @"DELETE FROM [dbo].[Telefone] WHERE idsocio = @idsocio";
                var parametros = new Dictionary<string, object>();
                parametros.Add("@idsocio", socio.ID);
                ExecuteCommand(sql, parametros);
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public void Update(Telefone item)
        {
            try
            {
                string sql = @"UPDATE [dbo].[Telefone] SET [ddd] = @ddd ,[numero] = @numero WHERE idsocio = @idsocio";
                var parametros = new Dictionary<string, object>();
                parametros.Add("@idsocio", item.Socio.ID);
                parametros.Add("@numero", item.Numero);
                parametros.Add("@ddd", item.DDD);

                ExecuteCommand(sql, parametros);
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public Telefone GetBy(int id)
        {
            throw new NotImplementedException();
        }

        public List<Telefone> GetAll()
        {
            try
            {
                var enderecos = new List<Telefone>();
                string sql = @"SELECT [id] ,[idsocio] ,[ddd] ,[numero] FROM [SVDB].[dbo].[Telefone]";
                var dataReader = ExecuteReader(sql);
                while (dataReader.Read())
                    enderecos.Add(Populate(dataReader));

                return enderecos;
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public List<Telefone> GetAllBy(Socio socio)
        {
            try
            {
                if (socio != null)
                {
                    if (!string.IsNullOrEmpty(socio.CPF.ToString()))
                        socio = socioRepositorio.GetByCPF(socio.CPF);
                    else if (!string.IsNullOrEmpty(socio.RG.ToString()))
                        socio = socioRepositorio.GetByRG(socio.RG);
                    else if (!string.IsNullOrEmpty(socio.Email))
                        socio = socioRepositorio.GetBy(socio.Email);
                }

                var telefones = new List<Telefone>();
                string sql = @"SELECT [id] ,[idsocio] ,[ddd] ,[numero] FROM [SVDB].[dbo].[Telefone] WHERE idsocio = @idsocio";
                var parametros = new Dictionary<string, object>();
                parametros.Add("@idsocio", socio.ID);
                var dataReader = ExecuteReader(sql, parametros);
                socio = socioRepositorio.GetBy(socio.ID);
                while (dataReader.Read())
                {
                    var tel = Populate(dataReader);
                    tel.Socio = socio;
                    telefones.Add(tel);
                }

                return telefones;
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        protected override Telefone Populate(System.Data.IDataReader dataReader)
        {
            if (dataReader != null || !dataReader.IsClosed)
            {
                _id = dataReader.GetOrdinal("id");
                _idsocio = dataReader.GetOrdinal("idsocio");
                _numero = dataReader.GetOrdinal("numero");
                _ddd = dataReader.GetOrdinal("ddd");

                Telefone telefone = new Telefone();
                telefone.DDD = dataReader.GetString(_ddd);
                telefone.Numero = dataReader.GetString(_numero);

                return telefone;
            }

            throw new ArgumentNullException("Objeto DataReader não foi inicializado ou está fechado...");
        }
    }
}
