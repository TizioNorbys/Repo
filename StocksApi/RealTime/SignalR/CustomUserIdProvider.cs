using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace StocksApi.RealTime.SignalR;

public class CustomUserIdProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
    {
        return connection.User?.FindFirst(c => c.Type == ClaimTypes.Email)?.Value ?? string.Empty;
    }
}