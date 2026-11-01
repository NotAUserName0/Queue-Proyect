using Azure;
using Microsoft.AspNetCore.WebUtilities;
using QueueCore.Application.Interfaces;
using QueueCore.Application.Models.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace QueueCore.Infrastructure.Common
{
    public class LoggerFile : ILoggerFile
    {
        public async Task GenerateLog(LogDto log)
        {
            // genera directorio de logs en la raiz del proyecto
            var logDirectory = Path.Combine(AppContext.BaseDirectory, "Logs");

            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            // genera el archivo de los logs
            var logFilePath = Path.Combine(logDirectory, $"ApiLogs_{DateTime.Now:yyyyMMdd}.txt");

            // contenido de logs
            var logEntry = $@"
                =========================================
                FECHA: {DateTime.Now:yyyy-MM-dd HH:mm:ss}
                REQUEST: {log.Method} {log.Endpoint}
                PAYLOAD: {log.Payload ?? "N/A"}
                RESPONSE STATUS: {log.StatusCode}
                RESPONSE BODY: {log.ResponseBody}
                =========================================";

            //agrega el log al final del documento
            await File.AppendAllTextAsync(logFilePath, logEntry + Environment.NewLine);
        }
    }
}
