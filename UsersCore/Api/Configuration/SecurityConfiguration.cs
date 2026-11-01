using System.ComponentModel.DataAnnotations;

namespace Api.Configuration
{
    public sealed class SecurityConfiguration
    {
        public const string SectionName = "Security";

        [Required(ErrorMessage = "ApiKey is mandatory")]
        public string ApiKey { get; init; } = string.Empty;
    }
}
