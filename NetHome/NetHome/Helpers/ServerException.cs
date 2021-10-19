using System;
using System.Collections.Generic;
using System.Text;

namespace NetHome.Helpers
{
    public class ServerException : Exception
    {
        public string Reason { get; set; }
        public ServerException(string reason, string message) : base(message)
        {
            Reason = reason;
        }
    }
}
