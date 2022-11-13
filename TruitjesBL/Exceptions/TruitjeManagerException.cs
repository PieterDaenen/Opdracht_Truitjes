using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerkoopTruithesBL.Exceptions
{
    public class TruitjeManagerException : Exception
    {
        public TruitjeManagerException(string? message) : base(message)
        {
        }

        public TruitjeManagerException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
