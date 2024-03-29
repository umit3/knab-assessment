using Newtonsoft.Json;

namespace Infrastructure.Models.CoinMarketCap;

public class CryptoCurrencyMetadata
{
    [JsonProperty("id")]
    public string? Id { get; set; }
    [JsonProperty("symbol")]
    public string? Symbol { get; set; }
    [JsonProperty("name")]
    public string? Name { get; set; }
}