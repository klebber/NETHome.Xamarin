using System;
using System.Collections.Generic;
using System.Text;

namespace NetHome.Exceptions
{
    public class ServerAuthorizationException : UnauthorizedAccessException
    {
        public ServerAuthorizationException() : base("Unknown authorization error occurred.") { }
        public ServerAuthorizationException(string message) : base(message) { }
    }
}
