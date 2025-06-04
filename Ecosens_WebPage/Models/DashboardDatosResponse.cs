using System.Text.Json.Serialization;

namespace Ecosens_WebPage.Models
{
    public class DashboardDatosResponse
    {
        [JsonPropertyName("plastico")]
        public double Plastico { get; set; }
        [JsonPropertyName("metal")]
        public double Metal { get; set; }

    }
}
