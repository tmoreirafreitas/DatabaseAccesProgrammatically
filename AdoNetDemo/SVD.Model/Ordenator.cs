using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SVD.Model
{
    public class Ordenator<T> : IComparer<T>
    {
        PropertyInfo propriedade;
        bool ascendente;

        public Ordenator(string nomePropriedade, bool ascendente)
        {
            propriedade = typeof(T).GetProperty(nomePropriedade);
            this.ascendente = ascendente;
        }

        public int Compare(T x, T y)
        {
            if (ascendente)
            {
                return ((IComparable)propriedade.GetValue(x, null)).CompareTo(propriedade.GetValue(y, null));
            }
            else
            {
                return ((IComparable)propriedade.GetValue(y, null)).CompareTo(propriedade.GetValue(x, null));
            }
        }
    }
}
