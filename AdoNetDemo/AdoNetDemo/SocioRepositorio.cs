using System;
using System.Collections.Generic;
using SVD.Model;

namespace AdoNetDemo
{
    public class SocioRepositorio : RepositorioBase<Socio>, IRepositorio<Socio>
    {
        //[id]
        //[idEndereco]
        //[nome]
        //[aniversario]
        //[rg]
        //[cpf]
        //[email]

        private int _id;
        private int _idEndereco;
        private int _nome;
        private int _aniversario;
        private int _rg;
        private int _cpf;
        private int _email;
        public EnderecoRepositorio EnderecoRepositorio { get { return new EnderecoRepositorio(); } }
        public TelefoneRepositorio TelefoneRepositorio { get { return new TelefoneRepositorio(); } }
        public LocacaoRepositorio LocacaoRepositorio { get { return new LocacaoRepositorio(); } }

        public int Insert(Socio item)
        {
            try
            {
                if (item.Endereco != null)
                    EnderecoRepositorio.Insert(item.Endereco);

                foreach (var telefone in item.GetTelefones())
                    TelefoneRepositorio.Insert(telefone);

                const string sql = @"INSERT INTO [dbo].[Socio]
           ([idEndereco]
           ,[nome]
           ,[aniversario]
           ,[rg]
           ,[cpf]
           ,[email])
     VALUES
           (@idEndereco
           ,@nome
           ,@aniversario
           ,@rg
           ,@cpf
           ,@email);SELECT CAST(SCOPE_IDENTITY() AS INT);";

                var parametros = new Dictionary<string, object>
                {
                    {"@idEndereco", item.Endereco.ID},
                    {"@nome", item.Nome},
                    {"@aniversario", item.Aniversario},
                    {"@rg", item.Rg},
                    {"@cpf", item.Cpf},
                    {"@email", item.Email}
                };

                return ExecuteCommand(sql, parametros);
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public void Remove(Socio item)
        {
            try
            {
                var sql = string.Empty;
                var parametros = new Dictionary<string, object>();

                if (!string.IsNullOrEmpty(item.Rg))
                {
                    item = GetByRg(item.Rg);
                    sql = @"DELETE FROM [dbo].[Socio] WHERE rg = @rg";
                    parametros.Add("@rg", item.Rg);
                }
                else if (!string.IsNullOrEmpty(item.Cpf))
                {
                    item = GetByCpf(item.Cpf);
                    sql = @"DELETE FROM [dbo].[Socio] WHERE cpf = @cpf";
                    parametros.Add("@cpf", item.Cpf);
                }
                else if (!string.IsNullOrEmpty(item.Email))
                {
                    item = GetBy(item.Email);
                    sql = @"DELETE FROM [dbo].[Socio] WHERE email = @email";
                    parametros.Add("@email", item.Email);
                }

                // Remove todos os telefones do sócio.
                TelefoneRepositorio.RemoveAllBy(item);
                
                // Remove todas as locações do sócio
                LocacaoRepositorio.RemoveAllBy(item);

                ExecuteCommand(sql, parametros);
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public void Update(Socio item)
        {
            var sql = string.Empty;
            var parametros = new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(item.Rg))
            {
                sql = @"UPDATE [dbo].[Socio]
   SET [id] = @id
      ,[idEndereco] = @idEndereco
      ,[nome] = @nome
      ,[aniversario] = @aniversario
      ,[rg] = @rg
      ,[cpf] = @cpf
      ,[email] = @email
 WHERE rg = @rg";
                parametros.Add("@rg", item.Rg);
            }
            else if (!string.IsNullOrEmpty(item.Cpf))
            {
                sql = @"UPDATE [dbo].[Socio]
   SET [id] = @id
      ,[idEndereco] = @idEndereco
      ,[nome] = @nome
      ,[aniversario] = @aniversario
      ,[rg] = @rg
      ,[cpf] = @cpf
      ,[email] = @email
 WHERE cpf = @cpf";
                parametros.Add("@cpf", item.Cpf);
            }

            ExecuteCommand(sql, parametros);
        }

        public Socio GetBy(int id)
        {
            const string sql = @"SELECT [id]
      ,[idEndereco]
      ,[nome]
      ,[aniversario]
      ,[rg]
      ,[cpf]
      ,[email]
  FROM [dbo].[Socio]  WHERE [id] = @id";
            var parametro = new Dictionary<string, object> {{"@id", id}};
            var dataReader = ExecuteReader(sql, parametro);
            var item = Populate(dataReader);

            dataReader.Close();
            dataReader.Dispose();
            ConnectionFactory.Fechar();

            return item;
        }

        public Socio GetBy(string email)
        {
            const string sql = @"SELECT [id]
      ,[idEndereco]
      ,[nome]
      ,[aniversario]
      ,[rg]
      ,[cpf]
      ,[email]
  FROM [dbo].[Socio] WHERE [email] = @email";
            var parametro = new Dictionary<string, object> {{"@email", email}};
            var dataReader = ExecuteReader(sql, parametro);
            var item = Populate(dataReader);

            dataReader.Close();
            dataReader.Dispose();
            ConnectionFactory.Fechar();

            return item;
        }

        public Socio GetByCpf(string cpf)
        {
            const string sql = @"SELECT [id]
      ,[idEndereco]
      ,[nome]
      ,[aniversario]
      ,[rg]
      ,[cpf]
      ,[email]
  FROM [dbo].[Socio] WHERE [cpf] = @cpf";
            var parametro = new Dictionary<string, object> {{"@cpf", cpf}};
            var dataReader = ExecuteReader(sql, parametro);
            var item = Populate(dataReader);

            dataReader.Close();
            dataReader.Dispose();
            ConnectionFactory.Fechar();

            return item;
        }

        public Socio GetByRg(string rg)
        {
            const string sql = @"SELECT [id]
      ,[idEndereco]
      ,[nome]
      ,[aniversario]
      ,[rg]
      ,[cpf]
      ,[email]
  FROM [dbo].[Socio] WHERE [rg] = @rg";
            var parametro = new Dictionary<string, object> {{"@rg", rg}};
            var dataReader = ExecuteReader(sql, parametro);
            var item = Populate(dataReader);

            dataReader.Close();
            dataReader.Dispose();
            ConnectionFactory.Fechar();

            return item;
        }

        public List<Socio> GetAll()
        {
            var socios = new List<Socio>();
            const string sql = @"SELECT [id]
      ,[idEndereco]
      ,[nome]
      ,[aniversario]
      ,[rg]
      ,[cpf]
      ,[email]
  FROM [dbo].[Socio]";
            var dataReader = ExecuteReader(sql);

            while (dataReader.Read())
                socios.Add(Populate(dataReader));

            dataReader.Close();
            dataReader.Dispose();
            ConnectionFactory.Fechar();

            return socios;
        }        

        protected override Socio Populate(System.Data.SqlClient.SqlDataReader dataReader)
        {
            const string msg = "Objeto DataReader não foi inicializado ou está fechado...";

            if (dataReader == null || dataReader.IsClosed)
                throw new ArgumentNullException(msg);

            _id = dataReader.GetOrdinal("id");
            _idEndereco = dataReader.GetOrdinal("idEndereco");
            _nome = dataReader.GetOrdinal("nome");
            _aniversario = dataReader.GetOrdinal("aniversario");
            _rg = dataReader.GetOrdinal("rg");
            _cpf = dataReader.GetOrdinal("cpf");
            _email = dataReader.GetOrdinal("email");

            var socio = new Socio();

            if (dataReader.IsDBNull(_id))
                socio.Id = dataReader.GetInt32(_id);

            if (dataReader.IsDBNull(_idEndereco))
                socio.Endereco = EnderecoRepositorio.GetBy(dataReader.GetInt32(_idEndereco));

            if (dataReader.IsDBNull(_nome))
                socio.Nome = dataReader.GetString(_nome);

            if (dataReader.IsDBNull(_aniversario))
                socio.Aniversario = dataReader.GetDateTime(_aniversario);

            if (dataReader.IsDBNull(_rg))
                socio.Rg = dataReader.GetString(_rg);

            if (dataReader.IsDBNull(_cpf))
                socio.Cpf = dataReader.GetString(_cpf);

            if (dataReader.IsDBNull(_email))
                socio.Email = dataReader.GetString(_email);

            return socio;
        }
    }
}
