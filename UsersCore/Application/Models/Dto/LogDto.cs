using System;
using System.Collections.Generic;
using System.Text;

namespace UsersCore.Application.Models.Dto
{
    public class LogDto
    {
        public HttpMethod Method { get; set; } = null!;
        public string Endpoint { get; set; } = string.Empty;
        public string Payload { get; set; } = string.Empty;
        public int StatusCode { get; set; } = 0;
        public string ResponseBody { get; set; } = string.Empty;
        public string LogType { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
