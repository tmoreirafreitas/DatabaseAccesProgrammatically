using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SVD.Model
{
    public class Filme
    {
        public int ID { get; set; }
        public Genero Genero { get; set; }
        public Categoria Categoria { get; set; }
        public string Titulo { get; set; }
        public string Duracao { get; set; }
    }
}
