using F1Soft.Starmap.Service.Controllers.Setting.Models;
using F1Soft.Starmap.Service.Services.EnvService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace F1Soft.Starmap.Service.Services.Groupware.Setting;

/// <summary>
/// 설정 서비스 컨트롤러
/// </summary>
/// 
[ApiController]
[Route("api/[controller]")]
#if !DEBUG
[Authorize]
#endif
[ProducesResponseType(200)]
[ProducesResponseType(401)]
public class SettingController : Controller
{
    private readonly ILogger<SettingController> _logger;
    private readonly string _connectionString;
    private readonly IEnvService _envService;
    private readonly ISettingService _SettingService;

    /// <summary>
    /// LOGGER
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="envService"></param>
    /// <param name="SettingService"></param>
    public SettingController(ILogger<SettingController> logger, IEnvService envService, ISettingService SettingService)
    {
        _envService = envService;
        _connectionString = _envService.GetConnectionString();
        _logger = logger;
        _SettingService = SettingService;

    }

    /// <summary>
    /// 세팅 리스트 조회
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// /// <remarks>
    /// Sample request:
    /// 
    ///     {
    ///       "ticker": "SYN",
    ///       "employeeNo": "0000"
    ///     }
    ///     
    /// </remarks>
    [HttpPost]
    [Route("GetSetting")]
    public async Task<IActionResult> GetSettingList([FromBody] SettingListRequest request)
    {
        try
        {
            if (request == null)
            {
                return BadRequest(new { Message = "유효하지 않은 요청입니다." });
            }

            var result = await _SettingService.GetSettingList(request);

            _logger.LogDebug("성공");
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "실패");
            return StatusCode(500, new { Message = "서버 오류 발생", Error = ex.Message });
        }
    }

    /// <summary>
    /// 세팅 정보 저장
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// /// <remarks>
    /// Sample request:
    /// 
    ///     {
    ///       "ticker": "SYN",
    ///       "employeeNo": "0000",
    ///       "approvalChkFlag": "1",
    ///       "boardChkFlag": "1",
    ///       "notiChkFlag": "1",
    ///       "scheduleChkFlag": "1"
    ///     }
    ///     
    /// </remarks>
    [HttpPost]
    [Route("SetSetting")]
    public async Task<IActionResult> SetSetting([FromBody] SetSettingRequest request)
    {
        try
        {
            if (request == null)
            {
                return BadRequest(new { Message = "유효하지 않은 요청입니다." });
            }

            var result = await _SettingService.SetSetting(request);

            _logger.LogDebug("성공");
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "실패");
            return StatusCode(500, new { Message = "서버 오류 발생", Error = ex.Message });
        }
    }

}