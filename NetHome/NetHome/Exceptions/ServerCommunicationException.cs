using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace NetHome.Exceptions
{
    public class ServerCommunicationException : Exception
    {
        public string Reason { get; set; }

        public ServerCommunicationException(string reason, string message) : base(message)
        {
            Reason = reason;
        }
    }
}
