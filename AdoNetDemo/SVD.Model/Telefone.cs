using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVD.Model
{
    public class Telefone
    {
        public long?  ID     { get; set; }
        public string DDD    { get; set; }
        public string Numero { get; set; }
        public Socio  Socio  { get; set; }
    }
}
