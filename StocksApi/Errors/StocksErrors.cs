using FluentResults;
using FluentValidation.Results;

namespace StocksApi.Errors;

public static class StocksErrors
{
    public static InvalidSymbolError InvalidSymbol(string symbol) => new(symbol);

    public static HistoricalSearchError HistoricalSearch(string symbol, DateOnly startDate, DateOnly endDate)
        => new(symbol, startDate, endDate);

    public static ValidationError Validation(IEnumerable<ValidationFailure> errors) => new(errors);
}

public class InvalidSymbolError : Error
{
    public InvalidSymbolError(string symbol)
        : base($"\"{symbol}\" is not a valid symbol") { }
}

public class HistoricalSearchError : Error
{
    public HistoricalSearchError(string symbol, DateOnly startDate, DateOnly endDate)
        : base($"There are no historical prices of \"{symbol}\" between {startDate} and {endDate}") { }
}

public class ValidationError : Error
{
    public ValidationError(IEnumerable<ValidationFailure> errors)
        : base("Validation error")
    {
        var metadata = errors.ToDictionary(e => e.PropertyName, e => (object)e.ErrorMessage);
        WithMetadata(metadata);
    }
}