using System.Text.Json;
using System.Text.Json.Serialization;

namespace StocksApi.Serialization;

public class DateOnlyJsonConverter : JsonConverter<DateOnly>
{
    private const string key = "Json:Serializer:DateTimeFormat";
    private readonly IConfiguration _configuration;

    public DateOnlyJsonConverter(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return DateOnly.ParseExact(reader.GetString()!, _configuration[key]!);
    }

    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(_configuration[key]));
    }
}