﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetHome.Common.Models
{
    public class LoginResponseModel
    {
        public UserModel User { get; set; }
        public string Token { get; set; }
    }
}
