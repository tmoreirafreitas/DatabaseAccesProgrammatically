using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Utilities
{
    public class Validation
    {
        public static bool NumberValidation(string number)
        {
            //Expressao regular que valida número
            const string numeroValido = @"^[0-9]+?(.|,[0-9]+)$";

            Match match = Regex.Match(number, numeroValido);

            return match.Success;
        }

        public static bool EmailValidation(string email)
        {
            //Expressao regular que valida email

            const string emailValido = @"^([\w\-]+\.)*[\w\- ]+@([\w\- ]+\.)+([\w\-]{2,3})$";

            Match match = Regex.Match(email, emailValido);

            return match.Success;
        }

        public static bool TelefoneValidation(string telefone)
        {
            //Expressão regular que valida telefone
            const string telefoneValido = @"^[0-9]{2}-[0-9]{4}-[0-9]{4}$";

            Match match = Regex.Match(telefone, telefoneValido);

            return match.Success;
        }

        public static bool CelularValidation(string celular)
        {
            //Expressão regular que valida celular
            const string celularValido = @"^[0-9]{2}-[0-9]{5}-[0-9]{4}$";

            Match match = Regex.Match(celular, celularValido);

            return match.Success;
        }
    }
}
