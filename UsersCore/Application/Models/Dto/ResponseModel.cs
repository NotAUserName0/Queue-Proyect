using System;
using System.Collections.Generic;

namespace Application.Models.Dto
{
    public class ResponseModel<T>
    {
        public T Data { get; set; }

        public enum ResponseType
        {
            WARNING = 0,
            SUCCESS = 1,
            FAIL = -1
        }

        public ResponseType Type { get; set; }
        public string MessageKey { get; set; }
        public string Message { get; set; }
        public object[] MessageParams { get; set; }
        public string InnerMessage { get; set; }
        public int StatusCode { get; set; }
        public bool Success { get; set; }
        public int TotalRecords { get; set; }

        // Nueva propiedad para almacenar los errores de validación por campo (Key/Message)
        public Dictionary<string, string[]> Errors { get; set; } = new();

        public ResponseModel()
        {
            Type = ResponseType.SUCCESS;
            MessageKey = string.Empty;
            MessageParams = Array.Empty<object>();
            Message = string.Empty;
            InnerMessage = string.Empty;
            StatusCode = -1;
            Success = true;
            TotalRecords = 0;
        }

        public ResponseModel(string Mensaje, bool Exitoso, T data)
        {
            Type = Exitoso ? ResponseType.SUCCESS : ResponseType.FAIL;
            MessageKey = string.Empty;
            MessageParams = Array.Empty<object>();
            Message = Mensaje;
            InnerMessage = string.Empty;
            StatusCode = 1;
            Success = Exitoso;
            Data = data;
        }
    }
}