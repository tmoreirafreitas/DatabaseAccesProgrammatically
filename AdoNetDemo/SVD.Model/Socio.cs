using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVD.Model
{
    public class Socio
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public DateTime? Aniversario { get; set; }
        public string Rg { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        private readonly List<Telefone> _telefones;

        public Socio()
        {
            _telefones = new List<Telefone>();
        }

        public Endereco Endereco { get; set; }

        public void AddTelefone(Telefone telefone)
        {
            if (telefone.Socio == null)
                telefone.Socio = this;

            _telefones.Add(telefone);
        }

        public List<Telefone> GetTelefones()
        {
            return _telefones;
        }
    }
}
