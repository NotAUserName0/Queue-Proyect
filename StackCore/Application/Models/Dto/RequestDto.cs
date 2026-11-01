using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Models.Dto
{
    public class RequestDto
    {
        // Indica qué operación hará el Worker (Ej: "CREATE", "UPDATE", "DELETE")
        public string Action { get; set; } = string.Empty;

        // Los datos del usuario (reutilizas tu DTO existente)
        public UserDto UserData { get; set; } = null!;

        // Fecha en la que el API 1 recibió la solicitud original
        public DateTime QueuedAt { get; set; } = DateTime.UtcNow;
    }
}
