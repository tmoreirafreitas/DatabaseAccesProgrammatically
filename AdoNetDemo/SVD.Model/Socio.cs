using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVD.Model
{
    public class Socio
    {
        public long ID { get; set; }
        public string Nome { get; set; }
        public DateTime Aniversario { get; set; }
        public char[] RG { get; set; }
        public char[] CPF { get; set; }
        public string Email { get; set; }
        public List<Telefone> Telefones { get; set; }
        public Endereco Endereco { get; set; }
    }
}
