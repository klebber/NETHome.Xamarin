using System;
using System.Collections.Generic;
using System.Text;

namespace NetHome.Helpers
{
    public class RequestResult
    {
        public bool IsSuccessful { get; set; }
        public bool ShowMessage { get; set; }
        public string ErrorType { get; set; }
        public string ErrorMessage { get; set; }

        public RequestResult(bool isSuccessful)
        {
            IsSuccessful = isSuccessful;
            ShowMessage = true;
        }
    }
}
