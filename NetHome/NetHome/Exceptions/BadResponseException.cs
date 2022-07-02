using NetHome.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace NetHome.Exceptions
{
    public class BadResponseException : Exception
    {
        public string Reason { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        private BadResponseException(string reason, string message, HttpStatusCode statusCode) : base(message)
        {
            Reason = reason;
            StatusCode = statusCode;
        }

        public static BadResponseException Create(HttpResponseMessage response)
        {
            var reason = response.ReasonPhrase;
            Stream stream = response.Content.ReadAsStreamAsync().Result;
            string message;
            try
            {
                message = (JsonSerializer.Deserialize<Dictionary<string, string>>(stream, JsonHelper.GetOptions()))["Message"];
            }
            catch (Exception)
            {
                message = "Unknown error has occurred. No message recieved from server.";
            }
            return new BadResponseException(reason, message, response.StatusCode);
        }
    }
}
