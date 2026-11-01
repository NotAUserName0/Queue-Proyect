using System;
using System.Collections.Generic;
using System.Text;

namespace QueueCore.Application.Models.Dto
{
    public class LogDto
    {
        public HttpMethod Method { get; set; } = null!;
        public string Endpoint { get; set; } = string.Empty;
        public string Payload { get; set; } = string.Empty;
        public int StatusCode { get; set; } = 0;
        public string ResponseBody { get; set; } = string.Empty;

        public LogDto(HttpMethod method, string endpoint, string payload)
        {
            Method = method;
            Endpoint = endpoint;
            Payload = payload;
        }

        public LogDto(HttpMethod method, string endpoint, string payload, int statusCode, string responseBody)
        {
            Method = method;
            Endpoint = endpoint;
            Payload = payload;
            StatusCode = statusCode;
            ResponseBody = responseBody;
        }
    }
}
