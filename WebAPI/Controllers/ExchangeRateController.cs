using Application.Services;
using Infrastructure.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebAPI.Models;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ExchangeRateController : ControllerBase
{
    private readonly ICryptoCurrencyService _cryptoCurrencyService;
    private readonly FiatConfiguration _fiatConfiguration;

    public ExchangeRateController(
        ICryptoCurrencyService cryptoCurrencyService,
        IOptions<FiatConfiguration> fiatConfiguration)
    {
        _cryptoCurrencyService = cryptoCurrencyService;
        _fiatConfiguration = fiatConfiguration.Value;
    }

    [HttpGet("GetRates")]
    public async Task<ActionResult<IEnumerable<RateResponse>>> GetRates([FromQuery] RateRequest request)
    {
        var responses = await _cryptoCurrencyService.GetExchangeRatesAsync(request, _fiatConfiguration.Fiats);
        return Ok(responses);
    }
}