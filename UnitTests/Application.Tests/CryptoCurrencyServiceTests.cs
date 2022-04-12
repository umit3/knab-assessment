using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Services;
using FluentAssertions;
using Infrastructure.Abstractions;
using Infrastructure.Dtos;
using Infrastructure.Models.CoinMarketCap;
using Moq;
using Xunit;

namespace UnitTests.Application.Tests;

public class CryptoCurrencyServiceTests
{
    private readonly CryptoCurrencyService _sut;
    private readonly Mock<ICryptoExchangeClient> _mockCryptoExchangeClient;

    public CryptoCurrencyServiceTests()
    {
        _mockCryptoExchangeClient = new Mock<ICryptoExchangeClient>();
        _sut = new CryptoCurrencyService(_mockCryptoExchangeClient.Object);
    }

    [Fact]
    public async Task GetExchangeRatesAsync_Success()
    {
        //Arrange
        var symbol = "BTC";
        var fiats = new[] { "USD" };

        var mockRateResponses = GetProperRateResponses(symbol, fiats);

        var mockQuoteResponse = GetProperQuoteMockResponse(symbol, fiats.First());

        _mockCryptoExchangeClient
            .Setup(s => s.GetCryptoQuoteAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(mockQuoteResponse);

        //Act
        var result = await _sut.GetExchangeRatesAsync(new RateRequest { Symbol = symbol }, fiats);
        //Assert
        result.Should().NotBeNull().And.BeEquivalentTo(mockRateResponses);
    }
    
    [Fact]
    public async Task GetExchangeRatesAsync_Error()
    {
        //Arrange
        var symbol = "BTC";
        var fiats = new[] { "USD" };

        var mockRateResponses = GetErrorRateResponses();

        var mockQuoteResponse = GetErrorQuoteMockResponse();

        _mockCryptoExchangeClient
            .Setup(s => s.GetCryptoQuoteAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(mockQuoteResponse);

        //Act
        var result = await _sut.GetExchangeRatesAsync(new RateRequest { Symbol = symbol }, fiats);
        //Assert
        result.Should().NotBeNull().And.BeEquivalentTo(mockRateResponses);
    }

    private static List<RateResponse> GetProperRateResponses(string symbol, string[] fiats)
    {
        return fiats.Select(fiat => new RateResponse
            {
                Succeeded = true,
                ErrorMessage = null,
                CryptoCurrency = new CryptoCurrency
                {
                    Id = "1",
                    Name = "Name",
                    Symbol = symbol,
                    Quote = new()
                    {
                        { fiat, new CryptoCurrencyPrice { Value = 1000 } }
                    }
                }
            }).ToList();
    }
    
    private static List<RateResponse> GetErrorRateResponses()
    {
        return new List<RateResponse>
        {
            new()
            {
                Succeeded = false,
                ErrorMessage = "An error occured while processing your request."
            }
        };
    }

    private static CoinMarketCapResponseModel<CryptoCurrency> GetProperQuoteMockResponse(string symbol, string fiat)
    {
        return new CoinMarketCapResponseModel<CryptoCurrency>
        {
            Status = new Status
            {
                ErrorCode = 0,
                ErrorMessage = null
            },
            Data = new Dictionary<string, List<CryptoCurrency>>
            {
                {
                    symbol, new List<CryptoCurrency>
                    {
                        new()
                        {
                            Id = "1",
                            Symbol = symbol,
                            Name = "Name",
                            Quote = new Dictionary<string, CryptoCurrencyPrice>
                            {
                                { fiat, new CryptoCurrencyPrice { Value = 1000 } }
                            }
                        }
                    }
                }
            }
        };
    }

    private static CoinMarketCapResponseModel<CryptoCurrency> GetErrorQuoteMockResponse()
    {
        return new CoinMarketCapResponseModel<CryptoCurrency>
        {
            Status = new Status
            {
                ErrorCode = 400,
                ErrorMessage = "An error occured while processing your request."
            }
        };
    }
}