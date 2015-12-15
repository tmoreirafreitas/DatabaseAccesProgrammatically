using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVD.Model
{
    public class Atuacao
    {
        public int ID { get; set; }
        public Ator Ator { get; set; }
        public Filme Filme { get; set; }
        public string Papel { get; set; }
    }
}
