using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVD.Model
{
    public class ItemLocacao
    {
        public long ID { get; set; }
        public Locacao Locacao { get; set; }
        public Copia Copia { get; set; }
        public decimal ValorLocacao { get; set; }
    }
}
