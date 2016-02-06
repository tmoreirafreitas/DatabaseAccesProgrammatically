using System;
using System.Collections.Generic;
using SVD.Model;

namespace AdoNetDemo
{
    public class EnderecoRepositorio : RepositorioBase<Endereco>, IRepositorio<Endereco>
    {
        //id	        int
        //idsocio	    bigint
        //rua	        varchar
        //numero	    int
        //complemento	text
        //cep	        char
        //bairro	    varchar
        //cidade	    varchar
        //estado	    varchar

        private int _id;
        private int _idsocio;
        private int _rua;
        private int _numero;
        private int _complemento;
        private int _cep;
        private int _bairro;
        private int _cidade;
        private int _estado;

        public int Insert(Endereco item)
        {
            try
            {
                const string sql = @"INSERT INTO [dbo].[Endereco]
           ([rua]
           ,[numero]
           ,[complemento]
           ,[cep]
           ,[bairro]
           ,[cidade]
           ,[estado])
     VALUES
           (@rua
           ,@numero
           ,@complemento
           ,@cep
           ,@bairro
           ,@cidade
           ,@estado);SELECT CAST(SCOPE_IDENTITY() AS INT);";

                var parametros = new Dictionary<string, object>
                {
                    {"@rua", item.Rua},
                    {"@numero", item.Numero},
                    {"@complemento", item.Complemento},
                    {"@cep", item.CEP},
                    {"@bairro", item.Bairro},
                    {"@cidade", item.Cidade},
                    {"@estado", item.Estado}
                };

                return ExecuteCommand(sql, parametros);
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public void Remove(Endereco item)
        {
            try
            {
                const string sql = @"DELETE FROM [dbo].[Endereco] WHERE cep = @cep";
                var parametros = new Dictionary<string, object> {{"@cep", item.CEP}};
                ExecuteCommand(sql, parametros);
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public void Update(Endereco item)
        {
            try
            {
                const string sql = @"UPDATE [dbo].[Endereco] SET [rua] = @rua ,[numero] = @numero ,[complemento] = " +
                                   "@complemento ,[bairro] = @bairro ,[cidade] = @cidade ,[estado] = @estado WHERE cep = @cep";

                var parametros = new Dictionary<string, object>
                {
                    {"@rua", item.Rua},
                    {"@numero", item.Numero},
                    {"@complemento", item.Complemento},
                    {"@cep", item.CEP},
                    {"@bairro", item.Bairro},
                    {"@cidade", item.Cidade},
                    {"@estado", item.Estado}
                };
                ExecuteCommand(sql, parametros);
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public Endereco GetBy(int id)
        {
            try
            {
                const string sql = @"SELECT [id] ,[rua] ,[numero] ,[complemento] ,[cep] ,[bairro] ,[cidade] ,[estado] FROM [dbo].[Endereco] WHERE id = @id";
                var parametros = new Dictionary<string, object> {{"@id", id}};
                var dataReader = ExecuteReader(sql, parametros);
                var item = Populate(dataReader);

                dataReader.Close();
                dataReader.Dispose();
                ConnectionFactory.Fechar();

                return item;
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public Endereco GetBy(char[] cep)
        {
            try
            {
                const string sql = @"SELECT [id] ,[rua] ,[numero] ,[complemento] ,[cep] ,[bairro] ,[cidade] ,[estado] FROM [dbo].[Endereco] WHERE cep = @cep";
                var parametros = new Dictionary<string, object> {{"@cep", cep}};
                var dataReader = ExecuteReader(sql, parametros);
                var item = Populate(dataReader);

                dataReader.Close();
                dataReader.Dispose();
                ConnectionFactory.Fechar();

                return item;
            }
            catch (SystemException ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public List<Endereco> GetAll()
        {
            try
            {
                var enderecos = new List<Endereco>();
                const string sql = @"SELECT [id] ,[rua] ,[numero] ,[complemento] ,[cep] ,[bairro] ,[cidade] ,[estado] FROM [dbo].[Endereco] ORDER BY estado, cidade, rua";
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

        protected override Endereco Populate(System.Data.SqlClient.SqlDataReader dataReader)
        {
            const string msg = "Objeto DataReader não foi inicializado ou está fechado...";

            if (dataReader == null || dataReader.IsClosed)
                throw new ArgumentNullException(msg);

            _id = dataReader.GetOrdinal("id");
            _rua = dataReader.GetOrdinal("rua");
            _numero = dataReader.GetOrdinal("numero");
            _complemento = dataReader.GetOrdinal("complemento");
            _cep = dataReader.GetOrdinal("cep");
            _bairro = dataReader.GetOrdinal("bairro");
            _cidade = dataReader.GetOrdinal("cidade");
            _estado = dataReader.GetOrdinal("estado");

            var endereco = new Endereco();
            if (!dataReader.IsDBNull(_id))
                endereco.ID = dataReader.GetInt32(_id);

            if (!dataReader.IsDBNull(_estado))
                endereco.Estado = dataReader.GetString(_estado);

            if (!dataReader.IsDBNull(_bairro))
                endereco.Bairro = dataReader.GetString(_bairro);

            if (!dataReader.IsDBNull(_cep))
                endereco.CEP = dataReader.GetString(_cep);

            if (!dataReader.IsDBNull(_cidade))
                endereco.Cidade = dataReader.GetString(_cidade);

            if (!dataReader.IsDBNull(_complemento))
                endereco.Complemento = dataReader.GetString(_complemento);

            return endereco;
        }
    }
}
