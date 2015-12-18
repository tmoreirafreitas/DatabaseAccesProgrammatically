using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVD.Model
{
    public class Endereco
    {
        public int?   ID          { get; set; }
        public string Rua         { get; set; }
        public int    Numero      { get; set; }
        public string Complemento { get; set; }
        public char   CEP         { get; set; }
        public string Bairro      { get; set; }
        public string Cidade      { get; set; }
        public string Estado      { get; set; }
        public Socio  Socio       { get; set; }
    }
}
