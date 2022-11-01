using Newtonsoft.Json;
using System.Collections.Generic;

namespace Demo.Luka.Api.Models
{
    [JsonObject(Title = "detalle_error_model")]
    public class ErrorDetailModel
    {
        public string Code { get; set; } = string.Empty;
        public List<string> Errors { get; set; }
        public ErrorDetailModel()
        {
            Errors = new List<string>();
        }
    }
}
