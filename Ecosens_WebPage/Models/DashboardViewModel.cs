using System.Text.Json.Serialization;

namespace Ecosens_WebPage.Models
{
    public class DashboardViewModel
    {
        [JsonPropertyName("plastico")]
        public double Plastico { get; set; }
        [JsonPropertyName("metal")]
        public double Metal { get; set; }
        public double Total => Plastico + Metal;
        public int TotalAlertas { get; set; }
    }
}
