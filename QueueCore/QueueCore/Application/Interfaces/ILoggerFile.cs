using QueueCore.Application.Models.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace QueueCore.Application.Interfaces
{
    public interface ILoggerFile
    {
        Task GenerateLog(LogDto log);
    }
}
