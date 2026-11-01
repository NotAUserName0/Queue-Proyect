using System;
using System.Collections.Generic;
using System.Text;

namespace QueueCore.Application.Models.Dto
{
    public class UserQueueDto
    {
        public string Action { get; set; } = string.Empty;
        public object UserData { get; set; } = null!;
    }
}
