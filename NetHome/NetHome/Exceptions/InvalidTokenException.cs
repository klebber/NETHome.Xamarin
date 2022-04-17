using System;
using System.Collections.Generic;
using System.Text;

namespace NetHome.Exceptions
{
    public class InvalidTokenException : UnauthorizedAccessException
    {
        public InvalidTokenException() : base("Unknown token authorization error occurred.") { }
        public InvalidTokenException(string message) : base(message) { }
    }
}
