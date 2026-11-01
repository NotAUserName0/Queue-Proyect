using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IAzureQueueService
    {
        Task SendMessageAsync(string messageContent, CancellationToken cancellationToken = default);
    }
}
