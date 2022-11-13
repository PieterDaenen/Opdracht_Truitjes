using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerkoopTruithesBL.Exceptions
{
    public class ClubManagerException : Exception
    {
        public ClubManagerException(string? message) : base(message)
        {
        }

        public ClubManagerException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
