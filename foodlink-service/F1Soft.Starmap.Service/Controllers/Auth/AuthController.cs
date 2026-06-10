using F1Soft.Starmap.Service.Controllers.Auth.Models;
using F1Soft.Starmap.Service.Controllers.Users.Models;
using F1Soft.Starmap.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace F1Soft.Starmap.Service.Controllers.Auth;

/// <summary>
/// JWT 권한 인증
/// </summary>
[ApiController]
[Route("/api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly TokenService _tokenService;
    private readonly UserManager _userManager;

    /// <summary>
    /// 권한 인증 생성자
    /// </summary>
    /// <param name="tokenService"></param>
    /// <param name="logger"></param>
    /// <param name="userManager"></param>
    public AuthController(TokenService tokenService, ILogger<AuthController> logger, UserManager userManager)
    {
        _logger = logger;
        _tokenService = tokenService;
        _userManager = userManager;
    }

    /// <summary>
    /// 인증 - 사용자 로그인 (로그인 시 JWT 토큰 반환)
    /// </summary>
    /// <param name="request">유효한 ID와 Password를 입력하세요</param>
    /// <returns></returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     {
    ///         "userID": "19039",
    ///         "userPassword": "f1soft@6"
    ///     }
    ///
    /// </remarks>
    [HttpPost]
    [Route("Login")]
    public async Task<ActionResult<AuthResponse>> Authenticate([FromBody] AuthRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var managedUser = await _userManager.GetAppUserAsync(request);

        if (managedUser is null)
            return BadRequest("Bad credentials");

        //토큰 무조건 발행 (같은 날이면 그대로 사용??)
        var accessToken = _tokenService.CreateToken(managedUser);

        managedUser.Token = accessToken;

        return Ok(managedUser);
    }

    /// <summary>
    /// 자동 로그인
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Route("AutoLogin")]
    public async Task<ActionResult<AuthResponse>> AutoLogin()
    {
        string token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        AuthRequest authRequest = new AuthRequest();
        // 토큰이 유효한지 확인
        bool isValid = ValidateJwtToken(token);

        //토큰이 유효하지 않으면 401 반환
        if(!isValid)
            return Unauthorized(new { message = "Invalid token" });


        //jwt를 authResponse로 변환
        var claims = _tokenService.DecodeToken(token);
        var authResponse = _tokenService.GetAppUserFromClaimsPrincipal(claims);
        authRequest.UserID = authResponse.UserId;
        authRequest.UserPassword = "f1soft@6";

        return await Authenticate(authRequest);
    }


    /// <summary>
    /// 인증 테스트 API (JWT 인증 받은 사용자만 API 호출 가능)
    /// </summary>
    /// <returns>인증 성공 여부</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///
    /// </remarks>
    [HttpGet]
    [Route("TestAuthorize")]
    [Authorize]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    public async Task<ActionResult<AuthResponse>> TestAuthorize()
    {
        await Task.Delay(100);
        return Ok("authorize success");
    }

    /// <summary>
    /// 토큰이 유효한지 확인 (파라미터 사용)
    /// </summary>
    /// <param name="token">JWT 토큰</param>
    /// <returns>인증 성공 여부</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///
    /// </remarks>
    [HttpPost]
    [Route("ValidateStringToken")]
    public IActionResult ValidateStringToken(string token)
    {
        // 토큰이 유효한지 확인
        bool isValid = ValidateJwtToken(token);

        if (isValid)
        {
            // 토큰이 유효하면 토큰 정보 반환
            // (예: 토큰에 포함된 사용자 정보 등)
            return Ok(new { message = "Token is valid", token = token });
        }
        else
        {
            return Unauthorized(new { message = "Invalid token" });
        }
    }

    /// <summary>
    /// 토큰이 유효한지 확인 (헤더 사용)
    /// </summary>
    /// <returns></returns>
    /// <returns>인증 성공 여부</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///
    /// </remarks>
    [HttpPost]
    [Route("ValidateHeaderToken")]
    public IActionResult ValidateHeaderToken()
    {
        // Authorization 헤더에서 토큰 가져오기
        string token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        // 토큰이 유효한지 확인
        bool isValid = ValidateJwtToken(token);

        if (isValid)
        {
            // 토큰이 유효하면 토큰 정보 반환
            // (예: 토큰에 포함된 사용자 정보 등)
            return Ok(new { message = "Token is valid", token = token });
        }
        else
        {
            return Unauthorized(new { message = "Invalid token" });
        }
    }

    private bool ValidateJwtToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtTokenSettings")["SymmetricSecurityKey"]!);
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtTokenSettings")["ValidIssuer"],
                ValidAudience = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtTokenSettings")["ValidAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            }, out SecurityToken validatedToken);
        }
        catch
        {
            return false;
        }
        return true;
    }

}