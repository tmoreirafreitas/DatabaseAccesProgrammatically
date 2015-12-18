using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVD.Model
{
    public class Categoria
    {
        public int?        ID           { get; set; }
        public string      Descricao    { get; set; }
        public decimal     ValorLocacao { get; set; }
        public List<Filme> Filmes       { get { return new List<Filme>(); } }

        public bool AddFilme(Filme filme)
        {
            if (filme != null)
            {
                Filmes.Add(filme);
                return true;
            }

            return false;
        }
    }
}
