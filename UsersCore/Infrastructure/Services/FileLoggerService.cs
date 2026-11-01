using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Text.Json;
using UsersCore.Application.Models.Dto;

namespace Infrastructure.Services
{
    public class FileLoggerService : IFileLogger
    {
        private readonly string _logDirectory;

        public FileLoggerService(IConfiguration configuration)
        {
            var customPath = configuration["Logging:LogDirectoryPath"];

            _logDirectory = !string.IsNullOrEmpty(customPath) ? customPath : Path.Combine(AppContext.BaseDirectory, "Logs");
        }

        public async Task GenerateLog(LogDto log)
        {

            if (!Directory.Exists(_logDirectory))
            {
                Directory.CreateDirectory(_logDirectory);
            }

            // genera el archivo de los logs
            var uniqueId = Guid.NewGuid().ToString("N").Substring(0, 6);
            var logFilePath = Path.Combine(_logDirectory, $"ApiLogs_{DateTime.Now:yyyyMMdd}_{uniqueId}.txt");

            var logEntry = CreateLogMessage(log);

            //agrega el log al final del documento
            await File.WriteAllTextAsync(logFilePath, logEntry);
        }

        public string CreateLogMessage(LogDto log)
        {
            var logEntry = "";
            if (log.LogType == "command")
            {
                logEntry = $@"
                    =========================================
                    FECHA: {DateTime.Now:yyyy-MM-dd HH:mm:ss}
                    IS_SUCCESS: {log.ResponseBody}
                    ERROR: {log.Message}
                    =========================================";

                return logEntry;
            }

            logEntry = $@"
                    =========================================
                    FECHA: {DateTime.Now:yyyy-MM-dd HH:mm:ss}
                    DATA: {log.ResponseBody}
                    =========================================";

            return logEntry;
        }
    }
}
