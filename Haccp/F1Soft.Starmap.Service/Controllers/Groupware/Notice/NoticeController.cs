using F1Soft.Starmap.Service.Controllers.Groupware.Approval;
using F1Soft.Starmap.Service.Controllers.Groupware.Approval.Models;
using F1Soft.Starmap.Service.Controllers.Groupware.Notice.Models;
using F1Soft.Starmap.Service.Services.EnvService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace F1Soft.Starmap.Service.Services.Groupware.Notice;

/// <summary>
/// 공지사항 컨트롤러
/// </summary>
/// 
[ApiController]
[Route("api/[controller]")]
#if !DEBUG
[Authorize]
#endif
[ProducesResponseType(200)]
[ProducesResponseType(401)]
public class NoticeController : Controller
{
    private readonly ILogger<ApprovalController> _logger;
    private readonly string _connectionString;
    private readonly IEnvService _envService;
    private readonly INoticeService _NoticeService;

    /// <summary>
    /// LOGGER
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="envService"></param>
    /// <param name="NoticeService"></param>
    public NoticeController(ILogger<ApprovalController> logger, IEnvService envService, INoticeService NoticeService)
    {
        _envService = envService;
        _connectionString = _envService.GetConnectionString();
        _logger = logger;
        _NoticeService = NoticeService;

    }

    /// <summary>
    /// 공지사항 리스트 조회
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// /// <remarks>
    /// Sample request:
    /// 
    ///     {
    ///       "SDate": "20101213",
    ///       "EDate": "20251210",
    ///       "EmployeeNo": "",
    ///       "EmployeeName": "",
    ///       "Title": "",
    ///       "StatusGbn": ""
    ///     }
    ///     
    /// </remarks>
    [HttpPost]
    [Route("GetNotice")]
    public async Task<IActionResult> GetNoticeList([FromBody] NoticeRequest request)
    {
        try
        {
            if (request == null)
            {
                return BadRequest(new { Message = "유효하지 않은 요청입니다." });
            }

            var result = await _NoticeService.GetNoticeList(request);

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