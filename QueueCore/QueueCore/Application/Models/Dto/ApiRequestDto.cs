using System;
using System.Collections.Generic;
using System.Text;

namespace QueueCore.Application.Models.Dto
{
    public class ApiRequestDto
    {
        public HttpMethod Method { get; set; }
        public string Endpoint { get; set; } = string.Empty;
        public string Payload { get; set; } = string.Empty;

        public ApiRequestDto() { }

        public ApiRequestDto(HttpMethod method, string endpoint, string payload)
        {
            Method = method;
            Endpoint = endpoint;
            Payload = payload;
        }

        public ApiRequestDto(HttpMethod method, string endpoint)
        {
            Method = method;
            Endpoint = endpoint;
        }
    }
}
