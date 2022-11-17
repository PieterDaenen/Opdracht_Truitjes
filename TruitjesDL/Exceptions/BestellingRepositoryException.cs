using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruitjesDL.Exceptions
{
    public class BestellingRepositoryException : Exception
    {
        public BestellingRepositoryException(string? message) : base(message)
        {
        }

        public BestellingRepositoryException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
