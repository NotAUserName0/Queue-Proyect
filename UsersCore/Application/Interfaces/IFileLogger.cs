using System;
using System.Collections.Generic;
using System.Text;
using UsersCore.Application.Models.Dto;

namespace Application.Interfaces
{
    public interface IFileLogger
    {
        Task GenerateLog(LogDto log);
    }
}
