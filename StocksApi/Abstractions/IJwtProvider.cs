using StocksApi.Persistence.Entities;

namespace StocksApi.Abstractions
{
    public interface IJwtProvider
    {
        string Generate(AppUser user);
    }
}