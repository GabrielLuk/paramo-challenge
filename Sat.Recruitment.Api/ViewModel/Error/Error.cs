using Newtonsoft.Json;

namespace Demo.Luka.Api.Models
{
    /// <summary>
    /// Error
    /// </summary>
    [JsonObject(Title = "error")]
    public class Error
    {
        /// <summary>
        /// Codigo
        /// </summary>
        [JsonProperty(PropertyName = "codigo")]
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Titulo
        /// </summary>
        [JsonProperty(PropertyName = "titulo")]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Detalle
        /// </summary>
        [JsonProperty(PropertyName = "detalle")]
        public string Detail { get; set; } = string.Empty;
    }
}
