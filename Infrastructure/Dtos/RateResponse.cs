using Infrastructure.Models.CoinMarketCap;

namespace Infrastructure.Dtos;

public class RateResponse
{
    public bool Succeeded { get; set; }
    public string? ErrorMessage { get; set; }
    
    public CryptoCurrency? CryptoCurrency { get; set; }
}