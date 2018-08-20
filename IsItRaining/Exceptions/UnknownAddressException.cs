using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsItRaining.Exceptions
{
    public class UnknownAddressException : Exception
    {
        public UnknownAddressException() : base("Unkwnown address")
        {
        }
    }
}
