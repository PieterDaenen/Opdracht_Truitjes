using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerkoopTruithesBL.Exceptions
{
    public class TruitjeException : Exception
    {
        public TruitjeException(string? message) : base(message)
        {
        }

        public TruitjeException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
