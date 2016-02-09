using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVD.Model
{
    [Serializable]
    public class Categoria
    {
        public int         ID           { get; set; }
        public string      Descricao    { get; set; }
        public decimal     ValorLocacao { get; set; }
        public List<Filme> Filmes       { get { return new List<Filme>(); } }

        public bool AddFilme(Filme filme)
        {
            if (filme == null) return false;
            Filmes.Add(filme);
            return true;
        }
    }
}
