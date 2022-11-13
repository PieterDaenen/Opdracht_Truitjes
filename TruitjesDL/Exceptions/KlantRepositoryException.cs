using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerkoopTruitjesDL.Exceptions
{
    public class KlantRepositoryException : Exception
    {
        public KlantRepositoryException(string? message) : base(message)
        {
        }

        public KlantRepositoryException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
