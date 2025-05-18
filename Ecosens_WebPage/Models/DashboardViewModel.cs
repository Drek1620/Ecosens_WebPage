using System.Text.Json.Serialization;

namespace Ecosens_WebPage.Models
{
    public class DashboardViewModel
    {
        [JsonPropertyName("plastico")]
        public int Plastico { get; set; }
        [JsonPropertyName("metal")]
        public int Metal { get; set; }
        public int Total => Plastico + Metal;
        public int TotalAlertas { get; set; }
    }
}
