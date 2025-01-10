namespace StocksApi.IO.Requests
{
	public record RegistrationRequest(string Email, string? Name, string? LastName, string Password);
}