using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Models.Dto
{
    public class AzureConfigurationDto
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string QueueName { get; set; } = string.Empty;
    }
}
