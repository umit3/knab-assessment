using FluentValidation;
using Infrastructure.Abstractions;

namespace Infrastructure.Dtos;

public class RateRequestValidator : AbstractValidator<RateRequest>
{
    private readonly ICryptoExchangeClient _cryptoCurrencyService;

    public RateRequestValidator(ICryptoExchangeClient cryptoCurrencyService)
    {
        _cryptoCurrencyService = cryptoCurrencyService;
        
        RuleFor(v => v.Symbol)
            .NotEmpty()
            .WithMessage("Symbol cannot be empty")
            .Length(3, 12)
            .WithMessage("Symbol's character length can only be between [3-12]")
            .MustAsync(BeValidSymbol)
            .WithMessage("The symbol you've entered does not exist or invalid.");
    }

    private async Task<bool> BeValidSymbol(RateRequest rateRequest, string arg2, CancellationToken arg3)
    {
        return (await _cryptoCurrencyService.GetCryptoCurrencyMetadataAsync(rateRequest.Symbol)).Succeeded;
    }
}