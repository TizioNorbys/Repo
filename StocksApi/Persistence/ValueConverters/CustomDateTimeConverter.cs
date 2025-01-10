using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace StocksApi.Persistence.ValueConverters;

public class CustomDateTimeConverter : ValueConverter<DateTime, DateTime>
{
	public CustomDateTimeConverter() : base(
		value => new DateTime(
			value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second),
		dbValue => dbValue)
	{ }
}