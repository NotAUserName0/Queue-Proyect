using Microsoft.Extensions.Options;
using QueueCore.Application.Models.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace QueueCore.Application.Interfaces
{
    public interface IPetitionGenerator
    {
        // Genera la peticion HTTP y la envia al endpoint correspondiente
        Task ProcessMessageAsync(string message);
    }
}
