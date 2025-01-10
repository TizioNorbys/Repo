using FluentValidation;
using StocksApi.DTOs;

namespace StocksApi.Validators;

public class HistoricalDataDtoValidator : AbstractValidator<HistoricalDataDto>
{
    public HistoricalDataDtoValidator()
    {
        RuleFor(x => x.StartDate)
            .NotEmpty()
            .GreaterThanOrEqualTo(DateOnly.MinValue);

        RuleFor(x => x.EndDate)
            .NotEmpty()
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            .Must((x, endDate) => endDate > x.StartDate)
                .WithMessage("End date must be higher than start date");
    }
}