using System;
using System.Collections.Generic;
using System.Text;

namespace NetHome.Helpers
{
    public class RequestResultPayload<T> : RequestResult
    {
        public T Paylaod { get; set; }

        public RequestResultPayload(bool isSuccessful) : base(isSuccessful) { }

        public RequestResultPayload(bool isSuccessful, T payload) : base(isSuccessful)
        {
            Paylaod = payload;
        }
    }
}
