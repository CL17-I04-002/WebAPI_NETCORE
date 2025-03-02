﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeWEB.Utility
{
    public class Resource
    {
        public const string APIBaseUrl = "https://localhost:44368/";
        public const string EmployeeAPIUrl = APIBaseUrl + "api/employee/";
        public const string RegisterAPIUrl = APIBaseUrl + "api/user/register";
        public const string LoginAPIUrl = APIBaseUrl + "api/user/login";
        public const string contentType = "application/json";
    }
}
