using NetHome.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace NetHome.Exceptions
{
    public class BadResponseException : Exception
    {
        public string Reason { get; set; }
        public int StatusCode { get; set; }
        new public string Message { get; set; }
        public BadResponseException(HttpResponseMessage response) : base()
        {
            Reason = response.ReasonPhrase;
            Stream stream = response.Content.ReadAsStreamAsync().Result;
            try
            {
                Message = (JsonSerializer.Deserialize<Dictionary<string, string>>(stream, JsonHelper.GetOptions()))["Message"];
            }
            catch (Exception)
            {
                Message = "Unknown error has occurred. No message recieved from server.";
            }
        }
    }
}
