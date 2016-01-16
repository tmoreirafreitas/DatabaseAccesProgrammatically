using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public EnderecoRepositorio enderecoRepositorio { get { return new EnderecoRepositorio(); } }
        public TelefoneRepositorio telefoneRepositorio { get { return new TelefoneRepositorio(); } }
        public LocacaoRepositorio locacaoRepositorio { get { return new LocacaoRepositorio(); } }

        public int Insert(Socio item)
        {
            try
            {
                var idEndereco = 0;
                if (item.Endereco != null)
                    idEndereco = enderecoRepositorio.Insert(item.Endereco);

                foreach (var telefone in item.GetTelefones())
                    telefoneRepositorio.Insert(telefone);

                string sql = @"INSERT INTO [dbo].[Socio]
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
                
                var parametros = new Dictionary<string, object>();
                parametros.Add("@idEndereco", item.Endereco.ID);
                parametros.Add("@nome", item.Nome);
                parametros.Add("@aniversario", item.Aniversario);
                parametros.Add("@rg", item.RG);
                parametros.Add("@cpf", item.CPF);
                parametros.Add("@email", item.Email);

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
                string sql = string.Empty;
                var parametros = new Dictionary<string, object>();

                if (!string.IsNullOrEmpty(item.RG))
                {
                    item = GetByRG(item.RG);
                    sql = @"DELETE FROM [dbo].[Socio] WHERE rg = @rg";
                    parametros.Add("@rg", item.RG);
                }
                else if (!string.IsNullOrEmpty(item.CPF))
                {
                    item = GetByCPF(item.CPF);
                    sql = @"DELETE FROM [dbo].[Socio] WHERE cpf = @cpf";
                    parametros.Add("@cpf", item.CPF);
                }
                else if (!string.IsNullOrEmpty(item.Email))
                {
                    item = GetBy(item.Email);
                    sql = @"DELETE FROM [dbo].[Socio] WHERE email = @email";
                    parametros.Add("@email", item.Email);
                }

                // Remove todos os telefones do sócio.
                telefoneRepositorio.RemoveAllBy(item);
                
                // Remove todas as locações do sócio
                locacaoRepositorio.RemoveAllBy(item);

                ExecuteCommand(sql, parametros);
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public void Update(Socio item)
        {
            string sql = string.Empty;
            var parametros = new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(item.RG))
            {
                sql = @"UPDATE [dbo].[Socio] SET [id] = @id ,[nome] = @nome ,[aniversario] = @aniversario ,[email] = @email WHERE rg = @rg";
                parametros.Add("@rg", item.RG);
            }
            else if (!string.IsNullOrEmpty(item.CPF))
            {
                sql = @"UPDATE [dbo].[Socio] SET [id] = @id ,[nome] = @nome ,[aniversario] = @aniversario ,[email] = @email WHERE cpf = @cpf";
                parametros.Add("@cpf", item.CPF);
            }

            ExecuteCommand(sql, parametros);
        }

        public Socio GetBy(int id)
        {
            string sql = @"SELECT [id] ,[idEndereco] ,[nome] ,[aniversario] ,[rg] ,[cpf] ,[email] FROM [dbo].[Socio]  WHERE [id] = @id";
            var parametros = new Dictionary<string, object>();
            parametros.Add("@id", id);
            return Populate(ExecuteReader(sql, parametros));
        }

        public Socio GetBy(string email)
        {
            string sql = @"SELECT [id] ,[idEndereco] ,[nome] ,[aniversario] ,[rg] ,[cpf] ,[email] FROM [dbo].[Socio] WHERE [email] = @email";
            var parametros = new Dictionary<string, object>();
            parametros.Add("@email", email);
            return Populate(ExecuteReader(sql, parametros));
        }

        public Socio GetByCPF(string cpf)
        {
            string sql = @"SELECT [id] ,[idEndereco] ,[nome] ,[aniversario] ,[rg] ,[cpf] ,[email] FROM [dbo].[Socio] WHERE [cpf] = @cpf";
            var parametros = new Dictionary<string, object>();
            parametros.Add("@cpf", cpf);
            return Populate(ExecuteReader(sql, parametros));
        }

        public Socio GetByRG(string rg)
        {
            string sql = @"SELECT [id] ,[idEndereco] ,[nome] ,[aniversario] ,[rg] ,[cpf] ,[email] FROM [dbo].[Socio] WHERE [rg] = @rg";
            var parametros = new Dictionary<string, object>();
            parametros.Add("@rg", rg);
            return Populate(ExecuteReader(sql, parametros));
        }

        public List<Socio> GetAll()
        {
            List<Socio> socios = new List<Socio>();
            string sql = @"SELECT [id] ,[idEndereco] ,[nome] ,[aniversario] ,[rg] ,[cpf] ,[email] FROM [dbo].[Socio]";
            var dataReader = ExecuteReader(sql);

            while (dataReader.Read())
                socios.Add(Populate(dataReader));

            return socios;
        }        

        protected override Socio Populate(System.Data.IDataReader dataReader)
        {
            Socio socio = new Socio();

            if (dataReader != null)
            {
                _id = dataReader.GetOrdinal("id");
                _idEndereco = dataReader.GetOrdinal("idEndereco");
                _nome = dataReader.GetOrdinal("nome");
                _aniversario = dataReader.GetOrdinal("aniversario");
                _rg = dataReader.GetOrdinal("rg");
                _cpf = dataReader.GetOrdinal("cpf");
                _email = dataReader.GetOrdinal("email");

                if (dataReader.IsDBNull(_id))
                    socio.ID = dataReader.GetInt32(_id);

                if (dataReader.IsDBNull(_idEndereco))
                    socio.Endereco = enderecoRepositorio.GetBy(dataReader.GetInt32(_idEndereco));

                if (dataReader.IsDBNull(_nome))
                    socio.Nome = dataReader.GetString(_nome);

                if (dataReader.IsDBNull(_aniversario))
                    socio.Aniversario = dataReader.GetDateTime(_aniversario);

                if (dataReader.IsDBNull(_rg))
                    socio.RG = dataReader.GetString(_rg);

                if (dataReader.IsDBNull(_cpf))
                    socio.CPF = dataReader.GetString(_cpf);

                if (dataReader.IsDBNull(_email))
                    socio.Email = dataReader.GetString(_email);

                return socio;
            }

            throw new ArgumentException();
        }
    }
}
