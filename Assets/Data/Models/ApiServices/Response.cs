﻿using System.Collections.Generic;

namespace Models.ApiServices
{
    public class Response
    {
        public long StatusCode { get; set; }
        public string Error { get;set; }
        public string Data { get; set; }  
        public Dictionary<string, string> Headers {get; set;} 
    }
}

