using System.Text.Json.Serialization;

namespace Ecosens_WebPage.Models
{
    public class DashboardDatosResponse
    {
        [JsonPropertyName("plastico")]
        public int Plastico { get; set; }
        [JsonPropertyName("metal")]
        public int Metal { get; set; }

    }
}
