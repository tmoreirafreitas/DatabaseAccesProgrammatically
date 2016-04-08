using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SVD.Model;
using AdoNetDemo;

namespace SVDConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            AtorRepositorio repositorio = new AtorRepositorio();
            Console.WriteLine("Nome:");

            Ator ator = new Ator { Nome = Console.ReadLine() };
            repositorio.Insert(ator);

            Console.ReadKey();
        }
    }
}
