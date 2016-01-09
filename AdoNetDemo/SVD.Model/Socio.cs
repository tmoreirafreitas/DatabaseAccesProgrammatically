using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVD.Model
{
    public class Socio
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public DateTime? Aniversario { get; set; }
        public string RG { get; set; }
        public string CPF { get; set; }
        public string Email { get; set; }
        private List<Telefone> telefones;
        public Endereco Endereco { get; set; }

        public void AddTelefone(Telefone telefone)
        {
            if (telefone.Socio == null)
                telefone.Socio = this;

            telefones.Add(telefone);
        }

        public List<Telefone> GetTelefones()
        {
            return telefones;
        }
    }
}
