using Newtonsoft.Json;

namespace Infrastructure.Models.CoinMarketCap;

public class CoinMarketCapResponseModel<T>
{
    public bool Succeeded => Status?.ErrorCode == 0;

    [JsonProperty("status")]
    public Status? Status { get; set; }
    
    [JsonProperty("data")]
    public Dictionary<string, List<T>>? Data { get; set; }
}

public class Status
{
    [JsonProperty("error_code")] public int ErrorCode { get; set; }
    [JsonProperty("error_message")] public string? ErrorMessage { get; set; }
}