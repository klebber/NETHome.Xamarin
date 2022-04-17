using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace NetHome.Helpers
{
    public class ServerCommunicationException : Exception
    {
        public string Reason { get; set; }
        public string DetailedMessage { get; set; }

        public ServerCommunicationException(string reason, string detailedMessage)
        {
            Reason = reason;
            DetailedMessage = detailedMessage;
        }

        public ServerCommunicationException(HttpResponseMessage response)
        {
            Reason = response.ReasonPhrase;
            Stream stream = response.Content.ReadAsStreamAsync().Result;
            try
            {
                DetailedMessage = (JsonSerializer.Deserialize<Dictionary<string, string>>(stream, JsonHelper.GetOptions()))["Message"];
            }
            catch (Exception)
            {
                DetailedMessage = "Unknown error has occurred. No message recieved from server.";
            }
        }
    }
}
