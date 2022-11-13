using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerkoopTruithesBL.Exceptions
{
    public class BestellingManagerException : Exception
    {
        public BestellingManagerException(string? message) : base(message)
        {
        }

        public BestellingManagerException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
