using QueueCore.Application.Models.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace QueueCore.Application.Interfaces
{
    public interface IApiConsumer
    {
        // Generea y envia la peticion http
        Task<bool> SendRequestAsync(ApiRequestDto apiRequestDto);
    }
}
