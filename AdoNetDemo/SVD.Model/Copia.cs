using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVD.Model
{
    public class Copia
    {
        public int ID { get; set; }
        public Filme Filme { get; set; }
        public DateTime DataCopia { get; set; }
        public bool SituacaoCopia { get; set; }
    }
}
