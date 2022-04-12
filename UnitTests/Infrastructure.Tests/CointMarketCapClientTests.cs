using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Infrastructure;
using Infrastructure.Models.CoinMarketCap;
using Moq;
using Moq.Language.Flow;
using Moq.Protected;
using Newtonsoft.Json;
using Xunit;

namespace UnitTests.Infrastructure.Tests;

public class CointMarketCapClientTests
{
    private readonly Mock<HttpMessageHandler> _httpMessageHandler;

    public CointMarketCapClientTests()
    {
        _httpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
    }
    
    [Fact]
    public async Task GetExchangeRate_Success()
    {
        //Arrange
        var symbol = "BTC";
        var fiat = "USD";
        var pathAndQuery = $"/v2/cryptocurrency/quotes/latest?symbol={symbol}&convert={fiat}";
        var setup = GetSetup(pathAndQuery);
        var sut = GetSut();
        var mockResponse = GetQuoteProperMockResponse(symbol, fiat);
        var exchangeRateQueryString = string.Empty;
        
        setup.ReturnsAsync(new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonConvert.SerializeObject(mockResponse))
        }).Callback<HttpRequestMessage, CancellationToken>(async (r, c) =>
        {
            exchangeRateQueryString = r.RequestUri.PathAndQuery;
        });

        //Act
        var result = await sut.GetCryptoQuoteAsync(symbol, fiat);
        //Assert
        exchangeRateQueryString.Should().Be(pathAndQuery);
        result.Should().NotBeNull().And.BeEquivalentTo(mockResponse);
    }
    
    [Theory]
    [InlineData("InvalidSymbol", "USD")]
    [InlineData("InvalidSymbol", "InvalidFiat")]
    [InlineData("BTC", "InvalidFiat")]
    public async Task GetExchangeRate_With_ErrorMessage(string symbol, string fiat)
    {
        //Arrange
        var pathAndQuery = $"/v2/cryptocurrency/quotes/latest?symbol={symbol}&convert={fiat}";
        var setup = GetSetup(pathAndQuery);
        var sut = GetSut();
        var mockResponse = GetQuoteErrorMockResponse();
        var exchangeRateQueryString = string.Empty;

        setup.ReturnsAsync(new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.BadRequest,
            Content = new StringContent(JsonConvert.SerializeObject(mockResponse))
        }).Callback<HttpRequestMessage, CancellationToken>(async (r, c) =>
        {
            exchangeRateQueryString = r.RequestUri.PathAndQuery;
        });

        //Act
        var result = await sut.GetCryptoQuoteAsync(symbol, fiat);
        //Assert
        exchangeRateQueryString.Should().Be(pathAndQuery);
        result.Should().NotBeNull().And.BeEquivalentTo(mockResponse);
    }
    
    [Fact]
    public async Task GetMetadata_Success()
    {
        //Arrange
        var symbol = "BTC";
        var pathAndQuery = $"/v2/cryptocurrency/info?symbol={symbol}";
        var setup = GetSetup(pathAndQuery);
        var sut = GetSut();
        var mockResponse = GetMetadataProperMockResponse(symbol);
        var exchangeRateQueryString = string.Empty;
        
        setup.ReturnsAsync(new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonConvert.SerializeObject(mockResponse))
        }).Callback<HttpRequestMessage, CancellationToken>(async (r, c) =>
        {
            exchangeRateQueryString = r.RequestUri.PathAndQuery;
        });

        //Act
        var result = await sut.GetCryptoCurrencyMetadataAsync(symbol);
        //Assert
        exchangeRateQueryString.Should().Be(pathAndQuery);
        result.Should().NotBeNull().And.BeEquivalentTo(mockResponse);
    }
    
    [Fact]
    public async Task GetMetadata_With_ErrorMessage()
    {
        //Arrange
        var symbol = "InvalidSymbol";
        var pathAndQuery = $"/v2/cryptocurrency/info?symbol={symbol}";
        var setup = GetSetup(pathAndQuery);
        var sut = GetSut();
        var mockResponse = GetQuoteErrorMockResponse();
        var exchangeRateQueryString = string.Empty;

        setup.ReturnsAsync(new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.BadRequest,
            Content = new StringContent(JsonConvert.SerializeObject(mockResponse))
        }).Callback<HttpRequestMessage, CancellationToken>(async (r, c) =>
        {
            exchangeRateQueryString = r.RequestUri.PathAndQuery;
        });

        //Act
        var result = await sut.GetCryptoCurrencyMetadataAsync(symbol);
        //Assert
        exchangeRateQueryString.Should().Be(pathAndQuery);
        result.Should().NotBeNull().And.BeEquivalentTo(mockResponse);
    }

    private ISetup<HttpMessageHandler, Task<HttpResponseMessage>> GetSetup(string pathAndQuery)
    {
        var mockHttpMessageHandler = _httpMessageHandler;
        return mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(message => message.Method == HttpMethod.Get 
                    && message.RequestUri.PathAndQuery == pathAndQuery),
                ItExpr.IsAny<CancellationToken>());
    }

    private CoinMarketCapClient GetSut()
    {
        var mockHttpMessageHandler = _httpMessageHandler;
        var mockHttpClient = new HttpClient(mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri("https://mockBaseAddress.com")
        };

        return new CoinMarketCapClient(mockHttpClient);
    }

    private static CoinMarketCapResponseModel<CryptoCurrency> GetQuoteProperMockResponse(string symbol, string fiat)
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
    
    private static CoinMarketCapResponseModel<CryptoCurrency> GetQuoteErrorMockResponse()
    {
        return new CoinMarketCapResponseModel<CryptoCurrency>
        {
            Status = new Status
            {
                ErrorCode = 400,
                ErrorMessage = "Invalid request."
            }
        };
    }
    
    private static CoinMarketCapResponseModel<CryptoCurrencyMetadata> GetMetadataProperMockResponse(string symbol)
    {
        return new CoinMarketCapResponseModel<CryptoCurrencyMetadata>
        {
            Status = new Status
            {
                ErrorCode = 0,
                ErrorMessage = null
            },
            Data = new Dictionary<string, List<CryptoCurrencyMetadata>>
            {
                {
                    symbol, new List<CryptoCurrencyMetadata>
                    {
                        new()
                        {
                            Id = "1",
                            Symbol = symbol,
                            Name = "Name"
                        }
                    }
                }
            }
        };
    }
    
    private static CoinMarketCapResponseModel<CryptoCurrency> GetMetadataErrorMockResponse()
    {
        return new CoinMarketCapResponseModel<CryptoCurrency>
        {
            Status = new Status
            {
                ErrorCode = 400,
                ErrorMessage = "Invalid request."
            }
        };
    }
}