using System;
using System.Collections.Generic;
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
        private static SocioRepositorio SocioRepositorio { get { return new SocioRepositorio(); } }

        public int Insert(Telefone item)
        {
            try
            {
                if (item.Socio != null)
                {
                    if (item.Socio.Id == 0)
                    {
                        if (!string.IsNullOrEmpty(item.Socio.Cpf))
                            item.Socio = SocioRepositorio.GetByCpf(item.Socio.Cpf);
                        else if (!string.IsNullOrEmpty(item.Socio.Rg))
                            item.Socio = SocioRepositorio.GetByRg(item.Socio.Rg);
                        else if (!string.IsNullOrEmpty(item.Socio.Email))
                            item.Socio = SocioRepositorio.GetBy(item.Socio.Email);
                    }
                }

                const string sql = @"INSERT INTO [dbo].[Telefone]
           ([idsocio]
           ,[ddd]
           ,[numero])
     VALUES
           (@idsocio
           ,@ddd
           ,@numero);SELECT CAST(SCOPE_IDENTITY() AS INT);";

                var parametros = new Dictionary<string, object>
                    {
                        {"@ddd", item.DDD},
                        {"@numero", item.Numero}
                    };
                if (item.Socio != null) parametros.Add("@idsocio", item.Socio.Id);
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
                const string sql = @"DELETE FROM [dbo].[Telefone] WHERE ddd = @ddd AND numero = @numero";
                var parametros = new Dictionary<string, object> { { "@ddd", item.DDD }, { "@numero", item.Numero } };
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
                const string sql = @"DELETE FROM [dbo].[Telefone] WHERE idsocio = @idsocio";
                var parametros = new Dictionary<string, object> { { "@idsocio", socio.Id } };
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
                const string sql = @"UPDATE [dbo].[Telefone] SET [ddd] = @ddd ,[numero] = @numero WHERE idsocio = @idsocio";
                var parametros = new Dictionary<string, object>
                {
                    {"@idsocio", item.Socio.Id},
                    {"@numero", item.Numero},
                    {"@ddd", item.DDD}
                };

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
                const string sql = @"SELECT [id] ,[idsocio] ,[ddd] ,[numero] FROM [SVDB].[dbo].[Telefone]";
                var dataReader = ExecuteReader(sql);
                while (dataReader.Read())
                    enderecos.Add(Populate(dataReader));

                dataReader.Close();
                dataReader.Dispose();
                ConnectionFactory.Fechar();

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
                    if (!string.IsNullOrEmpty(socio.Cpf))
                        socio = SocioRepositorio.GetByCpf(socio.Cpf);
                    else if (!string.IsNullOrEmpty(socio.Rg))
                        socio = SocioRepositorio.GetByRg(socio.Rg);
                    else if (!string.IsNullOrEmpty(socio.Email))
                        socio = SocioRepositorio.GetBy(socio.Email);
                }

                var telefones = new List<Telefone>();
                const string sql = @"SELECT [id] ,[idsocio] ,[ddd] ,[numero] FROM [SVDB].[dbo].[Telefone] WHERE idsocio = @idsocio";
                if (socio != null)
                {
                    var parametros = new Dictionary<string, object> { { "@idsocio", socio.Id } };
                    var dataReader = ExecuteReader(sql, parametros);
                    socio = SocioRepositorio.GetBy(socio.Id);
                    while (dataReader.Read())
                    {
                        var tel = Populate(dataReader);
                        tel.Socio = socio;
                        telefones.Add(tel);
                    }

                    dataReader.Close();
                    dataReader.Dispose();
                }
                ConnectionFactory.Fechar();

                return telefones;
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        protected override Telefone Populate(System.Data.SqlClient.SqlDataReader dataReader)
        {
            const string msg = @"Objeto DataReader não foi inicializado ou está fechado...";

            if (dataReader == null || dataReader.IsClosed)
                throw new ArgumentNullException(msg);

            _id = dataReader.GetOrdinal("id");
            _idsocio = dataReader.GetOrdinal("idsocio");
            _numero = dataReader.GetOrdinal("numero");
            _ddd = dataReader.GetOrdinal("ddd");

            var telefone = new Telefone();

            if (dataReader.IsDBNull(_id))
                telefone.ID = dataReader.GetInt32(_id);

            if (!dataReader.IsDBNull(_ddd))
                telefone.DDD = dataReader.GetString(_ddd);

            if (!dataReader.IsDBNull(_numero))
                telefone.Numero = dataReader.GetString(_numero);

            if (!dataReader.IsDBNull(_idsocio))
                telefone.Socio = SocioRepositorio.GetBy(_idsocio);

            return telefone;
        }
    }
}
