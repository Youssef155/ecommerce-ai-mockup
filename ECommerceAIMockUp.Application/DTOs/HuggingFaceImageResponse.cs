using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ECommerceAIMockUp.Application.DTOs
{
    public class HuggingFaceImageResponse
    {
        [JsonProperty("b64_json")]
        public string Base64 { get; set; } = "";

        [JsonProperty("mime_type")]
        public string MimeType { get; set; } = "image/png";
    }
}
