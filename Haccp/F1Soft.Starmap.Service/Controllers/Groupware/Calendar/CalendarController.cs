using F1Soft.Starmap.Service.Controllers.Groupware.Approval;
using F1Soft.Starmap.Service.Controllers.Groupware.Approval.Models;
using F1Soft.Starmap.Service.Controllers.Groupware.Calendar.Models;
using F1Soft.Starmap.Service.Services.EnvService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace F1Soft.Starmap.Service.Controllers.Groupware.Calendar;

/// <summary>
/// 캘린더 컨트롤러
/// </summary>
/// 
[ApiController]
[Route("api/[controller]")]
#if !DEBUG
[Authorize]
#endif
[ProducesResponseType(200)]
[ProducesResponseType(401)]
public class CalendarController : Controller
{
    private readonly ILogger<ApprovalController> _logger;
    private readonly string _connectionString;
    private readonly IEnvService _envService;
    private readonly ICalendarService _calendarService;

    /// <summary>
    /// 캘린더 컨트롤러 생성자
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="envService"></param>
    /// <param name="calendarService"></param>
    public CalendarController(ILogger<ApprovalController> logger, IEnvService envService, ICalendarService calendarService)
    {
        _envService = envService;
        _connectionString = _envService.GetConnectionString();
        _logger = logger;
        _calendarService = calendarService;

    }

    /// <summary>
    /// 캘린더 전체 리스트
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <remarks>
    /// Sample request:
    /// 
    ///     {
    ///      "sDate": "20250101",
    ///      "eDate": "20250206",
    ///      "employeeNo": "19039"
    ///     }
    ///     
    /// </remarks>
    [HttpPost]
    [Route("GetCalendarAllList")]
    public async Task<IActionResult> GetCalendarAllList([FromBody] CalendarAllListRequest request)
    {
        try
        {
            if (request == null)
            {
                return BadRequest(new { Message = "유효하지 않은 요청입니다." });
            }

            var result = await _calendarService.GetCalendarAllList(request);

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
