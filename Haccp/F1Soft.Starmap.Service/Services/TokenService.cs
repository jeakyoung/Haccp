namespace F1Soft.Starmap.Service.Services;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using F1Soft.Starmap.Service.Controllers.Users.Models;
using F1Soft.Starmap.Service.Controllers.Users.Enums;
using System.Security.Cryptography;

/// <summary>
/// JWT 토큰 서비스
/// </summary>
public class TokenService
{
    /// <summary>
    /// 유효 기간
    /// </summary>
    private const int ExpirationDay = 90;
    private readonly ILogger<TokenService> _logger;

    /// <summary>
    /// TokenService 생성자
    /// </summary>
    /// <param name="logger"></param>
    public TokenService(ILogger<TokenService> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// JWT 토큰 생성
    /// </summary>
    /// <param name="authResponse"></param>
    /// <returns></returns>
    public string CreateToken(AuthResponse authResponse)
    {
        var expiration = DateTime.UtcNow.AddDays(ExpirationDay);
        var token = CreateJwtToken(
            CreateClaims(authResponse),
            CreateSigningCredentials(),
            expiration
        );
        var tokenHandler = new JwtSecurityTokenHandler();

        _logger.LogInformation("JWT Token created");

        return tokenHandler.WriteToken(token);
    }

    private JwtSecurityToken CreateJwtToken(List<Claim> claims, SigningCredentials credentials,
        DateTime expiration) =>
        new(
            new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtTokenSettings")["ValidIssuer"],
            new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtTokenSettings")["ValidAudience"],
            claims,
            expires: expiration,
            signingCredentials: credentials
        );

    private List<Claim> CreateClaims(AuthResponse user)
    {
        var jwtSub = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtTokenSettings")["JwtRegisteredClaimNamesSub"];

        try
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, jwtSub!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.UserId!),
                new Claim(ClaimTypes.Name, user.UserId!),
                new Claim(ClaimTypes.Email, user.Email!)
                //new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            return claims;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    /// <summary>
    /// appsettings.json에서 토큰 키를 받아와 SHA256으로 암호화 키 생성
    /// </summary>
    /// <returns></returns>
    private SigningCredentials CreateSigningCredentials()
    {
        var symmetricSecurityKey = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtTokenSettings")["SymmetricSecurityKey"];

        return new SigningCredentials(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(symmetricSecurityKey!)
            ),
            SecurityAlgorithms.HmacSha256
        );
    }

    /// <summary>
    /// JWT 토큰 복호화
    /// </summary>
    /// <param name="token">JWT 토큰</param>
    /// <returns></returns>
    public ClaimsPrincipal DecodeToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtTokenSettings")["SymmetricSecurityKey"]!);

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtTokenSettings")["ValidIssuer"],
            ValidAudience = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtTokenSettings")["ValidAudience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };

        // 토큰 복호화 및 유효성 검사
        ClaimsPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

        return principal;

    }

    /// <summary>
    /// Claims를 AppUser로 변환
    /// </summary>
    /// <param name="principal"></param>
    /// <returns></returns>
    public AuthResponse GetAppUserFromClaimsPrincipal(ClaimsPrincipal principal)
    {
        var user = new AuthResponse
        {
            UserId = principal.FindFirstValue(ClaimTypes.Name)!,
        };
        return user;
    }

}
