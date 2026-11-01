using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Models.Dto
{
    public sealed record StatusDto(
        string Status, string Message, DateTime Timestamp
        );
}
