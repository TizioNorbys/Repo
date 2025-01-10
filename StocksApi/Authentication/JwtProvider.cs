using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using StocksApi.Abstractions;
using StocksApi.Persistence.Entities;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace StocksApi.Authentication;

public class JwtProvider : IJwtProvider
{
    private readonly JwtOptions jwtOptions;

    public JwtProvider(IOptions<JwtOptions> options)
    {
        jwtOptions = options.Value;
    }

    public string Generate(AppUser user)
    {
        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Name,  user.Name!),
            new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName!)
        };

        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey
            (Encoding.UTF8.GetBytes(jwtOptions.SecretKey)), SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtOptions.Issuer,
            audience: jwtOptions.Audience,
            claims,
            null,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}