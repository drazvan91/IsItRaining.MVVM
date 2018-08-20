using System;

namespace IsItRaining.Exceptions
{
    public class UnknownAddressException : Exception
    {
        public UnknownAddressException()
            : base("Unkwnown address")
        {
        }
    }
}
